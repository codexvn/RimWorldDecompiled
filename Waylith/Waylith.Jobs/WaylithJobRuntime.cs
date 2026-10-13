using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using Brutal.Logging;
using Waylith.Profiling;
using Waylith.Runtime;

namespace Waylith.Jobs;

public static class WaylithJobRuntime
{
	private sealed class ParallelForJobRunner<TJob> where TJob : struct, IWaylithParallelForJob
	{
		public readonly Action ExecuteChunk;

		private readonly TJob job;

		private readonly int iterationCount;

		private readonly int chunkSize;

		private int nextStart;

		public ParallelForJobRunner(in TJob job, int iterationCount, int chunkSize)
		{
			this.job = job;
			this.iterationCount = iterationCount;
			this.chunkSize = chunkSize;
			ExecuteChunk = RunChunk;
		}

		private void RunChunk()
		{
			int num = Interlocked.Add(ref nextStart, chunkSize) - chunkSize;
			if (num < iterationCount)
			{
				int num2 = System.Math.Min(iterationCount, num + chunkSize);
				TJob val = job;
				for (int i = num; i < num2; i++)
				{
					val.Execute(i);
				}
			}
		}
	}

	private sealed class ParallelForActionRunner
	{
		public readonly Action ExecuteChunk;

		private readonly Action<int> action;

		private readonly int startInclusive;

		private readonly int endExclusive;

		private readonly int chunkSize;

		private int nextStart;

		public ParallelForActionRunner(Action<int> action, int startInclusive, int endExclusive, int chunkSize)
		{
			this.action = action;
			this.startInclusive = startInclusive;
			this.endExclusive = endExclusive;
			this.chunkSize = chunkSize;
			ExecuteChunk = RunChunk;
		}

		private void RunChunk()
		{
			int num = startInclusive + Interlocked.Add(ref nextStart, chunkSize) - chunkSize;
			if (num < endExclusive)
			{
				int num2 = System.Math.Min(endExclusive, num + chunkSize);
				for (int i = num; i < num2; i++)
				{
					action(i);
				}
			}
		}
	}

	private sealed class SingleJobRunner<TJob> where TJob : struct, IWaylithJob
	{
		private static readonly ConcurrentBag<SingleJobRunner<TJob>> Pool = new ConcurrentBag<SingleJobRunner<TJob>>();

		public readonly Action Execute;

		private TJob job;

		private SingleJobRunner()
		{
			Execute = Run;
		}

		public static SingleJobRunner<TJob> Rent(in TJob job)
		{
			if (!Pool.TryTake(out SingleJobRunner<TJob> result))
			{
				result = new SingleJobRunner<TJob>();
			}
			result.job = job;
			return result;
		}

		private void Run()
		{
			TJob val = job;
			job = default(TJob);
			Pool.Add(this);
			val.Execute();
		}
	}

	internal sealed class HandleState : IDisposable
	{
		private const int Pending = 0;

		private const int Running = 1;

		private const int Completed = 2;

		private const int Faulted = 3;

		private ManualResetEventSlim? _done;

		private readonly Lock _lock = new Lock();

		private List<HandleState>? _continuations;

		private ExceptionDispatchInfo? _failure;

		private Action? _action = action;

		private int _completionFrame = int.MinValue;

		private int _status;

		private int _runsToStart = runCount;

		private int _runsToFinish = runCount;

		internal int PendingDepCount;

		public int Id { get; }

		public int RunCount { get; }

		public bool IsCompleted
		{
			get
			{
				int num = Volatile.Read(in _status);
				if (num != 2)
				{
					return num == 3;
				}
				return true;
			}
		}

		public HandleState(int id, Action? action, int runCount = 1)
		{
			Id = id;
			RunCount = runCount;
			base._002Ector();
		}

		private bool TryClaimRun()
		{
			int num;
			do
			{
				num = Volatile.Read(in runCount);
				if (num <= 0)
				{
					return false;
				}
			}
			while (Interlocked.CompareExchange(ref runCount, num - 1, num) != num);
			return true;
		}

		public void Execute()
		{
			if (TryClaimRun())
			{
				ExecuteRun();
			}
		}

