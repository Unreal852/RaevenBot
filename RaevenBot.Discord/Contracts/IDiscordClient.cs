using DSharpPlus;

namespace RaevenBot.Discord.Contracts;

public interface IDiscordClient
{
    DiscordClient Client { get; }

    void Initialize();
    Task InitializeAndConnectAsync();
    Task ConnectAsync();
    Task DisconnectAsync();
}