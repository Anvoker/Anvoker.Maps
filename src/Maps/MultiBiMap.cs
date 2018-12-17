using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable RCS1169 // Mark field as read-only.

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a HashSet-based generic collection of key-values mappings
    /// where  keys can also be retrieved by their associated value. Values must
    /// be unique per key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.
    /// </typeparam>
    /// <typeparam name="TVal">The type of the values.
    /// </typeparam>
    public partial class MultiBiMap<TKey, TVal> : IMultiBiMap<TKey, TVal>
    {
        #region Private Fields

        private IEqualityComparer<TKey> comparerKey;
        private IEqualityComparer<TVal> comparerValue;
        private Dictionary<TKey, ValueSet<TKey, TVal>> dictFwd;
        private Dictionary<ValueSet<TKey, TVal>, TKey> dictRev;
        private Dictionary<TVal, HashSet<TKey>> dictRevFlat;

        /// <summary>
        /// Holds all of the keys that are associated with null values. We need
        /// it because <see cref="dictRevFlat"/> cannot hold null as keys.
        /// </summary>
        private HashSet<TKey> keysWithNullValue;

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

            dictFwd = new Dictionary<TKey, ValueSet<TKey, TVal>>(
                capacity, comparerKey);
            dictRev = new Dictionary<ValueSet<TKey, TVal>, TKey>(capacity);
            dictRevFlat = new Dictionary<TVal, HashSet<TKey>>(
                capacity, comparerValue);
            this.comparerKey = comparerKey
                ?? EqualityComparer<TKey>.Default;
            this.comparerValue = comparerValue
                ?? EqualityComparer<TVal>.Default;
            keysWithNullValue = new HashSet<TKey>(comparerKey);
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
        public IEqualityComparer<TKey> ComparerKey => comparerKey;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="BiMap{TKey, TVal}"/>.
        /// </summary>
        public IEqualityComparer<TVal> ComparerValue => comparerValue;

        /// <summary>
        /// Gets the number of unique values in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        public int UniqueValueCount => dictRevFlat.Count;

        /// <summary>
        /// Gets the number of key-to-values elements contained in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>
        /// </summary>
        public int Count => dictFwd.Count;

        /// <summary>
        /// Gets an enumeration of the <see cref="MultiBiMap{TKey, TVal}"/>'s
        /// keys.
        /// </summary>
        public IReadOnlyCollection<TKey> Keys => dictFwd.Keys;

        /// <summary>
        /// Gets an enumeration of the <see cref="MultiBiMap{TKey, TVal}"/>'s
        /// values, grouped by their respective key.
        /// </summary>
        public IReadOnlyCollection<ISet<TVal>> Values => dictFwd.Values;

        public IEnumerable<TVal> this[TKey key]
        {
            get => dictFwd[key];
            set => Replace(key, value);
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// If the specified key already exists in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>, adds the specified value to
        /// that key; otherwise it adds a new element with the specified key and
        /// value.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value of the element. The value can
        /// be null for reference types.</param>
        /// <returns>True if either the key didn't already exist or the value
        /// didn't already exist on the specified key; otherwise, false.
        /// </returns>
        public bool Add(TKey key, TVal value)
        {
            if (ContainsKey(key))
            {
                return AddValue(key, value);
            }
            else
            {
                AddKey(key, value);
                return true;
            }
        }

        /// <summary>
        /// If the specified key already exists in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>, adds the specified values to
        /// that key; otherwise it adds a new element with the specified key and
        /// values.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values of the element.</param>
        /// <returns>True if either the key didn't already exist or at least one
        /// value didn't already exist on the specified key; otherwise, false.
        /// </returns>
        public bool Add(TKey key, IEnumerable<TVal> values)
        {
            if (ContainsKey(key))
            {
                return AddValues(key, values);
            }
            else
            {
                AddKey(key, values);
                return true;
            }
        }

        /// <summary>
        /// If the specified key already exists in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>, adds the specified values to
        /// that key; otherwise it adds a new element with the specified key and
        /// values. <para>This doesn't copy the collection, it passes it by
        /// reference.</para> <para>Will throw <see cref="ArgumentException"/>
        /// if the passed <see cref="HashSet{TVal}"/>'s comparer isn't reference
        /// equal to <see cref="ComparerValue"/>.</para>
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values of the element.</param>
        /// <returns>True if either the key didn't already exist or at least one
        /// value didn't already exist on the specified key; otherwise, false.
        /// </returns>
        public bool Add(TKey key, HashSet<TVal> values)
        {
            if (ContainsKey(key) && values.Comparer.Equals(ComparerValue))
            {
                return AddValues(key, values);
            }
            else
            {
                AddKey(key, values);
                return true;
            }
        }

        /// <summary>
        /// Adds the specified key to the <see cref="MultiBiMap{TKey, TVal}"/>
        /// with an empty value collection.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        public void AddKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            AddNewKey(key);
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
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            AddNewKey(key, value);
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

            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            AddNewKey(key, values);
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
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            return AddExistingKey(key, value);
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
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return AddExistingKey(key, values);
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>. The internal key and value
        /// collections are cleared before being removed from the
        /// <see cref="MultiBiMap{TKey, TVal}"/> which will reflect in any
        /// variable referencing them.
        /// </summary>
        public void Clear()
        {
            foreach (var hashSetValues in dictFwd.Values)
            {
                hashSetValues.Clear();
            }

            foreach (var hashSetKeys in dictRevFlat.Values)
            {
                hashSetKeys.Clear();
            }

            ShallowClear();
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
        public bool ContainsKeyWithValue(TKey key, TVal value)
            => dictFwd.ContainsKey(key) && dictFwd[key].Contains(value);

        /// <summary>
        /// Determines whether the <see cref="MultiBiMap{TKey, TVal}"/> contains
        /// all of the specified values at the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.</param>
        /// <param name="values">The values to locate in the
        /// <see cref="MultiBiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="MultiMap{TKey, TVal}"/> contains
        /// an element with the specified key and values; otherwise, false.
        /// </returns>
        public bool ContainsKeyWithValues(TKey key, IEnumerable<TVal> values)
            => dictFwd.ContainsKey(key) && dictFwd[key].SetEquals(values);

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

        public IEnumerator<KeyValuePair<TKey, ISet<TVal>>> GetEnumerator()
        {
            foreach (var kvp in dictFwd)
            {
                yield return new KeyValuePair<TKey, ISet<TVal>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        /// <summary>
        /// Gets keys whose associated value collection has at least one value
        /// in common with the specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        public IEnumerable<TKey> GetKeys(Func<IEnumerable<TVal>, bool> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return GetKeysIterator();

            IEnumerable<TKey> GetKeysIterator()
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

        public IEnumerable<TKey> GetKeys(Func<HashSet<TVal>, bool> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return GetKeysIterator();

            IEnumerable<TKey> GetKeysIterator()
            {
                foreach (var mapValues in dictRev.Keys)
                {
                    if (selector(mapValues.HashSet))
                    {
                        yield return dictRev[mapValues];
                    }
                }
            }
        }

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

            return GetKeys((HashSet<TVal> x) => x.Overlaps(values));
        }

        /// <summary>
        /// Gets keys whose associated value collection satisfies set equality
        /// with the specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        public IEnumerable<TKey> GetKeysWithEqualSet(
            IEnumerable<TVal> values,
            bool ignoreKeysWithNoValues = true)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (ignoreKeysWithNoValues)
            {
                return GetKeys((HashSet<TVal> x) => x.Count > 0
                    && x.SetEquals(values));
            }
            else
            {
                return GetKeys((HashSet<TVal> x) => x.SetEquals(values));
            }
        }

        /// <summary>
        /// Gets keys whose associated value collection is a subset of the
        /// specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        public IEnumerable<TKey> GetKeysWithSubset(
            IEnumerable<TVal> values,
            bool ignoreKeysWithNoValues = true)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (ignoreKeysWithNoValues)
            {
                return GetKeys((HashSet<TVal> x) => x.Count > 0
                    && x.IsSubsetOf(values));
            }
            else
            {
                return GetKeys((HashSet<TVal> x) => x.IsSubsetOf(values));
            }
        }

        /// <summary>
        /// Gets keys whose associated value collection is a superset of the
        /// specified collection.
        /// </summary>
        /// <param name="values">The collection of values to search by.</param>
        /// <returns>An enumeration of all of the associated keys.
        /// </returns>
        public IEnumerable<TKey> GetKeysWithSuperset(
            IEnumerable<TVal> values,
            bool ignoreKeysWithNoValues = true)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (ignoreKeysWithNoValues)
            {
                return GetKeys((HashSet<TVal> x) => x.Count > 0
                    && x.IsSupersetOf(values));
            }
            else
            {
                return GetKeys((HashSet<TVal> x) => x.IsSupersetOf(values));
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the key-values elements
        /// of the <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <returns>A enumerator structure for the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.</returns>
        public IEnumerator<KeyValuePair<IReadOnlyCollection<TVal>, TKey>>
            GetReverseEnumerator()
        {
            foreach (var kvp in dictRev)
            {
                yield return new
                    KeyValuePair<IReadOnlyCollection<TVal>, TKey>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false.</returns>
        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            if (!dictFwd.ContainsKey(key))
            {
                return false;
            }

            var hashSet = dictFwd[key];
            bool fwdSuccess = dictFwd.Remove(key);
            bool revSuccess = dictRev.Remove(hashSet);

#if DEVELOPMENT_BUILD || UNITY_EDITOR || DEBUG
            if (!fwdSuccess)
            {
                throw new InvalidProgramException();
            }

            if (!revSuccess)
            {
                throw new InvalidProgramException();
            }
#endif

            foreach (TVal val in hashSet)
            {
                if (val != null)
                {
                    bool revFlatSuccess = dictRevFlat[val].Remove(key);
#if DEVELOPMENT_BUILD || UNITY_EDITOR || DEBUG
                    if (!revFlatSuccess)
                    {
                        throw new InvalidProgramException();
                    }
#endif
                }
                else
                {
                    bool keysNullSuccess = keysWithNullValue.Remove(key);
#if DEVELOPMENT_BUILD || UNITY_EDITOR || DEBUG
                    if (!keysNullSuccess)
                    {
                        throw new InvalidProgramException();
                    }
#endif
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
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            if (!dictFwd.ContainsKey(key))
            {
                return false;
            }

            if (!dictFwd[key].Remove(value))
            {
                return false;
            }

            if (value != null)
            {
                dictRevFlat[value].Remove(key);
            }
            else
            {
                keysWithNullValue.Remove(key);
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
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!dictFwd.ContainsKey(key))
            {
                return false;
            }

            var hashSet = dictFwd[key];
            bool removedAtLeastOne = false;

            foreach (TVal val in values)
            {
                if (dictFwd[key].Contains(val))
                {
                    removedAtLeastOne |= dictFwd[key].Remove(val);

                    if (val != null)
                    {
                        dictRevFlat[val].Remove(key);
                    }
                    else
                    {
                        keysWithNullValue.Remove(key);
                    }
                }
            }

            return removedAtLeastOne;
        }

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and its values
        /// removed; otherwise, false.</returns>
        public bool RemoveValuesAll(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            if (!dictFwd.ContainsKey(key))
            {
                return false;
            }

            var hashSetValues = dictFwd[key];

            foreach (TVal val in hashSetValues)
            {
                if (val != null)
                {
                    dictRevFlat[val].Remove(key);
                }
                else
                {
                    keysWithNullValue.Remove(key);
                }
            }

            hashSetValues.Clear();
            return true;
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

            if (values is ValueSet<TKey, TVal> hash
                && dictRev.TryGetValue(hash, out TKey key))
            {
                hash.Clear();
                Remove(key);
                AddNewKey(key);
                return true;
            }

            return false;
        }

        public void Replace(TKey key, IEnumerable<TVal> value)
        {
            RemoveValuesAll(key);
            AddValues(key, value);
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>. The internal key and value
        /// collections are not cleared before being removed from the
        /// <see cref="MultiBiMap{TKey, TVal}"/>.
        /// </summary>
        public void ShallowClear()
        {
            dictFwd.Clear();
            dictRev.Clear();
            dictRevFlat.Clear();
            keysWithNullValue.Clear();
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
            IEnumerable<TVal> values, out TKey key)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values is ValueSet<TKey, TVal> hash)
            {
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
        public bool TryGetValue(TKey key, out ICollection<TVal> value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Key cannot be null.");
            }

            bool success = dictFwd.TryGetValue(
                key, out ValueSet<TKey, TVal> hashSet);
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
            }
            else
            {
                keysWithNullValue.Add(key);
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
                    }
                    else
                    {
                        keysWithNullValue.Add(key);
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
            var hashSetValues = new ValueSet<TKey, TVal>(key, this);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
        }

        private void AddNewKey(TKey key, TVal val)
        {
            var hashSetValues = new ValueSet<TKey, TVal>(key, this, val);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            if (val != null)
            {
                AddFlatEntry(key, val);
            }
            else
            {
                keysWithNullValue.Add(key);
            }
        }

        private void AddNewKey(TKey key, IEnumerable<TVal> vals)
        {
            var hashSetValues = new ValueSet<TKey, TVal>(key, this, vals);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            foreach (TVal val in hashSetValues)
            {
                if (val != null)
                {
                    AddFlatEntry(key, val);
                }
                else
                {
                    keysWithNullValue.Add(key);
                }
            }
        }

        private void AddNewKey(TKey key, HashSet<TVal> vals)
        {
            var hashSetValues = new ValueSet<TKey, TVal>(key, this, vals);
            dictFwd.Add(key, hashSetValues);
            dictRev.Add(hashSetValues, key);
            foreach (TVal val in hashSetValues)
            {
                if (val != null)
                {
                    AddFlatEntry(key, val);
                }
                else
                {
                    keysWithNullValue.Add(key);
                }
            }
        }

        private void PopulateWithEmptyHashsets(IEnumerable<TKey> enumerable)
        {
            foreach (TKey key in enumerable)
            {
                var hashSet = new ValueSet<TKey, TVal>(key, this);
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

        IEqualityComparer<IReadOnlyCollection<TVal>>
            IReadOnlyBiMap<TKey, IReadOnlyCollection<TVal>>.ComparerValue
            => EqualityComparer<IReadOnlyCollection<TVal>>.Default;

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>
                    .IsReadOnly => false;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>
            .Keys => dictFwd.Keys;

        ICollection<TKey> IDictionary<TKey, ICollection<TVal>>
            .Keys => dictFwd.Keys;

        IEnumerable<IReadOnlyCollection<TVal>>
            IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>
            .Values => dictFwd.Values;

        IReadOnlyCollection<IReadOnlyCollection<TVal>>
            IMultiMap<TKey, TVal>
            .Values => dictFwd.Values;

        ICollection<ICollection<TVal>>
            IDictionary<TKey, ICollection<TVal>>
            .Values => dictFwd.Values.Cast<ICollection<TVal>>().ToList();

        #endregion Public Properties

        #region Public Indexers

        ICollection<TVal> IDictionary<TKey, ICollection<TVal>>.this[TKey key]
        {
            get => dictFwd[key];
            set => Replace(key, value);
        }

        IReadOnlyCollection<TVal>
            IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>
            .this[TKey key]
        {
            get => dictFwd[key];
        }

        #endregion Public Indexers

        #region Public Methods

        IReadOnlyCollection<TKey>
            IFixedKeysBiMap<TKey, IReadOnlyCollection<TVal>>
            .this[IReadOnlyCollection<TVal> val]
            => GetKeysWithEqualSet(val).ToList();

        void IDictionary<TKey, ICollection<TVal>>
                    .Add(TKey key, ICollection<TVal> value)
            => Add(key, value);

        void ICollection<KeyValuePair<TKey, ICollection<TVal>>>
            .Add(KeyValuePair<TKey, ICollection<TVal>> item)
            => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>
            .Contains(KeyValuePair<TKey, ICollection<TVal>> item)
            => ContainsKeyWithValues(item.Key, item.Value);

        bool IReadOnlyBiMap<TKey, IReadOnlyCollection<TVal>>
            .ContainsValue(IReadOnlyCollection<TVal> value)
            => GetKeysWithEqualSet(value).Any();

        bool IReadOnlyMultiMap<TKey, TVal>
            .ContainsValue(IEnumerable<TVal> value)
            => GetKeysWithEqualSet(value).Any();

        void ICollection<KeyValuePair<TKey, ICollection<TVal>>>
            .CopyTo(
            KeyValuePair<TKey, ICollection<TVal>>[] array,
            int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (dictFwd.Count + arrayIndex > array.Length)
            {
                throw new ArgumentException("The number of elements in the " +
                    "source is greater than the available space from " +
                    "arrayIndex to the end of the destination array.");
            }

            int i = arrayIndex;
            foreach (var kvp in dictFwd)
            {
                array[i]
                    = new KeyValuePair<TKey, ICollection<TVal>>(
                        kvp.Key,
                        kvp.Value);
                i++;
            }
        }

        IEnumerator<KeyValuePair<TKey, ICollection<TVal>>>
            IMultiMap<TKey, TVal>.GetEnumerator()
        {
            foreach (var kvp in dictFwd)
            {
                yield return new KeyValuePair<TKey, ICollection<TVal>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        IEnumerator<KeyValuePair<TKey, ICollection<TVal>>>
            IEnumerable<KeyValuePair<TKey, ICollection<TVal>>>.GetEnumerator()
        {
            foreach (var kvp in dictFwd)
            {
                yield return new KeyValuePair<TKey, ICollection<TVal>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TVal>>>
            IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TVal>>>
            .GetEnumerator()
        {
            foreach (var kvp in dictFwd)
            {
                yield return new KeyValuePair<TKey, IReadOnlyCollection<TVal>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => dictFwd.GetEnumerator();

        IReadOnlyCollection<TKey>
            IReadOnlyBiMap<TKey, IReadOnlyCollection<TVal>>
            .GetKeysWithValue(IReadOnlyCollection<TVal> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new List<TKey>(GetKeysWithEqualSet(value));
        }

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>
            .Remove(KeyValuePair<TKey, ICollection<TVal>> item)
            => ContainsKeyWithValues(item.Key, item.Value) && Remove(item.Key);

        void IFixedKeysBiMap<TKey, IReadOnlyCollection<TVal>>
            .Replace(TKey key, IReadOnlyCollection<TVal> value)
            => Replace(key, value);

        bool IDictionary<TKey, ICollection<TVal>>
                                                                                                            .TryGetValue(TKey key, out ICollection<TVal> value)
        {
            value = null;
            if (dictFwd.TryGetValue(key, out var hashSet))
            {
                value = hashSet;
                return true;
            }

            return false;
        }

        bool IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>
            .TryGetValue(TKey key, out IReadOnlyCollection<TVal> value)
        {
            var success = TryGetValue(key, out var value2);
            value = (IReadOnlyCollection<TVal>)value2;
            return success;
        }

        #endregion Public Methods
    }
}