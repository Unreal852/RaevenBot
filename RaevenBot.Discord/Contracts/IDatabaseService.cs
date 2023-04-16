using LiteDB;

namespace RaevenBot.Discord.Contracts;

public interface IDatabaseService
{
    ILiteCollection<T> GetCollection<T>(string? collectionName = null);
}