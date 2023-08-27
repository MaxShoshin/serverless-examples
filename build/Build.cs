using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Pulumi;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

using static Nuke.Common.Tools.Pulumi.PulumiTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    public readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    public Solution Solution;

    Project InfoProj => Solution.GetProject("InfoFunc");
    Project BuildProj => Solution.GetProject("_build");
    Project DeployProj => Solution.GetProject("PulumiDeploy");

    string Rid = "ubuntu.18.04-x64";
    AbsolutePath OutDir = RootDirectory / "out";
    AbsolutePath PublishDir => OutDir / "published";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean(s => s.SetProject(Solution));
            RootDirectory.GlobDirectories("*/bin", "*/obj").ForEach(x => x.DeleteDirectory());
            OutDir.DeleteDirectory();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                    .SetProjectFile(Solution));
            DotNetToolRestore(s => s);

        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            ProcessTasks.StartProcess("dotnet", "tt proc -f View.tt", InfoProj.Path.Parent);

            DotNetBuild(s => s
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration));
        });

    Target DeployCheck => _ => _
        .DependsOn(Compile)
        .Requires(() => Configuration == Configuration.Release)
        .Requires(() => CheckEnvVar("YC_TOKEN"))
        .Requires(() => CheckEnvVar("YC_CLOUD_ID"))
        .Requires(() => CheckEnvVar("YC_FOLDER_ID"))
        .Executes(() =>
        {
            OutDir.CreateOrCleanDirectory();
        });

    bool CheckEnvVar(string varName)
    {
        return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(varName));
    }

    Target Deploy => _ => _
        .DependsOn(DeployCheck)
        .Executes(() =>
        {
            var funcProjects = Solution.AllProjects.Except(new[] { DeployProj, BuildProj}).ToList();
            foreach (var funcProject in funcProjects)
            {
                Publish(funcProject);
            }

            DeployFunc(funcProjects);
        });

    void Publish(Project project)
    {
        DotNetPublish(s => s
                          .SetProject(project)
                          .SetConfiguration(Configuration)
                          .SetSelfContained(false)
                          .SetOutput(PublishDir / project.Name)
                          .SetRuntime(Rid));
    }

    void DeployFunc(IReadOnlyList<Project> projects)
    {
        PulumiUp(s =>
        {
            foreach (var proj in projects)
            {
                var archiveFile = OutDir / proj.Name + ".zip";
                (PublishDir /  proj.Name).ZipTo(archiveFile);

                var envName = $"UPLOAD_{proj.Name.ToUpperInvariant()}_PATH";
                s = s.SetProcessEnvironmentVariable(envName, archiveFile);
            }

            return s
                .SetCwd(DeployProj.Path.Parent)
                .SetStack("dev")
                .SetSkipPreview(true);
        });
    }
}
