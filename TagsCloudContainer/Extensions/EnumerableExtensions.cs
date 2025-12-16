using System.Numerics;

namespace TagsCloudContainer.Extensions;

public static class EnumerableExtensions
{
    public static void Increment<K, V>(this IDictionary<K, V> thiz, K key) where K : notnull where V : INumber<V>
    {
        if (thiz.TryGetValue(key, out var value))
        {
            thiz[key] = value + V.One;
        }
        else
        {
            thiz.Add(key, V.One);
        }
    }
}