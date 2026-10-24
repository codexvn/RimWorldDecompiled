using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.Jobs;

namespace Waylith.Runtime;

public static class NativeAllocationRuntime
{
	private enum TempScopeKind
	{
		None,
		Frame,
		Job
	}

	private struct TempScopeFrame
	{
		public int ScopeId;

		public int FrameIndex;

		public int JobHandleId;

		public TempScopeKind Kind;
	}

	private sealed class AllocationRecord
	{
		public required nint Pointer { get; init; }

		public required long Size { get; init; }

		public required int Alignment { get; init; }

		public required AllocationKind Allocator { get; init; }

		public required int FrameAllocated { get; init; }

		public required int ThreadId { get; init; }

		public int TempScopeId { get; init; }

		public bool TempJobWarningIssued { get; set; }
	}

	private sealed class AllocationBucket
	{
		public Dictionary<nint, AllocationRecord> Records { get; } = new Dictionary<nint, AllocationRecord>();

		public long TotalBytes { get; set; }
	}

	private static class GuardPages
	{
		public static readonly bool IsEnabled = Environment.GetEnvironmentVariable("WAYLITH_NATIVE_GUARD_PAGES") == "1";

		private static readonly nuint PageSize = (nuint)Environment.SystemPageSize;

		private const int ProtNone = 0;

		private const int ProtReadWrite = 3;

		private const int MapPrivate = 2;

		private static readonly int MapAnonymous = (OperatingSystem.IsMacOS() ? 4096 : 32);

		private const uint MemCommitReserve = 12288u;

		private const uint PageReadWrite = 4u;

		private const uint PageNoAccess = 1u;

		private static readonly Lock Lock = new Lock();

		private static readonly Dictionary<nint, (nint Base, nuint Total)> Regions = new Dictionary<nint, (nint, nuint)>();

		[LibraryImport("libc", SetLastError = true)]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
		[SkipLocalsInit]
		private static nint mmap(nint addr, nuint length, int prot, int flags, int fd, nint offset)
		{
			Marshal.SetLastSystemError(0);
			nint result = __PInvoke(addr, length, prot, flags, fd, offset);
			Marshal.SetLastPInvokeError(Marshal.GetLastSystemError());
			return result;
			[DllImport("libc", EntryPoint = "mmap", ExactSpelling = true)]
			static extern nint __PInvoke(nint __addr_native, nuint __length_native, int __prot_native, int __flags_native, int __fd_native, nint __offset_native);
		}

		[LibraryImport("libc", SetLastError = true)]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
		[SkipLocalsInit]
		private static int mprotect(nint addr, nuint length, int prot)
		{
			Marshal.SetLastSystemError(0);
			int result = __PInvoke(addr, length, prot);
			Marshal.SetLastPInvokeError(Marshal.GetLastSystemError());
			return result;
			[DllImport("libc", EntryPoint = "mprotect", ExactSpelling = true)]
			static extern int __PInvoke(nint __addr_native, nuint __length_native, int __prot_native);
		}

		[LibraryImport("kernel32", SetLastError = true)]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
		[SkipLocalsInit]
		private static nint VirtualAlloc(nint lpAddress, nuint dwSize, uint flAllocationType, uint flProtect)
		{
			Marshal.SetLastSystemError(0);
			nint result = __PInvoke(lpAddress, dwSize, flAllocationType, flProtect);
			Marshal.SetLastPInvokeError(Marshal.GetLastSystemError());
			return result;
			[DllImport("kernel32", EntryPoint = "VirtualAlloc", ExactSpelling = true)]
			static extern nint __PInvoke(nint __lpAddress_native, nuint __dwSize_native, uint __flAllocationType_native, uint __flProtect_native);
		}

