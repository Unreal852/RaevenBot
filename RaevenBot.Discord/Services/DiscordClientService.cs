using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using RaevenBot.Discord.Contracts;
using Serilog;

namespace RaevenBot.Discord.Services;

public class DiscordClientService : IDiscordClient
{
    public DiscordClientService()
    {
    }

    public DiscordClient? Client { get; private set; }

    public void Initialize()
    {
        if (Client != null)
            return;
        var discordConfig = new DiscordConfiguration
        {
                Token = Environment.GetEnvironmentVariable("Token") ??
                        throw new Exception("Missing environment variable 'Token'"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                LoggerFactory = new LoggerFactory().AddSerilog()
        };

        Client = new DiscordClient(discordConfig);

        var commandsConfig = new CommandsNextConfiguration
        {
                StringPrefixes = new[] { "!" },
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