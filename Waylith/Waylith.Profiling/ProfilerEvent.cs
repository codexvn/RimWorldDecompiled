using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Waylith.Profiling;

[StructLayout(LayoutKind.Explicit, Size = 16)]
internal struct ProfilerEvent
{
	[FieldOffset(0)]
	public long Timestamp;

	[FieldOffset(14)]
	public ProfilerEventType Type;

	public readonly ProfilerZoneBeginEvent AsZoneBegin => Unsafe.BitCast<ProfilerEvent, ProfilerZoneBeginEvent>(this);

	public readonly ProfilerFrameMarkEvent AsFrameMark => Unsafe.BitCast<ProfilerEvent, ProfilerFrameMarkEvent>(this);

	public readonly ProfilerAllocEvent AsAlloc => Unsafe.BitCast<ProfilerEvent, ProfilerAllocEvent>(this);
}
