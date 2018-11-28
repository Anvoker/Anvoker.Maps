using System;
using System.Collections.Generic;
using System.Linq;

namespace Anvoker.Tests.Collections
{
    // TODO: Break this static class into less nondescript static classes once
    // there are enough methods to justify their existence. Having "misc"
    // static classes is kind of messy.

    /// <summary>
    /// Contains misc helper methods for unit testing.
    /// </summary>
    public static class HelperMethods
    {
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
    }
}