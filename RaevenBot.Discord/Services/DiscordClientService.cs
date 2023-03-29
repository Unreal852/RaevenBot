using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Models;
using Serilog;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RaevenBot.Discord.Services;

#pragma warning disable CS8618

public class DiscordClientService : IDiscordClient
{
    public DiscordClientService()
    {
    }

    public DiscordClient Client { get; private set; }

    private BotConfig? LoadBotConfig()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "config.json");
        var rawJson = File.ReadAllText(filePath);
        var config = JsonSerializer.Deserialize<BotConfig>(rawJson);
        return config;
    }

    public void Initialize()
    {
        if (Client != null)
            return;

        var botConfig = LoadBotConfig();
        if (botConfig == null)
        {
            Log.Error("Failed to load the bot configuration");
            return;
        }

        var discordConfig = new DiscordConfiguration
        {
                Token = botConfig.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                LoggerFactory = new LoggerFactory().AddSerilog()
        };

        Client = new DiscordClient(discordConfig);

        var commandsConfig = new CommandsNextConfiguration
        {
#if RELEASE
                StringPrefixes = botConfig.Prefixes,
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