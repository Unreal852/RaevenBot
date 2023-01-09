using LiteDB;
using RaevenBot.Discord.Contracts;

namespace RaevenBot.Discord.Services;

public class DatabaseStorageService : IDatabaseStorage
{
    private const string DatabaseFileName = "database.db";

    public DatabaseStorageService()
    {
        var databasePath = Path.Combine(AppContext.BaseDirectory, "db");
        Directory.CreateDirectory(databasePath);
        Database = new LiteDatabase(Path.Combine(databasePath, DatabaseFileName));
    }

    private LiteDatabase Database { get; set; }

    public ILiteCollection<T> GetCollection<T>(string? collectionName = null)
    {
        return collectionName is null ? Database.GetCollection<T>() : Database.GetCollection<T>(collectionName);
    }
}