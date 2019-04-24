using System.Collections.Generic;

namespace Anvoker.Maps.Interfaces
{
    /// <summary>
    /// Represents a generic collection of keys and values where keys cannot be
    /// added or removed and where each key may be associated with multiple
    /// values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IFixedKeysMultiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IFixedKeysMultiMap{TKey, TVal}"/>.</typeparam>
    public interface IFixedKeysMultiMap<TKey, TVal> :
        IReadOnlyMultiMap<TKey, TVal>
    {
        /// <summary>
        /// Adds the value to the specified key in the
        /// <see cref="IFixedKeysMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the value didn't exist already; otherwise,
        /// false.</returns>
        bool AddValue(TKey key, TVal value);

        /// <summary>
        /// Adds the values to the specified key in the
        /// <see cref="IFixedKeysMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if at least one value didn't exist already and was
        /// added; otherwise, false.</returns>
        bool AddValues(TKey key, IEnumerable<TVal> values);

        /// <summary>
        /// Removes the value associated with the specified key from the
        /// <see cref="IFixedKeysMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to remove from the element. Values
        /// can be null for reference types.</param>
        /// <returns>true if the element is successfully found and the value
        /// removed; otherwise, false.</returns>
        bool RemoveValue(TKey key, TVal value);

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="IFixedKeysMultiMap{TKey, TVal}"/> that are found in common
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
        /// <see cref="IFixedKeysMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <returns>true if the element is successfully found and its values
        /// removed; otherwise, false.</returns>
        bool RemoveValuesAll(TKey key);

        /// <summary>
        /// Replaces the values currently associated with the specified key with
        /// a new collection of values.
        /// </summary>
        /// <param name="key">The key of the value to replace.</param>
        /// <param name="values">The new values.</param>
        void Replace(TKey key, IEnumerable<TVal> values);
    }
}