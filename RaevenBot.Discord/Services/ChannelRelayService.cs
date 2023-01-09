using System.Collections.Concurrent;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Models;
using SerilogTimings;

namespace RaevenBot.Discord.Services;

public class ChannelRelayService : IChannelRelayService
{
    private readonly IDiscordClient                                _discordClient;
    private readonly IDatabaseStorage                              _databaseStorage;
    private readonly ConcurrentDictionary<ulong, ChannelRelayInfo> _relays = new();

    public ChannelRelayService(IDiscordClient discordClient, IDatabaseStorage databaseStorage)
    {
        _discordClient = discordClient;
        _databaseStorage = databaseStorage;
        discordClient.Client.MessageCreated += OnMessageCreated;

        
        using (Operation.Time("Fetching relay channels from database"))
        {
            foreach (var channelRelay in _databaseStorage.GetCollection<ChannelRelayInfo>().FindAll())
            {
                _relays.TryAdd(channelRelay.FromChannelId, channelRelay);
            }
        }
    }

    public Task<bool> CreateRelay(ulong fromChannelId, ulong toChannelId)
    {
        return CreateRelay(new ChannelRelayInfo { FromChannelId = fromChannelId, ToChannelId = toChannelId });
    }

    public Task<bool> CreateRelay(ChannelRelayInfo relayInfo)
    {
        if (_relays.TryAdd(relayInfo.FromChannelId, relayInfo))
        {
            var dbCol = _databaseStorage.GetCollection<ChannelRelayInfo>();
            dbCol.Insert(relayInfo);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    private async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
    {
        if (_relays.TryGetValue(e.Channel.Id, out var relayInfo))
        {
            var channel = await relayInfo.GetTargetedChannel(_discordClient);
            var messageBuilder = new DiscordMessageBuilder(e.Message);
            await channel.SendMessageAsync(messageBuilder);
        }
    }
}