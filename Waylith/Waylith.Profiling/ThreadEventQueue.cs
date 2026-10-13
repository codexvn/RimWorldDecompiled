using System.Threading;

namespace Waylith.Profiling;

internal sealed class ThreadEventQueue
{
	public readonly int ManagedThreadId;

	public readonly string ThreadName;

	public EventChunk WriteChunk;

	public long DroppedEvents;

	public Thread OwnerThread;

	public unsafe void* CursorBlock;

	public EventChunk ReadChunk;

	public int ReadIndex;

	public bool OwnerExited => !OwnerThread.IsAlive;

	public bool IsDrained
	{
		get
		{
			if (ReadChunk == WriteChunk && Volatile.Read(in ReadChunk.Next) == null)
			{
				return ReadIndex == ReadChunk.Count;
			}
			return false;
		}
	}

	public ThreadEventQueue(int managedThreadId, string threadName, EventChunk firstChunk)
	{
		ManagedThreadId = managedThreadId;
		ThreadName = threadName;
		WriteChunk = firstChunk;
		ReadChunk = firstChunk;
	}

	public unsafe void Write(ProfilerEvent evt)
	{
		EventChunk eventChunk = WriteChunk;
		int num = eventChunk.Count;
		if (num == 8192)
		{
			EventChunk eventChunk2 = EventChunkPool.TryAcquire();
			if (eventChunk2 == null)
			{
				if (evt.Type == ProfilerEventType.Alloc)
				{
					DroppedEvents++;
					return;
				}
				eventChunk2 = EventChunkPool.AcquireOverBudget();
			}
			Volatile.Write(ref eventChunk.Next, eventChunk2);
			WriteChunk = eventChunk2;
			eventChunk = eventChunk2;
			num = 0;
		}
		eventChunk.Slots[num] = evt;
		Volatile.Write(ref *eventChunk.CountSlot, num + 1);
	}
}
