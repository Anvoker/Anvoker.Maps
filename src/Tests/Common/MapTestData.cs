using System;
using System.Collections.Generic;

namespace Anvoker.Collections.Tests.Common
{
    /// <summary>
    /// Provides key, values and comparers appropriate for testing maps.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TVal">The type of the value.</typeparam>
    public class MapTestData<TKey, TVal>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MapTestData{TKey, TVal}"/> class using the specified
        /// keys, values and comparers.
        /// </summary>
        /// <param name="testDataName">A string used to construct test names
        /// later.</param>
        /// <param name="keysInitial">An array of keys initially contained
        /// in the collection.</param>
        /// <param name="keysToAdd">An array of keys to be added to the
        /// collection. Not initially contained.</param>
        /// <param name="keysExcluded">An array of keys not contained in
        /// the collection.</param>
        /// <param name="valuesInitial">An array of values associated with
        /// <paramref name="keysInitial"/>.</param>
        /// <param name="valuesToAdd">An array of values associated with
        /// <paramref name="keysToAdd"/>.</param>
        /// <param name="valuesExcluded">An array of values associated with
        /// <paramref name="keysExcluded"/>.</param>
        /// <param name="comparerKey">The comparer to use for key types.
        /// </param>
        /// <param name="comparerValue">The comparer to use for value types.
        /// </param>
        public MapTestData(
            string testDataName,
            TKey[] keysInitial,
            TKey[] keysToAdd,
            TKey[] keysExcluded,
            TVal[][] valuesInitial,
            TVal[][] valuesToAdd,
            TVal[][] valuesExcluded,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
        {
            TestDataName = testDataName
                ?? throw new ArgumentNullException(nameof(testDataName));

            KeysInitial = keysInitial
                ?? throw new ArgumentNullException(nameof(keysInitial));

            KeysToAdd = keysToAdd
                ?? throw new ArgumentNullException(nameof(keysToAdd));

            KeysExcluded = keysExcluded
                ?? throw new ArgumentNullException(nameof(keysExcluded));

            ValuesInitial = valuesInitial
                ?? throw new ArgumentNullException(nameof(valuesInitial));

            ValuesToAdd = valuesToAdd
                ?? throw new ArgumentNullException(nameof(valuesToAdd));

            ValuesExcluded = valuesExcluded
                ?? throw new ArgumentNullException(nameof(valuesExcluded));

            ComparerKey = comparerKey ?? EqualityComparer<TKey>.Default;
            ComparerValue = comparerValue ?? EqualityComparer<TVal>.Default;

            HasDuplicatesThrow(keysInitial, nameof(keysInitial), ComparerKey);
            HasDuplicatesThrow(keysToAdd, nameof(keysToAdd), ComparerKey);
            HasDuplicatesThrow(keysExcluded, nameof(keysExcluded), ComparerKey);

            LengthDiffersThrow(
                keysInitial,
                valuesInitial,
                nameof(keysInitial),
                nameof(valuesInitial));

            LengthDiffersThrow(
                keysToAdd,
                valuesToAdd,
                nameof(keysToAdd),
                nameof(valuesToAdd));

            LengthDiffersThrow(
                keysExcluded,
                valuesExcluded,
                nameof(keysExcluded),
                nameof(valuesExcluded));

            HasOverlapThrow(
                keysInitial,
                keysToAdd,
                nameof(keysInitial),
                nameof(keysToAdd),
                ComparerKey);

            HasOverlapThrow(
                keysInitial,
                keysExcluded,
                nameof(keysInitial),
                nameof(keysExcluded),
                ComparerKey);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MapTestData{TKey, TVal}"/> class by copying members from
        /// another <see cref="MapTestData{TKey, TVal}"/> instance.
        /// </summary>
        /// <param name="data">Instance to copy from.</param>
        public MapTestData(MapTestData<TKey, TVal> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            TestDataName = data.TestDataName;
            KeysInitial = data.KeysInitial;
            KeysToAdd = data.KeysToAdd;
            KeysExcluded = data.KeysExcluded;
            ValuesInitial = data.ValuesInitial;
            ValuesToAdd = data.ValuesToAdd;
            ValuesExcluded = data.ValuesExcluded;
            ComparerKey = data.ComparerKey;
            ComparerValue = data.ComparerValue;
        }

        /// <summary>
        /// Gets the name of this test data case.
        /// </summary>
        public string TestDataName { get; }

        /// <summary>
        /// Gets a unique set of keys.
        /// </summary>
        public TKey[] KeysInitial { get; }

        /// <summary>
        /// Gets a unique set of keys to add to the collection. Has no elements
        /// in common with <see cref="KeysInitial"/> or <see cref="KeysExcluded"/>.
        /// </summary>
        public TKey[] KeysToAdd { get; }

        /// <summary>
        /// Gets a unique set of keys to guaranteed to not be in the collection.
        /// Has no elements in common with <see cref="KeysInitial"/> or
        /// <see cref="KeysToAdd"/>.
        /// </summary>
        public TKey[] KeysExcluded { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysInitial"/>.
        /// </summary>
        public TVal[][] ValuesInitial { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysToAdd"/>.
        /// </summary>
        public TVal[][] ValuesToAdd { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysExcluded"/>.
        /// </summary>
        public TVal[][] ValuesExcluded { get; }

        /// <summary>
        /// Gets the comparer for the key type.
        /// </summary>
        public IEqualityComparer<TKey> ComparerKey { get; }

        /// <summary>
        /// Gets the comparer for the value type.
        /// </summary>
        public IEqualityComparer<TVal> ComparerValue { get; }

        private static void HasOverlapThrow(
            TKey[] arr1,
            TKey[] arr2,
            string name1,
            string name2,
            IEqualityComparer<TKey> comparer)
        {
            if (new HashSet<TKey>(arr1, comparer).Overlaps(
                new HashSet<TKey>(arr2, comparer)))
            {
                throw new ArgumentException($"{name1} and {name2} can't have " +
                    "any keys in common.");
            }
        }

        private static void LengthDiffersThrow(
            TKey[] arr1, TVal[][] arr2, string name1, string name2)
        {
            if (arr1.Length != arr2.Length)
            {
                throw new ArgumentException(
                    $"{name1} and {name2} must have equal length.");
            }
        }

        private static void HasDuplicatesThrow(
            TKey[] array, string name, IEqualityComparer<TKey> comparer)
        {
            var hashset = new HashSet<TKey>(comparer);
            foreach (TKey item in array)
            {
                if (!hashset.Add(item))
                {
                    throw new ArgumentException($"{name} must have no " +
                        "duplicate keys.");
                }
            }
        }
    }
}