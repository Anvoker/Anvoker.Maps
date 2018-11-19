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
    /// Represents a generic collection of key/value pairs where keys can also
    /// be retrieved by their associated value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the two way dictionary.
    /// </typeparam>
    /// <typeparam name="TVal">The type of the values in the two way dictionary.
    /// </typeparam>
    public partial class MultiMap<TKey, TVal> : IMultiMap<TKey, TVal>,
        IFixedKeysMultiMap<TKey, TVal>,
        IReadOnlyMultiMap<TKey, TVal>
    {
        #region Private Fields

        private Dictionary<TKey, HashSet<TVal>> multiDict;
        private IEqualityComparer<TVal> comparerValue;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiMap{TKey, TVal}"/>
        /// class that is empty, has the default initial capacity, and uses
        /// the default equality comparer for the key type.
        /// </summary>
        public MultiMap() : this(0, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiMap{TKey, TVal}"/>
        /// class that is empty, has the specified initial capacity, and uses
        /// the default equality comparer for the key types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="MultiMap{TKey, TVal}"/> can contain.</param>
        public MultiMap(int capacity) : this(capacity, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiMap{TKey, TVal}"/>
        /// class that is empty, has the default initial capacity, and uses
        /// the specified equality comparer for the key types.
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
        public MultiMap(
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
            : this(0, comparerKey, comparerValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiMap{TKey, TVal}"/>
        /// class that is empty, has the specified initial capacity, and uses
        /// the specified equality comparer for the key types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="MultiMap{TKey, TVal}"/> can contain.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public MultiMap(
            int capacity,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
        {
            multiDict = new Dictionary<TKey, HashSet<TVal>>(
                capacity, comparerKey);
            this.comparerValue = comparerValue;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMap{TKey, TVal}"/> class that contains elements
        /// copied from the specified
        /// <see cref="IDictionary{TKey, HashSet{TVal}}"/> and uses the
        /// specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="multiMap">The
        /// <see cref="IDictionary{TKey, HashSet{TVal}}"/> that is copied to the
        /// new <see cref="MultiMap{TKey, TVal}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public MultiMap(
            IDictionary<TKey, HashSet<TVal>> multiMap,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
        {
            multiDict = new Dictionary<TKey, HashSet<TVal>>(
                multiMap, comparerKey);
            this.comparerValue = comparerValue;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMap{TKey, TVal}"/> class that contains elements
        /// copied from the specified
        /// <see cref="IDictionary{TKey, HashSet{TVal}}"/> and uses the
        /// default equality comparers for the key and value types.
        /// </summary>
        /// <param name="multiMap">The
        /// <see cref="IDictionary{TKey, HashSet{TVal}}"/> that is copied to the
        /// new <see cref="MultiMap{TKey, TVal}"/>.</param>
        public MultiMap(IDictionary<TKey, HashSet<TVal>> multiMap)
            : this(multiMap, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMap{TKey, TVal}"/> class that contains elements with
        /// keys copied from the specified
        /// <see cref="IEnumerable{TKey}"/> and uses the
        /// default equality comparers for the key and value types. <para>The
        /// elements start out with empty value collections.</para>
        /// </summary>
        /// <param name="keys">The key collection that is copied to the new
        /// <see cref="MultiMap{TKey, TVal}"/>.</param>
        public MultiMap(IEnumerable<TKey> keys) : this(keys, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMap{TKey, TVal}"/> class that contains elements with
        /// keys copied from the specified
        /// <see cref="ICollection{TKey}"/> and uses the
        /// default equality comparers for the key and value types. <para>The
        /// elements start out with empty value collections.</para> <para>
        /// Compared to the <see cref="IEnumerable{T}"/> based constructor,
        /// this is slightly more performant due to being able to set the
        /// capacity of <see cref="MultiMap{TKey, TVal}"/> to the count of the
        /// specified key collection.</para>
        /// </summary>
        /// <param name="keys">The key collection that is copied to the new
        /// <see cref="MultiMap{TKey, TVal}"/>.</param>
        public MultiMap(ICollection<TKey> keys) : this(keys, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMap{TKey, TVal}"/> class that contains elements with
        /// keys copied from the specified
        /// <see cref="IEnumerable{TKey}"/> and uses the
        /// default equality comparers for the key and value types. <para>The
        /// elements start out with empty value collections.</para>
        /// </summary>
        /// <param name="keys">The key collection that is copied to the new
        /// <see cref="MultiMap{TKey, TVal}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public MultiMap(
            IEnumerable<TKey> keys,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue) :
            this(comparerKey, comparerValue)
        {
            foreach (TKey key in keys)
            {
                AddKey(key);
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMap{TKey, TVal}"/> class that contains elements with
        /// keys copied from the specified
        /// <see cref="ICollection{TKey}"/> and uses the
        /// default equality comparers for the key and value types. <para>The
        /// elements start out with empty value collections.</para> <para>
        /// Compared to the <see cref="IEnumerable{T}"/> based constructor,
        /// this is slightly more performant due to being able to set the
        /// capacity of <see cref="MultiMap{TKey, TVal}"/> to the count of the
        /// specified key collection.</para>
        /// </summary>
        /// <param name="keys">The key collection that is copied to the new
        /// <see cref="MultiMap{TKey, TVal}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public MultiMap(
            ICollection<TKey> keys,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue) :
            this(keys.Count, comparerKey, comparerValue)
        {
            foreach (TKey key in keys)
            {
                AddKey(key);
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        public IEqualityComparer<TKey> ComparerKey => multiDict.Comparer;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        public IEqualityComparer<TVal> ComparerValue => comparerValue;

        /// <summary>
        /// Gets the number of elements contained in the
        /// <see cref="MultiMap{TKey, TVal}"/>
        /// </summary>
        public int Count => multiDict.Count;

        /// <summary>
        /// Gets an enumeration of the <see cref="MultiMap{TKey, TVal}"/>'s
        /// keys.
        /// </summary>
        public ICollection<TKey> Keys => multiDict.Keys;

        /// <summary>
        /// Gets an enumeration of the <see cref="MultiMap{TKey, TVal}"/>'s
        /// values.
        /// </summary>
        public ICollection<ICollection<TVal>> Values
            => ((IDictionary<TKey, ICollection<TVal>>)multiDict).Values;

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        /// Gets the value collection associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value collection to get.</param>
        /// <returns>The value collection associated with the specified key.
        /// </returns>
        public ICollection<TVal> this[TKey key] => multiDict[key];

        #endregion Public Indexers

        #region Public Methods

        /// <summary>
        /// Adds the specified key to the <see cref="MultiMap{TKey, TVal}"/>
        /// with an empty value collection.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        public void AddKey(TKey key)
            => multiDict.Add(key, new HashSet<TVal>());

        /// <summary>
        /// Adds the specified key and its associated value to the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        public void AddKey(TKey key, TVal value)
        {
#pragma warning disable IDE0022 // Use expression body for methods
            multiDict.Add(key, new HashSet<TVal>() { value });
#pragma warning restore IDE0022 // Use expression body for methods
        }

        /// <summary>
        /// Adds the specified key and its associated values to the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add.</param>
        public void AddKey(TKey key, IEnumerable<TVal> values)
            => multiDict.Add(key, new HashSet<TVal>(values));

        /// <summary>
        /// Adds the specified key and its associated values to the
        /// <see cref="MultiMap{TKey, TVal}"/>. This doesn't copy the
        /// collection, it passes it directly.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add.</param>
        public void AddKey(TKey key, HashSet<TVal> values)
            => multiDict.Add(key, values);

        /// <summary>
        /// Adds the value to the specified key in the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The values to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the value didn't exist already; otherwise,
        /// false.</returns>
        public bool AddValue(TKey key, TVal value)
        {
            try
            {
                return multiDict[key].Add(value);
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
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to add to the element.</param>
        /// <returns>true if at least one value didn't exist already and was
        /// added; otherwise, false.</returns>
        public bool AddValues(TKey key, IEnumerable<TVal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            bool atLeastOneValueAdded = false;
            HashSet<TVal> hashSetValues = null;

            try
            {
                hashSetValues = multiDict[key];
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null.");
            }

            foreach (TVal value in values)
            {
                atLeastOneValueAdded |= hashSetValues.Add(value);
            }

            return atLeastOneValueAdded;
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="MultiMap{TKey, TVal}"/>. The internal key and value
        /// collections are not cleared before being removed from the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        public void Clear() => multiDict.Clear();

        /// <summary>
        /// Determines whether the <see cref="MultiMap{TKey, TVal}"/> contains
        /// a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="MultiMap{TKey, TVal}"/>.</param>
        /// <returns>true if the <see cref="MultiMap{TKey, TVal}"/> contains
        /// an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key) => multiDict.ContainsKey(key);

        /// <summary>
        /// Determines whether the <see cref="MultiMap{TKey, TVal}"/> contains
        /// a specific value at the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="MultiMap{TKey, TVal}"/>.</param>
        /// <param name="value">The value to locate in the
        /// <see cref="MultiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="MultiMap{TKey, TVal}"/> contains
        /// an element with the specified key and value; otherwise, false.
        /// </returns>
        public bool ContainsValue(TKey key, TVal value)
            => multiDict.ContainsKey(key) && multiDict[key].Contains(value);

        /// <summary>
        /// Determines whether the <see cref="MultiMap{TKey, TVal}"/> contains
        /// a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the
        /// <see cref="MultiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="MultiMap{TKey, TVal}"/> contains
        /// an element with the specified value; otherwise, false.</returns>
        public bool ContainsValue(TVal value)
        {
            foreach (HashSet<TVal> hashSetValues in multiDict.Values)
            {
                if (hashSetValues.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the key-values elements
        /// of the <see cref="MultiMap{TKey, TVal}"/>, with values associated
        /// with a key being grouped in their own collection in each entry.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, ICollection{TVal}}
        /// .Enumerator"/> structure for the
        /// <see cref="MultiMap{TKey, TVal}"/>.</returns>
        public IEnumerator<KeyValuePair<TKey, ICollection<TVal>>>
            GetEnumerator()
            => ((IDictionary<TKey, ICollection<TVal>>)multiDict)
            .GetEnumerator();

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false.</returns>
        public bool RemoveKey(TKey key) => multiDict.Remove(key);

        /// <summary>
        /// Removes the value associated with the specified key from the
        /// <see cref="MultiMap{TKey, TVal}"/>.
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
                if (multiDict.TryGetValue(key, out HashSet<TVal> values))
                {
                    return values.Remove(value);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }
        }

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="values">The values to remove from the element.</param>
        /// <returns>true if the element is successfully found and at least one
        /// value removed; otherwise, false.</returns>
        public bool RemoveValues(TKey key, IEnumerable<TVal> values)
        {
            try
            {
                bool atLeastOneValueRemoved = false;
                if (multiDict.TryGetValue(key, out HashSet<TVal> hashSetValues))
                {
                    foreach (TVal value in values)
                    {
                        atLeastOneValueRemoved |= hashSetValues.Add(value);
                    }

                    return atLeastOneValueRemoved;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }
        }

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public void RemoveValuesAll(TKey key)
        {
            try
            {
                if (multiDict.TryGetValue(key, out HashSet<TVal> hashSetValues))
                {
                    hashSetValues.Clear();
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }
        }

        /// <summary>
        /// Gets the value collection associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <param name="value">When this method returns, contains the value
        /// collection associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the value parameter.
        /// </param>
        /// <returns>true if the <see cref="MultiMap{TKey, TVal}"/> contains
        /// an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out ICollection<TVal> value)
        {
            value = null;
            bool success = false;

            try
            {
                if (success = multiDict.TryGetValue(key, out HashSet<TVal> hashSetValues))
                {
                    value = hashSetValues;
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }

            return success;
        }

        #endregion Public Methods
    }

    /// <content>
    /// Explicit interface implementations.
    /// </content>
    public partial class MultiMap<TKey, TVal>
    {
        #region Public Properties

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>.IsReadOnly
            => false;

        IEnumerable<TKey>
            IReadOnlyDictionary<TKey, ICollection<TVal>>.Keys => multiDict.Keys;

        IEnumerable<TKey>
            IReadOnlyDictionary<TKey, TVal>.Keys => multiDict.Keys;

        IEnumerable<ICollection<TVal>>
            IReadOnlyDictionary<TKey, ICollection<TVal>>.Values
            => multiDict.Values;

        IEnumerable<TVal> IReadOnlyDictionary<TKey, TVal>.Values
        {
            get
            {
                foreach (TKey key in multiDict.Keys)
                {
                    foreach (TVal value in multiDict[key])
                    {
                        yield return value;
                    }
                }
            }
        }

        #endregion Public Properties

        #region Public Indexers

        TVal IReadOnlyDictionary<TKey, TVal>.this[TKey key]
        {
            get
            {
                foreach (TVal value in multiDict[key])
                {
                    return value;
                }

                if (multiDict[key].Count <= 0)
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
                return multiDict[key];
            }

            set
            {
                multiDict[key] = new HashSet<TVal>(value);
            }
        }

        #endregion Public Indexers

        #region Public Methods

        void IDictionary<TKey, ICollection<TVal>>.Add(
            TKey key, ICollection<TVal> value)
            => multiDict.Add(key, new HashSet<TVal>(value));

        void ICollection<KeyValuePair<TKey, ICollection<TVal>>>.Add(
            KeyValuePair<TKey, ICollection<TVal>> item)
            => multiDict.Add(item.Key, new HashSet<TVal>(item.Value));

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>.Contains(
            KeyValuePair<TKey, ICollection<TVal>> item)
            => multiDict.ContainsKey(item.Key)
            && multiDict[item.Key].SetEquals(item.Value);

        void ICollection<KeyValuePair<TKey, ICollection<TVal>>>.CopyTo(
            KeyValuePair<TKey, ICollection<TVal>>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<TKey, ICollection<TVal>>>)multiDict)
            .CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => multiDict.GetEnumerator();

        IEnumerator<KeyValuePair<TKey, TVal>>
            IEnumerable<KeyValuePair<TKey, TVal>>.GetEnumerator()
        {
            foreach (TKey key in multiDict.Keys)
            {
                foreach (TVal value in multiDict[key])
                {
                    yield return new KeyValuePair<TKey, TVal>(key, value);
                }
            }
        }

        bool IDictionary<TKey, ICollection<TVal>>.Remove(TKey key)
            => multiDict.Remove(key);

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>.Remove(
            KeyValuePair<TKey, ICollection<TVal>> item)
            => ((IDictionary<TKey, ICollection<TVal>>)multiDict).Remove(item);

        bool IReadOnlyDictionary<TKey, TVal>.TryGetValue(
            TKey key,
            out TVal value)
        {
            HashSet<TVal> hashSet;
            bool success = false;

            try
            {
                success = multiDict.TryGetValue(key, out hashSet);
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

        #endregion Public Methods
    }
}