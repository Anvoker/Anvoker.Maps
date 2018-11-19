using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable RCS1169 // Mark field as read-only.
#pragma warning disable RCS1227 // Validate arguments correctly.
#pragma warning disable RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.
#pragma warning disable IDE0020 // Use pattern matching
#pragma warning disable IDE0034 // Simplify 'default' expression
#pragma warning disable IDE0027 // Use expression body for accessors

namespace Anvoker.Collections.Maps
{
    /// <content>
    /// Contains <see cref="BiMap{TKey, TVal}.ValueCollection"/>
    /// implementation.
    /// </content>
    public partial class BiMap<TKey, TVal>
    {
        /// <summary>
        /// Represents the collection of values in a
        /// <see cref="BiMap{TKey, TVal}"/> This class cannot be inherited.
        /// </summary>
        public sealed class ValueCollection : ICollection<TVal>,
            IReadOnlyCollection<TVal>
        {
            private BiMap<TKey, TVal> bimap;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValueCollection"/>
            /// class that reflects the values in the specified
            /// <see cref="BiMap{TKey, TVal}"/>.
            /// Represents the collection of values in a
            /// <see cref="BiMap{TKey, TVal}"/> This class cannot be inherited.
            /// </summary>
            /// <param name="bimap">The <see cref="BiMap{TKey, TVal}"/>
            /// this <see cref="ValueCollection"/> is referencing.</param>
            public ValueCollection(BiMap<TKey, TVal> bimap)
            {
                this.bimap = bimap;
            }

            /// <summary>
            /// Gets the number of values contained in the
            /// <see cref="BiMap{TKey, TVal}"/>
            /// </summary>
            public int Count => bimap.dictFwd.Count;

            /// <summary>
            /// Gets a value indicating whether the value collection is
            /// read-only.
            /// </summary>
            public bool IsReadOnly => true;

            /// <summary>
            /// Values cannot be added to the
            /// <see cref="BiMap{TKey, TVal}"/> via its
            /// <see cref="ValueCollection"/>. Using this method will throw
            /// <see cref="NotSupportedException"/>.
            /// </summary>
            /// <param name="item">The value of the element to add.</param>
            void ICollection<TVal>.Add(TVal item)
                => throw new NotSupportedException();

            /// <summary>
            /// Values cannot be cleared from the
            /// <see cref="BiMap{TKey, TVal}"/> via its
            /// <see cref="ValueCollection"/>. Using this method will throw
            /// <see cref="NotSupportedException"/>.
            /// </summary>
            void ICollection<TVal>.Clear()
                => throw new NotSupportedException();

            /// <summary>
            /// Determines whether the <see cref="BiMap{TKey, TVal}"/> contains
            /// a specific value.
            /// </summary>
            /// <param name="item">The value to locate in the
            /// <see cref="BiMap{TKey, TVal}"/>. The value can be null for
            /// reference types.</param>
            /// <returns>true if the <see cref="BiMap{TKey, TVal}"/> contains an
            /// element with the specified value; otherwise, false.</returns>
            public bool Contains(TVal item) => bimap.ContainsValue(item);

            /// <summary>
            /// Copies the values of the <see cref="BiMap{TKey, TVal}"/> to an
            /// array, starting at the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional array that is the
            /// destination of the values copied from
            /// <see cref="BiMap{TKey, TVal}"/>. The array must have
            /// zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which
            /// copying begins.</param>
            public void CopyTo(TVal[] array, int arrayIndex)
                => bimap.dictFwd.Values.CopyTo(array, arrayIndex);

            /// <summary>
            /// Returns an enumerator that iterates through the values
            /// of the <see cref="BiMap{TKey, TVal}"/>.
            /// </summary>
            /// <returns>A ValueCollection enumerator structure for the
            /// <see cref="BiMap{TKey, TVal}"/>.</returns>
            public IEnumerator<TVal> GetEnumerator()
                => bimap.dictFwd.Values.GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through the values
            /// of the <see cref="BiMap{TKey, TVal}"/>.
            /// </summary>
            /// <returns>A ValueCollection enumerator structure for the
            /// <see cref="BiMap{TKey, TVal}"/>.</returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            /// Values cannot be removed from the
            /// <see cref="BiMap{TKey, TVal}"/> via its
            /// <see cref="ValueCollection"/>. Using this method will throw
            /// <see cref="NotSupportedException"/>.
            /// </summary>
            /// <param name="item">The value of the element to remove.</param>
            /// <returns>Never returns. Always throws
            /// <see cref="NotSupportedException"/>.</returns>
            bool ICollection<TVal>.Remove(TVal item)
                => throw new NotSupportedException();
        }
    }
}