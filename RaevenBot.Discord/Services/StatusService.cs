using Microsoft.Extensions.Hosting;
using RaevenBot.Discord.Contracts;

namespace RaevenBot.Discord.Services;

internal sealed class StatusService : BackgroundService, IStatusService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(5));
    private readonly IDiscordClient _discordClient;

    public StatusService(IDiscordClient discordClient)
    {
        _discordClient = discordClient;
        discordClient.ClientConnected += OnDiscordClientReady;
    }

    private bool IsConnected { get; set; }

    private void OnDiscordClientReady(object? sender, EventArgs e)
    {
        IsConnected = true;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(stoppingToken))
        {
            if (!IsConnected)
                continue;
            await _discordClient.SetRandomActivity();
        }
    }
}