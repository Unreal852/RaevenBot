using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using RaevenBot.Discord.Contracts;
using Serilog;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

[Group("developper"), Aliases("dev", "d")]
[Description("Various tools for developpers")]
internal sealed class DevCommandModule : BaseCommandModule
{
    public ILogger Logger { private get; set; } = null!;
    public IDiscordClient Client { private get; set; } = null!;

    [Command("guid"), Aliases("uid"), Description("Generates a new Guid (Global Unique Id)")]
    public async Task GuidGeneratorCommand(CommandContext ctx)
    {
        await ctx.RespondAsync($"`{Guid.NewGuid()}`");
    }

    [Command("status"), Aliases("s"), Description("Update the bot status")]
    public async Task BotStatusCommand(CommandContext ctx)
    {
        await Client.SetRandomActivity();
    }
}