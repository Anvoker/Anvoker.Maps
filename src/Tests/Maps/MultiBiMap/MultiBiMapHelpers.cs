using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework.Internal;

namespace Anvoker.Collections.Tests.Maps.MultiBiMap
{
    public static class MultiBiMapHelpers
    {
        public static string Name { get; } = typeof(CompositeMultiBiMap<,>).Name;

        public static IEqualityComparer<TVCol>
            ComparerCol<TK, TV, TVCol>(MultiMapData<TK, TV> d)
            where TVCol : IEnumerable<TV>
            => new CollectionEqualityComparer<TV, TVCol>(d.ComparerValue);

        public static IMultiBiMap<TKey, TVal> Ctor<TKey, TVal>(
            MultiMapData<TKey, TVal> d)
        {
            var m = new CompositeMultiBiMap<TKey, TVal>(d.ComparerKey, d.ComparerValue);
            for (int i = 0; i < d.KeysInitial.Length; i++)
            {
                m.Add(d.KeysInitial[i], d.ValuesInitial[i]);
            }

            return m;
        }

        public static TVCol CtorVCol<TV, TVCol>(
            IEnumerable<TV> values,
            IEqualityComparer<TV> comparer)
            where TVCol : IEnumerable<TV>
            => (TVCol)(IEnumerable<TV>)(new HashSet<TV>(values, comparer));

        public static TestFixtureParameters
            GetFixtureParams<TK, TV, TVCol, TCollection>(
                MultiMapData<TK, TV> data,
                string fixtureName)
            where TVCol : IEnumerable<TV>
        {
            return MultiMapFixtureParamConstructor<TK, TV, TVCol, TCollection>
                .Construct(
                    (d) => (TCollection)Ctor(d),
                    CtorVCol<TV, TVCol>,
                    ComparerCol<TK, TV, TVCol>,
                    data,
                    Name,
                    fixtureName);
        }
    }
}