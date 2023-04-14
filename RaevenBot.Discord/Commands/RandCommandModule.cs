using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Serilog;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

[Group("random"), Aliases("rand", "rd", "r", "rdm")]
[Description("Random draw generator")]
public class RandCommandModule : BaseCommandModule
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public ILogger Logger { private get; set; } = null!;

    [Command("randomNumber"), Aliases("randNumber", "number", "rn", "n"), Description("Generates a random number")]
    public async Task RandomNumberGenerator(CommandContext ctx, [Description("The positive maximum number.")] int maxNumber)
    {
        if (maxNumber <= 0)
        {
            await ctx.RespondAsync("Veuillez entrer un nombre positif !");
            return;
        }

        await ctx.RespondAsync("Vous avez tiré le chiffre " + Random.Shared.Next(maxNumber + 1));
    }

    [Command("randomCoinFlip"), Aliases("randCoinFlip", "coinflip", "rcf", "cf"), Description("Generates a random coin flip")]
    public async Task CoinFlipGenerator(CommandContext ctx)
    {
        if (Random.Shared.Next(2) == 0)
            await ctx.RespondAsync("Pile !");
        else
            await ctx.RespondAsync("Face !");
    }

    [Command("randomLetter"), Aliases("randLetter", "letter", "rl", "l"), Description("Generates a random letter")]
    public async Task RandomLetterGenerator(CommandContext ctx)
    {
        await ctx.RespondAsync($"La lettre que vous avez obtenue est la lettre {Chars[Random.Shared.Next(Chars.Length)]} !");
    }

    [Command("randomGame"), Aliases("randGame", "game", "rg", "g"), Description("Generates a random game")]
    public async Task RandomGameGenerator(CommandContext ctx, params string[] args)
    {
        if (args.Length == 2)
        {
            await ctx.RespondAsync($"Veuillez entrer au moins 2 valeurs");
            return;
        }
        await ctx.RespondAsync($"Et vous êtes tombé sur... {args[Random.Shared.Next(args.Length)]} !");
    }
}