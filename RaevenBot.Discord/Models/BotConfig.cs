using DSharpPlus.Entities;
using System.Text.Json.Serialization;

namespace RaevenBot.Discord.Models;

internal sealed class BotConfig : IJsonOnDeserialized
{
    public string Token { get; set; } = string.Empty;
    public string[] Prefixes { get; set; } = { "!" };

    public BotStatus[] Statuses { get; set; } =
    {
        new() { ActivityType = DiscordActivityType.Custom, Activity = "Idle", Status = DiscordUserStatus.Idle }
    };

    public void OnDeserialized()
    {
    }
}

internal sealed class BotStatus
{
    public DiscordActivityType ActivityType { get; set; } = DiscordActivityType.Custom;
    public DiscordUserStatus Status { get; set; } = DiscordUserStatus.Online;
    public string Activity { get; set; } = string.Empty;
}