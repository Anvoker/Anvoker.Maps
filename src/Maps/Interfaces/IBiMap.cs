using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a generic collection of keys and values where keys can be
    /// retrieved by their associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="IBiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="IBiMap{TKey, TVal}"/>.</typeparam>
    public interface IBiMap<TKey, TVal> : IFixedKeysBiMap<TKey, TVal>
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the
        /// specified key is not found, a get operation throws a
        /// <see cref="KeyNotFoundException"/>, and a set operation creates a
        /// new element with the specified key.</returns>
        new TVal this[TKey key] { get; set; }

        /// <summary>
        /// Adds the specified key and value to the
        /// <see cref="IBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        void Add(TKey key, TVal value);

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="IBiMap{TKey, TVal}"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="IBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false. This method returns false if key is not found in
        /// the <see cref="IBiMap{TKey, TVal}"/>.</returns>
        bool Remove(TKey key);
    }
}
