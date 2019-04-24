using System.Collections.Generic;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Maps.Tests
{
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IMultiMap),
        nameof(MultiMap.FixtureSource_IMultiMap.GetArgs))]
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IMultiMap),
        nameof(MultiBiMap.FixtureSource_IMultiMap.GetArgs))]
    public class FF_IMultiMap<TKey, TVal, TMultiMap, TValCol>
        : IMultiMapTester<TKey, TVal, TMultiMap, TValCol>
        where TMultiMap : IMultiMap<TKey, TVal>
        where TValCol : IReadOnlyCollection<TVal>
    {
        public FF_IMultiMap(
            MultiMapDataConcrete<TKey, TVal, TMultiMap, TValCol> args)
            : base(args)
        { }
    }
}