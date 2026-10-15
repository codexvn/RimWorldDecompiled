using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

public static class Delay
{
	private class CoroutineRunner : MonoBehaviour
	{
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public int targetTick;

		internal bool _003CInternal_AfterNTicks_003Eb__0()
		{
			return GenTicks.TicksGame >= targetTick;
		}
	}

	[CompilerGenerated]
	private sealed class _003CInternal_AfterNSeconds_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float seconds;

		public Action action;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CInternal_AfterNSeconds_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForSeconds(seconds);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				action?.Invoke();
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CInternal_AfterNTicks_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public int targetTick;

		public Action action;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CInternal_AfterNTicks_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitUntil(new _003C_003Ec__DisplayClass6_0
				{
					targetTick = targetTick
				}._003CInternal_AfterNTicks_003Eb__0);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				action?.Invoke();
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	private static CoroutineRunner coroutineRunner;

	private static Coroutine StartCoroutine(IEnumerator coroutine)
	{
		if (!coroutineRunner)
		{
			coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
		}
		return coroutineRunner.StartCoroutine(coroutine);
	}

	public static void AfterNSeconds(float seconds, Action action)
	{
		StartCoroutine(Internal_AfterNSeconds(seconds, action));
	}

	[IteratorStateMachine(typeof(_003CInternal_AfterNSeconds_003Ed__4))]
	private static IEnumerator Internal_AfterNSeconds(float seconds, Action action)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CInternal_AfterNSeconds_003Ed__4(0)
		{
			seconds = seconds,
			action = action
		};
	}

	public static void AfterNTicks(int ticks, Action action)
	{
		StartCoroutine(Internal_AfterNTicks(GenTicks.TicksGame + ticks, action));
	}

	[IteratorStateMachine(typeof(_003CInternal_AfterNTicks_003Ed__6))]
	private static IEnumerator Internal_AfterNTicks(int targetTick, Action action)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CInternal_AfterNTicks_003Ed__6(0)
		{
			targetTick = targetTick,
			action = action
		};
	}
}
