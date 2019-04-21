using System;
using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.BiMap.BiMapHelpers;
using static Anvoker.Collections.Tests.Maps.MapDataSource;

namespace Anvoker.Collections.Tests.Maps.BiMap
{
    public static class FixtureSource_IDictionary
    {
        private static readonly string fixtureName
            = typeof(IDictionaryTester<,,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            MapFixtureParamConstructor<int, decimal, IDictionary<int, decimal>>
                .Construct(Ctor, IntDecimal, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IDictionary<string, string>>
                .Construct(Ctor, StringStringInsensitive, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IDictionary<string, string>>
                .Construct(Ctor, StringStringSensitive, Name, fixtureName),

            MapFixtureParamConstructor<int[], Type, IDictionary<int[], Type>>
                .Construct(Ctor, ArrayIntType, Name, fixtureName),
        };
    }
}