using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Anvoker.Collections.Tests.Common;
using System.Reflection;
using NUnit.Framework;

namespace Anvoker.Collections.Tests.Maps
{
    [TestFixture, Order(0)]
    public class MapTestDataSourceValidator
    {
        private readonly static MethodInfo validateMethodUnbound =
                typeof(MapTestDataSourceValidator).GetMethod(
                nameof(Validate),
                BindingFlags.NonPublic | BindingFlags.Static);

        private readonly static Type genericMap
            = typeof(MapTestData<,>).GetGenericTypeDefinition();

        private readonly static FieldInfo[] fields
            = typeof(MapTestDataSource).GetFields(
                BindingFlags.Static | BindingFlags.Public);

        [Test]
        public void Validate()
        {
            var sb = new StringBuilder();

            foreach (FieldInfo field in fields)
            {
                var mapType = field.FieldType;
                if (mapType.GetGenericTypeDefinition() == genericMap)
                {
                    Type[] args = mapType.GetGenericArguments();
                    Type keyType = args[0];
                    Type valType = args[1];
                    object mapData = field.GetValue(null);

                    var validateMethod = validateMethodUnbound
                        .MakeGenericMethod(new Type[] { keyType, valType });

                    var result = validateMethod.Invoke(
                        null,
                        new object[] { mapData, field.Name });

                    string str = (string)result;
                    str = str.Trim();
                    if (str.Length > 0)
                    {
                        sb.AppendLine((string)result);
                        sb.AppendLine("=============");
                    }
                }
            }

            if (sb.Length > 0)
            {
                Assert.Fail(sb.ToString());
            }
            else
            {
                Assert.Pass();
            }
        }

        private static string Validate<TKey, TVal>(
            MapTestData<TKey, TVal> d,
            string name)
        {
            string source = nameof(MapTestDataSource) + "." + name;
            var sb = new StringBuilder();

            if (d.TestDataName == null)
            {
                sb.Append(nameof(MapTestData<TKey, TVal>.TestDataName))
                    .Append("cannot be null in ")
                    .AppendLine(source);
            }

            if (d.KeysInitial == null)
            {
                sb.Append(nameof(MapTestData<TKey, TVal>.KeysInitial))
                    .Append("cannot be null in ")
                    .AppendLine(source);
            }

            if (d.KeysToAdd == null)
            {
                sb.Append(nameof(MapTestData<TKey, TVal>.KeysToAdd))
                    .Append("cannot be null in ")
                    .AppendLine(source);
            }

            if (d.KeysExcluded == null)
            {
                sb.Append(nameof(MapTestData<TKey, TVal>.KeysExcluded))
                    .Append("cannot be null in ")
                    .AppendLine(source);
            }

            if (d.ValuesInitial == null)
            {
                sb.Append(nameof(MapTestData<TKey, TVal>.ValuesInitial))
                    .Append("cannot be null in ")
                    .AppendLine(source);
            }

            if (d.ValuesToAdd == null)
            {
                sb.Append(nameof(MapTestData<TKey, TVal>.ValuesToAdd))
                    .Append("cannot be null in ")
                    .AppendLine(source);
            }

            if (d.ValuesExcluded == null)
            {
                sb.Append(nameof(MapTestData<TKey, TVal>.ValuesExcluded))
                    .Append("cannot be null in ")
                    .AppendLine(source);
            }

            HasDuplicatesThrow(
                sb,
                d.KeysInitial,
                nameof(MapTestData<TKey, TVal>.KeysInitial),
                d.ComparerKey,
                source);

            HasDuplicatesThrow(
                sb,
                d.KeysToAdd,
                nameof(MapTestData<TKey, TVal>.KeysToAdd),
                d.ComparerKey,
                source);

            HasDuplicatesThrow(
                sb,
                d.KeysExcluded,
                nameof(MapTestData<TKey, TVal>.KeysExcluded),
                d.ComparerKey,
                source);

            LengthDiffersThrow(
                sb,
                d.KeysInitial,
                d.ValuesInitial,
                nameof(MapTestData<TKey, TVal>.KeysInitial),
                nameof(MapTestData<TKey, TVal>.ValuesInitial),
                source);

            LengthDiffersThrow(
                sb,
                d.KeysToAdd,
                d.ValuesToAdd,
                nameof(MapTestData<TKey, TVal>.KeysToAdd),
                nameof(MapTestData<TKey, TVal>.ValuesToAdd),
                source);

            LengthDiffersThrow(
                sb,
                d.KeysExcluded,
                d.ValuesExcluded,
                nameof(MapTestData<TKey, TVal>.KeysExcluded),
                nameof(MapTestData<TKey, TVal>.ValuesExcluded),
                source);

            HasOverlapThrow(
                sb,
                d.KeysInitial,
                d.KeysToAdd,
                nameof(MapTestData<TKey, TVal>.KeysInitial),
                nameof(MapTestData<TKey, TVal>.KeysToAdd),
                d.ComparerKey,
                source);

            HasOverlapThrow(
                sb,
                d.KeysInitial,
                d.KeysExcluded,
                nameof(MapTestData<TKey, TVal>.KeysInitial),
                nameof(MapTestData<TKey, TVal>.KeysExcluded),
                d.ComparerKey,
                source);

            HasOverlapThrow(
                sb,
                d.ValuesInitial,
                d.ValuesExcluded,
                nameof(MapTestData<TKey, TVal>.ValuesInitial),
                nameof(MapTestData<TKey, TVal>.ValuesExcluded),
                d.ComparerValue,
                source);

            return sb.ToString();
        }

