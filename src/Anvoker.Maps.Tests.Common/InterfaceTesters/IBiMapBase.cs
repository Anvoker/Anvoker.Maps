using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Anvoker.Collections.Maps;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Common
{
    /// <summary>
    /// Provides functionality for testing whether a class correctly implements
    /// a dictionary interface with a nested collection as the value type.
    /// <para>
    /// The specific interface is <see cref="IDictionary{TKey, TValue}"/>
    /// where <typeparamref name="TKey"/> is the type of the dictionary's keys
    /// and <typeparamref name="TValCol"/> is the type of dictionary's values.
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">Type of the keys in
    /// <see cref="IDictionary{TKey, TValue}"/></typeparam>
    /// <typeparam name="TVal">
    /// Type of the values used in <see cref="TValCol"/>.</typeparam>
    /// <typeparam name="TBiMap">Type of the class implementing
    /// <see cref="IDictionary{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TValCol">Type of the nested collection used as the
    /// value type in <see cref="IDictionary{TKey, TValue}"/>.</typeparam>
    public class IBiMapBase<TKey, TVal, TBiMap, TValCol> :
        MapTestDataConstructible<TKey, TVal, TBiMap, TValCol>
        where TBiMap : IBiMap<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly MapTestDataConcrete<TKey, TVal, TBiMap, TValCol> d;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IDictionaryNestedBase{TKey, TVal, TIDict, TValCol}"/>
        /// class with the specified collection that implements
        /// <typeparamref name="TBiMap"/> with the specified keys and
        /// values, and with matching test data.
        /// </summary>
        /// <param name="args">A data class containing all of the necessary
        /// arguments for initializing the tests.</param>
        public IBiMapBase(
            MapTestDataConcrete<TKey, TVal, TBiMap, TValCol> args)
        {
            d = args;
        }

        [Test]
        public void Add_KeyAndValue_NonExistingKey()
        {
            var collection = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    var kvp = new KeyValuePair<TKey, TValCol>(
                        d.KeysToAdd[i],
                        d.ValuesToAdd[i]);
                    collection.Add(d.KeysToAdd[i], d.ValuesToAdd[i]);
                    Assert.IsTrue(collection.ContainsKey(d.KeysToAdd[i]));
                    Assert.IsTrue(collection.Contains(kvp));
                }
            });
        }

        [Test]
        public void Add_KeyAndValue_ExistingKey_NewValues()
        {
            var collection = d.ImplementorCtor();
            int l = Math.Min(d.KeysInitial.Length, d.ValuesToAdd.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < l; i++)
                {
                    var union = d.ValuesInitial[i].Union(d.ValuesToAdd[i]);
                    var kvpUnion = new KeyValuePair<TKey, TValCol>(
                        d.KeysInitial[i],
                        d.ValueCollectionCtor(union));
                    collection.Add(d.KeysInitial[i], d.ValuesToAdd[i]);
                    Assert.IsTrue(collection.ContainsKey(d.KeysInitial[i]));
                    Assert.IsTrue(collection.Contains(kvpUnion));
                }
            });
        }

        [Test]
        public void Add_KeyAndValue_ExistingKey_NoNewValues()
        {
            var collection = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    collection.Add(d.KeysInitial[i], d.ValuesInitial[i]);
                    Assert.IsTrue(collection.ContainsKey(d.KeysInitial[i]));
                    Assert.IsTrue(collection.Contains(d.KVPsInitial[i]));
                }
            });
        }

        [Test]
        public void Add_Key_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var collection = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() =>
                collection.Add(default(TKey), d.ValuesToAdd[0]));
        }

        [Test]
        public void Add_KVP_ExistingKey_NullValue()
        {
            if (default(TVal) != null)
            {
                Assert.Pass();
            }

            var collection = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    var value = AsEnumerable(default(TVal));
                    var valCol = d.ValueCollectionCtor(value);
                    var union = d.ValuesInitial[i].Union(value);
                    var kvp = new KeyValuePair<TKey, TValCol>(
                        d.KeysInitial[i],
                        valCol);
                    var kvpUnion = new KeyValuePair<TKey, TValCol>(
                        d.KeysInitial[i],
                        d.ValueCollectionCtor(union));
                    collection.Add(d.KeysInitial[i], valCol);
                    Assert.IsTrue(collection.ContainsKey(d.KeysInitial[i]));
                    Assert.IsTrue(collection.Contains(kvpUnion));
                }
            });
        }

        [Test]
        public void Add_KVP_ExistingKey()
        {
            var collection = d.ImplementorCtor();
            int l = Math.Min(d.KeysInitial.Length, d.ValuesToAdd.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < l; i++)
                {
                    var value = d.ValuesToAdd[i].Any()
                        ? AsEnumerable(d.ValuesToAdd[i].First())
                        : Enumerable.Empty<TVal>();
                    var union = d.ValuesInitial[i].Union(value);
                    var kvp = new KeyValuePair<TKey, TValCol>(
                        d.KeysInitial[i],
                        d.ValueCollectionCtor(value));
                    var kvpUnion = new KeyValuePair<TKey, TValCol>(
                        d.KeysInitial[i],
                        d.ValueCollectionCtor(union));
                    collection.Add(kvp);
                    Assert.IsTrue(collection.ContainsKey(d.KeysInitial[i]));
                    Assert.IsTrue(collection.Contains(kvpUnion));
                }
            });
        }

        [Test]
        public void Add_KVP_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                default(TKey), d.ValuesToAdd[0]);
            Assert.Throws<ArgumentNullException>(() => collection.Add(kvp));
        }

        [Test]
        public void Add_KVP_Regular()
        {
            var collection = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    var kvp = new KeyValuePair<TKey, TValCol>(
                        d.KeysToAdd[i], d.ValuesToAdd[i]);
                    collection.Add(kvp);
                    Assert.IsTrue(collection.ContainsKey(kvp.Key));
                    Assert.IsTrue(collection.Contains(kvp));
                }
            });
        }

        [Test]
        public void Replace_ExistingKey()
        {
            var bimap = d.ImplementorCtor();
            var length = Math.Max(d.KeysInitial.Length, d.KeysExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    bimap.Replace(d.KeysInitial[i], d.ValuesExcluded[i]);
                    CollectionAssert.AreEquivalent(
                        d.ValuesExcluded[i],
                        bimap[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Replace_NonExistingKey()
        {
            var bimap = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysExcluded.Length; i++)
                {
                    Assert.Throws<ArgumentException>(()
                        => bimap.Replace(d.KeysExcluded[i], d.ValuesExcluded[i]));
                }
            });
        }

        [Test]
        public void Replace_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var collection = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(()
                => collection.Replace(default(TKey), d.ValuesExcluded[0]));
        }

        [Test]
        public void Clear_SetsCountToZero()
        {
            var collection = d.ImplementorCtor();
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void ContainsKey_InitialKeys()
        {
            var collection = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                foreach (var key in d.KeysInitial)
                {
                    Assert.IsTrue(collection.ContainsKey(key));
                }
            });
        }

        [Test]
        public void Contains_InitialKVPs()
        {
            var collection = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                foreach (var kvp in d.KVPsInitial)
                {
                    Assert.IsTrue(collection.Contains(kvp));
                }
            });
        }

        [Test]
        public void ContainsKey_NoExcludedKeys()
        {
            var collection = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                foreach (var key in d.KeysExcluded)
                {
                    Assert.IsFalse(collection.ContainsKey(key));
                }
            });
        }

        [Test]
        public void Contains_NoExcludedKVP()
        {
            var collection = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                foreach (var kvp in d.KVPsExcluded)
                {
                    Assert.IsFalse(collection.Contains(kvp));
                }
            });
        }

        [Test]
        public void Count_InitialValueIsCorrect()
        {
            var collection = d.ImplementorCtor();
            Assert.AreEqual(d.KeysInitial.Length, collection.Count);
        }

        [Test]
        public void Remove_Key_Existing()
        {
            var collection = d.ImplementorCtor();
            int initialCount = collection.Count;

            Assert.Multiple(() =>
            {
                Assert.True(
                    collection.Remove(d.KeysInitial[0]),
                    "A successful Remove should return true.");

                Assert.AreEqual(
                    collection.Count,
                    initialCount - 1,
                    "A successful Remove should decrement the count.");

                Assert.False(
                    collection.ContainsKey(d.KeysInitial[0]),
                    "Removed key was still found in the collection.");
            });
        }

        [Test]
        public void Remove_Key_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var collection = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() =>
                collection.Remove(default(TKey)));
        }

        [Test]
        public void Remove_KVP_NonExistingKey()
        {
            var collection = d.ImplementorCtor();
            var initialCount = collection.Count;

            Assert.Multiple(() =>
            {
                Assert.False(
                    collection.Remove(d.KVPsExcluded[0]),
                    "A failed Remove should return false.");

                Assert.AreEqual(
                    collection.Count,
                    initialCount,
                    "A failed Remove should not modify the count.");
            });
        }

        [Test]
        public void Remove_KVP_NonExistingValue()
        {
            var collection = d.ImplementorCtor();
            var initialCount = collection.Count;
            var kvp = new KeyValuePair<TKey, TValCol>(
                d.KeysInitial[0],
                d.ValuesExcluded[0]);

            Assert.Multiple(() =>
            {
                Assert.False(
                    collection.Remove(kvp),
                    "A failed Remove should return false.");

                Assert.AreEqual(
                    collection.Count,
                    initialCount,
                    "A failed Remove should not modify the count.");
            });
        }

        [Test]
        public void Remove_KVP_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var collection = d.ImplementorCtor();
            var kvp = new KeyValuePair<TKey, TValCol>(
                default(TKey),
                d.ValuesInitial[0]);

            Assert.Throws<ArgumentNullException>(() =>
                collection.Remove(kvp));
        }

        /// <summary>
        /// Checks that removing an existing key-value pair leaves the
        /// collection in the expected state.
        /// </summary>
        [Test]
        public void Remove_KVP_Existing()
        {
            var collection = d.ImplementorCtor();
            int initialCount = collection.Count;

            Assert.Multiple(() =>
            {
                Assert.True(
                    collection.Remove(d.KVPsInitial[0]),
                    "A successful Remove should return true.");

                Assert.AreEqual(
                    collection.Count,
                    initialCount - 1,
                    "A successful Remove should decrement the count.");

                Assert.False(
                    collection.Contains(d.KVPsInitial[0]),
                    "Removed KVP was still found in the collection.");
            });
        }

        public void Remove_Key_NonExisting()
        {
            var collection = d.ImplementorCtor();
            var initialCount = collection.Count;

            Assert.Multiple(() =>
            {
                Assert.False(
                    collection.Remove(d.KeysExcluded[0]),
                    "A failed Remove should return false.");

                Assert.AreEqual(
                    collection.Count,
                    initialCount,
                    "A failed Remove should not modify the count.");

                Assert.True(
                    collection.ContainsKey(d.KeysExcluded[0]),
                    "A failed Remove's target key should still be contained.");
            });
        }

        [Test]
        public void TryGetValue_ExistingKey()
        {
            var collection = d.ImplementorCtor();

            var returnValue = collection
                .TryGetValue(d.KeysInitial[0], out TValCol value);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(
                value,
                d.ValuesInitial[0],
                "TryGetValue retrieved a different value than expected.");

                Assert.True(returnValue);
            });
        }

        [Test]
        public void TryGetValue_NonExistingKey()
        {
            var collection = d.ImplementorCtor();

            var returnValue = collection
                .TryGetValue(d.KeysExcluded[0], out TValCol value);

            Assert.Multiple(() =>
            {
                const string msg = "TryGetValue should return default on a " +
                "non-existing key.";
                Assert.AreEqual(value, default(TValCol), msg);

                Assert.False(returnValue);
            });
        }

        [Test]
        public void TryGetValue_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var collection = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() =>
                collection.TryGetValue(default(TKey), out TValCol value));
        }

        [Test]
        public void GetEnumerator()
        {
            var collection = d.ImplementorCtor();
            var enumerator = collection.GetEnumerator();
            var keys = new List<TKey>();
            var values = new List<TValCol>();
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
                CollectionAssert.AreEquivalent(collection.Keys, keys);
                CollectionAssert.AreEquivalent(collection.Values, values);
            });
        }

        [Test]
        public void CopyTo()
        {
            var collection = d.ImplementorCtor();
            var actual = new KeyValuePair<TKey, TValCol>[collection.Count];
            collection.CopyTo(actual, 0);

            CollectionAssert.AreEquivalent(d.KVPsInitial, actual);
        }

        [Test]
        public void Keys_Initial()
        {
            var collection = d.ImplementorCtor();
            CollectionAssert.AreEquivalent(d.KeysInitial, collection.Keys);
        }

        [Test]
        public void Values_Initial()
        {
            var collection = d.ImplementorCtor();
            CollectionAssert.AreEquivalent(d.ValuesInitial, collection.Values);
        }

        [Test]
        public void Indexer_Get_ExistingKey()
        {
            var collection = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    CollectionAssert.AreEquivalent(
                        d.ValuesInitial[i],
                        collection[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Get_NonExistingKey()
        {
            var collection = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysExcluded.Length; i++)
                {
                    IEnumerable<TVal> value;
                    Assert.Throws<KeyNotFoundException>(() =>
                        value = collection[d.KeysExcluded[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Set_ExistingKey()
        {
            var collection = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    collection[d.KeysInitial[i]] = d.ValuesExcluded[i];
                    CollectionAssert.AreEquivalent(
                        d.ValuesExcluded[i],
                        collection[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Set_NonExistingKey()
        {
            var collection = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysExcluded.Length; i++)
                {
                    TValCol value = d.ValuesExcluded[i];
                    Assert.Throws<KeyNotFoundException>(() =>
                        collection[d.KeysExcluded[i]] = value);
                }
            });
        }

        [Test]
        public void Indexer_Set_NullValueThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var collection = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() =>
                collection[d.KeysInitial[0]] = default(TValCol));
        }
    }
}