using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of key-values elements.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IMultiMap<TKey, TVal> :
        IDictionary<TKey, ICollection<TVal>>
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
        /// Adds the specified key to the
        /// <see cref="IMultiMap{TKey, TVal}"/> with an empty value
        /// collection.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        void AddKey(TKey key);

        /// <summary>
        /// Adds the specified key and its associated value to the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        void AddKey(TKey key, TVal value);

        /// <summary>
        /// Adds the specified key and its associated values to the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add. The value can
        /// be null for reference types.</param>
        void AddKey(TKey key, IEnumerable<TVal> values);

        /// <summary>
        /// Adds the value to an existing specified key in the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the key was found and the value didn't exist
        /// already; otherwise, false.</returns>
        bool AddValue(TKey key, TVal value);

        /// <summary>
        /// Adds the values to an existing specified key in the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the key was found and at least one value didn't
        /// exist already; otherwise, false.</returns>
        bool AddValues(TKey key, IEnumerable<TVal> values);

        /// <summary>
        /// Determines whether the <see cref="IMultiMap{TKey, TVal}"/>
        /// contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the
        /// <see cref="IMultiMap{TKey, TVal}"/>. The value can be null
        /// for reference types.</param>
        /// <returns>true if the <see cref="IMultiMap{TKey, TVal}"/>
        /// contains an element with the specified value; otherwise, false.
        /// </returns>
        bool ContainsValue(TVal value);

        /// <summary>
        /// Determines whether the <see cref="IMultiMap{TKey, TVal}"/> contains
        /// a specific value at the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="IMultiMap{TKey, TVal}"/>.</param>
        /// <param name="value">The value to locate in the
        /// <see cref="IMultiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="IMultiMap{TKey, TVal}"/> contains
        /// an element with the specified key and value; otherwise, false.
        /// </returns>
        bool ContainsKeyWithValue(TKey key, TVal value);

        /// <summary>
        /// Determines whether the <see cref="IMultiMap{TKey, TVal}"/> contains
        /// all of the specified values at the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="IMultiMap{TKey, TVal}"/>.</param>
        /// <param name="values">The values to locate in the
        /// <see cref="IMultiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="IMultiMap{TKey, TVal}"/> contains
        /// an element with the specified key and values; otherwise, false.
        /// </returns>
        bool ContainsKeyWithValues(TKey key, IEnumerable<TVal> values);

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false.</returns>
        bool RemoveKey(TKey key);

        /// <summary>
        /// Removes the value associated with the specified key from the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to remove from the element. Values
        /// can be null for reference types.</param>
        /// <returns>true if the element is successfully found and the value
        /// removed; otherwise, false.</returns>
        bool RemoveValue(TKey key, TVal value);

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="IMultiMap{TKey, TVal}"/> that are found in common
        /// with the specified enumeration of values.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to remove from the element. Values
        /// can be null for reference types.</param>
        /// <returns>true if the element is successfully found and at least one
        /// value removed; otherwise, false.</returns>
        bool RemoveValues(TKey key, IEnumerable<TVal> values);

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        void RemoveValuesAll(TKey key);
    }
}