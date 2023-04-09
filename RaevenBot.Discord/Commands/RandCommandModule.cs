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

    [Command("randNumber"), Aliases("number", "rn", "n"), Description("Generates a random number")]
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

    [Command("coinFlip"), Aliases("cf"), Description("Generates a coin flip")]
    public async Task CoinFlipGenerator(CommandContext ctx)
    {
        if (RandomFlip() == 0)
            await ctx.RespondAsync("Pile !");
        else
            await ctx.RespondAsync("Face !");
    }
}