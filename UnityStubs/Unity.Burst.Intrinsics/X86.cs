using System.Numerics;

namespace Unity.Burst.Intrinsics;

public static class X86
{
	public static class Sse
	{
		public static bool IsSseSupported => false;
	}

	public static class Sse2
	{
		public static bool IsSse2Supported => false;
	}

	public static class Sse3
	{
		public static bool IsSse3Supported => false;
	}

	public static class Ssse3
	{
		public static bool IsSsse3Supported => false;
	}

	public static class Sse4_1
	{
		public static bool IsSse41Supported => false;
	}

	public static class Sse4_2
	{
		public static bool IsSse42Supported => false;
	}

	public static class Popcnt
	{
		public static bool IsPopcntSupported => false;

		public static int popcnt_u32(uint v)
		{
			return BitOperations.PopCount(v);
		}

		public static int popcnt_u64(ulong v)
		{
			return BitOperations.PopCount(v);
		}
	}

	public static class Avx
	{
		public static bool IsAvxSupported => false;
	}

	public static class Avx2
	{
		public static bool IsAvx2Supported => false;
	}

	public static class Fma
	{
		public static bool IsFmaSupported => false;
	}

	public static class F16C
	{
		public static bool IsF16CSupported => false;
	}

	public static class Bmi1
	{
		public static bool IsBmi1Supported => false;
	}

	public static class Bmi2
	{
		public static bool IsBmi2Supported => false;
	}
}
