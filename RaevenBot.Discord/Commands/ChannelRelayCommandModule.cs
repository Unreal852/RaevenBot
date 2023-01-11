using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RaevenBot.Discord.Contracts;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

public enum ChannelRelaySubCommand
{
    Sub,
    Unsub,
    Unknown,
}

public class ChannelRelayCommandModule : BaseCommandModule
{
    public IChannelRelayService ChannelRelayService { private get; set; } = null!;

    [Command("channel")]
    public async Task HandleCommand(CommandContext ctx, string subCommand, DiscordChannel channel)
    {
        var cmd = ParseSubCommand(subCommand);
        if (cmd == ChannelRelaySubCommand.Unknown)
        {
            await ctx.RespondAsync("Unknown command.");
            return;
        }

        if (cmd == ChannelRelaySubCommand.Sub)
        {
            if (await ChannelRelayService.CreateRelay(channel.Id, ctx.Channel.Id))
                await ctx.RespondAsync($"The messages from the channel '{channel.Name}' will be forwarded here");
            else
                await ctx.RespondAsync($"Failed to subscribe to the '{channel.Name}' channel");
        }
        else if (cmd == ChannelRelaySubCommand.Unsub)
        {
            if (await ChannelRelayService.RemoveRelay(channel.Id, ctx.Channel.Id))
                await ctx.RespondAsync($"Successfully unsubscribed from the '{channel.Name}' channel");
            else
                await ctx.RespondAsync($"Failed to unsubscribed from the '{channel.Name}' channel");
        }
    }

    private ChannelRelaySubCommand ParseSubCommand(string subCommand)
    {
        return subCommand.ToLower() switch
        {
                "sub"   => ChannelRelaySubCommand.Sub,
                "unsub" => ChannelRelaySubCommand.Unsub,
                _       => ChannelRelaySubCommand.Unknown
        };
    }
}