		public void WaitForCompletion(bool allowStealing = true)
		{
			switch (Volatile.Read(in _status))
			{
			case 2:
				return;
			case 3:
				_failure.Throw();
				return;
			}
			if (Volatile.Read(in PendingDepCount) == 0)
			{
				while (TryClaimRun())
				{
					ExecuteRun();
				}
			}
			while (!IsCompleted)
			{
				if (allowStealing && WorkBag.TryTake(out HandleState result))
				{
					result.Execute();
					continue;
				}
				ManualResetEventSlim manualResetEventSlim;
				using (_lock.EnterScope())
				{
					if (IsCompleted)
					{
						break;
					}
					manualResetEventSlim = _done ?? (_done = new ManualResetEventSlim(initialState: false));
					goto IL_0095;
				}
				IL_0095:
				manualResetEventSlim.Wait();
				break;
			}
			if (Volatile.Read(in _status) == 3)
			{
				_failure.Throw();
			}
		}

		private void ExecuteRun()
		{
			Interlocked.CompareExchange(ref _status, 1, 0);
			bool flag = false;
			try
			{
				Action action = action;
				if (action != null)
				{
					NativeAllocationRuntime.EnterJobTempScope(Id);
					flag = true;
					using (FrameProfiler.Scope("Run Job Action"))
					{
						action();
					}
				}
			}
			catch (Exception ex)
			{
				try
				{
					if (flag)
					{
						NativeAllocationRuntime.ExitJobTempScope(Id);
					}
					return;
				}
				finally
				{
					if (RunCount == 1)
					{
						MarkFaulted(ex);
					}
					else
					{
						OnError($"Job {Id} chunk faulted with exception: {ex}");
						if (Interlocked.Decrement(ref runCount) == 0)
						{
							MarkCompleted();
						}
					}
				}
			}
			if (flag)
			{
				NativeAllocationRuntime.ExitJobTempScope(Id);
			}
			if (Interlocked.Decrement(ref runCount) == 0)
			{
				MarkCompleted();
			}
		}

		public void OnDepCompleted()
		{
			if (Interlocked.Decrement(ref PendingDepCount) == 0)
			{
				Submit(this);
			}
		}

		public void AddContinuation(HandleState dependent)
		{
			bool flag;
			using (_lock.EnterScope())
			{
				int num = Volatile.Read(in _status);
				if (num == 2 || num == 3)
				{
					flag = true;
				}
				else
				{
					(_continuations ?? (_continuations = new List<HandleState>(1))).Add(dependent);
					flag = false;
				}
			}
			if (flag)
			{
				dependent.OnDepCompleted();
			}
		}

		public bool IsEligibleForCleanup(int completedBeforeFrame)
		{
			int num = Volatile.Read(in _status);
			if (num != 2 && num != 3)
			{
				return false;
			}
			return _completionFrame <= completedBeforeFrame;
		}

		public WaylithJobStateStatus GetStatus()
		{
			return (WaylithJobStateStatus)Volatile.Read(in _status);
		}

		public void Dispose()
		{
			_done?.Dispose();
		}

		private void MarkCompleted()
		{
			Interlocked.Decrement(ref inFlightCount);
			List<HandleState> continuations;
			using (_lock.EnterScope())
			{
				Volatile.Write(ref _completionFrame, Volatile.Read(in currentFrame));
				Interlocked.Exchange(ref _status, 2);
				continuations = _continuations;
				_continuations = null;
				action = null;
				_done?.Set();
			}
			if (continuations == null)
			{
				return;
			}
			foreach (HandleState item in continuations)
			{
				item.OnDepCompleted();
			}
		}

		private void MarkFaulted(Exception exception)
		{
			Interlocked.Decrement(ref inFlightCount);
			_failure = ExceptionDispatchInfo.Capture(exception);
			List<HandleState> continuations;
			using (_lock.EnterScope())
			{
				Volatile.Write(ref _completionFrame, Volatile.Read(in currentFrame));
				Interlocked.Exchange(ref _status, 3);
				continuations = _continuations;
				_continuations = null;
				action = null;
				_done?.Set();
			}
			if (continuations != null)
			{
				foreach (HandleState item in continuations)
				{
					item.OnDepCompleted();
				}
			}
			OnError($"Job {Id} faulted with exception: {exception}");
		}
	}

	private static readonly LogCategory Log;

	private static Action<string> OnError;

	private const int CompletedHandleRetentionFrames = 32;

	private static readonly ConcurrentDictionary<int, HandleState> States;

	private static readonly ConcurrentDictionary<int, byte> PinnedHandles;

	private static readonly ConcurrentBag<HandleState> WorkBag;

	private static readonly SemaphoreSlim WorkAvailable;

	private static readonly CancellationTokenSource Cts;

	private static readonly Thread[] Workers;

	private static int currentFrame;

	private static int lastCleanupFrame;

	private static int nextHandleId;

	private static int inFlightCount;

