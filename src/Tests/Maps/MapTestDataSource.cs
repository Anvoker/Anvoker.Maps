using System;
using System.Collections.Generic;

namespace Anvoker.Tests.Collections
{
    /// <summary>
    /// Provides key and values appropriate for testing maps.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Naked data class.")]
    public static class MapTestDataSource
    {
        public static readonly MapTestData<int, decimal> IntDecimal
            = new MapTestData<int, decimal>(
                keysInitial: new int[] { 25, 37, 99, 20, -5 },
                keysToAdd: new int[] { 900, 901, 525 },
                keysExcluded: new int[] { 24, 2, -8 },
                valuesInitial: new decimal[][]
                {
                    new decimal[] { 0.1m },
                    new decimal[] { 5.25m, 0.0m },
                    new decimal[] { 5.25m, 0.0m, 2.0m, 5.0m },
                    new decimal[] { },
                    new decimal[] { 3.75m },
                },
                valuesToAdd: new decimal[][]
                {
                    new decimal[] { 99.1m },
                    new decimal[] { },
                    new decimal[] { 5.25m, 0.0m, 25.0m },
                },
                valuesExcluded: new decimal[][]
                {
                    new decimal[] { 5.25m, 0.0m },
                    new decimal[] { },
                    new decimal[] { 5.25m, 0.0m, 25.0m },
                },
                comparerKey: null,
                comparerValue: null);

        public static readonly
            MapTestData<string, string> StringStringCaseSensitive
            = new MapTestData<string, string>(
                keysInitial: new string[] { "nyaa", "nyAA", "\"\"", string.Empty },
                keysToAdd: new string[] { "meow", "meoW", "Meow" },
                keysExcluded: new string[] { "NYAA", "NYaA", " " },
                valuesInitial: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { "aaaa", "bbbb", "cccc" },
                    new string[] { },
                    new string[] { "ffff" },
                },
                valuesToAdd: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { },
                    new string[] { "ffff", "eeee" },
                },
                valuesExcluded: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { },
                    new string[] { "zzzz", "\"\"", "wwww" },
                },
                comparerKey: StringComparer.InvariantCulture,
                comparerValue: StringComparer.InvariantCulture);

        public static readonly
            MapTestData<string, string> StringStringCaseInsensitive
            = new MapTestData<string, string>(
                keysInitial: new string[]
                {
                    "nyaa1", "nyaa2", string.Empty, "  "
                },
                keysToAdd: new string[] { "meow", "\"\"", "Meow3" },
                keysExcluded: new string[] { "Nyaa3", "nyaA4", " " },
                valuesInitial: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { "aaAA", "bbbb", "cccc" },
                    new string[] { },
                    new string[] { "ffff" },
                },
                valuesToAdd: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { },
                    new string[] { "ffff", "eeee" },
                },
                valuesExcluded: new string[][]
                {
                    new string[] { "aaaa" },
                    new string[] { },
                    new string[] { "zzzz", "\"\"", "wwww" },
                },
                comparerKey: StringComparer.InvariantCultureIgnoreCase,
                comparerValue: StringComparer.InvariantCultureIgnoreCase);

        public static readonly
            MapTestData<List<int>, Type> ListType
            = new MapTestData<List<int>, Type>(
                keysInitial: new List<int>[]
                {
                    new List<int>(),
                    new List<int>(),
                    new List<int>() { 5, 7 },
                    new List<int>() { 5, 7 },
                },
                keysToAdd: new List<int>[]
                {
                    new List<int>(),
                    new List<int>() { 1, 2, 3 },
                    new List<int>() { 1, 2, 5 }
                },
                keysExcluded: new List<int>[]
                {
                    new List<int>(),
                    new List<int>() { 1, 2, 3 },
                    new List<int>() { 1, 2, 5 }
                },
                valuesInitial: new Type[][]
                {
                    new Type[] { typeof(int) },
                    new Type[] { },
                    new Type[] { typeof(int), typeof(float) },
                    new Type[] { typeof(decimal) },
                },
                valuesToAdd: new Type[][]
                {
                    new Type[] { typeof(int) },
                    new Type[] { },
                    new Type[] { typeof(int?) },
                },
                valuesExcluded: new Type[][]
                {
                    new Type[] { typeof(int) },
                    new Type[] { },
                    new Type[] { typeof(DateTime) },
                },
                comparerKey: null,
                comparerValue: null);
    }
}