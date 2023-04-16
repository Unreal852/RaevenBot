using LiteDB;
using RaevenBot.Discord.Contracts;

namespace RaevenBot.Discord.Services;

public sealed class DatabaseService : IDatabaseService
{
    private const string DatabaseFileName = "database.db";

    private readonly IFileService _fileService;

    public DatabaseService(IFileService fileService)
    {
        _fileService = fileService;
        Database = new LiteDatabase(Path.Combine(_fileService.DataFolder.FullName, DatabaseFileName));
    }

    private LiteDatabase Database { get; set; }

    public ILiteCollection<T> GetCollection<T>(string? collectionName = null)
    {
        return collectionName is null ? Database.GetCollection<T>() : Database.GetCollection<T>(collectionName);
    }
}