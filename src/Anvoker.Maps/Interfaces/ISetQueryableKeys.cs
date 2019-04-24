using System;
using System.Collections.Generic;

namespace Anvoker.Maps.Interfaces
{
    /// <summary>
    /// Represents a collection where keys can be retrieved by querying the set
    /// of their associated values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="ISetQueryableKeys{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="ISetQueryableKeys{TKey, TVal}"/>.</typeparam>
    public interface ISetQueryableKeys<TKey, TVal>
    {
        /// <summary>
        /// Gets all keys whose set of associated values passes the
        /// conditions specified in the <paramref name="selector"/>.
        /// </summary>
        /// <param name="selector">The predicate that selects which keys
        /// to return based on their value collection.</param>
        /// <returns>An enumeration of all selected keys.</returns>
        IEnumerable<TKey> GetKeys(Func<ISet<TVal>, bool> selector);
    }
}