namespace Unity.Collections.LowLevel.Unsafe;

public static class NativeListUnsafeUtility
{
	public unsafe static T0* GetUnsafePtr<T0>(this NativeList<T0> list) where T0 : unmanaged
	{
		return (T0*)list.GetUnsafePtrInternal();
	}

	public unsafe static T0* GetUnsafeReadOnlyPtr<T0>(this NativeList<T0> list) where T0 : unmanaged
	{
		return (T0*)list.GetUnsafePtrInternal();
	}
}
