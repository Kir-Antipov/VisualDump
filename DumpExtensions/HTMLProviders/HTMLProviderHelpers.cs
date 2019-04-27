using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VisualDump.HTMLProviders
{
    public static class HTMLProviderHelpers
    {
        public static StringBuilder AppendFullTypeIfNotSystem(this StringBuilder Builder, string TypeName)
        {
            if (!TypeName.StartsWith("System"))
                Builder                        
                    .Append("<tr>")
                        .Append("<th class='type' colspan='100'>")
                            .Append(TypeName)
                        .Append("</th>")
                    .Append("</tr>");
            return Builder;
        }
        public static StringBuilder AppendFullTypeIfNotSystem(this StringBuilder Builder, Type T) => IsAnonymous(T) ? Builder : AppendFullTypeIfNotSystem(Builder, T.FullName);
        public static StringBuilder AppendTypeName(this StringBuilder Builder, Type T) => Builder.Append(T.GetName());

        public static bool IsAnonymous(this Type T) =>
            Attribute.IsDefined(T, typeof(CompilerGeneratedAttribute), false)
        && T.IsGenericType && T.Name.Contains("AnonymousType")
        && (T.Name.StartsWith("<>") || T.Name.StartsWith("VB$"))
        && T.Attributes.HasFlag(TypeAttributes.NotPublic);

        private static readonly Dictionary<TypeCode, string> Names = new Dictionary<TypeCode, string> {
            { TypeCode.Boolean, "bool" },
            { TypeCode.Byte, "byte" },
            { TypeCode.Char, "char" },
            { TypeCode.DateTime, "DateTime" },
            { TypeCode.DBNull, "DBNull" },
            { TypeCode.Decimal, "decimal" },
            { TypeCode.Double, "double" },
            { TypeCode.Empty, "null" },
            { TypeCode.Int16, "short" },
            { TypeCode.Int32, "int" },
            { TypeCode.Int64, "long" },
            { TypeCode.SByte, "sbyte" },
            { TypeCode.Single, "float" },
            { TypeCode.String, "string" },
            { TypeCode.UInt16, "ushort" },
            { TypeCode.UInt32, "uint" },
            { TypeCode.UInt64, "ulong" }
        };
        private static readonly HashSet<string> DefaultNamespaces = new HashSet<string> {
            string.Empty,
            "System",
            "System.Collections.Generic"
        };
        private const string AnonymousName = "\u00D8";
        public static string GetName(this Type T)
        {
            if (T.IsAnonymous())
                return AnonymousName;
            if (Names.TryGetValue(Type.GetTypeCode(T), out string value))
                return value;
            if (T == typeof(object))
                return "object";
            if (T.IsArray)
                return $"{T.GetElementType().GetName()}[{new string(',', T.GetArrayRank() - 1)}]";
            if (T.IsGenericType && !T.IsGenericTypeDefinition)
                return $"{T.GetGenericTypeDefinition().GetName()}<{string.Join(", ", T.GetGenericArguments().Select(x => x.GetName()))}>";
            string name = T.Name;
            if (T.IsGenericTypeDefinition)
                name = name.Substring(0, name.LastIndexOf('`'));
            return DefaultNamespaces.Contains(T.Namespace) ? name : $"{T.Namespace}.{name}";
        }
    }
}
