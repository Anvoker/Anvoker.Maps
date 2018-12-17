using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Anvoker.Collections.Tests.Common
{
    public static class MapTestDataFixtureParametersConstructor
    {
        public static TestFixtureParameters Construct<TKey, TVal, TIDict, TValCol, TInterfaceTester>(
            MapTestData<TKey, TVal> data,
            Func<TKey[], TVal[][], IEqualityComparer<TKey>, IEqualityComparer<TVal>, TIDict> collectionCtor,
            Func<IEnumerable<TVal>, IEqualityComparer<TVal>, TValCol> valueCollectionCtor)
            where TInterfaceTester : MapTestDataConstructible<TKey, TVal, TIDict, TValCol>
        {
            var keyType = typeof(TKey);
            var valType = typeof(TVal);
            string testName
                = $"{nameof(TIDict)} | {data.TestDataName}";
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
