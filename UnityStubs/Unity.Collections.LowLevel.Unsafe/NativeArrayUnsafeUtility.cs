namespace Unity.Collections.LowLevel.Unsafe;

public static class NativeArrayUnsafeUtility
{
	public unsafe static void* GetUnsafeBufferPointerWithoutChecks<T0>(NativeArray<T0> nativeArray)
	{
		return nativeArray.GetUnsafePtrInternal();
	}

	public unsafe static void* GetUnsafePtr<T0>(this NativeArray<T0> nativeArray)
	{
		return nativeArray.GetUnsafePtrInternal();
	}

	public unsafe static void* GetUnsafeReadOnlyPtr<T0>(this NativeArray<T0> nativeArray)
	{
		return nativeArray.GetUnsafeReadOnlyPtrInternal();
	}

	public unsafe static void* GetUnsafeReadOnlyPtr<T0>(this NativeArray<T0>.ReadOnly nativeArray)
	{
		return nativeArray.GetUnsafeReadOnlyPtrInternal();
	}

	public static void SetAtomicSafetyHandle<T>(ref NativeArray<T> array, AtomicSafetyHandle safetyHandle) where T : struct
	{
	}
}
