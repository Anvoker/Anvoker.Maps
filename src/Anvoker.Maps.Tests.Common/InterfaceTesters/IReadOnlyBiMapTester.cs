using System.Linq;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common.Interfaces;
using NUnit.FixtureDependent;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Anvoker.Maps.Tests.Common
{
    public class IReadOnlyBiMapTester<TK, TV, TROBiMap>
        where TROBiMap : IReadOnlyBiMap<TK, TV>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValueData<TK, TV, TROBiMap> d;

        private TROBiMap bimap;

        public IReadOnlyBiMapTester(IKeyValueData<TK, TV, TROBiMap> args)
        {
            d = args;
        }

        [OneTimeSetUp]
        public void Setup() => bimap = d.ImplementorCtor();

        [Test]
        public void ComparerKeyReturnsCorrecTVue()
            => Assert.AreEqual(d.ComparerKey, bimap.ComparerKey);

        [Test]
        public void ComparerValueReturnsCorrecTVue()
            => Assert.AreEqual(d.ComparerValue, bimap.ComparerValue);

        [Test, SequentialDependent]
        public void ContainsValue_Excluded(
            [FixtureValueSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TROBiMap>.ValuesExcluded))]
            TV value)
            => Assert.False(bimap.ContainsValue(value));

        [Test, SequentialDependent]
        public void ContainsValue_Initial(
            [FixtureValueSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TROBiMap>.ValuesInitial))]
            TV value)
            => Assert.True(bimap.ContainsValue(value));

        [Test, SequentialDependent]
        public void GetKeysWithValue_Existing(
            [FixtureValueSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TROBiMap>.ValuesInitial))]
            TV value)
        {
            var expected = d.KVPsInitial
                .Where(x => d.ComparerValue.Equals(x.Value, value))
                .Select(x => x.Key);
            var actual = bimap.GetKeysWithValue(value);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test, SequentialDependent]
        public void GetKeysWithValue_NonExisting(
            [FixtureValueSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TROBiMap>.ValuesExcluded))]
            TV value)
            => CollectionAssert.IsEmpty(bimap.GetKeysWithValue(value));
    }
}