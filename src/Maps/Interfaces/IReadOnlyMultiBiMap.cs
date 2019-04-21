using System;
using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a read-only generic collection of keys and values where each
    /// key may be associated with multiple values, and where keys can be
    /// retrieved by their associated values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IReadOnlyMultiBiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IReadOnlyMultiBiMap{TKey, TVal}"/>.</typeparam>
    public interface IReadOnlyMultiBiMap<TKey, TVal> :
        IReadOnlyMultiMap<TKey, TVal>,
        IReadOnlyBiMap<TKey, IReadOnlyCollection<TVal>>
    {
        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="IReadOnlyMultiBiMap{TKey, TVal}"/>.
        /// </summary>
        new IEqualityComparer<TKey> ComparerKey { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="IReadOnlyMultiBiMap{TKey, TVal}"/>.
        /// </summary>
        new IEqualityComparer<TVal> ComparerValue { get; }

        /// <summary>
        /// Gets all keys whose collection of associated values passes the
        /// conditions specified in the <paramref name="selector"/>.
        /// </summary>
        /// <param name="selector">The predicate that selects which keys
        /// to return based on their value collection.</param>
        /// <returns>An enumeration of all selected keys.</returns>
        IEnumerable<TKey> GetKeys(Func<IEnumerable<TVal>, bool> selector);

        /// <summary>
        /// Gets all keys that have any values in common with the specified
        /// collection.
        /// </summary>
        /// <param name="values">The values to test against for overlap.</param>
        /// <returns>An enumeration containing all keys whose value collection
        /// has any values in common with the specified collection.</returns>
        IEnumerable<TKey> GetKeysWithAny(IEnumerable<TVal> values);

        /// <summary>
        /// Gets all keys associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>An enumeration containing all keys associated with the
        /// specified value.</returns>
        IEnumerable<TKey> GetKeysWithValue(TVal value);
    }
}