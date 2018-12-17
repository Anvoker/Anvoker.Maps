using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Anvoker.Collections.Tests.Common
{
    public class MapTestDataConstructible<TKey, TVal, TIDict, TValCol>
        : IMapTestDataConstructible
    {
        public static TestFixtureParameters
            MakeFixtureParams(
            Func<MapTestData<TKey, TVal>, TIDict> ctorImplementor,
            Func<IEnumerable<TVal>, IEqualityComparer<TVal>, TValCol> ctorValCol,
            MapTestData<TKey, TVal> data,
            string concreteTypeName)
        {
            TIDict pAppCtorImplementor()
                => ctorImplementor(data);

            TValCol pAppCtorValCol(IEnumerable<TVal> x)
                => ctorValCol(x, data.ComparerValue);

            var args = new MapTestDataConcrete<TKey, TVal, TIDict, TValCol>(
                pAppCtorImplementor, pAppCtorValCol, data);
            var exposedParams = new ExposedTestFixtureParams()
            {
                TestName = $"{concreteTypeName} | {data.TestDataName}",
                Arguments = new object[] { args },
                Properties = new PropertyBag(),
                RunState = RunState.Runnable,
                TypeArgs = new Type[]
                {
                    typeof(TKey),
                    typeof(TVal),
                    typeof(TIDict),
                    typeof(TValCol)
                }
            };

            return new TestFixtureParameters(exposedParams);
        }
    }

    public interface IMapTestDataConstructible
    { }
}
