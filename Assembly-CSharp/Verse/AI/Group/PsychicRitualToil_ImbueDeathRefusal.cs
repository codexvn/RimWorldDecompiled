using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001072 RID: 4210
	public class PsychicRitualToil_ImbueDeathRefusal : PsychicRitualToil
	{
		// Token: 0x060066F6 RID: 26358 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_ImbueDeathRefusal()
		{
		}

		// Token: 0x060066F7 RID: 26359 RVA: 0x0020913A File Offset: 0x0020733A
		public PsychicRitualToil_ImbueDeathRefusal(PsychicRitualRoleDef targetRole)
		{
			this.targetRole = targetRole;
		}

		// Token: 0x060066F8 RID: 26360 RVA: 0x0020914C File Offset: 0x0020734C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			float skillOffsetPct = ((PsychicRitualDef_ImbueDeathRefusal)psychicRitual.def).skillOffsetPercentFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			PsychicRitualToil_ImbueDeathRefusal.tmpTargetPawns.Clear();
			PsychicRitualToil_ImbueDeathRefusal.tmpTargetPawns.AddRange(psychicRitual.assignments.AssignedPawns(this.targetRole));
			foreach (Pawn pawn in PsychicRitualToil_ImbueDeathRefusal.tmpTargetPawns)
			{
				this.ApplyOutcome(psychicRitual, pawn, skillOffsetPct);
			}
		}

		// Token: 0x060066F9 RID: 26361 RVA: 0x002091E4 File Offset: 0x002073E4
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn pawn, float skillOffsetPct)
		{
			Hediff_DeathRefusal firstHediff = pawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			if (firstHediff != null)
			{
				firstHediff.SetUseAmountDirect(firstHediff.UsesLeft + 1, false);
			}
			else
			{
				Hediff_DeathRefusal hediff_DeathRefusal = (Hediff_DeathRefusal)HediffMaker.MakeHediff(HediffDefOf.DeathRefusal, pawn, null);
				hediff_DeathRefusal.SetUseAmountDirect(1, false);
				pawn.health.AddHediff(hediff_DeathRefusal, null, null, null);
			}
			float num = 0f;
			foreach (SkillRecord skillRecord in pawn.skills.skills)
			{
				float num2 = skillRecord.XpTotalEarned * skillOffsetPct;
				skillRecord.Learn(num2, false, false);
				num += num2;
			}
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "ImbueDeathRefuralCompleteText".Translate(pawn, Mathf.Abs((int)num)), LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060066FA RID: 26362 RVA: 0x00209304 File Offset: 0x00207504
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
		}

		// Token: 0x04004970 RID: 18800
		public PsychicRitualRoleDef targetRole;

		// Token: 0x04004971 RID: 18801
		private static List<Pawn> tmpTargetPawns = new List<Pawn>(4);
	}
}
