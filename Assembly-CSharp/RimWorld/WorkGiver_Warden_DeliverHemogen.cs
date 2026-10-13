using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001588 RID: 5512
	public class WorkGiver_Warden_DeliverHemogen : WorkGiver_Warden
	{
		// Token: 0x060082A8 RID: 33448 RVA: 0x00273E90 File Offset: 0x00272090
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn prisoner = (Pawn)t;
			if (!prisoner.guest.CanBeBroughtFood || !prisoner.Position.IsInPrisonCell(prisoner.Map))
			{
				return null;
			}
			if (WardenFeedUtility.ShouldBeFed(prisoner))
			{
				return null;
			}
			Pawn_GeneTracker genes = prisoner.genes;
			Gene_Hemogen gene_Hemogen = ((genes != null) ? genes.GetGene(GeneDefOf.Hemogenic) : null) as Gene_Hemogen;
			if (gene_Hemogen == null)
			{
				return null;
			}
			if (!gene_Hemogen.hemogenPacksAllowed)
			{
				return null;
			}
			if (!gene_Hemogen.ShouldConsumeHemogenNow())
			{
				return null;
			}
			if (this.HemogenPackAlreadyAvailableFor(prisoner))
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.HemogenPack), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing pack) => !pack.IsForbidden(pawn) && pawn.CanReserve(pack, 1, -1, null, false) && pack.GetRoom(RegionType.Set_All) != prisoner.GetRoom(RegionType.Set_All), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DeliverFood, thing, prisoner);
			job.count = 1;
			job.targetC = RCellFinder.SpotToChewStandingNear(prisoner, thing, null);
			return job;
		}

		// Token: 0x060082A9 RID: 33449 RVA: 0x00273FEC File Offset: 0x002721EC
		private bool HemogenPackAlreadyAvailableFor(Pawn prisoner)
		{
			if (prisoner.carryTracker.CarriedCount(ThingDefOf.HemogenPack) > 0)
			{
				return true;
			}
			if (prisoner.inventory.Count(ThingDefOf.HemogenPack) > 0)
			{
				return true;
			}
			Room room = prisoner.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				List<Region> regions = room.Regions;
				for (int i = 0; i < regions.Count; i++)
				{
					if (regions[i].ListerThings.ThingsOfDef(ThingDefOf.HemogenPack).Count > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
