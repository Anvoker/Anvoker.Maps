using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anvoker.Collections.Maps
{
    /// <summary>
    /// Represents a set of values used in a multimap. Changes made to this
    /// collection will reflect in the parent map.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.
    /// </typeparam>
    /// <typeparam name="TVal">The type of the values.
    /// </typeparam>
    public class ValueSet<TKey, TVal> :
        ISet<TVal>,
        ICollection<TVal>,
        IReadOnlyCollection<TVal>
    {
        private readonly HashSet<TVal> hashSet;

        #region Public Constructors

        public ValueSet(TKey key, IMultiMap<TKey, TVal> parent)
        {
            Key = key;
            Parent = parent;
            hashSet = new HashSet<TVal>(parent.ComparerValue);
        }

        public ValueSet(TKey key, IMultiMap<TKey, TVal> parent, IEnumerable<TVal> values)
        {
            Key = key;
            Parent = parent;
            hashSet = new HashSet<TVal>(values, parent.ComparerValue);
        }

        public ValueSet(TKey key, IMultiMap<TKey, TVal> parent, HashSet<TVal> values)
        {
            Key = key;
            Parent = parent;
            hashSet = values;
        }

        public ValueSet(TKey key, IMultiMap<TKey, TVal> parent, TVal value)
        {
            Key = key;
            Parent = parent;
            hashSet = new HashSet<TVal>(parent.ComparerValue);
            hashSet.Add(value);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the key this <see cref="ValueSet{TKey, TVal}"/> is associated
        /// to in <see cref="ValueSet{TKey, TVal}.Parent"/>.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// Gets the <see cref="IMultiMap{TKey, TVal}"/> this
        /// <see cref="ValueSet{TKey, TVal}"/> belongs to.
        /// </summary>
        public IMultiMap<TKey, TVal> Parent { get; }

        public int Count => hashSet.Count;

        bool ICollection<TVal>.IsReadOnly => false;

        /// <summary>
        /// Gets the underlying <see cref="HashSet{T}"/>, which needs to be
        /// accessed if one wants to mutate a <see cref="ValueSet{TKey, TVal}"/>
        /// without having side effects happen in its parent map.
        /// The <see cref="ValueSet{TKey, TVal}"/> MUST be accessed this way
        /// when mutating it from inside its parent.
        /// </summary>
        internal HashSet<TVal> Internal => hashSet;

        #endregion Public Properties

        #region Public Methods

        public bool Add(TVal item)
            => Parent.AddValue(Key, item);

        public void Clear()
            => Parent.RemoveValuesAll(Key);

        public bool Contains(TVal item)
            => hashSet.Contains(item);

        public void ExceptWith(IEnumerable<TVal> other)
        {
            var common = hashSet.Intersect(other, hashSet.Comparer);
            Parent.RemoveValues(Key, common);
        }

        public IEnumerator<TVal> GetEnumerator()
            => hashSet.GetEnumerator();

        public void IntersectWith(IEnumerable<TVal> other)
        {
            var common = hashSet.Intersect(other, hashSet.Comparer);
            Parent.RemoveValuesAll(Key);
            Parent.AddValues(Key, common);
        }

        public bool IsProperSubsetOf(IEnumerable<TVal> other)
            => hashSet.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<TVal> other)
            => hashSet.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<TVal> other)
            => hashSet.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<TVal> other)
            => hashSet.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<TVal> other)
            => hashSet.Overlaps(other);

        public bool Remove(TVal item)
            => Parent.RemoveValue(Key, item);

        public bool SetEquals(IEnumerable<TVal> other)
            => hashSet.SetEquals(other);

        public void SymmetricExceptWith(IEnumerable<TVal> other)
        {
            var common = hashSet.Intersect(other, hashSet.Comparer);
            Parent.AddValues(Key, other);
            Parent.RemoveValues(Key, common);
        }

        public void UnionWith(IEnumerable<TVal> other)
            => Parent.AddValues(Key, other);

        void ICollection<TVal>.Add(TVal item)
            => Parent.AddValue(Key, item);

        void ICollection<TVal>.CopyTo(TVal[] array, int arrayIndex)
            => hashSet.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator()
            => hashSet.GetEnumerator();

        #endregion Public Methods
    }
}