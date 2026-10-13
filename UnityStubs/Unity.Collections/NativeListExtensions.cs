using System;

namespace Unity.Collections;

public static class NativeListExtensions
{
	public static bool Contains<T0, T1>(this NativeList<T0> list, T1 value) where T0 : unmanaged, IEquatable<T1>
	{
		for (int i = 0; i < list.Length; i++)
		{
			if (list[i].Equals(value))
			{
				return true;
			}
		}
		return false;
	}
}
