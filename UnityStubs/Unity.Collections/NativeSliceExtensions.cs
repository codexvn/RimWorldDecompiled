namespace Unity.Collections;

public static class NativeSliceExtensions
{
	public static NativeSlice<T0> Slice<T0>(this NativeArray<T0> thisArray, int start, int length)
	{
		return new NativeSlice<T0>(thisArray, start, length);
	}

	public static NativeSlice<T0> Slice<T0>(this NativeArray<T0> thisArray, int start)
	{
		return new NativeSlice<T0>(thisArray, start);
	}

	public static NativeSlice<T0> Slice<T0>(this NativeArray<T0> thisArray)
	{
		return new NativeSlice<T0>(thisArray);
	}
}
