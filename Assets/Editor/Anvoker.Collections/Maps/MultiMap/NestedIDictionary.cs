using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Anvoker.Collections.Maps.Tests.MultiMap.IDictionary
{
    /// <summary>
    /// Does nothing but specify some of the types for
    /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>,
    /// leaving only the key and value types as generic.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys in the
    /// <see cref="MultiMap{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TVal">Type of the values in the
    /// <see cref="MultiMap{TKey, TVal}"/>.</typeparam>
    public class NestedIDictionary<TKey, TVal>
        : NestedIDictionaryBase<TKey, TVal, MultiMap<TKey, TVal>,
            ICollection<TVal>>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NestedIDictionary{TKey, TVal}"/> class with and the
        /// specified keys and values.
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
        public NestedIDictionary(
            MultiMap<TKey, TVal> multiMap,
            TKey[] initialKeys,
            ICollection<TVal>[] initialValueCollections,
            TKey[] excludedKeys)
            : base(multiMap, initialKeys, initialValueCollections, excludedKeys)
        {
        }
    }
}