using NUnit.Framework.Interfaces;
using static Anvoker.Collections.Tests.Common.HelperMethods;

namespace Anvoker.Collections.Tests.Maps.NestedIDictionary
{
    /// <summary>
    /// Contains test fixture sources used to initialize the tests in
    /// <see cref="ForwardingFixture{TKey, TVal, TIDict, TValCol}"/>.
    /// </summary>
    public static class SourceMap
    {
        private static readonly ITestFixtureData[][] testFixtureSources =
            new ITestFixtureData[][]
            {
                MultiMap.IDictionaryNested_FixtureSource.GetFixtureArgs(),
                MultiBiMap.IDictionaryNested_FixtureSource.GetFixtureArgs()
            };

        /// <summary>
        /// Gets an array of <see cref="ITestFixtureData"/> collated from the
        /// test fixture sources this source map contains.
        /// </summary>
        public static object[] TestFixtureSources
            => CombineArraysFromSources(testFixtureSources);
    }
}