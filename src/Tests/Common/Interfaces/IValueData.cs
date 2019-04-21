using System;

namespace Anvoker.Collections.Tests.Common.Interfaces
{
    public interface IValueData<TV, TCollection> : IData
    {
        /// <summary>
        /// Gets a delegate pointing to the constructor of the collection
        /// class implementing the interface being tested.
        /// </summary>
        Func<TCollection> ImplementorCtor { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysExcluded"/>.
        /// </summary>
        TV[] ValuesExcluded { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysInitial"/>.
        /// </summary>
        TV[] ValuesInitial { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysToAdd"/>.
        /// </summary>
        TV[] ValuesToAdd { get; }
    }
}