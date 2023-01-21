using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using RaevenBot.Discord.Contracts;

namespace RaevenBot.Discord.Commands;

[Group("guilded"), Aliases("g")]
[Description("Manage guilded relays.")]
[RequirePermissions(Permissions.ManageChannels)]
public class GuildedRelayCommandModule : BaseCommandModule
{
    public IGuildedRelayService GuildedRelayService { private get; set; } = null!;

    [Command("subscribe"), Aliases("sub"), Description("Subscribe this channel to a guilded webhook.")]
    public async Task SubscribeCommand(CommandContext ctx, string webhook)
    {
        if (!Uri.TryCreate(webhook, UriKind.RelativeOrAbsolute, out _))
            return;
        var result = await GuildedRelayService.CreateRelay(ctx.Channel, webhook);
        await ctx.RespondAsync(result.ToEmbed());
    }

    [Command("unsubscribe"), Aliases("unsub"), Description("Unsubscribe all webhooks from this channel.")]
    public async Task UnsubscribeCommand(CommandContext ctx)
    {
        var result = await GuildedRelayService.RemoveRelay(ctx.Channel);
        await ctx.RespondAsync(result.ToEmbed());
    }
}