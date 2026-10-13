using System;
using System.Collections.Concurrent;

namespace Waylith.Rendering;

public sealed class RenderToMainThreadDispatcher
{
	private readonly ConcurrentQueue<Action> _callbacks = new ConcurrentQueue<Action>();

	public void Enqueue(Action callback)
	{
		ArgumentNullException.ThrowIfNull(callback, "callback");
		_callbacks.Enqueue(callback);
	}

	public void Invoke()
	{
		Action result;
		while (_callbacks.TryDequeue(out result))
		{
			try
			{
				result();
			}
			catch (Exception value)
			{
				Console.Error.WriteLine($"Main-thread render callback threw: {value}");
			}
		}
	}

	public void Clear()
	{
		_callbacks.Clear();
	}
}
