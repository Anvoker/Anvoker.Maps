using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Anvoker.Collections.Maps.Tests.MultiMap
{
    /// <summary>
    /// Provides test data for <see cref="NestedIDictionary_IntDecimal"/>.
    /// </summary>
    internal static class FixtureSource_IntDecimal
    {
        private static readonly int[] intKeys = new int[]
        {
            25, 37, 99, 20, -5
        };

        private static readonly int[] intKeysExcluded = new int[]
        {
            24, 2, -8
        };

        private static readonly decimal[][] decimalValues = new decimal[][]
        {
            new decimal[] { 0.1m },
            new decimal[] { 5.25m, 0.0m },
            new decimal[] { 5.25m, 0.0m, 2.0m, 5.0m },
            new decimal[] { 1.75m },
            new decimal[] { 3.75m },
        };

        /// <summary>
        /// Provides the arguments for a test fixture that is decorated with
        /// <see cref="TestCaseSourceAttribute"/> and has a constructor with
        /// matching parameter types.
        /// </summary>
        /// <returns>An array of objects where each object is a collection
        /// that contains all of the necessary parameters to run a constructor
        /// of matching type.</returns>
        public static object[] FixtureArgs() => new object[]
        {
            ConstructIntDecimalCase()
        };

        private static object[] ConstructIntDecimalCase()
        {
            var multiMap = new MultiMap<int, decimal>()
            {
                { intKeys[0], decimalValues[0] },
                { intKeys[1], decimalValues[1] },
                { intKeys[2], decimalValues[2] },
                { intKeys[3], decimalValues[3] },
                { intKeys[4], decimalValues[4] }
            };

            return new object[]
            {
                multiMap,
                intKeys,
                (ICollection<decimal>[])decimalValues,
                intKeysExcluded
            };
        }
    }
}