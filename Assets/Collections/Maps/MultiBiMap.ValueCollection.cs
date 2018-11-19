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
    /// Contains <see cref="MultiBiMap{TKey, TVal}.ValueCollection"/>
    /// implementation.
    /// </content>
    public partial class MultiBiMap<TKey, TVal>
    {
        /// <summary>
        /// Represents the collection of value collections in a
        /// <see cref="MultiBiMap{TKey, TVal}"/> This class cannot be inherited.
        /// </summary>
        public sealed class ValueCollection : ICollection<ICollection<TVal>>,
            IReadOnlyCollection<ICollection<TVal>>
        {
            private MultiBiMap<TKey, TVal> mbimap;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValueCollection"/>
            /// class that reflects the values in the specified
            /// <see cref="MultiBiMap{TKey, TVal}"/>.        /// <summary>
            /// Represents the collection of values in a
            /// <see cref="MultiBiMap{TKey, TVal}"/> This class cannot be inherited.
            /// </summary>
            /// </summary>
            /// <param name="mbimap">The <see cref="MultiBiMap{TKey, TVal}"/>
            /// this <see cref="ValueCollection"/> is referencing.</param>
            public ValueCollection(MultiBiMap<TKey, TVal> mbimap)
            {
                this.mbimap = mbimap;
            }

            /// <summary>
            /// Gets the number of values contained in the
            /// <see cref="MultiBiMap{TKey, TVal}"/>
            /// </summary>
            public int Count => mbimap.dictFwd.Count;

            /// <summary>
            /// Gets a value indicating whether the
            /// <see cref="ValueCollection"/> is read-only.
            /// </summary>
            public bool IsReadOnly => false;

            /// <summary>
            /// Values cannot be added to the
            /// <see cref="MultiBiMap{TKey, TVal}"/> via its
            /// <see cref="ValueCollection"/>. Using this method will throw
            /// <see cref="NotSupportedException"/>.
            /// </summary>
            /// <param name="item">The value of the element to add.</param>
            public void Add(ICollection<TVal> item)
                => throw new NotSupportedException();

            /// <summary>
            /// Removes all of the values from every key in the
            /// <see cref="MultiBiMap{TKey, TVal}"/> leaving each key with an
            /// empty value collection.
            /// </summary>
            public void Clear()
            {
                foreach (TKey key in mbimap.Keys)
                {
                    mbimap.RemoveValuesAll(key);
                }
            }

            /// <summary>
            /// Determines whether the <see cref="MultiBiMap{TKey, TVal}"/>
            /// contains any element with the specified value collection, using
            /// reference equality comparison.
            /// </summary>
            /// <param name="item">The values to locate in the
            /// <see cref="MultiBiMap{TKey, TVal}"/>.</param>
            /// <returns>true if the <see cref="MultiBiMap{TKey, TVal}"/>
            /// contains an element with the specified value collection;
            /// otherwise, false.
            /// </returns>
            public bool Contains(ICollection<TVal> item)
                => mbimap.TryGetKeyByCollectionRef(
                    (IReadOnlyCollection<TVal>)item, out TKey key);

            /// <summary>
            /// Determines whether the <see cref="MultiBiMap{TKey, TVal}"/>
            /// contains any element with the specified value collection, using
            /// set equality comparison.
            /// </summary>
            /// <param name="item">The values to locate in the
            /// <see cref="MultiBiMap{TKey, TVal}"/>.</param>
            /// <returns>true if the <see cref="MultiBiMap{TKey, TVal}"/>
            /// contains an element with the specified value collection;
            /// otherwise, false.
            /// </returns>
            public bool ContainsSet(ICollection<TVal> item)
                => mbimap.TryGetKeyByCollectionRef(
                    (IReadOnlyCollection<TVal>)item, out TKey key);

            /// <summary>
            /// Copies the values of the <see cref="MultiBiMap{TKey, TVal}"/>,
            /// grouped by their respective elements, to an array, starting at
            /// the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional array that is the
            /// destination of the values copied from
            /// <see cref="MultiBiMap{TKey, TVal}"/>. The array must have
            /// zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which
            /// copying begins.</param>
            public void CopyTo(ICollection<TVal>[] array, int arrayIndex)
                => ((ICollection<ICollection<TVal>>)(ICollection<HashSet<TVal>>)
                mbimap.dictFwd.Values).CopyTo(array, arrayIndex);

            /// <summary>
            /// Returns an enumerator that iterates through the values
            /// of the <see cref="MultiBiMap{TKey, TVal}"/>, grouped by their
            /// respective elements.
            /// </summary>
            /// <returns>A ValueCollection enumerator structure for the
            /// <see cref="MultiBiMap{TKey, TVal}"/>.</returns>
            public IEnumerator<ICollection<TVal>> GetEnumerator()
                => mbimap.dictFwd.Values.GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through the values
            /// of the <see cref="MultiBiMap{TKey, TVal}"/>, grouped by their
            /// respective elements.
            /// </summary>
            /// <returns>A ValueCollection enumerator structure for the
            /// <see cref="MultiBiMap{TKey, TVal}"/>.</returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            /// Removes a value collection from the
            /// <see cref="MultiBiMap{TKey, TVal}"/>, locating it by reference
            /// equality comparison. The value collection will be emptied and
            /// replaced with a new collection instance.
            /// </summary>
            /// <param name="item">The value collection to remove from the
            /// element.</param>
            /// <returns>true if the value collection is successfully found
            /// removed; otherwise, false.</returns>
            public bool Remove(ICollection<TVal> item)
                => mbimap.RemoveValuesByRef(item);
        }
    }
}
