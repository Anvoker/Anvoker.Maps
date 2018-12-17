using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework.Internal;

namespace Anvoker.Collections.Tests.Common
{
    /// <summary>
    /// Provides key, values and comparers appropriate for testing maps.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TVal">The type of the value.</typeparam>
    public static class MapTestDataExtensionMethods
    {
        public static TestFixtureParameters Construct
            <TKey, TVal, TIDict, TValCol>(
            this MapTestData<TKey, TVal> data,
            Func<TKey[], TVal[][], IEqualityComparer<TKey>, IEqualityComparer<TVal>, TIDict> collectionCtor,
            Func<IEnumerable<TVal>, IEqualityComparer<TVal>, TValCol> valueCollectionCtor,
            string concreteTypeName)
        {
            var keyType = typeof(TKey);
            var valType = typeof(TVal);
            string testName
                = $"{concreteTypeName} | {data.TestDataName}";
            return MapTestDataConstructible<TKey, TVal, TIDict, TValCol>
                .ConstructFixtureParams(
                () => collectionCtor(
                    data.KeysInitial,
                    data.ValuesInitial,
                    data.ComparerKey,
                    data.ComparerValue),
                (IEnumerable<TVal> values) => valueCollectionCtor(
                    values,
                    data.ComparerValue),
                data,
                testName);
        }
    }
}