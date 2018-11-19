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
    /// <summary>
    /// A set of values associated with a key in a generic key-values based
    /// collection.
    /// </summary>
    /// <remarks>
    /// Used as an adapter to
    /// <see cref="Dictionary{TKey, TValue}.ValueCollection"/> in order to
    /// avoid the need for nested generic collection casts, which does not work
    /// on any collection interfaces that aren't generic type covariant. This
    /// class acts as a map that encapsulates all of the needed casts.
    /// </remarks>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TVal">The type of the values.</typeparam>
    public class ValueSets<TKey, TVal> : IValueSets<TVal>
    {
        private ICollection<HashSet<TVal>> hashSetCol;

        private Dictionary<TKey, HashSet<TVal>>.ValueCollection
            valueCollection;

        public ValueSets(Dictionary<TKey, HashSet<TVal>>.ValueCollection
            valueCollection)
        {
            this.valueCollection = valueCollection;
            hashSetCol = valueCollection;
        }

        public int Count
            => hashSetCol.Count;

        public bool IsReadOnly
            => hashSetCol.IsReadOnly;

        public void Add(HashSet<TVal> item)
            => hashSetCol.Add(item);

        public void Add(ICollection<TVal> item)
            => hashSetCol.Add((HashSet<TVal>)item);

        public void Clear()
            => hashSetCol.Clear();

        public bool Contains(HashSet<TVal> item)
            => hashSetCol.Contains(item);

        public bool Contains(ICollection<TVal> item)
            => hashSetCol.Contains((HashSet<TVal>)item);

        public void CopyTo(HashSet<TVal>[] array, int arrayIndex)
            => hashSetCol.CopyTo(array, arrayIndex);

        public void CopyTo(ICollection<TVal>[] array, int arrayIndex)
            => hashSetCol.CopyTo((HashSet<TVal>[])array, arrayIndex);

        public IEnumerator<HashSet<TVal>> GetEnumerator()
            => hashSetCol.GetEnumerator();

        public bool Remove(HashSet<TVal> item)
            => hashSetCol.Remove(item);

        public bool Remove(ICollection<TVal> item)
            => hashSetCol.Remove((HashSet<TVal>)item);

        IEnumerator IEnumerable.GetEnumerator()
            => hashSetCol.GetEnumerator();

        IEnumerator<ICollection<TVal>>
            IEnumerable<ICollection<TVal>>.GetEnumerator()
            => hashSetCol.GetEnumerator();

        public static implicit operator
            Dictionary<TKey, HashSet<TVal>>.ValueCollection(
            ValueSets<TKey, TVal> valueSet)
            => valueSet.valueCollection;
    }
}