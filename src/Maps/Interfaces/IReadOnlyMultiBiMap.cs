using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic read-only collection of key-values elements where
    /// keys can be retrieved by their associated values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IReadOnlyMultiBiMap<TKey, TVal> :
        IReadOnlyMultiMap<TKey, TVal>,
        IReadOnlyBiMap<TKey, IReadOnlyCollection<TVal>>
    {
        /// <summary>
        /// Gets the key associated with the specified value collection, using
        /// reference equality on the collection.
        /// </summary>
        /// <param name="values">The value collection of the key to get.</param>
        /// <param name="key">When this method returns, contains the key
        /// associated with the value collection, if the collection is found;
        /// otherwise, the default value for the type of the key parameter.
        /// This parameter is passed uninitialized.</param>
        /// <returns>true if <see cref="IReadOnlyMultiBiMap{TKey, TVal}"/>
        /// contains a key with the specified collection; otherwise false.
        /// </returns>
        bool TryGetKeyByCollectionRef(
            IEnumerable<TVal> values, out TKey key);

        /// <summary>
        /// Gets keys whose associated value collection satisfies set equality
        /// with the specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        IEnumerable<TKey> GetKeysWithEqualSet(
            IEnumerable<TVal> values,
            bool ignoreKeysWithNoValues = true);

        /// <summary>
        /// Gets keys whose associated value collection is a superset of the
        /// specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        IEnumerable<TKey> GetKeysWithSuperset(
            IEnumerable<TVal> values,
            bool ignoreKeysWithNoValues = true);

        /// <summary>
        /// Gets keys whose associated value collection is a subset of the
        /// specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        IEnumerable<TKey> GetKeysWithSubset(
            IEnumerable<TVal> values,
            bool ignoreKeysWithNoValues = true);

        /// <summary>
        /// Gets keys whose associated value collection has at least one value
        /// in common with the specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        IEnumerable<TKey> GetKeysWithAny(IEnumerable<TVal> values);
    }
}