using System;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Maps.Tests.BiMap.BiMapHelpers;
using static Anvoker.Maps.Tests.MapDataSource;

namespace Anvoker.Maps.Tests.BiMap
{
    public static class FixtureSource_IReadOnlyBiMap
    {
        private static readonly string fixtureName
            = typeof(IReadOnlyBiMapTester<,,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            MapFixtureParamConstructor<int, decimal, IReadOnlyBiMap<int, decimal>>
                .Construct(CompositeCtor, IntDecimal, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IReadOnlyBiMap<string, string>>
                .Construct(CompositeCtor, StringStringInsensitive, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IReadOnlyBiMap<string, string>>
                .Construct(CompositeCtor, StringStringSensitive, Name, fixtureName),

            MapFixtureParamConstructor<int[], Type, IReadOnlyBiMap<int[], Type>>
                .Construct(CompositeCtor, ArrayIntType, Name, fixtureName),
        };
    }
}