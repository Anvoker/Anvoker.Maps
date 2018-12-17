using System;
using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.MapTestDataSource;
using static Anvoker.Collections.Tests.Maps.MultiMap.MultiMapHelpers;

namespace Anvoker.Collections.Tests.Maps.MultiMap
{
    /// <summary>
    /// Provides test data for a
    /// <see cref="IDictionaryNestedBase{TKey, TVal, TMultiMap, TValCol}"/> test
    /// fixture.
    /// </summary>
    public static class FixtureSource_IDictionaryNested
    {
        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            IDictionaryNestedBase<int, decimal,
                IMultiMap<int, decimal>,
                ICollection<decimal>>
            .MakeFixtureParams(Ctor, CtorValCol, IntDecimal, Name),
            IDictionaryNestedBase<string, string,
                IMultiMap<string, string>,
                ICollection<string>>
            .MakeFixtureParams(Ctor, CtorValCol, StringStringInsensitive, Name),
            IDictionaryNestedBase<string, string,
                IMultiMap<string, string>,
                ICollection<string>>
            .MakeFixtureParams(Ctor, CtorValCol, StringStringSensitive, Name),
            IDictionaryNestedBase<List<int>, Type,
                IMultiMap<List<int>, Type>,
                ICollection<Type>>
            .MakeFixtureParams(Ctor, CtorValCol, ListIntType, Name)
        };
    }
}
