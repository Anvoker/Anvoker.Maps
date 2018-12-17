using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps.IMultiMap
{
    [TestFixtureSource(
        typeof(MultiBiMap.IMultiMap_FixtureSource),
        nameof(MultiBiMap.IMultiMap_FixtureSource.GetFixtureArgs))]
    [TestFixtureSource(
        typeof(MultiMap.IMultiMap_FixtureSource),
        nameof(MultiMap.IMultiMap_FixtureSource.GetFixtureArgs))]
    public class ForwardingFixture<TKey, TVal, TMultiMap, TValCol>
        : IMultiMapBase<TKey, TVal, TMultiMap, TValCol>
        where TMultiMap : IMultiMap<TKey, TVal>
        where TValCol : ICollection<TVal>
    {
        public ForwardingFixture(
            MapTestDataConcrete<TKey, TVal, TMultiMap, TValCol> args)
            : base(args)
        { }
    }
}