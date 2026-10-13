using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022FC RID: 8956
	public class Recipe_ExtractHemogen : Recipe_Surgery
	{
		// Token: 0x0600C2A8 RID: 49832 RVA: 0x003873B0 File Offset: 0x003855B0
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			return (((pawn != null) ? pawn.genes : null) == null || !pawn.genes.HasActiveGene(GeneDefOf.Hemogenic)) && (pawn == null || pawn.health.CanBleed) && base.AvailableOnNow(thing, part);
		}

		// Token: 0x0600C2A9 RID: 49833 RVA: 0x00387400 File Offset: 0x00385600
		public override AcceptanceReport AvailableReport(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.DevelopmentalStage.Baby())
			{
				return "TooSmall".Translate();
			}
			return base.AvailableReport(thing, part);
		}

		// Token: 0x0600C2AA RID: 49834 RVA: 0x0038743C File Offset: 0x0038563C
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return base.CompletableEver(surgeryTarget) && this.PawnHasEnoughBloodForExtraction(surgeryTarget);
		}

		// Token: 0x0600C2AB RID: 49835 RVA: 0x00387450 File Offset: 0x00385650
		public override void CheckForWarnings(Pawn medPawn)
		{
			base.CheckForWarnings(medPawn);
			if (!this.PawnHasEnoughBloodForExtraction(medPawn))
			{
				Messages.Message("MessageCannotStartHemogenExtraction".Translate(medPawn.Named("PAWN")), medPawn, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x0600C2AC RID: 49836 RVA: 0x00387490 File Offset: 0x00385690
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModLister.CheckBiotech("Hemogen extraction"))
			{
				return;
			}
			if (!this.PawnHasEnoughBloodForExtraction(pawn))
			{
				Messages.Message("MessagePawnHadNotEnoughBloodToProduceHemogenPack".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, true);
				return;
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, pawn, null);
			hediff.Severity = 0.45f;
			pawn.health.AddHediff(hediff, null, null, null);
			this.OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -1, HistoryEventDefOf.ExtractedHemogenPack);
			}
		}

		// Token: 0x0600C2AD RID: 49837 RVA: 0x00387540 File Offset: 0x00385740
		protected override void OnSurgerySuccess(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.HemogenPack, null), pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near, null, null, null, 1))
			{
				Log.Error("Could not drop hemogen pack near " + pawn.PositionHeld.ToString());
			}
		}

		// Token: 0x0600C2AE RID: 49838 RVA: 0x0038759C File Offset: 0x0038579C
		private bool PawnHasEnoughBloodForExtraction(Pawn pawn)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			return firstHediffOfDef == null || firstHediffOfDef.Severity < 0.45f;
		}

		// Token: 0x040084E0 RID: 34016
		private const float BloodlossSeverity = 0.45f;

		// Token: 0x040084E1 RID: 34017
		private const float MinBloodlossForFailure = 0.45f;
	}
}
