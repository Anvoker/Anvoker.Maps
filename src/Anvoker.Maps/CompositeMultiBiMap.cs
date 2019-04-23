using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable RCS1104 // Simplify conditional expression

namespace Anvoker.Maps
{
    /// <summary>
    /// Represents a generic collection of keys and values where each key may be
    /// associated with multiple values and where keys can be retrieved by their
    /// associated value. The backing store consists of dictionaries with the
    /// values residing in a hashset.
    /// </summary>
    /// <typeparam name="TK">The type of the keys.
    /// </typeparam>
    /// <typeparam name="TV">The type of the values.
    /// </typeparam>
    public class CompositeMultiBiMap<TK, TV> :
        IMultiBiMap<TK, TV>,
        IDictionary<TK, ISet<TV>>,
        ISetQueryableKeys<TK, TV>
    {
        #region Private Fields

        private readonly ICollectionEqualityComparer<TV, IReadOnlyCollection<TV>> comparerCollection;
        private readonly Dictionary<TK, ValueSet<TK, TV>> dictFwd;
        private readonly Dictionary<ValueSet<TK, TV>, TK> dictRev;
        private readonly Dictionary<TV, HashSet<TK>> dictRevFlat;

        /// <summary>
        /// Holds all of the keys that are associated with null values. We need
        /// it because <see cref="dictRevFlat"/> cannot hold null as keys.
        /// </summary>
        private readonly HashSet<TK> hashRevFlatNullKeys;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that is empty, has the
        /// default initial capacity, and uses the default equality comparers
        /// for the key and value types.
        /// </summary>
        public CompositeMultiBiMap() : this(0, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that is empty, has the
        /// specified initial capacity, and uses the default equality comparers
        /// for the key and value types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> can contain.</param>
        public CompositeMultiBiMap(int capacity) : this(capacity, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that is empty, has the
        /// default initial capacity, and uses the specified equality comparers
        /// for the key and value types.
        /// </summary>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiBiMap(
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
            : this(0, comparerKey, comparerValue)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that is empty, has the
        /// specified initial capacity, and uses the specified equality
        /// comparers for the key and value types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> can contain.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiBiMap(
            int capacity,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity), "Capacity cannot be less than zero.");
            }

