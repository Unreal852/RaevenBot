using DSharpPlus.Entities;
using LiteDB;
using RaevenBot.Discord.Contracts;

namespace RaevenBot.Discord.Models;

public class ChannelRelayInfo
{
    public ulong FromChannelId { get; set; }
    public ulong ToChannelId   { get; set; }
    
    [BsonIgnore]
    private DiscordChannel? ToChannel { get; set; }

    public async Task<DiscordChannel> GetTargetedChannel(IDiscordClient discordClient)
    {
        if (ToChannel is not null)
            return ToChannel;
        ToChannel = await discordClient.Client.GetChannelAsync(ToChannelId);
        return ToChannel;
    }
}