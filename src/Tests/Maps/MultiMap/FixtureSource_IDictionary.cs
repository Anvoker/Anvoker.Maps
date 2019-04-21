using System;
using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.MultiMap.MultiMapHelpers;
using static Anvoker.Collections.Tests.Maps.MultiMapDataSource;

namespace Anvoker.Collections.Tests.Maps.MultiMap
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