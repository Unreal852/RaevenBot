using DSharpPlus;
using DSharpPlus.Entities;

namespace RaevenBot.Discord.Contracts;

internal interface IDiscordClient
{
    event EventHandler ClientConnected;

    DiscordClient Client { get; }

    Task SetActivity(DiscordActivityType activityType, string activityName, DiscordUserStatus status);
    Task SetRandomActivity();
}