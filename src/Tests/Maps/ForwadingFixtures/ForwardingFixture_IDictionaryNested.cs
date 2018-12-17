using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps.ForwardingFixtures
{
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IDictionaryNested),
        nameof(MultiBiMap.FixtureSource_IDictionaryNested.GetArgs))]
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IDictionaryNested),
        nameof(MultiMap.FixtureSource_IDictionaryNested.GetArgs))]
    public class IDictionaryNestedDerived<TKey, TVal, TIDict, TValCol>
        : IDictionaryNestedBase<TKey, TVal, TIDict, TValCol>
        where TIDict : IDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        public IDictionaryNestedDerived(
            MapTestDataConcrete<TKey, TVal, TIDict, TValCol> args) : base(args)
        { }
    }
}