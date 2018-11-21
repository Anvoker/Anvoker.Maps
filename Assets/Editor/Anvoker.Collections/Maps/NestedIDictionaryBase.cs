using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Anvoker.Collections.Maps.Tests
{
    /// <summary>
    /// Provides functionality for testing whether a class correctly implements
    /// a dictionary interface with a collection as the value type.
    /// <para>This class is inherited from and fed the proper type arguments
    /// in order to run all of the tests on a particular class.</para>
    /// </summary>
    /// <typeparam name="TKey">Type of the key inside the IDictionary.
    /// </typeparam>
    /// <typeparam name="TVal">Type of the value inside of the nested collection
    /// <see cref="TValCol"/>, which works as the value type of the IDictionary.
    /// </typeparam>
    /// <typeparam name="TIDict">Type of the dictionary interface.
    /// </typeparam>
    /// <typeparam name="TValCol">Type of the nested collection used as the
    /// value type of the dictionary interface.
    /// </typeparam>
    [TestFixture]
    public class NestedIDictionaryBase<TKey, TVal, TIDict, TValCol>
        where TIDict : IDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        private readonly Args d;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
        /// class with the specified collection that implements
        /// <typeparamref name="TIDict"/> and the specified keys and
        /// values.
        /// </summary>
        /// <param name="args">A data class containing all of the necessary
        /// arguments for initializing the tests.</param>
        public NestedIDictionaryBase(Args args)
        {
            d = args;
        }

        /// <summary>
        /// Checks that the collection contains all of the specified initial
        /// keys.
        /// </summary>
        [Test]
        public void ContainsInitialKeys()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnTrue(collection, d.InitialKeys);
        }

        /// <summary>
        /// Checks that the collection contains all key-value pairs
        /// constructed using the specified initial keys and values.
        /// </summary>
        [Test]
        public void ContainsInitialKVPs()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnTrue(collection, d.InitialKeys, d.InitialValues);
        }

        /// <summary>
        /// Checks that the collection doesn't contain any of the excluded keys.
        /// Used to root out false positives.
        /// </summary>
        [Test]
        public void ContainsNoExcludedKeys()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnFalse(collection, d.ExcludedKeys);
        }

        /// <summary>
        /// Checks that the collection doesn't contain any key-value pairs
        /// constructed from the initial keys and default value collection. Used
        /// to root out false positives.
        /// </summary>
        [Test]
        public void ContainsNoKVPsWithInitialKeysAndDefaultValues()
        {
            var collection = d.ImplementorCtor();
            AllContainsReturnFalse(collection, d.InitialKeys, null);
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
            AllContainsReturnFalse(collection, d.ExcludedKeys, null);
        }

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
        /// Checks that adding a null key triggers an
        /// <see cref="ArgumentNullException"/> throw.
        /// </summary>
        [Test]
        public void AddNullKeyThrow()
        {
            if (default(TKey) != null)
            {
                Assert.Inconclusive(@"The key is not of a nullable type, so
                    this test is not applicable.");
            }

            var collection = d.ImplementorCtor();

            Assert.Throws(
                typeof(ArgumentNullException),
                () => collection.Add(default(TKey), d.ValuesToAdd[0]));
        }

        /// <summary>
        /// Checks that trying to add with an existing key causes the specified
        /// value to be associatd to that key in the implementor.
        /// </summary>
        [Test]
        public void AddExistingKey()
        {
            var collection = d.ImplementorCtor();
            int initialCount = collection.Count;

            var expectedValues = new TValCol[d.InitialKeys.Length];

            int length = Math.Min(d.InitialKeys.Length, d.ValuesToAdd.Length);
            for (int i = 0; i < length; i++)
            {
                collection.Add(d.InitialKeys[i], d.ValuesToAdd[i]);
            }

            TValCol[] newValues = HelperMethods.UnionValues(
                d.InitialValues,
                d.ValuesToAdd,
                d.ComparerValue,
                d.ValueCollectionCtor);

            AllContainsReturnTrue(collection, d.InitialKeys, newValues);

            const string Msg = "The count of the collection should not be " +
                "different after adding existing keys.";

            Assert.AreEqual(initialCount, collection.Count, Msg);
        }

        /*
         *             if (hasThrownAll)
            {
                Assert.Pass();
            }
            else
            {
                string strKeys = "{ " + string.Join(",", keysWhereFail) + " }";
                Assert.Fail($@"All attempts at adding need to throw
                    {nameof(ArgumentException)} but the following keys did not
                    trigger a throw: {strKeys}.");
            }*/

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
        /// Checks that adding a key value pair with ar null key triggers an
        /// <see cref="ArgumentNullException"/> throw.
        /// </summary>
        [Test]
        public void AddKVPNullKeyThrow()
        {
            if (default(TKey) != null)
            {
                Assert.Inconclusive("The key is not of a nullable type, so " +
                    "this test is not applicable.");
            }

            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                default(TKey), d.ValuesToAdd[0]);

            Assert.Throws(
                typeof(ArgumentNullException),
                () => collection.Add(kvp));
        }

        private void AllContainsReturnTrue(TIDict col, TKey[] keys)
            => AllContainsReturnExpected(col, keys, true);

        private void AllContainsReturnFalse(TIDict col, TKey[] keys)
            => AllContainsReturnExpected(col, keys, false);

        private void AllContainsReturnTrue(
            TIDict col, TKey[] keys, TValCol[] values)
            => AllContainsReturnExpected(
                col, keys, values ?? new TValCol[keys.Length], true);

        private void AllContainsReturnFalse(
            TIDict col, TKey[] keys, TValCol[] values)
            => AllContainsReturnExpected(
                col, keys, values ?? new TValCol[keys.Length], false);

        private void AllContainsReturnExpected(
            TIDict col, TKey[] keys, bool expected)
        {
            var keysWhereFail = new List<TKey>();
            bool allResultsHaveExpectedValue = true;

            for (int i = 0; i < keys.Length; i++)
            {
                bool result = col.ContainsKey(keys[i]);
                if (result != expected)
                {
                    allResultsHaveExpectedValue = false;
                    keysWhereFail.Add(keys[i]);
                }
            }

            if (allResultsHaveExpectedValue)
            {
                Assert.Pass();
            }
            else
            {
                string strKeys = "{ " + string.Join(",", keysWhereFail) + " }";
                Assert.Fail($"{nameof(col.ContainsKey)} method should have " +
                    $"returned {expected} but didn't on the following keys:" +
                    $" {strKeys}.");
            }
        }

        private void AllContainsReturnExpected(
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

        /// <summary>
        /// Encapsulates all of the data
        /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
        /// needs. Exists for readability purposes mostly.
        /// </summary>
        public class Args
        {
            /// <summary>
            /// Delegate pointing to the constructor of the collection
            /// class implementing the interface being tested. Referred to as
            /// the implementor.
            /// </summary>
            public readonly Func<TIDict> ImplementorCtor;

            /// <summary>
            /// Delegate pointing to the constructor of the value collection
            /// type of the implementor.
            /// </summary>
            public readonly Func<IEnumerable<TVal>, TValCol>
                ValueCollectionCtor;

            /// <summary>
            /// An array with all of the initial keys contained in the
            /// implementor after construction.
            /// </summary>
            public readonly TKey[] InitialKeys;

            /// <summary>
            /// An array with all of the inital values contained in the
            /// implementor after construction.
            /// </summary>
            public readonly TValCol[] InitialValues;

            /// <summary>
            /// An array of keys not contained in the implementor after
            /// construction.
            /// </summary>
            public readonly TKey[] ExcludedKeys;

            /// <summary>
            /// An array of values associated with the excluded keys.
            /// </summary>
            public readonly TValCol[] ExcludedValues;

            /// <summary>
            /// An array of keys to add to the implementor after construction.
            /// </summary>
            public readonly TKey[] KeysToAdd;

            /// <summary>
            /// An array of values to add to the implementor after construction.
            /// </summary>
            public readonly TValCol[] ValuesToAdd;

            private readonly IEqualityComparer<TKey> comparerKey;
            private readonly IEqualityComparer<TVal> comparerValue;

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="Args"/> class which encapsulates all of the data
            /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
            /// needs for its constructor.
            /// </summary>
            /// <param name="implementorCtor">The constructor of a
            /// collection type that implements <typeparamref name="TIDict"/>
            /// and is initialized with data from <paramref name="initialKeys"/>
            /// and <paramref name="initialValues"/>.</param>
            /// <param name="valueCollectionCtor">The constructor of
            /// the contained value collection type.</param>
            /// <param name="initialKeys">An array of keys also found in the
            /// implementor collection.</param>
            /// <param name="initialValues">An array of collections
            /// of values also found in the implementor collection.</param>
            /// <param name="excludedKeys">An array of values of the same
            /// type as the keys in the implementor collection, none of which
            /// are contained in the implementor collection.
            /// <para>Used to test for false positives.</para></param>
            /// <param name="excludedValues">An array of values of the same
            /// type as the values in the implementor collection, associated
            /// with the <paramref name="excludedKeys"/>.
            /// <para>Used to test for false positives.</para></param>
            /// <param name="keysToAdd">An array of keys to add to the
            /// implementor collection. Must have no keys in common with
            /// <paramref name="initialKeys"/>.</param>
            /// <param name="valuesToAdd">An array of values to add to the
            /// implementor collection.</param>
            /// <param name="comparerKey">The comparer to use for key types.
            /// </param>
            /// <param name="comparerValue">The comparer to use for value types.
            /// </param>
            public Args(
                Func<TIDict> implementorCtor,
                Func<IEnumerable<TVal>, TValCol> valueCollectionCtor,
                TKey[] initialKeys,
                TValCol[] initialValues,
                TKey[] excludedKeys,
                TValCol[] excludedValues,
                TKey[] keysToAdd,
                TValCol[] valuesToAdd,
                IEqualityComparer<TKey> comparerKey,
                IEqualityComparer<TVal> comparerValue)
            {
                ImplementorCtor = implementorCtor
                    ?? throw new ArgumentNullException(
                        nameof(implementorCtor));

                ValueCollectionCtor = valueCollectionCtor
                    ?? throw new ArgumentNullException(
                        nameof(valueCollectionCtor));

                InitialKeys = initialKeys
                    ?? throw new ArgumentNullException(
                        nameof(initialKeys));

                InitialValues = initialValues
                    ?? throw new ArgumentNullException(
                        nameof(initialValues));

                ExcludedKeys = excludedKeys
                    ?? throw new ArgumentNullException(
                        nameof(excludedKeys));

                ExcludedValues = excludedValues
                    ?? throw new ArgumentNullException(
                        nameof(excludedValues));

                KeysToAdd = keysToAdd
                    ?? throw new ArgumentNullException(
                        nameof(keysToAdd));

                ValuesToAdd = valuesToAdd
                    ?? throw new ArgumentNullException(
                        nameof(valuesToAdd));

                this.comparerKey = comparerKey;
                this.comparerValue = comparerValue;

                if (initialKeys.Length != initialValues.Length)
                {
                    throw new ArgumentException(
                        $"{nameof(initialKeys)} and {nameof(initialValues)} " +
                        "must have equal length.");
                }

                if (keysToAdd.Length != valuesToAdd.Length)
                {
                    throw new ArgumentException(
                        $"{nameof(keysToAdd)} and {nameof(valuesToAdd)} " +
                        "must have equal length.");
                }

                if (excludedKeys.Length != excludedValues.Length)
                {
                    throw new ArgumentException(
                        $"{nameof(excludedKeys)} and {nameof(excludedValues)}" +
                        " must have equal length.");
                }

                if (new HashSet<TKey>(initialKeys, ComparerKey).Overlaps(
                    new HashSet<TKey>(keysToAdd, ComparerKey)))
                {
                    throw new ArgumentException($"{nameof(initialKeys)} and " +
                        $"{nameof(keysToAdd)} can't have any keys in common.");
                }

                if (new HashSet<TKey>(initialKeys, ComparerKey).Overlaps(
                    new HashSet<TKey>(excludedKeys, ComparerKey)))
                {
                    throw new ArgumentException($"{nameof(initialKeys)} and " +
                        $"{nameof(excludedKeys)} can't have any keys in " +
                        $"common.");
                }
            }

            /// <summary>
            /// The comparer for the key type.
            /// </summary>
            public IEqualityComparer<TKey> ComparerKey => comparerKey
                ?? EqualityComparer<TKey>.Default;

            /// <summary>
            /// The comparer for the value type.
            /// </summary>
            public IEqualityComparer<TVal> ComparerValue => comparerValue
                ?? EqualityComparer<TVal>.Default;
        }
    }
}
