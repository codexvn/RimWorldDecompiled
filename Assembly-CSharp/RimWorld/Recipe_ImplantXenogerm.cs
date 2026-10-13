using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002302 RID: 8962
	public class Recipe_ImplantXenogerm : Recipe_Surgery
	{
		// Token: 0x0600C2C2 RID: 49858 RVA: 0x00387A78 File Offset: 0x00385C78
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!base.AvailableOnNow(thing, part))
			{
				return false;
			}
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn == null || !pawn.Spawned)
			{
				return false;
			}
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Xenogerm);
			if (list.Any<Thing>())
			{
				foreach (Thing thing2 in list)
				{
					if (!thing2.IsForbidden(pawn) && !thing2.Position.Fogged(pawn.Map))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600C2C3 RID: 49859 RVA: 0x00387B2C File Offset: 0x00385D2C
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModLister.CheckBiotech("xenogerm implanting"))
			{
				return;
			}
			if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			if (bill.xenogerm != null)
			{
				GeneUtility.ImplantXenogermItem(pawn, bill.xenogerm);
			}
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
			}
			if (ModsConfig.IdeologyActive)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InstalledProsthetic, billDoer.Named(HistoryEventArgsNames.Doer)), true);
			}
		}
	}
}
