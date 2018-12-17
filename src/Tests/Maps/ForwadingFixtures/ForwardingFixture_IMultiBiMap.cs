using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps.IMultiBiMap
{
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IMultiBiMap),
        nameof(MultiBiMap.FixtureSource_IMultiBiMap.GetArgs))]
    public class ForwardingFixture_IMultiBiMap<TKey, TVal, TMultiBiMap, TValCol>
        : IMultiBiMapBase<TKey, TVal, TMultiBiMap, TValCol>
        where TMultiBiMap : IMultiBiMap<TKey, TVal>
        where TValCol : ICollection<TVal>
    {
        public ForwardingFixture_IMultiBiMap(
            MapTestDataConcrete<TKey, TVal, TMultiBiMap, TValCol> args)
            : base(args)
        { }
    }
}