using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Serilog;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

[Group("developper"), Aliases("dev", "d")]
[Description("Various tools for developpers")]
public class DevCommandModule : BaseCommandModule
{
    public ILogger Logger { private get; set; } = null!;

    [Command("guid"), Aliases("uid"), Description("Generates a new Guid (Global Unique Id)")]
    public async Task GuidGeneratorCommand(CommandContext ctx)
    {
        await ctx.RespondAsync($"`{Guid.NewGuid()}`");
    }
}