using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of key-values elements where keys
    /// can be retrieved by their associated values and elements cannot be
    /// added or removed.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IFixedKeysMultiBiMap<TKey, TVal> :
        IFixedKeysMultiMap<TKey, TVal>,
        IFixedKeysBiMap<TKey, IReadOnlyCollection<TVal>>,
        IReadOnlyMultiBiMap<TKey, TVal>
    { }
}