using System.Collections.Generic;

namespace Anvoker.Maps.Interfaces
{
    /// <summary>
    /// Represents a generic collection of keys and values where keys cannot be
    /// removed or added, where each key may be associated with multiple
    /// values, and where keys can be retrieved by their associated values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IFixedKeysMultiBiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IFixedKeysMultiBiMap{TKey, TVal}"/>.</typeparam>
    public interface IFixedKeysMultiBiMap<TKey, TVal> :
        IFixedKeysMultiMap<TKey, TVal>,
        IFixedKeysBiMap<TKey, IReadOnlyCollection<TVal>>,
        IReadOnlyMultiBiMap<TKey, TVal>
    { }
}