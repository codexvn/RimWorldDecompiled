using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200231C RID: 8988
	public class Recipe_SurgicalInspection : Recipe_Surgery
	{
		// Token: 0x0600C326 RID: 49958 RVA: 0x00389210 File Offset: 0x00387410
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			string t;
			SurgicalInspectionOutcome surgicalInspectionOutcome = pawn.DoSurgicalInspection(billDoer, out t);
			if (surgicalInspectionOutcome != SurgicalInspectionOutcome.DetectedNoLetter)
			{
				TaggedString label = "LetterSurgicallyInspectedLabel".Translate();
				TaggedString taggedString = "LetterSurgicallyInspectedHeader".Translate(billDoer.Named("DOCTOR"), pawn.Named("PATIENT"));
				if (surgicalInspectionOutcome == SurgicalInspectionOutcome.Nothing)
				{
					taggedString += " " + "LetterSurgicallyInspectedNothing".Translate(billDoer.Named("DOCTOR"), pawn.Named("PATIENT")).CapitalizeFirst();
				}
				else
				{
					taggedString += t;
				}
				Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
			}
			if (billDoer != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 4f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			HediffComp_Disappears hediffComp_Disappears = pawn.health.AddHediff(HediffDefOf.Anesthetic, null, null, null).TryGetComp<HediffComp_Disappears>();
			hediffComp_Disappears.disappearsAfterTicks = 60000;
			hediffComp_Disappears.ticksToDisappear = 60000;
		}

		// Token: 0x04008518 RID: 34072
		private const int AnestheticTicks = 60000;

		// Token: 0x04008519 RID: 34073
		private const int CutDamage = 4;
	}
}
