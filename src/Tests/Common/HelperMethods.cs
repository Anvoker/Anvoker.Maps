using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using NUnit.Framework.Interfaces;

namespace Anvoker.Collections.Tests.Common
{
    // TODO: Break this static class into less nondescript static classes once
    // there are enough methods to justify their existence. Having "misc"
    // static classes is kind of messy.

    /// <summary>
    /// Contains misc helper methods for unit testing.
    /// </summary>
    public static class HelperMethods
    {
        public enum MsgKeys
        {
            NullableSkip
        }

        public readonly static Dictionary<MsgKeys, string>
            AssertMsgs =
            new Dictionary<MsgKeys, string>()
            {
                { MsgKeys.NullableSkip, "The key is not nullable, so this test is not applicable." }
            };

        /// <summary>
        /// Pretty prints a key-value pair.
        /// </summary>
        /// <typeparam name="T">The type of the key.</typeparam>
        /// <typeparam name="K">The type of the value.</typeparam>
        /// <param name="kvp">The key-value pair to pretty print.</param>
        /// <returns>A text representation of the specified key-value pair.
        /// </returns>
        public static string KVPToString<T, K>(KeyValuePair<T, K> kvp)
        {
            string strT;
            if (kvp.Key is IEnumerable keyEnum)
            {
                strT = $"{typeof(T).Name}{{ ";
                bool ran = false;
                foreach (object keyItem in keyEnum)
                {
                    strT += keyItem.ToString() + ", ";
                    ran = true;
                }

                if (ran)
                {
                    strT = strT.Remove(strT.Length - 2, 2);
                }

                strT += " }";
            }
            else
            {
                if (kvp.Key != null)
                {
                    strT = kvp.Key.ToString();
                }
                else
                {
                    strT = "null";
                }
            }

            string strK;
            if (kvp.Value is IEnumerable valEnum)
            {
                strK = $"{typeof(K).Name}{{ ";
                bool ran = false;
                foreach (object valItem in valEnum)
                {
                    strK += valItem.ToString() + ", ";
                    ran = true;
                }

                if (ran)
                {
                    strT = strT.Remove(strT.Length - 2, 2);
                }

                strK += " }";
            }
            else
            {
                if (kvp.Value != null)
                {
                    strK = kvp.Value.ToString();
                }
                else
                {
                    strK = "null";
                }
            }

            return $"{{ Key: {strT} | Value: {strK} }}";
        }

        public static string ObjectsToString(
            Func<object, string>[] toStringMethods,
            IEnumerable args,
            string delimiter = ", ")
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var sb = new StringBuilder();

            int i = 0;
            foreach (object arg in args)
            {
                if (toStringMethods?[i] != null)
                {
                    sb.Append(toStringMethods[i](arg)).Append(delimiter);
                }
                else
                {
                    sb.Append(arg.ToString()).Append(delimiter);
                }

                i++;
            }

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }

        /// <summary>
        /// Instantiates an array of collections where each collection is
        /// constructed from a pairwise union between the collections in the
        /// <paramref name="first"/> and <paramref name="second"/> arrays.
        /// </summary>
        /// <typeparam name="TCollection">Type of the collection.
        /// </typeparam>
        /// <typeparam name="TValue">Type of the element in the collection.
        /// </typeparam>
        /// <param name="first">The first array of collections.</param>
        /// <param name="second">The second array of collections.</param>
        /// <param name="comparer">The comparer for
        /// <typeparamref name="TValue"/></param>
        /// <param name="collectionConstructor">A delegate pointing to a
        /// constructor of <typeparamref name="TCollection"/> that takes
        /// <see cref="IEnumerable{T}"/> as its parameter.</param>
        /// <returns>An array with new collections, created from pairwise
        /// unions.</returns>
        public static TCollection[] UnionValues<TCollection, TValue>(
            TCollection[] first,
            TCollection[] second,
            IEqualityComparer<TValue> comparer,
            Func<IEnumerable<TValue>, TCollection> collectionConstructor)
            where TCollection : IEnumerable<TValue>
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

