namespace RaevenBot.Discord.Extensions;

public static class ArrayExtensions
{
    /// <summary>
    /// Pick a random element from the specified array
    /// </summary>
    /// <param name="array">The array to pick an element from</param>
    /// <typeparam name="T">The array type</typeparam>
    /// <returns>A random element from the specified array</returns>
    public static T RandomElement<T>(this T[] array)
    {
        return array[Random.Shared.Next(0, array.Length)];
    }
}
