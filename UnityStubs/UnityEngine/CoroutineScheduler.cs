using System;
using System.Collections;
using System.Collections.Generic;
using UnityStubs;
using Waylith.UnityCompat;

namespace UnityEngine;

internal static class CoroutineScheduler
{
	private static readonly List<CoroutineHandle> _handles = new List<CoroutineHandle>();

	private static readonly List<CoroutineHandle> _scratch = new List<CoroutineHandle>();

	private static bool _subscribed;

	private static bool _endOfFramePending;

	internal static CoroutineHandle Start(object owner, IEnumerator routine)
	{
		EnsureSubscribed();
		CoroutineHandle coroutineHandle = new CoroutineHandle
		{
			Owner = owner
		};
		coroutineHandle.Stack.Push(routine);
		_handles.Add(coroutineHandle);
		Advance(coroutineHandle, Time.time);
		return coroutineHandle;
	}

	internal static void Stop(CoroutineHandle handle)
	{
		if (handle != null)
		{
			handle.IsDone = true;
		}
	}

	internal static void StopAll(object owner)
	{
		foreach (CoroutineHandle handle in _handles)
		{
			if (handle.Owner == owner)
			{
				handle.IsDone = true;
			}
		}
	}

	private static void EnsureSubscribed()
	{
		if (!_subscribed)
		{
			UnityWorld unityWorld = UnityRuntime.Current?.World;
			if (unityWorld != null)
			{
				unityWorld.Updating += StepPreUpdate;
				unityWorld.PostUpdate += StepPostUpdate;
				unityWorld.PostLateUpdate += StepEndOfFrame;
				_subscribed = true;
			}
		}
	}

	private static void StepPreUpdate(UnityFrame frame)
	{
		Step(endOfFrame: false, frame.ElapsedTime, fixedUpdate: true);
	}

	private static void StepPostUpdate(UnityFrame frame)
	{
		Step(endOfFrame: false, frame.ElapsedTime);
	}

	private static void StepEndOfFrame(UnityFrame frame)
	{
		if (EndOfFrame.DrivenByHost)
		{
			_endOfFramePending = true;
		}
		else
		{
			Step(endOfFrame: true, frame.ElapsedTime);
		}
	}

	internal static void RunPendingEndOfFrame()
	{
		if (_endOfFramePending)
		{
			_endOfFramePending = false;
			Step(endOfFrame: true, Time.time);
		}
	}

	internal static bool HasEndOfFrameWaiters()
	{
		foreach (CoroutineHandle handle in _handles)
		{
			if (!handle.IsDone && handle.SuspendKind == CoroutineSuspendKind.EndOfFrame)
			{
				return true;
			}
		}
		return false;
	}

	private static void Step(bool endOfFrame, float elapsed, bool fixedUpdate = false)
	{
		_scratch.Clear();
		_scratch.AddRange(_handles);
		foreach (CoroutineHandle item in _scratch)
		{
			if (!item.IsDone)
			{
				CoroutineSuspendKind suspendKind = item.SuspendKind;
				if ((suspendKind != CoroutineSuspendKind.FixedUpdate) ? (!fixedUpdate && suspendKind switch
				{
					CoroutineSuspendKind.NextFrame => !endOfFrame, 
					CoroutineSuspendKind.EndOfFrame => endOfFrame, 
					CoroutineSuspendKind.WaitSeconds => !endOfFrame && elapsed >= item.ResumeTime, 
					CoroutineSuspendKind.AwaitCoroutine => !endOfFrame && (item.AwaitedHandle == null || item.AwaitedHandle.IsDone), 
					CoroutineSuspendKind.AwaitOperation => !endOfFrame && item.AwaitedOperation.isDone, 
					_ => false, 
				}) : fixedUpdate)
				{
					Advance(item, elapsed);
				}
			}
		}
		_handles.RemoveAll((CoroutineHandle h) => h.IsDone);
	}

	private static void Advance(CoroutineHandle handle, float elapsed)
	{
		while (true)
		{
			if (handle.Stack.Count == 0)
			{
				handle.IsDone = true;
				return;
			}
			IEnumerator enumerator = handle.Stack.Peek();
			bool flag;
			try
			{
				flag = enumerator.MoveNext();
			}
			catch (Exception value)
			{
				Console.Error.WriteLine($"[Coroutine] {value}");
				handle.IsDone = true;
				return;
			}
			if (!flag)
			{
				handle.Stack.Pop();
				continue;
			}
			object current = enumerator.Current;
			if (current != null)
			{
				if (!(current is WaitForEndOfFrame))
				{
					if (!(current is WaitForFixedUpdate))
					{
						if (!(current is WaitForSeconds waitForSeconds))
						{
							if (!(current is Coroutine coroutine))
							{
								if (current is AsyncOperation asyncOperation)
								{
									handle.SuspendKind = ((!asyncOperation.isDone) ? CoroutineSuspendKind.AwaitOperation : CoroutineSuspendKind.NextFrame);
									handle.AwaitedOperation = asyncOperation;
									return;
								}
								if (!(current is IEnumerator item))
								{
									break;
								}
								handle.Stack.Push(item);
							}
							else if (coroutine.Handle != null && !coroutine.Handle.IsDone)
							{
								handle.SuspendKind = CoroutineSuspendKind.AwaitCoroutine;
								handle.AwaitedHandle = coroutine.Handle;
								return;
							}
							continue;
						}
						handle.SuspendKind = CoroutineSuspendKind.WaitSeconds;
						handle.ResumeTime = elapsed + waitForSeconds.Seconds;
						return;
					}
					handle.SuspendKind = CoroutineSuspendKind.FixedUpdate;
					return;
				}
				handle.SuspendKind = CoroutineSuspendKind.EndOfFrame;
				return;
			}
			handle.SuspendKind = CoroutineSuspendKind.NextFrame;
			return;
		}
		handle.SuspendKind = CoroutineSuspendKind.NextFrame;
	}
}
