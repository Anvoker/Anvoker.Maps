using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable RCS1169 // Mark field as read-only.

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a HashSet-based generic collection of key-values mappings.
    /// Values must be unique per key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the two way dictionary.
    /// </typeparam>
    /// <typeparam name="TVal">The type of the values in the two way dictionary.
    /// </typeparam>
    public partial class MultiMap<TKey, TVal> : IMultiMap<TKey, TVal>
    {
        #region Private Fields

        private IEqualityComparer<TVal> comparerValue;
        private Dictionary<TKey, ValueSet<TKey, TVal>> multiDict;

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
            multiDict = new Dictionary<TKey, ValueSet<TKey, TVal>>(
                capacity, comparerKey);
            this.comparerValue = comparerValue
                ?? EqualityComparer<TVal>.Default;
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
            MultiMap<TKey, TVal> multiMap,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
        {
            multiDict = new Dictionary<TKey, ValueSet<TKey, TVal>>(
                multiMap.multiDict, comparerKey);
            this.comparerValue = comparerValue
                ?? EqualityComparer<TVal>.Default;
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
        /// this has slightly better performance due to being able to set the
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
        /// this has slightly better performance due to being able to set the
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
        public IReadOnlyCollection<TKey> Keys => multiDict.Keys;

        /// <summary>
        /// Gets an enumeration of the <see cref="MultiMap{TKey, TVal}"/>'s
        /// values.
        /// </summary>
        public IReadOnlyCollection<ISet<TVal>> Values => multiDict.Values;

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        /// Gets the value collection associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value collection to get.</param>
        /// <returns>The value collection associated with the specified key.
        /// </returns>
        public IEnumerable<TVal> this[TKey key]
        {
            get => multiDict[key];
            set
            {
                RemoveValuesAll(key);
                AddValues(key, value);
            }
        }

        #endregion Public Indexers

        #region Public Methods

        /// <summary>
        /// If the specified key already exists in the
        /// <see cref="MultiMap{TKey, TVal}"/>, adds the specified value to that
        /// key; otherwise it adds a new element with the specified key and
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
        /// <see cref="MultiMap{TKey, TVal}"/>, adds the specified values to
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
        /// <see cref="MultiMap{TKey, TVal}"/>, adds the specified values to
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
        /// Adds the specified key to the <see cref="MultiMap{TKey, TVal}"/>
        /// with an empty value collection.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        public void AddKey(TKey key)
            => multiDict.Add(key,
                new ValueSet<TKey, TVal>(key, this));

        /// <summary>
        /// Adds the specified key and its associated value to the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can
        /// be null for reference types.</param>
        public void AddKey(TKey key, TVal value)
            => multiDict.Add(key,
                new ValueSet<TKey, TVal>(key, this, value));

        /// <summary>
        /// Adds the specified key and its associated values to the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add.</param>
        public void AddKey(TKey key, IEnumerable<TVal> values)
            => multiDict.Add(key,
                new ValueSet<TKey, TVal>(key, this, values));

        /// <summary>
        /// Adds the specified key and its associated values to the
        /// <see cref="MultiMap{TKey, TVal}"/>. This doesn't copy the
        /// collection, it passes it by reference. <para>Will throw
        /// <see cref="ArgumentException"/> if the passed
        /// <see cref="HashSet{TVal}"/>'s comparer isn't reference equal to
        /// <see cref="ComparerValue"/>.</para>
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="values">The values of the element to add.</param>
        public void AddKey(TKey key, HashSet<TVal> values)
        {
            if (values.Comparer.Equals(ComparerValue))
            {
                throw new ArgumentException(
                    $@"The Comparer of the passed HashSet argument has to be
                        equal to the {nameof(ComparerValue)} of the
                        {nameof(MultiMap<TKey, TVal>)}.",
                    nameof(values));
            }

            multiDict.Add(key,
                new ValueSet<TKey, TVal>(key, this, values));
        }

        /// <summary>
        /// Adds the value to an existing specified key in the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the key was found and the value didn't exist
        /// already; otherwise, false.</returns>
        public bool AddValue(TKey key, TVal value)
        {
            if (key == null)
            {
                throw new
                    ArgumentNullException(nameof(key), "Keys cannot be null.");
            }

            return ContainsKey(key) && multiDict[key].HashSet.Add(value);
        }

        /// <summary>
        /// Adds the values to an existing specified key in the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="values">The values to add to the element. The value can
        /// be null for reference types.</param>
        /// <returns>true if the key was found and at least one value didn't
        /// exist already; otherwise, false.</returns>
        public bool AddValues(TKey key, IEnumerable<TVal> values)
        {
            if (key == null)
            {
                throw new
                    ArgumentNullException(nameof(key), "Keys cannot be null.");
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            bool atLeastOneValueAdded = false;
            HashSet<TVal> hashSetValues = multiDict[key].HashSet;
            foreach (TVal value in values)
            {
                atLeastOneValueAdded |= hashSetValues.Add(value);
            }

            return atLeastOneValueAdded;
        }

        public void Clear()
        {
            foreach (var values in multiDict.Values)
            {
                values.Clear();
            }

            ShallowClear();
        }

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
        public bool ContainsKeyWithValue(TKey key, TVal value)
            => multiDict.ContainsKey(key) && multiDict[key].Contains(value);

        /// <summary>
        /// Determines whether the <see cref="MultiMap{TKey, TVal}"/> contains
        /// all of the specified values at the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="MultiMap{TKey, TVal}"/>.</param>
        /// <param name="values">The values to locate in the
        /// <see cref="MultiMap{TKey, TVal}"/>. The value can be null for
        /// reference types.</param>
        /// <returns>true if the <see cref="MultiMap{TKey, TVal}"/> contains
        /// an element with the specified key and values; otherwise, false.
        /// </returns>
        public bool ContainsKeyWithValues(TKey key, IEnumerable<TVal> values)
            => multiDict.ContainsKey(key) && multiDict[key].SetEquals(values);

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
            foreach (var hashSetValues in multiDict.Values)
            {
                if (hashSetValues.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsValue(IEnumerable<TVal> values)
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

        public IEnumerator<KeyValuePair<TKey, ISet<TVal>>> GetEnumerator()
        {
            foreach (var kvp in multiDict)
            {
                yield return new KeyValuePair<TKey, ISet<TVal>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed;
        /// otherwise, false.</returns>
        public bool Remove(TKey key)
            => multiDict.ContainsKey(key) && multiDict.Remove(key);

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
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }

            if (multiDict.TryGetValue(key, out var values))
            {
                return values.Remove(value);
            }
            else
            {
                return false;
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
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }

            bool atLeastOneValueRemoved = false;
            if (multiDict.TryGetValue(key, out var hashSetValues))
            {
                foreach (TVal value in values)
                {
                    atLeastOneValueRemoved |= hashSetValues.Remove(value);
                }

                return atLeastOneValueRemoved;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes all values associated with the specified key from the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and its values
        /// removed; otherwise, false.</returns>
        public bool RemoveValuesAll(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }

            if (multiDict.TryGetValue(key, out var hashSetValues))
            {
                hashSetValues.Clear();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Replace(TKey key, IEnumerable<TVal> values)
        {
            RemoveValuesAll(key);
            AddValues(key, values);
        }

        /// <summary>
        /// Removes all keys and values from the
        /// <see cref="MultiMap{TKey, TVal}"/>. The internal key and value
        /// collections are not cleared before being removed from the
        /// <see cref="MultiMap{TKey, TVal}"/>.
        /// </summary>
        public void ShallowClear() => multiDict.Clear();

        /// <summary>
        /// Gets the value collection associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <param name="values">When this method returns, contains the value
        /// collection associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the value parameter.
        /// </param>
        /// <returns>true if the <see cref="MultiMap{TKey, TVal}"/> contains
        /// an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out ICollection<TVal> values)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }

            values = null;
            bool success = false;
            if (success = multiDict.TryGetValue(
                key, out var hashSetValues))
            {
                values = hashSetValues;
            }

            return success;
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
        public bool TryGetValue(TKey key, out ISet<TVal> value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key), "Keys cannot be null");
            }

            value = null;
            bool success = false;
            if (success = multiDict.TryGetValue(
                key, out var hashSetValues))
            {
                value = hashSetValues;
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

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>
            .IsReadOnly => false;

        ICollection<TKey>
            IDictionary<TKey, ICollection<TVal>>
            .Keys => multiDict.Keys;

        IEnumerable<TKey>
            IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>
            .Keys => Keys;

        ICollection<ICollection<TVal>> IDictionary<TKey, ICollection<TVal>>
            .Values => multiDict.Values.Cast<ICollection<TVal>>().ToList();

        IReadOnlyCollection<IReadOnlyCollection<TVal>> IMultiMap<TKey, TVal>
            .Values => multiDict.Values;

        IEnumerable<IReadOnlyCollection<TVal>>
            IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>
            .Values => multiDict.Values;

        #endregion Public Properties

        #region Public Indexers

        ICollection<TVal>
            IDictionary<TKey, ICollection<TVal>>.this[TKey key]
        {
            get => multiDict[key];
            set
            {
                RemoveValuesAll(key);
                AddValues(key, value);
            }
        }

        IReadOnlyCollection<TVal>
            IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>.this[TKey key]
        {
            get => multiDict[key];
        }

        #endregion Public Indexers

        #region Public Methods

        void IDictionary<TKey, ICollection<TVal>>
            .Add(TKey key, ICollection<TVal> value)
            => Add(key, value);

        void ICollection<KeyValuePair<TKey, ICollection<TVal>>>
            .Add(KeyValuePair<TKey, ICollection<TVal>> item)
            => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>
            .Contains(KeyValuePair<TKey, ICollection<TVal>> item)
            => multiDict.ContainsKey(item.Key)
            && multiDict[item.Key].SetEquals(item.Value);

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

            if (multiDict.Count + arrayIndex > array.Length)
            {
                throw new ArgumentException("The number of elements in the " +
                    "source is greater than the available space from " +
                    "arrayIndex to the end of the destination array.");
            }

            int i = arrayIndex;
            foreach (var kvp in multiDict)
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
            foreach (var kvp in multiDict)
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
            foreach (var kvp in multiDict)
            {
                yield return new KeyValuePair<TKey, IReadOnlyCollection<TVal>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        IEnumerator<KeyValuePair<TKey, ICollection<TVal>>>
            IEnumerable<KeyValuePair<TKey, ICollection<TVal>>>.GetEnumerator()
        {
            foreach (var kvp in multiDict)
            {
                yield return new KeyValuePair<TKey, ICollection<TVal>>(
                    kvp.Key,
                    kvp.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => multiDict.GetEnumerator();

        bool ICollection<KeyValuePair<TKey, ICollection<TVal>>>.Remove(
                    KeyValuePair<TKey, ICollection<TVal>> item)
            => multiDict.ContainsKey(item.Key)
            && multiDict[item.Key].SetEquals(item.Value)
            && multiDict.Remove(item.Key);

        bool IReadOnlyDictionary<TKey, IReadOnlyCollection<TVal>>
            .TryGetValue(TKey key, out IReadOnlyCollection<TVal> value)
        {
            var success = multiDict.TryGetValue(key, out var value2);
            value = value2;
            return success;
        }

        #endregion Public Methods
    }
}