using DSharpPlus.Entities;
using System.Text.Json.Serialization;

namespace RaevenBot.Discord.Models;

public sealed class BotConfig : IJsonOnDeserialized
{
    public string Token { get; set; } = string.Empty;
    public string[] Prefixes { get; set; } = new[] { "!" };
    public BotStatus[] Statuses { get; set; } = new BotStatus[]
    {
        new() { ActivityType=ActivityType.Custom, Activity = "Idle", Status = UserStatus.Idle}
    };

    public void OnDeserialized()
    {

    }
}

public sealed class BotStatus
{
    public ActivityType ActivityType { get; set; } = ActivityType.Custom;
    public UserStatus Status { get; set; } = UserStatus.Online;
    public string Activity { get; set; } = string.Empty;
}