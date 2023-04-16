using System.Collections.Concurrent;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Models;
using SerilogTimings;

namespace RaevenBot.Discord.Services;

public sealed class ChannelRelayService : IChannelRelayService
{
    // TODO: Support multi-cast

    private readonly IDiscordClient                                _discordClient;
    private readonly IDatabaseService                              _databaseStorage;
    private readonly ConcurrentDictionary<ulong, ChannelRelayInfo> _relays = new();

    public ChannelRelayService(IDiscordClient discordClient, IDatabaseService databaseStorage)
    {
        _discordClient = discordClient;
        _databaseStorage = databaseStorage;

        discordClient.Client.MessageCreated += OnMessageCreated;
        discordClient.Client.ChannelDeleted += OnChannelDeleted;

        using (Operation.Time("Fetching relay channels from database"))
        {
            foreach (var channelRelay in _databaseStorage.GetCollection<ChannelRelayInfo>().FindAll())
            {
                _relays.TryAdd(channelRelay.FromChannelId, channelRelay);
            }
        }
    }

    public Task<OpResult> CreateRelay(DiscordChannel sourceChannel, DiscordChannel targetChannel)
    {
        var channelRelayInfo = new ChannelRelayInfo(sourceChannel, targetChannel);

        if (_relays.TryAdd(sourceChannel.Id, channelRelayInfo))
        {
            var databaseCollection = _databaseStorage.GetCollection<ChannelRelayInfo>();
            var insertedDocument = databaseCollection.Insert(channelRelayInfo);
            if (insertedDocument == null)
            {
                _relays.TryRemove(sourceChannel.Id, out _);
                return Task.FromResult(OpResult.NewFailed("Failed to save the relay into the database."));
            }

            return Task.FromResult(OpResult.NewSuccess(
                    $"Messages from the channel '{sourceChannel.Name}' will be relayed into '{targetChannel.Name}'."));
        }

        return Task.FromResult(OpResult.NewFailed(
                $"The channel '{sourceChannel.Name}' is already registered. (Multi-Cast not yet supported)"));
    }

    public Task<OpResult> RemoveRelay(DiscordChannel sourceChannel, DiscordChannel targetChannel)
    {
        if (_relays.TryRemove(sourceChannel.Id, out _))
        {
            var dbCol = _databaseStorage.GetCollection<ChannelRelayInfo>();
            var deleted = dbCol.DeleteMany(info =>
                    info.FromChannelId == sourceChannel.Id && info.ToChannelId == targetChannel.Id);
            return Task.FromResult(deleted >= 1
                    ? OpResult.NewSuccess("The relay has been deleted.")
                    : OpResult.NewFailed("No relays were deleted."));
        }

        return Task.FromResult(OpResult.NewFailed("This channel has no subscribed channels"));
    }

    private async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
    {
        if (e.Author == sender.CurrentUser)
            return;
        if (_relays.TryGetValue(e.Channel.Id, out var relayInfo))
        {
            var channel = await relayInfo.GetTargetedChannel(_discordClient);
            var messageBuilder = new DiscordMessageBuilder(e.Message);
            foreach (var messageAttachment in e.Message.Attachments)
            {
                if (!messageAttachment.MediaType.StartsWith("image"))
                    continue;
                var builder = new DiscordEmbedBuilder
                {
                        ImageUrl = messageAttachment.Url
                };
                messageBuilder.AddEmbed(builder.Build());
            }

            await channel.SendMessageAsync(messageBuilder);
        }
    }

    private Task OnChannelDeleted(DiscordClient sender, ChannelDeleteEventArgs e)
    {
        // TODO: Automatically remove relay when registered channel is deleted
        return Task.CompletedTask;
    }
}