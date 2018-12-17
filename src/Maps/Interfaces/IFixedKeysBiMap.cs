using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of key-value pairs where the values can
    /// be changed but keys cannot be added or removed.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IFixedKeysBiMap<TKey, TVal> :
        IReadOnlyBiMap<TKey, TVal>
    {
        /// <summary>
        /// Gets all keys that are associated with the specified value.
        /// </summary>
        /// <param name="val">The value to locate the keys by.</param>
        /// <returns>A read-only collection with all of the associated keys.
        /// </returns>
        IReadOnlyCollection<TKey> this[TVal val] { get; }

        /// <summary>
        /// Replaces the value currently associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to replace.</param>
        /// <param name="value">The new value.</param>
        void Replace(TKey key, TVal value);
    }
}