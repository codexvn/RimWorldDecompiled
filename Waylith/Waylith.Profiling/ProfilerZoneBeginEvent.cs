using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Waylith.Profiling;

[StructLayout(LayoutKind.Explicit, Size = 16)]
internal struct ProfilerZoneBeginEvent
{
	[FieldOffset(0)]
	public long Timestamp;

	[FieldOffset(8)]
	public ProfilerLabelId Label;

	[FieldOffset(14)]
	public ProfilerEventType Type;

	public readonly ProfilerEvent AsEvent => Unsafe.BitCast<ProfilerZoneBeginEvent, ProfilerEvent>(this);

	public ProfilerZoneBeginEvent(long timestamp, ProfilerLabelId label)
	{
		Timestamp = timestamp;
		Label = label;
		Type = ProfilerEventType.ZoneBegin;
	}
}