        /// <summary>
        /// Instantiates an array of collections where each collection is
        /// constructed from a pairwise union between the collections in the
        /// <paramref name="first"/> and <paramref name="second"/> arrays.
        /// </summary>
        /// <typeparam name="TCollection">Type of the collection.
        /// </typeparam>
        /// <typeparam name="TValue">Type of the element in the collection.
        /// </typeparam>
        /// <param name="first">The first array of collections.</param>
        /// <param name="second">The second array of collections.</param>
        /// <param name="comparer">The comparer for
        /// <typeparamref name="TValue"/></param>
        /// <param name="collectionConstructor">A delegate pointing to a
        /// constructor of <typeparamref name="TCollection"/> that takes
        /// <see cref="IEnumerable{T}"/> as its parameter.</param>
        /// <returns>An array with new collections, created from pairwise
        /// unions.</returns>
        public static TCollection[] UnionValues<TCollection, TValue>(
            TCollection[] first,
            IEnumerable<TValue> second,
            Func<IEnumerable<TValue>, TCollection> collectionConstructor)
            where TCollection : IEnumerable<TValue>
        {
            var values = new TCollection[first.Length];
            var enumerator = second.GetEnumerator();
            for (int i = 0; i < first.Length; i++, enumerator.MoveNext())
            {
                var union = new List<TValue>(first[i]);
                union.Add(enumerator.Current);
                values[i] = collectionConstructor(union);
            }

            return values;
        }

        public static object[] CombineArraysFromSources(
            ITestFixtureData[][] testFixtureSources)
        {
            var list = new List<object>();
            foreach (ITestFixtureData[] fixtureDataArray in testFixtureSources)
            {
                foreach (ITestFixtureData fixtureData in fixtureDataArray)
                {
                    list.Add(fixtureData);
                }
            }

            return list.ToArray();
        }

        public static IEnumerable<T> AsEnumerable<T> (T item)
        {
            yield return item;
        }

        /// <summary>
        /// Uses the provided method to build a string array containing the
        /// text representation of all of the specified elements.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="itemToString">Method used to get the text
        /// representation of an element.</param>
        /// <param name="items">A collection of elements.</param>
        /// <returns>The text representation of the specified elements.
        /// </returns>
        public static string[] ItemsToString<T>(
        Func<T, string> itemToString,
        T[] items)
        {
            var strElements = new string[items.Length];
            if (itemToString != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    strElements[i] = itemToString(items[i]);
                }
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    strElements[i] = items[i].ToString();
                }
            }

            return strElements;
        }

        /// <summary>
        /// Uses the provided method to build a string array containing the
        /// text representation of all of the specified elements.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="ToStringMethod">Method used to get the text
        /// representation of an element.</param>
        /// <param name="items">A collection of elements.</param>
        /// <returns>The text representation of the specified elements.
        /// </returns>
        public static string[] ItemsToString<T>(
            Func<T, string>[] ToStringMethod,
            T[][] items)
        {
            var sb = new StringBuilder();

            var strElements = new string[items.Length];
            if (ToStringMethod != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    sb.Clear();
                    for (int j = 0; j < items.Length; j++)
                    {
                        sb.Append(ToStringMethod[i](items[i][j]));
                        sb.Append(", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    strElements[i] = sb.ToString();
                }
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    sb.Clear();
                    for (int j = 0; j < items.Length; j++)
                    {
                        sb.Append(items[i][j]);
                        sb.Append(", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    strElements[i] = sb.ToString();
                }
            }

            return strElements;
        }

        /// <summary>
        /// Builds an array of key-value pairs from the specified arrays of keys
        /// and values.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TVal">The type of the value.</typeparam>
        /// <param name="keys">An array of keys.</param>
        /// <param name="vals">An array of values.</param>
        /// <returns>An array of key-value pairs with the specified keys and
        /// values.</returns>
        public static KeyValuePair<TKey, TVal>[] ConstructKVPs<TKey, TVal>(
            TKey[] keys,
            TVal[] vals)
        {
            var kvps = new KeyValuePair<TKey, TVal>[keys.Length];

            if (vals != null)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    kvps[i] = new KeyValuePair<TKey, TVal>(
                        keys[i], vals[i]);
                }
            }
            else
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    kvps[i] = new KeyValuePair<TKey, TVal>(
                        keys[i], default(TVal));
                }
            }

            return kvps;
        }
    }
}