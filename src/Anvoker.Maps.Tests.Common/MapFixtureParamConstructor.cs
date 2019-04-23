using System;
using NUnit.FixtureDependent;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Anvoker.Maps.Tests.Common
{
    public static class MapFixtureParamConstructor<TK, TV, TCollection>
    {
        public static TestFixtureParameters
            Construct(
            Func<MapData<TK, TV>, TCollection> ctorImplementor,
            MapData<TK, TV> data,
            string concreteTypeName,
            string fixtureName = "")
        {
            var args = new MapDataConcrete<TK, TV, TCollection>(
                () => ctorImplementor(data), data);

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
                }
            };

            return new TestFixtureParameters(exposedParams);
        }
    }
}
