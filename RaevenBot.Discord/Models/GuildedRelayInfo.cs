using DSharpPlus.Entities;
using LiteDB;

namespace RaevenBot.Discord.Models;

#pragma warning disable CS8618

public class GuildedRelayInfo
{
    public GuildedRelayInfo()
    {
    }

    public GuildedRelayInfo(DiscordChannel sourceChannel, string webhookUrl)
    {
        FromChannelId = sourceChannel.Id;
        ToWebhookUrl = webhookUrl;
    }

    public ulong FromChannelId { get; set; }
    public string ToWebhookUrl { get; set; }

    [BsonIgnore]
    private Uri? ToUri { get; set; }

    public Uri GetWebhookUri()
    {
        if (ToUri == null)
            ToUri = new Uri(ToWebhookUrl);
        return ToUri;
    }
}