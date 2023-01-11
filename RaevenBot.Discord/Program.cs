using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Services;
using Serilog;

namespace RaevenBot.Discord;

internal static class Program
{
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    private static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    private static async Task MainAsync(string[] args)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        ServiceProvider = new ServiceProvider();
        var discord = ServiceProvider.GetService<IDiscordClient>();

        await discord.InitializeAndConnectAsync();
        
        while (Console.ReadLine() != "exit")
        {
        }

        await discord.DisconnectAsync();
    }
}