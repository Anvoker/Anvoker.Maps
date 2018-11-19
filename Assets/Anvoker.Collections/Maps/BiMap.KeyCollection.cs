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
    /// Contains <see cref="BiMap{TKey, TVal}.KeyCollection"/>
    /// implementation.
    /// </content>
    public partial class BiMap<TKey, TVal>
    {
        /// <summary>
        /// Represents the collection of values in a
        /// <see cref="BiMap{TKey, TVal}"/> This class cannot be inherited.
        /// </summary>
        public sealed class KeyCollection : ICollection<TKey>,
            IReadOnlyCollection<TKey>
        {
            private BiMap<TKey, TVal> bimap;

            /// <summary>
            /// Initializes a new instance of the <see cref="KeyCollection"/>
            /// class that reflects the keys in the specified
            /// <see cref="BiMap{TKey, TVal}"/>.
            /// Represents the collection of values in a
            /// <see cref="BiMap{TKey, TVal}"/> This class cannot be inherited.
            /// </summary>
            /// <param name="bimap">The <see cref="BiMap{TKey, TVal}"/>
            /// this <see cref="KeyCollection"/> is referencing.</param>
            public KeyCollection(BiMap<TKey, TVal> bimap)
            {
                this.bimap = bimap;
            }

            /// <summary>
            /// Gets the number of keys contained in the
            /// <see cref="BiMap{TKey, TVal}"/>
            /// </summary>
            public int Count => bimap.dictFwd.Count;

            /// <summary>
            /// Gets a value indicating whether the <see cref="KeyCollection"/>
            /// is read-only.
            /// </summary>
            public bool IsReadOnly => false;

            /// <summary>
            /// Adds a new element to the <see cref="BiMap{TKey, TVal}"/>
            /// with the specified key and the default value for the
            /// <see cref="BiMap{TKey, TVal}"/>'s value type.
            /// </summary>
            /// <param name="item">The key of the element to add.</param>
            public void Add(TKey item) => bimap.Add(item, default(TVal));

            /// <summary>
            /// Removes all of the keys from the <see cref="KeyCollection"/> by
            /// clearing the <see cref="BiMap{TKey, TVal}"/> of all its
            /// elements.
            /// </summary>
            public void Clear() => bimap.Clear();

            /// <summary>
            /// Determines whether the <see cref="BiMap{TKey, TVal}"/> contains
            /// a specific key.
            /// </summary>
            /// <param name="item">The key to locate in the
            /// <see cref="BiMap{TKey, TVal}"/>.</param>
            /// <returns>true if the <see cref="BiMap{TKey, TVal}"/> contains
            /// an element with the specified key; otherwise, false.
            /// </returns>
            public bool Contains(TKey item) => bimap.ContainsKey(item);

            /// <summary>
            /// Copies the keys of the <see cref="BiMap{TKey, TVal}"/>
            /// to an array, starting at the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional array that is the
            /// destination of the keys copied from
            /// <see cref="BiMap{TKey, TVal}"/>. The array must have
            /// zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which
            /// copying begins.</param>
            public void CopyTo(TKey[] array, int arrayIndex)
                => bimap.dictFwd.Keys.CopyTo(array, arrayIndex);

            /// <summary>
            /// Returns an enumerator that iterates through the keys
            /// of the <see cref="BiMap{TKey, TVal}"/>.
            /// </summary>
            /// <returns>A KeyCollection enumerator structure for the
            /// <see cref="BiMap{TKey, TVal}"/>.</returns>
            public IEnumerator<TKey> GetEnumerator()
                => bimap.dictFwd.Keys.GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through the keys
            /// of the <see cref="BiMap{TKey, TVal}"/>.
            /// </summary>
            /// <returns>A KeyCollection enumerator structure for the
            /// <see cref="BiMap{TKey, TVal}"/>.</returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            /// Removes the element with the specified key from the
            /// <see cref="BiMap{TKey, TVal}"/>.
            /// </summary>
            /// <param name="item">The key of the element to remove.</param>
            /// <returns>true if the element is successfully found and removed;
            /// otherwise, false.</returns>
            public bool Remove(TKey item) => bimap.Remove(item);
        }
    }
}