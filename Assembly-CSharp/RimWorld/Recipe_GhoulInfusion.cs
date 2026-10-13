using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022FF RID: 8959
	public class Recipe_GhoulInfusion : Recipe_Surgery
	{
		// Token: 0x0600C2B6 RID: 49846 RVA: 0x003877D0 File Offset: 0x003859D0
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && !pawn.IsMutant && pawn.ageTracker.Adult && base.AvailableOnNow(thing, part);
		}

		// Token: 0x0600C2B7 RID: 49847 RVA: 0x0038780A File Offset: 0x00385A0A
		public override TaggedString GetConfirmation(Pawn pawn)
		{
			if (pawn.IsColonist)
			{
				return "GhoulConfirmation".Translate(pawn.Named("PAWN"));
			}
			return base.GetConfirmation(pawn);
		}

		// Token: 0x0600C2B8 RID: 49848 RVA: 0x00387834 File Offset: 0x00385A34
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			bool flag = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			Faction homeFaction = pawn.HomeFaction;
			MutantUtility.SetPawnAsMutantInstantly(pawn, MutantDefOf.Ghoul, RotStage.Fresh);
			if (pawn.Faction != Faction.OfPlayer)
			{
				pawn.SetFaction(Faction.OfPlayer, null);
			}
			if (billDoer != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			if (flag)
			{
				base.ReportViolation(pawn, billDoer, homeFaction, -50, HistoryEventDefOf.GhoulInfusionPerformed);
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ColonyGhouls, OpportunityType.GoodToKnow);
		}
	}
}
