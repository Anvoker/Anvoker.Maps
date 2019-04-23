using Anvoker.Maps.Tests.Common;

namespace Anvoker.Maps.Tests.BiMap
{
    public static class BiMapHelpers
    {
        public static string Name { get; } = typeof(CompositeBiMap<,>).Name;

        public static CompositeBiMap<TKey, TVal> CompositeCtor<TKey, TVal>(MapData<TKey, TVal> d)
        {
            var m = new CompositeBiMap<TKey, TVal>(d.ComparerKey, d.ComparerValue);
            for (int i = 0; i < d.KeysInitial.Length; i++)
            {
                m.Add(d.KeysInitial[i], d.ValuesInitial[i]);
            }

            return m;
        }
    }
}