using Messaging.Web;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MessagingServer>();
builder.Services.AddSingleton<MessageHandler>();
builder.Services.AddCors();

var app = builder.Build();

var handler = app.Services.GetRequiredService<MessageHandler>();
handler.Initialize();

app.UseCors(x =>
{
    x.AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true);
});

app.UseWebSockets();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        MessagingServer messagingServer = context.RequestServices.GetRequiredService<MessagingServer>();

        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        var ws = await context.WebSockets.AcceptWebSocketAsync();
        await messagingServer.ReceiveConnection(ws);
    }
    else
    {
        await next();
    }
});

app.Run();
