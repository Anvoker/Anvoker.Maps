using System;
using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    public class CollectionEqualityComparer<T, TCollection>
        : ICollectionEqualityComparer<T, TCollection>,
        IEquatable<CollectionEqualityComparer<T, TCollection>>
        where TCollection : IEnumerable<T>
    {
        private IEqualityComparer<T> comparerValue;

        public CollectionEqualityComparer(IEqualityComparer<T> comparerValue)
        {
            this.comparerValue = comparerValue;
        }

        public CollectionEqualityComparer()
        {
            comparerValue = EqualityComparer<T>.Default;
        }

        public static CollectionEqualityComparer<T, TCollection> Default
                            => new CollectionEqualityComparer<T, TCollection>();

        public bool Equals(TCollection x, TCollection y)
        {
            var hashSetX = x as HashSet<T>
                ?? new HashSet<T>(x, comparerValue);
            return hashSetX.SetEquals(y);
        }

        public bool Equals(CollectionEqualityComparer<T, TCollection> other)
            => other.comparerValue == comparerValue;

        public int GetHashCode(TCollection obj)
        {
            int hash = 12582917;
            foreach (T val in obj)
            {
                unchecked
                {
                    hash *= 786433 + comparerValue.GetHashCode(val);
                }
            }

            return hash;
        }
    }
}