using System.Collections.Concurrent;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Guilded.Webhook;
using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Extensions;
using RaevenBot.Discord.Models;
using SerilogTimings;

namespace RaevenBot.Discord.Services;

public class GuildedRelayService : IGuildedRelayService
{
    private readonly IDiscordClient                                _discordClient;
    private readonly IDatabaseStorage                              _databaseStorage;
    private readonly GuildedWebhookClient                          _guildedWebhookClient = new(Array.Empty<string>());
    private readonly ConcurrentDictionary<ulong, GuildedRelayInfo> _relays               = new();

    public GuildedRelayService(IDiscordClient discordClient, IDatabaseStorage databaseStorage)
    {
        _discordClient = discordClient;
        _databaseStorage = databaseStorage;

        discordClient.Client.MessageCreated += OnMessageCreated;
        discordClient.Client.ChannelDeleted += OnChannelDeleted;

        using (Operation.Time("Fetching guilded relay channels from database"))
        {
            foreach (var guildedRelay in _databaseStorage.GetCollection<GuildedRelayInfo>().FindAll())
            {
                _relays.TryAdd(guildedRelay.FromChannelId, guildedRelay);
            }
        }
    }

    public Task<OpResult> CreateRelay(DiscordChannel sourceChannel, string webhookUrl)
    {
        var guildedRelayInfo = new GuildedRelayInfo(sourceChannel, webhookUrl);

        if (_relays.TryAdd(sourceChannel.Id, guildedRelayInfo))
        {
            var databaseCollection = _databaseStorage.GetCollection<GuildedRelayInfo>();
            var insertedDocument = databaseCollection.Insert(guildedRelayInfo);
            if (insertedDocument == null)
            {
                _relays.TryRemove(sourceChannel.Id, out _);
                return Task.FromResult(OpResult.NewFailed("Failed to save the relay into the database."));
            }

            return Task.FromResult(OpResult.NewSuccess(
                    $"Messages from the channel '{sourceChannel.Name}' will be relayed to the guilded webhook '{webhookUrl}'."));
        }

        return Task.FromResult(OpResult.NewFailed(
                $"The channel '{sourceChannel.Name}' is already registered. (Multi-Cast not yet supported)"));
    }

    public Task<OpResult> RemoveRelay(DiscordChannel sourceChannel)
    {
        if (_relays.TryRemove(sourceChannel.Id, out _))
        {
            var dbCol = _databaseStorage.GetCollection<GuildedRelayInfo>();
            var deleted = dbCol.DeleteMany(info => info.FromChannelId == sourceChannel.Id);
            return Task.FromResult(deleted >= 1
                    ? OpResult.NewSuccess("The relay has been deleted.")
                    : OpResult.NewFailed("No relays were deleted."));
        }

        return Task.FromResult(OpResult.NewFailed("This channel has no targeted webhooks"));
    }

    private async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
    {
        if (e.Author == sender.CurrentUser)
            return;
        if (_relays.TryGetValue(e.Channel.Id, out var relayInfo))
        {
            await _guildedWebhookClient.CreateMessageAsync(relayInfo.GetWebhookUri(), e.Message.ToGuildedMessage());
        }
    }

    private async Task OnChannelDeleted(DiscordClient sender, ChannelDeleteEventArgs e)
    {
        await RemoveRelay(e.Channel);
    }
}