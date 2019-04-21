using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IReadOnlyDictionary),
        nameof(MultiMap.FixtureSource_IReadOnlyDictionary.GetArgs))]
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IReadOnlyDictionary),
        nameof(MultiBiMap.FixtureSource_IReadOnlyDictionary.GetArgs))]
    public class FF_IReadOnlyDictionary<TKey, TVal, TCollection, TValCol>
        : IReadOnlyDictionaryTester<TKey, TValCol, TCollection>
        where TCollection : IReadOnlyDictionary<TKey, TValCol>
        where TValCol : IReadOnlyCollection<TVal>
    {
        public FF_IReadOnlyDictionary(
            MultiMapDataConcrete<TKey, TVal, TCollection, TValCol> args) : base(args)
        { }
    }

    [TestFixtureSource(
        typeof(BiMap.FixtureSource_IReadOnlyDictionary),
        nameof(BiMap.FixtureSource_IReadOnlyDictionary.GetArgs))]
    public class FF_IReadOnlyDictionary<TKey, TVal, TCollection>
        : IReadOnlyDictionaryTester<TKey, TVal, TCollection>
        where TCollection : IReadOnlyDictionary<TKey, TVal>
    {
        public FF_IReadOnlyDictionary(
            MapDataConcrete<TKey, TVal, TCollection> args) : base(args)
        { }
    }
}