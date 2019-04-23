using System;
using Anvoker.Maps.Tests.Common;

namespace Anvoker.Maps.Tests
{
    /// <summary>
    /// Provides key and values appropriate for testing maps.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Naked data class.")]
    public static class MapDataSource
    {
        public static readonly MapData<int, decimal> IntDecimal
            = new MapData<int, decimal>(
                testDataName:   nameof(IntDecimal),
                keysInitial:    new int[] { 25, 37, 99 },
                keysToAdd:      new int[] { 900, -901 },
                keysExcluded:   new int[] { 24, 2 },
                valuesInitial:  new decimal[] { 0.1m, 5.25m, 0.0m },
                valuesToAdd:    new decimal[] { 0.1m, 25.0m },
                valuesExcluded: new decimal[] { 0.025m, 999.9m },
                comparerKey:    null,
                comparerValue:  null);

        public static readonly MapData<string, string> StringStringSensitive
            = new MapData<string, string>(
                testDataName:   nameof(StringStringSensitive),
                keysInitial:    new string[] { "nyaa", "nyAA", string.Empty },
                keysToAdd:      new string[] { "nYAA", "meoW" },
                keysExcluded:   new string[] { "NYAA", " " },
                valuesInitial:  new string[] { "aaaa", "bbbb", "cccc" },
                valuesToAdd:    new string[] { "aaaa", null },
                valuesExcluded: new string[] { "acde", "\"\"", },
                comparerKey:    StringComparer.InvariantCulture,
                comparerValue:  StringComparer.InvariantCulture);

        public static readonly MapData<string, string> StringStringInsensitive
            = new MapData<string, string>(
                testDataName:   nameof(StringStringInsensitive),
                keysInitial:    new string[] { "nyaa1", "nyaa2", string.Empty },
                keysToAdd:      new string[] { "meow", "aaaa" },
                keysExcluded:   new string[] { "Nyaa3", "nyaA4" },
                valuesInitial:  new string[] { "aaAA", "bbbb", null },
                valuesToAdd:    new string[] { "ffff",  null },
                valuesExcluded: new string[] { "zzzz", "\"\"", },
                comparerKey:    StringComparer.InvariantCultureIgnoreCase,
                comparerValue:  StringComparer.InvariantCultureIgnoreCase);

        public static readonly MapData<int[], Type> ArrayIntType
            = new MapData<int[], Type>(
                testDataName: nameof(ArrayIntType),
                keysInitial:  new int[][]
                {
                    new int[0],
                    new int[] { 5, 7 },
                    new int[] { 0, 7 },
                },
                keysToAdd: new int[][]
                {
                    new int[0],
                    new int[] { 1, 2, 3 },
                },
                keysExcluded: new int[][]
                {
                    new int[] { 2 },
                    new int[] { 1, 2, 3 },
                },
                valuesInitial:  new Type[] { typeof(int), typeof(int), typeof(float) },
                valuesToAdd:    new Type[] { typeof(int?), null },
                valuesExcluded: new Type[] { typeof(DateTime), typeof(byte) },
                comparerKey:    null,
                comparerValue:  null);
    }
}