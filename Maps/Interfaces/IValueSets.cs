using System.Collections;
using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    public interface IValueSets<T> : ICollection<HashSet<T>>,
        ICollection<ICollection<T>>
    {
    }
}
