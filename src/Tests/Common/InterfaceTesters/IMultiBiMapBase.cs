using System;
using System.Collections.Generic;
using System.Linq;
using Anvoker.Collections.Maps;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Common
{
    public class IMultiBiMapBase<TKey, TVal, TMultiBiMap, TValCol> :
        MapTestDataConstructible<TKey, TVal, TMultiBiMap, TValCol>
        where TMultiBiMap : IMultiBiMap<TKey, TVal>
        where TValCol : ICollection<TVal>
    {
        public delegate bool SelectorFunc(IEnumerable<TVal> x, IEnumerable<TVal> y, IEqualityComparer<TVal> comparer);
        public delegate IEnumerable<TKey> SelectorActualFunc(TMultiBiMap map, IEnumerable<TVal> values, bool ignoreEmpty);

        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly MapTestDataConcrete<TKey, TVal, TMultiBiMap, TValCol> d;

        public IMultiBiMapBase(
            MapTestDataConcrete<TKey, TVal, TMultiBiMap, TValCol> args)
        {
            d = args;
        }

        private static readonly
            Func<KeyValuePair<TKey, TValCol>,
                 IEnumerable<TVal>,
                 SelectorFunc,
                 IEqualityComparer<TVal>,
                 bool,
                 bool>
            selectorAdapter =
                (kvp, values, method, comparer, ignoreKeysWithNoValues) =>
                    (!ignoreKeysWithNoValues || kvp.Value.Count > 0)
                    && method(kvp.Value, values, comparer);

        private static readonly IReadOnlyList<SelectorFunc>
            Selectors = new List<SelectorFunc>()
            {
                (x, y, comp) => new HashSet<TVal>(x, comp).IsSubsetOf(y),
                (x, y, comp) => new HashSet<TVal>(x, comp).IsSupersetOf(y),
                (x, y, comp) => new HashSet<TVal>(x, comp).SetEquals(y),
                (x, y, comp) => new HashSet<TVal>(x, comp).Overlaps(y)
            };

        private static readonly IReadOnlyList<SelectorActualFunc>
            SelectorsActual = new List<SelectorActualFunc>()
            {
                (x, y, ignoreEmpty) => x.GetKeysWithSubset(y, ignoreEmpty),
                (x, y, ignoreEmpty) => x.GetKeysWithSuperset(y, ignoreEmpty),
                (x, y, ignoreEmpty) => x.GetKeysWithEqualSet(y, ignoreEmpty),
#pragma warning disable RCS1163 // Unused parameter.
                (x, y, ignoreEmpty) => x.GetKeysWithAny(y)
#pragma warning restore RCS1163 // Unused parameter.
            };

        private static readonly string[] SelectorNames = new string[]
        {
            "Subset",
            "Superset",
            "EqualSet",
            "Any"
        };

#pragma warning disable RCS1158 // Static member in generic type should use a type parameter.
        public static IEnumerable<TestCaseData> SelectorCases
#pragma warning restore RCS1158 // Static member in generic type should use a type parameter.
        {
            get
            {
                for(int i = 0; i < Selectors.Count; i++)
                {
                    yield return new
                        TestCaseData(false, Selectors[i], SelectorsActual[i])
                        .SetArgDisplayNames(false.ToString(), SelectorNames[i]);
                }

                for (int i = 0; i < Selectors.Count; i++)
                {
                    yield return new
                        TestCaseData(true, Selectors[i], SelectorsActual[i])
                        .SetArgDisplayNames(true.ToString(), SelectorNames[i]);
                }
            }
        }

        [Test, TestCaseSource(nameof(SelectorCases))]
        public void GetKeysWith_Initial(
            bool ignoreEmpty,
            SelectorFunc selector,
            SelectorActualFunc selectorActual)
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    var values = new HashSet<TVal>(
                        d.ValuesInitial[i],
                        d.ComparerValue);
                    var expected = d.KVPsInitial
                        .Where(x => selectorAdapter(x, values, selector,
                            d.ComparerValue, ignoreEmpty))
                        .Select(x => x.Key);
                    var actual = selectorActual(map, values, ignoreEmpty);
                    CollectionAssert.AreEquivalent(expected, actual);
                }
            });
        }

        [Test, TestCaseSource(nameof(SelectorCases))]
        public void GetKeysWith_Excluded(
            bool ignoreEmpty,
            SelectorFunc selector,
            SelectorActualFunc selectorActual)
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    var values = new HashSet<TVal>(
                        d.ValuesExcluded[i],
                        d.ComparerValue);
                    var expected = d.KVPsInitial
                        .Where(x => selectorAdapter(x, values, selector,
                            d.ComparerValue, ignoreEmpty))
                        .Select(x => x.Key);
                    var actual = selectorActual(map, values, ignoreEmpty);
                    CollectionAssert.AreEquivalent(expected, actual);
                }
            });
        }

        [Test]
        public void GetKeysWithSubset_Excluded()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    if (d.ValuesExcluded[i].Count <= 0
                    || d.ValuesInitial[i].Count <= 0) { continue; }
                    var expected = Enumerable.Empty<TVal>();
                    var actual = map.GetKeysWithSubset(d.ValuesExcluded[i]);
                    CollectionAssert.AreEquivalent(expected, actual);
                }
            });
        }

        [Test]
        public void GetKeysWithAny_NullArgumentThrows()
        {
            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(()
                => map.GetKeysWithAny(null));
        }

        [Test]
        public void GetKeysWithEqualSet_NullArgumentThrows()
        {
            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(()
                => map.GetKeysWithEqualSet(null));
        }

        [Test]
        public void GetKeysWithSubset_NullArgumentThrows()
        {
            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(()
                => map.GetKeysWithSubset(null));
        }

        [Test]
        public void GetKeysWithSuperset_NullArgumentThrows()
        {
            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(()
                => map.GetKeysWithSuperset(null));
        }

        [Test]
        public void TryGetKeyByCollectionRef()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    var values = map[d.KeysInitial[i]];
                    map.TryGetKeyByCollectionRef(values, out TKey key);
                    Assert.AreEqual(d.KeysInitial[i], key);
                }
            });
        }

        [Test]
        public void TryGetKeyByCollectionRef_NullArgumentThrows()
        {
            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() =>
                map.TryGetKeyByCollectionRef(null, out TKey key));
        }
    }
}
