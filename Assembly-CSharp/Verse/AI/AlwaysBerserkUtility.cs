using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F55 RID: 3925
	public static class AlwaysBerserkUtility
	{
		// Token: 0x06006126 RID: 24870 RVA: 0x001EC278 File Offset: 0x001EA478
		public static bool TryTriggerBerserkBloodRage(Pawn pawn)
		{
			return ModsConfig.AnomalyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.BloodRage, false) && pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_BloodRage".Translate(), MentalBreakDefOf.Berserk);
		}

		// Token: 0x06006127 RID: 24871 RVA: 0x001EC2CC File Offset: 0x001EA4CC
		public static bool TryTriggerBerserkFrenzyInducer(Pawn pawn)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.FrenzyField, false);
			return firstHediffOfDef != null && firstHediffOfDef.CurStageIndex > 0 && pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_FrenzyInducer".Translate(), MentalBreakDefOf.Berserk);
		}
	}
}
