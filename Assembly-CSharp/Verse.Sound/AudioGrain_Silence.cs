using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse.Sound;

public class AudioGrain_Silence : AudioGrain
{
	[CompilerGenerated]
	private sealed class _003CGetResolvedGrains_003Ed__1 : IEnumerable<ResolvedGrain>, IEnumerable, IEnumerator<ResolvedGrain>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ResolvedGrain _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AudioGrain_Silence _003C_003E4__this;

		ResolvedGrain IEnumerator<ResolvedGrain>.Current
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
		public _003CGetResolvedGrains_003Ed__1(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			AudioGrain_Silence sourceGrain = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new ResolvedGrain_Silence(sourceGrain);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
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

		[DebuggerHidden]
		IEnumerator<ResolvedGrain> IEnumerable<ResolvedGrain>.GetEnumerator()
		{
			_003CGetResolvedGrains_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetResolvedGrains_003Ed__1(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ResolvedGrain>)this).GetEnumerator();
		}
	}

	[EditSliderRange(0f, 5f)]
	public FloatRange durationRange = new FloatRange(1f, 2f);

	[IteratorStateMachine(typeof(_003CGetResolvedGrains_003Ed__1))]
	public override IEnumerable<ResolvedGrain> GetResolvedGrains()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetResolvedGrains_003Ed__1(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override int GetHashCode()
	{
		return durationRange.GetHashCode();
	}
}
