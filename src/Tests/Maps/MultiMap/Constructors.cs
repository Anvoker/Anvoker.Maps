using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.MultiMapTests
{
    using Anvoker.Collections.Maps;

    /// <summary>
    /// Unit tests for <see cref="MultiMap{TKey, TVal}"/>'s constructors.
    /// </summary>
    [TestFixture]
    public class Constructors
    {
        /// <summary>
        /// Checks that the parameterless constructor leaves the new MultiMap
        /// in the expected state.
        /// </summary>
        [Test]
        public void ParameterlessConstructor()
        {
            var multiMap = new MultiMap<string, int>();
            Assert.AreEqual(0, multiMap.Count);
            Assert.AreEqual(0, multiMap.Keys.Count);
            Assert.AreEqual(0, multiMap.Values.Count);
            Assert.AreEqual(
                EqualityComparer<string>.Default, multiMap.ComparerKey);
            Assert.AreEqual(
                EqualityComparer<int>.Default, multiMap.ComparerValue);
        }

        /// <summary>
        /// Checks that the constructor with the capacity parameter leaves
        /// the new MultiMap in the expected state.
        /// </summary>
        [Test]
        public void CapacityConstructor()
        {
            var multiMap = new MultiMap<string, int>(10);
            Assert.AreEqual(0, multiMap.Count);
            Assert.AreEqual(0, multiMap.Keys.Count);
            Assert.AreEqual(0, multiMap.Values.Count);
            Assert.AreEqual(
                EqualityComparer<string>.Default, multiMap.ComparerKey);
            Assert.AreEqual(
                EqualityComparer<int>.Default, multiMap.ComparerValue);
        }

        /// <summary>
        /// Checks that the constructor with the comparer parameters leaves
        /// the new MultiMap in the expected state.
        /// </summary>
        [Test]
        public void ComparerConstructor()
        {
            var comparerKey = StringComparer.OrdinalIgnoreCase;
            var comparerValue = StringComparer.InvariantCulture;
            var multiMap = new MultiMap<string, string>(
                comparerKey, comparerValue);
            Assert.AreEqual(0, multiMap.Count);
            Assert.AreEqual(comparerKey, multiMap.ComparerKey);
            Assert.AreEqual(comparerValue, multiMap.ComparerValue);
            Assert.AreEqual(0, multiMap.Keys.Count);
            Assert.AreEqual(0, multiMap.Values.Count);
        }
    }
}
