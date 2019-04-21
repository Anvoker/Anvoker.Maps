using System;
using System.Collections.Generic;
using Anvoker.Collections.Tests.Common.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Common
{
    public class ICollectionTester<T, TCollection>
        where TCollection : ICollection<T>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IValueData<T, TCollection> d;

        private TCollection collection;

        public ICollectionTester(IValueData<T, TCollection> args)
        {
            d = args;
        }

        [SetUp]
        public void Setup() => collection = d.ImplementorCtor();

        [Test]
        public void Add_Existing()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.ValuesInitial.Length; i++)
                {
                    Assert.Throws<ArgumentException>(()
                        => collection.Add(d.ValuesInitial[i]));
                }
            });
        }

        [Test]
        public void Add_NonExisting()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.ValuesToAdd.Length; i++)
                {
                    collection.Add(d.ValuesToAdd[i]);
                    Assert.IsTrue(collection.Contains(d.ValuesToAdd[i]));
                }
            });
        }

        [Test]
        public void Add_NullThrows()
        {
            if (default(T) != null)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(()
                => collection.Add(default(T)));
        }

        [Test]
        public void Clear_SetsCountToZero()
        {
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Contains_Initial()
        {
            Assert.Multiple(() =>
            {
                foreach (var value in d.ValuesInitial)
                {
                    Assert.IsTrue(collection.Contains(value));
                }
            });
        }

        [Test]
        public void Contains_NoExcluded()
        {
            Assert.Multiple(() =>
            {
                foreach (var value in d.ValuesExcluded)
                {
                    Assert.IsFalse(collection.Contains(value));
                }
            });
        }

        [Test]
        public void Count_Initial()
            => Assert.AreEqual(d.ValuesInitial.Length, collection.Count);

        [Test]
        public void Count_InitialValueIsCorrect()
            => Assert.AreEqual(d.ValuesInitial.Length, collection.Count);

        [Test]
        public void Remove_Key_Existing()
        {
            int initialCount = collection.Count;

            Assert.Multiple(() =>
            {
                Assert.True(
                    collection.Remove(d.ValuesInitial[0]),
                    "A successful Remove should return true.");

                Assert.AreEqual(
                    collection.Count,
                    initialCount - 1,
                    "A successful Remove should decrement the count.");

                Assert.False(
                    collection.Contains(d.ValuesInitial[0]),
                    "Removed key was still found in the collection.");
            });
        }

        public void Remove_Key_NonExisting()
        {
            var initialCount = collection.Count;

            Assert.Multiple(() =>
            {
                Assert.False(
                    collection.Remove(d.ValuesExcluded[0]),
                    "A failed Remove should return false.");

                Assert.AreEqual(
                    collection.Count,
                    initialCount,
                    "A failed Remove should not modify the count.");

                Assert.True(
                    collection.Contains(d.ValuesExcluded[0]),
                    "A failed Remove's target key should still be contained.");
            });
        }

        [Test]
        public void Remove_Key_NullKeyThrows()
        {
            if (default(T) != null)
            {
                Assert.Pass(AssertMsgs[MsgKeys.NonNullableSkip]);
            }

            Assert.Throws<ArgumentNullException>(()
                => collection.Remove(default(T)));
        }
    }
}