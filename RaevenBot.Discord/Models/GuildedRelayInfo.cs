using DSharpPlus.Entities;
using LiteDB;
using RaevenBot.Discord.Contracts;

namespace RaevenBot.Discord.Models;

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

    public ulong  FromChannelId { get; set; }
    public string ToWebhookUrl  { get; set; }

    [BsonIgnore]
    private Uri? ToUri { get; set; }

    public Uri GetWebhookUri()
    {
        if (ToUri == null)
            ToUri = new Uri(ToWebhookUrl);
        return ToUri;
    }
}