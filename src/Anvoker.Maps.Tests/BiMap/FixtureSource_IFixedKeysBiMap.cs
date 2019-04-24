using System;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Maps.Tests.BiMap.BiMapHelpers;
using static Anvoker.Maps.Tests.MapDataSource;

namespace Anvoker.Maps.Tests.BiMap
{
    public static class FixtureSource_IFixedKeysBiMap
    {
        private static readonly string fixtureName
            = typeof(IFixedKeysBiMapTester<,,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            MapFixtureParamConstructor<int, decimal, IFixedKeysBiMap<int, decimal>>
                .Construct(CompositeCtor, IntDecimal, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IFixedKeysBiMap<string, string>>
                .Construct(CompositeCtor, StringStringInsensitive, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IFixedKeysBiMap<string, string>>
                .Construct(CompositeCtor, StringStringSensitive, Name, fixtureName),

            MapFixtureParamConstructor<int[], Type, IFixedKeysBiMap<int[], Type>>
                .Construct(CompositeCtor, ArrayIntType, Name, fixtureName),
        };
    }
}