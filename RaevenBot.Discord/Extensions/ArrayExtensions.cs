namespace RaevenBot.Discord.Extensions;

public static class ArrayExtensions
{
    public static T RandomElement<T>(this T[] array)
    {
        return array[Random.Shared.Next(0, array.Length)];
    }
}
