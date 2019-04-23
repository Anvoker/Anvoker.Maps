using System;
using System.Collections.Generic;
using System.Linq;
using Anvoker.Maps.Tests.Common.Interfaces;

namespace Anvoker.Maps.Tests.Common
{
    /// <summary>
    /// Provides key, values and comparers appropriate for testing a map of
    /// the specified type.
    /// </summary>
    /// <typeparam name="TK">The type of the key.</typeparam>
    /// <typeparam name="TV">The type of the value.</typeparam>
    /// <typeparam name="TMap">The type of the map being tested.</typeparam>
    /// <typeparam name="TVCol">The type of the value collection used in the
    /// map.</typeparam>
    public class MultiMapDataConcrete<TK, TV, TMap, TVCol> :
        MultiMapData<TK, TV>, IKeyValuesData<TK, TV, TMap, TVCol>
    {
        private IEqualityComparer<TVCol> comparerCollection;

        private TV[] valuesExcludedFlat;

        private TV[] valuesInitialFlat;

        private TV[] valuesToAddFlat;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MultiMapDataConcrete{TK, TV, TMap, TVCol}"/> class
        /// which encapsulates data a map testing fixture would use.
        /// </summary>
        /// <param name="implementorCtor">The constructor of a
        /// collection type that implements <typeparamref name="TMap"/>
        /// and is initialized with data from <paramref name="initialKeys"/>
        /// and <paramref name="initialValues"/>.</param>
        /// <param name="valueCollectionCtor">Delegate pointing to a
        /// constructor of <see cref="TVCol"/> that takes
        /// <see cref="IEnumerable{T}"/> of type <see cref="TV"/> as
        /// parameter.</param>
        /// <param name="comparerCollection">The comparer used for determining
        /// equality between value collections. Leave null if not
        /// applicable.</param>
        /// <param name="data">The test data used for this fixture.</param>
        public MultiMapDataConcrete(
            Func<TMap> implementorCtor,
            Func<IEnumerable<TV>, TVCol> valueCollectionCtor,
            IEqualityComparer<TVCol> comparerCollection,
            MultiMapData<TK, TV> data) : base(data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            ImplementorCtor = implementorCtor
                ?? throw new ArgumentNullException(nameof(implementorCtor));

            ValueCollectionCtor = valueCollectionCtor
                ?? throw new ArgumentNullException(nameof(valueCollectionCtor));

            ValuesInitial = ConstructVCols(data.ValuesInitial, valueCollectionCtor);
            ValuesToAdd = ConstructVCols(data.ValuesToAdd, valueCollectionCtor);
            ValuesExcluded = ConstructVCols(data.ValuesExcluded, valueCollectionCtor);
            KVPsInitial = HelperMethods.ConstructKVPs(KeysInitial, ValuesInitial);
            KVPsToAdd = HelperMethods.ConstructKVPs(KeysToAdd, ValuesToAdd);
            KVPsExcluded = HelperMethods.ConstructKVPs(KeysExcluded, ValuesExcluded);
            valuesInitialFlat = data.ValuesInitial.SelectMany(a => a).ToArray();
            valuesToAddFlat = data.ValuesToAdd.SelectMany(a => a).ToArray();
            valuesExcludedFlat = data.ValuesExcluded.SelectMany(a => a).ToArray();

            this.comparerCollection = comparerCollection;
        }

        /// <summary>
        /// Gets a delegate pointing to the constructor of the collection
        /// class implementing the interface being tested.
        /// </summary>
        public Func<TMap> ImplementorCtor { get; }

        /// <summary>
        /// Gets an array of key value pairs not contained in the
        /// implementor collection after construction.
        /// </summary>
        public KeyValuePair<TK, TVCol>[] KVPsExcluded { get; }

        /// <summary>
        /// Gets an array of all of the initial key value pairs contained
        /// in the implementor collection after construction.
        /// </summary>
        public KeyValuePair<TK, TVCol>[] KVPsInitial { get; }

        /// <summary>
        /// Gets an array of key value pairs to add to the implementor
        /// collection after construction.
        /// </summary>
        public KeyValuePair<TK, TVCol>[] KVPsToAdd { get; }

        /// <summary>
        /// Gets a delegate pointing to a constructor of
        /// <see cref="TVCol"/> that takes <see cref="IEnumerable{T}"/>
        /// of type <see cref="TV"/> as parameter.
        /// </summary>
        public Func<IEnumerable<TV>, TVCol> ValueCollectionCtor { get; }

        /// <summary>
        /// Gets an array of values associated with the excluded keys.
        /// </summary>
        public new TVCol[] ValuesExcluded { get; }

        /// <summary>
        /// Gets an array of values associated with the excluded keys.
        /// </summary>
        public TV[] ValuesExcludedFlat => valuesExcludedFlat;

        /// <summary>
        /// Gets an array with all of the initial values contained in the
        /// implementor collection after construction.
        /// </summary>
        public new TVCol[] ValuesInitial { get; }

        /// <summary>
        /// Gets an array with all of the initial values contained in the
        /// implementor collection after construction.
        /// </summary>
        public TV[] ValuesInitialFlat => valuesInitialFlat;

        /// <summary>
        /// Gets an array of values to add to the implementor collection
        /// after construction.
        /// </summary>
        public new TVCol[] ValuesToAdd { get; }

        /// <summary>
        /// Gets an array of values to add to the implementor collection
        /// after construction.
        /// </summary>
        public TV[] ValuesToAddFlat => valuesToAddFlat;

        IEqualityComparer<TVCol>
            IKeyValueData<TK, TVCol, TMap>
            .ComparerValue => comparerCollection;

        KeyValuePair<TK, TVCol>[]
            IValueData<KeyValuePair<TK, TVCol>, TMap>
            .ValuesExcluded => KVPsExcluded;

        KeyValuePair<TK, TVCol>[]
                    IValueData<KeyValuePair<TK, TVCol>, TMap>
            .ValuesInitial => KVPsInitial;

        KeyValuePair<TK, TVCol>[]
            IValueData<KeyValuePair<TK, TVCol>, TMap>
            .ValuesToAdd => KVPsToAdd;

        public override string ToString()
            => $"{nameof(MultiMapDataConcrete<TK, TV, TMap, TVCol>)}({TestDataName})";

        private static TVCol[] ConstructVCols(
            TV[][] vals, Func<IEnumerable<TV>, TVCol> ctor)
        {
            var result = new TVCol[vals.Length];

            for (int i = 0; i < vals.Length; i++)
            {
                result[i] = ctor(vals[i]);
            }

            return result;
        }
    }
}