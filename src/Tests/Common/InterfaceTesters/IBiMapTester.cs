using System;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common.Interfaces;
using NUnit.FixtureDependent;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Common
{
    public class IBiMapTester<TK, TV, TBiMap>
        where TBiMap : IBiMap<TK, TV>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValueData<TK, TV, TBiMap> d;

        private TBiMap bimap;

        public IBiMapTester(IKeyValueData<TK, TV, TBiMap> args)
        {
            d = args;
        }

        [SetUp]
        public void Setup() => bimap = d.ImplementorCtor();

        [Test, SequentialDependent]
        public void Add_ExistingKeyThrows(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.KeysInitial))]
            TK key,
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.ValuesInitial))]
            TV value)
            => Assert.Throws<ArgumentException>(() => bimap.Add(key, value));

        [Test, SequentialDependent]
        public void Add_NonExistingKey(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.KeysToAdd))]
            TK key,
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.ValuesToAdd))]
            TV value)
        {
            bimap.Add(key, value);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(bimap.ContainsKey(key));
                Assert.AreEqual(value, bimap[key]);
            });
        }

        [Test]
        public void Add_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(() =>
                bimap.Add(default(TK), d.ValuesToAdd[0]));
        }

        [Test]
        public void Clear_SetsCountToZero()
        {
            bimap.Clear();
            Assert.AreEqual(0, bimap.Count);
        }

        [Test, SequentialDependent]
        public void Indexer_Set_ExistingKey(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.KeysInitial))]
            TK key,
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.ValuesInitial))]
            TV value)
        {
            bimap[key] = value;
            Assert.AreEqual(value, bimap[key]);
        }

        [Test, SequentialDependent]
        public void Indexer_Set_NonExistingKey(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.KeysToAdd))]
            TK key,
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.ValuesToAdd))]
            TV value)
        {
            bimap[key] = value;
            Assert.AreEqual(value, bimap[key]);
        }

        [Test]
        public void Indexer_Set_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(() =>
                bimap[default(TK)] = default(TV));
        }

        [Test, SequentialDependent]
        public void Remove_Key_Existing(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.KeysInitial))]
            TK key)
        {
            int initialCount = bimap.Count;

            Assert.Multiple(() =>
            {
                Assert.True(
                    bimap.Remove(key),
                    "A successful Remove should return true.");

                Assert.AreEqual(
                    bimap.Count,
                    initialCount - 1,
                    "A successful Remove should decrement the count.");

                Assert.False(
                    bimap.ContainsKey(key),
                    "Removed key was still found in the collection.");
            });
        }

        [Test, SequentialDependent]
        public void Remove_Key_NonExisting(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.KeysExcluded))]
            TK key)
        {
            var initialCount = bimap.Count;

            Assert.Multiple(() =>
            {
                Assert.False(
                    bimap.Remove(key),
                    "A failed Remove should return false.");

                Assert.AreEqual(
                    bimap.Count,
                    initialCount,
                    "A failed Remove should not modify the count.");
            });
        }

        [Test]
        public void Remove_Key_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(() =>
                bimap.Remove(default(TK)));
        }

        [Test]
        [SequentialDependent]
        public void Replace_ExistingKey(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.KeysInitial))]
            TK key,
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TBiMap>.ValuesToAdd))]
            TV newValue)
        {
            bimap.Replace(key, newValue);
            Assert.AreEqual(newValue, bimap[key]);
        }
    }
}