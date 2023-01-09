using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RaevenBot.Discord.Contracts;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

public class CommandModule : BaseCommandModule
{
    public IChannelRelayService ChannelRelayService { private get; set; } = null!;

    [Command("ping")]
    public async Task GreetCommand(CommandContext ctx)
    {
        await ctx.RespondAsync("pong");
    }

    [Command("sub")]
    public async Task SubscribeToChannel(CommandContext ctx, DiscordChannel channel)
    {
        var channelId = ctx.Channel.Id;
        if (await ChannelRelayService.CreateRelay(channel.Id, ctx.Channel.Id))
        {
            await ctx.RespondAsync($"The messages from the channel '{channel.Name}' will be forwarded here");
        }
        else 
            await ctx.RespondAsync($"Failed to subscribe to the '{channel.Name}' channel");
    }
}