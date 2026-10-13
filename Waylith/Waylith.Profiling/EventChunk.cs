using System.Runtime.InteropServices;
using System.Threading;

namespace Waylith.Profiling;

internal sealed class EventChunk
{
	public const int Capacity = 8192;

	private const int SlotsOffset = 64;

	internal const int BlockBytes = 131136;

	private unsafe byte* block;

	public unsafe readonly ProfilerEvent* Slots;

	public unsafe readonly int* CountSlot;

	public EventChunk? Next;

	public EventChunk? PoolNext;

	public unsafe int Count => Volatile.Read(in *CountSlot);

	public unsafe EventChunk()
	{
		block = (byte*)NativeMemory.AlignedAlloc(131136u, 64u);
		NativeMemory.Clear(block, 64u);
		CountSlot = (int*)block;
		Slots = (ProfilerEvent*)(block + 64);
	}

	public unsafe ProfilerEvent SlotAt(int index)
	{
		return Slots[index];
	}

	public unsafe void ResetCount()
	{
		Volatile.Write(ref *CountSlot, 0);
	}

	public unsafe void FreeStorage()
	{
		NativeMemory.AlignedFree(block);
		block = null;
	}
}
