using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualOutcomeEffectWorker_RoleChange : RitualOutcomeEffectWorker
{
	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__6 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualOutcomeEffectWorker_RoleChange _003C_003E4__this;

		private Precept_Ritual ritual;

		public Precept_Ritual _003C_003E3__ritual;

		private TargetInfo target;

		public TargetInfo _003C_003E3__target;

		private RitualRoleAssignments assignments;

		public RitualRoleAssignments _003C_003E3__assignments;

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
		public _003CBlockingIssues_003Ed__6(int _003C_003E1__state)
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
			RitualOutcomeEffectWorker_RoleChange ritualOutcomeEffectWorker_RoleChange = _003C_003E4__this;
			Pawn pawn;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				RitualOutcomeComp ritualOutcomeComp = ritualOutcomeEffectWorker_RoleChange.def.comps.First();
				RitualOutcomeComp_DataRoleChangeParticipants ritualOutcomeComp_DataRoleChangeParticipants = (RitualOutcomeComp_DataRoleChangeParticipants)ritualOutcomeEffectWorker_RoleChange.DataForComp(ritualOutcomeComp);
				if (ritualOutcomeComp.GetQualityFactor(ritual, target, null, assignments, ritualOutcomeComp_DataRoleChangeParticipants).quality < 0.5f)
				{
					_003C_003E2__current = "MessageNotEnoughSpectators".Translate(ritualOutcomeComp_DataRoleChangeParticipants.desiredParticipantCount, ritual.Label);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00ad;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00ad;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00ad:
				pawn = assignments.FirstAssignedPawn("role_changer");
				if (pawn == null)
				{
					return false;
				}
				if (assignments.RoleChangeSelection == pawn.Ideo.GetRole(pawn))
				{
					_003C_003E2__current = "MessageRoleChangeChooseDifferentRole".Translate(pawn.Named("PAWN"));
					_003C_003E1__state = 2;
					return true;
				}
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
			_003CBlockingIssues_003Ed__6 _003CBlockingIssues_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBlockingIssues_003Ed__ = this;
			}
			else
			{
				_003CBlockingIssues_003Ed__ = new _003CBlockingIssues_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CBlockingIssues_003Ed__.ritual = _003C_003E3__ritual;
			_003CBlockingIssues_003Ed__.target = _003C_003E3__target;
			_003CBlockingIssues_003Ed__.assignments = _003C_003E3__assignments;
			return _003CBlockingIssues_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public override bool SupportsAttachableOutcomeEffect => false;

	public RitualOutcomeEffectWorker_RoleChange()
	{
	}

	public RitualOutcomeEffectWorker_RoleChange(RitualOutcomeEffectDef def)
		: base(def)
	{
	}

	public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
	{
		RitualOutcomeComp ritualOutcomeComp = def.comps.First();
		float num = ritualOutcomeComp.QualityOffset(jobRitual, DataForComp(ritualOutcomeComp));
		bool num2 = num > 0.5f;
		RitualOutcomePossibility ritualOutcomePossibility = (num2 ? def.outcomeChances[1] : def.outcomeChances[0]);
		string text = ritualOutcomePossibility.description.Formatted(jobRitual.Ritual.Label, jobRitual.Ritual.ideo.Named("IDEO")).CapitalizeFirst();
		text += "\n\n" + "RitualOutcomeQualitySpecific".Translate(jobRitual.Ritual.Label, num.ToStringPercent()).CapitalizeFirst() + ":\n";
		text = text + "\n  - " + ritualOutcomeComp.GetDesc(jobRitual, DataForComp(ritualOutcomeComp)).CapitalizeFirst();
		Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(ritualOutcomePossibility.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text, ritualOutcomePossibility.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, jobRitual.selectedTarget);
		if (num2)
		{
			Pawn pawn = jobRitual.assignments.FirstAssignedPawn("role_changer");
			Precept_Role roleChangeSelection = jobRitual.assignments.RoleChangeSelection;
			Precept_Role role = pawn.Ideo.GetRole(pawn);
			if (roleChangeSelection != null)
			{
				role?.Unassign(pawn, generateThoughts: false);
				roleChangeSelection.Assign(pawn, addThoughts: true);
			}
			else
			{
				role?.Unassign(pawn, generateThoughts: true);
			}
		}
	}

	public override RitualOutcomePossibility GetForcedOutcome(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
	{
		RitualOutcomeComp ritualOutcomeComp = def.comps.First();
		if (!(ritualOutcomeComp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, DataForComp(ritualOutcomeComp)).quality > 0.5f))
		{
			return def.outcomeChances[0];
		}
		return def.outcomeChances[1];
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__6))]
	public override IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__ritual = ritual,
			_003C_003E3__target = target,
			_003C_003E3__assignments = assignments
		};
	}
}
