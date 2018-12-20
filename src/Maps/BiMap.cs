using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable RCS1169 // Mark field as read-only.
#pragma warning disable RCS1227 // Validate arguments correctly.
#pragma warning disable RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.
#pragma warning disable IDE0020 // Use pattern matching
#pragma warning disable IDE0034 // Simplify 'default' expression
#pragma warning disable IDE0027 // Use expression body for accessors

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a HashSet-based generic collection of key-value pairs where
    /// keys can also be retrieved by their associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.
    /// </typeparam>
    /// <typeparam name="TVal">The type of the values.
    /// </typeparam>
    public partial class BiMap<TKey, TVal> : IBiMap<TKey, TVal>
    {
        #region Private Fields

        private Dictionary<TKey, TVal> dictFwd;
        private Dictionary<TVal, HashSet<TKey>> dictRev;

        /// <summary>
        /// Holds all of the keys that are associated with null values. We need
        /// it because dictRev cannot hold null as keys.
        /// </summary>
        private HashSet<TKey> keysWithNullValue;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BiMap{TKey, TVal}"/>
        /// class that is empty, has the default initial capacity, and uses
        /// the default equality comparers for the key and value types.
        /// </summary>
        public BiMap() : this(0, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiMap{TKey, TVal}"/>
        /// class that is empty, has the specified initial capacity, and uses
        /// the default equality comparers for the key and value types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="BiMap{TKey, TVal}"/> can contain.</param>
        public BiMap(int capacity) : this(capacity, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiMap{TKey, TVal}"/>
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
        public BiMap(
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
            : this(0, comparerKey, comparerValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiMap{TKey, TVal}"/>
        /// class that is empty, has the specified initial capacity, and uses
        /// the specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="BiMap{TKey, TVal}"/> can contain.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// value.</param>
        public BiMap(
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
        /// Initializes a new instance of the <see cref="BiMap{TKey, TVal}"/>
        /// class that contains elements copied from the specified KeyValuePair
        /// collection and uses the default equality comparers for the key and
        /// value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection whose
        /// elements are copied to the new <see cref="BiMap{TKey, TVal}"/>.
        /// </param>
        public BiMap(ICollection<KeyValuePair<TKey, TVal>> keyValuePairs)
            : this(keyValuePairs, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiMap{TKey, TVal}"/>
        /// class that contains elements copied from the specified
        /// KeyValuePair collection and uses the specified equality comparers
        /// for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection whose
        /// elements are copied to the new <see cref="BiMap{TKey, TVal}"/>.
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
        public BiMap(
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
        /// Initializes a new instance of the <see cref="BiMap{TKey, TVal}"/>
        /// class that contains keys copied from the specified enumeration
        /// and uses the default equality comparers for the key and value types.
        /// All of the keys start out associated with an empty value collection.
        /// </summary>
        /// <param name="keys">The enumeration of keys that are
        /// copied to the new <see cref="MultiBiMap{TKey, TVal}"/>. Cannot
        /// have duplicates.</param>
        public BiMap(IEnumerable<TKey> keys) : this(keys, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiMap{TKey, TVal}"/>
        /// class that contains keys copied from the specified enumeration
        /// and uses specified equality comparers for the key and value types.
        /// All of the keys start out associated with an empty value collection.
        /// </summary>
        /// <param name="keys">The enumeration of keys that are
        /// copied to the new <see cref="MultiBiMap{TKey, TVal}"/>. Cannot
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
        public BiMap(
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
                AddReverseEntry(key, default(TVal));
            }
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        public IEqualityComparer<TKey> ComparerKey => dictFwd.Comparer;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        public IEqualityComparer<TVal> ComparerValue => dictRev.Comparer;

        /// <summary>
        /// Gets the number of elements contained in the
        /// <see cref="BiMap{TKey, TVal}"/>
        /// </summary>
        public int Count => dictFwd.Count;

        /// <summary>
        /// Gets a collection containing the keys in the
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        public IReadOnlyCollection<TKey> Keys => dictFwd.Keys;

        /// <summary>
        /// Gets the number of unique values contained in the
        /// <see cref="BiMap{TKey, TVal}"/>
        /// </summary>
        public int UniqueValueCount => dictRev.Count;

        /// <summary>
        /// Gets a collection containing the values in the
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        public IReadOnlyCollection<TVal> Values => dictFwd.Values;

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        /// Gets all keys that are associated with the specified value.
        /// </summary>
        /// <param name="val">The value to locate the keys by.</param>
        /// <returns>A read-only collection with all of the associated keys.
        /// </returns>
        public IReadOnlyCollection<TKey> this[TVal val]
            => GetKeysWithValue(val);

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public TVal this[TKey key]
        {
            get
            {
                return dictFwd[key];
            }

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
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        public void Add(TKey key, TVal value)
        {
            dictFwd.Add(key, value);
            AddReverseEntry(key, value);
        }

        /// <summary>
        /// Adds the specified key and value to the
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="item">The element to add.</param>
        public void Add(KeyValuePair<TKey, TVal> item)
            => Add(item.Key, item.Value);

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>. The internal key collections
        /// are not cleared before being removed from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        public void ShallowClear()
        {
            dictFwd.Clear();
            dictRev.Clear();
            keysWithNullValue.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="BiMap{TKey, TVal}"/> contains a
        /// specific key-value pair.
        /// </summary>
        /// <param name="item">The key-value pair to locate in the
        /// <see cref="BiMap{TKey, TVal}"/>.</param>
        /// <returns>true if the <see cref="BiMap{TKey, TVal}"/> contains the
        /// specified element; otherwise, false.</returns>
        public bool Contains(KeyValuePair<TKey, TVal> item)
            => ((ICollection<KeyValuePair<TKey, TVal>>)dictFwd).Contains(item);

        /// <summary>
        /// Determines whether the <see cref="BiMap{TKey, TVal}"/> contains a
        /// specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="BiMap{TKey, TVal}"/>.</param>
        /// <returns>true if the <see cref="BiMap{TKey, TVal}"/> contains an
        /// element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key) => dictFwd.ContainsKey(key);

        /// <summary>
        /// Determines whether the <see cref="BiMap{TKey, TVal}"/> contains a
        /// specific value.
        /// </summary>
        /// <param name="value">The value to locate in the
        /// <see cref="BiMap{TKey, TVal}"/>. The value can be null for reference
        /// types.</param>
        /// <returns>true if the <see cref="BiMap{TKey, TVal}"/> contains an
        /// element with the specified value; otherwise, false.</returns>
        public bool ContainsValue(TVal value) => dictFwd.ContainsValue(value);

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="BiMap{TKey, TVal}"/>. The internal key collections are
        /// cleared before being removed from the <see cref="BiMap{TKey, TVal}"/>
        /// which will reflect in any variable referencing them though read-only
        /// wrappers.
        /// </summary>
        public void Clear()
        {
            foreach (HashSet<TKey> hashSetKeys in dictRev.Values)
            {
                hashSetKeys.Clear();
            }

            ShallowClear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the key-value pairs of
        /// the <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TVal}.Enumerator"/> structure
        /// for the <see cref="BiMap{TKey, TVal}"/>.</returns>
        public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
            => dictFwd.GetEnumerator();

        /// <summary>
        /// Gets all keys that are associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate the keys by.</param>
        /// <returns>A read-only collection with all of the associated keys.
        /// </returns>
        public IReadOnlyCollection<TKey> GetKeysWithValue(TVal value)
        {
            if (ContainsValue(value))
            {
                if (value == null)
                {
                    return keysWithNullValue;
                }
                else
                {
                    return dictRev[value];
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false.</returns>
        public bool Remove(TKey key)
        {
            if (ContainsKey(key))
            {
                return false;
            }

            var value = dictFwd[key];
            dictFwd.Remove(key);

            if (value == null)
            {
                keysWithNullValue.Remove(key);
            }
            else
            {
                dictRev[value].Remove(key);
            }

            return true;
        }

        /// <summary>
        /// Removes the specified element from the
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="item">The key-value pair to remove from the
        /// <see cref="BiMap{TKey, TVal}"/>.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false.</returns>
        public bool Remove(KeyValuePair<TKey, TVal> item)
            => dictFwd.Remove(item.Key);

        /// <summary>
        /// Replaces the value currently associated with the specified key.
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
        /// <returns>true if the <see cref="BiMap{TKey, TVal}"/> contains an
        /// element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TVal value)
            => dictFwd.TryGetValue(key, out value);

        #endregion Public Methods

        #region Private Methods

        private void AddReverseEntry(TKey key, TVal value)
        {
            if (value == null)
            {
                if (ContainsValue(value))
                {
                    dictRev[value].Add(key);
                }
                else
                {
                    dictRev.Add(value, new HashSet<TKey>(ComparerKey) { key });
                }
            }
            else
            {
                if (!ContainsValue(value))
                {
                    keysWithNullValue.Add(key);
                }
                else
                {
                    dictRev.Add(value, new HashSet<TKey>(ComparerKey) { key });
                }
            }
        }

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

            if (oldValue == null)
            {
                keysWithNullValue.Remove(key);
            }
            else
            {
                dictRev[oldValue].Remove(key);
            }

            AddReverseEntry(key, value);
        }

        #endregion Private Methods
    }

    /// <content>
    /// Explicit interface implementations.
    /// </content>
    public partial class BiMap<TKey, TVal> : IBiMap<TKey, TVal>
    {
        #region Public Properties

        bool ICollection<KeyValuePair<TKey, TVal>>.IsReadOnly => false;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TVal>.Keys
            => dictFwd.Keys;

        ICollection<TKey> IDictionary<TKey, TVal>.Keys => dictFwd.Keys;

        IEnumerable<TVal> IReadOnlyDictionary<TKey, TVal>.Values
            => dictFwd.Values;

        ICollection<TVal> IDictionary<TKey, TVal>.Values => dictFwd.Values;

        #endregion Public Properties

        #region Public Methods

        void ICollection<KeyValuePair<TKey, TVal>>.CopyTo(
            KeyValuePair<TKey, TVal>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<TKey, TVal>>)dictFwd)
            .CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => dictFwd.GetEnumerator();

        #endregion Public Methods
    }
}