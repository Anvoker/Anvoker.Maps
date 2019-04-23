using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable RCS1104 // Simplify conditional expression

namespace Anvoker.Maps
{
    /// <summary>
    /// Represents a generic collection of keys and values where keys can be
    /// retrieved by their associated value. The backing store consists of
    /// dictionaries with the values residing in a hashset.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys
    /// <see cref="CompositeBiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">The type of the values
    /// <see cref="CompositeBiMap{TKey, TVal}"/>.</typeparam>
    public class CompositeBiMap<TKey, TVal> :
        IBiMap<TKey, TVal>,
        IDictionary<TKey, TVal>
    {
        #region Private Fields

        /// <summary>
        /// An empty read-only collection to avoid instantiating new empty
        /// collections when one needs to be returned.
        /// </summary>
        private static readonly IReadOnlyCollection<TKey> empty = new List<TKey>();

        private readonly Dictionary<TKey, TVal> dictFwd;
        private readonly Dictionary<TVal, HashSet<TKey>> dictRev;

        /// <summary>
        /// Holds all of the keys that are associated with null values. We need
        /// it because dictRev cannot hold null as keys.
        /// </summary>
        private readonly HashSet<TKey> keysWithNullValue;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// class that is empty, has the default initial capacity, and uses
        /// the default equality comparers for the key and value types.
        /// </summary>
        public CompositeBiMap() : this(0, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// class that is empty, has the specified initial capacity, and uses
        /// the default equality comparers for the key and value types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="CompositeBiMap{TKey, TVal}"/> can contain.</param>
        public CompositeBiMap(int capacity) : this(capacity, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// class that is empty, has the default initial capacity, and uses
        /// the specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// value.</param>
        public CompositeBiMap(
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
            : this(0, comparerKey, comparerValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// class that is empty, has the specified initial capacity, and uses
        /// the specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="CompositeBiMap{TKey, TVal}"/> can contain.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// value.</param>
        public CompositeBiMap(
            int capacity,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity), "Capacity cannot be less than zero.");
            }

            dictFwd = new Dictionary<TKey, TVal>(capacity, comparerKey);
            dictRev = new Dictionary<TVal, HashSet<TKey>>(
                capacity,
                comparerValue);
            keysWithNullValue = new HashSet<TKey>(comparerKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// class that contains elements copied from the specified KeyValuePair
        /// collection and uses the default equality comparers for the key and
        /// value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection whose
        /// elements are copied to the new <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </param>
        public CompositeBiMap(ICollection<KeyValuePair<TKey, TVal>> keyValuePairs)
            : this(keyValuePairs, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// class that contains elements copied from the specified
        /// KeyValuePair collection and uses the specified equality comparers
        /// for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection whose
        /// elements are copied to the new <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// value.</param>
        public CompositeBiMap(
            ICollection<KeyValuePair<TKey, TVal>> keyValuePairs,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
            : this(keyValuePairs.Count, comparerKey, comparerValue)
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            PopulateReverseFromOtherDictionary(keyValuePairs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// class that contains keys copied from the specified enumeration
        /// and uses the default equality comparers for the key and value types.
        /// All of the keys start out associated with an empty value collection.
        /// </summary>
        /// <param name="keys">The enumeration of keys that are
        /// copied to the new <see cref="CompositeMultiBiMap{TKey, TVal}"/>. Cannot
        /// have duplicates.</param>
        public CompositeBiMap(IEnumerable<TKey> keys) : this(keys, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// class that contains keys copied from the specified enumeration
        /// and uses specified equality comparers for the key and value types.
        /// All of the keys start out associated with an empty value collection.
        /// </summary>
        /// <param name="keys">The enumeration of keys that are
        /// copied to the new <see cref="CompositeMultiBiMap{TKey, TVal}"/>. Cannot
        /// have duplicates.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// value.</param>
        public CompositeBiMap(
            IEnumerable<TKey> keys,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
            : this(0, comparerKey, comparerValue)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            foreach (TKey key in keys)
            {
                dictFwd.Add(key, default(TVal));
                AddRevEntry(key, default(TVal));
            }
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </summary>
        public IEqualityComparer<TKey> ComparerKey => dictFwd.Comparer;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </summary>
        public IEqualityComparer<TVal> ComparerValue => dictRev.Comparer;

        /// <summary>
        /// Gets the number of unique values contained in the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>
        /// </summary>
        public int UniqueValueCount => dictRev.Count;

        /// <summary>
        /// Gets the number of elements contained in the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>
        /// </summary>
        public int Count => dictFwd.Count;

        /// <summary>
        /// Gets a collection containing the keys in the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </summary>
        public IReadOnlyCollection<TKey> Keys => dictFwd.Keys;

        /// <summary>
        /// Gets a collection containing the values in the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </summary>
        public IReadOnlyCollection<TVal> Values => dictFwd.Values;

        ICollection<TKey> IDictionary<TKey, TVal>.Keys
            => ((IDictionary<TKey, TVal>)dictFwd).Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TVal>.Keys
            => ((IReadOnlyDictionary<TKey, TVal>)dictFwd).Keys;

        bool ICollection<KeyValuePair<TKey, TVal>>.IsReadOnly
            => ((IDictionary<TKey, TVal>)dictFwd).IsReadOnly;

        ICollection<TVal> IDictionary<TKey, TVal>.Values
            => ((IDictionary<TKey, TVal>)dictFwd).Values;

        IEnumerable<TVal> IReadOnlyDictionary<TKey, TVal>.Values
            => ((IReadOnlyDictionary<TKey, TVal>)dictFwd).Values;

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the
        /// specified key is not found, a get operation throws a
        /// <see cref="KeyNotFoundException"/>, and a set operation creates a
        /// new element with the specified key.</returns>
        public TVal this[TKey key]
        {
            get => dictFwd[key];
            set
            {
                if (ContainsKey(key))
                {
                    ReplaceValue(key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        #endregion Public Indexers

        #region Public Methods

        /// <summary>
        /// Adds the specified key and value to the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        public void Add(TKey key, TVal value)
        {
            dictFwd.Add(key, value);
            AddRevEntry(key, value);
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>. The internal key
        /// collections are cleared before being removed from the
        /// <see cref="CompositeBiMap{TKey, TVal}"/> which will reflect in any
        /// variable referencing them.
        /// </summary>
        public void Clear()
        {
            foreach (HashSet<TKey> hashSetKeys in dictRev.Values)
            {
                hashSetKeys.Clear();
            }

            dictFwd.Clear();
            dictRev.Clear();
            keysWithNullValue.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>.</param>
        /// <returns>true if the <see cref="CompositeBiMap{TKey, TVal}"/> contains an
        /// element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key) => dictFwd.ContainsKey(key);

        /// <summary>
        /// Determines whether the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// contains an element that has the specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>true if the <see cref="CompositeBiMap{TKey, TVal}"/>
        /// contains an element that has the specified value; otherwise,
        /// false.</returns>
        public bool ContainsValue(TVal value) => value == null
            ? keysWithNullValue.Count > 0
            : dictRev.ContainsKey(value);

        /// <summary>
        /// Returns an enumerator that iterates through the key-value pairs of
        /// the <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TVal}.Enumerator"/> structure
        /// for the <see cref="CompositeBiMap{TKey, TVal}"/>.</returns>
        public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
            => dictFwd.GetEnumerator();

        /// <summary>
        /// Gets all keys associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>A read-only collection containing all keys associated
        /// with the specified value.</returns>
        public IEnumerable<TKey> GetKeysWithValue(TVal value)
            => ContainsValue(value)
                ? (value == null ? keysWithNullValue : dictRev[value])
                : empty;

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="CompositeBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false. This method returns false if key is not found in
        /// the <see cref="CompositeBiMap{TKey, TVal}"/>.</returns>
        public bool Remove(TKey key)
            => (dictFwd.TryGetValue(key, out var value) && dictFwd.Remove(key))
                ? RemoveRevEntry(key, value)
                : false;

        /// <summary>
        /// Replaces the value currently associated with the specified key with
        /// a new value.
        /// </summary>
        /// <param name="key">The key of the value to replace.</param>
        /// <param name="value">The new value.</param>
        public void Replace(TKey key, TVal value) => ReplaceValue(key, value);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <param name="value">When this method returns, contains the value
        /// associated with the specified key, if the key is found; otherwise,
        /// the default value for the type of the value parameter.</param>
        /// <returns>true if the <see cref="CompositeBiMap{TKey, TVal}"/> contains an
        /// element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TVal value)
            => dictFwd.TryGetValue(key, out value);

        void ICollection<KeyValuePair<TKey, TVal>>.Add(KeyValuePair<TKey, TVal> item)
            => ((ICollection<KeyValuePair<TKey, TVal>>)dictFwd).Add(item);

        bool ICollection<KeyValuePair<TKey, TVal>>.Contains(KeyValuePair<TKey, TVal> item)
            => ((ICollection<KeyValuePair<TKey, TVal>>)dictFwd).Contains(item);

        bool ICollection<KeyValuePair<TKey, TVal>>.Remove(KeyValuePair<TKey, TVal> item)
            => ((ICollection<KeyValuePair<TKey, TVal>>)dictFwd).Remove(item);

        void ICollection<KeyValuePair<TKey, TVal>>.CopyTo(
            KeyValuePair<TKey, TVal>[] array,
            int arrayIndex)
            => ((IDictionary<TKey, TVal>)dictFwd).CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator()
            => ((IDictionary<TKey, TVal>)dictFwd).GetEnumerator();

        #endregion Public Methods

        #region Private Methods

        private void AddRevEntry(TKey key, TVal value)
        {
            if (value == null)
            {
                keysWithNullValue.Add(key);
            }
            else
            {
                if (dictRev.TryGetValue(value, out var keys))
                {
                    keys.Add(key);
                }
                else
                {
                    dictRev.Add(value, new HashSet<TKey>(ComparerKey) { key });
                }
            }
        }

        private bool RemoveRevEntry(TKey key, TVal value)
            => (value == null)
                ? keysWithNullValue.Remove(key)
                : dictRev[value].Remove(key);

        private void PopulateReverseFromOtherDictionary(
            ICollection<KeyValuePair<TKey, TVal>> kvps)
        {
            foreach (KeyValuePair<TKey, TVal> kvp in kvps)
            {
                if (kvp.Value == null)
                {
                    keysWithNullValue.Add(kvp.Key);
                    continue;
                }

                if (dictRev.ContainsKey(kvp.Value))
                {
                    dictRev[kvp.Value].Add(kvp.Key);
                }
                else
                {
                    dictRev[kvp.Value]
                        = new HashSet<TKey>(ComparerKey) { kvp.Key };
                }
            }
        }

        private void ReplaceValue(TKey key, TVal value)
        {
            TVal oldValue = dictFwd[key];
            dictFwd[key] = value;

            RemoveRevEntry(key, oldValue);
            AddRevEntry(key, value);
        }

        #endregion Private Methods
    }
}