using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C25 RID: 11301
	public class CompBiosculpterPod_AgeReversalCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x0600FD61 RID: 64865 RVA: 0x00498210 File Offset: 0x00496410
		public override string Description(Pawn tunedFor)
		{
			int num = 3600000;
			if (tunedFor != null)
			{
				num *= (int)tunedFor.ageTracker.AdultAgingMultiplier;
			}
			return base.Description(tunedFor).Formatted(num.ToStringTicksToPeriod(false, false, false, true, false).Named("DURATION"));
		}

		// Token: 0x0600FD62 RID: 64866 RVA: 0x0049825C File Offset: 0x0049645C
		public override void CycleCompleted(Pawn pawn)
		{
			int num = (int)(3600000f * pawn.ageTracker.AdultAgingMultiplier);
			long val = (long)(3600000f * pawn.ageTracker.AdultMinAge);
			pawn.ageTracker.AgeBiologicalTicks = Math.Max(val, pawn.ageTracker.AgeBiologicalTicks - (long)num);
			pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.ViaTreatment, false);
			int num2 = (int)(pawn.ageTracker.AgeReversalDemandedDeadlineTicks / 60000L);
			string text = "BiosculpterAgeReversalCompletedMessage".Translate(pawn.Named("PAWN"));
			Ideo ideo = pawn.Ideo;
			if (ideo != null && ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
			{
				text += " " + "AgeReversalExpectationDeadline".Translate(pawn.Named("PAWN"), num2.Named("DEADLINE")).CapitalizeFirst();
			}
			Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
