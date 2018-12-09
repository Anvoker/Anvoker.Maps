﻿using System;
using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Anvoker.Collections.Tests.Maps.MultiBiMap
{
    /// <summary>
    /// Provides test data for a
    /// <see cref="IMultiMapBase{TKey, TVal, TMultiMap, TValCol}"/> test
    /// fixture.
    /// </summary>
    public static class IMultiMap_FixtureSource
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
                ConstructFixtureParams(
                    MapTestDataSource.IntDecimal),
                ConstructFixtureParams(
                    MapTestDataSource.StringStringCaseInsensitive),
                ConstructFixtureParams(
                    MapTestDataSource.StringStringCaseSensitive),
                ConstructFixtureParams(
                    MapTestDataSource.ListType)
            };

        private static Func<MultiBiMap<TKey, TVal>> GetCtor<TKey, TVal>(
            TKey[] keys,
            TVal[][] values,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
        {
            return () =>
            {
                var multiMap = new MultiBiMap<TKey, TVal>(
                    comparerKey,
                    comparerValue);
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
            var keyType = typeof(TKey);
            var valType = typeof(TVal);
            var ctor = GetCtor(
                data.KeysInitial,
                data.ValuesInitial,
                data.ComparerKey,
                data.ComparerValue);
            string testName
                = $"{nameof(MultiBiMap<TKey, TVal>)} | {data.TestDataName}";
            return IMultiMapBase<TKey, TVal,
                MultiBiMap<TKey, TVal>, ICollection<TVal>>
                .ConstructFixtureParams(
                ctor,
                (x) => new HashSet<TVal>(x, data.ComparerValue),
                data,
                testName);
        }
    }
}