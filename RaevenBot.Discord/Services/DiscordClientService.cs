using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using LiteDB;
using Microsoft.Extensions.Logging;
using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Models;
using Serilog;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RaevenBot.Discord.Services;

public class DiscordClientService : IDiscordClient
{
    public DiscordClientService()
    {
    }

    public DiscordClient? Client { get; private set; }

    private string GetToken()
    {
        var token = Environment.GetEnvironmentVariable("Token");
        if (token != null)
            return token;
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "bot_config.json"));
        var tokenFile = JsonSerializer.Deserialize<BotConfig>(json);
        return tokenFile?.Token ?? throw new Exception("Missing Token");
    }

    public void Initialize()
    {
        if (Client != null)
            return;
        var discordConfig = new DiscordConfiguration
        {
                Token = GetToken(),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                LoggerFactory = new LoggerFactory().AddSerilog()
        };

        Client = new DiscordClient(discordConfig);

        var commandsConfig = new CommandsNextConfiguration
        {
#if RELEASE
                StringPrefixes = new[] { "!" },
#elif DEBUG
                StringPrefixes = new[] { "?" },
#endif
                Services = Program.ServiceProvider
        };

        var commands = Client.UseCommandsNext(commandsConfig);
        commands.RegisterCommands(Assembly.GetExecutingAssembly());
    }

    public Task InitializeAndConnectAsync()
    {
        if (Client == null)
            Initialize();
        return ConnectAsync();
    }

    public Task ConnectAsync()
    {
        if (Client == null)
            throw new Exception("Please initialize the service before trying to connect.");
        return Client.ConnectAsync();
    }

    public Task DisconnectAsync()
    {
        if (Client == null)
            throw new Exception("Please initialize the service before trying to connect.");
        return Client.DisconnectAsync();
    }
}