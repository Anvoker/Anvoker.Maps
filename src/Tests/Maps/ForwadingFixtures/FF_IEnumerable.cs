using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IEnumerable),
        nameof(MultiMap.FixtureSource_IEnumerable.GetArgs))]
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IEnumerable),
        nameof(MultiBiMap.FixtureSource_IEnumerable.GetArgs))]
    public class FF_IEnumerable<TKey, TVal, TCollection, TValCol>
        : IEnumerableTester<KeyValuePair<TKey, TValCol>, TCollection>
        where TCollection : IEnumerable<KeyValuePair<TKey, TValCol>>
        where TValCol : IEnumerable<TVal>
    {
        public FF_IEnumerable(
            MultiMapDataConcrete<TKey, TVal, TCollection, TValCol> args) : base(args)
        { }
    }

    [TestFixtureSource(
        typeof(BiMap.FixtureSource_IEnumerable),
        nameof(BiMap.FixtureSource_IEnumerable.GetArgs))]
    public class FF_IEnumerable<TKey, TVal, TCollection>
        : IEnumerableTester<KeyValuePair<TKey, TVal>, TCollection>
        where TCollection : IEnumerable<KeyValuePair<TKey, TVal>>
    {
        public FF_IEnumerable(
            MapDataConcrete<TKey, TVal, TCollection> args) : base(args)
        { }
    }
}