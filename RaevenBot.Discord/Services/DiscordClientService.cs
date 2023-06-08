using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Extensions;
using RaevenBot.Discord.Models;
using Serilog;

namespace RaevenBot.Discord.Services;

public sealed class DiscordClientService : IHostedService, IDiscordClient
{
    private readonly ILogger<DiscordClientService> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IFileService _fileService;
    private BotConfig _botConfig = null!;

    public DiscordClientService(ILogger<DiscordClientService> logger, IHostApplicationLifetime applicationLifetime, IFileService fileService)
    {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _fileService = fileService;

        if (!_fileService.TryGetConfiguration(out _botConfig!))
        {
            _logger.LogError("Failed to load the bot configuration");
        }

        if (string.IsNullOrWhiteSpace(_botConfig.Token))
        {
            _logger.LogError("Missing token in the bot configuration");
        }

        var discordConfig = new DiscordConfiguration
        {
            Token = _botConfig.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            LoggerFactory = new LoggerFactory().AddSerilog()
        };

        Client = new(discordConfig);
        logger.LogInformation("Instantiated");
    }

    public event EventHandler? ClientConnected;

    public DiscordClient Client { get; private set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Client.GuildDownloadCompleted += OnGuildDownloadCompleted;

        var commandsConfig = new CommandsNextConfiguration
        {
#if RELEASE
            StringPrefixes = _botConfig.Prefixes,
#elif DEBUG
            StringPrefixes = new[] { "?" },
#endif
            Services = Program.ServiceProvider
        };

        var commands = Client.UseCommandsNext(commandsConfig);
        commands.RegisterCommands(Assembly.GetExecutingAssembly());

        _logger.LogInformation("Connecting to Discord...");
        return Client.ConnectAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Disconnecting from Discord...");
        return Client.DisconnectAsync();
    }

    public Task SetActivity(ActivityType activityType, string activityName, UserStatus status)
    {
        var activity = new DiscordActivity(activityName, activityType);
        return Client.UpdateStatusAsync(activity, status);
    }

    public Task SetRandomActivity()
    {
        var status = _botConfig.Statuses.RandomElement();
        return SetActivity(status.ActivityType, status.Activity, status.Status);
    }

    private Task OnGuildDownloadCompleted(DiscordClient sender, DSharpPlus.EventArgs.GuildDownloadCompletedEventArgs args)
    {
        _logger.LogInformation("Connected !");
        ClientConnected?.Invoke(this, null!);
        if (_botConfig.Statuses.Length > 0)
            return SetRandomActivity();
        return Task.CompletedTask;
    }
}