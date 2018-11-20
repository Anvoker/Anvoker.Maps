using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Anvoker.Collections.Maps.Tests
{
    /// <summary>
    /// Provides functionality for testing whether a class correctly implements
    /// a dictionary interface with a collection as the value type.
    /// <para>This class is inherited from and fed the proper type arguments
    /// in order to run all of the tests on a particular class.</para>
    /// </summary>
    /// <typeparam name="TKey">Type of the key inside the IDictionary.</typeparam>
    /// <typeparam name="TVal">Type of the value inside of the nested collection
    /// <see cref="TValCol"/>, which works as the value type of the IDictionary.
    /// </typeparam>
    /// <typeparam name="TIDict">Type of the dictionary interface.
    /// </typeparam>
    /// <typeparam name="TValCol">Type of the nested collection used as the value
    /// type of the dictionary interface.</typeparam>
    public class NestedIDictionaryBase<TKey, TVal, TIDict, TValCol>
        where TIDict : IDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        private readonly TIDict collection;
        private readonly TKey[] initialKeys;
        private readonly TValCol[] initialValueCollections;
        private readonly TKey[] excludedKeys;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
        /// class with the specified collection that implements
        /// <typeparamref name="TIDict"/> and the specified keys and
        /// values.
        /// </summary>
        /// <param name="implementorCollection">A collection that implements
        /// <typeparamref name="TIDict"/> and is initialized with data
        /// from <paramref name="initialKeys"/> and
        /// <paramref name="initialValueCollections"/>.</param>
        /// <param name="initialKeys">An enumeration of keys also found in the
        /// specified <paramref name="implementorCollection"/>.
        /// <para>Used to verify tests.</para></param>
        /// <param name="initialValueCollections">An enumeration of collections
        /// of values also found in <paramref name="implementorCollection"/>.
        /// <para>Used to verify tests.</para></param>
        /// <param name="excludedKeys">An enumeration of value of the same type
        /// as the keys in <paramref name="implementorCollection"/>, none of
        /// which are contained in <paramref name="implementorCollection"/>.
        /// <para>Used to test for false positives.</para></param>
        public NestedIDictionaryBase(
            TIDict implementorCollection,
            TKey[] initialKeys,
            TValCol[] initialValueCollections,
            TKey[] excludedKeys)
        {
            this.collection = implementorCollection;
            this.initialKeys = initialKeys;
            this.initialValueCollections = initialValueCollections;
            this.excludedKeys = excludedKeys;
        }

        /// <summary>
        /// Checks that the collection initially contains all of the specified
        /// keys.
        /// </summary>
        [Test]
        public void ContainsInitialKeys()
        {
            bool[] result = new bool[initialKeys.Length];
            var fails = new List<TKey>();
            bool testPassed = true;

            for (int i = 0; i < initialKeys.Length; i++)
            {
                result[i] = collection.ContainsKey(initialKeys[i]);
                testPassed |= result[i];
                if (!result[i])
                {
                    fails.Add(initialKeys[i]);
                }
            }

            if (testPassed)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail($@"{nameof(collection.ContainsKey)} method should
                    have returned true but didn't on the following keys:
                    {fails}.");
            }
        }

        /// <summary>
        /// Checks that the collection doesn't contain any of the excluded keys.
        /// Used to root out false positives.
        /// </summary>
        [Test]
        public void ContainsNoExcludedKeys()
        {
            bool[] result = new bool[excludedKeys.Length];
            var fails = new List<TKey>();
            bool testPassed = true;

            for (int i = 0; i < initialKeys.Length; i++)
            {
                result[i] = collection.ContainsKey(excludedKeys[i]);
                testPassed |= !result[i];
                if (result[i])
                {
                    fails.Add(excludedKeys[i]);
                }
            }

            if (testPassed)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail($@"{nameof(collection.ContainsKey)} method should
                    have returned false but didn't on the following keys:
                    {fails}.");
            }
        }
    }
}