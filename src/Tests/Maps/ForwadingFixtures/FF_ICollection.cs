using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixtureSource(
        typeof(MultiMap.FixtureSource_ICollection),
        nameof(MultiMap.FixtureSource_ICollection.GetArgs))]
    [TestFixtureSource(
        typeof(MultiBiMap.FixtureSource_ICollection),
        nameof(MultiBiMap.FixtureSource_ICollection.GetArgs))]
    public class FF_ICollection<TKey, TVal, TCollection, TValCol>
        : ICollectionTester<KeyValuePair<TKey, TValCol>, TCollection>
        where TCollection : ICollection<KeyValuePair<TKey, TValCol>>
    {
        public FF_ICollection(
            MultiMapDataConcrete<TKey, TVal, TCollection, TValCol> args) : base(args)
        { }
    }

    [TestFixtureSource(
        typeof(BiMap.FixtureSource_ICollection),
        nameof(BiMap.FixtureSource_ICollection.GetArgs))]
    public class FF_ICollection<TKey, TVal, TCollection>
        : ICollectionTester<KeyValuePair<TKey, TVal>, TCollection>
        where TCollection : ICollection<KeyValuePair<TKey, TVal>>
    {
        public FF_ICollection(
            MapDataConcrete<TKey, TVal, TCollection> args) : base(args)
        { }
    }
}