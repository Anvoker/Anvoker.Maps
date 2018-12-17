using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable RCS1169 // Mark field as read-only.

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a set of values used in a collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.
    /// </typeparam>
    /// <typeparam name="TVal">The type of the values.
    /// </typeparam>
    public class ValueSet<TKey, TVal> :
        ISet<TVal>,
        IReadOnlyCollection<TVal>
    {
        #region Public Constructors

        public ValueSet(TKey key, IMultiMap<TKey, TVal> parent)
        {
            Key = key;
            Parent = parent;
            HashSet = new HashSet<TVal>(parent.ComparerValue);
        }

        public ValueSet(TKey key, IMultiMap<TKey, TVal> parent, IEnumerable<TVal> values)
        {
            Key = key;
            Parent = parent;
            HashSet = new HashSet<TVal>(values, parent.ComparerValue);
        }

        public ValueSet(TKey key, IMultiMap<TKey, TVal> parent, HashSet<TVal> values)
        {
            Key = key;
            Parent = parent;
            HashSet = values;
        }

        public ValueSet(TKey key, IMultiMap<TKey, TVal> parent, TVal value)
        {
            Key = key;
            Parent = parent;
            HashSet = new HashSet<TVal>(parent.ComparerValue);
            Add(value);
        }

        #endregion Public Constructors

        #region Public Properties

        public HashSet<TVal> HashSet { get; }

        public bool IsReadOnly => false;

        public TKey Key { get; }

        public IMultiMap<TKey, TVal> Parent { get; }

        public int Count => HashSet.Count;

        #endregion Public Properties

        #region Public Methods

        public bool Add(TVal item)
            => HashSet.Add(item);

        public void Clear()
            => HashSet.Clear();

        public bool Contains(TVal item)
            => HashSet.Contains(item);

        public void CopyTo(TVal[] array, int arrayIndex)
            => HashSet.CopyTo(array, arrayIndex);

        public void ExceptWith(IEnumerable<TVal> other)
            => HashSet.ExceptWith(other);

        public IEnumerator<TVal> GetEnumerator()
            => HashSet.GetEnumerator();

        public void IntersectWith(IEnumerable<TVal> other)
            => HashSet.IntersectWith(other);

        public bool IsProperSubsetOf(IEnumerable<TVal> other)
            => HashSet.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<TVal> other)
            => HashSet.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<TVal> other)
            => HashSet.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<TVal> other)
            => HashSet.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<TVal> other)
            => HashSet.Overlaps(other);

        public bool Remove(TVal item)
            => HashSet.Remove(item);

        public bool SetEquals(IEnumerable<TVal> other)
            => HashSet.SetEquals(other);

        public void SymmetricExceptWith(IEnumerable<TVal> other)
            => HashSet.SymmetricExceptWith(other);

        public void UnionWith(IEnumerable<TVal> other)
            => HashSet.UnionWith(other);

        bool ISet<TVal>.Add(TVal item)
            => Parent.AddValue(Key, item);

        void ICollection<TVal>.Add(TVal item)
            => Parent.AddValue(Key, item);

        void ICollection<TVal>.Clear()
            => Parent.RemoveValuesAll(Key);

        IEnumerator IEnumerable.GetEnumerator()
            => HashSet.GetEnumerator();

        void ISet<TVal>.IntersectWith(IEnumerable<TVal> other)
        {
            var common = HashSet.Intersect(other, HashSet.Comparer);
            Parent.RemoveValuesAll(Key);
            Parent.AddValues(Key, common);
        }

        bool ICollection<TVal>.Remove(TVal item)
            => Parent.RemoveValue(Key, item);

        void ISet<TVal>.SymmetricExceptWith(IEnumerable<TVal> other)
        {
            var common = HashSet.Intersect(other, HashSet.Comparer);
            Parent.AddValues(Key, other);
            Parent.RemoveValues(Key, common);
        }

        void ISet<TVal>.UnionWith(IEnumerable<TVal> other)
            => Parent.AddValues(Key, other);

        #endregion Public Methods
    }
}