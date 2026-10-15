using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class MentalFitDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__3 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MentalFitDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MentalFitDef mentalFitDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)mentalFitDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0073;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0073;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00b2;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0073:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (mentalFitDef.mentalState == null)
					{
						_003C_003E2__current = "mentalState not set.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00b2;
					IL_00b2:
					if (mentalFitDef.mtbDaysMoodCurve == null)
					{
						_003C_003E2__current = "mtbDaysMoodCurve not set.";
						_003C_003E1__state = 3;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public MentalStateDef mentalState;

	public SimpleCurve mtbDaysMoodCurve;

	public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__3))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public float CalculateMTBDays(Pawn pawn)
	{
		if (!developmentalStageFilter.Has(pawn.DevelopmentalStage))
		{
			return float.PositiveInfinity;
		}
		if (pawn.needs.mood == null)
		{
			return float.PositiveInfinity;
		}
		return mtbDaysMoodCurve?.Evaluate(pawn.needs.mood.CurLevelPercentage) ?? float.PositiveInfinity;
	}
}
