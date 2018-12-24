using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of key-value pairs where keys can be
    /// retrieved by their associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IBiMap<TKey, TVal> :
        IDictionary<TKey, TVal>,
        IFixedKeysBiMap<TKey, TVal>
    {
        new int Count { get; }

        new IReadOnlyCollection<TKey> Keys { get; }

        new IReadOnlyCollection<TVal> Values { get; }

        new bool ContainsKey(TKey key);

        new TVal this[TKey key] { get; set; }

        new IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator();

        new bool TryGetValue(TKey key, out TVal values);
    }
}