	[ThreadStatic]
	private static int threadSlot;

	[ThreadStatic]
	private static bool threadSlotAssigned;

	private static int nextExternalSlot;

	public static int CurrentThreadIndex
	{
		get
		{
			if (threadSlotAssigned)
			{
				return threadSlot;
			}
			int num = Workers.Length + Interlocked.Increment(ref nextExternalSlot) - 1;
			int processorCount = Environment.ProcessorCount;
			if (num > processorCount)
			{
				num = processorCount;
			}
			threadSlot = num;
			threadSlotAssigned = true;
			return num;
		}
	}

	public static bool HasPendingWork => Volatile.Read(in inFlightCount) > 0;

	public static int WorkerCount => Workers.Length;

	static WaylithJobRuntime()
	{
		Log = LogCategory.Make("JobRuntime");
		OnError = delegate(string s)
		{
			Log.Error(s, "OnError", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Jobs\\WaylithJobRuntime.cs", 15);
		};
		States = new ConcurrentDictionary<int, HandleState>();
		PinnedHandles = new ConcurrentDictionary<int, byte>();
		WorkBag = new ConcurrentBag<HandleState>();
		WorkAvailable = new SemaphoreSlim(0);
		Cts = new CancellationTokenSource();
		int num = System.Math.Max(1, Environment.ProcessorCount - 1);
		Workers = new Thread[num];
		for (int i = 0; i < num; i++)
		{
			int workerIndex = i;
			Workers[i] = new Thread((ThreadStart)delegate
			{
				WorkerLoop(workerIndex);
			})
			{
				Name = $"WaylithJob {i}",
				IsBackground = true,
				Priority = ThreadPriority.AboveNormal
			};
			Workers[i].Start();
		}
	}

	private static void WorkerLoop(int workerIndex)
	{
		threadSlot = workerIndex;
		threadSlotAssigned = true;
		while (true)
		{
			try
			{
				WorkAvailable.Wait(Cts.Token);
			}
			catch (OperationCanceledException)
			{
				break;
			}
			if (WorkBag.TryTake(out HandleState result))
			{
				result.Execute();
			}
		}
	}

	private static void Submit(HandleState state)
	{
		for (int i = 0; i < state.RunCount; i++)
		{
			WorkBag.Add(state);
		}
		WorkAvailable.Release(state.RunCount);
	}

	public static void BeginFrame(int frameIndex)
	{
		Volatile.Write(ref currentFrame, frameIndex);
		if (frameIndex - lastCleanupFrame >= 32)
		{
			lastCleanupFrame = frameIndex;
			CleanupCompletedStates(frameIndex - 32);
		}
	}

	public static void For<T>(T[] array, Action<T> action, int batchSize = 1)
	{
		Complete(ScheduleFor(array, action, batchSize));
	}

	public static void For<T>(List<T> list, Action<T> action, int batchSize = 1)
	{
		Complete(ScheduleFor(list, action, batchSize));
	}

	public static void For(int startInclusive, int endExclusive, Action<int> action, int batchSize = 1)
	{
		Complete(ScheduleFor(startInclusive, endExclusive, action, batchSize));
	}

	public static void For<TJob>(TJob job, int iterationCount, int batchSize = 1) where TJob : struct, IWaylithParallelForJob
	{
		Complete(ScheduleFor(job, iterationCount, batchSize));
	}

	public static int ScheduleFor<T>(T[] array, Action<T> action, int batchSize = 1, int dependsOn = 0)
	{
		if (array.Length == 0)
		{
			return dependsOn;
		}
		if (batchSize <= 0)
		{
			batchSize = 1;
		}
		return ScheduleFor(0, array.Length, delegate(int i)
		{
			action(array[i]);
		}, batchSize, dependsOn);
	}

	public static int ScheduleFor<T>(List<T> list, Action<T> action, int batchSize = 1, int dependsOn = 0)
	{
		if (list.Count == 0)
		{
			return dependsOn;
		}
		if (batchSize <= 0)
		{
			batchSize = 1;
		}
		return ScheduleFor(0, list.Count, delegate(int i)
		{
			action(list[i]);
		}, batchSize, dependsOn);
	}

	public static int ScheduleFor<TJob>(TJob job, int iterationCount, int batchSize = 1, int dependsOn = 0) where TJob : struct, IWaylithParallelForJob
	{
		if (iterationCount <= 0)
		{
			return dependsOn;
		}
		int num = System.Math.Max(1, batchSize);
		int num2 = (iterationCount + num - 1) / num;
		ParallelForJobRunner<TJob> parallelForJobRunner = new ParallelForJobRunner<TJob>(in job, iterationCount, num);
		if (num2 == 1)
		{
			Action executeChunk = parallelForJobRunner.ExecuteChunk;
			int reference = dependsOn;
			return Schedule(executeChunk, new ReadOnlySpan<int>(in reference), iterationCount);
		}
		return ScheduleChunks(parallelForJobRunner.ExecuteChunk, num2, num, dependsOn);
	}

	public static int ScheduleFor(int startInclusive, int endExclusive, Action<int> action, int batchSize = 1, int dependsOn = 0)
	{
		ArgumentNullException.ThrowIfNull(action, "action");
		int num = endExclusive - startInclusive;
		if (num <= 0)
		{
			return dependsOn;
		}
		int num2 = System.Math.Max(1, batchSize);
		int num3 = (num + num2 - 1) / num2;
		ParallelForActionRunner parallelForActionRunner = new ParallelForActionRunner(action, startInclusive, endExclusive, num2);
		if (num3 == 1)
		{
			Action executeChunk = parallelForActionRunner.ExecuteChunk;
			int reference = dependsOn;
			return Schedule(executeChunk, new ReadOnlySpan<int>(in reference), num);
		}
		return ScheduleChunks(parallelForActionRunner.ExecuteChunk, num3, num2, dependsOn);
	}

	private static int ScheduleChunks(Action executeChunk, int chunkCount, int timeCostEstimate, int dependsOn)
	{
		int num = Interlocked.Increment(ref nextHandleId);
		HandleState handleState = new HandleState(num, executeChunk, chunkCount);
		States[num] = handleState;
		int reference = dependsOn;
		RegisterAndMaybeSubmit(handleState, new ReadOnlySpan<int>(in reference));
		return num;
	}

	public static void Run<TJob>(TJob job) where TJob : struct, IWaylithJob
	{
		job.Execute();
	}

	public static int Schedule<TJob>(TJob job, int dependsOn = 0) where TJob : struct, IWaylithJob
	{
		Action execute = SingleJobRunner<TJob>.Rent(in job).Execute;
		int reference = dependsOn;
		return Schedule(execute, new ReadOnlySpan<int>(in reference));
	}

	public static int ScheduleForDeferred<TJob>(TJob job, Func<int> liveCount, int batchSize = 1, int dependsOn = 0) where TJob : struct, IWaylithParallelForJob
	{
		ArgumentNullException.ThrowIfNull(liveCount, "liveCount");
		TJob scheduledJob = job;
		Action action = delegate
		{
			Complete(ScheduleFor(scheduledJob, liveCount(), batchSize));
		};
		int reference = dependsOn;
		return Schedule(action, new ReadOnlySpan<int>(in reference));
	}

	public static int Schedule(Action action, ReadOnlySpan<int> dependencies, int timeCostEstimate = 1)
	{
		ArgumentNullException.ThrowIfNull(action, "action");
		int num = Interlocked.Increment(ref nextHandleId);
		HandleState handleState = new HandleState(num, action);
		States[num] = handleState;
		RegisterAndMaybeSubmit(handleState, dependencies);
		return num;
	}

	public static int ScheduleParallelBatch(Action[] actions, ReadOnlySpan<int> dependencies, int timeCostEstimate = 1)
	{
		ArgumentNullException.ThrowIfNull(actions, "actions");
		if (actions.Length == 0)
		{
			return CombineDependencies(dependencies);
		}
		if (actions.Length == 1)
		{
			return Schedule(actions[0], dependencies, timeCostEstimate);
		}
		Span<int> span = ((actions.Length > 256) ? ((Span<int>)new int[actions.Length]) : stackalloc int[actions.Length]);
		Span<int> span2 = span;
		for (int i = 0; i < actions.Length; i++)
		{
			Action action = actions[i];
			int num = Interlocked.Increment(ref nextHandleId);
			HandleState handleState = new HandleState(num, action);
			States[num] = handleState;
			span2[i] = num;
			RegisterAndMaybeSubmit(handleState, dependencies);
		}
		return CreateBarrierHandle(span2);
	}

	public static int CombineDependencies(int dependency0, int dependency1)
	{
		InlineArray2<int> buffer = default(InlineArray2<int>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<int>, int>(ref buffer, 0) = dependency0;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<int>, int>(ref buffer, 1) = dependency1;
		return CombineDependencies(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<int>, int>(in buffer, 2));
	}

	public static int CombineDependencies(ReadOnlySpan<int> dependencies)
	{
		int num = 0;
		int num2 = 0;
		ReadOnlySpan<int> readOnlySpan = dependencies;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			int num3 = readOnlySpan[i];
			if (num3 != 0)
			{
				num++;
				num2 = num3;
			}
		}
		return num switch
		{
			0 => 0, 
			1 => num2, 
			_ => CreateBarrierHandle(dependencies), 
		};
	}

	public static void Complete(int handleId)
	{
		if (handleId != 0 && States.TryGetValue(handleId, out HandleState value))
		{
			value.WaitForCompletion();
		}
	}

	private static void CompleteWithoutStealing(int handleId)
	{
		if (handleId != 0 && States.TryGetValue(handleId, out HandleState value))
		{
			value.WaitForCompletion(allowStealing: false);
		}
	}

	public static void ForWithoutStealing(int startInclusive, int endExclusive, Action<int> action, int batchSize = 1)
	{
		CompleteWithoutStealing(ScheduleFor(startInclusive, endExclusive, action, batchSize));
	}

	public static bool IsCompleted(int handleId)
	{
		if (handleId == 0)
		{
			return true;
		}
		if (States.TryGetValue(handleId, out HandleState value))
		{
			return value.IsCompleted;
		}
		return true;
	}

	public static int IdealBatchCount(int itemCount)
	{
		return System.Math.Max(1, itemCount / (WorkerCount * 4));
	}

	public static LoadingScope BeginLoadingScope(string label)
	{
		return new LoadingScope(label);
	}

	internal static void PinHandle(int handleId)
	{
		if (handleId != 0)
		{
			PinnedHandles.TryAdd(handleId, 0);
		}
	}

	internal static void UnpinHandle(int handleId)
	{
		PinnedHandles.TryRemove(handleId, out var _);
	}

	public static void Shutdown()
	{
		Cts.Cancel();
		bool flag = true;
		Thread[] workers = Workers;
		foreach (Thread thread in workers)
		{
			flag &= thread.Join(1000);
		}
		if (!flag)
		{
			return;
		}
		foreach (KeyValuePair<int, HandleState> state in States)
		{
			state.Value.Dispose();
		}
		States.Clear();
	}

	public static void SetErrorHandler(Action<string> onError)
	{
		OnError = onError;
	}

	public static WaylithJobRuntimeDiagnosticsSnapshot GetDiagnosticsSnapshot()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		foreach (KeyValuePair<int, HandleState> state in States)
		{
			state.Deconstruct(out var _, out var value);
			HandleState handleState = value;
			num++;
			switch (handleState.GetStatus())
			{
			case WaylithJobStateStatus.Pending:
				num2++;
				break;
			case WaylithJobStateStatus.Running:
				num3++;
				break;
			case WaylithJobStateStatus.Completed:
				num4++;
				break;
			case WaylithJobStateStatus.Faulted:
				num5++;
				break;
			}
		}
		return new WaylithJobRuntimeDiagnosticsSnapshot(Volatile.Read(in currentFrame), num, num2, num3, num4, num5);
	}

