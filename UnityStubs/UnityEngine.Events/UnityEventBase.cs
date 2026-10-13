using System;
using System.Collections.Generic;

namespace UnityEngine.Events;

public abstract class UnityEventBase
{
	private readonly List<Delegate> _listeners = new List<Delegate>();

	public int GetPersistentEventCount()
	{
		return 0;
	}

	public void RemoveAllListeners()
	{
		_listeners.Clear();
	}

	protected void AddCall(Delegate call)
	{
		if ((object)call != null)
		{
			_listeners.Add(call);
		}
	}

	protected void RemoveCall(Delegate call)
	{
		_listeners.Remove(call);
	}

	protected Delegate[] Snapshot()
	{
		return _listeners.ToArray();
	}
}
