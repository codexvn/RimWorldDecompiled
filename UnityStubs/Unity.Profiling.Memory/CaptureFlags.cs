using System;

namespace Unity.Profiling.Memory;

[Flags]
public enum CaptureFlags : uint
{
	ManagedObjects = 1u,
	NativeAllocations = 4u,
	NativeAllocationSites = 8u,
	NativeObjects = 2u,
	NativeStackTraces = 0x10u
}
