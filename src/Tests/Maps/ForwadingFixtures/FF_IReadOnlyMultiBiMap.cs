using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IReadOnlyMultiBiMap),
        nameof(MultiBiMap.FixtureSource_IReadOnlyMultiBiMap.GetArgs))]
    public class FF_IReadOnlyMultiBiMap<TKey, TVal, TROMultiBiMap, TValCol>
        : IReadOnlyMultiBiMapTester<TKey, TVal, TROMultiBiMap, TValCol>
        where TROMultiBiMap : IReadOnlyMultiBiMap<TKey, TVal>
        where TValCol : IReadOnlyCollection<TVal>
    {
        public FF_IReadOnlyMultiBiMap(
            MultiMapDataConcrete<TKey, TVal, TROMultiBiMap, TValCol> args)
            : base(args)
        { }
    }
}