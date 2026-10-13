using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Waylith.Profiling.Instrumentation;
using WaylithClr;

namespace Waylith.Profiling;

public static class FrameProfiler
{
	public struct PauseSpan
	{
		public int startMicroseconds;

		public int suspendedMicroseconds;

		public int endMicroseconds;

		public RuntimePauseReason reason;

		public int condemnedGeneration;

		public int triggeringThreadId;

		public long promotedBytes;

		public long collectedBytes;
	}

	public struct TypeAllocStat
	{
		public Type type;

		public ProfilerLabelId labelId;

		public int threadId;

		public int count;

		public long bytes;
	}

	public struct ThreadSection
	{
		public int threadId;

		public ProfilerLabelId nameLabelId;

		public int recordedScopes;

		public long busyNanoseconds;

		public long startNanoseconds;

		public long endNanoseconds;

		public int firstPathEntry;

		public int pathEntryCount;
	}

	public struct NodeAllocStat
	{
		public int totalBytes;

		public int selfBytes;

		public int count;
	}

	public struct PathEntry
	{
		public int pathId;

		public float totalMicroseconds;

		public float selfMicroseconds;

		public int calls;

		public int allocBytes;

		public int selfAllocBytes;
	}

	public sealed class FrameSnapshot
	{
		public PauseSpan[] pauses = Array.Empty<PauseSpan>();

		public TypeAllocStat[] topAllocTypes = Array.Empty<TypeAllocStat>();

		public ThreadSection[] sections = Array.Empty<ThreadSection>();

		public PathEntry[] pathEntries = Array.Empty<PathEntry>();

		public int pathEntryCount;

		public int sectionCount;

		public int topAllocTypeCount;

		public int pauseCount;

		public float pauseMs;

		public bool valid;

		public bool isSpike;

		public int captureOrdinal;

		public int frameIndex;

		public long beginTimeNanoseconds;

		public long endTimeNanoseconds;

		public long boundaryTimeNanoseconds;

		public int publishedNodeCount;

		internal volatile RetainedZoneRange[]? retainedZones;

		internal int holds;

		public int tickStart;

		public int tickEnd;

		public int tickDelta;

		public int overflowedScopes;

		public int recordedScopes;

		public int unbalancedEnds;

		public long droppedEvents;

		public float totalMs;

		public long allocatedBytes;

		public long trackedAllocBytes;

		public int allocatedObjectCount;

		public int allocOverflowCount;

		public long allocOverflowBytes;

		public bool allocationTracked;

		public int gen0Collections;

		public int gen1Collections;

		public int gen2Collections;

		public long promotedBytes;

		public long DisplayAllocBytes
		{
			get
			{
				if (!allocationTracked)
				{
					return allocatedBytes;
				}
				return trackedAllocBytes;
			}
		}

		public long nodesEndNanoseconds => ((boundaryTimeNanoseconds > beginTimeNanoseconds) ? boundaryTimeNanoseconds : endTimeNanoseconds) - beginTimeNanoseconds;

		public void RetainZones()
		{
			ProfilerWorker.Assembler.RetainZones(this);
		}

		public void Retain()
		{
			Interlocked.Increment(ref holds);
		}

		public void Release()
		{
			if (Interlocked.Decrement(ref holds) == 0)
			{
				ProfilerWorker.Assembler.RecycleSnapshot(this);
			}
		}

		internal void Clear()
		{
			pathEntryCount = 0;
			sectionCount = 0;
			topAllocTypeCount = 0;
			pauseCount = 0;
			pauseMs = 0f;
			valid = false;
			isSpike = false;
			captureOrdinal = 0;
			frameIndex = 0;
			beginTimeNanoseconds = 0L;
			endTimeNanoseconds = 0L;
			boundaryTimeNanoseconds = 0L;
			publishedNodeCount = 0;
			retainedZones = null;
			tickStart = 0;
			tickEnd = 0;
			tickDelta = 0;
			overflowedScopes = 0;
			recordedScopes = 0;
			unbalancedEnds = 0;
			droppedEvents = 0L;
			totalMs = 0f;
			allocatedBytes = 0L;
			trackedAllocBytes = 0L;
			allocatedObjectCount = 0;
			allocOverflowCount = 0;
			allocOverflowBytes = 0L;
			allocationTracked = false;
			gen0Collections = 0;
			gen1Collections = 0;
			gen2Collections = 0;
			promotedBytes = 0L;
		}

