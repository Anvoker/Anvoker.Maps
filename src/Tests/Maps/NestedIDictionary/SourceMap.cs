using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace Anvoker.Collections.Tests.NestedIDictionary
{
    /// <summary>
    /// Contains test fixture sources used to initialize the tests in
    /// <see cref="NestedIDictionaryBase{TKey, TVal, TIDict, TValCol}"/>.
    /// </summary>
    public static class SourceMap
    {
        private static readonly ITestFixtureData[][] testFixtureSources =
            new ITestFixtureData[][]
            {
                MultiMapTests.NestedIDictionary_FixtureSource.GetFixtureArgs(),
                MultiBiMapTests.NestedIDictionary_FixtureSource.GetFixtureArgs()
            };

        /// <summary>
        /// Gets an array of <see cref="ITestFixtureData"/> collated from the
        /// test fixture sources this source map contains.
        /// </summary>
        public static object[] TestFixtureSources
            => CombineArraysFromSources();

        private static object[] CombineArraysFromSources()
        {
            var list = new List<object>();
            foreach (ITestFixtureData[] fixtureDataArray in testFixtureSources)
            {
                foreach (ITestFixtureData fixtureData in fixtureDataArray)
                {
                    list.Add(fixtureData);
                }
            }

            return list.ToArray();
        }
    }
}