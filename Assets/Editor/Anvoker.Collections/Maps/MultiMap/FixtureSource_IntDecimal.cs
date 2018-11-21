using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NestedIDictionaryArgs = Anvoker.Collections.Maps.Tests
    .NestedIDictionaryBase<int, decimal,
        Anvoker.Collections.Maps.MultiMap<int, decimal>,
        System.Collections.Generic.ICollection<decimal>>
    .Args;

namespace Anvoker.Collections.Maps.Tests.MultiMap
{
    /// <summary>
    /// Provides test data for <see cref="NestedIDictionary_IntDecimal"/>.
    /// </summary>
    internal static class FixtureSource_IntDecimal
    {
        private static readonly int[] keys = new int[]
        {
            25, 37, 99, 20, -5
        };

        private static readonly int[] keysToAdd = new int[]
        {
            900, 901, 525
        };

        private static readonly int[] keysExcluded = new int[]
        {
            24, 2, -8
        };

        private static readonly decimal[][] values = new decimal[][]
        {
            new decimal[] { 0.1m },
            new decimal[] { 5.25m, 0.0m },
            new decimal[] { 5.25m, 0.0m, 2.0m, 5.0m },
            new decimal[] { },
            new decimal[] { 3.75m },
        };

        private static readonly decimal[][] valuesToAdd = new decimal[][]
        {
            new decimal[] { 99.1m },
            new decimal[] { },
            new decimal[] { 5.25m, 0.0m, 25.0m },
        };

        private static readonly decimal[][] valuesExcluded = new decimal[][]
        {
            new decimal[] { 5.25m, 0.0m },
            new decimal[] { },
            new decimal[] { 5.25m, 0.0m, 25.0m },
        };

        private static readonly Func<IEnumerable<decimal>, ICollection<decimal>>
            valueCollectionCtor = (x) => new HashSet<decimal>(x);

        /// <summary>
        /// Provides the arguments for a test fixture that is decorated with
        /// <see cref="TestCaseSourceAttribute"/> and has a constructor with
        /// matching parameter types.
        /// </summary>
        /// <returns>An array of objects where each object is a collection
        /// that contains all of the necessary parameters to run a constructor
        /// of matching type.</returns>
        public static object[] GetNestedIDictionaryArgs() => new object[]
        {
            ConstructIntDecimalCase()
        };

        private static object[] ConstructIntDecimalCase()
        {
            #pragma warning disable IDE0039 // Use local function
            Func<MultiMap<int, decimal>> multiMapCtor = ()
                => new MultiMap<int, decimal>()
            {
                { keys[0], values[0] },
                { keys[1], values[1] },
                { keys[2], values[2] },
                { keys[3], values[3] },
                { keys[4], values[4] }
            };
            #pragma warning restore IDE0039 // Use local function

            return new object[]
            {
                new NestedIDictionaryArgs(
                    multiMapCtor,
                    valueCollectionCtor,
                    keys,
                    values,
                    keysExcluded,
                    valuesExcluded,
                    keysToAdd,
                    valuesToAdd,
                    null,
                    null)
            };
        }
    }
}