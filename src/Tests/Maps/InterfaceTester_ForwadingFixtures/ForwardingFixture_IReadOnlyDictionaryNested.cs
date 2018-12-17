using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps.IReadOnlyDictionaryNested
{
    [TestFixtureSource(
        typeof(MultiBiMap.IReadOnlyDictionaryNested_FixtureSource),
        nameof(MultiBiMap.IReadOnlyDictionaryNested_FixtureSource.GetFixtureArgs))]
    [TestFixtureSource(
        typeof(MultiMap.IReadOnlyDictionaryNested_FixtureSource),
        nameof(MultiMap.IReadOnlyDictionaryNested_FixtureSource.GetFixtureArgs))]
    public class ForwardingFixture<TKey, TVal, TIDict, TValCol>
        : IReadOnlyDictionaryNestedBase<TKey, TVal, TIDict, TValCol>
        where TIDict : IReadOnlyDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        public ForwardingFixture(
            MapTestDataConcrete<TKey, TVal, TIDict, TValCol> args) : base(args)
        { }
    }
}