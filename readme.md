# Sign up from ngrok

1. Get token from https://dashboard.ngrok.com/get-started/setup
2. `ngrok config add-authtoken <YOUR NGROK TOKEN>`
> Example: `ngrok config add-authtoken 2GDllE5bZJN26CC7JhJ23OJVYfZ_5VJhoc77Y9VWPJTZbKSYL`

***

# Run API

1. `dotnet run`
2. `ngrok http https://localhost:7146`

3. https://api.telegram.org/bot{BOT_TOKEN}/setWebhook?url=NGROK_URL/
>Example: https://api.telegram.org/bot5992354516:AAEdcw3Fi2ahUfMkom3IoLPEbdoSDusApTs/setWebhook?url=https://684a-84-54-80-149.ngrok-free.app/


***