using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IReadOnlyMultiMap),
        nameof(MultiMap.FixtureSource_IReadOnlyMultiMap.GetArgs))]
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IReadOnlyMultiMap),
        nameof(MultiBiMap.FixtureSource_IReadOnlyMultiMap.GetArgs))]
    public class FF_IReadOnlyMultiMap<TKey, TVal, TMultiMap, TValCol>
        : IReadOnlyMultiMapTester<TKey, TVal, TMultiMap, TValCol>
        where TMultiMap : IReadOnlyMultiMap<TKey, TVal>
        where TValCol : IReadOnlyCollection<TVal>
    {
        public FF_IReadOnlyMultiMap(
            MultiMapDataConcrete<TKey, TVal, TMultiMap, TValCol> args)
            : base(args)
        { }
    }
}