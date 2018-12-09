using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Anvoker.Collections.Maps;
using static Anvoker.Collections.Tests.Common.HelperMethods;
using System.Collections;

namespace Anvoker.Collections.Tests.Common
{
    public class IMultiMapBase<TKey, TVal, TMultiMap, TValCol>
        where TMultiMap : IMultiMap<TKey, TVal>
        where TValCol : ICollection<TVal>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly MapTestDataConcrete<TKey, TVal, TMultiMap, TValCol> d;

        private readonly string nullabeSkipMsg
            = "The key is not nullable, so this test is not applicable.";

        public IMultiMapBase(
            MapTestDataConcrete<TKey, TVal, TMultiMap, TValCol> args)
        {
            d = args;
        }

        /// <summary>
        /// Constructs the parameter class required for instantiating this
        /// test fixture.
        /// </summary>
        /// <param name="ctorImplementor">Delegate pointing to a
        /// parameterless constructor of <typeparamref name="TMultiMap"/>.
        /// </param>
        /// <param name="ctorTValCol">Delegate pointing to a constructor that
        /// takes <see cref="IEnumerable{T}"/> and returns a new value
        /// map <typeparamref name="TValCol"/></param>
        /// <param name="data">The concrete test data.</param>
        /// <param name="testName">Name displayed in the test runner.</param>
        /// <returns>A new instance of <see cref="TestFixtureParameters"/> that
        /// can be used to instantiate a
        /// <see cref="IMultiMapBase{TKey, TVal, TMultiMap, TValCol}"/>
        /// fixture.</returns>
        public static TestFixtureParameters
            ConstructFixtureParams(
            Func<TMultiMap> ctorImplementor,
            Func<IEnumerable<TVal>, TValCol> ctorTValCol,
            MapTestData<TKey, TVal> data,
            string testName)
        {
            var args = new MapTestDataConcrete<TKey, TVal, TMultiMap, TValCol>(
                ctorImplementor, ctorTValCol, data);
            var exposedParams = new ExposedTestFixtureParams()
            {
                TestName = testName,
                Arguments = new object[] { args },
                Properties = new PropertyBag(),
                RunState = RunState.Runnable,
                TypeArgs = new Type[]
                {
                    typeof(TKey),
                    typeof(TVal),
                    typeof(TMultiMap),
                    typeof(TValCol)
                }
            };

            return new TestFixtureParameters(exposedParams);
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
            if (d.KeyIsNullabe)
            {
                var map = d.ImplementorCtor();

                Assert.Throws<ArgumentNullException>(() =>
                    map.AddKey(default(TKey)));
            }
            else
            {
                Assert.False(d.KeyIsNullabe, nullabeSkipMsg);
            }
        }

        [Test]
        public void AddKey_KeyIEnumerable_NullKeyThrows()
        {
            if (d.KeyIsNullabe)
            {
                var map = d.ImplementorCtor();

                Assert.Throws<ArgumentNullException>(() =>
                    map.AddKey(default(TKey), d.ValuesToAdd[0]));
            }
            else
            {
                Assert.False(d.KeyIsNullabe, nullabeSkipMsg);
            }
        }

        [Test]
        public void AddKey_KeyHashSet_NullKeyThrows()
        {
            if (d.KeyIsNullabe)
            {
                var map = d.ImplementorCtor();
                var hashSet = new HashSet<TVal>(d.ValuesToAdd[0]);

                Assert.Throws<ArgumentNullException>(() =>
                    map.AddKey(default(TKey), hashSet));
            }
            else
            {
                Assert.False(d.KeyIsNullabe, nullabeSkipMsg);
            }
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
            if (d.KeyIsNullabe)
            {
                var map = d.ImplementorCtor();
                var value = d.ValuesToAdd[0].First();

                Assert.Throws<ArgumentNullException>(
                    () => map.AddValue(default(TKey), value));
            }
            else
            {
                Assert.False(d.KeyIsNullabe, nullabeSkipMsg);
            }
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
                        $"Value: {union.Aggregate("", (string str, TVal x) => x.ToString() + ", ")}");
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
            if (d.KeyIsNullabe)
            {
                var map = d.ImplementorCtor();

                Assert.Throws<ArgumentNullException>(
                    () => map.AddValues(default(TKey), d.ValuesToAdd[0]));
            }
            else
            {
                Assert.False(d.KeyIsNullabe, nullabeSkipMsg);
            }
        }
        #endregion AddValue
    }
}
