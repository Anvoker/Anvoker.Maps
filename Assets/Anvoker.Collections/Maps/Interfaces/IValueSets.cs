using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anvoker.Collections.Maps
{
    public interface IValueSets<T> : ICollection<HashSet<T>>,
        ICollection<ICollection<T>>
    {
    }
}
