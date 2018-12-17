using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps.IDictionaryNested
{
    [TestFixtureSource(
        typeof(MultiBiMap.IDictionaryNested_FixtureSource),
        nameof(MultiBiMap.IDictionaryNested_FixtureSource.GetFixtureArgs))]
    [TestFixtureSource(
        typeof(MultiMap.IDictionaryNested_FixtureSource),
        nameof(MultiMap.IDictionaryNested_FixtureSource.GetFixtureArgs))]
    public class ForwardingFixture<TKey, TVal, TIDict, TValCol>
        : IDictionaryNestedBase<TKey, TVal, TIDict, TValCol>
        where TIDict : IDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        public ForwardingFixture(
            MapTestDataConcrete<TKey, TVal, TIDict, TValCol> args) : base(args)
        { }
    }
}