using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RaevenBot.Discord.Contracts;
using Serilog;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

[Group("random"), Aliases("rand", "rd", "r")]
[Description("Random draw generator")]
public class RandCommandModule : BaseCommandModule
{
    private int RandomInt(int max)
    {
        Random rand = new Random();
        int randNumber = rand.Next(1, max + 1);
        return randNumber;
    }

    private int RandomFlip()
    {
        Random rand = new Random();
        int randFlip = rand.Next(2);
        return randFlip;
    }

    private char RandomLetter()
    {
        Random rand = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var letterSelected = chars[rand.Next(chars.Length)];
        return letterSelected;
    }

    [Command("randomNumber"), Aliases("randNumber", "number", "rn", "n"), Description("Generates a random number")]
    public async Task RandomNumberGenerator(CommandContext ctx, int maxNumber)
    {
        try
        {
            string randNumber = RandomInt(maxNumber).ToString();
            await ctx.RespondAsync("Vous avez tiré le chiffre: " + randNumber);
        }
        catch (Exception e)
        {
            Log.Warning("Need to choice a number greater than 0");
            await ctx.RespondAsync("Vous devez choisir un nombre supérieur à 0 !");
            throw;
        }
    }

    [Command("randomCoinFlip"), Aliases("randCoinFlip", "coinflip", "rcf", "cf"), Description("Generates a random coin flip")]
    public async Task CoinFlipGenerator(CommandContext ctx)
    {
        if (RandomFlip() == 0)
            await ctx.RespondAsync("Pile !");
        else
            await ctx.RespondAsync("Face !");
    }
    
    [Command("randomLetter"), Aliases("randLetter", "letter", "rl", "l"), Description("Generates a random letter")]
    public async Task RandomLetterGenerator(CommandContext ctx)
    {
        await ctx.RespondAsync($"La lettre que vous avez obtenue est la lettre {RandomLetter()} !");
    }
    
    [Command("randomGame"), Aliases("randGame", "game", "rg", "g"), Description("Generates a random game")]
    public async Task RandomGameGenerator(CommandContext ctx, params string[] args)
    {
        try
        {
            Random rand = new Random();
            int randGame = rand.Next(args.Length);
            await ctx.RespondAsync($"Et vous êtes tombé sur... {args[randGame]} !");
        }
        catch (Exception e)
        {
            Log.Warning("You have to enter games to choose one at random !");
            await ctx.RespondAsync("Vous devez enter des jeux pour un choisir un au hasard !");
            throw;
        }
        
    }
}