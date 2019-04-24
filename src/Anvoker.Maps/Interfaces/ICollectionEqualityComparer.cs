using System;
using System.Collections.Generic;

namespace Anvoker.Maps.Interfaces
{
    public interface ICollectionEqualityComparer<T, TCollection>
        : IEqualityComparer<TCollection>
        where TCollection : IEnumerable<T>
    {
    }
}