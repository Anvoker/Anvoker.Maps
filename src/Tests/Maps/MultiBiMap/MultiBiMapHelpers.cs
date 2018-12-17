using System.Collections.Generic;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common;

namespace Anvoker.Collections.Tests.Maps.MultiBiMap
{
    public static class MultiBiMapHelpers
    {
        public static string Name { get; } = typeof(MultiBiMap<,>).Name;

        public static IMultiBiMap<TKey, TVal> Ctor<TKey, TVal>(
            MapTestData<TKey, TVal> d)
        {
            var m = new MultiBiMap<TKey, TVal>(d.ComparerKey, d.ComparerValue);
            for (int i = 0; i < d.KeysInitial.Length; i++)
            {
                m.Add(d.KeysInitial[i], d.ValuesInitial[i]);
            }

            return m;
        }

        public static ICollection<TVal> CtorValCol<TVal>(
            IEnumerable<TVal> values,
            IEqualityComparer<TVal> comparer)
            => new HashSet<TVal>(values, comparer);

        public static IReadOnlyCollection<TVal> CtorValColRO<TVal>(
            IEnumerable<TVal> values,
            IEqualityComparer<TVal> comparer)
            => new HashSet<TVal>(values, comparer);
    }
}
