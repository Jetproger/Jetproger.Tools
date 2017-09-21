using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Xml.Bases
{
    public static partial class XmlExtensions
    {
        private static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        private static bool IsEnumNull(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArgumentType().IsEnum;
        }

        private static bool IsBool(this Type type)
        {
            return type == typeof(bool);
        }

        private static bool IsBoolNull(this Type type)
        {
            return type == typeof(bool?);
        }

        private static bool IsByte(this Type type)
        {
            return type == typeof(byte);
        }

        private static bool IsByteNull(this Type type)
        {
            return type == typeof(byte?);
        }

        private static bool IsBytes(this Type type)
        {
            return type == typeof(byte[]);
        }

        private static bool IsSbyte(this Type type)
        {
            return type == typeof(sbyte);
        }

        private static bool IsSbyteNull(this Type type)
        {
            return type == typeof(sbyte?);
        }

        private static bool IsChar(this Type type)
        {
            return type == typeof(char);
        }

        private static bool IsCharNull(this Type type)
        {
            return type == typeof(char?);
        }

        private static bool IsChars(this Type type)
        {
            return type == typeof(char[]);
        }

        private static bool IsShort(this Type type)
        {
            return type == typeof(short);
        }

        private static bool IsShortNull(this Type type)
        {
            return type == typeof(short?);
        }

        private static bool IsUshort(this Type type)
        {
            return type == typeof(ushort);
        }

        private static bool IsUshortNull(this Type type)
        {
            return type == typeof(ushort?);
        }

        private static bool IsInt(this Type type)
        {
            return type == typeof(int);
        }

        private static bool IsIntNull(this Type type)
        {
            return type == typeof(int?);
        }

        private static bool IsUint(this Type type)
        {
            return type == typeof(uint);
        }

        private static bool IsUintNull(this Type type)
        {
            return type == typeof(uint?);
        }

        private static bool IsLong(this Type type)
        {
            return type == typeof(long);
        }

        private static bool IsLongNull(this Type type)
        {
            return type == typeof(long?);
        }

        private static bool IsUlong(this Type type)
        {
            return type == typeof(ulong);
        }

        private static bool IsUlongNull(this Type type)
        {
            return type == typeof(ulong?);
        }

        private static bool IsGuid(this Type type)
        {
            return type == typeof(Guid);
        }

        private static bool IsGuidNull(this Type type)
        {
            return type == typeof(Guid?);
        }

        private static bool IsFloat(this Type type)
        {
            return type == typeof(float);
        }

        private static bool IsFloatNull(this Type type)
        {
            return type == typeof(float?);
        }

        private static bool IsDecimal(this Type type)
        {
            return type == typeof(decimal);
        }

        private static bool IsDecimalNull(this Type type)
        {
            return type == typeof(decimal?);
        }

        private static bool IsDouble(this Type type)
        {
            return type == typeof(double);
        }

        private static bool IsDoubleNull(this Type type)
        {
            return type == typeof(double?);
        }

        private static bool IsDateTime(this Type type)
        {
            return type == typeof(DateTime);
        }

        private static bool IsDateTimeNull(this Type type)
        {
            return type == typeof(DateTime?);
        }

        private static bool IsString(this Type type)
        {
            return type == typeof(string);
        }

        private static readonly HashSet<Type> _simpleTypes = new HashSet<Type>
        {
            typeof(bool), typeof(bool?),
            typeof(byte), typeof(byte?),
            typeof(sbyte), typeof(sbyte?),
            typeof(char), typeof(char?),
            typeof(short), typeof(short?), typeof(ushort), typeof(ushort?),
            typeof(int), typeof(int?), typeof(uint), typeof(uint?),
            typeof(IntPtr), typeof(IntPtr?), typeof(UIntPtr), typeof(UIntPtr?),
            typeof(long), typeof(long?), typeof(ulong), typeof(ulong?),
            typeof(Guid), typeof(Guid?),
            typeof(float), typeof(float?),
            typeof(decimal), typeof(decimal?),
            typeof(double), typeof(double?),
            typeof(DateTime), typeof(DateTime?),
            typeof(char[]), typeof(byte[]),
            typeof(string),
        };

        private static bool IsSimple(object value)
        {
            return value != null && IsSimple(value.GetType());
        }

        private static bool IsSimple(this Type type)
        {
            return type != null && (type.IsEnum || _simpleTypes.Contains(type));
        }

        private static Type GetGenericArgumentType(this Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length > 0) return types[0];
            var elementType = type.HasElementType ? type.GetElementType() : null;
            if (elementType != null) return elementType;
            var interfaces = type.GetInterfaces();
            foreach (var item in interfaces)
            {
                types = item.GetGenericArguments();
                if (types.Length > 0) return types[0];
                elementType = item.HasElementType ? item.GetElementType() : null;
                if (elementType != null) return elementType;
            }
            return null;
        }
    }
}