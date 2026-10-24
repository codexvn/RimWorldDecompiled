using System;
using System.Runtime.InteropServices;

namespace Waylith.AssetBake;

internal static class NativeHeap
{
	private const int MmapThreshold = -3;

	public static void KeepLargeBlocksMapped()
	{
		if (OperatingSystem.IsLinux())
		{
			mallopt(-3, 1048576);
		}
	}

	public static void Trim()
	{
		if (OperatingSystem.IsLinux())
		{
			malloc_trim(0u);
		}
	}

	[DllImport("libc", ExactSpelling = true)]
	[LibraryImport("libc")]
	private static extern int mallopt(int parameter, int value);

	[DllImport("libc", ExactSpelling = true)]
	[LibraryImport("libc")]
	private static extern int malloc_trim(nuint pad);
}
