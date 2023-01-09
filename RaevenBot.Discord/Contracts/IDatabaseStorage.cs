using LiteDB;

namespace RaevenBot.Discord.Contracts;

public interface IDatabaseStorage
{
    ILiteCollection<T> GetCollection<T>(string? collectionName = null);
}