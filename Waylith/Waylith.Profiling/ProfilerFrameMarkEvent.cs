using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Waylith.Profiling;

[StructLayout(LayoutKind.Explicit, Size = 16)]
internal struct ProfilerFrameMarkEvent
{
	[FieldOffset(0)]
	public long Timestamp;

	[FieldOffset(8)]
	public int FrameNumber;

	[FieldOffset(14)]
	public ProfilerEventType Type;

	public readonly ProfilerEvent AsEvent => Unsafe.BitCast<ProfilerFrameMarkEvent, ProfilerEvent>(this);

	public ProfilerFrameMarkEvent(ProfilerEventType type, long timestamp, int frameNumber)
	{
		Timestamp = timestamp;
		FrameNumber = frameNumber;
		Type = type;
	}
}
