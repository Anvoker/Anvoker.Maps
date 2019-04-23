using System.Collections.Generic;

namespace Anvoker.Maps
{
    /// <summary>
    /// Represents a generic read-only collection of keys and values where keys
    /// can be retrieved by their associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IReadOnlyBiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IReadOnlyBiMap{TKey, TVal}"/>.</typeparam>
    public interface IReadOnlyBiMap<TKey, TVal> :
        IReadOnlyDictionary<TKey, TVal>
    {
        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the map.
        /// </summary>
        IEqualityComparer<TKey> ComparerKey { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the map.
        /// </summary>
        IEqualityComparer<TVal> ComparerValue { get; }

        /// <summary>
        /// Determines whether the <see cref="IReadOnlyBiMap{TKey, TVal}"/>
        /// contains an element that has the specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>true if the <see cref="IReadOnlyBiMap{TKey, TVal}"/>
        /// contains an element that has the specified value; otherwise,
        /// false.</returns>
        bool ContainsValue(TVal value);

        /// <summary>
        /// Gets all keys associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>A read-only collection containing all keys associated
        /// with the specified value.</returns>
        IEnumerable<TKey> GetKeysWithValue(TVal value);
    }
}