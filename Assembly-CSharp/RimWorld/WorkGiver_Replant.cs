using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001571 RID: 5489
	public class WorkGiver_Replant : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x0600823D RID: 33341 RVA: 0x00271B82 File Offset: 0x0026FD82
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		// Token: 0x0600823E RID: 33342 RVA: 0x002729B0 File Offset: 0x00270BB0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Blueprint_Install blueprint_Install = t as Blueprint_Install;
			if (blueprint_Install != null)
			{
				ThingDef thingDef = blueprint_Install.def.entityDefToBuild as ThingDef;
				if (thingDef != null && thingDef.plant != null)
				{
					if (GenConstruct.FirstBlockingThing(blueprint_Install, pawn) != null)
					{
						return GenConstruct.HandleBlockingThingJob(blueprint_Install, pawn, forced);
					}
					Thing thing;
					AcceptanceReport report = thingDef.CanEverPlantAt(t.Position, t.Map, out thing, true, true, false);
					if (!report)
					{
						JobFailReason.Is(report.Reason, null);
						return null;
					}
					if (!thingDef.CanNowPlantAt(t.Position, t.Map, true))
					{
						return null;
					}
					if (thingDef.plant.interferesWithRoof && t.Position.Roofed(t.Map))
					{
						JobFailReason.Is("BlockedByRoof".Translate(), null);
						return null;
					}
					Thing miniToInstallOrBuildingToReinstall = blueprint_Install.MiniToInstallOrBuildingToReinstall;
					IThingHolder parentHolder = miniToInstallOrBuildingToReinstall.ParentHolder;
					if (parentHolder != null)
					{
						Pawn_CarryTracker pawn_CarryTracker = parentHolder as Pawn_CarryTracker;
						if (pawn_CarryTracker != null)
						{
							JobFailReason.Is("BeingCarriedBy".Translate(pawn_CarryTracker.pawn), null);
							return null;
						}
					}
					if (miniToInstallOrBuildingToReinstall.IsForbidden(pawn))
					{
						JobFailReason.Is(WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated, null);
						return null;
					}
					if (!pawn.CanReach(miniToInstallOrBuildingToReinstall, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						JobFailReason.Is(WorkGiver_ConstructDeliverResources.NoPathTranslated, null);
						return null;
					}
					if (!pawn.CanReserve(miniToInstallOrBuildingToReinstall, 1, -1, null, false))
					{
						Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(miniToInstallOrBuildingToReinstall, pawn, null);
						if (pawn2 != null)
						{
							JobFailReason.Is("ReservedBy".Translate(pawn2.LabelShort, pawn2), null);
						}
						return null;
					}
					Job job = JobMaker.MakeJob(JobDefOf.Replant);
					job.targetA = miniToInstallOrBuildingToReinstall;
					job.targetB = blueprint_Install;
					job.plantDefToSow = thingDef;
					job.count = 1;
					job.haulMode = HaulMode.ToContainer;
					return job;
				}
			}
			return null;
		}
	}
}
