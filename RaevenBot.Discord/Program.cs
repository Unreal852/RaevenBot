using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Services;
using Serilog;
using Serilog.Events;

namespace RaevenBot.Discord;

internal static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var host = Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .UseSerilog()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IFileService, FileService>();
                services.AddSingleton<IDatabaseService, DatabaseService>();
                services.AddSingleton<IChannelRelayService, ChannelRelayService>();

                services.AddSingleton<IDiscordClient, DiscordClientService>();
                services.AddHostedService(p => (DiscordClientService)p.GetRequiredService<IDiscordClient>());

                //services.AddHostedService<StatusService>();
            }).Build();

        ServiceProvider = host.Services;

        await host.RunAsync();
    }
}