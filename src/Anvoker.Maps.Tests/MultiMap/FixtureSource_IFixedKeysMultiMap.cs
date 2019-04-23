using System;
using System.Collections.Generic;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Maps.Tests.MultiMap.MultiMapHelpers;
using static Anvoker.Maps.Tests.MultiMapDataSource;

namespace Anvoker.Maps.Tests.MultiMap
{
    public static class FixtureSource_IFixedKeysMultiMap
    {
        private static readonly string fixtureName
            = typeof(IFixedKeysMultiMapTester<,,,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            GetFixtureParams<int, decimal, IReadOnlyCollection<decimal>,
                CompositeMultiMap<int, decimal>>(IntDecimal, fixtureName),

            GetFixtureParams<string, string, IReadOnlyCollection<string>,
                CompositeMultiMap<string, string>>(StringStringSensitive, fixtureName),

            GetFixtureParams<string, string, IReadOnlyCollection<string>,
                CompositeMultiMap<string, string>>(StringStringInsensitive, fixtureName),

            GetFixtureParams<int[], Type, IReadOnlyCollection<Type>,
                CompositeMultiMap<int[], Type>>(ArrayIntType, fixtureName),
        };
    }
}