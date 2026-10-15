using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class PsychicRitualDef_Chronophagy : PsychicRitualDef_InvocationCircle
{
	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PsychicRitualDef_Chronophagy _003C_003E4__this;

		private PsychicRitualRoleAssignments assignments;

		public PsychicRitualRoleAssignments _003C_003E3__assignments;

		private Map map;

		public Map _003C_003E3__map;

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
		public _003CBlockingIssues_003Ed__4(int _003C_003E1__state)
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
				PsychicRitualDef_Chronophagy psychicRitualDef_Chronophagy = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((PsychicRitualDef_InvocationCircle)psychicRitualDef_Chronophagy).BlockingIssues(assignments, map).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00e9;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (assignments.FirstAssignedPawn(psychicRitualDef_Chronophagy.invokerRole).ageTracker.AgeBiologicalYears < 13)
					{
						_003C_003E2__current = "InvokerTooYoung".Translate(13);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00e9;
					IL_00e9:
					if (assignments.FirstAssignedPawn(psychicRitualDef_Chronophagy.targetRole).ageTracker.AgeBiologicalYears < 13)
					{
						_003C_003E2__current = "PsychicRitualTargetTooYoung".Translate(13);
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
			_003CBlockingIssues_003Ed__4 _003CBlockingIssues_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBlockingIssues_003Ed__ = this;
			}
			else
			{
				_003CBlockingIssues_003Ed__ = new _003CBlockingIssues_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CBlockingIssues_003Ed__.assignments = _003C_003E3__assignments;
			_003CBlockingIssues_003Ed__.map = _003C_003E3__map;
			return _003CBlockingIssues_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPawnTooltipExtras_003Ed__6 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

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
		public _003CGetPawnTooltipExtras_003Ed__6(int _003C_003E1__state)
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
				_003C_003E2__current = string.Concat("Stat_Age_Label".Translate() + ": ", pawn.ageTracker.AgeBiologicalYears.ToString());
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetPawnTooltipExtras_003Ed__6 _003CGetPawnTooltipExtras_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPawnTooltipExtras_003Ed__ = this;
			}
			else
			{
				_003CGetPawnTooltipExtras_003Ed__ = new _003CGetPawnTooltipExtras_003Ed__6(0);
			}
			_003CGetPawnTooltipExtras_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGetPawnTooltipExtras_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public FloatRange yearsTransferredFromQualityRange;

	public SimpleCurve brainDamageFromAgeCurve;

	public const int MinimumAge = 13;

	public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
	{
		List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
		list.Add(new PsychicRitualToil_Chronophagy(InvokerRole, TargetRole, yearsTransferredFromQualityRange));
		list.Add(new PsychicRitualToil_TargetCleanup(InvokerRole, TargetRole));
		return list;
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__4))]
	public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__assignments = assignments,
			_003C_003E3__map = map
		};
	}

	public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
	{
		float num = yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min);
		Pawn pawn = assignments.FirstAssignedPawn(invokerRole);
		Pawn pawn2 = assignments.FirstAssignedPawn(targetRole);
		if (pawn != null)
		{
			float num2 = Mathf.Max(pawn.ageTracker.AgeBiologicalYearsFloat - num, 13f);
			float num3 = pawn.ageTracker.AgeBiologicalYearsFloat - num2;
			TaggedString result = outcomeDescription.Formatted(num3, yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min));
			if (pawn2 != null)
			{
				result += "\n\n" + "CronophagyAgeChange".Translate(pawn.Named("INVOKER"), pawn.ageTracker.AgeBiologicalYearsFloat - num3, pawn2.Named("TARGET"), pawn2.ageTracker.AgeBiologicalYearsFloat + yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min));
			}
			return result;
		}
		return outcomeDescription.Formatted(num, num);
	}

	[IteratorStateMachine(typeof(_003CGetPawnTooltipExtras_003Ed__6))]
	public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPawnTooltipExtras_003Ed__6(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}
}
