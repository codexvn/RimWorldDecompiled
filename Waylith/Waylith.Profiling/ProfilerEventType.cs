namespace Waylith.Profiling;

internal enum ProfilerEventType : byte
{
	ZoneBegin = 1,
	ZoneEnd = 2,
	FrameBegin = 3,
	FrameEnd = 4,
	Alloc = 7,
	Reset = 8
}
