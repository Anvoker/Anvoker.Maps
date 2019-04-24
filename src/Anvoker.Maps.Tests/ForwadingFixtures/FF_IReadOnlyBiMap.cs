using System.Collections.Generic;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Maps.Tests
{
    [TestFixtureSource(
        typeof(BiMap.FixtureSource_IReadOnlyBiMap),
        nameof(BiMap.FixtureSource_IReadOnlyBiMap.GetArgs))]
    public class FF_IReadOnlyBiMap<TKey, TVal, TBiMap>
        : IReadOnlyBiMapTester<TKey, TVal, TBiMap>
        where TBiMap : IReadOnlyBiMap<TKey, TVal>
    {
        public FF_IReadOnlyBiMap(MapDataConcrete<TKey, TVal, TBiMap> args)
            : base(args)
        { }
    }

    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IReadOnlyBiMap),
        nameof(MultiBiMap.FixtureSource_IReadOnlyBiMap.GetArgs))]
    public class FF_IReadOnlyBiMap<TKey, TVal, TBiMap, TValCol>
        : IReadOnlyBiMapTester<TKey, TValCol, TBiMap>
        where TBiMap : IReadOnlyBiMap<TKey, TValCol>
        where TValCol : IReadOnlyCollection<TVal>
    {
        public FF_IReadOnlyBiMap(MultiMapDataConcrete<TKey, TVal, TBiMap, TValCol> args)
            : base(args)
        { }
    }
}