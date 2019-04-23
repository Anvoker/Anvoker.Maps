using System;
using System.Reflection;
using System.Text;
using Anvoker.Maps.Tests.Common;
using NUnit.Framework;
using static Anvoker.Maps.Tests.ValidationHelper;

namespace Anvoker.Maps.Tests
{
    [TestFixture]
    public class MapDataSourceValidator
    {
        private static readonly FieldInfo[] fields
            = typeof(MapDataSource).GetFields(BindingFlags.Static | BindingFlags.Public);

        private static readonly Type genericMap
            = typeof(MapData<,>).GetGenericTypeDefinition();

        private static readonly MethodInfo validateMethodUnbound
            = typeof(MapDataSourceValidator).GetMethod(
                nameof(Validate),
                BindingFlags.NonPublic | BindingFlags.Static);

        [Test]
        public void Validate([ValueSource(nameof(fields))] FieldInfo field)
        {
            var sb = new StringBuilder();

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
            MapData<TKey, TVal> d,
            string name)
        {
            string source = nameof(MapDataSource) + "." + name;
            var sb = new StringBuilder();

            NullGuard(sb, d.TestDataName, nameof(MapData<TKey, TVal>.TestDataName), source);
            NullGuard(sb, d.KeysInitial, nameof(MapData<TKey, TVal>.KeysInitial), source);
            NullGuard(sb, d.KeysToAdd, nameof(MapData<TKey, TVal>.KeysToAdd), source);
            NullGuard(sb, d.KeysExcluded, nameof(MapData<TKey, TVal>.KeysExcluded), source);
            NullGuard(sb, d.ValuesInitial, nameof(MapData<TKey, TVal>.ValuesInitial), source);
            NullGuard(sb, d.ValuesToAdd, nameof(MapData<TKey, TVal>.ValuesToAdd), source);
            NullGuard(sb, d.ValuesExcluded, nameof(MapData<TKey, TVal>.ValuesExcluded), source);

            HasDuplicatesThrow(
                sb,
                d.KeysInitial,
                nameof(MapData<TKey, TVal>.KeysInitial),
                d.ComparerKey,
                source);

            HasDuplicatesThrow(
                sb,
                d.KeysToAdd,
                nameof(MapData<TKey, TVal>.KeysToAdd),
                d.ComparerKey,
                source);

            HasDuplicatesThrow(
                sb,
                d.KeysExcluded,
                nameof(MapData<TKey, TVal>.KeysExcluded),
                d.ComparerKey,
                source);

            LengthDiffersThrow(
                sb,
                d.KeysInitial,
                d.ValuesInitial,
                nameof(MapData<TKey, TVal>.KeysInitial),
                nameof(MapData<TKey, TVal>.ValuesInitial),
                source);

            LengthDiffersThrow(
                sb,
                d.KeysToAdd,
                d.ValuesToAdd,
                nameof(MapData<TKey, TVal>.KeysToAdd),
                nameof(MapData<TKey, TVal>.ValuesToAdd),
                source);

            LengthDiffersThrow(
                sb,
                d.KeysExcluded,
                d.ValuesExcluded,
                nameof(MapData<TKey, TVal>.KeysExcluded),
                nameof(MapData<TKey, TVal>.ValuesExcluded),
                source);

            HasOverlapThrow(
                sb,
                d.KeysInitial,
                d.KeysToAdd,
                nameof(MapData<TKey, TVal>.KeysInitial),
                nameof(MapData<TKey, TVal>.KeysToAdd),
                d.ComparerKey,
                source);

            HasOverlapThrow(
                sb,
                d.KeysInitial,
                d.KeysExcluded,
                nameof(MapData<TKey, TVal>.KeysInitial),
                nameof(MapData<TKey, TVal>.KeysExcluded),
                d.ComparerKey,
                source);

            HasOverlapThrow(
                sb,
                d.ValuesInitial,
                d.ValuesExcluded,
                nameof(MapData<TKey, TVal>.ValuesInitial),
                nameof(MapData<TKey, TVal>.ValuesExcluded),
                d.ComparerValue,
                source);

            BothHaveEmptyCollectionThrow(
                sb,
                d.ValuesInitial,
                d.ValuesExcluded,
                nameof(MapData<TKey, TVal>.ValuesInitial),
                nameof(MapData<TKey, TVal>.ValuesExcluded),
                source);

            return sb.ToString();
        }
    }
}