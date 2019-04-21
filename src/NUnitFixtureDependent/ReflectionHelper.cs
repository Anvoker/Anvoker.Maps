using System;
using System.Reflection;

namespace NUnitFixtureDependent
{
    public static class ReflectionHelper
    {
        public static Type GetMemberUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException("Input MemberInfo must be " +
                        "if type EventInfo, FieldInfo, MethodInfo, or " +
                        "PropertyInfo");
            }
        }

        public static Type GetCollectionElementType(Type collection)
        {
            if (collection == null || collection == typeof(string))
            {
                return null;
            }

            if (collection.IsArray)
            {
                return collection.GetElementType();
            }

            Type[] genericArguments;
            if (collection.IsGenericType
                && (genericArguments = collection.GetGenericArguments()).Length > 0)
            {
                return genericArguments[0];
            }

            Type[] ifaces = collection.GetInterfaces();

            if (ifaces?.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = GetCollectionElementType(iface);
                    if (ienum != null)
                    {
                        return ienum;
                    }
                }
            }

            if (collection.BaseType != null && collection.BaseType != typeof(object))
            {
                return GetCollectionElementType(collection.BaseType);
            }

            return null;
        }

        public static object GetValue(this MemberInfo member, object instance)
        {
            var field = member as FieldInfo;
            if (field != null)
            {
                return field.IsStatic
                    ? field.GetValue(null)
                    : field.GetValue(instance);
            }

            var property = member as PropertyInfo;
            if (property != null)
            {
                return property.GetMethod.IsStatic
                    ? property.GetValue(null, null)
                    : property.GetMethod.Invoke(instance, null);
            }

            var method = member as MethodInfo;
            if (method != null)
            {
                return method.IsStatic
                    ? method.Invoke(null, null)
                    : method.Invoke(instance, null);
            }

            throw new ArgumentException(
                $"Argument {nameof(member)} has to be {nameof(FieldInfo)}," +
                $"{nameof(PropertyInfo)}, or {nameof(MethodInfo)}.");
        }
    }
}
