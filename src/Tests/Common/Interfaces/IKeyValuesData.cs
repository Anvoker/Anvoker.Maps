using System.Collections.Generic;

namespace Anvoker.Collections.Tests.Common.Interfaces
{
    public interface IKeyValuesData<TK, TV, TCollection, TVCol>
        : IKeyValueData<TK, TVCol, TCollection>
    {
        /// <summary>
        /// Gets the comparer for the value type.
        /// </summary>
        new IEqualityComparer<TV> ComparerValue { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysExcluded"/>.
        /// </summary>
        TV[] ValuesExcludedFlat { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysInitial"/>.
        /// </summary>
        TV[] ValuesInitialFlat { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysToAdd"/>.
        /// </summary>
        TV[] ValuesToAddFlat { get; }
    }
}