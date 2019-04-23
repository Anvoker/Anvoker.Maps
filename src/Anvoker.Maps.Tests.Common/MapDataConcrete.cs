using System;
using System.Collections.Generic;
using Anvoker.Maps.Tests.Common.Interfaces;

namespace Anvoker.Maps.Tests.Common
{
    /// <summary>
    /// Provides key, values and comparers appropriate for testing a map of
    /// the specified type.
    /// </summary>
    /// <typeparam name="TK">The type of the key.</typeparam>
    /// <typeparam name="TV">The type of the value.</typeparam>
    /// <typeparam name="TCollection">The type of the collection to be tested.
    /// </typeparam>
    public class MapDataConcrete<TK, TV, TCollection> :
        MapData<TK, TV>, IKeyValueData<TK, TV, TCollection>
    {
        /// <summary>
        /// Initializes a new instance of
        /// the <see cref="MapDataConcrete{TK, TV, TCollection}"/> class
        /// which encapsulates data a map testing fixture would use.
        /// </summary>
        /// <param name="implementorCtor">The constructor of a
        /// collection type that implements <typeparamref name="TMap"/>
        /// and is initialized with data from <paramref name="initialKeys"/>
        /// and <paramref name="initialValues"/>.</param>
        /// <param name="data">The test data used for this fixture.</param>
        public MapDataConcrete(
            Func<TCollection> implementorCtor,
            MapData<TK, TV> data) : base(data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            ImplementorCtor = implementorCtor
                ?? throw new ArgumentNullException(nameof(implementorCtor));

            ValuesInitial  = data.ValuesInitial;
            ValuesToAdd    = data.ValuesToAdd;
            ValuesExcluded = data.ValuesExcluded;
            KVPsInitial    = HelperMethods.ConstructKVPs(KeysInitial, ValuesInitial);
            KVPsToAdd      = HelperMethods.ConstructKVPs(KeysToAdd, ValuesToAdd);
            KVPsExcluded   = HelperMethods.ConstructKVPs(KeysExcluded, ValuesExcluded);
        }

        /// <summary>
        /// Gets a delegate pointing to the constructor of the collection
        /// class implementing the interface being tested.
        /// </summary>
        public Func<TCollection> ImplementorCtor { get; }

        /// <summary>
        /// Gets an array of key value pairs not contained in the
        /// implementor collection after construction.
        /// </summary>
        public KeyValuePair<TK, TV>[] KVPsExcluded { get; }

        /// <summary>
        /// Gets an array of all of the initial key value pairs contained
        /// in the implementor collection after construction.
        /// </summary>
        public KeyValuePair<TK, TV>[] KVPsInitial { get; }

        /// <summary>
        /// Gets an array of key value pairs to add to the implementor
        /// collection after construction.
        /// </summary>
        public KeyValuePair<TK, TV>[] KVPsToAdd { get; }

        /// <summary>
        /// Gets an array of values associated with the excluded keys.
        /// </summary>
        public new TV[] ValuesExcluded { get; }

        /// <summary>
        /// Gets an array with all of the initial values contained in the
        /// implementor collection after construction.
        /// </summary>
        public new TV[] ValuesInitial { get; }

        /// <summary>
        /// Gets an array of values to add to the implementor collection
        /// after construction.
        /// </summary>
        public new TV[] ValuesToAdd { get; }

        KeyValuePair<TK, TV>[]
            IValueData<KeyValuePair<TK, TV>, TCollection>
            .ValuesInitial => KVPsInitial;

        KeyValuePair<TK, TV>[]
            IValueData<KeyValuePair<TK, TV>, TCollection>
            .ValuesToAdd => KVPsToAdd;

        KeyValuePair<TK, TV>[]
            IValueData<KeyValuePair<TK, TV>, TCollection>
            .ValuesExcluded => KVPsExcluded;

        public override string ToString()
            => $"{nameof(MapDataConcrete<TK, TV, TCollection>)}({TestDataName})";
    }
}
