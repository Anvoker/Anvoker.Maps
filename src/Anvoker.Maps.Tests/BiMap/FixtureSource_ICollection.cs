using System;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Maps.Tests.BiMap.BiMapHelpers;
using static Anvoker.Maps.Tests.MapDataSource;

namespace Anvoker.Maps.Tests.BiMap
{
    public static class FixtureSource_ICollection
    {
        private static readonly string fixtureName
            = typeof(ICollectionTester<,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            MapFixtureParamConstructor<int, decimal, CompositeBiMap<int, decimal>>
                .Construct(CompositeCtor, IntDecimal, Name, fixtureName),

            MapFixtureParamConstructor<string, string, CompositeBiMap<string, string>>
                .Construct(CompositeCtor, StringStringInsensitive, Name, fixtureName),

            MapFixtureParamConstructor<string, string, CompositeBiMap<string, string>>
                .Construct(CompositeCtor, StringStringSensitive, Name, fixtureName),

            MapFixtureParamConstructor<int[], Type, CompositeBiMap<int[], Type>>
                .Construct(CompositeCtor, ArrayIntType, Name, fixtureName),
        };
    }
}