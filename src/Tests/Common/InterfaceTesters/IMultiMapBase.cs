using System;
using System.Collections.Generic;
using System.Linq;
using Anvoker.Collections.Maps;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Common
{
    public class IMultiMapBase<TKey, TVal, TMultiMap, TValCol> :
        MapTestDataConstructible<TKey, TVal, TMultiMap, TValCol>
        where TMultiMap : IMultiMap<TKey, TVal>
        where TValCol : ICollection<TVal>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly MapTestDataConcrete<TKey, TVal, TMultiMap, TValCol> d;

        public IMultiMapBase(
            MapTestDataConcrete<TKey, TVal, TMultiMap, TValCol> args)
        {
            d = args;
        }

        /// <summary>
        /// Tests that the <see cref="IMultiMap{TKey, TVal}.ComparerKey"/>
        /// property returns the correct value.
        /// </summary>
        [Test]
        public void ComparerKeyReturnsCorrectValue()
        {
            var map = d.ImplementorCtor();
            Assert.AreEqual(d.ComparerKey, map.ComparerKey);
        }

        /// <summary>
        /// Tests that the <see cref="IMultiMap{TKey, TVal}.ComparerValue"/>
        /// property returns the correct value.
        /// </summary>
        [Test]
        public void ComparerValueReturnsCorrectValue()
        {
            var map = d.ImplementorCtor();
            Assert.AreEqual(d.ComparerValue, map.ComparerValue);
        }

        #region AddKey
        [Test]
        public void AddKey_Key_Regular()
        {
            var map = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    map.AddKey(d.KeysToAdd[i]);
                    Assert.IsTrue(map.ContainsKey(d.KeysToAdd[i]));
                }
            });
        }

        [Test]
        public void AddKey_KeyIEnumerable_Regular()
        {
            var map = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    map.AddKey(d.KeysToAdd[i], d.ValuesToAdd[i]);
                    Assert.IsTrue(map.ContainsKeyWithValues(
                        d.KeysToAdd[i],
                        d.ValuesToAdd[i]));
                }
            });
        }

        [Test]
        public void AddKey_KeyHashSet_Regular()
        {
            var map = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    var hashSet = new HashSet<TVal>(d.ValuesToAdd[i]);
                    map.AddKey(d.KeysToAdd[i], hashSet);
                    Assert.IsTrue(map.ContainsKeyWithValues(
                        d.KeysToAdd[i],
                        d.ValuesToAdd[i]));
                }
            });
        }

        [Test]
        public void AddKey_Key_ExistingKeyThrows()
        {
            var map = d.ImplementorCtor();

            Assert.Throws<ArgumentException>(() =>
                map.AddKey(d.KeysInitial[0]));
        }

        [Test]
        public void AddKey_KeyIEnumerable_ExistingKeyThrows()
        {
            var map = d.ImplementorCtor();

            Assert.Throws<ArgumentException>(() =>
                map.AddKey(d.KeysInitial[0], d.ValuesToAdd[0]));
        }

        [Test]
        public void AddKey_KeyHashSet_ExistingKeyThrows()
        {
            var map = d.ImplementorCtor();
            var hashSet = new HashSet<TVal>(d.ValuesToAdd[0]);

            Assert.Throws<ArgumentException>(() =>
                map.AddKey(d.KeysInitial[0], hashSet));
        }

        [Test]
        public void AddKey_Key_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() =>
                map.AddKey(default(TKey)));
        }

        [Test]
        public void AddKey_KeyIEnumerable_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() =>
                map.AddKey(default(TKey), d.ValuesToAdd[0]));
        }

        [Test]
        public void AddKey_KeyHashSet_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var map = d.ImplementorCtor();
            var hashSet = new HashSet<TVal>(d.ValuesToAdd[0]);
            Assert.Throws<ArgumentNullException>(() =>
                map.AddKey(default(TKey), hashSet));
        }
        #endregion

        #region AddValue
        [Test]
        public void AddValue_ExistingKey_NonExistingValue()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.ValuesExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    if (d.ValuesToAdd[i].Count <= 0) { continue; }
                    var value = d.ValuesExcluded[i].First();
                    var union = d.ValuesInitial[i].Union(
                        AsEnumerable(value),
                        d.ComparerValue);

                    Assert.IsTrue(
                        map.AddValue(d.KeysInitial[i], value));
                    Assert.IsTrue(
                        map.ContainsKeyWithValues(d.KeysInitial[i], union));
                }
            });
        }

        [Test]
        public void AddValue_ExistingKey_ExistingValue()
        {
            var map = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0) { continue; }
                    var value = d.ValuesInitial[i].First();
                    Assert.IsFalse(map.AddValue(d.KeysInitial[i], value));
                }
            });
        }

        [Test]
        public void AddValue_NonExistingKey()
        {
            var map = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0) { continue; }
                    var value = d.ValuesInitial[i].First();
                    Assert.IsFalse(map.AddValue(d.KeysInitial[i], value));
                }
            });
        }

        [Test]
        public void AddValue_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var map = d.ImplementorCtor();
            var value = d.ValuesToAdd[0].First();
            Assert.Throws<ArgumentNullException>(
                () => map.AddValue(default(TKey), value));
        }

        #endregion AddValue

        #region AddValues
        [Test]
        public void AddValues_ExistingKey_NoCommonValues()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.ValuesExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    var union = d.ValuesInitial[i].Union(
                        d.ValuesExcluded[i],
                        d.ComparerValue);

                    if (d.ValuesExcluded[i].Count > 0)
                    {
                        Assert.IsTrue(
                            map.AddValues(
                                d.KeysInitial[i],
                                d.ValuesExcluded[i]));
                    }
                    else
                    {
                        Assert.IsFalse(
                            map.AddValues(
                                d.KeysInitial[i],
                                d.ValuesExcluded[i]));
                    }
                    Assert.IsTrue(
                        map.ContainsKeyWithValues(d.KeysInitial[i], union));
                }
            });
        }

        [Test]
        public void AddValues_ExistingKey_SomeCommonValues()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.ValuesToAdd.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    var union = d.ValuesInitial[i].Union(
                        d.ValuesToAdd[i],
                        d.ComparerValue);
                    bool expected =
                    (union.Count() > d.ValuesInitial[i].Count
                        || union.Count() > d.ValuesToAdd[i].Count)
                    && (d.ValuesToAdd[i].Count > 0);

                    Assert.AreEqual(
                        expected,
                        map.AddValues(d.KeysInitial[i], union),
                        $"Key: {d.KeysInitial[i]} | " +
                        $"Value: {union.Aggregate("", (string str, TVal x) => str += x.ToString() + ", ")}");
                    Assert.IsTrue(
                        map.ContainsKeyWithValues(d.KeysInitial[i], union));
                }
            });
        }

        [Test]
        public void AddValues_ExistingKey_OnlyCommonValues()
        {
            var map = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.IsFalse(map.AddValues(
                        d.KeysInitial[i],
                        d.ValuesInitial[i]));
                }
            });
        }

        [Test]
        public void AddValues_NonExistingKey()
        {
            var map = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.IsFalse(
                        map.AddValues(d.KeysInitial[i], d.ValuesInitial[i]));
                }
            });
        }

        [Test]
        public void AddValues_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(
                () => map.AddValues(default(TKey), d.ValuesToAdd[0]));
        }

        #endregion AddValue

        #region RemoveValue

        [Test]
        public void RemoveValue_ExistingValue()
        {
            var map = d.ImplementorCtor();
            int initialCount = map.Count;

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0) { continue; }
                    var value = d.ValuesInitial[i].First();

                    Assert.True(
                        map.RemoveValue(d.KeysInitial[i], value),
                        "A successful Remove should return true.");

                    Assert.False(
                        map.ContainsKeyWithValue(d.KeysInitial[i], value),
                        "Removed value was still found in the collection.");
                }
            });
        }

        [Test]
        public void RemoveValue_NonExistingValue()
        {
            var map = d.ImplementorCtor();
            int initialCount = map.Count;
            int l = Math.Min(d.KeysInitial.Length, d.ValuesExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < l; i++)
                {
                    if (d.ValuesExcluded[i].Count <= 0) { continue; }
                    var value = d.ValuesExcluded[i].First();

                    Assert.False(
                        map.RemoveValue(d.KeysInitial[i], value),
                        "A failed Remove should return false.");
                }
            });
        }

        [Test]
        public void RemoveValue_NonExistingKey()
        {
            var map = d.ImplementorCtor();
            int initialCount = map.Count;
            int l = Math.Min(d.KeysExcluded.Length, d.ValuesInitial.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < l; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0) { continue; }
                    var value = d.ValuesInitial[i].First();

                    Assert.False(
                        map.RemoveValue(d.KeysExcluded[i], value),
                        "A failed Remove should return false.");
                }
            });
        }

        [Test]
        public void RemoveValue_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(
                () => map.RemoveValue(default(TKey), default(TVal)));
        }

        #endregion

        #region RemoveValues

        [Test]
        public void RemoveValues_NoCommonValues()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.ValuesExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    Assert.IsFalse(
                        map.RemoveValues(
                            d.KeysInitial[i],
                            d.ValuesExcluded[i]));
                }
            });
        }

        [Test]
        public void RemoveValues_SomeCommonValues()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.ValuesExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0
                    || d.ValuesExcluded[i].Count <= 0)
                    {
                        continue;
                    }

                    var initialValue = d.ValuesInitial[i].First();

                    var union = d.ValuesExcluded[i].Union(
                        AsEnumerable(initialValue),
                        d.ComparerValue);

                    Assert.IsTrue(map.RemoveValues(d.KeysInitial[i], union));

                    Assert.IsFalse(
                        map.ContainsKeyWithValue(d.KeysInitial[i], initialValue));
                }
            });
        }

        [Test]
        public void RemoveValues_OnlyCommonValues()
        {
            var map = d.ImplementorCtor();

            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0) { continue; }

                    Assert.IsTrue(map.RemoveValues(
                        d.KeysInitial[i],
                        d.ValuesInitial[i]));

                    foreach (TVal value in d.ValuesInitial[i])
                    {
                        Assert.IsFalse(
                            map.ContainsKeyWithValue(d.KeysInitial[i], value));
                    }
                }
            });
        }

        #endregion

        [Test]
        public void RemoveValuesAll()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    map.RemoveValuesAll(d.KeysInitial[i]);
                    Assert.IsFalse(map[d.KeysInitial[i]].Any());
                }
            });
        }

        [Test]
        public void ContainsValue_Initial()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.ValuesInitial.Length; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0) { continue; }
                    var value = d.ValuesInitial[i].First();
                    Assert.True(map.ContainsValue(value));
                }
            });
        }

        [Test]
        public void ContainsValue_Excluded()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.ValuesExcluded.Length; i++)
                {
                    if (d.ValuesExcluded[i].Count <= 0) { continue; }
                    var value = d.ValuesExcluded[i].First();
                    Assert.False(map.ContainsValue(value));
                }
            });
        }

        [Test]
        public void ContainsKeyWithValue_Initial()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0) { continue; }
                    var value = d.ValuesInitial[i].First();
                    Assert.True(
                        map.ContainsKeyWithValue(d.KeysInitial[i], value));
                }
            });
        }

        [Test]
        public void ContainsKeyWithValue_Initial_Excluded()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    if (d.ValuesExcluded[i].Count <= 0) { continue; }
                    var value = d.ValuesExcluded[i].First();
                    Assert.False(
                        map.ContainsKeyWithValue(d.KeysInitial[i], value));
                }
            });
        }

        [Test]
        public void ContainsKeyWithValue_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(()
                => map.ContainsKeyWithValue(default(TKey), default(TVal)));
        }

        [Test]
        public void ContainsKeyWithValues_Initial()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.True(
                        map.ContainsKeyWithValues(
                            d.KeysInitial[i],
                            d.ValuesInitial[i]));
                }
            });
        }

        [Test]
        public void ContainsKeyWithValues_Initial_Excluded()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    Assert.False(
                        map.ContainsKeyWithValues(
                            d.KeysInitial[i],
                            d.ValuesExcluded[i]));
                }
            });
        }

        [Test]
        public void ContainsKeyWithValues_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NullableSkip]);
            }

            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(()
                => map.ContainsKeyWithValue(default(TKey), default(TVal)));
        }

        [Test]
        public void ContainsKeyWithValues_NullValuesThrows()
        {
            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(()
                => map.ContainsKeyWithValues(d.KeysInitial[0], null));
        }

        [Test]
        public void Keys_Initial()
        {
            var map = d.ImplementorCtor();
            CollectionAssert.AreEquivalent(d.KeysInitial, map.Keys);
        }

        [Test]
        public void Values_Initial()
        {
            var map = d.ImplementorCtor();
            CollectionAssert.AreEquivalent(d.ValuesInitial, map.Values);
        }

        [Test]
        public void CopyTo_Initial()
        {
            var map = d.ImplementorCtor();
            var array = new KeyValuePair<TKey, ICollection<TVal>>[map.Count];
            var enumerator = map.GetEnumerator();
            map.CopyTo(array, 0);
            CollectionAssert.AreEquivalent(Expected(), array);

            IEnumerable<KeyValuePair<TKey, ICollection<TVal>>> Expected()
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        [Test]
        public void TryGetValue_Existing()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    var key = d.KeysInitial[i];
                    Assert.True(map.TryGetValue(key, out var actual));
                    CollectionAssert.AreEquivalent(d.ValuesInitial[i], actual);
                }
            });
        }

        [Test]
        public void TryGetValue_NonExisting()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysExcluded.Length; i++)
                {
                    var key = d.KeysExcluded[i];
                    Assert.False(map.TryGetValue(key, out var actual));
                    Assert.AreEqual(null, actual);
                }
            });
        }

        [Test]
        public void Replace_Existing_DifferentValue()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    map.Replace(d.KeysInitial[i], d.ValuesExcluded[i]);
                    CollectionAssert.AreEquivalent(
                        d.ValuesExcluded[i],
                        map[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Get_ExistingKey()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    CollectionAssert.AreEquivalent(
                        d.ValuesInitial[i],
                        map[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Get_NonExistingKey()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysExcluded.Length; i++)
                {
                    IEnumerable<TVal> value;
                    Assert.Throws<KeyNotFoundException>(() =>
                        value = map[d.KeysExcluded[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Set_ExistingKey()
        {
            var map = d.ImplementorCtor();
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    map[d.KeysInitial[i]] = d.ValuesExcluded[i];
                    CollectionAssert.AreEquivalent(
                        d.ValuesExcluded[i],
                        map[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Indexer_Set_NonExistingKey()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysExcluded.Length; i++)
                {
                    IEnumerable<TVal> value = new TVal[] { };
                    Assert.Throws<KeyNotFoundException>(() =>
                        map[d.KeysExcluded[i]] = value);
                }
            });
        }

        [Test]
        public void Indexer_Set_NullValueThrows()
        {
            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() =>
                map[d.KeysInitial[0]] = null);
        }

        [Test]
        public void Replace_Existing_SameValue()
        {
            var map = d.ImplementorCtor();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    map.Replace(d.KeysInitial[i], d.ValuesInitial[i]);
                    CollectionAssert.AreEquivalent(
                        d.ValuesInitial[i],
                        map[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void GetEnumerator_Initial()
        {
            var map = d.ImplementorCtor();
            var enumerator = map.GetEnumerator();

            CollectionAssert.AreEquivalent(d.KVPsInitial, Actual());

            IEnumerable<KeyValuePair<TKey, ICollection<TVal>>> Actual()
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }
    }
}
