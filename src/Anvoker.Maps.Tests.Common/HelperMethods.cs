using System;
using System.Collections.Generic;
using System.Linq;

namespace Anvoker.Maps.Tests.Common
{
    // TODO: Break this static class into less nondescript static classes once
    // there are enough methods to justify their existence. Having "misc"
    // static classes is kind of messy.

    /// <summary>
    /// Contains misc helper methods for unit testing.
    /// </summary>
    public static class HelperMethods
    {
        public static readonly Dictionary<MsgKeys, string>
            AssertMsgs =
            new Dictionary<MsgKeys, string>()
            {
                { MsgKeys.NonNullableSkip, "The key is not nullable, so this test is not applicable." }
            };

        public enum MsgKeys
        {
            /// <summary>
            /// A message detailing that this test can be skipped because it has
            /// a non-nullable element.
            /// </summary>
            NonNullableSkip
        }

        public static IEnumerable<T> AsEnumerable<T>(T item)
        {
            yield return item;
        }

        public static TV[] ConstructFlat<TV>(TV[][] arr)
        {
            int index = 0;
            int width = arr.GetLength(0);
            int height = arr.GetLength(1);
            TV[] flatArr = new TV[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    flatArr[index] = arr[x][y];
                    index++;
                }
            }

            return flatArr;
        }

        /// <summary>
        /// Builds an array of key-value pairs from the specified arrays of keys
        /// and values.
        /// </summary>
        /// <typeparam name="TK">The type of the key.</typeparam>
        /// <typeparam name="TV">The type of the value.</typeparam>
        /// <param name="keys">An array of keys.</param>
        /// <param name="vals">An array of values.</param>
        /// <returns>An array of key-value pairs with the specified keys and
        /// values.</returns>
        public static KeyValuePair<TK, TV>[] ConstructKVPs<TK, TV>(
            TK[] keys,
            TV[] vals)
        {
            var kvps = new KeyValuePair<TK, TV>[keys.Length];

            if (vals != null)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    kvps[i] = new KeyValuePair<TK, TV>(
                        keys[i], vals[i]);
                }
            }
            else
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    kvps[i] = new KeyValuePair<TK, TV>(
                        keys[i], default(TV));
                }
            }

            return kvps;
        }

        /// <summary>
        /// Instantiates an array of collections where each collection is
        /// constructed from a pairwise union between the collections in the
        /// <paramref name="first"/> and <paramref name="second"/> arrays.
        /// </summary>
        /// <typeparam name="TCollection">Type of the collection.
        /// </typeparam>
        /// <typeparam name="T">Type of the element in the collection.
        /// </typeparam>
        /// <param name="first">The first array of collections.</param>
        /// <param name="second">The second array of collections.</param>
        /// <param name="comparer">The comparer for
        /// <typeparamref name="T"/></param>
        /// <param name="collectionConstructor">A delegate pointing to a
        /// constructor of <typeparamref name="TCollection"/> that takes
        /// <see cref="IEnumerable{T}"/> as its parameter.</param>
        /// <returns>An array with new collections, created from pairwise
        /// unions.</returns>
        public static TCollection[] UnionValues<TCollection, T>(
            TCollection[] first,
            TCollection[] second,
            IEqualityComparer<T> comparer,
            Func<IEnumerable<T>, TCollection> collectionConstructor)
            where TCollection : IEnumerable<T>
        {
            TCollection[] longer;
            TCollection[] shorter;

            if (first.Length > second.Length)
            {
                longer = first;
                shorter = second;
            }
            else
            {
                longer = second;
                shorter = first;
            }

            var values = new TCollection[longer.Length];
            for (int i = 0; i < longer.Length; i++)
            {
                if (i < shorter.Length)
                {
                    values[i] = collectionConstructor(
                            longer[i].Union(shorter[i], comparer));
                }
                else
                {
                    values[i] = longer[i];
                }
            }

            return values;
        }
    }
}