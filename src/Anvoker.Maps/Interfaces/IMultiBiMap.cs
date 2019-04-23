namespace Anvoker.Maps
{
    /// <summary>
    /// Represents a generic collection of keys and values where each key may be
    /// associated with multiple values, and where keys can be retrieved by
    /// their associated values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IMultiBiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IMultiBiMap{TKey, TVal}"/>.</typeparam>
    public interface IMultiBiMap<TKey, TVal> :
        IFixedKeysMultiBiMap<TKey, TVal>,
        IMultiMap<TKey, TVal>
    { }
}