using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic read-only collection of key-value pairs where keys
    /// can be retrieved by their associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IReadOnlyBiMap<TKey, TVal> :
        IReadOnlyDictionary<TKey, TVal>
    {
        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="IReadOnlyBiMap{TKey, TVal}"/>.
        /// </summary>
        IEqualityComparer<TKey> ComparerKey { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="IReadOnlyBiMap{TKey, TVal}"/>.
        /// </summary>
        IEqualityComparer<TVal> ComparerValue { get; }

        /// <summary>
        /// Gets the number of unique values in the
        /// <see cref="IReadOnlyBiMap{TKey, TVal}"/>.
        /// </summary>
        int UniqueValueCount { get; }

        /// <summary>
        /// Determines whether the <see cref="IReadOnlyBiMap{TKey, TVal}"/>
        /// contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the
        /// <see cref="IReadOnlyBiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="IReadOnlyBiMap{TKey, TVal}"/>
        /// contains an element with the specified value; otherwise,
        /// false.</returns>
        bool ContainsValue(TVal value);

        /// <summary>
        /// Gets all keys that are associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate the keys by.</param>
        /// <returns>A read-only collection with all of the associated keys.
        /// </returns>
        IReadOnlyCollection<TKey> GetKeysWithValue(TVal value);
    }
}