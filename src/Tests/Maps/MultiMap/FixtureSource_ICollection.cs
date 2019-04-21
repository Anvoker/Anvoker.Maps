using System;
using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.MultiMap.MultiMapHelpers;
using static Anvoker.Collections.Tests.Maps.MultiMapDataSource;

namespace Anvoker.Collections.Tests.Maps.MultiMap
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