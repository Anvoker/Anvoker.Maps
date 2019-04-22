using System;
using System.Collections.Generic;

namespace Anvoker.Collections.Tests.Common
{
    /// <summary>
    /// Provides key, values and comparers appropriate for testing maps.
    /// </summary>
    /// <typeparam name="TK">The type of the key.</typeparam>
    /// <typeparam name="TV">The type of the value.</typeparam>
    public class MultiMapData<TK, TV>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMapData{TK, TV}"/> class using the specified
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
        public MultiMapData(
            string testDataName,
            TK[] keysInitial,
            TK[] keysToAdd,
            TK[] keysExcluded,
            TV[][] valuesInitial,
            TV[][] valuesToAdd,
            TV[][] valuesExcluded,
            IEqualityComparer<TK> comparerKey,
            IEqualityComparer<TV> comparerValue)
        {
            TestDataName = testDataName;
            KeysInitial = keysInitial;
            KeysToAdd = keysToAdd;
            KeysExcluded = keysExcluded;
            ValuesInitial = valuesInitial;
            ValuesToAdd = valuesToAdd;
            ValuesExcluded = valuesExcluded;
            ComparerKey = comparerKey ?? EqualityComparer<TK>.Default;
            ComparerValue = comparerValue ?? EqualityComparer<TV>.Default;
            KeyIsNullabe = default(TK) == null;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMapData{TK, TV}"/> class by copying members from
        /// another <see cref="MultiMapData{TK, TV}"/> instance.
        /// </summary>
        /// <param name="data">Instance to copy from.</param>
        public MultiMapData(MultiMapData<TK, TV> data)
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
            ComparerKey = data.ComparerKey ?? EqualityComparer<TK>.Default;
            ComparerValue = data.ComparerValue ?? EqualityComparer<TV>.Default;
            KeyIsNullabe = default(TK) == null;
        }

        /// <summary>
        /// Gets or sets the name of this test data case.
        /// </summary>
        public string TestDataName { get; set; }

        /// <summary>
        /// Gets a unique set of keys.
        /// </summary>
        public TK[] KeysInitial { get; }

        /// <summary>
        /// Gets a unique set of keys to add to the collection. Has no elements
        /// in common with <see cref="KeysInitial"/> or <see cref="KeysExcluded"/>.
        /// </summary>
        public TK[] KeysToAdd { get; }

        /// <summary>
        /// Gets a unique set of keys to guaranteed to not be in the collection.
        /// Has no elements in common with <see cref="KeysInitial"/> or
        /// <see cref="KeysToAdd"/>.
        /// </summary>
        public TK[] KeysExcluded { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysInitial"/>.
        /// </summary>
        public TV[][] ValuesInitial { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysToAdd"/>.
        /// </summary>
        public TV[][] ValuesToAdd { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysExcluded"/>.
        /// </summary>
        public TV[][] ValuesExcluded { get; }

        /// <summary>
        /// Gets the comparer for the key type.
        /// </summary>
        public IEqualityComparer<TK> ComparerKey { get; }

        /// <summary>
        /// Gets the comparer for the value type.
        /// </summary>
        public IEqualityComparer<TV> ComparerValue { get; }

        /// <summary>
        /// Gets a value indicating whether the key is of a nullable type.
        /// </summary>
        public bool KeyIsNullabe { get; }
    }
}