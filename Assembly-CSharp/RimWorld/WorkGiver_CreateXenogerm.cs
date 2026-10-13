using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015AA RID: 5546
	public class WorkGiver_CreateXenogerm : WorkGiver_Scanner
	{
		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x06008322 RID: 33570 RVA: 0x002759A3 File Offset: 0x00273BA3
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GeneAssembler);
			}
		}

		// Token: 0x06008323 RID: 33571 RVA: 0x0027511F File Offset: 0x0027331F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}

		// Token: 0x06008324 RID: 33572 RVA: 0x002759B0 File Offset: 0x00273BB0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_GeneAssembler building_GeneAssembler = t as Building_GeneAssembler;
			if (building_GeneAssembler == null)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced) || !pawn.CanReserveSittableOrSpot(t.InteractionCell, forced))
			{
				return false;
			}
			if (building_GeneAssembler.ArchitesRequiredNow <= 0)
			{
				return building_GeneAssembler.CanBeWorkedOnNow.Accepted;
			}
			if (this.FindArchiteCapsule(pawn) == null)
			{
				JobFailReason.Is("NoIngredient".Translate(ThingDefOf.ArchiteCapsule), null);
				return false;
			}
			return true;
		}

		// Token: 0x06008325 RID: 33573 RVA: 0x00275A34 File Offset: 0x00273C34
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_GeneAssembler building_GeneAssembler = t as Building_GeneAssembler;
			if (building_GeneAssembler == null)
			{
				return null;
			}
			if (building_GeneAssembler.ArchitesRequiredNow > 0)
			{
				Thing thing = this.FindArchiteCapsule(pawn);
				if (thing != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer, thing, t);
					job.count = Mathf.Min(building_GeneAssembler.ArchitesRequiredNow, thing.stackCount);
					return job;
				}
			}
			return JobMaker.MakeJob(JobDefOf.CreateXenogerm, t, 1200, true);
		}

		// Token: 0x06008326 RID: 33574 RVA: 0x00275AA8 File Offset: 0x00273CA8
		private Thing FindArchiteCapsule(Pawn pawn)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.ArchiteCapsule), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}
	}
}
