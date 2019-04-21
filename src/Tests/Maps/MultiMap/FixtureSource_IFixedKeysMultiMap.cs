using System;
using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.MultiMap.MultiMapHelpers;
using static Anvoker.Collections.Tests.Maps.MultiMapDataSource;

namespace Anvoker.Collections.Tests.Maps.MultiMap
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