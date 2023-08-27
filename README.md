# serverless-examples

Проекты с примерями Cloud Functions на C#.

Сборка проекта осуществляется Nuke (см. папку build).

Развертывание сделано с помощью Pulumi (см. PulumiDeploy)

Чтобы развернуть проект нужно запустить команду `./build.sh  Deploy --configuration Release` с выставленными переменными окружения:
- YC_CLOUD_ID - идентификатор облака для установки
- YC_FOLDER_ID - идентификатор папки для установки
- YC_TOKEN - IAM Token, можно получить командой `yc iam create-token`

- BOT_TOKEN токен бота, получается при регистрации бота в телеграмме
- API_KEY - сгенерировать секретную строку, используется при заведении WebHook для TelegramBot (параметр secret_token). См. https://core.telegram.org/bots/api#setwebhook

