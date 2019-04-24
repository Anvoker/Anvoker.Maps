using System.Collections.Generic;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Maps.Tests
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