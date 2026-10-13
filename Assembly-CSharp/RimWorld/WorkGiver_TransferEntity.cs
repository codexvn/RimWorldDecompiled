using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001628 RID: 5672
	public class WorkGiver_TransferEntity : WorkGiver_Scanner
	{
		// Token: 0x060085BF RID: 34239 RVA: 0x0027D860 File Offset: 0x0027BA60
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x17001487 RID: 5255
		// (get) Token: 0x060085C0 RID: 34240 RVA: 0x0027D86A File Offset: 0x0027BA6A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.EntityHolder);
			}
		}

		// Token: 0x060085C1 RID: 34241 RVA: 0x0027ED70 File Offset: 0x0027CF70
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			Pawn entity = this.GetEntity(t);
			if (entity == null)
			{
				return false;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = entity.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget == null)
			{
				return false;
			}
			if (compHoldingPlatformTarget.targetHolder == null)
			{
				return false;
			}
			if (!pawn.CanReserve(compHoldingPlatformTarget.targetHolder, 1, -1, null, forced) || !pawn.CanReserve(entity, 1, -1, null, forced))
			{
				return false;
			}
			if (compHoldingPlatformTarget.HeldPlatform.IsForbidden(pawn) || compHoldingPlatformTarget.targetHolder.IsForbidden(pawn))
			{
				JobFailReason.Is("ForbiddenLower".Translate(), null);
				return false;
			}
			return compHoldingPlatformTarget.HeldPlatform != compHoldingPlatformTarget.targetHolder;
		}

		// Token: 0x060085C2 RID: 34242 RVA: 0x0027EE24 File Offset: 0x0027D024
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn entity = this.GetEntity(t);
			if (entity == null)
			{
				return null;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = entity.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.TransferBetweenEntityHolders, t, compHoldingPlatformTarget.targetHolder, entity).WithCount(1);
		}

		// Token: 0x060085C3 RID: 34243 RVA: 0x0027EE74 File Offset: 0x0027D074
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
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.HeldPlatform == compHoldingPlatformTarget.targetHolder)
			{
				return null;
			}
			return heldPawn;
		}
	}
}
