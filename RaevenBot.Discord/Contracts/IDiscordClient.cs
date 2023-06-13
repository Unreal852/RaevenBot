using DSharpPlus;
using DSharpPlus.Entities;

namespace RaevenBot.Discord.Contracts;

internal interface IDiscordClient
{
    event EventHandler ClientConnected;

    DiscordClient Client { get; }

    Task SetActivity(ActivityType activityType, string activityName, UserStatus status);
    Task SetRandomActivity();
}