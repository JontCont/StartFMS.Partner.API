using Discord;
using Discord.WebSocket;
using isRock.LineBot;
using StartFMS.Extensions.Line;

namespace StartFMS.Partner.API.Helper;

public class DiscordBot
{
    private DiscordSocketClient _client;
    public DiscordBot(DiscordSocketClient client)
    {
        _client = client;
    }

    public void Execute()
    {
        _client.MessageReceived += MessageReceived;
        _client.Ready += Ready;
        _client.Log += Log;
    }

    public async Task MessageReceived(SocketMessage message)
    {
        if (message.Author.IsBot) return;
        if (message.Content == "!ping")
        {
            await message.Channel.SendMessageAsync("pong");
        }
    }

    public async Task Ready()
    {
        Console.WriteLine("Discord bot is ready.");
        await Task.CompletedTask;
    }

    public async Task Log(LogMessage logMessage)
    {
        Console.WriteLine(logMessage.Message);
        await Task.CompletedTask;
    }
}
