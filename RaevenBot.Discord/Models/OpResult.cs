using DSharpPlus.Entities;

namespace RaevenBot.Discord.Models;

internal sealed class OpResult
{
    public static readonly OpResult Success = new() { IsSuccess = true };
    public static readonly OpResult Failed = new() { IsSuccess = false };

    public static OpResult NewSuccess(string? message) => new() { IsSuccess = true, Message = message };
    public static OpResult NewFailed(string? message) => new() { IsSuccess = false, Message = message };

    public bool IsSuccess { get; init; }
    public string? Message { get; init; }

    public DiscordEmbed ToEmbed(string? overrideMessage = null)
    {
        var embedBuilder = new DiscordEmbedBuilder()
            .WithTitle(IsSuccess ? "Success" : "Failed")
            .WithColor(IsSuccess ? DiscordColor.SpringGreen : DiscordColor.IndianRed)
            .WithDescription(overrideMessage ?? Message ?? string.Empty);
        return embedBuilder.Build();
    }
}