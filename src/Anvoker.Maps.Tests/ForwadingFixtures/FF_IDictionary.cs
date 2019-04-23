using System.Collections.Generic;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Maps.Tests
{
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IDictionary),
        nameof(MultiMap.FixtureSource_IDictionary.GetArgs))]
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IDictionary),
        nameof(MultiBiMap.FixtureSource_IDictionary.GetArgs))]
    public class FF_IDictionary<TKey, TVal, TCollection, TValCol>
        : IDictionaryTester<TKey, TValCol, TCollection>
        where TCollection : IDictionary<TKey, TValCol>
    {
        public FF_IDictionary(
            MultiMapDataConcrete<TKey, TVal, TCollection, TValCol> args) : base(args)
        { }
    }

    [TestFixtureSource(
        typeof(BiMap.FixtureSource_IDictionary),
        nameof(BiMap.FixtureSource_IDictionary.GetArgs))]
    public class FF_IDictionary<TKey, TVal, TCollection>
        : IDictionaryTester<TKey, TVal, TCollection>
        where TCollection : IDictionary<TKey, TVal>
    {
        public FF_IDictionary(
            MapDataConcrete<TKey, TVal, TCollection> args) : base(args)
        { }
    }
}