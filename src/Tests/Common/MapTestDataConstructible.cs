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
            ConstructFixtureParams(
            Func<TIDict> ctorImplementor,
            Func<IEnumerable<TVal>, TValCol> ctorTValCol,
            MapTestData<TKey, TVal> data,
            string testName)
        {
            var args = new MapTestDataConcrete<TKey, TVal, TIDict, TValCol>(
                ctorImplementor, ctorTValCol, data);
            var exposedParams = new ExposedTestFixtureParams()
            {
                TestName = testName,
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

    public interface IMapTestDataConstructible { }
}
