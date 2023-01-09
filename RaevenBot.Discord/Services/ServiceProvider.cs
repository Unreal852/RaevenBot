using Jab;
using RaevenBot.Discord.Contracts;
using Serilog;

namespace RaevenBot.Discord.Services;

[ServiceProvider]
[Singleton<ILogger>(Instance = nameof(Logger))]
[Singleton<IDiscordClient, DiscordClientService>]
[Singleton<IChannelRelayService, ChannelRelayService>]
[Singleton<IDatabaseStorage, DatabaseStorageService>]
public partial class ServiceProvider
{
    public static ILogger Logger => Log.Logger;
}