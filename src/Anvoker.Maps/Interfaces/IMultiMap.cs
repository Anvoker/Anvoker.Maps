using System.Collections.Generic;

namespace Anvoker.Maps
{
    /// <summary>
    /// Represents a generic collection of keys and values where each key may be
    /// associated with multiple values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IMultiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IMultiMap{TKey, TVal}"/>.</typeparam>
    public interface IMultiMap<TKey, TVal> :
        IFixedKeysMultiMap<TKey, TVal>
    {
        /// <summary>
        /// Adds the specified key to the <see cref="IMultiMap{TKey, TVal}"/>
        /// with no associated values.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        void Add(TKey key);

        /// <summary>
        /// Adds the specified key and value to the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        void Add(TKey key, TVal value);

        /// <summary>
        /// Adds the specified key and values to the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add.</param>
        void Add(TKey key, IEnumerable<TVal> values);

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="IMultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false. This method returns false if key is not found in
        /// the <see cref="IMultiMap{TKey, TVal}"/>.</returns>
        bool Remove(TKey key);
    }
}