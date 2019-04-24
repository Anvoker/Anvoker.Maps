using System;
using System.Collections.Generic;
using System.Linq;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common.Interfaces;
using NUnit.FixtureDependent;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Anvoker.Maps.Tests.Common
{
    public class IReadOnlyMultiBiMapTester<TK, TV, TROMBiMap, TVCol>
        where TROMBiMap : IReadOnlyMultiBiMap<TK, TV>
        where TVCol : IReadOnlyCollection<TV>
    {
        private static readonly
            Func<KeyValuePair<TK, TVCol>, IEnumerable<TV>,
                SelectorExpected, IEqualityComparer<TV>, bool>
            selectorAdapter = (kvp, values, method, comparer)
                => method(kvp.Value, values, comparer);

        private static readonly
            IReadOnlyDictionary<string, Tuple<SelectorExpected, SelectorActual>>
            Selectors = new Dictionary<string, Tuple<SelectorExpected, SelectorActual>>()
            {
                {
                    "WithAny",
                    new Tuple<SelectorExpected, SelectorActual>(
                        (valuesMap, valuesQuery, comparer)
                            => new HashSet<TV>(valuesMap, comparer)
                            .Overlaps(valuesQuery),
                        (valuesMap, valuesQuery)
                            => valuesMap.GetKeysWithAny(valuesQuery))
                },
            };

        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValuesData<TK, TV, TROMBiMap, TVCol> d;

        private TROMBiMap map;

        public IReadOnlyMultiBiMapTester(
            IKeyValuesData<TK, TV, TROMBiMap, TVCol> args)
        {
            d = args;
        }

        public delegate IEnumerable<TK> SelectorActual(
            TROMBiMap map,
            IEnumerable<TV> values);

        public delegate bool SelectorExpected(
            IEnumerable<TV> x,
            IEnumerable<TV> y,
            IEqualityComparer<TV> comparer);

        public static IEnumerable<string> SelectorCases => Selectors.Keys;

        [OneTimeSetUp]
        public void Setup() => map = d.ImplementorCtor();

        [Test]
        public void ComparerKeyReturnsCorrecTVue()
            => Assert.AreEqual(d.ComparerKey, map.ComparerKey);

        [Test]
        public void ComparerValueReturnsCorrecTVue()
            => Assert.AreEqual(d.ComparerValue, map.ComparerValue);

        [Test, CombinatorialDependent]
        public void GetKeysWith_Excluded(
            [ValueSource(nameof(SelectorCases))]
            string selectorKey,
            [FixtureValueSource(typeof(IKeyValuesData<,,,>),
                nameof(IKeyValuesData<TK, TV, TROMBiMap, TVCol>.KVPsExcluded))]
            KeyValuePair<TK, TVCol> kvp)
        {
            var values = new HashSet<TV>(kvp.Value, d.ComparerValue);
            var selectors = Selectors[selectorKey];
            var selectorExpected = selectors.Item1;
            var selectorActual = selectors.Item2;
            var expected = GetKeysWithSelector(values, selectorExpected);
            var actual = selectorActual(map, values);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test, CombinatorialDependent]
        public void GetKeysWith_Initial(
            [ValueSource(nameof(SelectorCases))]
            string selectorKey,
            [FixtureValueSource(typeof(IKeyValuesData<,,,>),
                nameof(IKeyValuesData<TK, TV, TROMBiMap, TVCol>.KVPsInitial))]
            KeyValuePair<TK, TVCol> kvp)
        {
            var values = new HashSet<TV>(kvp.Value, d.ComparerValue);
            var selectors = Selectors[selectorKey];
            var selectorExpected = selectors.Item1;
            var selectorActual = selectors.Item2;
            var expected = GetKeysWithSelector(values, selectorExpected);
            var actual = selectorActual(map, values);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test, SequentialDependent]
        public void GetKeysWithValue_Excluded(
            [FixtureValueSource(typeof(IKeyValuesData<,,,>),
            nameof(IKeyValuesData<TK, TV, TROMBiMap, TVCol>.KVPsExcluded))]
            KeyValuePair<TK, TVCol> kvp)
        {
            if (kvp.Value.Count <= 0) { Assert.Pass(); }
            var value = kvp.Value.First();
            var values = HelperMethods.AsEnumerable(value);
            var selectors = Selectors["WithAny"];
            var selectorExpected = selectors.Item1;
            var selectorActual = selectors.Item2;
            var expected = d.KVPsInitial
                .Where(x => selectorAdapter(x, values, selectorExpected, d.ComparerValue))
                .Select(x => x.Key);
            var actual = map.GetKeysWithValue(value);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test, SequentialDependent]
        public void GetKeysWithValue_Initial(
            [FixtureValueSource(typeof(IKeyValuesData<,,,>),
            nameof(IKeyValuesData<TK, TV, TROMBiMap, TVCol>.KVPsInitial))]
            KeyValuePair<TK, TVCol> kvp)
        {
            if (kvp.Value.Count <= 0) { Assert.Pass(); }
            var value = kvp.Value.First();
            var values = HelperMethods.AsEnumerable(value);
            var selectors = Selectors["WithAny"];
            var selectorExpected = selectors.Item1;
            var selectorActual = selectors.Item2;
            var expected = d.KVPsInitial
                .Where(x => selectorAdapter(x, values, selectorExpected, d.ComparerValue))
                .Select(x => x.Key);
            var actual = map.GetKeysWithValue(value);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        private IEnumerable<TK> GetKeysWithSelector(
            IEnumerable<TV> valuesToSelectWith,
            SelectorExpected selector)
            => d.KVPsInitial
                .Where(x => selectorAdapter(x, valuesToSelectWith, selector, d.ComparerValue))
                .Select(x => x.Key);
    }
}