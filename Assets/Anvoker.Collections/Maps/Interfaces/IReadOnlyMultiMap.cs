using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic read-only collection of key-values elements.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IReadOnlyMultiMap<TKey, TVal> :
        IReadOnlyDictionary<TKey, ICollection<TVal>>,
        IReadOnlyDictionary<TKey, TVal>
    {
        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="IBiMap{TKey, TVal}"/>.
        /// </summary>
        IEqualityComparer<TKey> ComparerKey { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="IBiMap{TKey, TVal}"/>.
        /// </summary>
        IEqualityComparer<TVal> ComparerValue { get; }

        /// <summary>
        /// Determines whether the <see cref="IReadOnlyMultiMap{TKey, TVal}"/>
        /// contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the
        /// <see cref="IReadOnlyMultiMap{TKey, TVal}"/>. The value can be null
        /// for reference types.</param>
        /// <returns>true if the <see cref="IReadOnlyMultiMap{TKey, TVal}"/>
        /// contains an element with the specified value; otherwise, false.
        /// </returns>
        bool ContainsValue(TVal value);

        /// <summary>
        /// Determines whether the <see cref="IReadOnlyMultiMap{TKey, TVal}"/>
        /// contains a specific value at the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="IReadOnlyMultiMap{TKey, TVal}"/>.</param>
        /// <param name="value">The value to locate in the
        /// <see cref="IReadOnlyMultiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="IReadOnlyMultiMap{TKey, TVal}"/>
        /// contains an element with the specified key and value; otherwise,
        /// false.</returns>
        bool ContainsValue(TKey key, TVal value);
    }
}