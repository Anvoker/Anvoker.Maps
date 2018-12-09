using System;
using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Anvoker.Collections.Tests.Maps.MultiMap
{
    /// <summary>
    /// Provides test data for a
    /// <see cref="IDictionaryNestedBase{TKey, TVal, TIDict, TValCol}"/> test
    /// fixture.
    /// </summary>
    public static class IDictionaryNested_FixtureSource
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

        private static Func<MultiMap<TKey, TVal>> GetCtor<TKey, TVal>(
            TKey[] keys,
            TVal[][] values,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerVal)
        {
            return () =>
            {
                var multiMap = new MultiMap<TKey, TVal>(
                    comparerKey,
                    comparerVal);
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
                = $"{nameof(MultiMap<TKey, TVal>)} | {data.TestDataName}";
            return IDictionaryNestedBase<TKey, TVal,
                MultiMap<TKey, TVal>, ICollection<TVal>>
                .ConstructFixtureParams(
                ctor,
                (x) => new HashSet<TVal>(x, data.ComparerValue),
                data,
                testName);
        }
    }
}