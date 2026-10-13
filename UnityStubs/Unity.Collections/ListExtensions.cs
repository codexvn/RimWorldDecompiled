using System;
using System.Collections.Generic;

namespace Unity.Collections;

public static class ListExtensions
{
	public static NativeArray<T0> ToNativeArray<T0>(this List<T0> list, AllocatorManager.AllocatorHandle allocator)
	{
		ArgumentNullException.ThrowIfNull(list, "list");
		NativeArray<T0> result = new NativeArray<T0>(list.Count, allocator.ToAllocator, NativeArrayOptions.UninitializedMemory);
		for (int i = 0; i < list.Count; i++)
		{
			result[i] = list[i];
		}
		return result;
	}
}
