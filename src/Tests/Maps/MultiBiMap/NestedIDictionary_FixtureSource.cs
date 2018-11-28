﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using Anvoker.Collections.Tests.Maps.NestedIDictionary;

namespace Anvoker.Collections.Tests.Maps.MultiBiMap
{
    using Anvoker.Collections.Maps;
    using NestedIDictionary;

    /// <summary>
    /// Provides test data for a
    /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/> test
    /// fixture.
    /// </summary>
    public static class NestedIDictionary_FixtureSource
    {
        /// <summary>
        /// Provides the arguments for a test fixture that is decorated with
        /// <see cref="TestCaseSourceAttribute"/> and has a constructor with
        /// matching parameter types.
        /// </summary>
        /// <returns>An array of objects where each object is a collection
        /// that contains all of the necessary parameters to run a constructor
        /// of matching type.</returns>
        public static TestFixtureParameters[] GetFixtureArgs()
            => new TestFixtureParameters[]
            {
                ConstructFixtureParams(MapTestDataSource.IntDecimal),
                ConstructFixtureParams(MapTestDataSource.StringStringCaseInsensitive),
                ConstructFixtureParams(MapTestDataSource.StringStringCaseSensitive),
                ConstructFixtureParams(MapTestDataSource.ListType)
            };

        private static Func<MultiBiMap<TKey, TVal>> GetCtor<TKey, TVal>(
            TKey[] keys, TVal[][] values)
        {
            return () =>
            {
                var multiMap = new MultiBiMap<TKey, TVal>();
                for (int i = 0; i < keys.Length; i++)
                {
                    multiMap.Add(keys[i], values[i]);
                }

                return multiMap;
            };
        }

        private static TestFixtureParameters ConstructFixtureParams<TKey, TVal>(
            MapTestData<TKey, TVal> data)
        {
            return NestedIDictionaryMaps<TKey, TVal,
                MultiBiMap<TKey, TVal>, ICollection<TVal>>
                .ConstructFixtureParams(
                GetCtor(data.KeysInitial, data.ValuesInitial),
                (x) => new HashSet<TVal>(x),
                data);
        }
    }
}