using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Maps.MapTestDataSource;

namespace Anvoker.Collections.Tests.Maps.MultiMap
{
    /// <summary>
    /// Provides test data for a
    /// <see cref="IDictionaryNestedBase{TKey, TVal, TMultiMap, TValCol}"/> test
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
        public static ITestFixtureData[] GetFixtureArgs()
            => new TestFixtureParameters[]
            {
                IntDecimal                 .Construct(GetCtor, GetCtorVal, typeof(MultiMap<,>).Name),
                StringStringCaseInsensitive.Construct(GetCtor, GetCtorVal, typeof(MultiMap<,>).Name),
                StringStringCaseSensitive  .Construct(GetCtor, GetCtorVal, typeof(MultiMap<,>).Name),
                ListType                   .Construct(GetCtor, GetCtorVal, typeof(MultiMap<,>).Name)
            };

        private static ICollection<TVal> GetCtorVal<TVal>(
            IEnumerable<TVal> x,
            IEqualityComparer<TVal> y)
            => new HashSet<TVal>(x, y);

        private static IDictionary<TKey, ICollection<TVal>> GetCtor<TKey, TVal>(
            TKey[] keys,
            TVal[][] values,
            IEqualityComparer<TKey> comparerKey,
            IEqualityComparer<TVal> comparerValue)
        {
            var map = new MultiMap<TKey, TVal>(comparerKey, comparerValue);
            for (int i = 0; i < keys.Length; i++)
            {
                map.Add(keys[i], values[i]);
            }

            return (IDictionary<TKey, ICollection<TVal>>)map;
        }
    }
}