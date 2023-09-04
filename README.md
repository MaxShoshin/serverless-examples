# serverless-examples

Проекты с примерями Cloud Functions на C#.

## Описание примеров
- `Handlers`, `HandlersSdk` - примеры возможных сигнатур функций
- `InfoFunc` - функция, которая возвращает описание окружения в котором она запущена. Удобна для изучения Cloud Functions.
- `PulumiDeploy` - пример развертывания с помощью [Pulumi](https://www.pulumi.com/), можно развертывать с помошью [Terraform](https://github.com/yandex-cloud/terraform-provider-yandex)
- `QRCoderFunc` - пример Telegram чат-бота, который отвечает QR кодом, в котором закодировано сообщение которое ему прислали.
- `QueueReceiver` - пример функции, которая запускается при появлении сообщения в очереди
- `QueueSender` - пример функции, которая отправляет сообщение в очередь
- `Runner` - простой пример как можно запускать и дебажить локально функции - просто консольное приложение которое напрямую вызывает нужный FunctionHandler.


## Сборка и развертывание
Сборка проекта осуществляется с помощью Nuke (см. папку build).

Развертывание сделано с помощью Pulumi (см. PulumiDeploy)

Чтобы развернуть проект нужно запустить команду `./build.sh  Deploy --configuration Release` **с выставленными переменными окружения**:
- YC_CLOUD_ID - идентификатор облака для установки
- YC_FOLDER_ID - идентификатор папки для установки
- YC_TOKEN - IAM Token, можно получить командой `yc iam create-token`

- BOT_TOKEN токен бота, получается при регистрации бота в телеграмме
- API_KEY - сгенерировать секретную строку, используется при заведении WebHook для TelegramBot (параметр secret_token). См. https://core.telegram.org/bots/api#setwebhook

