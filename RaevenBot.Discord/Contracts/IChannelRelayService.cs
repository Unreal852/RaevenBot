﻿using DSharpPlus.Entities;
using RaevenBot.Discord.Models;

namespace RaevenBot.Discord.Contracts;

internal interface IChannelRelayService
{
    Task<OpResult> CreateRelay(DiscordChannel sourceChannel, DiscordChannel targetChannel);
    Task<OpResult> RemoveRelay(DiscordChannel sourceChannel, DiscordChannel targetChannel);
}