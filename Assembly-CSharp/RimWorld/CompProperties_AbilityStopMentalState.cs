using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class CompProperties_AbilityStopMentalState : CompProperties_AbilityEffect
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__11 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_AbilityStopMentalState _003C_003E4__this;

		private AbilityDef parentDef;

		public AbilityDef _003C_003E3__parentDef;

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
		public _003CConfigErrors_003Ed__11(int _003C_003E1__state)
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
				CompProperties_AbilityStopMentalState compProperties_AbilityStopMentalState = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((AbilityCompProperties)compProperties_AbilityStopMentalState).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c4;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00ee;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0080:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_AbilityStopMentalState.psyfocusCostForMinor < 0f)
					{
						_003C_003E2__current = "psyfocusCostForMinor must be defined ";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c4;
					IL_00ee:
					if (compProperties_AbilityStopMentalState.psyfocusCostForExtreme < 0f)
					{
						_003C_003E2__current = "psyfocusCostForExtreme must be defined ";
						_003C_003E1__state = 4;
						return true;
					}
					break;
					IL_00c4:
					if (compProperties_AbilityStopMentalState.psyfocusCostForMajor < 0f)
					{
						_003C_003E2__current = "psyfocusCostForMajor must be defined ";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00ee;
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
			_003CConfigErrors_003Ed__11 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public List<MentalStateDef> exceptions;

	public float psyfocusCostForMinor = -1f;

	public float psyfocusCostForMajor = -1f;

	public float psyfocusCostForExtreme = -1f;

	public override bool OverridesPsyfocusCost => true;

	public override string PsyfocusCostExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder("PsyfocusCostPerMentalBreakIntensity".Translate() + ":");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("  - " + "MentalBreakIntensityMinor".Translate().CapitalizeFirst() + ": " + psyfocusCostForMinor.ToStringPercent());
			stringBuilder.AppendLine("  - " + "MentalBreakIntensityMajor".Translate().CapitalizeFirst() + ": " + psyfocusCostForMajor.ToStringPercent());
			stringBuilder.AppendLine("  - " + "MentalBreakIntensityExtreme".Translate().CapitalizeFirst() + ": " + psyfocusCostForExtreme.ToStringPercent());
			return stringBuilder.ToString();
		}
	}

	public override FloatRange PsyfocusCostRange => new FloatRange(psyfocusCostForMinor, psyfocusCostForExtreme);

	public CompProperties_AbilityStopMentalState()
	{
		compClass = typeof(CompAbilityEffect_StopMentalState);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__11))]
	public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__11(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
