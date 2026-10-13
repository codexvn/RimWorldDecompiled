using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200277F RID: 10111
	public class RitualOutcomeEffectWorker_RoleChange : RitualOutcomeEffectWorker
	{
		// Token: 0x170021E8 RID: 8680
		// (get) Token: 0x0600DDA8 RID: 56744 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DDA9 RID: 56745 RVA: 0x00411463 File Offset: 0x0040F663
		public RitualOutcomeEffectWorker_RoleChange()
		{
		}

		// Token: 0x0600DDAA RID: 56746 RVA: 0x0041146B File Offset: 0x0040F66B
		public RitualOutcomeEffectWorker_RoleChange(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DDAB RID: 56747 RVA: 0x00413D8C File Offset: 0x00411F8C
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			RitualOutcomeComp ritualOutcomeComp = this.def.comps.First<RitualOutcomeComp>();
			float num = ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp));
			bool flag = num > 0.5f;
			RitualOutcomePossibility ritualOutcomePossibility = flag ? this.def.outcomeChances[1] : this.def.outcomeChances[0];
			string text = ritualOutcomePossibility.description.Formatted(jobRitual.Ritual.Label, jobRitual.Ritual.ideo.Named("IDEO")).CapitalizeFirst();
			text += "\n\n" + "RitualOutcomeQualitySpecific".Translate(jobRitual.Ritual.Label, num.ToStringPercent()).CapitalizeFirst() + ":\n";
			text = text + "\n  - " + ritualOutcomeComp.GetDesc(jobRitual, base.DataForComp(ritualOutcomeComp)).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(ritualOutcomePossibility.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text, ritualOutcomePossibility.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, jobRitual.selectedTarget, null, null, null, null, 0, true);
			if (flag)
			{
				Pawn pawn = jobRitual.assignments.FirstAssignedPawn("role_changer");
				Precept_Role roleChangeSelection = jobRitual.assignments.RoleChangeSelection;
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (roleChangeSelection != null)
				{
					if (role != null)
					{
						role.Unassign(pawn, false);
					}
					roleChangeSelection.Assign(pawn, true);
					return;
				}
				if (role != null)
				{
					role.Unassign(pawn, true);
				}
			}
		}

		// Token: 0x0600DDAC RID: 56748 RVA: 0x00413F58 File Offset: 0x00412158
		public override RitualOutcomePossibility GetForcedOutcome(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			RitualOutcomeComp ritualOutcomeComp = this.def.comps.First<RitualOutcomeComp>();
			if (ritualOutcomeComp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, base.DataForComp(ritualOutcomeComp)).quality <= 0.5f)
			{
				return this.def.outcomeChances[0];
			}
			return this.def.outcomeChances[1];
		}

		// Token: 0x0600DDAD RID: 56749 RVA: 0x00413FB7 File Offset: 0x004121B7
		public override IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
		{
			RitualOutcomeComp ritualOutcomeComp = this.def.comps.First<RitualOutcomeComp>();
			RitualOutcomeComp_DataRoleChangeParticipants ritualOutcomeComp_DataRoleChangeParticipants = (RitualOutcomeComp_DataRoleChangeParticipants)base.DataForComp(ritualOutcomeComp);
			if (ritualOutcomeComp.GetQualityFactor(ritual, target, null, assignments, ritualOutcomeComp_DataRoleChangeParticipants).quality < 0.5f)
			{
				yield return "MessageNotEnoughSpectators".Translate(ritualOutcomeComp_DataRoleChangeParticipants.desiredParticipantCount, ritual.Label);
			}
			Pawn pawn = assignments.FirstAssignedPawn("role_changer");
			if (pawn == null)
			{
				yield break;
			}
			if (assignments.RoleChangeSelection == pawn.Ideo.GetRole(pawn))
			{
				yield return "MessageRoleChangeChooseDifferentRole".Translate(pawn.Named("PAWN"));
			}
			yield break;
		}
	}
}
