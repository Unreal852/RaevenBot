using System.Text.Json.Serialization;

namespace RaevenBot.Discord.Models;

public class BotConfig : IJsonOnDeserialized
{
    public string Token { get; set; } = string.Empty;
    public string[] Prefixes { get; set; } = new[] { "!" };

    public void OnDeserialized()
    {
        
    }
}