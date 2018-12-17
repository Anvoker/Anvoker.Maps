using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of key-values elements.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public interface IMultiMap<TKey, TVal> :
        IDictionary<TKey, ICollection<TVal>>,
        IFixedKeysMultiMap<TKey, TVal>
    {
        /// <summary>
        /// Gets the number of elements contained in the
        /// <see cref="IMultiMap{TKey, TVal}"/>
        /// </summary>
        new int Count { get; }

        new IReadOnlyCollection<TKey> Keys { get; }

        new IReadOnlyCollection<IReadOnlyCollection<TVal>> Values { get; }

        new IEnumerable<TVal> this[TKey key] { get; set; }

        new IEnumerator<KeyValuePair<TKey, ICollection<TVal>>> GetEnumerator();

        new bool TryGetValue(TKey key, out ICollection<TVal> values);

        /// <summary>
        /// Determines whether the <see cref="IMultiMap{TKey, TVal}"/> contains
        /// a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="IMultiMap{TKey, TVal}"/>.</param>
        /// <returns>true if the <see cref="IMultiMap{TKey, TVal}"/> contains
        /// an element with the specified key; otherwise, false.</returns>
        new bool ContainsKey(TKey key);

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
    }
}