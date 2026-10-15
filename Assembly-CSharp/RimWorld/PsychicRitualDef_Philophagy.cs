using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class PsychicRitualDef_Philophagy : PsychicRitualDef_InvocationCircle
{
	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PsychicRitualDef_Philophagy _003C_003E4__this;

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
				PsychicRitualDef_Philophagy psychicRitualDef_Philophagy = _003C_003E4__this;
				Pawn pawn;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((PsychicRitualDef_InvocationCircle)psychicRitualDef_Philophagy).BlockingIssues(assignments, map).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
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
					pawn = assignments.FirstAssignedPawn(psychicRitualDef_Philophagy.TargetRole);
					if (pawn != null && !pawn.skills.skills.Any((SkillRecord s) => !s.TotallyDisabled))
					{
						_003C_003E2__current = "NoSkillsToTransfer".Translate(pawn.Named("TARGET"));
						_003C_003E1__state = 2;
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
	private sealed class _003CGetPawnTooltipExtras_003Ed__5 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
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
		public _003CGetPawnTooltipExtras_003Ed__5(int _003C_003E1__state)
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
				if (pawn.skills != null)
				{
					SkillRecord skillRecord = pawn.skills.skills.MaxBy((SkillRecord s) => s.XpTotalEarned);
					if (skillRecord.Level > 0)
					{
						_003C_003E2__current = string.Concat("HighestSkill".Translate() + ": " + skillRecord.def.skillLabel.CapitalizeFirst() + " (" + "SkillLevel".Translate() + " ", skillRecord.Level.ToString(), ")");
						_003C_003E1__state = 1;
						return true;
					}
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetPawnTooltipExtras_003Ed__5 _003CGetPawnTooltipExtras_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPawnTooltipExtras_003Ed__ = this;
			}
			else
			{
				_003CGetPawnTooltipExtras_003Ed__ = new _003CGetPawnTooltipExtras_003Ed__5(0);
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

	public FloatRange brainDamageRange;

	public SimpleCurve xpTransferFromQualityCurve;

	public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
	{
		List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
		list.Add(new PsychicRitualToil_Philophagy(InvokerRole, TargetRole, brainDamageRange));
		list.Add(new PsychicRitualToil_TargetCleanup(InvokerRole, TargetRole));
		return list;
	}

	public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
	{
		float num = xpTransferFromQualityCurve.Evaluate(qualityRange.min);
		string text = num.ToStringPercent();
		Pawn pawn = assignments.FirstAssignedPawn(InvokerRole);
		Pawn pawn2 = assignments.FirstAssignedPawn(TargetRole);
		TaggedString result = outcomeDescription.Formatted(text);
		if (pawn == null || pawn2 == null)
		{
			return result;
		}
		float xpTransfer;
		SkillDef philophagySkillAndXpTransfer = PsychicRitualUtility.GetPhilophagySkillAndXpTransfer(pawn, pawn2, num, out xpTransfer);
		if (philophagySkillAndXpTransfer != null)
		{
			result += "\n\n" + "PhilophagyTransferReport".Translate(pawn.Named("INVOKER"), xpTransfer, philophagySkillAndXpTransfer.Named("SKILL"), pawn2.Named("TARGET"));
		}
		return result;
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

	[IteratorStateMachine(typeof(_003CGetPawnTooltipExtras_003Ed__5))]
	public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPawnTooltipExtras_003Ed__5(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}
}
