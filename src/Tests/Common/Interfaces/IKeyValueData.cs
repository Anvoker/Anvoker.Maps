using System.Collections.Generic;

namespace Anvoker.Collections.Tests.Common.Interfaces
{
    public interface IKeyValueData<TK, TV, TCollection> :
        IValueData<KeyValuePair<TK, TV>, TCollection>
    {
        /// <summary>
        /// Gets the comparer for the key type.
        /// </summary>
        IEqualityComparer<TK> ComparerKey { get; }

        /// <summary>
        /// Gets the comparer for the value type.
        /// </summary>
        IEqualityComparer<TV> ComparerValue { get; }

        /// <summary>
        /// Gets a value indicating whether the key is of a nullable type.
        /// </summary>
        bool KeyIsNullabe { get; }

        /// <summary>
        /// Gets a unique set of keys to guaranteed to not be in the collection.
        /// Has no elements in common with <see cref="KeysInitial"/> or
        /// <see cref="KeysToAdd"/>.
        /// </summary>
        TK[] KeysExcluded { get; }

        /// <summary>
        /// Gets a unique set of keys.
        /// </summary>
        TK[] KeysInitial { get; }

        /// <summary>
        /// Gets a unique set of keys to add to the collection. Has no elements
        /// in common with <see cref="KeysInitial"/> or <see cref="KeysExcluded"/>.
        /// </summary>
        TK[] KeysToAdd { get; }

        /// <summary>
        /// Gets an array of key value pairs not contained in the
        /// implementor collection after construction.
        /// </summary>
        KeyValuePair<TK, TV>[] KVPsExcluded { get; }

        /// <summary>
        /// Gets an array of all of the initial key value pairs contained
        /// in the implementor collection after construction.
        /// </summary>
        KeyValuePair<TK, TV>[] KVPsInitial { get; }

        /// <summary>
        /// Gets an array of key value pairs to add to the implementor
        /// collection after construction.
        /// </summary>
        KeyValuePair<TK, TV>[] KVPsToAdd { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysExcluded"/>.
        /// </summary>
        new TV[] ValuesExcluded { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysInitial"/>.
        /// </summary>
        new TV[] ValuesInitial { get; }

        /// <summary>
        /// Gets a set of values associated with <see cref="KeysToAdd"/>.
        /// </summary>
        new TV[] ValuesToAdd { get; }
    }
}