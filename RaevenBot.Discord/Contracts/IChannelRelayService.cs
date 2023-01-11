using RaevenBot.Discord.Models;

namespace RaevenBot.Discord.Contracts;

public interface IChannelRelayService
{
    Task<bool> CreateRelay(ChannelRelayInfo relayInfo);
    Task<bool> CreateRelay(ulong fromChannelId, ulong toChannelId);
    Task<bool> RemoveRelay(ulong fromChannelId, ulong toChannelId);
}