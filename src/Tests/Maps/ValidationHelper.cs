using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anvoker.Collections.Tests.Maps
{
    public static class ValidationHelper
    {
        public static void BothHaveEmptyCollectionThrow<T>(
            StringBuilder sb,
            T[][] arr1,
            T[][] arr2,
            string name1,
            string name2,
            string source)
        {
            var l = Math.Min(arr1.Length, arr2.Length);
            for (int i = 0; i < l; i++)
            {
                if (arr1[i].Length == 0 && arr2[i].Length == 0)
                {
                    sb.Append(name1).Append(" and ").Append(name2).Append(" ")
                        .Append("can't both be empty. ")
                        .Append("At index (i = ").Append(i).Append(") ")
                        .Append("in ")
                        .AppendLine(source);
                }
            }
        }

        public static void BothHaveEmptyCollectionThrow<T>(
            StringBuilder sb,
            T[] arr1,
            T[] arr2,
            string name1,
            string name2,
            string source)
        {
            if (arr1.Length == 0 && arr2.Length == 0)
            {
                sb.Append(name1).Append(" and ").Append(name2).Append(" ")
                    .Append("can't both be empty. ")
                    .Append("in ")
                    .AppendLine(source);
            }
        }

        public static void HasDuplicatesThrow<T>(
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

        public static void NullGuard<T>(
            StringBuilder sb,
            T value,
            string name,
            string source)
        {
            if (value == null)
            {
                sb.Append(name)
                  .Append("cannot be null in ")
                  .AppendLine(source);
            }
        }

        public static void HasOverlapThrow<T>(
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
                    .Append(" can't have any elements in common in ")
                    .Append(source)
                    .Append(". Common elements: ")
                    .AppendLine(sbTemp.ToString().Trim(' ', ','));
            }
        }

        public static void HasValueCollectionOverlapThrow<T>(
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

        public static void LengthDiffersThrow<T, K>(
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

        public static void LengthDiffersThrow<T, K>(
            StringBuilder sb,
            T[] arr1,
            K[] arr2,
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
    }
}