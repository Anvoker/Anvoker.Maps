using System;
using Anvoker.Maps.Tests.Common;

namespace Anvoker.Maps.Tests
{
    /// <summary>
    /// Provides key and values appropriate for testing multimaps.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Naked data class.")]
    public static class MultiMapDataSource
    {
        public static readonly MultiMapData<int, decimal> IntDecimal
            = new MultiMapData<int, decimal>(
                testDataName: nameof(IntDecimal),
                keysInitial: new int[] { 0, int.MaxValue, int.MinValue },
                keysToAdd: new int[] { 900, 901 },
                keysExcluded: new int[] { 24, 2 },
                valuesInitial: new decimal[][]
                {
                    new decimal[] { 0.1m },
                    new decimal[] { 5.25m, 0.0m },
                    new decimal[] { },
                },
                valuesToAdd: new decimal[][]
                {
                    new decimal[] { 0.1m },
                    new decimal[] { 5.25m, 0.0m, 25.0m },
                },
                valuesExcluded: new decimal[][]
                {
                    new decimal[] { 0.55m, 0.777m },
                    new decimal[] { 0.025m, 999.9m, 99.0m },
                },
                comparerKey: null,
                comparerValue: null);

        public static readonly
            MultiMapData<string, string> StringStringSensitive
            = new MultiMapData<string, string>(
                testDataName: nameof(StringStringSensitive),
                keysInitial: new string[] { "nyaa", "nyAA", string.Empty },
                keysToAdd: new string[] { "meow", "meoW" },
                keysExcluded: new string[] { "NYAA", "NYaA" },
                valuesInitial: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { "aaaa", "bbbb", "cccc" },
                    new string[] { },
                },
                valuesToAdd: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { },
                },
                valuesExcluded: new string[][]
                {
                    new string[] { "acde" },
                    new string[] { "acde", "\"\"", "wwww" },
                },
                comparerKey: StringComparer.InvariantCulture,
                comparerValue: StringComparer.InvariantCulture);

        public static readonly
            MultiMapData<string, string> StringStringInsensitive
            = new MultiMapData<string, string>(
                testDataName: nameof(StringStringInsensitive),
                keysInitial: new string[] { "nyaa1", "nyaa2", string.Empty },
                keysToAdd: new string[] { "meow", "\"\"", },
                keysExcluded: new string[] { "Nyaa3", " " },
                valuesInitial: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { "aaAA", "bbbb", "cccc" },
                    new string[] { },
                },
                valuesToAdd: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { },
                },
                valuesExcluded: new string[][]
                {
                    new string[] { "acde" },
                    new string[] { "zzzz", "\"\"", "wwww" },
                },
                comparerKey: StringComparer.InvariantCultureIgnoreCase,
                comparerValue: StringComparer.InvariantCultureIgnoreCase);

        public static readonly
            MultiMapData<int[], Type> ArrayIntType
            = new MultiMapData<int[], Type>(
                testDataName: nameof(ArrayIntType),
                keysInitial: new int[][]
                {
                    new int[0],
                    new int[] { 0, 7 },
                    new int[] { 5, 7 },
                },
                keysToAdd: new int[][]
                {
                    new int[0],
                    new int[] { 1, 2, 5 }
                },
                keysExcluded: new int[][]
                {
                    new int[] { 0, 7 },
                    new int[] { 1, 2, 5 }
                },
                valuesInitial: new Type[][]
                {
                    new Type[] { typeof(int), null },
                    new Type[] { },
                    new Type[] { typeof(int), typeof(float) },
                },
                valuesToAdd: new Type[][]
                {
                    new Type[] { typeof(int) },
                    new Type[] { },
                },
                valuesExcluded: new Type[][]
                {
                    new Type[] { typeof(long) },
                    new Type[] { typeof(uint), typeof(DateTime) },
                },
                comparerKey: null,
                comparerValue: null);
    }
}