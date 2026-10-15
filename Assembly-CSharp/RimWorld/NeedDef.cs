using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class NeedDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__30 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public NeedDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__30(int _003C_003E1__state)
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
				NeedDef needDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)needDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c9;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00f4;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (needDef.description.NullOrEmpty() && needDef.showOnNeedList)
					{
						_003C_003E2__current = "no description";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c9;
					IL_00c9:
					if (needDef.needClass == null)
					{
						_003C_003E2__current = "needClass is null";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00f4;
					IL_00f4:
					if (needDef.needClass == typeof(Need_Seeker) && (needDef.seekerRisePerHour == 0f || needDef.seekerFallPerHour == 0f))
					{
						_003C_003E2__current = "seeker rise/fall rates not set";
						_003C_003E1__state = 4;
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
			_003CConfigErrors_003Ed__30 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__30(0)
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

	public Type needClass;

	public Intelligence minIntelligence;

	public bool colonistAndPrisonersOnly;

	public bool playerMechsOnly;

	public bool colonistsOnly;

	public bool slavesOnly;

	public bool onlyIfCausedByHediff;

	public bool onlyIfCausedByGene;

	public bool onlyIfCausedByTrait;

	public bool onlyIfCausedByIdeo;

	public bool neverOnPrisoner;

	public bool neverOnSlave;

	public List<RoyalTitleDef> titleRequiredAny;

	public List<HediffDef> hediffRequiredAny;

	public List<PreceptDef> nullifyingPrecepts;

	public List<CompProperties> requiredComps;

	public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

	public bool showOnNeedList = true;

	public float baseLevel = 0.5f;

	public bool major;

	public int listPriority;

	[NoTranslate]
	public string tutorHighlightTag;

	public bool showForCaravanMembers;

	public bool scaleBar;

	public bool showUnitTicks;

	public float fallPerDay = 0.5f;

	public float seekerRisePerHour;

	public float seekerFallPerHour;

	public bool freezeWhileSleeping;

	public bool freezeInMentalState;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__30))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__30(-2)
		{
			_003C_003E4__this = this
		};
	}
}