		public void SectionZoneRange(int sectionIndex, out long startNs, out long endNs)
		{
			startNs = beginTimeNanoseconds;
			endNs = ((sectionIndex == 0) ? endTimeNanoseconds : (beginTimeNanoseconds + nodesEndNanoseconds));
		}

		internal static void EnsureCapacity<T>(ref T[] array, int required)
		{
			if (array.Length < required)
			{
				Array.Resize(ref array, System.Math.Max(required, array.Length + array.Length / 4));
			}
		}
	}

	public readonly struct ScopeHandle : IDisposable
	{
		private readonly bool active;

		internal ScopeHandle(bool active)
		{
			this.active = active;
		}

		public void Dispose()
		{
			if (active)
			{
				LeaveScope();
			}
		}
	}

	public struct CaptureGap
	{
		public long beginTimeNanoseconds;

		public long endTimeNanoseconds;
	}

	public struct RetainedMemory
	{
		public long historyBytes;

		public long spikeBytes;

		public long timelineBytes;

		public long eventChunkBytes;

		public long pathTableBytes;

		public long aggregatorBytes;

		public long recyclePoolBytes;

		public int historySnapshots;

		public int spikeSnapshots;

		public long nodeBytes;

		public long nodeSlots;

		public long pauseBytes;

		public long allocTypeBytes;

		public long sectionBytes;

		public long allocStatBytes;

		public long pathEntryBytes;

		public long retainedZoneBytes;

		public long TotalBytes => historyBytes + spikeBytes + timelineBytes + eventChunkBytes + pathTableBytes + aggregatorBytes + recyclePoolBytes;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct ReadScope : IDisposable
	{
		public void Dispose()
		{
			Interlocked.Decrement(ref activeReaders);
		}
	}

	public const int DefaultHistoryCapacity = 2000;

	private const int MinHistoryCapacity = 64;

	private const int MaxHistoryCapacity = 10000;

	public const int SpikeCapacity = 24;

	internal const int MaxLabels = 65535;

	public const float SpikeAbsoluteThresholdMs = 25f;

	public const float SpikeRatioThreshold = 1.75f;

	private const int IdentityLabelCacheCapacity = 4096;

	private const string RootLabel = "Frame";

	private const string OverflowLabel = "<overflow>";

	private const double RecalibrationIntervalSeconds = 2.0;

	private static readonly object LabelLock;

	private static readonly Dictionary<string, int> OrdinalLabelIds;

	private static readonly ConcurrentDictionary<string, int> IdentityLabelIds;

	private static int identityLabelCount;

	private static readonly string[] Labels;

	private static readonly bool[] MutedLabels;

	private static readonly bool[] DiscardableLabels;

	private static readonly bool[] AutoMuteExemptLabels;

	private static readonly bool[] AutoMutedLabels;

	private static bool enabled;

	private static bool paused;

	private static bool mainThreadOnly;

	private static bool allocationTracking;

	private static int historyCapacity;

	private static bool frameActive;

	private static int labelCount;

	private static int mutedLabelCount;

	private static int autoMutedLabelCount;

	private static int mutedLabelsChangedPending;

	private static int rootLabelId;

	private static int overflowLabelId;

	private static int firstPauseLabelId;

	private static long scopeOverheadTicksPerPair;

	private static long nextCalibrationTimestamp;

	private static int currentFrameNumber;

	private static int currentFrameStartTick;

	private static long frameStartAllocatedBytes;

	private static int frameStartGen0Collections;

	private static int frameStartGen1Collections;

	private static int frameStartGen2Collections;

	[CompilerGenerated]
	private static bool _003CAutoMuteTrivialLabels_003Ek__BackingField;

