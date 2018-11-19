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
    /// Represents a generic collection of key-values pairs where keys can also
    /// be retrieved by their associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.
    /// </typeparam>
    /// <typeparam name="TVal">The type of the values.
    /// </typeparam>
    public partial class MultiBiMap<TKey, TVal> : IMultiBiMap<TKey, TVal>,
        IReadOnlyMultiBiMap<TKey, TVal>, IFixedKeysMultiBiMap<TKey, TVal>
    {
        #region Private Fields

        private Dictionary<TKey, HashSet<TVal>> dictFwd;
        private Dictionary<HashSet<TVal>, TKey> dictRev;
        private Dictionary<TVal, HashSet<TKey>> dictRevFlat;

        private KeyCollection keyCollection;
        private ValueCollection valueCollection;

        /// <summary>
        /// Keeps track of how often a value occurs in the multimap.
        /// </summary>
        private Dictionary<TVal, int> dictValOccurance;

        /// <summary>
        /// Holds all of the keys that are associated with null values. We need
        /// it because <see cref="dictRevFlat"/> cannot hold null as keys.
        /// </summary>
        private HashSet<TKey> keysWithNullValue;

        /// <summary>
        /// Keeps track of how often null occurs as a value in the multimap.
        /// This is needed because dictionaries can't hold null as keys.
        /// </summary>
        private int nullValOccurance;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that is empty, has the
        /// default initial capacity, and uses the default equality comparers
        /// for the key and value types.
        /// </summary>
        public MultiBiMap() : this(0, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that is empty, has the
        /// specified initial capacity, and uses the default equality comparers
        /// for the key and value types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="MultiBiMap{TKey, TVal}"/> can contain.</param>
        public MultiBiMap(int capacity) : this(capacity, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that is empty, has the
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
        public MultiBiMap(
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
            : this(0, comparerKey, comparerValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that is empty, has the
        /// specified initial capacity, and uses the specified equality
        /// comparers for the key and value types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="MultiBiMap{TKey, TVal}"/> can contain.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public MultiBiMap(
            int capacity,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity), "Capacity cannot be less than zero.");
            }

            dictFwd = new Dictionary<TKey, HashSet<TVal>>(
                capacity, comparerKey);
            dictRev = new Dictionary<HashSet<TVal>, TKey>(capacity);
            dictRevFlat = new Dictionary<TVal, HashSet<TKey>>(
                capacity, comparerValue);
            keysWithNullValue = new HashSet<TKey>(comparerKey);
            dictValOccurance = new Dictionary<TVal, int>(comparerValue);
            keyCollection = new KeyCollection(this);
            valueCollection = new ValueCollection(this);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that contains keys
        /// copied from the specified collection and uses the default equality
        /// comparers for the key and value types. <para>All of the keys start
        /// out associated with an empty value collection.</para>
        /// </summary>
        /// <param name="keys">The enumeration of keys that are
        /// copied to the new <see cref="MultiBiMap{TKey, TVal}"/>. Cannot
        /// have duplicates.</param>
        public MultiBiMap(IEnumerable<TKey> keys) : this(keys, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that contains keys copied
        /// from the specified collection and uses the specified equality
        /// comparers for the key and value types. <para>All of the keys start
        /// out associated with an empty value collection.</para>
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
        /// values.</param>
        public MultiBiMap(
            IEnumerable<TKey> keys,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
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
                var hash = new HashSet<TKey>(comparerKey);
                foreach (TKey key in keys)
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
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that contains elements
        /// copied from the specified KeyValuePair collection and uses the
        /// default equality comparers for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection that is
        /// copied to the new <see cref="MultiBiMap{TKey, TVal}"/>.</param>
        public MultiBiMap(
            ICollection<KeyValuePair<TKey, HashSet<TVal>>> keyValuePairs)
            : this(keyValuePairs, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that contains elements
        /// copied from the specified KeyValuePair collection and uses the
        /// specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection that is
        /// copied to the new <see cref="MultiBiMap{TKey, TVal}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public MultiBiMap(
            ICollection<KeyValuePair<TKey, HashSet<TVal>>> keyValuePairs,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
            : this(keyValuePairs.Count, comparerKey, comparerValue)
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            foreach (KeyValuePair<TKey, HashSet<TVal>> kvp in keyValuePairs)
            {
                AddNewKey(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that contains elements
        /// copied from the specified KeyValuePair collection and uses the
        /// specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection that is
        /// copied to the new <see cref="MultiBiMap{TKey, TVal}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public MultiBiMap(
            ICollection<KeyValuePair<TKey, TVal>> keyValuePairs,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
            : this(keyValuePairs.Count, comparerKey, comparerValue)
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            foreach (KeyValuePair<TKey, TVal> kvp in keyValuePairs)
            {
                AddNewKey(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiBiMap{TKey, TVal}"/> class that contains elements
        /// copied from the specified KeyValuePair collection and uses the
        /// specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="keyValuePairs">The KeyValuePair collection that is
        /// copied to the new <see cref="MultiBiMap{TKey, TVal}"/>.</param>
        public MultiBiMap(
            ICollection<KeyValuePair<TKey, TVal>> keyValuePairs)
            : this(keyValuePairs, null, null)
        {
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
        public IEqualityComparer<TVal> ComparerValue => dictRevFlat.Comparer;

        /// <summary>
        /// Gets the number of key-to-values elements contained in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>
        /// </summary>
        public int Count => dictFwd.Count;

        /// <summary>
        /// Gets an enumeration of the <see cref="MultiBiMap{TKey, TVal}"/>'s
        /// keys.
        /// </summary>
        public IEnumerable<TKey> Keys => dictFwd.Keys;

        /// <summary>
        /// Gets the number of unique values in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        public int UniqueValueCount => dictRevFlat.Count;

        /// <summary>
        /// Gets an enumeration of the <see cref="MultiBiMap{TKey, TVal}"/>'s
        /// values, grouped by their respective key.
        /// </summary>
        public IEnumerable<IReadOnlyCollection<TVal>> Values => dictFwd.Values;

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        /// Gets the value collection associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value collection to get.</param>
        /// <returns>A read-only wrapper to the value collection associated
        /// with the specified key.</returns>
        public IReadOnlyCollection<TVal> this[TKey key] => dictFwd[key];

        #endregion Public Indexers

        #region Public Methods

        /// <summary>
        /// Adds the specified key to the <see cref="MultiBiMap{TKey, TVal}"/>
        /// with an empty value collection.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        public void AddKey(TKey key)
        {
            try
            {
                AddNewKey(key);
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Keys cannot be null.");
                }

                throw;
            }
        }

        /// <summary>
        /// Adds the specified key and its associated value to the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        public void AddKey(TKey key, TVal value)
        {
            try
            {
                AddNewKey(key, value);
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Keys cannot be null.");
                }

                throw;
            }
        }

        /// <summary>
        /// Adds the specified key and its associated values to the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add.</param>
        public void AddKey(TKey key, IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                AddNewKey(key, values);
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Keys cannot be null.");
                }

                throw;
            }
        }

        /// <summary>
        /// Adds the value to the specified key in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the value didn't exist already; otherwise,
        /// false.</returns>
        public bool AddValue(TKey key, TVal value)
        {
            try
            {
                return AddExistingKey(key, value);
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Keys cannot be null.");
                }

                throw;
            }
        }

        /// <summary>
        /// Adds the values to the specified key in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to add to the element.</param>
        /// <returns>true if at least one value didn't exist already and was
        /// added; otherwise, false.</returns>
        public bool AddValues(TKey key, IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                return AddExistingKey(key, values);
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Keys cannot be null.");
                }

                throw;
            }
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>. The internal key and value
        /// collections are not cleared before being removed from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        public void Clear()
        {
            dictFwd.Clear();
            dictRev.Clear();
            dictRevFlat.Clear();
            dictValOccurance.Clear();
            keysWithNullValue.Clear();
            nullValOccurance = 0;
        }

        /// <summary>
        /// Determines whether the <see cref="MultiBiMap{TKey, TVal}"/> contains
        /// a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.</param>
        /// <returns>true if the <see cref="MultiBiMap{TKey, TVal}"/> contains
        /// an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key) => dictFwd.ContainsKey(key);

        /// <summary>
        /// Determines whether the <see cref="MultiBiMap{TKey, TVal}"/> contains
        /// a specific value at the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.</param>
        /// <param name="value">The value to locate in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="MultiBiMap{TKey, TVal}"/> contains
        /// an element with the specified key and value; otherwise, false.
        /// </returns>
        public bool ContainsValue(TKey key, TVal value)
            => dictFwd.ContainsKey(key) && dictFwd[key].Contains(value);

        /// <summary>
        /// Determines whether the <see cref="MultiBiMap{TKey, TVal}"/> contains
        /// a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="MultiBiMap{TKey, TVal}"/> contains
        /// an element with the specified value; otherwise, false.</returns>
        public bool ContainsValue(TVal value)
        {
            if (value == null)
            {
                return keysWithNullValue.Count > 0;
            }

            return dictRevFlat.ContainsKey(value);
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>. The internal key and value
        /// collections are cleared before being removed from the
        /// <see cref="MultiBiMap{TKey, TVal}"/> which will reflect in any
        /// variable referencing them.
        /// </summary>
        public void DeepClear()
        {
            foreach (HashSet<TVal> hashSetValues in dictFwd.Values)
            {
                hashSetValues.Clear();
            }

            foreach (HashSet<TKey> hashSetKeys in dictRevFlat.Values)
            {
                hashSetKeys.Clear();
            }

            Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the key-values elements
        /// of the <see cref="MultiBiMap{TKey, TVal}"/>, with values associated
        /// with a key being grouped in their own collection in each entry.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, IReadOnlyCollection{TVal}}
        /// .Enumerator"/> structure for the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.</returns>
        public IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TVal>>>
            GetEnumerator()
            => ((IDictionary<TKey, IReadOnlyCollection<TVal>>)dictFwd)
            .GetEnumerator();

        /// <summary>
        /// Gets keys whose associated value collection has at least one value
        /// in common with the specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        public IEnumerable<TKey> GetKeysWithAny(IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var matchingKeys = new HashSet<TKey>(ComparerKey);

            foreach (TVal value in values)
            {
                if (value != null)
                {
                    if (dictRevFlat.ContainsKey(value))
                    {
                        foreach (TKey key in dictRevFlat[value])
                        {
                            if (!matchingKeys.Contains(key))
                            {
                                matchingKeys.Add(key);
                            }
                        }
                    }
                }
                else
                {
                    foreach (TKey key in keysWithNullValue)
                    {
                        if (!matchingKeys.Contains(key))
                        {
                            matchingKeys.Add(key);
                        }
                    }
                }
            }

            return matchingKeys;
        }

        /// <summary>
        /// Gets keys whose associated value collection satisfies set equality
        /// with the specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        public IEnumerable<TKey> GetKeysWithEqualSet(IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valuesHashSet = new HashSet<TVal>(values, ComparerValue);
            foreach (HashSet<TVal> mapValues in dictRev.Keys)
            {
                if (mapValues.SetEquals(valuesHashSet))
                {
                    yield return dictRev[mapValues];
                }
            }
        }

        /// <summary>
        /// Gets keys whose associated value collection is a subset of the
        /// specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        public IEnumerable<TKey> GetKeysWithSubset(IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valuesHashSet = new HashSet<TVal>(values, ComparerValue);
            foreach (HashSet<TVal> mapValues in dictRev.Keys)
            {
                if (mapValues.IsSubsetOf(valuesHashSet))
                {
                    yield return dictRev[mapValues];
                }
            }
        }

        /// <summary>
        /// Gets keys whose associated value collection is a superset of the
        /// specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        public IEnumerable<TKey> GetKeysWithSuperset(IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valuesHashSet = new HashSet<TVal>(values, ComparerValue);
            foreach (HashSet<TVal> mapValues in dictRev.Keys)
            {
                if (mapValues.IsSupersetOf(valuesHashSet))
                {
                    yield return dictRev[mapValues];
                }
            }
        }

        /// <summary>
        /// Gets all keys that are associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate the keys by.</param>
        /// <returns>A read-only wrapper to the collection of the associated
        /// keys associated with the specified value.</returns>
        public IReadOnlyCollection<TKey> GetKeysWithValue(TVal value)
        {
            if (value != null)
            {
                bool success = dictRevFlat.TryGetValue(
                    value, out HashSet<TKey> result);
                if (success)
                {
                    return result;
                }
            }
            else
            {
                return keysWithNullValue;
            }

            return null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the values-key elements
        /// of the <see cref="MultiBiMap{TKey, TVal}"/>. Since the same value can be
        /// associated with multiple keys, keys are groupedin  in their own
        /// collection in each element.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TVal, IReadOnlyCollection{TKey}}
        /// .Enumerator"/> structure for the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.</returns>
        public IEnumerator<KeyValuePair<TVal, IReadOnlyCollection<TKey>>>
            GetReverseEnumerator()
            => ((IDictionary<TVal, IReadOnlyCollection<TKey>>)dictRevFlat)
            .GetEnumerator();

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false.</returns>
        public bool RemoveKey(TKey key)
        {
            try
            {
                if (dictFwd.ContainsKey(key))
                {
                    return false;
                }
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Keys cannot be null.");
                }

                throw;
            }

            var hashSet = dictFwd[key];
            dictFwd.Remove(key);
            dictRev.Remove(hashSet);

            foreach (TVal val in hashSet)
            {
                if (val != null)
                {
                    dictRevFlat[val].Remove(key);
                    DecrementOrRemoveValueOccurance(val);
                }
                else
                {
                    keysWithNullValue.Remove(key);
                    nullValOccurance--;
                }
            }

            hashSet.Clear();

            return true;
        }

        /// <summary>
        /// Removes the value associated with the specified key from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="value">The value to remove from the element. Values
        /// can be null for reference types.</param>
        /// <returns>true if the element is successfully found and the value
        /// removed; otherwise, false.</returns>
        public bool RemoveValue(TKey key, TVal value)
        {
            try
            {
                if (dictFwd.ContainsKey(key) || dictFwd[key].Contains(value))
                {
                    return false;
                }
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Keys cannot be null.");
                }

                throw;
            }

            dictFwd[key].Remove(value);

            if (value != null)
            {
                dictRevFlat[value].Remove(key);
                DecrementOrRemoveValueOccurance(value);
            }
            else
            {
                keysWithNullValue.Remove(key);
                nullValOccurance--;
            }

            return true;
        }

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="values">The values to remove from the element.</param>
        /// <returns>true if the element is successfully found and at least one
        /// value removed; otherwise, false.</returns>
        public bool RemoveValues(TKey key, IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                if (dictFwd.ContainsKey(key))
                {
                    return false;
                }
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Key cannot be null.");
                }

                throw;
            }

            var hashSet = dictFwd[key];

            foreach (TVal val in values)
            {
                if (dictFwd[key].Contains(val))
                {
                    dictFwd[key].Remove(val);

                    if (val != null)
                    {
                        dictRevFlat[val].Remove(key);
                        DecrementOrRemoveValueOccurance(val);
                    }
                    else
                    {
                        keysWithNullValue.Remove(key);
                        nullValOccurance--;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public void RemoveValuesAll(TKey key)
        {
            HashSet<TVal> hashSetValues = null;

            try
            {
                hashSetValues = dictFwd[key];
            }
            catch (Exception e)
            when (e is ArgumentNullException || e is KeyNotFoundException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Key cannot be null.");
                }

                throw;
            }

            foreach (TVal val in hashSetValues)
            {
                if (val != null)
                {
                    dictRevFlat[val].Remove(key);
                    DecrementOrRemoveValueOccurance(val);
                }
                else
                {
                    keysWithNullValue.Remove(key);
                    nullValOccurance--;
                }
            }

            hashSetValues.Clear();
        }

        /// <summary>
        /// Removes a value collection from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>, locating it by reference
        /// equality comparison. The value collection will be emptied and
        /// replaced with a new collection instance.
        /// </summary>
        /// <param name="values">The value collection to remove from the
        /// element.</param>
        /// <returns>true if the value collection is successfully found removed;
        /// otherwise, false.</returns>
        public bool RemoveValuesByRef(IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values is HashSet<TVal>)
            {
                var hash = (HashSet<TVal>)values;

                if (dictRev.TryGetValue(hash, out TKey key))
                {
                    hash.Clear();
                    RemoveKey(key);
                    AddNewKey(key);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a key associated with the specified value collection where the
        /// key is only retrieved if the specified value collection fulfills
        /// reference equality with the value collection in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>
        /// </summary>
        /// <param name="values">The value collection of the key to get.</param>
        /// <param name="key">When this method returns, contains the key
        /// associated with the specified value collection, if the value
        /// collection is found; otherwise, the default value for the type of
        /// the value parameter.</param>
        /// <returns>true if the <see cref="MultiBiMap{TKey, TVal}"/> contains the
        /// specified value collection; otherwise, false.</returns>
        public bool TryGetKeyByCollectionRef(
            IReadOnlyCollection<TVal> values, out TKey key)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values is HashSet<TVal>)
            {
                var hash = (HashSet<TVal>)values;
                return dictRev.TryGetValue(hash, out key);
            }

            key = default(TKey);
            return false;
        }

        /// <summary>
        /// Gets the value collection associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <param name="value">When this method returns, contains a read-only
        /// wrapper to the value collection associated with the specified key,
        /// if the key is found; otherwise, the default value for the type of
        /// the value parameter.</param>
        /// <returns>true if the <see cref="MultiBiMap{TKey, TVal}"/> contains
        /// an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out IReadOnlyCollection<TVal> value)
        {
            HashSet<TVal> hashSet = null;
            bool success = false;

            try
            {
                success = dictFwd.TryGetValue(key, out hashSet);
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Key cannot be null.");
                }

                throw;
            }

            value = hashSet;
            return success;
        }

        #endregion Public Methods

        #region Private Methods

        private bool AddExistingKey(TKey key, TVal val)
        {
            var valuesHashSet = dictFwd[key];

            if (valuesHashSet.Contains(val))
            {
                return false;
            }

            valuesHashSet.Add(val);

            if (val != null)
            {
                AddFlatEntry(key, val);
                IncrementOrSetValueOccurance(val);
            }
            else
            {
                keysWithNullValue.Add(key);
                nullValOccurance++;
            }

            return true;
        }

        private bool AddExistingKey(TKey key, IEnumerable<TVal> vals)
        {
            var hashSetValues = dictFwd[key];
            bool atLeastOneNewValueAdded = false;

            foreach (TVal val in vals)
            {
                bool doesNotContain = !hashSetValues.Contains(val);
                atLeastOneNewValueAdded |= doesNotContain;
                if (doesNotContain)
                {
                    hashSetValues.Add(val);

                    if (val != null)
                    {
                        AddFlatEntry(key, val);
                        IncrementOrSetValueOccurance(val);
                    }
                    else
                    {
                        keysWithNullValue.Add(key);
                        nullValOccurance++;
                    }
                }
            }

            return atLeastOneNewValueAdded;
        }

        private void AddFlatEntry(TKey key, TVal val)
        {
            if (dictRevFlat.ContainsKey(val))
            {
                dictRevFlat[val].Add(key);
            }
            else
            {
                dictRevFlat.Add(val, new HashSet<TKey>(ComparerKey) { key });
            }
        }

        private void AddNewKey(TKey key)
        {
            var hashSetValues = new HashSet<TVal>(ComparerValue);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
        }

        private void AddNewKey(TKey key, TVal val)
        {
            var hashSetValues = new HashSet<TVal>(ComparerValue) { val };
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            if (val != null)
            {
                AddFlatEntry(key, val);
                IncrementOrSetValueOccurance(val);
            }
            else
            {
                keysWithNullValue.Add(key);
                nullValOccurance++;
            }
        }

        private void AddNewKey(TKey key, IEnumerable<TVal> vals)
        {
            var hashSetValues = new HashSet<TVal>(vals, ComparerValue);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            foreach (TVal val in hashSetValues)
            {
                if (val != null)
                {
                    AddFlatEntry(key, val);
                    IncrementOrSetValueOccurance(val);
                }
                else
                {
                    keysWithNullValue.Add(key);
                    nullValOccurance++;
                }
            }
        }

        private void AddNewKey(TKey key, HashSet<TVal> vals)
        {
            var hashSetValues = new HashSet<TVal>(vals, ComparerValue);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            foreach (TVal val in hashSetValues)
            {
                if (val != null)
                {
                    AddFlatEntry(key, val);
                    IncrementOrSetValueOccurance(val);
                }
                else
                {
                    keysWithNullValue.Add(key);
                    nullValOccurance++;
                }
            }
        }

        private void DecrementOrRemoveValueOccurance(TVal val)
        {
            if (dictValOccurance[val] == 1)
            {
                dictValOccurance.Remove(val);
            }
            else
            {
                dictValOccurance[val]--;
            }
        }

        private void IncrementOrSetValueOccurance(TVal val)
        {
            if (dictValOccurance.ContainsKey(val))
            {
                dictValOccurance[val]++;
            }
            else
            {
                dictValOccurance[val] = 1;
            }
        }

        private void PopulateWithEmptyHashsets(IEnumerable<TKey> enumerable)
        {
            foreach (TKey key in enumerable)
            {
                var hashSet = new HashSet<TVal>(ComparerValue);
                dictFwd.Add(key, hashSet);
                dictRev.Add(hashSet, key);
            }
        }

        #endregion Private Methods
    }

    /// <content>
    /// Explicit interface implementations.
    /// </content>
    public partial class MultiBiMap<TKey, TVal>
    {
        #region Public Properties

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>.IsReadOnly
            => false;

        ICollection<TKey> IDictionary<TKey, ICollection<TVal>>.Keys
            => dictFwd.Keys;

        ICollection<ICollection<TVal>> IDictionary<TKey, ICollection<TVal>>
            .Values => (ICollection<ICollection<TVal>>)
            (ICollection<HashSet<TVal>>)dictFwd.Values;

        IEnumerable<TVal> IReadOnlyDictionary<TKey, TVal>.Values
        {
            get
            {
                foreach (HashSet<TVal> values in dictFwd.Values)
                {
                    foreach (TVal val in values)
                    {
                        yield return val;
                    }
                }
            }
        }

        IEnumerable<ICollection<TVal>>
            IReadOnlyDictionary<TKey, ICollection<TVal>>.Values
            => dictFwd.Values;

        #endregion Public Properties

        #region Public Indexers

        ICollection<TVal>
            IReadOnlyDictionary<TKey, ICollection<TVal>>.this[TKey key]
            => dictFwd[key];

        TVal IReadOnlyDictionary<TKey, TVal>.this[TKey key]
        {
            get
            {
                foreach (TVal val in dictFwd[key])
                {
                    return val;
                }

                if (dictFwd[key].Count <= 0)
                {
                    throw new InvalidOperationException(
                        "There are no values associated with this key.");
                }
                else
                {
                    throw new InvalidProgramException();
                }
            }
        }

        ICollection<TVal> IDictionary<TKey, ICollection<TVal>>.this[TKey key]
        {
            get
            {
                return dictFwd[key];
            }

            set
            {
                RemoveKey(key);
                AddKey(key, value);
            }
        }

        #endregion Public Indexers

        #region Public Methods

        void IDictionary<TKey, ICollection<TVal>>.Add(
            TKey key, ICollection<TVal> value) => AddNewKey(key, value);

        void ICollection<KeyValuePair<TKey, ICollection<TVal>>>.Add(
            KeyValuePair<TKey, ICollection<TVal>> item)
            => AddKey(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>.Contains(
            KeyValuePair<TKey, ICollection<TVal>> item)
            => ContainsKey(item.Key)
            && dictFwd[item.Key].SetEquals(item.Value);

        void ICollection<KeyValuePair<TKey, ICollection<TVal>>>.CopyTo(
            KeyValuePair<TKey, ICollection<TVal>>[] array,
            int arrayIndex)
            => ((ICollection<KeyValuePair<TKey, ICollection<TVal>>>)dictFwd)
            .CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => dictFwd.GetEnumerator();

        IEnumerator<KeyValuePair<TKey, TVal>>
            IEnumerable<KeyValuePair<TKey, TVal>>.GetEnumerator()
        {
            foreach (KeyValuePair<TKey, HashSet<TVal>> kvp in dictFwd)
            {
                foreach (TVal val in dictFwd[kvp.Key])
                {
                    yield return new KeyValuePair<TKey, TVal>(kvp.Key, val);
                }
            }
        }

        IEnumerator<KeyValuePair<TKey, ICollection<TVal>>>
            IEnumerable<KeyValuePair<TKey, ICollection<TVal>>>.GetEnumerator()
            => (IEnumerator<KeyValuePair<TKey, ICollection<TVal>>>)
            GetEnumerator();

        bool IDictionary<TKey, ICollection<TVal>>.Remove(TKey key)
            => RemoveKey(key);

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>.Remove(
            KeyValuePair<TKey, ICollection<TVal>> item) => RemoveKey(item.Key);

        bool IReadOnlyDictionary<TKey, ICollection<TVal>>.TryGetValue(
            TKey key,
            out ICollection<TVal> value)
            => ((IDictionary<TKey, ICollection<TVal>>)this).TryGetValue(
                key, out value);

        bool IReadOnlyDictionary<TKey, TVal>.TryGetValue(
            TKey key, out TVal value)
        {
            HashSet<TVal> hashSet;
            bool success = false;

            try
            {
                success = dictFwd.TryGetValue(key, out hashSet);
            }
            catch (ArgumentNullException)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(
                        nameof(key), "Key cannot be null.");
                }

                throw;
            }

            if (success)
            {
                foreach (TVal val in hashSet)
                {
                    value = val;
                    return true;
                }
            }

            value = default(TVal);
            return false;
        }

        bool IDictionary<TKey, ICollection<TVal>>.TryGetValue(
            TKey key, out ICollection<TVal> value)
        {
            bool success = TryGetValue(
                key, out IReadOnlyCollection<TVal> hashSetValues);
            value = (ICollection<TVal>)hashSetValues;
            return success;
        }

        #endregion Public Methods
    }
}