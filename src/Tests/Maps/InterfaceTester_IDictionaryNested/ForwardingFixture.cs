using System.Collections.Generic;
using Anvoker.Collections.Tests.Common;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps.NestedIDictionary
{
    [TestFixtureSource(
        typeof(SourceMap),
        nameof(SourceMap.TestFixtureSources))]
    public class ForwardingFixture<TKey, TVal, TIDict, TValCol> :
        IDictionaryNestedBase<TKey, TVal, TIDict, TValCol>
        where TIDict : IDictionary<TKey, TValCol>
        where TValCol : IEnumerable<TVal>
    {
        public ForwardingFixture(
            MapTestDataConcrete<TKey, TVal, TIDict, TValCol> args) : base(args)
        {
        }
    }
}