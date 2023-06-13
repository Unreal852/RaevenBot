using RaevenBot.Discord.Models;

namespace RaevenBot.Discord.Contracts;

internal interface IFileService
{
    DirectoryInfo DataFolder { get; }
    bool ContainsFile(string fileName);
    bool ContainsDirectory(string directoryName);
    bool TryGetConfiguration(out BotConfig? botConfig);
}
