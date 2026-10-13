using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityStubs.Internal;

internal static class ShaderArrays
{
	public static ReadOnlySpan<T> Check<T>(T[] values)
	{
		return CheckNotEmpty<T>(values ?? throw new ArgumentNullException("values"));
	}

	public static ReadOnlySpan<T> Check<T>(List<T> values)
	{
		return CheckNotEmpty<T>(CollectionsMarshal.AsSpan(values ?? throw new ArgumentNullException("values")));
	}

	private static ReadOnlySpan<T> CheckNotEmpty<T>(ReadOnlySpan<T> values)
	{
		if (!values.IsEmpty)
		{
			return values;
		}
		throw new ArgumentException("Zero-sized array is not allowed.");
	}

	public static void Set<T>(Dictionary<int, T[]> arrays, int nameID, ReadOnlySpan<T> values)
	{
		if (arrays.TryGetValue(nameID, out var value))
		{
			values.Slice(0, Math.Min(values.Length, value.Length)).CopyTo(value);
		}
		else
		{
			arrays[nameID] = values.ToArray();
		}
	}

	public static void Fill<T>(List<T> list, T[] values)
	{
		ArgumentNullException.ThrowIfNull(list, "list");
		list.Clear();
		if (values != null)
		{
			list.AddRange(values);
		}
	}
}
