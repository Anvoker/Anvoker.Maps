using System;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Anvoker.Maps.Tests.Common
{
    public class IFixedKeysBiMapTester<TK, TV, TFKBiMap>
        where TFKBiMap : IFixedKeysBiMap<TK, TV>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValueData<TK, TV, TFKBiMap> d;

        private TFKBiMap bimap;

        public IFixedKeysBiMapTester(IKeyValueData<TK, TV, TFKBiMap> args)
        {
            d = args;
        }

        [SetUp]
        public void Setup() => bimap = d.ImplementorCtor();

        [Test]
        public void Replace_Existing_DifferentValue()
        {
            var length = Math.Min(d.KeysInitial.Length, d.KeysExcluded.Length);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < length; i++)
                {
                    bimap.Replace(d.KeysInitial[i], d.ValuesExcluded[i]);
                    Assert.AreEqual(d.ValuesExcluded[i], bimap[d.KeysInitial[i]]);
                }
            });
        }

        [Test]
        public void Replace_Existing_SameValue()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < d.KeysInitial.Length; i++)
                {
                    bimap.Replace(d.KeysInitial[i], d.ValuesInitial[i]);
                    Assert.AreEqual(d.ValuesInitial[i], bimap[d.KeysInitial[i]]);
                }
            });
        }
    }
}