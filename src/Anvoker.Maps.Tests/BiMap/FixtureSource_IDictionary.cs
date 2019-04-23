using System;
using System.Collections.Generic;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Maps.Tests.BiMap.BiMapHelpers;
using static Anvoker.Maps.Tests.MapDataSource;

namespace Anvoker.Maps.Tests.BiMap
{
    public static class FixtureSource_IDictionary
    {
        private static readonly string fixtureName
            = typeof(IDictionaryTester<,,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            MapFixtureParamConstructor<int, decimal, IDictionary<int, decimal>>
                .Construct(CompositeCtor, IntDecimal, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IDictionary<string, string>>
                .Construct(CompositeCtor, StringStringInsensitive, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IDictionary<string, string>>
                .Construct(CompositeCtor, StringStringSensitive, Name, fixtureName),

            MapFixtureParamConstructor<int[], Type, IDictionary<int[], Type>>
                .Construct(CompositeCtor, ArrayIntType, Name, fixtureName),
        };
    }
}