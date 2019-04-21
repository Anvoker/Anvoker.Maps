using System.Collections.Generic;
using Anvoker.Collections.Tests.Common.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Anvoker.Collections.Tests.Common
{
    public class IReadOnlyCollectionTester<T, TROCollection>
        where TROCollection : IReadOnlyCollection<T>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IValueData<T, TROCollection> d;

        private TROCollection collection;

        public IReadOnlyCollectionTester(IValueData<T, TROCollection> args)
        {
            d = args;
        }

        [OneTimeSetUp]
        public void Setup() => collection = d.ImplementorCtor();

        [Test]
        public void Count_Initial()
            => Assert.AreEqual(d.ValuesInitial.Length, collection.Count);
    }
}