// ***********************************************************************
// Copyright (c) 2008-2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Compatibility;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using static NUnitFixtureDependent.ReflectionHelper;

namespace NUnit.Framework
{
    /// <summary>
    /// Indicates a source that will provide data for one parameter of a test
    /// method that is dependent on the arguments used to construct the Test
    /// Fixture. This ONLY works with a Test Fixture that uses arguments in
    /// its construction. So it works only when the Test Fixture has an
    /// attribute like <see cref="TestFixtureSourceAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class ValueDependentSourceAttribute : Attribute, IParameterDependentDataSource
    {
        private const BindingFlags ALL_BINDINGS = BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Static
                | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        #region Constructors

        /// <summary>
        /// Construct with a type and name. The type is used to locate an
        /// argument in the argument list of the Test Fixture constructor.
        /// The argument must either be of that type or inherit from that type.
        /// After the argument is located, <paramref name="sourceName"/> is used
        /// to retrieve data from it, by accessing a method, property or field
        /// by that name, that belongs to the located argument.
        /// </summary>
        /// <param name="sourceType">The Type of the fixture constructor
        /// argument that will provide data.</param>
        /// <param name="sourceName">The name of a method, property or field
        /// belonging to <paramref name="sourceType"/> that will provide
        /// data.</param>
        public ValueDependentSourceAttribute(
            Type sourceType,
            string sourceName)
        {
            SourceType = sourceType
                ?? throw new ArgumentNullException(nameof(sourceType));

            SourceName = sourceName
                ?? throw new ArgumentNullException(nameof(sourceName));

            if (sourceType.GetMember(sourceName, ALL_BINDINGS) == null)
            {
                throw new InvalidDataSourceException(
                    $"The {nameof(sourceName)} specified on a " +
                    $"{nameof(ValueDependentSourceAttribute)} must refer to a field, property or " +
                    $"method that belongs to {nameof(sourceType)}.");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the method, property or field, belonging to
        /// <see cref="SourceType"/>, to be used as a source.
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// The type of the Test Fixture argument to locate and use as a source.
        /// </summary>
        public Type SourceType { get; }

        #endregion

        #region IParameterDependentDataSource Members

        /// <summary>
        /// Gets an enumeration of data items for use as arguments
        /// for a test method parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which data is needed</param>
        /// <returns>
        /// An enumeration containing individual data items
        /// </returns>
        public IEnumerable GetData(IParameterInfo parameter, Test suite)
        {
            return GetDataSource(parameter, suite);
        }

        #endregion

        #region Helper Methods

        private IEnumerable GetDataSource(IParameterInfo parameter, Test suite)
        {
            var fixtureDataObject = LocateArgumentByType(suite.Arguments, SourceType);

            if (fixtureDataObject == null)
            {
                throw new InvalidDataSourceException(
                    $"The {nameof(SourceType)} specified on a " +
                    $"{nameof(ValueDependentSourceAttribute)} must refer to an argument of that " +
                    "type that exists in the argument list of the containing Test Fixture.");
            }

            var argumentType = fixtureDataObject.GetType();

            MemberInfo[] members = argumentType.GetMember(SourceName, ALL_BINDINGS);

            var dataSource = GetDataSourceValue(parameter, members, fixtureDataObject);

            if (dataSource == null)
            {
                throw new InvalidDataSourceException(
                    $"Could not retrieve a value from {argumentType}.{SourceName}. " +
                    $"{SourceName} is inaccessible via reflection or does not exist.");
            }

            return dataSource;
        }

        private static object LocateArgumentByType(object[] arguments, Type type)
        {
            foreach (var argument in arguments)
            {
                var argumentType = argument.GetType();
                if (type == argumentType || argumentType.IsSubclassOf(type))
                {
                    return argument;
                }
                else
                {
                    foreach (var i in argumentType.GetInterfaces())
                    {
                        if (i.GetGenericTypeDefinition() == type)
                        {
                            return argument;
                        }
                    }
                }
            }

            return null;
        }

        private static IEnumerable GetDataSourceValue(
            IParameterInfo parameter,
            MemberInfo[] members,
            object fixtureDataObject)
        {
            if (members.Length <= 0)
            {
                return null;
            }

            MemberInfo member = null;

            // If we have more than one member of the same name, this most
            // likely means that a member is hiding a member on a different
            // level of the inheritance hierarchy with the "new" keyword.
            // In this case we need to compare the type of the member and the
            // type of the parameter to figure out which member is the correct
            // one.
            if (members.Length > 1)
            {
                foreach (var m in members)
                {
                    var memberType = m.GetMemberUnderlyingType();
                    var elementType = GetCollectionElementType(memberType);
                    if (parameter.ParameterType.IsAssignableFrom(elementType))
                    {
                        member = m;
                    }
                }
            }
            else
            {
                member = members[0];
            }

            var value = member.GetValue(fixtureDataObject);

            return (value != null) ? (IEnumerable)value : null;
        }

        #endregion
    }
}