using LiteDB;

namespace RaevenBot.Discord.Contracts;

internal interface IDatabaseService
{
    ILiteCollection<T> GetCollection<T>(string? collectionName = null);
}