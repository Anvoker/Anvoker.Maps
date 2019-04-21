using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IFixedKeysMultiMap),
        nameof(MultiMap.FixtureSource_IFixedKeysMultiMap.GetArgs))]
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IFixedKeysMultiMap),
        nameof(MultiBiMap.FixtureSource_IFixedKeysMultiMap.GetArgs))]
    public class FF_IFixedKeysMultiMap<TKey, TVal, TMultiMap, TValCol>
        : IFixedKeysMultiMapTester<TKey, TVal, TMultiMap, TValCol>
        where TMultiMap : IFixedKeysMultiMap<TKey, TVal>
        where TValCol : IReadOnlyCollection<TVal>
    {
        public FF_IFixedKeysMultiMap(
            MultiMapDataConcrete<TKey, TVal, TMultiMap, TValCol> args)
            : base(args)
        { }
    }
}