		[LibraryImport("kernel32", SetLastError = true)]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
		[SkipLocalsInit]
		[return: MarshalAs(UnmanagedType.Bool)]
		private unsafe static bool VirtualProtect(nint lpAddress, nuint dwSize, uint flNewProtect, out uint lpflOldProtect)
		{
			lpflOldProtect = 0u;
			int num;
			int lastSystemError;
			fixed (uint* _lpflOldProtect_native = &lpflOldProtect)
			{
				Marshal.SetLastSystemError(0);
				num = __PInvoke(lpAddress, dwSize, flNewProtect, _lpflOldProtect_native);
				lastSystemError = Marshal.GetLastSystemError();
			}
			bool result = num != 0;
			Marshal.SetLastPInvokeError(lastSystemError);
			return result;
			[DllImport("kernel32", EntryPoint = "VirtualProtect", ExactSpelling = true)]
			static extern unsafe int __PInvoke(nint __lpAddress_native, nuint __dwSize_native, uint __flNewProtect_native, uint* __lpflOldProtect_native);
		}

		public unsafe static void* Allocate(nuint size, nuint alignment)
		{
			nuint num = (size + PageSize - 1) & ~(PageSize - 1);
			nuint num2 = num + PageSize;
			nint num3;
			if (OperatingSystem.IsWindows())
			{
				num3 = VirtualAlloc(0, num2, 12288u, 4u);
				if (num3 == 0)
				{
					return null;
				}
			}
			else
			{
				num3 = mmap(0, num2, 3, 2 | MapAnonymous, -1, 0);
				if (num3 == -1)
				{
					return null;
				}
			}
			ProtectNoAccess(num3 + (nint)num, PageSize);
			nuint num4 = (size + alignment - 1) & ~(alignment - 1);
			nint num5 = num3 + (nint)(num - num4);
			using (Lock.EnterScope())
			{
				Regions[num5] = (num3, num2);
				return (void*)num5;
			}
		}

		public static void Free(nint memory)
		{
			(nint, nuint) value;
			using (Lock.EnterScope())
			{
				if (!Regions.Remove(memory, out value))
				{
					throw new InvalidOperationException($"Guard-page free of unknown pointer 0x{((IntPtr)memory).ToInt64():X}.");
				}
			}
			ProtectNoAccess(value.Item1, value.Item2);
		}

		private static void ProtectNoAccess(nint address, nuint length)
		{
			if (!(OperatingSystem.IsWindows() ? VirtualProtect(address, length, 1u, out var _) : (mprotect(address, length, 0) == 0)))
			{
				throw new InvalidOperationException($"Failed to protect guard region 0x{address:X} ({length} bytes), error {Marshal.GetLastPInvokeError()}.");
			}
		}
	}

	private static readonly object Gate = new object();

	private static readonly Dictionary<nint, AllocationRecord> Allocations = new Dictionary<nint, AllocationRecord>();

	private static readonly AllocationBucket[] AllocationBuckets = CreateAllocationBuckets();

	private static readonly Dictionary<int, List<nint>> TempAllocationsByScope = new Dictionary<int, List<nint>>();

	private static int _currentFrame;

	private static int _lastJobActiveFrame;

	private static int _nextTempScopeId;

	private static int _liveScopedTempAllocationCount;

	private static LogCategory log = LogCategory.Make("NativeAllocationRuntime");

	[ThreadStatic]
	private static TempScopeFrame[]? _tempScopeStack;

	[ThreadStatic]
	private static int _tempScopeDepth;

