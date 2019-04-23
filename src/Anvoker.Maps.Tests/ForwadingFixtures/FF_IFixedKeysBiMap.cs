using System.Collections.Generic;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Maps.Tests
{
    [TestFixtureSource(
        typeof(BiMap.FixtureSource_IFixedKeysBiMap),
        nameof(BiMap.FixtureSource_IFixedKeysBiMap.GetArgs))]
    public class FF_IFixedKeysBiMap<TKey, TVal, TBiMap>
        : IFixedKeysBiMapTester<TKey, TVal, TBiMap>
        where TBiMap : IFixedKeysBiMap<TKey, TVal>
    {
        public FF_IFixedKeysBiMap(MapDataConcrete<TKey, TVal, TBiMap> args)
            : base(args)
        { }
    }

    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IFixedKeysBiMap),
        nameof(MultiBiMap.FixtureSource_IFixedKeysBiMap.GetArgs))]
    public class FF_IFixedKeysBiMap<TKey, TVal, TBiMap, TValCol>
        : IFixedKeysBiMapTester<TKey, TValCol, TBiMap>
        where TBiMap : IFixedKeysBiMap<TKey, TValCol>
        where TValCol : IReadOnlyCollection<TVal>
    {
        public FF_IFixedKeysBiMap(MultiMapDataConcrete<TKey, TVal, TBiMap, TValCol> args)
            : base(args)
        { }
    }
}