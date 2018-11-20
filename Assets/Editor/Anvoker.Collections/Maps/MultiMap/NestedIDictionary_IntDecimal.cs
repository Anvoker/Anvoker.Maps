using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Api;

namespace Anvoker.Collections.Maps.Tests.MultiMap.IDictionary
{
    /// <summary>
    /// Runs tests from
    /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
    /// on <see cref="MultiMap{TKey, TVal}"/> using test data from
    /// <see cref="FixtureSource_IntDecimal"/>.
    /// </summary>
    [TestFixture, TestFixtureSource(
        typeof(FixtureSource_IntDecimal),
        nameof(FixtureSource_IntDecimal.FixtureArgs))]
    public class IntDecimal : NestedIDictionary<int, decimal>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IntDecimal"/> class with and the
        /// specified keys and values of <see cref="int"/> and
        /// <see cref="decimal"/> type respectively.
        /// </summary>
        /// <param name="multiMap">An instance of
        /// <see cref="MultiMap{TKey, TVal}"/> already initialized with the
        /// specified keys and values.</param>
        /// <param name="initialKeys">An enumeration of keys also found in the
        /// specified <paramref name="multiMap"/>.
        /// <para>Used to verify tests.</para></param>
        /// <param name="initialValueCollections">An enumeration of collections
        /// of values also found in <paramref name="multiMap"/>.
        /// <para>Used to verify tests.</para></param>
        /// <param name="excludedKeys">An enumeration of value of the same type
        /// as the keys in <paramref name="multiMap"/>, none of which are
        /// contained in <paramref name="multiMap"/>.
        /// <para>Used to test for false positives.</para></param>
        public IntDecimal(
            MultiMap<int, decimal> multiMap,
            int[] initialKeys,
            ICollection<decimal>[] initialValueCollections,
            int[] excludedKeys)
            : base(multiMap, initialKeys, initialValueCollections, excludedKeys)
        {
        }
    }
}