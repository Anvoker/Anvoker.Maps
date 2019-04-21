namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of keys and values where keys cannot be
    /// added or removed and where keys can be retrieved by their associated
    /// value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IFixedKeysBiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IFixedKeysBiMap{TKey, TVal}"/>.</typeparam>
    public interface IFixedKeysBiMap<TKey, TVal> :
        IReadOnlyBiMap<TKey, TVal>
    {
        /// <summary>
        /// Replaces the value currently associated with the specified key with
        /// a new value.
        /// </summary>
        /// <param name="key">The key of the value to replace.</param>
        /// <param name="value">The new value.</param>
        void Replace(TKey key, TVal value);
    }
}