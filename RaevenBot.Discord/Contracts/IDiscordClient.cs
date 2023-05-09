using DSharpPlus;
using DSharpPlus.Entities;

namespace RaevenBot.Discord.Contracts;

public interface IDiscordClient
{
    DiscordClient Client { get; }

    void Initialize();
    Task InitializeAndConnectAsync();
    Task ConnectAsync();
    Task DisconnectAsync();
    Task SetActivity(ActivityType activityType, string activityName, UserStatus status);
    Task SetRandomActivity();
}