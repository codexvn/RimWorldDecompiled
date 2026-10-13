using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace Waylith;

public sealed class MainThreadSynchronizationContext : SynchronizationContext
{
	private static readonly LogCategory Log = LogCategory.Make("MainThreadSync");

	private readonly int _mainThreadId;

	private readonly ConcurrentQueue<(SendOrPostCallback callback, object? state)> _queue = new ConcurrentQueue<(SendOrPostCallback, object)>();

	public bool IsMainThread => Environment.CurrentManagedThreadId == _mainThreadId;

	public MainThreadSynchronizationContext(int mainThreadId)
	{
		_mainThreadId = mainThreadId;
	}

	public override void Post(SendOrPostCallback d, object? state)
	{
		_queue.Enqueue((d, state));
	}

	public override void Send(SendOrPostCallback d, object? state)
	{
		if (IsMainThread)
		{
			d(state);
			return;
		}
		ManualResetEventSlim done = new ManualResetEventSlim(initialState: false);
		try
		{
			ExceptionDispatchInfo error = null;
			_queue.Enqueue((delegate
			{
				try
				{
					d(state);
				}
				catch (Exception source)
				{
					error = ExceptionDispatchInfo.Capture(source);
				}
				finally
				{
					done.Set();
				}
			}, null));
			done.Wait();
			error?.Throw();
		}
		finally
		{
			if (done != null)
			{
				((IDisposable)done).Dispose();
			}
		}
	}

	public override SynchronizationContext CreateCopy()
	{
		return this;
	}

	public void Pump()
	{
		int count = _queue.Count;
		for (int i = 0; i < count; i++)
		{
			if (!_queue.TryDequeue(out (SendOrPostCallback, object) result))
			{
				break;
			}
			try
			{
				result.Item1(result.Item2);
			}
			catch (Exception value)
			{
				LogCategory log = Log;
				bool shouldAppend;
				LogString<Error> message = new LogString<Error>(51, 1, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Unhandled exception in a main-thread continuation: ");
					message.AppendFormatted(value);
				}
				log.Error(message, "Pump", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\MainThreadSynchronizationContext.cs", 92);
			}
		}
	}
}
