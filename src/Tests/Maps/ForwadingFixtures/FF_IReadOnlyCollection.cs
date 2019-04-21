using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_IReadOnlyCollection),
        nameof(MultiMap.FixtureSource_IReadOnlyCollection.GetArgs))]
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_IReadOnlyCollection),
        nameof(MultiBiMap.FixtureSource_IReadOnlyCollection.GetArgs))]
    public class FF_IReadOnlyCollection<TKey, TVal, TCollection, TValCol>
        : IReadOnlyCollectionTester<KeyValuePair<TKey, TValCol>, TCollection>
        where TCollection : IReadOnlyCollection<KeyValuePair<TKey, TValCol>>
        where TValCol : IReadOnlyCollection<TVal>
    {
        public FF_IReadOnlyCollection(
            MultiMapDataConcrete<TKey, TVal, TCollection, TValCol> args) : base(args)
        { }
    }

    [TestFixtureSource(
        typeof(BiMap.FixtureSource_IReadOnlyCollection),
        nameof(BiMap.FixtureSource_IReadOnlyCollection.GetArgs))]
    public class FF_IReadOnlyCollection<TKey, TVal, TCollection>
        : IReadOnlyCollectionTester<KeyValuePair<TKey, TVal>, TCollection>
        where TCollection : IReadOnlyCollection<KeyValuePair<TKey, TVal>>
    {
        public FF_IReadOnlyCollection(
            MapDataConcrete<TKey, TVal, TCollection> args) : base(args)
        { }
    }
}