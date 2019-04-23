using System;
using System.Collections.Generic;
using NUnit.FixtureDependent;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Anvoker.Maps.Tests.Common
{
    public static class MultiMapFixtureParamConstructor<TK, TV, TVCol, TCollection>
    {
        public static TestFixtureParameters
            Construct(
            Func<MultiMapData<TK, TV>, TCollection> ctorImplementor,
            Func<IEnumerable<TV>, IEqualityComparer<TV>, TVCol> ctorValCol,
            Func<MultiMapData<TK, TV>, IEqualityComparer<TVCol>> comparerCollection,
            MultiMapData<TK, TV> data,
            string concreteTypeName,
            string fixtureName = "")
        {
            var args = new MultiMapDataConcrete<TK, TV, TCollection, TVCol>(
                ()  => ctorImplementor(data),
                (x) => ctorValCol(x, data.ComparerValue),
                comparerCollection != null ? comparerCollection(data) : null,
                data);

            var exposedParams = new ExposedTestFixtureParams()
            {
                TestName = $"{fixtureName}({concreteTypeName}, {data.TestDataName})",
                Arguments = new object[] { args },
                Properties = new PropertyBag(),
                RunState = RunState.Runnable,
                TypeArgs = new Type[]
                {
                    typeof(TK),
                    typeof(TV),
                    typeof(TCollection),
                    typeof(TVCol)
                }
            };

            return new TestFixtureParameters(exposedParams);
        }
    }
}
