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
    [TestFixture]
    public class NestedIDictionary<TKey, TVal>
        : NestedIDictionaryBase<TKey, TVal, MultiMap<TKey, TVal>,
            ICollection<TVal>>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NestedIDictionary{TKey, TVal}"/>
        /// class with <see cref="MultiMap{TKey, TVal}"/> as the collection
        /// being tested.
        /// </summary>
        /// <param name="args">A data class containing all of the necessary
        /// arguments for initializing the tests.</param>
        public NestedIDictionary(Args args) : base(args)
        {
        }
    }
}