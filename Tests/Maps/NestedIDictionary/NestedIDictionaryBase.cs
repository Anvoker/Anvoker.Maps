using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

#pragma warning disable IDE0034 // Simplify 'default' expression

namespace Anvoker.Tests.Collections.NestedIDictionary
{
    /// <summary>
    /// Provides functionality for testing whether a class correctly implements
    /// a dictionary interface with a nested collection as the value type.
    /// <para>
    /// The specific interface is <see cref="IDictionary{TKey, TValue}"/>
    /// where <typeparamref name="TKey"/> is the type of the dictionary's keys
    /// and <typeparamref name="TValCol"/> is the type of dictionary's values.
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">Type of the keys in
    /// <see cref="IDictionary{TKey, TValue}"/></typeparam>
    /// <typeparam name="TVal">
    /// Type of the values used in <see cref="TValCol"/>.</typeparam>
    /// <typeparam name="TIDict">Type of the class implementing
    /// <see cref="IDictionary{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TValCol">Type of the nested collection used as the
    /// value type in <see cref="IDictionary{TKey, TValue}"/>.</typeparam>
    [TestFixtureSource(
        typeof(SourceMap),
        nameof(SourceMap.TestFixtureSources))]
    public class NestedIDictionaryBase<TKey, TVal, TIDict, TValCol>
        where TIDict : IDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly Args d;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
        /// class with the specified collection that implements
        /// <typeparamref name="TIDict"/> with the specified keys and
        /// values, and with matching test data.
        /// </summary>
        /// <param name="args">A data class containing all of the necessary
        /// arguments for initializing the tests.</param>
        public NestedIDictionaryBase(Args args)
        {
            d = args;
        }

        /// <summary>
        /// Constructs the parameter class required for instantiating this
        /// test fixture for a particular collection type that implements
        /// <see cref="IDictionary{TKey, TValue}"/> with a nested collection
        /// as the value.
        /// </summary>
        /// <param name="ctorImplementor">Delegate pointing to a
        /// parameterless constructor of <typeparamref name="TIDict"/>.</param>
        /// <param name="ctorTValCol">Delegate pointing to a constructor that
        /// takes <see cref="IEnumerable{T}"/> and returns a new value
        /// collection <typeparamref name="TValCol"/></param>
        /// <param name="data">The concrete test data.</param>
        /// <returns>A new instance of <see cref="TestFixtureParameters"/> that
        /// can be used to instantiate a
        /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
        /// fixture.</returns>
        public static TestFixtureParameters
            ConstructFixtureParams(
            Func<TIDict> ctorImplementor,
            Func<IEnumerable<TVal>, TValCol> ctorTValCol,
            MapTestData<TKey, TVal> data)
        {
            var args = new Args(
                ctorImplementor, ctorTValCol, data);
            var exposedParams = new ExposedTestFixtureParams()
            {
                Arguments = new object[] { args },
                Properties = new PropertyBag(),
                RunState = RunState.Runnable,
                TypeArgs = new Type[]
                {
                    typeof(TKey),
                    typeof(TVal),
                    typeof(TIDict),
                    typeof(TValCol)
                }
            };

            return new TestFixtureParameters(exposedParams);
        }

        #region Tests

        /// <summary>
        /// Checks that the collection contains the specified keys and values
        /// after they've been added.
        /// </summary>
        [Test]
        public void Add()
        {
            var collection = d.ImplementorCtor();

            for (int i = 0; i < d.KeysToAdd.Length; i++)
            {
                collection.Add(d.KeysToAdd[i], d.ValuesToAdd[i]);
            }

            AllContainsReturnTrue(collection, d.KeysToAdd);
            AllContainsReturnTrue(collection, d.KeysToAdd, d.ValuesToAdd);
        }

        /// <summary>
        /// Checks that adding a key value pair with a null key triggers an
        /// <see cref="ArgumentNullException"/> throw.
        /// </summary>
        [Test]
        public void AddingKVPWithNullKeyThrows()
        {
            if (default(TKey) != null)
            {
                Assert.Pass("The key is not of a nullable type, so " +
                    "this test is not applicable.");
            }

            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                default(TKey), d.ValuesToAdd[0]);

