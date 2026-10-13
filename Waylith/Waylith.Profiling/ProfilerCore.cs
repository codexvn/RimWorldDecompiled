using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Waylith.Profiling;

internal static class ProfilerCore
{
	private struct Cursor
	{
		public unsafe ProfilerEvent* Write;

		public unsafe ProfilerEvent* Limit;

		public unsafe ProfilerEvent* Base;

		public unsafe int* CountSlot;
	}

	private static class Tls
	{
		[ThreadStatic]
		public unsafe static Cursor* Cursor;
	}

	public static bool Capturing;

	public static int OnlyThreadId;

	private const int MaxQueues = 256;

	[ThreadStatic]
	private static ThreadEventQueue? tlsQueue;

	[ThreadStatic]
	private static int writeSlowDepth;

	private static readonly Lock RegistryLock = new Lock();

	private static readonly List<ThreadEventQueue> Queues = new List<ThreadEventQueue>();

	private static int queueCount;

	public static bool Enabled;

	private static volatile int mainThreadId;

	public static ThreadEventQueue? CurrentQueue => tlsQueue ?? RegisterCurrentThread();

	public static int MainThreadId => mainThreadId;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ShouldCaptureCurrentThread()
	{
		if (!Capturing)
		{
			return false;
		}
		int onlyThreadId = OnlyThreadId;
		if (onlyThreadId != 0)
		{
			return onlyThreadId == Environment.CurrentManagedThreadId;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void Write(ProfilerEvent evt)
	{
		Cursor* cursor = Tls.Cursor;
		if (cursor != null)
		{
			ProfilerEvent* write = cursor->Write;
			if (write < cursor->Limit)
			{
				*write = evt;
				cursor->Write = write + 1;
				Volatile.Write(ref *cursor->CountSlot, (int)(write + 1 - cursor->Base));
				return;
			}
		}
		WriteSlow(evt);
	}

	public static void WriteReset()
	{
		ProfilerEvent evt = default(ProfilerEvent);
		evt.Timestamp = ProfilerClock.GetTimestamp();
		evt.Type = ProfilerEventType.Reset;
		Write(evt);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void WriteSlow(ProfilerEvent evt)
	{
		if (writeSlowDepth > 0)
		{
			return;
		}
		writeSlowDepth++;
		try
		{
			ThreadEventQueue threadEventQueue = tlsQueue ?? RegisterCurrentThread();
			if (threadEventQueue != null)
			{
				threadEventQueue.Write(evt);
				AimCursor(threadEventQueue.WriteChunk);
			}
		}
		finally
		{
			writeSlowDepth--;
		}
	}

	private unsafe static void AimCursor(EventChunk chunk)
	{
		Cursor* cursor = Tls.Cursor;
		cursor->Base = chunk.Slots;
		cursor->Write = chunk.Slots + chunk.Count;
		cursor->Limit = chunk.Slots + 8192;
		cursor->CountSlot = chunk.CountSlot;
	}

	public static void IdentifyMainThread()
	{
		mainThreadId = Environment.CurrentManagedThreadId;
		_ = CurrentQueue;
	}

	private static ThreadEventQueue? RegisterCurrentThread()
	{
		writeSlowDepth++;
		try
		{
			return RegisterCurrentThreadCore();
		}
		finally
		{
			writeSlowDepth--;
		}
	}

	private unsafe static ThreadEventQueue? RegisterCurrentThreadCore()
	{
		using (RegistryLock.EnterScope())
		{
			if (queueCount >= 256)
			{
				return null;
			}
			EventChunk eventChunk = EventChunkPool.TryAcquire() ?? new EventChunk();
			Thread currentThread = Thread.CurrentThread;
			ThreadEventQueue threadEventQueue = new ThreadEventQueue(currentThread.ManagedThreadId, currentThread.Name ?? $"Thread {currentThread.ManagedThreadId}", eventChunk)
			{
				OwnerThread = currentThread
			};
			Queues.Add(threadEventQueue);
			queueCount++;
			tlsQueue = threadEventQueue;
			Tls.Cursor = (Cursor*)(threadEventQueue.CursorBlock = NativeMemory.AlignedAlloc((nuint)sizeof(Cursor), 64u));
			AimCursor(eventChunk);
			return threadEventQueue;
		}
	}

	public unsafe static void UnregisterQueue(ThreadEventQueue queue)
	{
		using (RegistryLock.EnterScope())
		{
			if (!Queues.Remove(queue))
			{
				return;
			}
			queueCount--;
		}
		NativeMemory.AlignedFree(queue.CursorBlock);
		queue.CursorBlock = null;
	}

	public static void CollectNewQueues(List<ThreadEventQueue> into, int known)
	{
		if (Volatile.Read(in queueCount) <= known)
		{
			return;
		}
		using (RegistryLock.EnterScope())
		{
			for (int i = known; i < Queues.Count; i++)
			{
				into.Add(Queues[i]);
			}
		}
	}
}
