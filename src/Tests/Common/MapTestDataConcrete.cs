using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvoker.Collections.Tests.Common
{
    /// <summary>
    /// Provides key, values and comparers appropriate for testing a map of
    /// the specified type.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TVal">The type of the value.</typeparam>
    /// <typeparam name="TMap">The type of the map being tested.</typeparam>
    /// <typeparam name="TValCol">The type of the value collection used in the
    /// map.</typeparam>
    public class MapTestDataConcrete<TKey, TVal, TMap, TValCol> :
        MapTestData<TKey, TVal>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MapTestDataConcrete{TKey, TVal, TMap, TValCol}"/> class
        /// which encapsulates data a map testing fixture would use.
        /// </summary>
        /// <param name="implementorCtor">The constructor of a
        /// collection type that implements <typeparamref name="TMap"/>
        /// and is initialized with data from <paramref name="initialKeys"/>
        /// and <paramref name="initialValues"/>.</param>
        /// <param name="valueCollectionCtor">Delegate pointing to a
        /// constructor of <see cref="TValCol"/> that takes
        /// <see cref="IEnumerable{T}"/> of type <see cref="TVal"/> as
        /// parameter.</param>
        /// <param name="data">The test data used for this fixture.</param>
        public MapTestDataConcrete(
            Func<TMap> implementorCtor,
            Func<IEnumerable<TVal>, TValCol> valueCollectionCtor,
            MapTestData<TKey, TVal> data) : base(data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.ImplementorCtor = implementorCtor
                ?? throw new ArgumentNullException(
                    nameof(implementorCtor));

            this.ValueCollectionCtor = valueCollectionCtor
                ?? throw new ArgumentNullException(
                    nameof(valueCollectionCtor));

            this.ValuesInitial = ConstructValCols(
                data.ValuesInitial, valueCollectionCtor);

            this.ValuesToAdd = ConstructValCols(
                data.ValuesToAdd, valueCollectionCtor);

            this.ValuesExcluded = ConstructValCols(
                data.ValuesExcluded, valueCollectionCtor);

            this.KVPsInitial  = HelperMethods
                .ConstructKVPs(this.KeysInitial, this.ValuesInitial);
            this.KVPsToAdd    = HelperMethods
                .ConstructKVPs(this.KeysToAdd, this.ValuesToAdd);
            this.KVPsExcluded = HelperMethods
                .ConstructKVPs(this.KeysExcluded, this.ValuesExcluded);
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
        public KeyValuePair<TKey, TValCol>[] KVPsExcluded { get; }

        /// <summary>
        /// Gets an array of all of the initial key value pairs contained
        /// in the implementor collection after construction.
        /// </summary>
        public KeyValuePair<TKey, TValCol>[] KVPsInitial { get; }

        /// <summary>
        /// Gets an array of key value pairs to add to the implementor
        /// collection after construction.
        /// </summary>
        public KeyValuePair<TKey, TValCol>[] KVPsToAdd { get; }

        /// <summary>
        /// Gets a delegate pointing to a constructor of
        /// <see cref="TValCol"/> that takes <see cref="IEnumerable{T}"/>
        /// of type <see cref="TVal"/> as parameter.
        /// </summary>
        public Func<IEnumerable<TVal>, TValCol> ValueCollectionCtor { get; }

        /// <summary>
        /// Gets an array of values associated with the excluded keys.
        /// </summary>
        public new TValCol[] ValuesExcluded { get; }

        /// <summary>
        /// Gets an array with all of the initial values contained in the
        /// implementor collection after construction.
        /// </summary>
        public new TValCol[] ValuesInitial { get; }

        /// <summary>
        /// Gets an array of values to add to the implementor collection
        /// after construction.
        /// </summary>
        public new TValCol[] ValuesToAdd { get; }

        private static TValCol[] ConstructValCols(
            TVal[][] vals, Func<IEnumerable<TVal>, TValCol> ctor)
        {
            var result = new TValCol[vals.Length];

            for (int i = 0; i < vals.Length; i++)
            {
                result[i] = ctor(vals[i]);
            }

            return result;
        }
    }
}
