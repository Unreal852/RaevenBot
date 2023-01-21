using DSharpPlus.Entities;
using RaevenBot.Discord.Models;

namespace RaevenBot.Discord.Contracts;

public interface IGuildedRelayService
{
    Task<OpResult> CreateRelay(DiscordChannel sourceChannel, string webhookUrl);
    Task<OpResult> RemoveRelay(DiscordChannel sourceChannel);
}