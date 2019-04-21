using System;
using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    public interface ICollectionEqualityComparer<T, TCollection>
        : IEqualityComparer<TCollection>
        where TCollection : IEnumerable<T>
    {
    }
}