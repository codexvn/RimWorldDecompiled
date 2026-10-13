using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Waylith.Profiling;

[StructLayout(LayoutKind.Explicit, Size = 16)]
internal struct ProfilerAllocEvent
{
	[FieldOffset(0)]
	public long Timestamp;

	[FieldOffset(8)]
	public int Size;

	[FieldOffset(12)]
	public ushort TypeId;

	[FieldOffset(14)]
	public ProfilerEventType Type;

	public readonly ProfilerEvent AsEvent => Unsafe.BitCast<ProfilerAllocEvent, ProfilerEvent>(this);

	public ProfilerAllocEvent(long timestamp, int size, ushort typeId)
	{
		Timestamp = timestamp;
		Size = size;
		TypeId = typeId;
		Type = ProfilerEventType.Alloc;
	}
}
