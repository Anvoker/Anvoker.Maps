using System;
using System.Collections.Generic;
using Anvoker.Collections.Tests.Common.Interfaces;
using NUnit.FixtureDependent;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Common
{
    public class IReadOnlyDictionaryTester<TK, TV, TRODictionary>
        where TRODictionary : IReadOnlyDictionary<TK, TV>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValueData<TK, TV, TRODictionary> d;

        private TRODictionary dictionary;

        public IReadOnlyDictionaryTester(IKeyValueData<TK, TV, TRODictionary> args)
        {
            d = args;
        }

        [OneTimeSetUp]
        public void Setup() => dictionary = d.ImplementorCtor();

        [Test, SequentialDependent]
        public void ContainsKey_ExcludedKeys(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TRODictionary>.KeysExcluded))]
            TK key)
            => Assert.IsFalse(dictionary.ContainsKey(key));

        [Test, SequentialDependent]
        public void ContainsKey_InitialKeys(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TRODictionary>.KeysInitial))]
            TK key)
            => Assert.IsTrue(dictionary.ContainsKey(key));

        [Test]
        public void Count_InitialValueIsCorrect()
            => Assert.AreEqual(d.KeysInitial.Length, dictionary.Count);

        [Test]
        public void GetEnumerator_Initial()
        {
            var enumerator = dictionary.GetEnumerator();
            var keys = new List<TK>();
            var values = new List<TV>();
            using (enumerator)
            {
                while (enumerator.MoveNext())
                {
                    keys.Add(enumerator.Current.Key);
                    values.Add(enumerator.Current.Value);
                }
            }

            Assert.Multiple(() =>
            {
                CollectionAssert.AreEquivalent(dictionary.Keys, keys);
                CollectionAssert.AreEquivalent(dictionary.Values, values);
            });
        }

        [Test, SequentialDependent]
        public void Indexer_Get_ExistingKey(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TRODictionary>.KeysInitial))]
            TK key,
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TRODictionary>.ValuesInitial))]
            TV expectedValue)
            => Assert.AreEqual(expectedValue, dictionary[key]);

        [Test, SequentialDependent]
        public void Indexer_Get_NonExistingKey(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TRODictionary>.KeysExcluded))]
            TK key)
            => Assert.Throws<KeyNotFoundException>(()
                =>
            { var v = dictionary[key]; });

        [Test]
        public void Keys_Initial()
            => CollectionAssert.AreEquivalent(d.KeysInitial, dictionary.Keys);

        [Test, SequentialDependent]
        public void TryGetValue_ExistingKey(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TRODictionary>.KeysInitial))]
            TK key,
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TRODictionary>.ValuesInitial))]
            TV expectedValue)
        {
            var returnValue = dictionary.TryGetValue(key, out TV value);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(value, expectedValue);
                Assert.True(returnValue);
            });
        }

        [Test, SequentialDependent]
        public void TryGetValue_NonExistingKey(
            [ValueDependentSource(typeof(IKeyValueData<,,>),
                nameof(IKeyValueData<TK, TV, TRODictionary>.KeysExcluded))]
            TK key)
        {
            var returnValue = dictionary.TryGetValue(key, out TV value);

            Assert.Multiple(() =>
            {
                const string msg = "Should return default on a non-existing key.";
                Assert.AreEqual(value, default(TV), msg);
                Assert.False(returnValue);
            });
        }

        [Test]
        public void TryGetValue_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(()
                => dictionary.TryGetValue(default(TK), out TV value));
        }

        [Test]
        public void Values_Initial()
            => CollectionAssert.AreEquivalent(d.ValuesInitial, dictionary.Values);
    }
}