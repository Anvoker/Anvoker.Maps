using System;
using System.Collections.Generic;
using System.Linq;

namespace Anvoker.Collections.Maps.Tests
{
    public static class HelperMethods
    {
        public static TCollection[] UnionValues<TCollection, TValue>(
            TCollection[] first,
            TCollection[] second,
            IEqualityComparer<TValue> comparer,
            Func<IEnumerable<TValue>, TCollection> colConstructor)
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
                    values[i] = colConstructor(
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