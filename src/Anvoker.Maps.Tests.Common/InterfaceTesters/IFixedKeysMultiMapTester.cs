using System;
using System.Collections.Generic;
using System.Linq;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static Anvoker.Maps.Tests.Common.HelperMethods;

namespace Anvoker.Maps.Tests.Common
{
    public class IFixedKeysMultiMapTester<TK, TV, TFKMMap, TVCol>
        where TFKMMap : IFixedKeysMultiMap<TK, TV>
        where TVCol : IReadOnlyCollection<TV>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValuesData<TK, TV, TFKMMap, TVCol> d;

        private TFKMMap map;

        public IFixedKeysMultiMapTester(
            IKeyValuesData<TK, TV, TFKMMap, TVCol> args)
        {
            d = args;
        }

        [SetUp]
        public void Setup() => map = d.ImplementorCtor();

        #region AddValue

        [Test]
        public void AddValue_ExistingKey_ExistingValue()
        {
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
        public void AddValue_ExistingKey_NonExistingValue()
        {
            var length = Math.Min(d.KeysInitial.Length, d.ValuesExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    if (d.ValuesToAdd[i].Count <= 0) { continue; }
                    var value = d.ValuesExcluded[i].First();
                    var expectedUnion = d.ValuesInitial[i].Union(
                        AsEnumerable(value),
                        d.ComparerValue);
                    var actualUnion = map[d.KeysInitial[i]];

                    Assert.IsTrue(map.AddValue(d.KeysInitial[i], value));
                    CollectionAssert.AreEquivalent(expectedUnion, actualUnion);
                }
            });
        }

        [Test]
        public void AddValue_NonExistingKey()
        {
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
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(
                () => map.AddValue(default(TK), default(TV)));
        }

        #endregion AddValue

        #region AddValues

        [Test]
        public void AddValues_ExistingKey_NoCommonValues()
        {
            var length = Math.Min(d.KeysInitial.Length, d.ValuesExcluded.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    var expectedUnion = d.ValuesInitial[i].Union(
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

                    var actualUnion = map[d.KeysInitial[i]];
                    CollectionAssert.AreEquivalent(expectedUnion, actualUnion);
                }
            });
        }

        [Test]
        public void AddValues_ExistingKey_OnlyCommonValues()
        {
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
        public void AddValues_ExistingKey_SomeCommonValues()
        {
            var length = Math.Min(d.KeysInitial.Length, d.ValuesToAdd.Length);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    var ToAdd = d.ValuesToAdd[i].Union(d.ValuesInitial[i], d.ComparerValue);
                    var expectedUnion = d.ValuesInitial[i].Union(ToAdd, d.ComparerValue);
                    Assert.AreEqual(
                        expectedUnion.Count() > d.ValuesInitial[i].Count,
                        map.AddValues(d.KeysInitial[i], expectedUnion));
                    var actualUnion = map[d.KeysInitial[i]];
                    CollectionAssert.AreEquivalent(expectedUnion, actualUnion);
                }
            });
        }

        [Test]
        public void AddValues_NonExistingKey()
        {
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
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(
                () => map.AddValues(
                    default(TK),
                    AsEnumerable(default(TV))));
        }

        #endregion AddValues

        #region RemoveValue

        [Test]
        public void RemoveValue_ExistingValue()
        {
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

                    var remainingValues = map[d.KeysInitial[i]];
                    CollectionAssert.IsEmpty(
                        AsEnumerable(value).Intersect(remainingValues),
                        "Removed value was still found in the collection.");
                }
            });
        }

        [Test]
        public void RemoveValue_NonExistingKey()
        {
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
        public void RemoveValue_NonExistingValue()
        {
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
        public void RemoveValue_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(
                () => map.RemoveValue(default(TK), default(TV)));
        }

        #endregion RemoveValue

        #region RemoveValues

        [Test]
        public void RemoveValues_NoCommonValues()
        {
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
        public void RemoveValues_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(
                () => map.RemoveValues(default(TK), AsEnumerable(default(TV))));
        }

        [Test]
        public void RemoveValues_OnlyCommonValues()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    if (d.ValuesInitial[i].Count <= 0) { continue; }
                    var valuesToRemove = d.ValuesInitial[i];

                    Assert.IsTrue(map.RemoveValues(
                        d.KeysInitial[i],
                        valuesToRemove));

                    var remainingValues = map[d.KeysInitial[i]];
                    CollectionAssert.IsEmpty(valuesToRemove.Intersect(remainingValues));
                }
            });
        }

        [Test]
        public void RemoveValues_SomeCommonValues()
        {
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
                    var valuesToRemove = d.ValuesExcluded[i].Union(
                        AsEnumerable(initialValue),
                        d.ComparerValue);

                    Assert.IsTrue(map.RemoveValues(d.KeysInitial[i], valuesToRemove));
                    var remainingValues = map[d.KeysInitial[i]];
                    CollectionAssert.IsEmpty(valuesToRemove.Intersect(remainingValues));
                }
            });
        }

        #endregion RemoveValues

        #region RemoveValuesAll

        [Test]
        public void RemoveValuesAll()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    map.RemoveValuesAll(d.KeysInitial[i]);
                    Assert.IsFalse(map[d.KeysInitial[i]].Count > 0);
                }
            });
        }

        [Test]
        public void RemoveValuesAll_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(
                () => map.RemoveValuesAll(default(TK)));
        }

        #endregion RemoveValuesAll

        #region Replace

        [Test]
        public void Replace_DifferentValue()
        {
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
        public void Replace_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(
                () => map.Replace(default(TK), AsEnumerable(default(TV))));
        }

        [Test]
        public void Replace_SameValue()
        {
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

        #endregion Replace
    }
}