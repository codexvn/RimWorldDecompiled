using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200231E RID: 8990
	public class Recipe_ImplantIUD : Recipe_AddHediff
	{
		// Token: 0x0600C32B RID: 49963 RVA: 0x0038941C File Offset: 0x0038761C
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && pawn.ageTracker.AgeBiologicalYears >= 16 && base.AvailableOnNow(thing, part);
		}

		// Token: 0x0600C32C RID: 49964 RVA: 0x00389450 File Offset: 0x00387650
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);
			if (pawn.RaceProps.Humanlike)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnantHuman, false);
				if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex == 0)
				{
					PregnancyUtility.TryTerminatePregnancy(pawn);
				}
			}
		}

		// Token: 0x0600C32D RID: 49965 RVA: 0x003894A0 File Offset: 0x003876A0
		public override TaggedString GetConfirmation(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnantHuman, false);
				if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex == 0)
				{
					return "ConfirmationPawnPregnancyTerminated".Translate(pawn.Named("PAWN"));
				}
			}
			return base.GetConfirmation(pawn);
		}
	}
}
