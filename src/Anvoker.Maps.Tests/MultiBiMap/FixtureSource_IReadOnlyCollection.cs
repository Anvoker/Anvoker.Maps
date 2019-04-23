using System;
using System.Collections.Generic;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Maps.Tests.MultiBiMap.MultiBiMapHelpers;
using static Anvoker.Maps.Tests.MultiMapDataSource;

namespace Anvoker.Maps.Tests.MultiBiMap
{
    public static class FixtureSource_IReadOnlyCollection
    {
        private static readonly string fixtureName
            = typeof(IReadOnlyCollectionTester<,>).Name;

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