using System.Collections.ObjectModel;
using System.Drawing;
using DSharpPlus.Entities;
using Guilded.Base;
using Guilded.Base.Embeds;
using Serilog;
using GuildedEmbed = Guilded.Base.Embeds.Embed;

namespace RaevenBot.Discord.Extensions;

public static class DiscordMessageExtensions
{
    public static MessageContent ToGuildedMessage(this DiscordMessage discordMessage)
    {
        var guildedMessage = new MessageContent
        {
            Username = discordMessage.Author.Username,
            Avatar = new Uri(discordMessage.Author.AvatarUrl),
            Content = discordMessage.Content.EmbedUrls(),
            Embeds = new List<Embed>()
        };

        foreach (var embed in discordMessage.Embeds)
            guildedMessage.Embeds.Add(embed.ToGuildedEmbed());

        foreach (var attachment in discordMessage.Attachments)
        {
#if DEBUG
            Log.Information("Attachement Type: {MediaType}", attachment.MediaType);
#endif
            if (attachment.MediaType.Contains("image") || attachment.MediaType.Contains("video"))
                guildedMessage.Embeds.Add(new Embed
                {
                    Title = attachment.FileName,
                    Url = new Uri(attachment.Url),
                    Image = new EmbedMedia(attachment.Url)
                });
        }

        return guildedMessage;
    }

    public static GuildedEmbed ToGuildedEmbed(this DiscordEmbed discordEmbed)
    {
        var guildedEmbed = new GuildedEmbed
        {
            Title = discordEmbed.Title,
            Description = discordEmbed.Description,
            Color = discordEmbed.Color.HasValue ? Color.FromArgb(discordEmbed.Color.Value.Value) : default,
            Timestamp = discordEmbed.Timestamp?.DateTime ?? default,
            Url = discordEmbed.Url
        };

        if (discordEmbed.Author is { } author)
            guildedEmbed.Author = new EmbedAuthor(author.Name, author.Url, author.IconUrl.ToUri());
        if (discordEmbed.Image is { } image)
            guildedEmbed.Image = new EmbedMedia(image.Url.ToUri());
        if (discordEmbed.Video is { } video)
            guildedEmbed.Image = new EmbedMedia(video.Url); // TODO: Does this works actually ?
        if (discordEmbed.Thumbnail is { } thumbnail)
            guildedEmbed.Thumbnail = new EmbedMedia(thumbnail.Url.ToUri());
        if (discordEmbed.Footer is { } footer)
            guildedEmbed.Footer = new EmbedFooter(footer.Text, footer.IconUrl.ToUri());

        if (discordEmbed.Fields is { Count: > 0 } fields)
        {
            guildedEmbed.Fields = new List<EmbedField>(fields.Count);
            foreach (var field in fields)
                guildedEmbed.Fields.Add(new EmbedField(field.Name, field.Value, field.Inline));
        }

        return guildedEmbed;
    }

    public static MessageContent WrapIntoGuildedMessage(this DiscordEmbed discordEmbed)
    {
        return new MessageContent
        {
            Embeds = new Collection<Embed> { discordEmbed.ToGuildedEmbed() }
        };
    }
}