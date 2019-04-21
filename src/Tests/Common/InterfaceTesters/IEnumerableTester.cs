using System.Collections.Generic;
using Anvoker.Collections.Tests.Common.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Anvoker.Collections.Tests.Common
{
    public class IEnumerableTester<T, TEnumerable>
        where TEnumerable : IEnumerable<T>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IValueData<T, TEnumerable> d;

        private TEnumerable collection;

        public IEnumerableTester(IValueData<T, TEnumerable> args)
        {
            d = args;
        }

        [OneTimeSetUp]
        public void Setup() => collection = d.ImplementorCtor();

        [Test]
        public void GetEnumerator_Initial()
        {
            var enumerator = collection.GetEnumerator();

            CollectionAssert.AreEquivalent(d.ValuesInitial, Actual());

            IEnumerable<T> Actual()
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }
    }
}