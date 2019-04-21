using System;
using System.Collections.Generic;
using Anvoker.Collections.Tests.Common.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Common
{
    public class IDictionaryTester<TK, TV, TDictionary>
        where TDictionary : IDictionary<TK, TV>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValueData<TK, TV, TDictionary> d;

        private TDictionary dictionary;

        public IDictionaryTester(IKeyValueData<TK, TV, TDictionary> args)
        {
            d = args;
        }

        [SetUp]
        public void Setup() => dictionary = d.ImplementorCtor();

        [Test]
        public void Add_NonExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    var kvp = new KeyValuePair<TK, TV>(
                        d.KeysToAdd[i],
                        d.ValuesToAdd[i]);
                    dictionary.Add(d.KeysToAdd[i], d.ValuesToAdd[i]);
                    Assert.IsTrue(dictionary.ContainsKey(d.KeysToAdd[i]));
                    Assert.IsTrue(dictionary.Contains(kvp));
                }
            });
        }

        [Test]
        public void Add_ExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.Throws<ArgumentException>(() =>
                        dictionary.Add(d.KeysInitial[i], d.ValuesInitial[i]));
                }
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
                dictionary.Add(default(TK), default(TV)));
        }

        [Test]
        public void ContainsKey_InitialKeys()
        {
            Assert.Multiple(() =>
            {
                foreach (var key in d.KeysInitial)
                {
                    Assert.IsTrue(dictionary.ContainsKey(key));
                }
            });
        }

        [Test]
        public void ContainsKey_NoExcludedKeys()
        {
            Assert.Multiple(() =>
            {
                foreach (var key in d.KeysExcluded)
                {
                    Assert.IsFalse(dictionary.ContainsKey(key));
                }
            });
        }

        [Test]
        public void Remove_Key_Existing()
        {
            int initialCount = dictionary.Count;

            Assert.Multiple(() =>
            {
                Assert.True(
                    dictionary.Remove(d.KeysInitial[0]),
                    "A successful Remove should return true.");

                Assert.AreEqual(
                    dictionary.Count,
                    initialCount - 1,
                    "A successful Remove should decrement the count.");

                Assert.False(
                    dictionary.ContainsKey(d.KeysInitial[0]),
                    "Removed key was still found in the collection.");
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
                dictionary.Remove(default(TK)));
        }

        [Test]
        public void Remove_Key_NonExisting()
        {
            var initialCount = dictionary.Count;

            Assert.Multiple(() =>
            {
                Assert.False(
                    dictionary.Remove(d.KeysExcluded[0]),
                    "A failed Remove should return false.");

                Assert.AreEqual(
                    dictionary.Count,
                    initialCount,
                    "A failed Remove should not modify the count.");
            });
        }

        [Test]
        public void TryGetValue_ExistingKey()
        {
            var returnValue = dictionary
                .TryGetValue(d.KeysInitial[0], out TV value);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(value, d.ValuesInitial[0]);
                Assert.True(returnValue);
            });
        }

        [Test]
        public void TryGetValue_NonExistingKey()
        {
            var returnValue = dictionary
                .TryGetValue(d.KeysExcluded[0], out TV value);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(value, default(TV));
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

            Assert.Throws<ArgumentNullException>(() =>
                dictionary.TryGetValue(default(TK), out TV value));
        }

        [Test]
        public void Keys_Initial()
            => CollectionAssert.AreEquivalent(d.KeysInitial, dictionary.Keys);

        [Test]
        public void Values_Initial()
            => CollectionAssert.AreEquivalent(d.ValuesInitial, dictionary.Values);

        [Test]
        public void Indexer_Get_ExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.AreEqual(
                        d.ValuesInitial[i],
                        dictionary[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Get_NonExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysExcluded.Length; i++)
                {
                    Assert.Throws<KeyNotFoundException>(()
                        => { var v = dictionary[d.KeysExcluded[i]]; });
                }
            });
        }

        [Test]
        public void Indexer_Set_ExistingKey()
        {
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    dictionary[d.KeysInitial[i]] = d.ValuesExcluded[i];
                    Assert.AreEqual(
                        d.ValuesExcluded[i],
                        dictionary[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Set_NonExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    dictionary[d.KeysToAdd[i]] = d.ValuesToAdd[i];
                    Assert.AreEqual(
                        d.ValuesToAdd[i],
                        dictionary[d.KeysToAdd[i]]);
                }
            });
        }
    }
}