        private static void HasOverlapThrow<T>(
            StringBuilder sb,
            T[] arr1,
            T[] arr2,
            string name1,
            string name2,
            IEqualityComparer<T> comparer,
            string source)
        {
            if (new HashSet<T>(arr1, comparer).Overlaps(
                new HashSet<T>(arr2, comparer)))
            {
                var common = new HashSet<T>(arr1, comparer);
                common.IntersectWith(new HashSet<T>(arr2, comparer));
                var sbTemp = new StringBuilder();
                common.All(x => sbTemp.Append(x).Append(", ") != null);
                sb.Append(name1).Append(" and ").Append(name2)
                    .Append(" can't have any keys in common in ")
                    .Append(source)
                    .Append(". Common elements: ")
                    .AppendLine(sbTemp.ToString().Trim(' ', ','));
            }
        }

        private static void HasOverlapThrow<T>(
            StringBuilder sb,
            T[][] arr1,
            T[][] arr2,
            string name1,
            string name2,
            IEqualityComparer<T> comparer,
            string source)
        {
            var l = Math.Min(arr1.Length, arr2.Length);
            for (int i = 0; i < l; i++)
            {
                if (new HashSet<T>(arr1[i], comparer).Overlaps(
                    new HashSet<T>(arr2[i], comparer)))
                {
                    var common = new HashSet<T>(arr1[i], comparer);
                        common.IntersectWith(new HashSet<T>(arr2[i], comparer));
                    var sbTemp = new StringBuilder();
                    common.All(x => sbTemp.Append(x).Append(", ") != null);
                    sb.Append(name1).Append(" and ").Append(name2).Append(" ")
                        .Append("can't have value collections on the same ")
                        .Append(" index (i = ").Append(i).Append(") ")
                        .Append("that have any value in common in ")
                        .Append(source)
                        .Append(". Common elements: ")
                        .AppendLine(sbTemp.ToString().Trim(' ', ','));
                }
            }
        }

        private static void LengthDiffersThrow<T, K>(
            StringBuilder sb,
            T[] arr1,
            K[][] arr2,
            string name1,
            string name2,
            string source)
        {
            if (arr1.Length != arr2.Length)
            {
                sb.Append(name1).Append(" and ").Append(name2).Append(" ")
                    .Append("must have equal length in ").Append(source);
            }
        }

        private static void HasDuplicatesThrow<T>(
            StringBuilder sb,
            T[] array,
            string name,
            IEqualityComparer<T> comparer,
            string source)
        {
            var hashset = new HashSet<T>(comparer);
            foreach (T item in array)
            {
                if (!hashset.Add(item))
                {
                    sb.Append(name)
                        .Append(" must have no duplicate keys in ")
                        .Append(source);
                }
            }
        }
    }
}
