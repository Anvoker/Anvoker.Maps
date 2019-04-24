using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Anvoker.Maps.Interfaces;

#pragma warning disable RCS1104 // Simplify conditional expression

namespace Anvoker.Maps
{
    /// <summary>
    /// Represents a generic collection of keys and values where each key may be
    /// associated with multiple values. The backing store consists of
    /// dictionaries with the values residing in a hashset.
    /// </summary>
    /// <typeparam name="TK">The type of the keys
    /// <see cref="CompositeMultiMap{TK, TV}"/>.</typeparam>
    /// <typeparam name="TV">The type of the values
    /// <see cref="CompositeMultiMap{TK, TV}"/>.</typeparam>
    public class CompositeMultiMap<TK, TV> :
        IMultiMap<TK, TV>,
        IDictionary<TK, ISet<TV>>
    {
        #region Private Fields

        private readonly IEqualityComparer<TV> comparerValue;
        private readonly Dictionary<TK, ValueSet<TK, TV>> multiDict;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMultiMap{TK, TV}"/>
        /// class that is empty, has the default initial capacity, and uses
        /// the default equality comparer for the key type.
        /// </summary>
        public CompositeMultiMap() : this(0, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMultiMap{TK, TV}"/>
        /// class that is empty, has the specified initial capacity, and uses
        /// the default equality comparer for the key types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="CompositeMultiMap{TK, TV}"/> can contain.</param>
        public CompositeMultiMap(int capacity) : this(capacity, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMultiMap{TK, TV}"/>
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
        public CompositeMultiMap(
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
            : this(0, comparerKey, comparerValue)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMultiMap{TK, TV}"/>
        /// class that is empty, has the specified initial capacity, and uses
        /// the specified equality comparer for the key types.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="CompositeMultiMap{TK, TV}"/> can contain.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiMap(
            int capacity,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
        {
            multiDict = new Dictionary<TK, ValueSet<TK, TV>>(capacity, comparerKey);
            this.comparerValue = comparerValue ?? EqualityComparer<TV>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiMap{TK, TV}"/> class that contains elements
        /// copied from the specified
        /// <see cref="IDictionary{TKey, HashSet{TV}}"/> and uses the
        /// specified equality comparers for the key and value types.
        /// </summary>
        /// <param name="multiMap">The
        /// <see cref="IDictionary{TKey, HashSet{TV}}"/> that is copied to the
        /// new <see cref="CompositeMultiMap{TK, TV}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiMap(
            CompositeMultiMap<TK, TV> multiMap,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
        {
            multiDict = new Dictionary<TK, ValueSet<TK, TV>>(
                multiMap.multiDict, comparerKey);
            this.comparerValue = comparerValue ?? EqualityComparer<TV>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiMap{TK, TV}"/> class that contains elements with
        /// keys copied from the specified
        /// <see cref="IEnumerable{TKey}"/> and uses the
        /// default equality comparers for the key and value types. <para>The
        /// elements start out with empty value collections.</para>
        /// </summary>
        /// <param name="keys">The key collection that is copied to the new
        /// <see cref="CompositeMultiMap{TK, TV}"/>.</param>
        public CompositeMultiMap(IEnumerable<TK> keys) : this(keys, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiMap{TK, TV}"/> class that contains elements with
        /// keys copied from the specified
        /// <see cref="ICollection{TKey}"/> and uses the
        /// default equality comparers for the key and value types. <para>The
        /// elements start out with empty value collections.</para> <para>
        /// Compared to the <see cref="IEnumerable{T}"/> based constructor,
        /// this has slightly better performance due to being able to set the
        /// capacity of <see cref="CompositeMultiMap{TK, TV}"/> to the count of the
        /// specified key collection.</para>
        /// </summary>
        /// <param name="keys">The key collection that is copied to the new
        /// <see cref="CompositeMultiMap{TK, TV}"/>.</param>
        public CompositeMultiMap(ICollection<TK> keys) : this(keys, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiMap{TK, TV}"/> class that contains elements with
        /// keys copied from the specified
        /// <see cref="IEnumerable{TKey}"/> and uses the
        /// default equality comparers for the key and value types. <para>The
        /// elements start out with empty value collections.</para>
        /// </summary>
        /// <param name="keys">The key collection that is copied to the new
        /// <see cref="CompositeMultiMap{TK, TV}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiMap(
            IEnumerable<TK> keys,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue) :
            this(comparerKey, comparerValue)
        {
            foreach (TK key in keys)
            {
                multiDict.Add(key, new ValueSet<TK, TV>(key, this));
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeMultiMap{TK, TV}"/> class that contains elements with
        /// keys copied from the specified
        /// <see cref="ICollection{TKey}"/> and uses the
        /// default equality comparers for the key and value types. <para>The
        /// elements start out with empty value collections.</para> <para>
        /// Compared to the <see cref="IEnumerable{T}"/> based constructor,
        /// this has slightly better performance due to being able to set the
        /// capacity of <see cref="CompositeMultiMap{TK, TV}"/> to the count of the
        /// specified key collection.</para>
        /// </summary>
        /// <param name="keys">The key collection that is copied to the new
        /// <see cref="CompositeMultiMap{TK, TV}"/>.</param>
        /// <param name="comparerKey">The <see cref="IEqualityComparer{TKey}"/>
        /// implementation to use when comparing keys, or null to use the
        /// default <see cref="EqualityComparer{TKey}"/> for the type of the
        /// key.</param>
        /// <param name="comparerValue">The
        /// <see cref="IEqualityComparer{TVal}"/> implementation to use when
        /// comparing values, or null to use the
        /// default <see cref="EqualityComparer{TVal}"/> for the type of the
        /// values.</param>
        public CompositeMultiMap(
            ICollection<TK> keys,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue) :
            this(keys.Count, comparerKey, comparerValue)
        {
            foreach (TK key in keys)
            {
                multiDict.Add(key, new ValueSet<TK, TV>(key, this));
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of keys for the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        public IEqualityComparer<TK> ComparerKey => multiDict.Comparer;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to
        /// determine equality of values for the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        public IEqualityComparer<TV> ComparerValue => comparerValue;

        /// <summary>
        /// Gets the number of elements contained in the
        /// <see cref="CompositeMultiMap{TK, TV}"/>
        /// </summary>
        public int Count => multiDict.Count;

        /// <summary>
        /// Gets a collection containing the keys in the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        public IReadOnlyCollection<TK> Keys => multiDict.Keys;

        /// <summary>
        /// Gets a collection containing the keys in the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        public IReadOnlyCollection<ValueSet<TK, TV>> Values => multiDict.Values;

        bool ICollection<KeyValuePair<TK, ISet<TV>>>.IsReadOnly => false;

        IEnumerable<TK> IReadOnlyDictionary<TK, IReadOnlyCollection<TV>>.Keys
            => multiDict.Keys;

        ICollection<TK> IDictionary<TK, ISet<TV>>.Keys
            => multiDict.Keys;

        IEnumerable<IReadOnlyCollection<TV>> IReadOnlyDictionary<TK, IReadOnlyCollection<TV>>
            .Values => multiDict.Values;

        ICollection<ISet<TV>> IDictionary<TK, ISet<TV>>.Values
            => multiDict.Values.ToList<ISet<TV>>();

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
        public ValueSet<TK, TV> this[TK key] => multiDict[key];

        IReadOnlyCollection<TV>
            IReadOnlyDictionary<TK, IReadOnlyCollection<TV>>.this[TK key]
            => multiDict[key];

        ISet<TV> IDictionary<TK, ISet<TV>>.this[TK key]
        {
            get => multiDict[key];
            set => multiDict[key] = new ValueSet<TK, TV>(key, this, value);
        }

        #endregion Public Indexers

        #region Public Methods

        /// <summary>
        /// Adds the specified key to the <see cref="CompositeMultiMap{TK, TV}"/>
        /// with no associated values.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        public void Add(TK key)
            => multiDict.Add(key, new ValueSet<TK, TV>(key, this));

        /// <summary>
        /// Adds the specified key and value to the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        public void Add(TK key, TV value)
            => multiDict.Add(key, new ValueSet<TK, TV>(key, this, value));

        /// <summary>
        /// Adds the specified key and values to the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add.</param>
        public void Add(TK key, IEnumerable<TV> values)
            => multiDict.Add(key, new ValueSet<TK, TV>(key, this, values));

        /// <summary>
        /// Adds the specified key and values to the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The values of the element to add.</param>
        public void Add(TK key, ISet<TV> value)
            => multiDict.Add(key, new ValueSet<TK, TV>(key, this, value));

        /// <summary>
        /// Adds the value to the specified key in the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the value didn't exist already; otherwise,
        /// false.</returns>
        public bool AddValue(TK key, TV value)
            => ContainsKey(key) && multiDict[key].Internal.Add(value);

        /// <summary>
        /// Adds the values to the specified key in the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
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

            bool atLeastOneValueAdded = false;

            if (multiDict.TryGetValue(key, out var hashSetValues))
            {
                foreach (TV value in values)
                {
                    atLeastOneValueAdded |= hashSetValues.Internal.Add(value);
                }
            }

            return atLeastOneValueAdded;
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="CompositeMultiMap{TK, TV}"/>. The internal key
        /// collections and value sets are cleared before being removed from the
        /// <see cref="CompositeMultiMap{TK, TV}"/> which will reflect in
        /// any variable referencing them.
        /// </summary>
        public void Clear()
        {
            foreach (var values in multiDict.Values)
            {
                values.Internal.Clear();
            }

            multiDict.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="CompositeMultiMap{TK, TV}"/>
        /// contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.</param>
        /// <returns>true if the <see cref="CompositeMultiMap{TK, TV}"/> contains an
        /// element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TK key) => multiDict.ContainsKey(key);

        /// <summary>
        /// Determines whether the <see cref="CompositeMultiMap{TK, TV}"/>
        /// contains at least one key associated with the specified value.
        /// </summary>
        /// <param name="value">The value to locate. The value can be null
        /// for reference types.</param>
        /// <returns>true if the <see cref="CompositeMultiMap{TK, TV}"/>
        /// contains an element with the specified value; otherwise, false.
        /// </returns>
        public bool ContainsValue(TV value)
        {
            foreach (var hashSetValues in multiDict.Values)
            {
                if (hashSetValues.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the <see cref="CompositeMultiMap{TK, TV}"/>
        /// contains at least one key associated with all the specified values.
        /// </summary>
        /// <param name="values">The values to locate.</param>
        /// <returns>true if the <see cref="CompositeMultiMap{TK, TV}"/>
        /// contains an element with the specified values; otherwise, false.
        /// </returns>
        public bool ContainsValues(IEnumerable<TV> values)
        {
            foreach (var hashSetValues in multiDict.Values)
            {
                if (hashSetValues.SetEquals(values))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the key-value pairs of
        /// the <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TVal}.Enumerator"/> structure
        /// for the <see cref="CompositeMultiMap{TK, TV}"/>.</returns>
        public IEnumerator<KeyValuePair<TK, ValueSet<TK, TV>>> GetEnumerator()
            => multiDict.GetEnumerator();

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false. This method returns false if key is not found in
        /// the <see cref="CompositeMultiMap{TK, TV}"/>.</returns>
        public bool Remove(TK key)
            => multiDict.ContainsKey(key) && multiDict.Remove(key);

        /// <summary>
        /// Removes the value associated with the specified key from the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to remove from the element. Values
        /// can be null for reference types.</param>
        /// <returns>true if the element is successfully found and the value
        /// removed; otherwise, false.</returns>
        public bool RemoveValue(TK key, TV value)
            => multiDict.TryGetValue(key, out var values) && values.Internal.Remove(value);

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="CompositeMultiMap{TK, TV}"/> that are found in common
        /// with the specified enumeration of values.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to remove from the element. Values
        /// can be null for reference types.</param>
        /// <returns>true if the element is successfully found and at least one
        /// value removed; otherwise, false.</returns>
        public bool RemoveValues(TK key, IEnumerable<TV> values)
        {
            bool atLeastOneValueRemoved = false;
            if (multiDict.TryGetValue(key, out var hashSetValues))
            {
                foreach (TV value in values)
                {
                    atLeastOneValueRemoved |= hashSetValues.Internal.Remove(value);
                }
            }

            return atLeastOneValueRemoved;
        }

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="CompositeMultiMap{TK, TV}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <returns>true if the element is successfully found and its values
        /// removed; otherwise, false.</returns>
        public bool RemoveValuesAll(TK key)
        {
            if (multiDict.TryGetValue(key, out var hashSetValues))
            {
                hashSetValues.Internal.Clear();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Replaces the values currently associated with the specified key with
        /// a new collection of values.
        /// </summary>
        /// <param name="key">The key of the value to replace.</param>
        /// <param name="values">The new values.</param>
        public void Replace(TK key, IEnumerable<TV> values)
        {
            RemoveValuesAll(key);
            AddValues(key, values);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <param name="value">When this method returns, contains the values
        /// associated with the specified key, if the key is found; otherwise,
        /// null.</param>
        /// <returns>true if the <see cref="CompositeMultiMap{TK, TV}"/> contains an
        /// element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TK key, out ISet<TV> value)
        {
            value = null;
            bool success = multiDict.TryGetValue(key, out var valueSet);
            value = valueSet;
            return success;
        }

        void ICollection<KeyValuePair<TK, ISet<TV>>>.Add(
            KeyValuePair<TK, ISet<TV>> item)
            => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TK, ISet<TV>>>.Contains(
            KeyValuePair<TK, ISet<TV>> item)
            => multiDict.TryGetValue(item.Key, out var values)
            && values.SetEquals(item.Value);

        void ICollection<KeyValuePair<TK, ISet<TV>>>.CopyTo(
            KeyValuePair<TK, ISet<TV>>[] array, int arrayIndex)
        {
            foreach (var kvp in multiDict)
            {
                array[arrayIndex++]
                    = new KeyValuePair<TK, ISet<TV>>(kvp.Key, kvp.Value);
            }
        }

        IEnumerator<KeyValuePair<TK, IReadOnlyCollection<TV>>>
            IEnumerable<KeyValuePair<TK, IReadOnlyCollection<TV>>>.GetEnumerator()
        {
            foreach (var kvp in multiDict)
            {
                yield return new KeyValuePair<TK, IReadOnlyCollection<TV>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        IEnumerator<KeyValuePair<TK, ISet<TV>>>
            IEnumerable<KeyValuePair<TK, ISet<TV>>>.GetEnumerator()
        {
            foreach (var kvp in multiDict)
            {
                yield return new KeyValuePair<TK, ISet<TV>>(kvp.Key, kvp.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => multiDict.GetEnumerator();

        bool ICollection<KeyValuePair<TK, ISet<TV>>>.Remove(
            KeyValuePair<TK, ISet<TV>> item)
            => TryGetValue(item.Key, out var values)
            && item.Value.SetEquals(values)
            && Remove(item.Key);

        bool IReadOnlyDictionary<TK, IReadOnlyCollection<TV>>.TryGetValue(
                    TK key,
            out IReadOnlyCollection<TV> values)
        {
            bool success = multiDict.TryGetValue(key, out var valueSets);
            values = valueSets;
            return success;
        }

        #endregion Public Methods
    }
}