using System.Runtime.InteropServices;

namespace Unity.Profiling;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ProfilerCategory
{
	public static ProfilerCategory Memory => default(ProfilerCategory);
}
