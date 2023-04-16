using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using RaevenBot.Discord.Contracts;
using Serilog;

namespace RaevenBot.Discord.Services;

#pragma warning disable CS8618

public sealed class DiscordClientService : IDiscordClient
{
    private readonly IFileService _fileService;
    private readonly ILogger _logger;

    public DiscordClientService(ILogger logger, IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
    }

    public DiscordClient Client { get; private set; }

    public void Initialize()
    {
        if (Client != null)
            return;

        if (!_fileService.TryGetConfiguration(out var botConfig))
        {
            _logger.Error("Failed to load the bot configuration");
        }

        if (string.IsNullOrWhiteSpace(botConfig!.Token))
        {
            _logger.Error("Missing token in the bot configuration");
        }

        var discordConfig = new DiscordConfiguration
        {
            Token = botConfig!.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            LoggerFactory = new Microsoft.Extensions.Logging.LoggerFactory().AddSerilog()
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