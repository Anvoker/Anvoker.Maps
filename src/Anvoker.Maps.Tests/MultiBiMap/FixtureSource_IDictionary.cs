using System;
using System.Collections.Generic;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Maps.Tests.MultiBiMap.MultiBiMapHelpers;
using static Anvoker.Maps.Tests.MultiMapDataSource;

namespace Anvoker.Maps.Tests.MultiBiMap
{
    public static class FixtureSource_IDictionary
    {
        private static readonly string fixtureName
            = typeof(IDictionaryTester<,,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            GetFixtureParams<int, decimal, ISet<decimal>,
                IDictionary<int, ISet<decimal>>>(IntDecimal, fixtureName),

            GetFixtureParams<string, string, ISet<string>,
                IDictionary<string, ISet<string>>>(StringStringSensitive, fixtureName),

            GetFixtureParams<string, string, ISet<string>,
                IDictionary<string, ISet<string>>>(StringStringInsensitive, fixtureName),

            GetFixtureParams<int[], Type, ISet<Type>,
                IDictionary<int[], ISet<Type>>>(ArrayIntType, fixtureName),
        };
    }
}