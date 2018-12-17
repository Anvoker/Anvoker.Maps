using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Common
{
    /// <summary>
    /// Provides functionality for testing whether a class correctly implements
    /// a <see cref="IReadOnlyDictionary<TKey, TValCol>"/> with a nested
    /// collection as the value type.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.
    /// <typeparam name="TVal"> Type of the values used in
    /// <see cref="TValCol"/>.</typeparam>
    /// <typeparam name="TIDict">Type of the class implementing
    /// <see cref="IReadOnlyDictionary{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TValCol">Type of the nested collection used as the
    /// value type in <see cref="IDictionary{TKey, TValue}"/>.</typeparam>
    public class IReadOnlyDictionaryNestedBase<TKey, TVal, TIDict, TValCol> :
        MapTestDataConstructible<TKey, TVal, TIDict, TValCol>
        where TIDict : IReadOnlyDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly MapTestDataConcrete<TKey, TVal, TIDict, TValCol> d;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IDictionaryNestedBase{TKey, TVal, TIDict, TValCol}"/>
        /// class with the specified collection that implements
        /// <typeparamref name="TIDict"/> with the specified keys and
        /// values, and with matching test data.
        /// </summary>
        /// <param name="args">A data class containing all of the necessary
        /// arguments for initializing the tests.</param>
        public IReadOnlyDictionaryNestedBase(
            MapTestDataConcrete<TKey, TVal, TIDict, TValCol> args)
        {
            d = args;
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
        public void Count_InitialValueIsCorrect()
        {
            var collection = d.ImplementorCtor();
            Assert.AreEqual(d.KeysInitial.Length, collection.Count);
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
    }
}