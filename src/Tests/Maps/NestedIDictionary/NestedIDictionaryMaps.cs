using System;
using System.Collections.Generic;
using System.Text;
using Anvoker.Collections.Tests.Common.NestedIDictionary;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

#pragma warning disable IDE0034 // Simplify 'default' expression

namespace Anvoker.Collections.Tests.Maps.NestedIDictionary
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
    [TestFixture(TestName = "Maps.NestedIDictionary")]
    [TestFixtureSource(
        typeof(SourceMap),
        nameof(SourceMap.TestFixtureSources))]
    public class NestedIDictionaryMaps<TKey, TVal, TIDict, TValCol> :
        NestedIDictionaryBase<TKey, TVal, TIDict, TValCol>
        where TIDict : IDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NestedIDictionaryMaps{TKey, TVal, TIDict, TValCol}"/>
        /// class with the specified collection that implements
        /// <typeparamref name="TIDict"/> with the specified keys and
        /// values, and with matching test data.
        /// </summary>
        /// <param name="args">A data class containing all of the necessary
        /// arguments for initializing the tests.</param>
        public NestedIDictionaryMaps(Args args) : base(args)
        {
        }
    }
}