	public static readonly bool AllocationTrackingSupported;

	public static readonly bool RuntimePausesSupported;

	private const int MaxCaptureGaps = 64;

	private static readonly List<CaptureGap> captureGaps;

	private static long captureGapStartNs;

	private static bool allocationSubscribed;

	private static readonly Action<object, nint> ObjectAllocatedHandler;

	private const int MaxAllocTypes = 65536;

	private static readonly ConcurrentDictionary<nint, ushort> AllocTypeIds;

	private static readonly Type?[] AllocTypes;

	private static readonly object AllocTypeLock;

	private static int allocTypeCount;

	private static int activeReaders;

	private static readonly string[] PauseLabelNames;

	private static double nanosecondsPerTick;

	public static bool Enabled => enabled;

	public static bool Paused => paused;

	public static bool MainThreadOnly => mainThreadOnly;

	public static bool IsCapturing => ProfilerCore.Capturing;

	public static bool AutoMuteTrivialLabels
	{
		[CompilerGenerated]
		get
		{
			return _003CAutoMuteTrivialLabels_003Ek__BackingField;
		}
		set
		{
			if (_003CAutoMuteTrivialLabels_003Ek__BackingField != value)
			{
				_003CAutoMuteTrivialLabels_003Ek__BackingField = value;
				if (!value)
				{
					UnmuteAutoMutedLabels();
				}
			}
		}
	}

	public static bool IgnoreThrottle { get; set; }

	public static int AutoMutedLabelCount => Volatile.Read(in autoMutedLabelCount);

	public static bool AllocationTracking => allocationTracking;

	public static int HistoryCapacity => historyCapacity;

	public static int HistoryCount => ProfilerWorker.HistoryCount;

	public static int SpikeCount => ProfilerWorker.SpikeCount;

	public static int CompletedFrameCount => ProfilerWorker.CompletedFrameCount;

	public static Func<int> CurrentTickProvider { get; set; }

	public static ProfilerLabelId RootLabelId => new ProfilerLabelId((ushort)rootLabelId);

	public static ProfilerLabelId OverflowLabelId => new ProfilerLabelId((ushort)overflowLabelId);

	public static int CaptureGapCount => captureGaps.Count;

	public static int MutedLabelCount => Volatile.Read(in mutedLabelCount);

	public static float ScopeOverheadNanoseconds => (float)scopeOverheadTicksPerPair * 1E+09f / (float)ProfilerClock.Frequency;

	public static float TimerResolutionNanoseconds => (float)ProfilerClock.Resolution * 1E+09f / (float)ProfilerClock.Frequency;

	internal static int ActiveReaders => Volatile.Read(in activeReaders);

	private static int MainThreadId => ProfilerCore.MainThreadId;

	private static bool IsMainThread => MainThreadId == Environment.CurrentManagedThreadId;

	public static event Action<bool>? EnabledChanged;

	public static event Action? MutedLabelsChanged;

	static FrameProfiler()
	{
		LabelLock = new object();
		OrdinalLabelIds = new Dictionary<string, int>(StringComparer.Ordinal);
		IdentityLabelIds = new ConcurrentDictionary<string, int>(ReferenceEqualityComparer.Instance);
		Labels = new string[65535];
		MutedLabels = new bool[65535];
		DiscardableLabels = new bool[65535];
		AutoMuteExemptLabels = new bool[65535];
		AutoMutedLabels = new bool[65535];
		mainThreadOnly = true;
		historyCapacity = 2000;
		currentFrameNumber = -1;
		_003CAutoMuteTrivialLabels_003Ek__BackingField = true;
		IgnoreThrottle = true;
		AllocationTrackingSupported = true;
		RuntimePausesSupported = true;
		CurrentTickProvider = () => 0;
		captureGaps = new List<CaptureGap>();
		captureGapStartNs = -1L;
		ObjectAllocatedHandler = OnObjectAllocated;
		AllocTypeIds = new ConcurrentDictionary<nint, ushort>();
		AllocTypes = new Type[65536];
		AllocTypeLock = new object();
		allocTypeCount = 1;
		PauseLabelNames = new string[10] { "GC pause (gen 0)", "GC pause (gen 1)", "GC pause (gen 2)", "GC pause", "GC prep pause", "Re-JIT pause", "Debugger pause", "Shutdown pause", "Profiler pause", "Runtime pause" };
		rootLabelId = RegisterLabelLocked("Frame");
		overflowLabelId = RegisterLabelLocked("<overflow>");
		firstPauseLabelId = labelCount;
		string[] pauseLabelNames = PauseLabelNames;
		for (int i = 0; i < pauseLabelNames.Length; i++)
		{
			RegisterLabelLocked(pauseLabelNames[i]);
		}
	}

