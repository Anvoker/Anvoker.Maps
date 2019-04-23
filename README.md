# Anvoker.Maps

Provides robust map collections for C# with high compatibility with core collection interfaces.

In almost every place you could use a normal Dictionary, you can also use a map from this repository due to them implementing all the relevant interfaces.

An important goal of this repository is to have extensive unit testing that can confirm a class correctly implements a map interface. This goal has been largely achieved and the structure of the testing suite is explained in [Testing](#testing).

## Table of Contents ##

- [Styles](#styles)
- [Implementations](#implementations)
- [Features & Complexity](#features-complexity)
- [Testing](#testing)
- [Contributing](#contributing)
- [Roadmap](#roadmap)

## Styles
There are three "styles" of maps available, represented by their respective interfaces:
- ``IBiMap<T, K>`` is like a dictionary where keys can also be retrieved by their associated value.
- ``IMultiMap<T, K>`` is like a dictionary where each key may be associated with multiple values. No duplicate values may exist on the same key.
- ``IMultiBiMap<T, K>`` combines the properties of the previous two at the cost of additional space.

Note that BiMaps and MultiBiMaps are not bijections because they are not injective, only surjective. In other words, they are not one-to-one mappings of keys to values because several keys might have the same value. If having strict one to one mappings is important, these interfaces will not satisfy that specific requirement.

## Implementations
So far the only implementation is the composite "family". Named thusly because the backing store uses regular dictionaries.
- ``CompositeBiMap<T, K>``. Uses two dictionaries, one for forward lookup and one for reverse lookup.
- ``CompositeMultiMap<T, K>``. Uses a dictionary that maps keys to value hashsets.
- ``CompositeMultiBiMap<T, K>``. Uses three dictionaries. One that maps keys to value hashsets, a second that maps value hashsets to keys, and a third dictionary that maps unique values to key hashsets.

## Features & Complexity
| Implementation      | Forward Lookup | Reverse Lookup | Add         | Remove      | ContainsKey | ContainsValue | Space            |
| ------------------- | -------------- | -------------- | ----------- | ----------- | ----------- | ------------- | ---------------- |
| CompositeBiMap      | O(1) / O(n)    | O(1) / O(n)    | O(1) / O(n) | O(1) / O(n) | O(1) / O(n) | O(1) / O(n)   | O(2\*k + vu)     |
| CompositeMultiMap   | O(1) / O(n)    | N/A            | O(1) / O(n) | O(1) / O(n) | O(1) / O(n) | O(k) / O(k^2) | O(k + v)         |
| CompositeMultiBiMap | O(1) / O(n)    | O(1) / O(n)    | O(1) / O(n) | O(1) / O(n) | O(1) / O(n) | O(1) / O(n)   | O(3\*k + v + vu) | 

Note: values to the left of the slash indicate the "average" case, generally signifying the absence of hash collisions. The value to the right indicates the actual worst case time complexity.
- n  => generic item count.
- k  => number of keys.
- v  => number of values.
- vu => number of unique values.

## Testing
(Relevant only to contributors)

Anvoker.Maps contains a heavy-duty unit testing suite that confirms almost every single interface, including standard library ones, is implemented correctly in the concrete collections. The design pattern used in the testing suite allows the same test logic and data to be reused for new implementations. No additional test logic has to be written for interfaces that already have an interface tester fixture defined. New test logic is required only for completely new interfaces or for methods belonging directly to the concrete class.

The testing suite is separated into two different projects.

### Tests.Common.csproj

Contains all the actual test logic and interfaces. 

Test fixtures for every relevant interface are defined here in ``Anvoker.Maps.Tests.Common\InterfaceTesters``.  

Interfaces for the test data are defined in ``Anvoker.Maps.Tests.Common\Interfaces`` which allow the same test data to be adapted for more than one "kind" of test fixture. 

``MapData<TK, TV>`` and ``MultiMap<TK, TV>`` are data classes used for fixtures that test maps/dictionaries and multimaps respectively.

``MapDataConcrete<TK, TV>`` and ``MultiMapConcrete<TK, TV>`` are refinements whose most important feature is containing a ``Func`` pointing to the concrete implementation's constructor. This allows test fixtures to freely create instances of the implementation despite having access only to one of its interfaces.

``MapFixtureParamConstructor`` and ``MultiMapFixtureParamConstructor`` are constructors of fixture parameters which allow us to specify the type arguments of the fixtures. This is important because without them NUnit would fail to correctly infer the type arguments.

### Tests.csproj

Contains all the data and boilerplate that determines what tests will ultimately be created.

``MapDataSource`` and ``MultiMapDataSource`` are static classes that each define 4 data sets - IntDecimal, StringStringInsensitive, StringStringSensitive and ArrayIntType.

``MapDataSourceValidator`` and ``MultiMapDataSourceValidator`` run unit tests on the defined data to make sure it's correct, running null checks, verifying key uniqueness etc.

Each 'family' of maps gets its own directory. The directory contains a number of FixtureSources classes. Each FixtureSource contains a static property that returns an array of ``ITestFixtureData``. This can be used by ``TestFixtureSource`` to instantiate a test fixture to test a specific implementation with a specific data set. Every family in part needs one FixtureSource class per interface being tested. The FixtureSource class calls a method in ``MapFixtureParamConstructor`` or ``MultiMapFixtureParamConstructor`` to obtain the constructed test fixture parameters.

For each interface tester fixture defined in Tests.Common, there is corresponding ForwardingFixture class here. A ForwardingFixture class is nothing more than an empty test fixture that inherits from its respective interface tester fixture. This is done firstly so that we can specify new implementations to test without modifying the interface tester fixture in Tests.Common, and secondly so that we can specify some concrete types if needed.

### An example

In the case of testing ``CompositeBiMap``'s implementation of ``IReadOnlyDictionary``, the FixtureSource would look like this:

```csharp
using static Anvoker.Maps.Tests.BiMap.BiMapHelpers;
using static Anvoker.Maps.Tests.MapDataSource;

namespace Anvoker.Maps.Tests.BiMap
{
    public static class FixtureSource_IReadOnlyDictionary
    {
        private static readonly string fixtureName
            = typeof(IReadOnlyDictionary<,>).Name;

        public static ITestFixtureData[] GetArgs
        { get; } = new ITestFixtureData[]
        {
            MapFixtureParamConstructor<int, decimal, IReadOnlyBiMap<int, decimal>>
                .Construct(CompositeCtor, IntDecimal, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IReadOnlyBiMap<string, string>>
                .Construct(CompositeCtor, StringStringInsensitive, Name, fixtureName),

            MapFixtureParamConstructor<string, string, IReadOnlyBiMap<string, string>>
                .Construct(CompositeCtor, StringStringSensitive, Name, fixtureName),

            MapFixtureParamConstructor<int[], Type, IReadOnlyBiMap<int[], Type>>
                .Construct(CompositeCtor, ArrayIntType, Name, fixtureName),
        };
    }
}
```

This construct method takes a ``Func`` delegate in its first parameter, in this case CompositeCtor, which later allows the test fixture to make new instances of the implementation without knowing what the implementation is. The delegate is implictly created simply by using the name of the method defined in ``Anvoker.Maps.Tests.BiMap.BiMapHelpers``. All this method does is take keys and values from a ``MapData`` instance and construct a new ``CompositeBiMap`` instance with them.

```
public static CompositeBiMap<TKey, TVal> CompositeCtor<TKey, TVal>(MapData<TKey, TVal> d)
{
    var m = new CompositeBiMap<TKey, TVal>(d.ComparerKey, d.ComparerValue);
    for (int i = 0; i < d.KeysInitial.Length; i++)
    {
        m.Add(d.KeysInitial[i], d.ValuesInitial[i]);
    }

    return m;
}
```

If we wanted to test an entirely new implementation we would just add another such constructor method to ``BiMapHelpers.cs`` and use it in calls to ``MapFixtureParamConstructor`` to assemble our test data as seen in ``FixtureSource_IReadOnlyDictionary``.

Finally we need to reference this test data in the appropriate ForwardingFixture through NUnit's ``TestFixtureSource`` attribute.

```csharp
[TestFixtureSource(
    typeof(BiMap.FixtureSource_IReadOnlyDictionary),
    nameof(BiMap.FixtureSource_IReadOnlyDictionary.GetArgs))]
public class FF_IReadOnlyDictionary<TKey, TVal, TCollection>
    : IReadOnlyDictionaryTester<TKey, TVal, TCollection>
    where TCollection : IReadOnlyDictionary<TKey, TVal>
{
    public FF_IReadOnlyDictionary(
        MapDataConcrete<TKey, TVal, TCollection> args) : base(args)
    { }
}
```

This is all that's needed in order to have generated 80 test cases that will confirm ``IReadOnlyDictionary`` is implemented correctly for ``CompositeBiMap``. It's a lot of boilerplate, but it allows a lot of sharing code of logic, vastly reducing the need to ever write new test logic.

## Contributing

Submitting pull requests for new implementations, bug fixes or additional test coverage is welcome, but new implementations will be accepted based on their quality, usefulness and lack of overlap with existing implementations. Basically don't make an implementation that achieves what another implementation has already achieved.

### Coding Style

Download StyleCop and use the ``Settings.StyleCop`` file in the respective project to respect the overall style of the repository. In large, the most important parts are:
- Allman style indentation with 4 spaces.
- Respecting 80 col limit. Minor exceptions for summaries or ridiculously long nested generic types.
- Preference for => notation where ever it can be used instead of a block.
- PascalCase for everything except private variables, local variables and parameters which are camelCase and constants which are ALL_CAPS.

## Roadmap
- Adding a red-black tree implementation of IMultiMap.
- Adding benchmarks to test implementation performance.
- Adding integration tests to confirm that combinations of successive method calls don't mess up an implementation.