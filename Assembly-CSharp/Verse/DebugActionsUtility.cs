using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public static class DebugActionsUtility
{
	[CompilerGenerated]
	private sealed class _003CPointsOptions_003Ed__1 : IEnumerable<float>, IEnumerable, IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool extended;

		public bool _003C_003E3__extended;

		private int _003Ci_003E5__2;

		float IEnumerator<float>.Current
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
		public _003CPointsOptions_003Ed__1(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!extended)
				{
					_003C_003E2__current = 35f;
					_003C_003E1__state = 1;
					return true;
				}
				_003Ci_003E5__2 = 20;
				goto IL_025d;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = 70f;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = 100f;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = 150f;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = 200f;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = 350f;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = 500f;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = 700f;
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = 1000f;
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = 1200f;
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = 1500f;
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = 2000f;
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E2__current = 3000f;
				_003C_003E1__state = 13;
				return true;
			case 13:
				_003C_003E1__state = -1;
				_003C_003E2__current = 4000f;
				_003C_003E1__state = 14;
				return true;
			case 14:
				_003C_003E1__state = -1;
				_003C_003E2__current = 5000f;
				_003C_003E1__state = 15;
				return true;
			case 15:
				_003C_003E1__state = -1;
				goto IL_0339;
			case 16:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 += 10;
				goto IL_025d;
			case 17:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 += 25;
				goto IL_029e;
			case 18:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 += 50;
				goto IL_02e5;
			case 19:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 += 100;
				goto IL_032c;
			case 20:
				_003C_003E1__state = -1;
				_003C_003E2__current = 7000f;
				_003C_003E1__state = 21;
				return true;
			case 21:
				_003C_003E1__state = -1;
				_003C_003E2__current = 8000f;
				_003C_003E1__state = 22;
				return true;
			case 22:
				_003C_003E1__state = -1;
				_003C_003E2__current = 9000f;
				_003C_003E1__state = 23;
				return true;
			case 23:
				_003C_003E1__state = -1;
				_003C_003E2__current = 10000f;
				_003C_003E1__state = 24;
				return true;
			case 24:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_02e5:
				if (_003Ci_003E5__2 < 1500)
				{
					_003C_003E2__current = _003Ci_003E5__2;
					_003C_003E1__state = 18;
					return true;
				}
				_003Ci_003E5__2 = 1500;
				goto IL_032c;
				IL_0339:
				_003C_003E2__current = 6000f;
				_003C_003E1__state = 20;
				return true;
				IL_025d:
				if (_003Ci_003E5__2 < 100)
				{
					_003C_003E2__current = _003Ci_003E5__2;
					_003C_003E1__state = 16;
					return true;
				}
				_003Ci_003E5__2 = 100;
				goto IL_029e;
				IL_032c:
				if (_003Ci_003E5__2 <= 5000)
				{
					_003C_003E2__current = _003Ci_003E5__2;
					_003C_003E1__state = 19;
					return true;
				}
				goto IL_0339;
				IL_029e:
				if (_003Ci_003E5__2 < 500)
				{
					_003C_003E2__current = _003Ci_003E5__2;
					_003C_003E1__state = 17;
					return true;
				}
				_003Ci_003E5__2 = 500;
				goto IL_02e5;
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
		IEnumerator<float> IEnumerable<float>.GetEnumerator()
		{
			_003CPointsOptions_003Ed__1 _003CPointsOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPointsOptions_003Ed__ = this;
			}
			else
			{
				_003CPointsOptions_003Ed__ = new _003CPointsOptions_003Ed__1(0);
			}
			_003CPointsOptions_003Ed__.extended = _003C_003E3__extended;
			return _003CPointsOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<float>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPopulationOptions_003Ed__2 : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private int _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private int _003Ci_003E5__2;

		int IEnumerator<int>.Current
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
		public _003CPopulationOptions_003Ed__2(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 1;
				goto IL_0057;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_0057;
			case 2:
				{
					_003C_003E1__state = -1;
					_003Ci_003E5__2 += 10;
					break;
				}
				IL_0057:
				if (_003Ci_003E5__2 <= 20)
				{
					_003C_003E2__current = _003Ci_003E5__2;
					_003C_003E1__state = 1;
					return true;
				}
				_003Ci_003E5__2 = 30;
				break;
			}
			if (_003Ci_003E5__2 <= 50)
			{
				_003C_003E2__current = _003Ci_003E5__2;
				_003C_003E1__state = 2;
				return true;
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
		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CPopulationOptions_003Ed__2(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<int>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRadiusOptions_003Ed__3 : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private int _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		int IEnumerator<int>.Current
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
		public _003CRadiusOptions_003Ed__3(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = 1;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = 2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = 3;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = 4;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = 5;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = 6;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = 7;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = 8;
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = 9;
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = 10;
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = 11;
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = 12;
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E2__current = 13;
				_003C_003E1__state = 13;
				return true;
			case 13:
				_003C_003E1__state = -1;
				_003C_003E2__current = 14;
				_003C_003E1__state = 14;
				return true;
			case 14:
				_003C_003E1__state = -1;
				_003C_003E2__current = 15;
				_003C_003E1__state = 15;
				return true;
			case 15:
				_003C_003E1__state = -1;
				_003C_003E2__current = 20;
				_003C_003E1__state = 16;
				return true;
			case 16:
				_003C_003E1__state = -1;
				_003C_003E2__current = 25;
				_003C_003E1__state = 17;
				return true;
			case 17:
				_003C_003E1__state = -1;
				_003C_003E2__current = 30;
				_003C_003E1__state = 18;
				return true;
			case 18:
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
		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CRadiusOptions_003Ed__3(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<int>)this).GetEnumerator();
		}
	}

	public static void DustPuffFrom(Thing t)
	{
		if (t is Pawn pawn)
		{
			pawn.Drawer.Notify_DebugAffected();
		}
	}

	[IteratorStateMachine(typeof(_003CPointsOptions_003Ed__1))]
	public static IEnumerable<float> PointsOptions(bool extended)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPointsOptions_003Ed__1(-2)
		{
			_003C_003E3__extended = extended
		};
	}

	[IteratorStateMachine(typeof(_003CPopulationOptions_003Ed__2))]
	public static IEnumerable<int> PopulationOptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPopulationOptions_003Ed__2(-2);
	}

	[IteratorStateMachine(typeof(_003CRadiusOptions_003Ed__3))]
	public static IEnumerable<int> RadiusOptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRadiusOptions_003Ed__3(-2);
	}
}