	public static void SetEnabled(bool value)
	{
		if (enabled != value)
		{
			enabled = value;
			paused = false;
			captureGaps.Clear();
			captureGapStartNs = -1L;
			if (enabled)
			{
				ProfilerClock.EnsureCalibrated();
				EventChunkPool.Preallocate();
				ProfilerWorker.SetHistoryCapacity(historyCapacity);
				ProfilerWorker.ResetRings();
				ProfilerWorker.EnsureStarted();
				ProfilerCore.WriteReset();
				UpdateCapturing();
				scopeOverheadTicksPerPair = 0L;
				CalibrateScopeOverhead();
			}
			else
			{
				frameActive = false;
				UpdateCapturing();
				ProfilerCore.WriteReset();
				ProfilerWorker.Signal();
				ProfilerWorker.ResetRings();
			}
			SyncAllocationSubscription();
			FrameProfiler.EnabledChanged?.Invoke(value);
		}
	}

	public static void SetPaused(bool value)
	{
		if (paused != value)
		{
			paused = value;
			UpdateCapturing();
			RecordCaptureGapEdge(value);
		}
	}

	public static CaptureGap GetCaptureGap(int index)
	{
		return captureGaps[index];
	}

	private static void RecordCaptureGapEdge(bool pausing)
	{
		if (!enabled)
		{
			return;
		}
		long endTimeNanoseconds = TimestampToNanoseconds(ProfilerClock.GetTimestamp());
		if (pausing)
		{
			captureGapStartNs = endTimeNanoseconds;
		}
		else if (captureGapStartNs >= 0)
		{
			if (captureGaps.Count >= 64)
			{
				captureGaps.RemoveAt(0);
			}
			captureGaps.Add(new CaptureGap
			{
				beginTimeNanoseconds = captureGapStartNs,
				endTimeNanoseconds = endTimeNanoseconds
			});
			captureGapStartNs = -1L;
		}
	}

	public static void SetMainThreadOnly(bool value)
	{
		if (mainThreadOnly != value)
		{
			mainThreadOnly = value;
			UpdateCapturing();
			if (enabled)
			{
				ProfilerCore.WriteReset();
				ProfilerWorker.Signal();
			}
		}
	}

	private static void UpdateCapturing()
	{
		ProfilerCore.OnlyThreadId = (mainThreadOnly ? MainThreadId : 0);
		Volatile.Write(ref ProfilerCore.Enabled, enabled);
		Volatile.Write(ref ProfilerCore.Capturing, enabled && !paused);
	}

	public static void Shutdown()
	{
		UpdateCapturing();
		ProfilerWorker.Shutdown();
	}

	public static bool IsLabelMuted(ProfilerLabelId id)
	{
		if ((uint)id.Value < 65535u)
		{
			return MutedLabels[id.Value];
		}
		return false;
	}

	public static void SetLabelMuted(ProfilerLabelId id, bool value)
	{
		if ((uint)id.Value >= 65535u)
		{
			return;
		}
		ushort value2 = id.Value;
		lock (LabelLock)
		{
			if (MutedLabels[value2] == value)
			{
				return;
			}
			if (!value)
			{
				AutoMuteExemptLabels[value2] = true;
				if (AutoMutedLabels[value2])
				{
					AutoMutedLabels[value2] = false;
					autoMutedLabelCount--;
				}
			}
			MutedLabels[value2] = value;
			mutedLabelCount += (value ? 1 : (-1));
		}
		RaiseOrDeferMutedLabelsChanged();
	}

