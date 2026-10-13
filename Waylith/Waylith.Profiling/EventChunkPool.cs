namespace Waylith.Profiling;

internal static class EventChunkPool
{
	private const int PreallocatedChunks = 64;

	private const int MaxChunks = 1024;

	private static readonly object PoolLock = new object();

	private static EventChunk? head;

	private static int allocatedChunks;

	private static int pooledChunks;

	public static long AllocatedBytes
	{
		get
		{
			lock (PoolLock)
			{
				return (long)allocatedChunks * 131136L;
			}
		}
	}

	public static void Preallocate()
	{
		lock (PoolLock)
		{
			while (allocatedChunks < 64)
			{
				allocatedChunks++;
				pooledChunks++;
				head = new EventChunk
				{
					PoolNext = head
				};
			}
		}
	}

	public static EventChunk AcquireOverBudget()
	{
		lock (PoolLock)
		{
			allocatedChunks++;
		}
		return new EventChunk();
	}

	public static EventChunk? TryAcquire()
	{
		lock (PoolLock)
		{
			EventChunk eventChunk = head;
			if (eventChunk != null)
			{
				head = eventChunk.PoolNext;
				eventChunk.PoolNext = null;
				pooledChunks--;
				return eventChunk;
			}
			if (allocatedChunks < 1024)
			{
				allocatedChunks++;
				return new EventChunk();
			}
			return null;
		}
	}

	public static void Release(EventChunk chunk)
	{
		chunk.ResetCount();
		chunk.Next = null;
		lock (PoolLock)
		{
			if (allocatedChunks > 1024 || pooledChunks >= 64)
			{
				allocatedChunks--;
				chunk.FreeStorage();
			}
			else
			{
				chunk.PoolNext = head;
				head = chunk;
				pooledChunks++;
			}
		}
	}
}
