using System;
using System.Collections.Generic;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Maps.Tests.MultiBiMap.MultiBiMapHelpers;
using static Anvoker.Maps.Tests.MultiMapDataSource;

namespace Anvoker.Maps.Tests.MultiBiMap
{
    public static class FixtureSource_ICollection
    {
        private static readonly string fixtureName
            = typeof(ICollectionTester<,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            GetFixtureParams<int, decimal, ISet<decimal>,
                ICollection<KeyValuePair<int, ISet<decimal>>>>(IntDecimal, fixtureName),

            GetFixtureParams<string, string, ISet<string>,
                ICollection<KeyValuePair<string, ISet<string>>>>(StringStringSensitive, fixtureName),

            GetFixtureParams<string, string, ISet<string>,
                ICollection<KeyValuePair<string, ISet<string>>>>(StringStringInsensitive, fixtureName),

            GetFixtureParams<int[], Type, ISet<Type>,
                ICollection<KeyValuePair<int[], ISet<Type>>>>(ArrayIntType, fixtureName),
        };
    }
}