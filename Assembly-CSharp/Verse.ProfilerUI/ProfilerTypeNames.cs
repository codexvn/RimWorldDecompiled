using System;
using System.Collections.Generic;
using System.Text;

namespace Verse.ProfilerUI;

internal static class ProfilerTypeNames
{
	private static readonly Dictionary<Type, string> Cache = new Dictionary<Type, string>();

	private static readonly StringBuilder Builder = new StringBuilder(128);

	private static readonly Dictionary<Type, string> PrimitiveNames = new Dictionary<Type, string>
	{
		[typeof(void)] = "void",
		[typeof(bool)] = "bool",
		[typeof(byte)] = "byte",
		[typeof(sbyte)] = "sbyte",
		[typeof(char)] = "char",
		[typeof(short)] = "short",
		[typeof(ushort)] = "ushort",
		[typeof(int)] = "int",
		[typeof(uint)] = "uint",
		[typeof(long)] = "long",
		[typeof(ulong)] = "ulong",
		[typeof(float)] = "float",
		[typeof(double)] = "double",
		[typeof(decimal)] = "decimal",
		[typeof(object)] = "object",
		[typeof(string)] = "string",
		[typeof(nint)] = "nint",
		[typeof(nuint)] = "nuint"
	};

	public static string Get(Type type)
	{
		if (Cache.TryGetValue(type, out var value))
		{
			return value;
		}
		Builder.Clear();
		Append(Builder, type);
		value = Builder.ToString();
		Cache[type] = value;
		return value;
	}

	private static void Append(StringBuilder builder, Type type)
	{
		if (PrimitiveNames.TryGetValue(type, out var value))
		{
			builder.Append(value);
		}
		else if (type.IsArray)
		{
			Append(builder, type.GetElementType());
			builder.Append('[');
			builder.Append(',', type.GetArrayRank() - 1);
			builder.Append(']');
		}
		else if (type.IsGenericParameter)
		{
			builder.Append(type.Name);
		}
		else
		{
			Type[] genericArgs = (type.IsGenericType ? type.GetGenericArguments() : Type.EmptyTypes);
			AppendSegments(builder, type, genericArgs, out var _);
		}
	}

	private static void AppendSegments(StringBuilder builder, Type type, Type[] genericArgs, out int argsConsumed)
	{
		Type declaringType = type.DeclaringType;
		if ((object)declaringType != null)
		{
			AppendSegments(builder, declaringType, genericArgs, out argsConsumed);
			builder.Append('.');
		}
		else
		{
			argsConsumed = 0;
			if (!string.IsNullOrEmpty(type.Namespace))
			{
				builder.Append(type.Namespace);
				builder.Append('.');
			}
		}
		string name = type.Name;
		int num = name.IndexOf('`');
		if (num < 0)
		{
			builder.Append(name);
			return;
		}
		builder.Append(name, 0, num);
		if (!int.TryParse(name.AsSpan(num + 1), out var result) || result <= 0)
		{
			return;
		}
		builder.Append('<');
		for (int i = 0; i < result && argsConsumed + i < genericArgs.Length; i++)
		{
			if (i > 0)
			{
				builder.Append(", ");
			}
			Append(builder, genericArgs[argsConsumed + i]);
		}
		builder.Append('>');
		argsConsumed += result;
	}
}