            Assert.Throws(
                typeof(ArgumentNullException),
                () => collection.Add(kvp));
        }

        /// <summary>
        /// Checks that adding a null key triggers an
        /// <see cref="ArgumentNullException"/> throw.
        /// </summary>
        [Test]
        public void AddingNullKeyThrows()
        {
            if (default(TKey) != null)
            {
                Assert.Pass("The key is not of a nullable type, so " +
                    "this test is not applicable.");
            }

            var collection = d.ImplementorCtor();

            Assert.Throws(
                typeof(ArgumentNullException),
                () => collection.Add(default(TKey), d.ValuesToAdd[0]));
        }

        /// <summary>
        /// Checks that the collection contains the specified key value pairs
        /// after they've been added.
        /// </summary>
        [Test]
        public void AddKVP()
        {
            var collection = d.ImplementorCtor();
            var kvps = new KeyValuePair<TKey, TValCol>[d.KeysToAdd.Length];

            for (int i = 0; i < d.KeysToAdd.Length; i++)
            {
                kvps[i] = new KeyValuePair<TKey, TValCol>(
                    d.KeysToAdd[i], d.ValuesToAdd[i]);
                collection.Add(kvps[i]);
            }

            AllContainsReturnTrue(collection, d.KeysToAdd);
            AllContainsReturnTrue(collection, d.KeysToAdd, d.ValuesToAdd);
        }

        /// <summary>
        /// Checks that adding a key-value pair with an existing key causes the
        /// specified value to be associated to that key in the implementor.
        /// </summary>
        [Test]
        public void AddKVPExistingKey()
        {
            var collection = d.ImplementorCtor();
            int initialCount = collection.Count;

            var expectedValues = new TValCol[d.KeysInitial.Length];

            int length = Math.Min(d.KeysInitial.Length, d.ValuesToAdd.Length);
            for (int i = 0; i < length; i++)
            {
                collection.Add(d.KeysInitial[i], d.ValuesToAdd[i]);
            }

            TValCol[] newValues = HelperMethods.UnionValues(
                d.ValuesInitial,
                d.ValuesToAdd,
                d.ComparerValue,
                d.ValueCollectionCtor);

            AllContainsReturnTrue(collection, d.KeysInitial, newValues);

            const string Msg = "The count of the collection should not be " +
                "different after adding existing keys.";

            Assert.AreEqual(initialCount, collection.Count, Msg);
        }

        /// <summary>
        /// Checks that adding a key-value pair with an existing key and a
        /// default value causes the specified value to be associated to that
        /// key in the implementor.
        /// </summary>
        [Test]
        public void AddKVPExistingKeyDefaultValue()
        {
            var collection = d.ImplementorCtor();
            int initialCount = collection.Count;

            var expectedValues = new TValCol[d.KeysInitial.Length];
            var valuesToAdd = new TValCol[d.KeysInitial.Length];

            for (int i = 0; i < d.KeysInitial.Length; i++)
            {
                valuesToAdd[i] = d.ValueCollectionCtor(
                    new TVal[] { default(TVal) });
                collection.Add(d.KeysInitial[i], valuesToAdd[i]);
            }

            TValCol[] newValues = HelperMethods.UnionValues(
                d.ValuesInitial,
                valuesToAdd,
                d.ComparerValue,
                d.ValueCollectionCtor);

            AllContainsReturnTrue(collection, d.KeysInitial, newValues);

            const string Msg = "The count of the collection should not be " +
                "different after adding existing keys.";

            Assert.AreEqual(initialCount, collection.Count, Msg);
        }

        /// <summary>
        /// Checks that clearing the collection makes the count equal to zero.
        /// </summary>
        [Test]
        public void ClearSetsCountToZero()
        {
            var collection = d.ImplementorCtor();
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        /// <summary>
        /// Checks that the collection contains all of the specified initial
        /// keys.
        /// </summary>
        [Test]
        public void ContainsInitialKeys()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnTrue(collection, d.KeysInitial);
        }

        /// <summary>
        /// Checks that the collection contains all key-value pairs
        /// constructed using the specified initial keys and values.
        /// </summary>
        [Test]
        public void ContainsInitialKVPs()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnTrue(collection, d.KeysInitial, d.ValuesInitial);
        }

        /// <summary>
        /// Checks that the collection doesn't contain any of the excluded keys.
        /// Used to root out false positives.
        /// </summary>
        [Test]
        public void ContainsNoExcludedKeys()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnFalse(collection, d.KeysExcluded);
        }

        /// <summary>
        /// Checks that the collection doesn't contain any key-value pairs
        /// constructed from the specified excluded keys and default values.
        /// Used to root out false positives.
        /// </summary>
        [Test]
        public void ContainsNoExcludedKVP()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnFalse(collection, d.KeysExcluded, null);
        }

        /// <summary>
        /// Checks that the collection doesn't contain any key-value pairs
        /// constructed from the initial keys and a null collection.
        /// </summary>
        [Test]
        public void ContainsNoKVPsWithNullValueCollection()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnFalse(collection, d.KeysInitial, null);
        }

        /// <summary>
        /// Checks that the Count parameter correctly reports initial size.
        /// </summary>
        [Test]
        public void CountInitialValueIsCorrect()
        {
            var collection = d.ImplementorCtor();
            Assert.AreEqual(d.KeysInitial.Length, collection.Count);
        }

        /// <summary>
        /// Checks that the collection doesn't contain the specified key-value
        /// pair after removing it.
        /// </summary>
        [Test]
        public void RemoveKVP()
        {
            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                d.KeysInitial[0], d.ValuesInitial[0]);

            Assert.True(
                collection.Contains(kvp),
                "Initial KVP wasn't found in collection before removal.");

            collection.Remove(kvp);

            Assert.False(
                collection.Contains(kvp),
                "Removed KVP was still found in the collection.");
        }

        /// <summary>
        /// Checks that removing an existing key decrements the count of the
        /// collection.
        /// </summary>
        [Test]
        public void RemovingExistingKeyDecrementsCount()
        {
            var collection = d.ImplementorCtor();

            int initialCount = collection.Count;

            collection.Remove(d.KeysInitial[0]);

            Assert.AreEqual(
                collection.Count,
                initialCount - 1,
                "Count differs from expected value of initialCount - 1.");
        }

        /// <summary>
        /// Checks that the collection doesn't contain the specified key after
        /// removing it.
        /// </summary>
        [Test]
        public void RemovingExistingKeyIsNoLongerContained()
        {
            var collection = d.ImplementorCtor();

            Assert.True(
                collection.ContainsKey(d.KeysInitial[0]),
                "Initial key wasn't found in collection before removal.");

            collection.Remove(d.KeysInitial[0]);

            Assert.False(
                collection.ContainsKey(d.KeysInitial[0]),
                "Removed key was still found in the collection.");
        }

        /// <summary>
        /// Checks that removing an existing key returns true.
        /// </summary>
        [Test]
        public void RemovingExistingKeyReturnsTrue()
        {
            var collection = d.ImplementorCtor();

            Assert.True(
                collection.Remove(d.KeysInitial[0]),
                "Remove should have returned true but returned false instead.");
        }

        /// <summary>
        /// Checks that the collection doesn't contain the specified key-value
        /// pair after removing it.
        /// </summary>
        [Test]
        public void RemovingExistingKVPDecrementsCount()
        {
            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                d.KeysInitial[0], d.ValuesInitial[0]);
            int initialCount = collection.Count;

            collection.Remove(kvp);

            Assert.AreEqual(
                collection.Count,
                initialCount - 1,
                "Count differs from expected value of initialCount - 1.");
        }

        /// <summary>
        /// Checks that the collection doesn't contain the specified key-value
        /// pair after removing it.
        /// </summary>
        [Test]
        public void RemovingExistingKVPIsNoLongerContained()
        {
            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                d.KeysInitial[0], d.ValuesInitial[0]);

            Assert.True(
                collection.Contains(kvp),
                "Initial KVP wasn't found in collection before removal.");

            collection.Remove(kvp);

            Assert.False(
                collection.Contains(kvp),
                "Removed KVP was still found in the collection.");
        }

        /// <summary>
        /// Checks that the collection doesn't contain the specified key-value
        /// pair after removing it.
        /// </summary>
        [Test]
        public void RemovingExistingKVPReturnsTrue()
        {
            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                d.KeysInitial[0], d.ValuesInitial[0]);

            Assert.True(
                collection.Remove(kvp),
                "Remove should have returned true but returned false instead.");
        }

        /// <summary>
        /// Checks that remove throws a <see cref="ArgumentNullException"/>
        /// when trying to remove a key-value pair with a null key.
        /// </summary>
        [Test]
        public void RemovingKVPWithNullKeyThrows()
        {
            if (default(TKey) != null)
            {
                Assert.Pass("The key is not of a nullable type, so " +
                    "this test is not applicable.");
            }

            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                default(TKey), d.ValuesExcluded[0]);

            Assert.Throws(
                typeof(ArgumentNullException),
                () => collection.Remove(kvp));
        }

        /// <summary>
        /// Checks that remove leaves the count of the collection unchanged when
        /// used on a non-existing key.
        /// </summary>
        public void RemovingNonExistingKeyLeavesCountUnchanged()
        {
            var collection = d.ImplementorCtor();
            var initialCount = collection.Count;

            collection.Remove(d.KeysExcluded[0]);

            Assert.AreEqual(collection.Count, initialCount);
        }

        /// <summary>
        /// Checks that remove returns false when used on a non-existing key.
        /// </summary>
        [Test]
        public void RemovingNonExistingKeyReturnsFalse()
        {
            var collection = d.ImplementorCtor();

            Assert.False(
                collection.Remove(d.KeysExcluded[0]),
                "Remove should have returned false but returned true instead.");
        }

        /// <summary>
        /// Checks that remove leaves the count of the collection unchanged when
        /// used on a non-existing key-value pair.
        /// </summary>
        public void RemovingNonExistingKVPLeavesCountUnchanged()
        {
            var collection = d.ImplementorCtor();
            var initialCount = collection.Count;
            var kvp = new KeyValuePair<TKey, TValCol>(
                d.KeysExcluded[0], d.ValuesExcluded[0]);

            collection.Remove(kvp);

            Assert.AreEqual(collection.Count, initialCount);
        }

        /// <summary>
        /// Checks that remove returns false when used on a non-existing
        /// key-value pair.
        /// </summary>
        [Test]
        public void RemovingNonExistingKVPReturnsFalse()
        {
            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                d.KeysExcluded[0], d.ValuesExcluded[0]);

            Assert.False(
                collection.Remove(kvp),
                "Remove should have returned false but returned true instead.");
        }

        /// <summary>
        /// Checks that remove throws a <see cref="ArgumentNullException"/>
        /// when trying to remove a null key.
        /// </summary>
        [Test]
        public void RemovingNullKeyThrows()
        {
            if (default(TKey) != null)
            {
                Assert.Pass("The key is not of a nullable type, so " +
                    "this test is not applicable.");
            }

            var collection = d.ImplementorCtor();

            Assert.Throws(
                typeof(ArgumentNullException),
                () => collection.Remove(default(TKey)));
        }

        /// <summary>
        /// Checks that TryGetValue gives the correct value in the out variable
        /// when used with an existing key.
        /// </summary>
        [Test]
        public void TryGetValueExistingKeyRetrievesCorrectValue()
        {
            var collection = d.ImplementorCtor();
            collection.TryGetValue(d.KeysInitial[0], out TValCol value);

            Assert.AreEqual(
                value,
                d.ValuesInitial[0],
                "TryGetValue retrieved a different value than expected.");
        }

        /// <summary>
        /// Checks that TryGetValue returns true when used with an existing key.
        /// </summary>
        [Test]
        public void TryGetValueExistingKeyReturnsTrue()
        {
            var collection = d.ImplementorCtor();
            Assert.True(
                collection.TryGetValue(d.KeysInitial[0], out TValCol value));
        }

        /// <summary>
        /// Checks that TryGetValue gives default value on the out variable when
        /// used with a non-existing key.
        /// </summary>
        [Test]
        public void TryGetValueNonExistingKeyRetrievesDefault()
        {
            var collection = d.ImplementorCtor();
            collection.TryGetValue(d.KeysExcluded[0], out TValCol value);

            Assert.AreEqual(value, default(TValCol));
        }

        /// <summary>
        /// Checks that TryGetValue returns false when used with a non-existing
        /// key.
        /// </summary>
        [Test]
        public void TryGetValueNonExistingKeyReturnsFalse()
        {
            var collection = d.ImplementorCtor();
            Assert.False(
                collection.TryGetValue(d.KeysExcluded[0], out TValCol value));
        }

        /// <summary>
        /// Checks that TryGetValue throws <see cref="ArgumentNullException"/>
        /// when used with a null key.
        /// </summary>
        [Test]
        public void TryGetValueNullKeyThrows()
        {
            if (default(TKey) != null)
            {
                Assert.Pass("The key is not of a nullable type, so " +
                    "this test is not applicable.");
            }

            var collection = d.ImplementorCtor();

            Assert.Throws(
                typeof(ArgumentNullException),
                () => collection.TryGetValue(default(TKey), out TValCol value));
        }

        #endregion Tests

        #region Private Methods

        private static void AllContainsReturnExpected(
            TIDict col, TKey[] keys, bool expected)
        {
            var keysWhereFail = new bool[keys.Length];
            bool allResultsHaveExpectedValue = true;

            for (int i = 0; i < keys.Length; i++)
            {
                bool result = col.ContainsKey(keys[i]);
                if (result != expected)
                {
                    allResultsHaveExpectedValue = false;
                    keysWhereFail[i] = true;
                }
            }

            if (allResultsHaveExpectedValue)
            {
                Assert.Pass();
            }
            else
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine().Append("{");
                for (int i = 0; i < keys.Length; i++)
                {
                    stringBuilder.AppendLine()
                        .Append("    Key: ")
                        .Append(keys[i]);

                    if (keysWhereFail[i])
                    {
                        stringBuilder.Append(" FAILED");
                    }
                }

                stringBuilder.AppendLine().Append("}");
                Assert.Fail($"{nameof(col.Contains)} method should have " +
                    $"returned {expected} but didn't on the following " +
                    $"keys pairs: {stringBuilder}.");
            }
        }

        private static void AllContainsReturnExpected(
            TIDict col, TKey[] keys, TValCol[] values, bool expected)
        {
            var kvpsWhereFail = new bool[keys.Length];
            var kvps = new KeyValuePair<TKey, TValCol>[keys.Length];
            bool allResultsHaveExpectedValue = true;

            for (int i = 0; i < keys.Length; i++)
            {
                var kvp = new KeyValuePair<TKey, TValCol>(keys[i], values[i]);
                bool result = col.Contains(kvp);
                kvps[i] = kvp;
                if (result != expected)
                {
                    allResultsHaveExpectedValue = false;
                    kvpsWhereFail[i] = true;
                }
            }

            if (allResultsHaveExpectedValue)
            {
                Assert.Pass();
            }
            else
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine().Append("{");
                for (int i = 0; i < kvps.Length; i++)
                {
                    stringBuilder.AppendLine()
                        .Append("    Key: ")
                        .Append(kvps[i].Key)
                        .Append(" | Values: { ")
                        .Append(string.Join(", ", kvps[i].Value))
                        .Append(" }");

                    if (kvpsWhereFail[i])
                    {
                        stringBuilder.Append(" FAILED");
                    }
                }

                stringBuilder.AppendLine().Append("}");
                Assert.Fail($"{nameof(col.Contains)} method should have " +
                    $"returned {expected} but didn't on the following " +
                    $"key-value pairs: {stringBuilder}.");
            }
        }

        private static void AllContainsReturnFalse(TIDict col, TKey[] keys)
            => AllContainsReturnExpected(col, keys, false);

        private static void AllContainsReturnFalse(
            TIDict col, TKey[] keys, TValCol[] values)
            => AllContainsReturnExpected(
                col, keys, values ?? new TValCol[keys.Length], false);

        private static void AllContainsReturnTrue(TIDict col, TKey[] keys)
            => AllContainsReturnExpected(col, keys, true);

        private static void AllContainsReturnTrue(
            TIDict col, TKey[] keys, TValCol[] values)
            => AllContainsReturnExpected(
                col, keys, values ?? new TValCol[keys.Length], true);

        #endregion Private Methods

        /// <summary>
        /// Encapsulates all of the data
        /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
        /// needs to run its test.
        /// </summary>
        public class Args
        {
            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="Args"/> class which encapsulates all of the data
            /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
            /// needs for its constructor and all of the data needed to
            /// construct a test fixture of matching type.
            /// </summary>
            /// <param name="implementorCtor">The constructor of a
            /// collection type that implements <typeparamref name="TIDict"/>
            /// and is initialized with data from <paramref name="initialKeys"/>
            /// and <paramref name="initialValues"/>.</param>
            /// <param name="valueCollectionCtor">Delegate pointing to a
            /// constructor of <see cref="TValCol"/> that takes
            /// <see cref="IEnumerable{T}"/> of type <see cref="TVal"/> as
            /// parameter.</param>
            /// <param name="data">The test data used for this fixture.</param>
            public Args(
                Func<TIDict> implementorCtor,
                Func<IEnumerable<TVal>, TValCol> valueCollectionCtor,
                MapTestData<TKey, TVal> data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                ImplementorCtor = implementorCtor
                    ?? throw new ArgumentNullException(
                        nameof(implementorCtor));

                ValueCollectionCtor = valueCollectionCtor
                    ?? throw new ArgumentNullException(
                        nameof(valueCollectionCtor));

                KeysInitial = data.KeysInitial;
                KeysToAdd = data.KeysToAdd;
                KeysExcluded = data.KeysExcluded;
                ComparerKey = data.ComparerKey;
                ComparerValue = data.ComparerValue;

                ValuesInitial = ConstructValCols(
                    data.ValuesInitial, valueCollectionCtor);

                ValuesToAdd = ConstructValCols(
                    data.ValuesToAdd, valueCollectionCtor);

                ValuesExcluded = ConstructValCols(
                    data.ValuesExcluded, valueCollectionCtor);
            }

            /// <summary>
            /// Gets a delegate pointing to the constructor of the collection
            /// class implementing the interface being tested.
            /// </summary>
            public Func<TIDict> ImplementorCtor { get; }

            /// <summary>
            /// Gets a delegate pointing to a constructor of
            /// <see cref="TValCol"/> that takes <see cref="IEnumerable{T}"/>
            /// of type <see cref="TVal"/> as parameter.
            /// </summary>
            public Func<IEnumerable<TVal>, TValCol> ValueCollectionCtor { get; }

            /// <summary>
            /// Gets an array with all of the initial keys contained in the
            /// implementor collection after construction.
            /// </summary>
            public TKey[] KeysInitial { get; }

            /// <summary>
            /// Gets an array with all of the initial values contained in the
            /// implementor collection after construction.
            /// </summary>
            public TValCol[] ValuesInitial { get; }

            /// <summary>
            /// Gets an array of keys not contained in the implementor
            /// collection after construction.
            /// </summary>
            public TKey[] KeysExcluded { get; }

            /// <summary>
            /// Gets an array of values associated with the excluded keys.
            /// </summary>
            public TValCol[] ValuesExcluded { get; }

            /// <summary>
            /// Gets an array of keys to add to the implementor collection after
            /// construction.
            /// </summary>
            public TKey[] KeysToAdd { get; }

            /// <summary>
            /// Gets an array of values to add to the implementor collection
            /// after construction.
            /// </summary>
            public TValCol[] ValuesToAdd { get; }

            /// <summary>
            /// Gets the comparer for the key type.
            /// </summary>
            public IEqualityComparer<TKey> ComparerKey { get; }

            /// <summary>
            /// Gets the comparer for the value type.
            /// </summary>
            public IEqualityComparer<TVal> ComparerValue { get; }

            private static TValCol[] ConstructValCols(
                TVal[][] vals, Func<IEnumerable<TVal>, TValCol> ctor)
            {
                var result = new TValCol[vals.Length];

                for (int i = 0; i < vals.Length; i++)
                {
                    result[i] = ctor(vals[i]);
                }

                return result;
            }
        }
    }
}