﻿using System;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.BiMap.BiMapHelpers;
using static Anvoker.Collections.Tests.Maps.MapDataSource;

namespace Anvoker.Collections.Tests.Maps.BiMap
{
    public static class FixtureSource_ICollection
    {
        private static readonly string fixtureName
            = typeof(ICollectionTester<,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            MapFixtureParamConstructor<int, decimal, CompositeBiMap<int, decimal>>
                .Construct(Ctor, IntDecimal, Name, fixtureName),

            MapFixtureParamConstructor<string, string, CompositeBiMap<string, string>>
                .Construct(Ctor, StringStringInsensitive, Name, fixtureName),

            MapFixtureParamConstructor<string, string, CompositeBiMap<string, string>>
                .Construct(Ctor, StringStringSensitive, Name, fixtureName),

            MapFixtureParamConstructor<int[], Type, CompositeBiMap<int[], Type>>
                .Construct(Ctor, ArrayIntType, Name, fixtureName),
        };
    }
}