	public static void ClearMutedLabels()
	{
		lock (LabelLock)
		{
			Array.Clear(MutedLabels);
			Array.Clear(AutoMuteExemptLabels);
			Array.Clear(AutoMutedLabels);
			mutedLabelCount = 0;
			autoMutedLabelCount = 0;
		}
		RaiseOrDeferMutedLabelsChanged();
	}

	internal static void AutoMuteLabels(List<int> labelIds)
	{
		int num = 0;
		lock (LabelLock)
		{
			foreach (int labelId in labelIds)
			{
				if ((uint)labelId < 65535u && !MutedLabels[labelId] && !AutoMuteExemptLabels[labelId])
				{
					MutedLabels[labelId] = true;
					AutoMutedLabels[labelId] = true;
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			mutedLabelCount += num;
			autoMutedLabelCount += num;
		}
		RaiseOrDeferMutedLabelsChanged();
	}

	private static void UnmuteAutoMutedLabels()
	{
		lock (LabelLock)
		{
			if (autoMutedLabelCount == 0)
			{
				return;
			}
			int num = labelCount;
			for (int i = 0; i < num; i++)
			{
				if (AutoMutedLabels[i])
				{
					AutoMutedLabels[i] = false;
					MutedLabels[i] = false;
					mutedLabelCount--;
				}
			}
			autoMutedLabelCount = 0;
		}
		RaiseOrDeferMutedLabelsChanged();
	}

	private static void RaiseOrDeferMutedLabelsChanged()
	{
		if (!IsMainThread)
		{
			Interlocked.Exchange(ref mutedLabelsChangedPending, 1);
			return;
		}
		Interlocked.Exchange(ref mutedLabelsChangedPending, 0);
		FrameProfiler.MutedLabelsChanged?.Invoke();
	}

	internal static void DispatchPendingNotifications()
	{
		if (Interlocked.Exchange(ref mutedLabelsChangedPending, 0) != 0)
		{
			FrameProfiler.MutedLabelsChanged?.Invoke();
		}
	}

	public static void MarkLabelDiscardable(ProfilerLabelId id)
	{
		if ((uint)id.Value < 65535u)
		{
			DiscardableLabels[id.Value] = true;
		}
	}

	internal static bool IsLabelDiscardable(ProfilerLabelId id)
	{
		if ((uint)id.Value < 65535u)
		{
			return DiscardableLabels[id.Value];
		}
		return false;
	}

	public static void CollectManuallyMutedLabelIds(List<ProfilerLabelId> into)
	{
		lock (LabelLock)
		{
			into.Clear();
			int num = mutedLabelCount - autoMutedLabelCount;
			int num2 = labelCount;
			for (int i = 0; i < num2; i++)
			{
				if (into.Count >= num)
				{
					break;
				}
				if (MutedLabels[i] && !AutoMutedLabels[i])
				{
					into.Add(new ProfilerLabelId((ushort)i));
				}
			}
		}
	}

	public static void SetAllocationTracking(bool value)
	{
		if (allocationTracking != value)
		{
			allocationTracking = value;
			SyncAllocationSubscription();
		}
	}

	private static void SyncAllocationSubscription()
	{
		bool flag = allocationTracking && enabled;
		if (flag != allocationSubscribed)
		{
			allocationSubscribed = flag;
			if (flag)
			{
				SelfProfiler.ObjectAllocated += ObjectAllocatedHandler;
			}
			else
			{
				SelfProfiler.ObjectAllocated -= ObjectAllocatedHandler;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static void OnObjectAllocated(object obj, nint size)
	{
		if (ProfilerCore.ShouldCaptureCurrentThread())
		{
			ProfilerCore.Write(new ProfilerAllocEvent(ProfilerClock.GetTimestamp(), (int)System.Math.Min(size, int.MaxValue), GetOrCreateTypeId(obj.GetType())).AsEvent);
		}
	}

	internal static ushort GetOrCreateTypeId(Type type)
	{
		nint value = type.TypeHandle.Value;
		if (AllocTypeIds.TryGetValue(value, out var value2))
		{
			return value2;
		}
		lock (AllocTypeLock)
		{
			if (AllocTypeIds.TryGetValue(value, out value2))
			{
				return value2;
			}
			if (allocTypeCount >= 65536)
			{
				return 0;
			}
			ushort num = (ushort)allocTypeCount++;
			AllocTypes[num] = type;
			AllocTypeIds[value] = num;
			return num;
		}
	}

	internal static Type GetAllocType(ushort typeId)
	{
		return AllocTypes[typeId] ?? typeof(object);
	}

	private static void CalibrateScopeOverhead()
	{
		nextCalibrationTimestamp = ProfilerClock.GetTimestamp() + (long)(2.0 * (double)ProfilerClock.Frequency);
		if (!IsMainThread)
		{
			scopeOverheadTicksPerPair = 0L;
			return;
		}
		ProfilerLabelId orCreateLabelId = GetOrCreateLabelId("<calibration>");
		long num = long.MaxValue;
		for (int i = 0; i <= 8; i++)
		{
			long timestamp = ProfilerClock.GetTimestamp();
			for (int j = 0; j < 256; j++)
			{
				if (TryEnterScope(orCreateLabelId))
				{
					LeaveScope();
				}
			}
			long num2 = ProfilerClock.GetTimestamp() - timestamp;
			if (i > 0 && num2 < num)
			{
				num = num2;
			}
		}
		long num3 = System.Math.Max(0L, num / 256);
		if (scopeOverheadTicksPerPair == 0L || num3 < scopeOverheadTicksPerPair)
		{
			scopeOverheadTicksPerPair = num3;
		}
	}

	public static void IdentifyMainThread()
	{
		Thread currentThread = Thread.CurrentThread;
		if (currentThread.Name == null)
		{
			string text2 = (currentThread.Name = "MainThread");
		}
		ProfilerCore.IdentifyMainThread();
	}

	public static void BeginFrame()
	{
		if (enabled && !paused && IsMainThread)
		{
			DispatchPendingNotifications();
			ProfilerInstrumentation.OnProfilerFrameStarted();
			if (frameActive)
			{
				EndFrameInternal();
			}
			if (ProfilerClock.GetTimestamp() >= nextCalibrationTimestamp)
			{
				CalibrateScopeOverhead();
			}
			frameActive = true;
			currentFrameNumber = Time.FrameCount;
			currentFrameStartTick = CurrentTickProvider();
			frameStartAllocatedBytes = GC.GetTotalAllocatedBytes();
			frameStartGen0Collections = GC.CollectionCount(0);
			frameStartGen1Collections = GC.CollectionCount(1);
			frameStartGen2Collections = GC.CollectionCount(2);
			ProfilerCore.Write(new ProfilerFrameMarkEvent(ProfilerEventType.FrameBegin, ProfilerClock.GetTimestamp(), currentFrameNumber).AsEvent);
		}
	}

	public static void EndFrame()
	{
		if (enabled && IsMainThread && frameActive)
		{
			EndFrameInternal();
		}
	}

	private static void EndFrameInternal()
	{
		FrameStatsRecord record = default(FrameStatsRecord);
		record.FrameNumber = currentFrameNumber;
		record.TickStart = currentFrameStartTick;
		record.TickEnd = CurrentTickProvider();
		record.AllocatedBytesDelta = System.Math.Max(0L, GC.GetTotalAllocatedBytes() - frameStartAllocatedBytes);
		record.Gen0 = GC.CollectionCount(0) - frameStartGen0Collections;
		record.Gen1 = GC.CollectionCount(1) - frameStartGen1Collections;
		record.Gen2 = GC.CollectionCount(2) - frameStartGen2Collections;
		ProfilerWorker.PushFrameStats(in record);
		ProfilerCore.Write(new ProfilerFrameMarkEvent(ProfilerEventType.FrameEnd, ProfilerClock.GetTimestamp(), currentFrameNumber).AsEvent);
		frameActive = false;
		currentFrameNumber = -1;
		ProfilerWorker.Signal();
	}

	public static bool TryEnterScope(string label)
	{
		if (!ProfilerCore.ShouldCaptureCurrentThread())
		{
			return false;
		}
		return TryEnterScopeCore(GetOrCreateLabelId(label, cacheByReference: true));
	}

	public static bool TryEnterScope(ProfilerLabelId labelId)
	{
		if (!ProfilerCore.ShouldCaptureCurrentThread())
		{
			return false;
		}
		return TryEnterScopeCore(labelId);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool TryEnterScopeCore(ProfilerLabelId labelId)
	{
		if (IsLabelMutedFast(labelId))
		{
			return false;
		}
		ProfilerCore.Write(new ProfilerZoneBeginEvent(ProfilerClock.GetTimestamp(), labelId).AsEvent);
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsLabelMutedFast(ProfilerLabelId labelId)
	{
		if ((uint)labelId.Value < 65535u)
		{
			return MutedLabels[labelId.Value];
		}
		return false;
	}

	public static void LeaveScope()
	{
		if (ProfilerCore.Enabled)
		{
			ProfilerEvent evt = default(ProfilerEvent);
			evt.Timestamp = ProfilerClock.GetTimestamp();
			evt.Type = ProfilerEventType.ZoneEnd;
			ProfilerCore.Write(evt);
		}
	}

	public static ScopeHandle Scope(string label)
	{
		return new ScopeHandle(TryEnterScope(label));
	}

	public static ReadScope BeginRead()
	{
		Interlocked.Increment(ref activeReaders);
		return default(ReadScope);
	}

	public static void CollectTimelines(List<ThreadTimeline> into)
	{
		ProfilerWorker.Assembler.CollectTimelines(into);
	}

	public static void CollectChunks(ThreadTimeline timeline, long startNs, long endNs, List<ZoneChunk> into)
	{
		ProfilerWorker.Assembler.CollectChunks(timeline, startNs, endNs, into);
	}

	public static void CollectChunks(FrameSnapshot frame, int threadId, long startNs, long endNs, List<ZoneChunk> into)
	{
		ProfilerWorker.Assembler.CollectChunks(frame, threadId, startNs, endNs, into);
	}

	public static bool HasChunks(ThreadTimeline timeline, long startNs, long endNs)
	{
		return ProfilerWorker.Assembler.HasChunks(timeline, startNs, endNs);
	}

	public static bool HasChunks(FrameSnapshot frame, int threadId, long startNs, long endNs)
	{
		return ProfilerWorker.Assembler.HasChunks(frame, threadId, startNs, endNs);
	}

	public static ProfilerLabelId PathLabel(int pathId)
	{
		ProfilerPathTable.Entry[] entries = ProfilerPathTable.Entries;
		if ((uint)pathId >= (uint)ProfilerPathTable.Count)
		{
			return ProfilerLabelId.None;
		}
		return entries[pathId].labelId;
	}

	public static RetainedMemory GetRetainedMemory()
	{
		RetainedMemory retainedMemory = ProfilerWorker.GetRetainedMemory();
		retainedMemory.eventChunkBytes = EventChunkPool.AllocatedBytes;
		retainedMemory.pathTableBytes = ProfilerPathTable.EstimateBytes();
		retainedMemory.aggregatorBytes = ProfilerWindowAggregator.EstimateBytes();
		retainedMemory.recyclePoolBytes = ProfilerWorker.Assembler.PooledBytes;
		return retainedMemory;
	}

	public static void SetHistoryCapacity(int capacity)
	{
		capacity = System.Math.Clamp(capacity, 64, 10000);
		if (capacity != historyCapacity)
		{
			historyCapacity = capacity;
			if (enabled)
			{
				ProfilerWorker.SetHistoryCapacity(capacity);
			}
		}
	}

	public static void ClearSpikes()
	{
		ProfilerWorker.ClearSpikes();
	}

	public static void ClearHistory()
	{
		ProfilerWorker.ClearHistory();
	}

	public static bool TryGetHistorySnapshot(int newestOffset, [NotNullWhen(true)] out FrameSnapshot? snapshot)
	{
		return ProfilerWorker.TryGetHistorySnapshot(newestOffset, out snapshot);
	}

	public static int CopyHistoryTotals(int sinceCompleted, Span<float> totalMs, Span<float> allocBytes, out int completedFrameCount, out int historyCount, out int historyEpoch)
	{
		return ProfilerWorker.CopyHistoryTotals(sinceCompleted, totalMs, allocBytes, out completedFrameCount, out historyCount, out historyEpoch);
	}

	public static bool TryGetSpikeSnapshot(int newestOffset, [NotNullWhen(true)] out FrameSnapshot? snapshot)
	{
		return ProfilerWorker.TryGetSpikeSnapshot(newestOffset, out snapshot);
	}

	public static string GetLabel(ProfilerLabelId id)
	{
		if ((uint)id.Value >= (uint)Volatile.Read(in labelCount))
		{
			return "<unknown>";
		}
		return Labels[id.Value];
	}

	public static bool IsPauseLabel(ProfilerLabelId id)
	{
		return (uint)(id.Value - firstPauseLabelId) < (uint)PauseLabelNames.Length;
	}

	private static int PauseLabelOffset(RuntimePauseReason reason, int condemnedGeneration)
	{
		switch (reason)
		{
		case RuntimePauseReason.GC:
			return ((uint)condemnedGeneration < 3u) ? condemnedGeneration : 3;
		case RuntimePauseReason.GCPrep:
			return 4;
		case RuntimePauseReason.ReJit:
			return 5;
		case RuntimePauseReason.Debugger:
		case RuntimePauseReason.DebuggerSweep:
			return 6;
		case RuntimePauseReason.AppDomainShutdown:
		case RuntimePauseReason.Shutdown:
			return 7;
		case RuntimePauseReason.Profiler:
			return 8;
		default:
			return 9;
		}
	}

	public static ProfilerLabelId GetPauseLabelId(RuntimePauseReason reason, int condemnedGeneration)
	{
		return new ProfilerLabelId((ushort)(firstPauseLabelId + PauseLabelOffset(reason, condemnedGeneration)));
	}

	public static string DescribePause(RuntimePauseReason reason, int condemnedGeneration)
	{
		return PauseLabelNames[PauseLabelOffset(reason, condemnedGeneration)];
	}

	public static ProfilerLabelId GetOrCreateLabelId(string label, bool cacheByReference = false)
	{
		string text = (string.IsNullOrEmpty(label) ? "<unnamed>" : label);
		if (IdentityLabelIds.TryGetValue(text, out var value))
		{
			return new ProfilerLabelId((ushort)value);
		}
		int value2;
		lock (LabelLock)
		{
			if (!OrdinalLabelIds.TryGetValue(text, out value2))
			{
				if (labelCount >= 65535)
				{
					return OverflowLabelId;
				}
				value2 = RegisterLabelLocked(text);
			}
		}
		if (cacheByReference && Volatile.Read(in identityLabelCount) < 4096 && IdentityLabelIds.TryAdd(text, value2))
		{
			Interlocked.Increment(ref identityLabelCount);
		}
		return new ProfilerLabelId((ushort)value2);
	}

	private static int RegisterLabelLocked(string label)
	{
		int num = labelCount;
		Labels[num] = label;
		OrdinalLabelIds[label] = num;
		Volatile.Write(ref labelCount, num + 1);
		return num;
	}

	internal static long TimestampToNanoseconds(long ticks)
	{
		if (ticks <= 0)
		{
			return 0L;
		}
		double num = nanosecondsPerTick;
		if (num == 0.0)
		{
			num = (nanosecondsPerTick = 1000000000.0 / (double)ProfilerClock.Frequency);
		}
		return (long)((double)ticks * num);
	}
}
