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
    { }
}