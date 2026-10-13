using System;
using System.Runtime.InteropServices;

namespace Unity.Profiling;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ProfilerMarker
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct AutoScope : IDisposable
	{
		public void Dispose()
		{
		}
	}

	public ProfilerMarker(string name)
	{
	}

	public AutoScope Auto()
	{
		return default(AutoScope);
	}
}
