using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of key-value pairs where keys can be
    /// retrieved by their associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IBiMap<TKey, TVal> : IDictionary<TKey, TVal>
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
        /// Gets the number of unique values in the
        /// <see cref="IBiMap{TKey, TVal}"/>.
        /// </summary>
        int UniqueValueCount { get; }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        new TVal this[TKey key] { get; set; }

        /// <summary>
        /// Gets all keys that are associated with the specified value.
        /// </summary>
        /// <param name="val">The value to locate the keys by.</param>
        /// <returns>A read-only collection with all of the associated keys.
        /// </returns>
        IReadOnlyCollection<TKey> this[TVal val] { get; }

        /// <summary>
        /// Determines whether the <see cref="IBiMap{TKey, TVal}"/> contains a
        /// specific value.
        /// </summary>
        /// <param name="value">The value to locate in the
        /// <see cref="IBiMap{TKey, TVal}"/>. The value can be null for reference
        /// types.</param>
        /// <returns>true if the <see cref="IBiMap{TKey, TVal}"/> contains an
        /// element with the specified value; otherwise, false.</returns>
        bool ContainsValue(TVal value);

        /// <summary>
        /// Gets all keys that are associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate the keys by.</param>
        /// <returns>A read-only collection with all of the associated keys.
        /// </returns>
        IReadOnlyCollection<TKey> GetKeysWithValue(TVal value);

        /// <summary>
        /// Returns an enumerator that iterates through the values-key elements
        /// of the <see cref="IBiMap{TKey, TVal}"/>. Since the same value can be
        /// associated with multiple keys, the keys are grouped in their own
        /// collection in each element.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TVal, IReadOnlyCollection{TKey}}
        /// .Enumerator"/> structure for the
        /// <see cref="IBiMap{TKey, TVal}"/>.</returns>
        IEnumerator<KeyValuePair<TVal, IReadOnlyCollection<TKey>>>
            GetReverseEnumerator();

        /// <summary>
        /// Replaces the value currently associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to replace.</param>
        /// <param name="value">The new value.</param>
        void Replace(TKey key, TVal value);
    }
}