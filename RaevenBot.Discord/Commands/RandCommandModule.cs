using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RaevenBot.Discord.Contracts;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace RaevenBot.Discord.Commands;

[Group("random"), Aliases("rand", "rd", "r")]
[Description("Random draw generator")]
public class RandCommandModule : BaseCommandModule
{
    private int RandomInt(int max)
    {
        Random rand = new Random();
        int randNumber = rand.Next(1, max+1);
        return randNumber;
    }
    
    [Command("randNumber"), Aliases("rn"), Description("Generates a random number")]
    public async Task RandomNumberGenerator(CommandContext ctx, int maxNumber)
    {
        string randNumber = RandomInt(maxNumber).ToString();
        await ctx.RespondAsync("Le chiffre tiré est: " + randNumber);
    }
    
}