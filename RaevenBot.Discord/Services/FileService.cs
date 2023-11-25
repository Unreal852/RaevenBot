using Microsoft.Extensions.Logging;
using RaevenBot.Discord.Contracts;
using RaevenBot.Discord.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RaevenBot.Discord.Services;

internal sealed class FileService : IFileService
{
    private const string DataFolderName = "data";
    private const string ConfigFileName = "config.json";

    private readonly ILogger<FileService> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
        DataFolder = new(Path.Combine(AppContext.BaseDirectory, DataFolderName));
        DataFolder.Create();
    }

    public DirectoryInfo DataFolder { get; }

    public bool ContainsDirectory(string directoryName)
    {
        return Directory.Exists(Path.Combine(DataFolder.FullName, directoryName));
    }

    public bool ContainsFile(string fileName)
    {
        return File.Exists(Path.Combine(DataFolder.FullName, fileName));
    }

    public bool TryGetConfiguration(out BotConfig? botConfig)
    {
        var configFilePath = Path.Combine(DataFolder.FullName, ConfigFileName);

        if (!File.Exists(configFilePath))
        {
            botConfig = new BotConfig();
            var serialized = JsonSerializer.Serialize(botConfig, _jsonSerializerOptions);
            File.WriteAllText(configFilePath, serialized);
            return true;
        }

        var fileContent = File.ReadAllText(configFilePath);
        try
        {
            botConfig = JsonSerializer.Deserialize<BotConfig>(fileContent, _jsonSerializerOptions);
            return botConfig != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read configuration file");
            botConfig = null;
            return false;
        }
    }
}
