using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of key-values elements where keys
    /// can be retrieved by their associated values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IMultiBiMap<TKey, TVal> :
        IFixedKeysMultiBiMap<TKey, TVal>,
        IMultiMap<TKey, TVal>
    { }
}