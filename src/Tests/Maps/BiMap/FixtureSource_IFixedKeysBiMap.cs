using System;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.BiMap.BiMapHelpers;
using static Anvoker.Collections.Tests.Maps.MapDataSource;

namespace Anvoker.Collections.Tests.Maps.BiMap
{
    public static class FixtureSource_IFixedKeysBiMap
    {
        private static readonly string fixtureName
            = typeof(IFixedKeysBiMapTester<,,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            MapFixtureParamConstructor<int, decimal, IFixedKeysBiMap<int, decimal>>
                .Construct(Ctor, IntDecimal, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IFixedKeysBiMap<string, string>>
                .Construct(Ctor, StringStringInsensitive, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IFixedKeysBiMap<string, string>>
                .Construct(Ctor, StringStringSensitive, Name, fixtureName),

            MapFixtureParamConstructor<int[], Type, IFixedKeysBiMap<int[], Type>>
                .Construct(Ctor, ArrayIntType, Name, fixtureName),
        };
    }
}