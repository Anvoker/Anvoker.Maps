using System;
using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.MultiBiMap.MultiBiMapHelpers;
using static Anvoker.Collections.Tests.Maps.MultiMapDataSource;

namespace Anvoker.Collections.Tests.Maps.MultiBiMap
{
    public static class FixtureSource_IFixedKeysBiMap
    {
        private static readonly string fixtureName
            = typeof(IFixedKeysBiMapTester<,,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            GetFixtureParams<int, decimal, IReadOnlyCollection<decimal>,
                CompositeMultiBiMap<int, decimal>>(IntDecimal, fixtureName),

            GetFixtureParams<string, string, IReadOnlyCollection<string>,
                CompositeMultiBiMap<string, string>>(StringStringSensitive, fixtureName),

            GetFixtureParams<string, string, IReadOnlyCollection<string>,
                CompositeMultiBiMap<string, string>>(StringStringInsensitive, fixtureName),

            GetFixtureParams<int[], Type, IReadOnlyCollection<Type>,
                CompositeMultiBiMap<int[], Type>>(ArrayIntType, fixtureName),
        };
    }
}