using System;
using System.Collections.Generic;
using System.Linq;
using Anvoker.Collections.Maps;
using Anvoker.Collections.Tests.Common.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Common
{
    public class IMultiMapTester<TK, TV, TMMap, TVCol>
        where TMMap : IMultiMap<TK, TV>
        where TVCol : IReadOnlyCollection<TV>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValuesData<TK, TV, TMMap, TVCol> d;

        private TMMap map;

        public IMultiMapTester(
            IKeyValuesData<TK, TV, TMMap, TVCol> args)
        {
            d = args;
        }

        [SetUp]
        public void Setup() => map = d.ImplementorCtor();

        [Test]
        public void Clear()
        {
            map.Clear();
            Assert.AreEqual(0, map.Count);
        }

        [Test]
        public void Add_NoValue()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    map.Add(d.KeysToAdd[i]);
                    Assert.IsTrue(map.ContainsKey(d.KeysToAdd[i]));
                }
            });
        }

        [Test]
        public void Add_NoValue_ExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.Throws<ArgumentException>(()
                        => map.Add(d.KeysInitial[i]));
                }
            });
        }

        [Test]
        public void Add_NoValue_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(() => map.Add(default(TK)));
        }

        [Test]
        public void Add_SingleValue()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    if (d.ValuesToAdd[i].Count <= 0) { continue; }
                    var value = d.ValuesToAdd[i].First();
                    map.Add(d.KeysToAdd[i], value);
                    Assert.AreEqual(value, map[d.KeysToAdd[i]].First());
                }
            });
        }

        [Test]
        public void Add_SingleValue_ExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.Throws<ArgumentException>(()
                        => map.Add(d.KeysInitial[i], default(TV)));
                }
            });
        }

        [Test]
        public void Add_SingleValue_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(() =>
                map.Add(default(TK), d.ValuesToAdd[0].First()));
        }

        [Test]
        public void Add_KeyIEnumerable()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysToAdd.Length; i++)
                {
                    map.Add(d.KeysToAdd[i], d.ValuesToAdd[i]);
                    Assert.AreEqual(d.ValuesToAdd[i], map[d.KeysToAdd[i]]);
                }
            });
        }

        [Test]
        public void Add_KeyIEnumerable_ExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.Throws<ArgumentException>(()
                        => map.Add(d.KeysInitial[i], AsEnumerable(default(TV))));
                }
            });
        }

        [Test]
        public void Add_KeyIEnumerable_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(() =>
                map.Add(default(TK), d.ValuesToAdd[0]));
        }

        [Test]
        public void Remove()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    Assert.IsTrue(map.Remove(d.KeysInitial[i]));
                    Assert.IsFalse(map.ContainsKey(d.KeysInitial[i]));
                }
            });
        }

        [Test]
        public void Remove_NonExistingKey()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysExcluded.Length; i++)
                {
                    Assert.IsFalse(map.Remove(d.KeysExcluded[i]));
                }
            });
        }

        [Test]
        public void Remove_NullKeyThrows()
        {
            if (!d.KeyIsNullabe)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            var map = d.ImplementorCtor();
            Assert.Throws<ArgumentNullException>(() => map.Remove(default(TK)));
        }
    }
}