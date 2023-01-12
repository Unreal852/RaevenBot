using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RaevenBot.Discord.Contracts;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

[Group("channel"), Aliases("chan", "ch")]
[Description("Manage channel relays.")]
[RequirePermissions(Permissions.ManageChannels)]
public class ChannelRelayCommandModule : BaseCommandModule
{
    public IChannelRelayService ChannelRelayService { private get; set; } = null!;

    [Command("subscribe"), Aliases("sub"), Description("Subscribe to a channel to relay the messages here.")]
    public async Task SubscribeCommand(CommandContext ctx, DiscordChannel channel)
    {
        var result = await ChannelRelayService.CreateRelay(channel, ctx.Channel);
        await ctx.RespondAsync(result.ToEmbed());
    }

    [Command("unsubscribe"), Aliases("unsub"), Description("Unsubscribe from a channel.")]
    public async Task UnsubscribeCommand(CommandContext ctx, DiscordChannel channel)
    {
        var result = await ChannelRelayService.RemoveRelay(channel, ctx.Channel);
        await ctx.RespondAsync(result.ToEmbed());
    }
}