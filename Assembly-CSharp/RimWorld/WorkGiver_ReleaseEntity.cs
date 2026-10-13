using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200160B RID: 5643
	public class WorkGiver_ReleaseEntity : WorkGiver_Scanner
	{
		// Token: 0x06008520 RID: 34080 RVA: 0x0027D860 File Offset: 0x0027BA60
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x06008521 RID: 34081 RVA: 0x0027D86A File Offset: 0x0027BA6A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.EntityHolder);
			}
		}

		// Token: 0x06008522 RID: 34082 RVA: 0x0027D873 File Offset: 0x0027BA73
		public override string PostProcessedGerund(Job job)
		{
			return "ReleasingEntity".Translate(this.def.gerund.Named("GERUND"), job.targetB.Label.Named("TARGETLABEL"));
		}

		// Token: 0x06008523 RID: 34083 RVA: 0x0027D8AE File Offset: 0x0027BAAE
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && this.GetEntity(t) != null;
		}

		// Token: 0x06008524 RID: 34084 RVA: 0x0027D8D0 File Offset: 0x0027BAD0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn entity = this.GetEntity(t);
			if (entity == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.ReleaseEntity, t, entity).WithCount(1);
		}

		// Token: 0x06008525 RID: 34085 RVA: 0x0027D908 File Offset: 0x0027BB08
		private Pawn GetEntity(Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return null;
			}
			Pawn heldPawn = building_HoldingPlatform.HeldPawn;
			if (heldPawn == null)
			{
				return null;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = heldPawn.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.containmentMode != EntityContainmentMode.Release)
			{
				return null;
			}
			return heldPawn;
		}
	}
}
