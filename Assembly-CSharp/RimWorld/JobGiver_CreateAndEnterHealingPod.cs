using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001575 RID: 5493
	public class JobGiver_CreateAndEnterHealingPod : JobGiver_CreateAndEnterDryadHolder
	{
		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x0600824A RID: 33354 RVA: 0x00272D08 File Offset: 0x00270F08
		public override JobDef JobDef
		{
			get
			{
				return JobDefOf.CreateAndEnterHealingPod;
			}
		}

		// Token: 0x0600824B RID: 33355 RVA: 0x00272D10 File Offset: 0x00270F10
		public override bool ExtraValidator(Pawn pawn, CompTreeConnection connectionComp)
		{
			if (pawn.mindState != null && pawn.mindState.returnToHealingPod)
			{
				if (HealthAIUtility.ShouldSeekMedicalRest(pawn))
				{
					return true;
				}
				if (pawn.health.hediffSet.GetMissingPartsCommonAncestors().Any<Hediff_MissingPart>())
				{
					return true;
				}
			}
			return base.ExtraValidator(pawn, connectionComp);
		}
	}
}
