using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Verse.Utility;

public static class EnumUtility
{
	[CompilerGenerated]
	private sealed class _003CGetBitFlags_003Ed__2 : IEnumerable<Enum>, IEnumerable, IEnumerator<Enum>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Enum _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Enum input;

		public Enum _003C_003E3__input;

		private Array _003Cvalues_003E5__2;

		private int _003Ci_003E5__3;

		Enum IEnumerator<Enum>.Current
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
		public _003CGetBitFlags_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cvalues_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00a6;
			}
			_003C_003E1__state = -1;
			_003Cvalues_003E5__2 = Enum.GetValues(input.GetType());
			_003Ci_003E5__3 = 0;
			goto IL_00b6;
			IL_00a6:
			_003Ci_003E5__3++;
			goto IL_00b6;
			IL_00b6:
			if (_003Ci_003E5__3 < _003Cvalues_003E5__2.Length)
			{
				if (math.countbits(Convert.ToUInt64(_003Cvalues_003E5__2.GetValue(_003Ci_003E5__3))) == 1 && input.HasFlag((Enum)_003Cvalues_003E5__2.GetValue(_003Ci_003E5__3)))
				{
					_003C_003E2__current = (Enum)_003Cvalues_003E5__2.GetValue(_003Ci_003E5__3);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00a6;
			}
			return false;
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
		IEnumerator<Enum> IEnumerable<Enum>.GetEnumerator()
		{
			_003CGetBitFlags_003Ed__2 _003CGetBitFlags_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetBitFlags_003Ed__ = this;
			}
			else
			{
				_003CGetBitFlags_003Ed__ = new _003CGetBitFlags_003Ed__2(0);
			}
			_003CGetBitFlags_003Ed__.input = _003C_003E3__input;
			return _003CGetBitFlags_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Enum>)this).GetEnumerator();
		}
	}

	public static IEnumerable<T> GetValues<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>();
	}

	public static IEnumerable<T> GetValuesReverse<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>().Reverse();
	}

	[IteratorStateMachine(typeof(_003CGetBitFlags_003Ed__2))]
	public static IEnumerable<Enum> GetBitFlags(this Enum input)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetBitFlags_003Ed__2(-2)
		{
			_003C_003E3__input = input
		};
	}
}
