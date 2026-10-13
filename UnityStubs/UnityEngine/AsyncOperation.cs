using System;

namespace UnityEngine;

public class AsyncOperation : YieldInstruction
{
	private bool _isDone;

	private Action<AsyncOperation> _completed;

	private bool _completedRaised;

	public bool isDone => IsDoneCore;

	public float progress => ProgressCore;

	internal virtual bool IsDoneCore => _isDone;

	internal virtual float ProgressCore
	{
		get
		{
			if (!_isDone)
			{
				return 0f;
			}
			return 1f;
		}
	}

	public event Action<AsyncOperation> completed
	{
		add
		{
			if (IsDoneCore && _completedRaised)
			{
				value?.Invoke(this);
			}
			else
			{
				_completed = (Action<AsyncOperation>)Delegate.Combine(_completed, value);
			}
		}
		remove
		{
			_completed = (Action<AsyncOperation>)Delegate.Remove(_completed, value);
		}
	}

	public AsyncOperation()
	{
	}

	protected AsyncOperation(bool isDone)
	{
		_isDone = isDone;
	}

	internal void RaiseCompleted()
	{
		if (!_completedRaised)
		{
			_completedRaised = true;
			Action<AsyncOperation> action = _completed;
			_completed = null;
			action?.Invoke(this);
		}
	}

	internal void MarkDone()
	{
		_isDone = true;
		RaiseCompleted();
	}

	internal static AsyncOperation CreateCompleted()
	{
		return new AsyncOperation
		{
			_isDone = true,
			_completedRaised = true
		};
	}
}
