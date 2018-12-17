using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps.IMultiMap
{
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IMultiMap),
        nameof(MultiBiMap.FixtureSource_IMultiMap.GetArgs))]
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IMultiMap),
        nameof(MultiMap.FixtureSource_IMultiMap.GetArgs))]
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