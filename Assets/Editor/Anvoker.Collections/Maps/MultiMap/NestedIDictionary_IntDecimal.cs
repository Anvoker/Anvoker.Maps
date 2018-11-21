using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

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
        nameof(FixtureSource_IntDecimal.GetNestedIDictionaryArgs))]
    public class IntDecimal : NestedIDictionary<int, decimal>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IntDecimal"/>
        /// class with that leverages
        /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
        /// with <see cref="MultiMap{TKey, TVal}"/> as the collection
        /// being tested and <see cref="int"/> and <see cref="decimal"/> as
        /// the key and value types respectively.
        /// </summary>
        /// <remark>This exists just for readability purposes. It offers an
        /// intermediary and more specific variant of
        /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>
        /// </remark>
        /// <param name="args">A data class containing all of the necessary
        /// arguments for initializing the tests.</param>
        public IntDecimal(Args args) : base(args)
        {
        }
    }
}