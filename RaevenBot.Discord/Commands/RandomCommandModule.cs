using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Serilog;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

[Group("random"), Aliases("rand", "rd", "r", "rdm")]
[Description("Random generator")]
public sealed class RandomCommandModule : BaseCommandModule
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string HeadCoinUrl = "https://flip-a-coin.com/img/coin/us/1cent_1.png";
    private const string TailCoinUrl = "https://flip-a-coin.com/img/coin/us/1cent_2.png";

    public ILogger Logger { private get; set; } = null!;

    [Command("number"), Aliases("n"), Description("Generates a random number between zero and the provided number")]
    public async Task RandomNumberCommand(CommandContext ctx, [Description("The positive maximum inclusive number")] int maxNumber)
    {
        if (maxNumber <= 0)
        {
            await ctx.RespondAsync("Please provide a number greater than zero.");
            return;
        }

        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Yellow)
            .WithTitle("Random number")
            .WithDescription($"You got **{Random.Shared.Next(maxNumber + 1)}**")
            .WithFooter($"Between {0} and {maxNumber}")
            .Build();

        await ctx.RespondAsync(embed);
    }

    [Command("letter"), Aliases("l"), Description("Generates a random letter")]
    public async Task RandomLetterCommand(CommandContext ctx)
    {
        await ctx.RespondAsync($"La lettre que vous avez obtenue est la lettre {Chars[Random.Shared.Next(Chars.Length)]} !");
    }

    [Command("elements"), Aliases("e"), Description("Pick a random element from the provided list.")]
    public async Task RandomElementCommand(CommandContext ctx, [Description("The list of elements to choose from")] params string[] args)
    {
        if (args.Length == 2)
        {
            await ctx.RespondAsync($"Please, provite at least two elements");
            return;
        }
        await ctx.RespondAsync($"You got **{args[Random.Shared.Next(args.Length)]}** !");
    }

    [Command("coinflip"), Aliases("cf"), Description("Generates a random coin flip")]
    public async Task CoinFlipCommand(CommandContext ctx)
    {
        var random = Random.Shared.Next(2);
        var embed = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Yellow)
            .WithTitle("Coin Flip")
            .WithDescription(random == 0 ? "Head" : "Tail")
            .WithImageUrl(random == 0 ? HeadCoinUrl : TailCoinUrl)
            .Build();

        await ctx.RespondAsync(embed);
    }
}