	private static void RegisterAndMaybeSubmit(HandleState state, ReadOnlySpan<int> depIds)
	{
		Interlocked.Increment(ref inFlightCount);
		int num = 0;
		ReadOnlySpan<int> readOnlySpan = depIds;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			if (readOnlySpan[i] != 0)
			{
				num++;
			}
		}
		if (num == 0)
		{
			Submit(state);
			return;
		}
		Volatile.Write(ref state.PendingDepCount, num);
		ReadOnlySpan<int> readOnlySpan2 = depIds;
		for (int i = 0; i < readOnlySpan2.Length; i++)
		{
			int num2 = readOnlySpan2[i];
			if (num2 != 0)
			{
				if (States.TryGetValue(num2, out HandleState value))
				{
					value.AddContinuation(state);
				}
				else
				{
					state.OnDepCompleted();
				}
			}
		}
	}

	private static int CreateBarrierHandle(ReadOnlySpan<int> depIds)
	{
		int num = Interlocked.Increment(ref nextHandleId);
		HandleState handleState = new HandleState(num, null);
		States[num] = handleState;
		RegisterAndMaybeSubmit(handleState, depIds);
		return num;
	}

	private static void CleanupCompletedStates(int completedBeforeFrame)
	{
		foreach (var (key, handleState2) in States)
		{
			if (!PinnedHandles.ContainsKey(key) && handleState2.IsEligibleForCleanup(completedBeforeFrame) && States.TryRemove(key, out HandleState value))
			{
				value.Dispose();
			}
		}
	}
}