	public unsafe static nint Allocate(long size, int alignment, AllocationKind allocator)
	{
		bool shouldAppend = (uint)allocator <= 1u;
		if (shouldAppend)
		{
			throw new ArgumentOutOfRangeException("allocator", $"Allocator '{allocator}' cannot be used for native memory allocation.");
		}
		long num = System.Math.Max(1L, size);
		checked
		{
			nuint num2 = (nuint)num;
			nuint num3 = (nuint)NormalizeAlignment(alignment);
			void* intPtr = (GuardPages.IsEnabled ? GuardPages.Allocate(num2, num3) : NativeMemory.AlignedAlloc(num2, num3));
			if (intPtr == null)
			{
				throw new OutOfMemoryException($"Failed to allocate {num} bytes with alignment {num3} using allocator '{allocator}'.");
			}
			nint num4 = unchecked((nint)intPtr);
			int currentManagedThreadId = Environment.CurrentManagedThreadId;
			int num5 = 0;
			if (allocator == AllocationKind.Temp)
			{
				if (!TryGetCurrentTempScope(out var scope))
				{
					LogCategory logCategory = log;
					LogString<Warning> message = new LogString<Warning>(122, 1, logCategory, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("Allocator.Temp allocation on thread ");
						message.AppendFormatted(currentManagedThreadId);
						message.AppendLiteral(" without an active temp scope. It will remain live until explicitly freed or shutdown.");
					}
					logCategory.Warning(message, "Allocate", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Runtime\\NativeAllocationRuntime.cs", 98);
				}
				else
				{
					num5 = scope.ScopeId;
				}
			}
			AllocationRecord record = new AllocationRecord
			{
				Pointer = num4,
				Size = num,
				Alignment = (int)num3,
				Allocator = allocator,
				FrameAllocated = Volatile.Read(in _currentFrame),
				ThreadId = currentManagedThreadId,
				TempScopeId = num5
			};
			lock (Gate)
			{
				TrackAllocationLocked(record);
				if (num5 != 0)
				{
					if (!TempAllocationsByScope.TryGetValue(num5, out List<nint> value))
					{
						value = new List<nint>();
						TempAllocationsByScope[num5] = value;
					}
					value.Add(num4);
					Interlocked.Increment(ref _liveScopedTempAllocationCount);
				}
			}
			return num4;
		}
	}

	public static void Free(nint memory, AllocationKind allocator)
	{
		if (memory == IntPtr.Zero)
		{
			return;
		}
		AllocationRecord value;
		lock (Gate)
		{
			if (!Allocations.TryGetValue(memory, out value))
			{
				LogCategory logCategory = log;
				LogCategory logCategory2 = logCategory;
				bool shouldAppend;
				LogString<Error> message = new LogString<Error>(73, 2, logCategory, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("NativeAllocationRuntime.Free ignored unknown pointer 0x");
					message.AppendFormatted(((IntPtr)memory).ToInt64(), 0, "X");
					message.AppendLiteral(" for allocator '");
					message.AppendFormatted(allocator);
					message.AppendLiteral("'.");
				}
				logCategory2.Error(message, "Free", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Runtime\\NativeAllocationRuntime.cs", 149);
				return;
			}
			UntrackAllocationLocked(value, removeTempScopeMapping: true);
		}
		if (value != null && !AllocatorsCompatible(value.Allocator, allocator))
		{
			LogCategory logCategory = log;
			LogCategory logCategory3 = logCategory;
			bool shouldAppend2;
			LogString<Warning> message2 = new LogString<Warning>(51, 3, logCategory, out shouldAppend2);
			if (shouldAppend2)
			{
				message2.AppendLiteral("Pointer 0x");
				message2.AppendFormatted(((IntPtr)memory).ToInt64(), 0, "X");
				message2.AppendLiteral(" was allocated with '");
				message2.AppendFormatted(value.Allocator);
				message2.AppendLiteral("' but freed with '");
				message2.AppendFormatted(allocator);
				message2.AppendLiteral("'.");
			}
			logCategory3.Warning(message2, "Free", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Runtime\\NativeAllocationRuntime.cs", 158);
		}
		ReleaseMemory(value.Pointer);
	}

	private unsafe static void ReleaseMemory(nint memory)
	{
		if (GuardPages.IsEnabled)
		{
			GuardPages.Free(memory);
		}
		else
		{
			NativeMemory.AlignedFree((void*)memory);
		}
	}

	public static bool TryGetAllocationSize(nint memory, out long size)
	{
		if (memory == IntPtr.Zero)
		{
			size = 0L;
			return false;
		}
		lock (Gate)
		{
			if (Allocations.TryGetValue(memory, out AllocationRecord value))
			{
				size = value.Size;
				return true;
			}
		}
		size = 0L;
		return false;
	}

	public static void BeginFrame(int frameIndex)
	{
		Volatile.Write(ref _currentFrame, frameIndex);
		EnterTempScope(TempScopeKind.Frame, frameIndex, 0);
	}

	public static void EndFrame(int frameIndex)
	{
		Volatile.Write(ref _currentFrame, frameIndex);
		List<AllocationRecord> list = null;
		ExitTempScope(TempScopeKind.Frame);
		if (WaylithJobRuntime.HasPendingWork)
		{
			_lastJobActiveFrame = frameIndex;
		}
		int num = frameIndex - _lastJobActiveFrame;
		lock (Gate)
		{
			foreach (AllocationRecord value in GetAllocationBucket(AllocationKind.TempJob).Records.Values)
			{
				if (!value.TempJobWarningIssued && frameIndex - value.FrameAllocated >= 4 && num >= 4)
				{
					value.TempJobWarningIssued = true;
					if (list == null)
					{
						list = new List<AllocationRecord>();
					}
					list.Add(value);
				}
			}
		}
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			AllocationRecord allocationRecord = list[i];
			LogCategory logCategory = log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(75, 4, logCategory, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("TempJob allocation 0x");
				message.AppendFormatted(((IntPtr)allocationRecord.Pointer).ToInt64(), 0, "X");
				message.AppendLiteral(" (");
				message.AppendFormatted(allocationRecord.Size);
				message.AppendLiteral(" bytes, align ");
				message.AppendFormatted(allocationRecord.Alignment);
				message.AppendLiteral(") has not been disposed after ");
				message.AppendFormatted(frameIndex - allocationRecord.FrameAllocated);
				message.AppendLiteral(" frames.");
			}
			logCategory.Warning(message, "EndFrame", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Runtime\\NativeAllocationRuntime.cs", 237);
		}
	}

	public static void EnterJobTempScope(int jobHandleId)
	{
		EnterTempScope(TempScopeKind.Job, Volatile.Read(in _currentFrame), jobHandleId);
	}

	public static void ExitJobTempScope(int jobHandleId)
	{
		ExitTempScope(TempScopeKind.Job, jobHandleId);
	}

	public static void Shutdown()
	{
		List<AllocationRecord> list = null;
		lock (Gate)
		{
			if (Allocations.Count > 0)
			{
				list = Allocations.Values.ToList();
				Allocations.Clear();
				ClearAllocationBucketsLocked();
				TempAllocationsByScope.Clear();
				Volatile.Write(ref _liveScopedTempAllocationCount, 0);
			}
		}
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			AllocationRecord allocationRecord = list[i];
			bool shouldAppend;
			switch (allocationRecord.Allocator)
			{
			case AllocationKind.Persistent:
			{
				LogCategory logCategory = log;
				LogCategory logCategory3 = logCategory;
				LogString<Warning> message = new LogString<Warning>(72, 3, logCategory, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Persistent allocation 0x");
					message.AppendFormatted(((IntPtr)allocationRecord.Pointer).ToInt64(), 0, "X");
					message.AppendLiteral(" (");
					message.AppendFormatted(allocationRecord.Size);
					message.AppendLiteral(" bytes, align ");
					message.AppendFormatted(allocationRecord.Alignment);
					message.AppendLiteral(") was not freed before shutdown.");
				}
				logCategory3.Warning(message, "Shutdown", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Runtime\\NativeAllocationRuntime.cs", 283);
				break;
			}
			case AllocationKind.TempJob:
			{
				LogCategory logCategory = log;
				LogCategory logCategory2 = logCategory;
				LogString<Warning> message = new LogString<Warning>(69, 3, logCategory, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("TempJob allocation 0x");
					message.AppendFormatted(((IntPtr)allocationRecord.Pointer).ToInt64(), 0, "X");
					message.AppendLiteral(" (");
					message.AppendFormatted(allocationRecord.Size);
					message.AppendLiteral(" bytes, align ");
					message.AppendFormatted(allocationRecord.Alignment);
					message.AppendLiteral(") was not freed before shutdown.");
				}
				logCategory2.Warning(message, "Shutdown", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Runtime\\NativeAllocationRuntime.cs", 291);
				break;
			}
			}
			ReleaseMemory(allocationRecord.Pointer);
		}
	}

	public static NativeAllocationRuntimeDiagnosticsSnapshot GetDiagnosticsSnapshot()
	{
		lock (Gate)
		{
			if (Allocations.Count == 0)
			{
				return new NativeAllocationRuntimeDiagnosticsSnapshot(Volatile.Read(in _currentFrame), 0, 0L, Array.Empty<UnityAllocationBucketDiagnostics>());
			}
			UnityAllocationBucketDiagnostics[] array = new UnityAllocationBucketDiagnostics[AllocationBuckets.Length];
			long num = 0L;
			AllocationKind[] values = Enum.GetValues<AllocationKind>();
			foreach (AllocationKind allocationKind in values)
			{
				AllocationBucket allocationBucket = GetAllocationBucket(allocationKind);
				num += allocationBucket.TotalBytes;
				array[(int)allocationKind] = new UnityAllocationBucketDiagnostics(allocationKind, allocationBucket.Records.Count, allocationBucket.TotalBytes);
			}
			return new NativeAllocationRuntimeDiagnosticsSnapshot(Volatile.Read(in _currentFrame), Allocations.Count, num, array);
		}
	}

	public static long GetAllocatedSize(AllocationKind allocator)
	{
		return GetAllocationBucket(allocator).TotalBytes;
	}

	private static int NormalizeAlignment(int alignment)
	{
		int num = System.Math.Max(IntPtr.Size, (alignment <= 0) ? IntPtr.Size : alignment);
		if ((num & (num - 1)) == 0)
		{
			return num;
		}
		int num2;
		for (num2 = 1; num2 < num; num2 <<= 1)
		{
		}
		return num2;
	}

	private static bool AllocatorsCompatible(AllocationKind allocated, AllocationKind released)
	{
		if (allocated == released)
		{
			return true;
		}
		if (allocated != AllocationKind.Persistent)
		{
			if (allocated == AllocationKind.Domain && released == AllocationKind.Persistent)
			{
				goto IL_001a;
			}
		}
		else if (released == AllocationKind.Domain)
		{
			goto IL_001a;
		}
		return false;
		IL_001a:
		return true;
	}

	private static void EnterTempScope(TempScopeKind kind, int frameIndex, int jobHandleId)
	{
		int scopeId = Interlocked.Increment(ref _nextTempScopeId);
		EnsureTempScopeCapacity(_tempScopeDepth + 1);
		_tempScopeStack[_tempScopeDepth++] = new TempScopeFrame
		{
			ScopeId = scopeId,
			Kind = kind,
			FrameIndex = frameIndex,
			JobHandleId = jobHandleId
		};
	}

	private static void ExitTempScope(TempScopeKind expectedKind, int expectedJobHandleId = 0)
	{
		if (!TryGetCurrentTempScope(out var scope))
		{
			return;
		}
		bool shouldAppend;
		if (scope.Kind != expectedKind)
		{
			LogCategory logCategory = log;
			LogCategory logCategory2 = logCategory;
			LogString<Warning> message = new LogString<Warning>(63, 3, logCategory, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Temp scope stack mismatch on thread ");
				message.AppendFormatted(Environment.CurrentManagedThreadId);
				message.AppendLiteral(": expected '");
				message.AppendFormatted(expectedKind);
				message.AppendLiteral("' but found '");
				message.AppendFormatted(scope.Kind);
				message.AppendLiteral("'.");
			}
			logCategory2.Warning(message, "ExitTempScope", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Runtime\\NativeAllocationRuntime.cs", 373);
		}
		else if (expectedKind == TempScopeKind.Job && scope.JobHandleId != expectedJobHandleId)
		{
			LogCategory logCategory = log;
			LogCategory logCategory3 = logCategory;
			LogString<Warning> message = new LogString<Warning>(65, 3, logCategory, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Temp job scope mismatch on thread ");
				message.AppendFormatted(Environment.CurrentManagedThreadId);
				message.AppendLiteral(": expected job ");
				message.AppendFormatted(expectedJobHandleId);
				message.AppendLiteral(" but found job ");
				message.AppendFormatted(scope.JobHandleId);
				message.AppendLiteral(".");
			}
			logCategory3.Warning(message, "ExitTempScope", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Runtime\\NativeAllocationRuntime.cs", 379);
		}
		else
		{
			_tempScopeDepth--;
			ReleaseTempAllocationsForScope(scope.ScopeId);
		}
	}

	private static bool TryGetCurrentTempScope(out TempScopeFrame scope)
	{
		if (_tempScopeDepth > 0 && _tempScopeStack != null)
		{
			scope = _tempScopeStack[_tempScopeDepth - 1];
			return true;
		}
		scope = default(TempScopeFrame);
		return false;
	}

	private static void EnsureTempScopeCapacity(int requiredCapacity)
	{
		if (_tempScopeStack == null)
		{
			_tempScopeStack = new TempScopeFrame[System.Math.Max(4, requiredCapacity)];
		}
		else if (_tempScopeStack.Length < requiredCapacity)
		{
			Array.Resize(ref _tempScopeStack, System.Math.Max(_tempScopeStack.Length * 2, requiredCapacity));
		}
	}

	private static void ReleaseTempAllocationsForScope(int scopeId)
	{
		if (Volatile.Read(in _liveScopedTempAllocationCount) == 0)
		{
			return;
		}
		List<AllocationRecord> list = null;
		lock (Gate)
		{
			if (!TempAllocationsByScope.TryGetValue(scopeId, out List<nint> value))
			{
				return;
			}
			TempAllocationsByScope.Remove(scopeId);
			Interlocked.Add(ref _liveScopedTempAllocationCount, -value.Count);
			list = new List<AllocationRecord>(value.Count);
			for (int i = 0; i < value.Count; i++)
			{
				nint key = value[i];
				if (Allocations.TryGetValue(key, out AllocationRecord value2))
				{
					UntrackAllocationLocked(value2, removeTempScopeMapping: false);
					list.Add(value2);
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			ReleaseMemory(list[j].Pointer);
		}
	}

	private static void RemoveTempAllocationFromScope(AllocationRecord record)
	{
		if (record.Allocator == AllocationKind.Temp && record.TempScopeId != 0 && TempAllocationsByScope.TryGetValue(record.TempScopeId, out List<nint> value))
		{
			if (value.Remove(record.Pointer))
			{
				Interlocked.Decrement(ref _liveScopedTempAllocationCount);
			}
			if (value.Count == 0)
			{
				TempAllocationsByScope.Remove(record.TempScopeId);
			}
		}
	}

	private static AllocationBucket[] CreateAllocationBuckets()
	{
		int num = 0;
		AllocationKind[] values = Enum.GetValues<AllocationKind>();
		foreach (AllocationKind val in values)
		{
			num = System.Math.Max(num, (int)val);
		}
		AllocationBucket[] array = new AllocationBucket[num + 1];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = new AllocationBucket();
		}
		return array;
	}

	private static AllocationBucket GetAllocationBucket(AllocationKind allocator)
	{
		if ((uint)allocator >= (uint)AllocationBuckets.Length)
		{
			throw new ArgumentOutOfRangeException("allocator", allocator, "Allocator bucket index was out of range.");
		}
		return AllocationBuckets[(int)allocator];
	}

	private static void TrackAllocationLocked(AllocationRecord record)
	{
		Allocations[record.Pointer] = record;
		AllocationBucket allocationBucket = GetAllocationBucket(record.Allocator);
		allocationBucket.Records[record.Pointer] = record;
		allocationBucket.TotalBytes += record.Size;
	}

	private static void UntrackAllocationLocked(AllocationRecord record, bool removeTempScopeMapping)
	{
		Allocations.Remove(record.Pointer);
		AllocationBucket allocationBucket = GetAllocationBucket(record.Allocator);
		if (allocationBucket.Records.Remove(record.Pointer))
		{
			allocationBucket.TotalBytes -= record.Size;
		}
		if (removeTempScopeMapping)
		{
			RemoveTempAllocationFromScope(record);
		}
	}

	private static void ClearAllocationBucketsLocked()
	{
		for (int i = 0; i < AllocationBuckets.Length; i++)
		{
			AllocationBuckets[i].Records.Clear();
			AllocationBuckets[i].TotalBytes = 0L;
		}
	}
}