            dictFwd = new Dictionary<TK, ValueSet<TK, TV>>(capacity, comparerKey);
            dictRev = new Dictionary<ValueSet<TK, TV>, TK>(capacity);
            dictRevFlat = new Dictionary<TV, HashSet<TK>>(capacity, comparerValue);
            ComparerKey = comparerKey ?? EqualityComparer<TK>.Default;
            ComparerValue = comparerValue ?? EqualityComparer<TV>.Default;
            comparerCollection = new CollectionEqualityComparer<TV, IReadOnlyCollection<TV>>(
                comparerValue);
            hashRevFlatNullKeys = new HashSet<TK>(comparerKey);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that contains keys
        /// copied from the specified collection and uses the default equality
        /// comparers for the key and value types. <para>All of the keys start
        /// out associated with an empty value collection.</para>
        /// </summary>
        /// <param name="keys">The enumeration of keys that are
        /// copied to the new <see cref="CompositeMultiBiMap{TK, TV}"/>. Cannot
        /// have duplicates.</param>
        public CompositeMultiBiMap(IEnumerable<TK> keys) : this(keys, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that contains keys copied
        /// from the specified collection and uses the specified equality
        /// comparers for the key and value types. <para>All of the keys start
        /// out associated with an empty value collection.</para>
        /// </summary>
        /// <param name="keys">The enumeration of keys that are
        /// copied to the new <see cref="CompositeMultiBiMap{TK, TV}"/>. Cannot
        /// have duplicates.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiBiMap(
            IEnumerable<TK> keys,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
            : this(0, comparerKey, comparerValue)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            try
            {
                PopulateWithEmptyHashsets(keys);
            }
            catch (ArgumentException)
            {
                var hash = new HashSet<TK>(comparerKey);
                foreach (TK key in keys)
                {
                    if (!hash.Add(key))
                    {
                        throw new ArgumentException(
                            @"The specified enumeration of keys to copy cannot
                            have duplicates.",
                            nameof(keys));
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that contains elements
        /// copied from the specified KeyValuePair collection and uses the
        /// default equality comparers for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection that is
        /// copied to the new <see cref="CompositeMultiBiMap{TK, TV}"/>.</param>
        public CompositeMultiBiMap(
            ICollection<KeyValuePair<TK, HashSet<TV>>> keyValuePairs)
            : this(keyValuePairs, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that contains elements
        /// copied from the specified KeyValuePair collection and uses the
        /// specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection that is
        /// copied to the new <see cref="CompositeMultiBiMap{TK, TV}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiBiMap(
            ICollection<KeyValuePair<TK, HashSet<TV>>> keyValuePairs,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
            : this(keyValuePairs.Count, comparerKey, comparerValue)
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            foreach (KeyValuePair<TK, HashSet<TV>> kvp in keyValuePairs)
            {
                AddNewKey(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that contains elements
        /// copied from the specified KeyValuePair collection and uses the
        /// specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection that is
        /// copied to the new <see cref="CompositeMultiBiMap{TK, TV}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiBiMap(
            ICollection<KeyValuePair<TK, TV>> keyValuePairs,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
            : this(keyValuePairs.Count, comparerKey, comparerValue)
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            foreach (KeyValuePair<TK, TV> kvp in keyValuePairs)
            {
                AddNewKey(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> class that contains elements
        /// copied from the specified KeyValuePair collection and uses the
        /// specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection that is
        /// copied to the new <see cref="CompositeMultiBiMap{TK, TV}"/>.</param>
        public CompositeMultiBiMap(
            ICollection<KeyValuePair<TK, TV>> keyValuePairs)
            : this(keyValuePairs, null, null)
        { }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        public IEqualityComparer<TK> ComparerKey { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        public IEqualityComparer<TV> ComparerValue { get; }

        /// <summary>
        /// Gets the number of unique values in the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        public int UniqueValueCount => dictRevFlat.Count;

        /// <summary>
        /// Gets the number of key-to-values elements contained in the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>
        /// </summary>
        public int Count => dictFwd.Count;

        /// <summary>
        /// Gets an enumeration of the <see cref="CompositeMultiBiMap{TK, TV}"/>'s
        /// keys.
        /// </summary>
        public IReadOnlyCollection<TK> Keys => dictFwd.Keys;

        /// <summary>
        /// Gets an enumeration of the <see cref="CompositeMultiBiMap{TK, TV}"/>'s
        /// values, grouped by their respective key.
        /// </summary>
        public IReadOnlyCollection<ISet<TV>> Values => dictFwd.Values;

        bool ICollection<KeyValuePair<TK, ISet<TV>>>.IsReadOnly => false;

        IEqualityComparer<IReadOnlyCollection<TV>>
            IReadOnlyBiMap<TK, IReadOnlyCollection<TV>>.ComparerValue
            => comparerCollection;

        IEnumerable<TK> IReadOnlyDictionary<TK, IReadOnlyCollection<TV>>.Keys
            => dictFwd.Keys;

        ICollection<TK> IDictionary<TK, ISet<TV>>.Keys => dictFwd.Keys;

        IEnumerable<IReadOnlyCollection<TV>>
            IReadOnlyDictionary<TK, IReadOnlyCollection<TV>>.Values
            => dictFwd.Values;

        ICollection<ISet<TV>> IDictionary<TK, ISet<TV>>.Values
            => dictFwd.Values.ToList<ISet<TV>>();

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key. If the
        /// specified key is not found, a get operation throws a
        /// <see cref="KeyNotFoundException"/>
        /// </returns>
        public ValueSet<TK, TV> this[TK key] => dictFwd[key];

        IReadOnlyCollection<TV>
            IReadOnlyDictionary<TK, IReadOnlyCollection<TV>>.this[TK key]
            => dictFwd[key];

        ISet<TV> IDictionary<TK, ISet<TV>>.this[TK key]
        {
            get => dictFwd[key];
            set
            {
                if (ContainsKey(key))
                {
                    Replace(key, value);
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
        /// Adds the specified key to the <see cref="CompositeMultiBiMap{TK, TV}"/>
        /// with no associated values.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        public void Add(TK key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            AddNewKey(key);
        }

        /// <summary>
        /// Adds the specified key and value to the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        public void Add(TK key, TV value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            AddNewKey(key, value);
        }

        /// <summary>
        /// Adds the specified key and values to the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add.</param>
        public void Add(TK key, IEnumerable<TV> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            AddNewKey(key, values);
        }

        /// <summary>
        /// Adds the value to the specified key in the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the value didn't exist already; otherwise,
        /// false.</returns>
        public bool AddValue(TK key, TV value)
            => (dictFwd.TryGetValue(key, out var hashSetValue) && hashSetValue.Internal.Add(value))
                ? AddRevFlatEntry(key, value)
                : false;

        /// <summary>
        /// Adds the values to the specified key in the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if at least one value didn't exist already and was
        /// added; otherwise, false.</returns>
        public bool AddValues(TK key, IEnumerable<TV> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (!dictFwd.TryGetValue(key, out var hashSetValues))
            {
                return false;
            }

            bool atLeastOneAdded = false;

            foreach (TV val in values)
            {
                if (atLeastOneAdded |= hashSetValues.Add(val))
                {
                    AddRevFlatEntry(key, val);
                }
            }

            return atLeastOneAdded;
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>. The internal key
        /// collections and value sets are cleared before being removed from the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> which will reflect in
        /// any variable referencing them.
        /// </summary>
        public void Clear()
        {
            foreach (var hashSetValues in dictFwd.Values)
            {
                hashSetValues.Internal.Clear();
            }

            foreach (var hashSetKeys in dictRevFlat.Values)
            {
                hashSetKeys.Clear();
            }

            dictFwd.Clear();
            dictRev.Clear();
            dictRevFlat.Clear();
            hashRevFlatNullKeys.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="CompositeMultiBiMap{TK, TV}"/>
        /// contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.</param>
        /// <returns>true if the <see cref="CompositeMultiBiMap{TK, TV}"/> contains an
        /// element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TK key) => dictFwd.ContainsKey(key);

        /// <summary>
        /// Determines whether the <see cref="CompositeMultiBiMap{TK, TV}"/>
        /// contains at least one key associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate. The value can be null
        /// for reference types.</param>
        /// <returns>true if the <see cref="CompositeMultiBiMap{TK, TV}"/>
        /// contains an element with the specified value; otherwise, false.
        /// </returns>
        public bool ContainsValue(TV value)
            => value == null
                ? hashRevFlatNullKeys.Count > 0
                : dictRevFlat.ContainsKey(value);

        /// <summary>
        /// Returns an enumerator that iterates through the key-value pairs of
        /// the <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TVal}.Enumerator"/> structure
        /// for the <see cref="CompositeMultiBiMap{TK, TV}"/>.</returns>
        public IEnumerator<KeyValuePair<TK, ValueSet<TK, TV>>> GetEnumerator()
            => dictFwd.GetEnumerator();

        /// <summary>
        /// Gets all keys whose collection of associated values passes the
        /// conditions specified in the <paramref name="selector"/>.
        /// </summary>
        /// <param name="selector">The predicate that selects which keys
        /// to return based on their value collection.</param>
        /// <returns>An enumeration of all selected keys.</returns>
        public IEnumerable<TK> GetKeys(Func<IEnumerable<TV>, bool> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return GetKeysIterator();

            IEnumerable<TK> GetKeysIterator()
            {
                foreach (var mapValues in dictRev.Keys)
                {
                    if (selector(mapValues))
                    {
                        yield return dictRev[mapValues];
                    }
                }
            }
        }

        /// <summary>
        /// Gets all keys whose set of associated values passes the
        /// conditions specified in the <paramref name="selector"/>.
        /// </summary>
        /// <param name="selector">The predicate that selects which keys
        /// to return based on their value collection.</param>
        /// <returns>An enumeration of all selected keys.</returns>
        public IEnumerable<TK> GetKeys(Func<ISet<TV>, bool> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return GetKeysIterator();

            IEnumerable<TK> GetKeysIterator()
            {
                foreach (var mapValues in dictRev.Keys)
                {
                    if (selector(mapValues.Internal))
                    {
                        yield return dictRev[mapValues];
                    }
                }
            }
        }

        /// <summary>
        /// Gets all keys that have any values in common with the specified
        /// collection.
        /// </summary>
        /// <param name="values">The values to test against for overlap.</param>
        /// <returns>An enumeration containing all keys whose value collection
        /// has any values in common with the specified collection.</returns>
        public IEnumerable<TK> GetKeysWithAny(IEnumerable<TV> values)
            => GetKeys(mapValues => mapValues.Overlaps(values));

        /// <summary>
        /// Gets all keys associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>An enumeration containing all keys associated with the
        /// specified value.</returns>
        public IEnumerable<TK> GetKeysWithValue(TV value)
            => dictRevFlat.TryGetValue(value, out var keys)
                ? keys
                : Enumerable.Empty<TK>();

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false. This method returns false if key is not found in
        /// the <see cref="CompositeMultiBiMap{TK, TV}"/>.</returns>
        public bool Remove(TK key)
        {
            if (dictFwd.TryGetValue(key, out var hashSetValues)
                && dictFwd.Remove(key)
                && dictRev.Remove(hashSetValues))
            {
                foreach (TV val in hashSetValues)
                {
                    RemoveRevFlatEntry(key, val);
                }

                hashSetValues.Internal.Clear();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the value associated with the specified key from the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to remove from the element. Values
        /// can be null for reference types.</param>
        /// <returns>true if the element is successfully found and the value
        /// removed; otherwise, false.</returns>
        public bool RemoveValue(TK key, TV value)
            => (dictFwd.TryGetValue(key, out var valueSet) && valueSet.Internal.Remove(value))
                ? RemoveRevFlatEntry(key, value)
                : false;

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/> that are found in common
        /// with the specified enumeration of values.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to remove from the element. Values
        /// can be null for reference types.</param>
        /// <returns>true if the element is successfully found and at least one
        /// value removed; otherwise, false.</returns>
        public bool RemoveValues(TK key, IEnumerable<TV> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!dictFwd.TryGetValue(key, out var hashSetValues))
            {
                return false;
            }

            bool removedAtLeastOne = false;

            foreach (TV val in values)
            {
                if (hashSetValues.Internal.Remove(val))
                {
                    removedAtLeastOne |= RemoveRevFlatEntry(key, val);
                }
            }

            return removedAtLeastOne;
        }

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="CompositeMultiBiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <returns>true if the element is successfully found and its values
        /// removed; otherwise, false.</returns>
        public bool RemoveValuesAll(TK key)
        {
            if (!dictFwd.TryGetValue(key, out var hashSetValues))
            {
                return false;
            }

            foreach (TV val in hashSetValues)
            {
                RemoveRevFlatEntry(key, val);
            }

            hashSetValues.Internal.Clear();
            return true;
        }

        /// <summary>
        /// Replaces the values currently associated with the specified key with
        /// a new collection of values.
        /// </summary>
        /// <param name="key">The key of the value to replace.</param>
        /// <param name="values">The new values.</param>
        public void Replace(TK key, IEnumerable<TV> values)
        {
            if (!dictFwd.TryGetValue(key, out var hashSetValues))
            {
                throw new KeyNotFoundException();
            }

            foreach (TV val in hashSetValues)
            {
                RemoveRevFlatEntry(key, val);
            }

            hashSetValues.Clear();

            foreach (TV val in values)
            {
                AddRevFlatEntry(key, val);
                hashSetValues.Internal.Add(val);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <param name="values">When this method returns, contains the values
        /// associated with the specified key, if the key is found; otherwise,
        /// null.</param>
        /// <returns>true if the <see cref="CompositeMultiBiMap{TK, TV}"/> contains an
        /// element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TK key, out ValueSet<TK, TV> values)
        {
            bool success = dictFwd.TryGetValue(
                key, out ValueSet<TK, TV> hashSet);
            values = hashSet;
            return success;
        }

        void IDictionary<TK, ISet<TV>>
            .Add(TK key, ISet<TV> value)
            => AddNewKey(key, value);

        void ICollection<KeyValuePair<TK, ISet<TV>>>
            .Add(KeyValuePair<TK, ISet<TV>> item)
            => AddNewKey(item.Key, item.Value);

        bool ICollection<KeyValuePair<TK, ISet<TV>>>
            .Contains(KeyValuePair<TK, ISet<TV>> item)
            => dictFwd.TryGetValue(item.Key, out var values)
            && values.SetEquals(item.Value);

        bool IReadOnlyBiMap<TK, IReadOnlyCollection<TV>>
            .ContainsValue(IReadOnlyCollection<TV> value)
        {
            foreach (var kvp in dictRev)
            {
                if (kvp.Key.SetEquals(value))
                {
                    return true;
                }
            }

            return false;
        }

        void ICollection<KeyValuePair<TK, ISet<TV>>>
            .CopyTo(KeyValuePair<TK, ISet<TV>>[] array, int arrayIndex)
        {
            foreach (var kvp in dictFwd)
            {
                array[arrayIndex++]
                    = new KeyValuePair<TK, ISet<TV>>(kvp.Key, kvp.Value);
            }
        }

        IEnumerator<KeyValuePair<TK, IReadOnlyCollection<TV>>>
            IEnumerable<KeyValuePair<TK, IReadOnlyCollection<TV>>>.GetEnumerator()
        {
            foreach (var kvp in dictFwd)
            {
                yield return new KeyValuePair<TK, IReadOnlyCollection<TV>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        IEnumerator<KeyValuePair<TK, ISet<TV>>>
            IEnumerable<KeyValuePair<TK, ISet<TV>>>.GetEnumerator()
        {
            foreach (var kvp in dictFwd)
            {
                yield return new KeyValuePair<TK, ISet<TV>>(kvp.Key, kvp.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerable<TK>
            IReadOnlyBiMap<TK, IReadOnlyCollection<TV>>.GetKeysWithValue(
            IReadOnlyCollection<TV> value)
        {
            foreach (var kvp in dictRev)
            {
                if (kvp.Key.SetEquals(value))
                {
                    yield return kvp.Value;
                }
            }
        }

        bool ICollection<KeyValuePair<TK, ISet<TV>>>
            .Remove(KeyValuePair<TK, ISet<TV>> item)
            => TryGetValue(item.Key, out var values)
            && item.Value.SetEquals(values)
            && Remove(item.Key);

        void IFixedKeysBiMap<TK, IReadOnlyCollection<TV>>
            .Replace(TK key, IReadOnlyCollection<TV> value)
            => Replace(key, value);

        bool IReadOnlyDictionary<TK, IReadOnlyCollection<TV>>.TryGetValue(
            TK key,
            out IReadOnlyCollection<TV> values)
        {
            bool success = TryGetValue(key, out var valueSets);
            values = valueSets;
            return success;
        }

        bool IDictionary<TK, ISet<TV>>.TryGetValue(TK key, out ISet<TV> value)
        {
            bool success = TryGetValue(key, out var valueSets);
            value = valueSets;
            return success;
        }

        #endregion Public Methods

        #region Private Methods

        private void AddNewKey(TK key)
        {
            var hashSetValues = new ValueSet<TK, TV>(key, this);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
        }

        private void AddNewKey(TK key, TV value)
        {
            var hashSetValues = new ValueSet<TK, TV>(key, this, value);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            AddRevFlatEntry(key, value);
        }

        private void AddNewKey(TK key, IEnumerable<TV> values)
        {
            var hashSetValues = new ValueSet<TK, TV>(key, this, values);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            foreach (TV val in hashSetValues)
            {
                AddRevFlatEntry(key, val);
            }
        }

        private void AddNewKey(TK key, HashSet<TV> values)
        {
            var hashSetValues = new ValueSet<TK, TV>(key, this, values);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            foreach (TV val in hashSetValues)
            {
                AddRevFlatEntry(key, val);
            }
        }

        private bool AddRevFlatEntry(TK key, TV value)
        {
            if (value != null)
            {
                if (dictRevFlat.TryGetValue(value, out var hashSetKeys))
                {
                    return hashSetKeys.Add(key);
                }
                else
                {
                    dictRevFlat.Add(value, new HashSet<TK>(ComparerKey) { key });
                    return true;
                }
            }
            else
            {
                return hashRevFlatNullKeys.Add(key);
            }
        }

        private void PopulateWithEmptyHashsets(IEnumerable<TK> enumerable)
        {
            foreach (TK key in enumerable)
            {
                var hashSet = new ValueSet<TK, TV>(key, this);
                dictFwd.Add(key, hashSet);
                dictRev.Add(hashSet, key);
            }
        }

        private bool RemoveRevFlatEntry(TK key, TV val)
                    => (val != null)
                ? dictRevFlat.TryGetValue(val, out var hashSetKeys)
                    ? hashSetKeys.Remove(key)
                    : false
                : hashRevFlatNullKeys.Remove(key);

        #endregion Private Methods
    }
}