using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixtureSource(
        typeof(BiMap.FixtureSource_IBiMap),
        nameof(BiMap.FixtureSource_IBiMap.GetArgs))]
    public class FF_IBiMap<TKey, TVal, TBiMap> : IBiMapTester<TKey, TVal, TBiMap>
        where TBiMap : IBiMap<TKey, TVal>
    {
        public FF_IBiMap(MapDataConcrete<TKey, TVal, TBiMap> args) : base(args)
        { }
    }
}