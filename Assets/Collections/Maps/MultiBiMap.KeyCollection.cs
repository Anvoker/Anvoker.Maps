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
    /// Contains <see cref="MultiBiMap{TKey, TVal}.KeyCollection"/>
    /// implementation.
    /// </content>
    public partial class MultiBiMap<TKey, TVal>
    {
        /// <summary>
        /// Represents the collection of values in a
        /// <see cref="MultiBiMap{TKey, TVal}"/> This class cannot be inherited.
        /// </summary>
        public sealed class KeyCollection : ICollection<TKey>,
            IReadOnlyCollection<TKey>
        {
            private MultiBiMap<TKey, TVal> mbimap;

            /// <summary>
            /// Initializes a new instance of the <see cref="KeyCollection"/>
            /// class that reflects the keys in the specified
            /// <see cref="MultiBiMap{TKey, TVal}"/>.
            /// </summary>
            /// <param name="mbimap">The <see cref="MultiBiMap{TKey, TVal}"/>
            /// this <see cref="KeyCollection"/> is referencing.</param>
            public KeyCollection(MultiBiMap<TKey, TVal> mbimap)
            {
                this.mbimap = mbimap;
            }

            /// <summary>
            /// Gets the number of keys contained in the
            /// <see cref="MultiBiMap{TKey, TVal}"/>
            /// </summary>
            public int Count => mbimap.dictFwd.Count;

            /// <summary>
            /// Gets a value indicating whether the key collection is read-only.
            /// </summary>
            public bool IsReadOnly => false;

            /// <summary>
            /// Adds a new element to the <see cref="MultiBiMap{TKey, TVal}"/>
            /// with the specified key and an empty value collection.
            /// </summary>
            /// <param name="item">The key of the element to add.</param>
            public void Add(TKey item) => mbimap.AddKey(item);

            /// <summary>
            /// Removes all of the keys from the <see cref="KeyCollection"/> by
            /// clearing the <see cref="MultiBiMap{TKey, TVal}"/> of all its
            /// elements.
            /// </summary>
            public void Clear() => mbimap.Clear();

            /// <summary>
            /// Determines whether the <see cref="MultiBiMap{TKey, TVal}"/>
            /// contains a specific key.
            /// </summary>
            /// <param name="item">The key to locate in the
            /// <see cref="MultiBiMap{TKey, TVal}"/>.</param>
            /// <returns>true if the <see cref="MultiBiMap{TKey, TVal}"/>
            /// contains an element with the specified key; otherwise, false.
            /// </returns>
            public bool Contains(TKey item) => mbimap.ContainsKey(item);

            /// <summary>
            /// Copies the keys of the <see cref="MultiBiMap{TKey, TVal}"/>
            /// to an array, starting at the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional array that is the
            /// destination of the keys copied from
            /// <see cref="MultiBiMap{TKey, TVal}"/>. The array must have
            /// zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which
            /// copying begins.</param>
            public void CopyTo(TKey[] array, int arrayIndex)
                => mbimap.dictFwd.Keys.CopyTo(array, arrayIndex);

            /// <summary>
            /// Returns an enumerator that iterates through the keys
            /// of the <see cref="MultiBiMap{TKey, TVal}"/>.
            /// </summary>
            /// <returns>A KeyCollection enumerator structure for the
            /// <see cref="MultiBiMap{TKey, TVal}"/>.</returns>
            public IEnumerator<TKey> GetEnumerator()
                => mbimap.dictFwd.Keys.GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through the keys
            /// of the <see cref="MultiBiMap{TKey, TVal}"/>.
            /// </summary>
            /// <returns>A KeyCollection enumerator structure for the
            /// <see cref="MultiBiMap{TKey, TVal}"/>.</returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            /// Removes the element with the specified key from the
            /// <see cref="MultiBiMap{TKey, TVal}"/>.
            /// </summary>
            /// <param name="item">The key of the element to remove.</param>
            /// <returns>true if the element is successfully found and removed;
            /// otherwise, false.</returns>
            public bool Remove(TKey item) => mbimap.RemoveKey(item);
        }
    }
}
