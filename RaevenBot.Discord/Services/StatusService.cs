using Microsoft.Extensions.Hosting;
using RaevenBot.Discord.Contracts;

namespace RaevenBot.Discord.Services;

internal class StatusService : IStatusService, IHostedService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(5));

    public StatusService(IDiscordClient discordClient)
    {
        discordClient.ClientConnected += OnDiscordClientReady;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnDiscordClientReady(object? sender, EventArgs e)
    {

    }
}
