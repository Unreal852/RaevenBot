using Jab;
using RaevenBot.Discord.Contracts;
using Serilog;

namespace RaevenBot.Discord.Services;

[ServiceProvider]
[Singleton<ILogger>(Instance = nameof(Logger))]
[Singleton<IFileService, FileService>]
[Singleton<IDiscordClient, DiscordClientService>]
[Singleton<IChannelRelayService, ChannelRelayService>]
[Singleton<IDatabaseService, DatabaseService>]
public sealed partial class ServiceProvider
{
    public static ILogger Logger => Log.Logger;
}