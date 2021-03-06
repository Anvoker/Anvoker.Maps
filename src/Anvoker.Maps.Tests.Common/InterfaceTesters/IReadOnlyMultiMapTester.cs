﻿using System.Collections.Generic;
using Anvoker.Maps.Interfaces;
using Anvoker.Maps.Tests.Common.Interfaces;
using NUnit.FixtureDependent;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Anvoker.Maps.Tests.Common
{
    public class IReadOnlyMultiMapTester<TK, TV, TROMMap, TVCol>
        where TROMMap : IReadOnlyMultiMap<TK, TV>
        where TVCol : IEnumerable<TV>
    {
        /// <summary>
        /// Gets a data class that stores all of the variables needed for this
        /// test fixture.
        /// </summary>
        private readonly IKeyValuesData<TK, TV, TROMMap, TVCol> d;

        private TROMMap map;

        public IReadOnlyMultiMapTester(
            IKeyValuesData<TK, TV, TROMMap, TVCol> args)
        {
            d = args;
        }

        [OneTimeSetUp]
        public void Setup() => map = d.ImplementorCtor();

        [Test]
        public void ComparerKeyReturnsCorrectValue()
            => Assert.AreEqual(d.ComparerKey, map.ComparerKey);

        [Test]
        public void ComparerValueReturnsCorrectValue()
            => Assert.AreEqual(d.ComparerValue, map.ComparerValue);

        [Test, SequentialDependent]
        public void ContainsValue_Excluded(
            [FixtureValueSource(typeof(IKeyValuesData<,,,>),
                nameof(IKeyValuesData<TK, TV, TROMMap, TVCol>.ValuesExcludedFlat))]
            TV value)
            => Assert.False(map.ContainsValue(value));

        [Test, SequentialDependent]
        public void ContainsValue_Initial(
            [FixtureValueSource(typeof(IKeyValuesData<,,,>),
                nameof(IKeyValuesData<TK, TV, TROMMap, TVCol>.ValuesInitialFlat))]
            TV value)
            => Assert.True(map.ContainsValue(value));
    }
}