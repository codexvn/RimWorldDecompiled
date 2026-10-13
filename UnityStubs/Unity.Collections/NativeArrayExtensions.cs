using System;

namespace Unity.Collections;

public static class NativeArrayExtensions
{
	public static bool Contains<T0, T1>(this NativeArray<T0>.ReadOnly array, T1 value) where T0 : struct, IEquatable<T1>
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Equals(value))
			{
				return true;
			}
		}
		return false;
	}
}
