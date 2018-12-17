using System;
using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.MapTestDataSource;
using static Anvoker.Collections.Tests.Maps.MultiBiMap.MultiBiMapHelpers;

namespace Anvoker.Collections.Tests.Maps.MultiBiMap
{
    /// <summary>
    /// Provides test data for a
    /// <see cref="IRODictionaryNestedBase{TKey, TVal, TMultiMap, TValCol}"/> test
    /// fixture.
    /// </summary>
    public static class FixtureSource_IRODictionaryNested
    {
        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            IRODictionaryNestedBase<int, decimal,
                IMultiBiMap<int, decimal>,
                IReadOnlyCollection<decimal>>
            .MakeFixtureParams(Ctor, CtorValColRO, IntDecimal, Name),
            IRODictionaryNestedBase<string, string,
                IMultiBiMap<string, string>,
                IReadOnlyCollection<string>>
            .MakeFixtureParams(Ctor, CtorValColRO, StringStringInsensitive, Name),
            IRODictionaryNestedBase<string, string,
                IMultiBiMap<string, string>,
                IReadOnlyCollection<string>>
            .MakeFixtureParams(Ctor, CtorValColRO, StringStringSensitive, Name),
            IRODictionaryNestedBase<List<int>, Type,
                IMultiBiMap<List<int>, Type>,
                IReadOnlyCollection<Type>>
            .MakeFixtureParams(Ctor, CtorValColRO, ListIntType, Name)
        };
    }
}
