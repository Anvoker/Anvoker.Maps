using System.Collections;
using System.Collections.Generic;

namespace Anvoker.Collections.Maps
{
    public interface IValueSets<T> : IReadOnlyValueSets<T>, ICollection<ICollection<T>>
    { }

    public interface IReadOnlyValueSets<T> : IReadOnlyCollection<IReadOnlyCollection<T>>
    { }
}
