using System.Collections.Generic;

namespace Anvoker.Maps.Interfaces
{
    /// <summary>
    /// Represents a read-only generic collection of keys and values where each
    /// key may be associated with multiple values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IReadOnlyMultiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IReadOnlyMultiMap{TKey, TVal}"/>.</typeparam>
    public interface IReadOnlyMultiMap<TKey, TVal> :
        IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>
    {
        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="IReadOnlyMultiMap{TKey, TVal}"/>.
        /// </summary>
        IEqualityComparer<TKey> ComparerKey { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="IReadOnlyMultiMap{TKey, TVal}"/>.
        /// </summary>
        IEqualityComparer<TVal> ComparerValue { get; }

        /// <summary>
        /// Determines whether the <see cref="IReadOnlyMultiMap{TKey, TVal}"/>
        /// contains at least one key associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate. The value can be null
        /// for reference types.</param>
        /// <returns>true if the <see cref="IReadOnlyMultiMap{TKey, TVal}"/>
        /// contains an element with the specified value; otherwise, false.
        /// </returns>
        bool ContainsValue(TVal value);
    }
}