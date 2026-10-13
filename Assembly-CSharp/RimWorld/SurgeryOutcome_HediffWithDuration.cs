using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037CE RID: 14286
	public class SurgeryOutcome_HediffWithDuration : SurgeryOutcome
	{
		// Token: 0x06013C8E RID: 81038 RVA: 0x005DEA68 File Offset: 0x005DCC68
		public override bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			if (Rand.Chance(this.chance))
			{
				Hediff hd = HediffMaker.MakeHediff(this.hediff, patient, this.applyEffectsToPart ? part : null);
				hd.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.RoundToInt(this.qualityToDurationDaysCurve.Evaluate(quality) * 60000f);
				patient.health.AddHediff(hd, null, null, null);
				return true;
			}
			return false;
		}

		// Token: 0x0400DB0A RID: 56074
		public HediffDef hediff;

		// Token: 0x0400DB0B RID: 56075
		public SimpleCurve qualityToDurationDaysCurve;
	}
}
