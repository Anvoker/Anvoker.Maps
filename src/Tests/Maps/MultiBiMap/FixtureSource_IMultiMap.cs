﻿using System;
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
    /// <see cref="IMultiMapBase{TKey, TVal, TMultiMap, TValCol}"/> test
    /// fixture.
    /// </summary>
    public static class FixtureSource_IMultiMap
    {
        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            IMultiMapBase<int, decimal,
                IMultiBiMap<int, decimal>,
                ICollection<decimal>>
            .MakeFixtureParams(Ctor, CtorValCol, IntDecimal, Name),
            IMultiMapBase<string, string,
                IMultiBiMap<string, string>,
                ICollection<string>>
            .MakeFixtureParams(Ctor, CtorValCol, StringStringInsensitive, Name),
            IMultiMapBase<string, string,
                IMultiBiMap<string, string>,
                ICollection<string>>
            .MakeFixtureParams(Ctor, CtorValCol, StringStringSensitive, Name),
            IMultiMapBase<List<int>, Type,
                IMultiBiMap<List<int>, Type>,
                ICollection<Type>>
            .MakeFixtureParams(Ctor, CtorValCol, ListIntType, Name)
        };
    }
}
