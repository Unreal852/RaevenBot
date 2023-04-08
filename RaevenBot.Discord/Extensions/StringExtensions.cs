using System.Text.RegularExpressions;

namespace RaevenBot.Discord.Extensions;

public static partial class StringExtensions
{
    private static readonly Regex UrlParser = MyRegex();

    public static IEnumerable<string> GetUrls(this string str)
    {
        var matches = UrlParser.Matches(str);
        if (matches.Count > 0)
        {
            foreach (Match match in matches)
            {
                if (Uri.TryCreate(match.Value.Split(' ', 2)[0], UriKind.RelativeOrAbsolute, out Uri? _))
                    yield return match.Value.Trim();
            }
        }
    }

    public static string EmbedUrls(this string str)
    {
        foreach (var url in str.GetUrls())
            str = str.Replace(url, $"[{url}]({url})");
        return str;
    }

    [GeneratedRegex("http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}