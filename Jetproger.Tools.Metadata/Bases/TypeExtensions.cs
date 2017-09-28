using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Tools
{
    public static partial class Metadata
    {
        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        public static bool IsEnumNull(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArgumentType().IsEnum;
        }

        public static bool IsBool(this Type type)
        {
            return type == typeof(bool);
        }

        public static bool IsBoolNull(this Type type)
        {
            return type == typeof(bool?);
        }

        public static bool IsByte(this Type type)
        {
            return type == typeof(byte);
        }

        public static bool IsByteNull(this Type type)
        {
            return type == typeof(byte?);
        }

        public static bool IsBytes(this Type type)
        {
            return type == typeof(byte[]);
        }

        public static bool IsSbyte(this Type type)
        {
            return type == typeof(sbyte);
        }

        public static bool IsSbyteNull(this Type type)
        {
            return type == typeof(sbyte?);
        }

        public static bool IsChar(this Type type)
        {
            return type == typeof(char);
        }

        public static bool IsCharNull(this Type type)
        {
            return type == typeof(char?);
        }

        public static bool IsChars(this Type type)
        {
            return type == typeof(char[]);
        }

        public static bool IsShort(this Type type)
        {
            return type == typeof(short);
        }

        public static bool IsShortNull(this Type type)
        {
            return type == typeof(short?);
        }

        public static bool IsUshort(this Type type)
        {
            return type == typeof(ushort);
        }

        public static bool IsUshortNull(this Type type)
        {
            return type == typeof(ushort?);
        }

        public static bool IsInt(this Type type)
        {
            return type == typeof(int);
        }

        public static bool IsIntNull(this Type type)
        {
            return type == typeof(int?);
        }

        public static bool IsUint(this Type type)
        {
            return type == typeof(uint);
        }

        public static bool IsUintNull(this Type type)
        {
            return type == typeof(uint?);
        }

        public static bool IsLong(this Type type)
        {
            return type == typeof(long);
        }

        public static bool IsLongNull(this Type type)
        {
            return type == typeof(long?);
        }

        public static bool IsUlong(this Type type)
        {
            return type == typeof(ulong);
        }

        public static bool IsUlongNull(this Type type)
        {
            return type == typeof(ulong?);
        }

        public static bool IsGuid(this Type type)
        {
            return type == typeof(Guid);
        }

        public static bool IsGuidNull(this Type type)
        {
            return type == typeof(Guid?);
        }

        public static bool IsFloat(this Type type)
        {
            return type == typeof(float);
        }

        public static bool IsFloatNull(this Type type)
        {
            return type == typeof(float?);
        }

        public static bool IsDecimal(this Type type)
        {
            return type == typeof(decimal);
        }

        public static bool IsDecimalNull(this Type type)
        {
            return type == typeof(decimal?);
        }

        public static bool IsDouble(this Type type)
        {
            return type == typeof(double);
        }

        public static bool IsDoubleNull(this Type type)
        {
            return type == typeof(double?);
        }

        public static bool IsDateTime(this Type type)
        {
            return type == typeof(DateTime);
        }

        public static bool IsDateTimeNull(this Type type)
        {
            return type == typeof(DateTime?);
        }

        public static bool IsString(this Type type)
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

        public static bool IsSimple(object value)
        {
            return value != null && IsSimple(value.GetType());
        }

        public static bool IsSimple(this Type type)
        {
            return type != null && (type.IsEnum || _simpleTypes.Contains(type));
        }

        public static bool IsNullable(this Type type)
        {
            return type == typeof(Nullable<>);
        }

        private static readonly HashSet<Type> _integerTypes = new HashSet<Type>
        {
            typeof(byte), typeof(byte?),
            typeof(sbyte), typeof(sbyte?),
            typeof(short), typeof(short?), typeof(ushort), typeof(ushort?),
            typeof(int), typeof(int?), typeof(uint), typeof(uint?),
            typeof(long), typeof(long?), typeof(ulong), typeof(ulong?),
        };

        public static bool IsInteger(this Type type)
        {
            return type != null && _integerTypes.Contains(type);
        }

        private static readonly HashSet<Type> _fractionalTypes = new HashSet<Type>
        {
            typeof(float), typeof(float?),
            typeof(decimal), typeof(decimal?),
            typeof(double), typeof(double?),
        };

        public static bool IsFractional(this Type type)
        {
            return type != null && _fractionalTypes.Contains(type);
        }

        public static bool IsTypeOf(this Type type, Type sample)
        {
            return type == sample || type.IsSubclassOf(sample) || type.GetInterfaces().Any(interfaceType => interfaceType == sample);
        }

        public static Type GetGenericArgumentType(this Type type)
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

        public static Type GetDeclaredType(this object o)
        {
            return o != null ? GetDeclaredType(o.GetType()) : null;
        }

        public static Type GetDeclaredType(this Type type)
        {
            return type.FullName.ToLower().StartsWith("dynamicmodule") ? type.BaseType : type;
        }

        public static string AsSqlScript(this SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar: return $"[{sqlDbType}](/*SIZE*/) COLLATE /*COLLATE*/ NOT NULL";
                case SqlDbType.Binary:
                case SqlDbType.VarBinary: return $"[{sqlDbType}](/*SIZE*/) NOT NULL";
                case SqlDbType.Text:
                case SqlDbType.NText: return $"[{sqlDbType}] COLLATE /*COLLATE*/ NOT NULL";
                default: return $"[{sqlDbType}] NOT NULL";
            }
        }

        public static string AsSqlScriptNull(this SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar: return $"[{sqlDbType}](/*SIZE*/) COLLATE /*COLLATE*/ NULL";
                case SqlDbType.Binary:
                case SqlDbType.VarBinary: return $"[{sqlDbType}](/*SIZE*/) NOT NULL";
                case SqlDbType.Timestamp: return $"[{sqlDbType}] NOT NULL";
                case SqlDbType.Text:
                case SqlDbType.NText: return $"[{sqlDbType}] COLLATE /*COLLATE*/ NULL";
                default: return $"[{sqlDbType}] NULL";
            }
        }
    }
}