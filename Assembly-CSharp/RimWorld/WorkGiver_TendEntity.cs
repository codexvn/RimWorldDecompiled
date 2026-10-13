using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200161E RID: 5662
	public class WorkGiver_TendEntity : WorkGiver_EntityOnPlatform
	{
		// Token: 0x06008591 RID: 34193 RVA: 0x0027E878 File Offset: 0x0027CA78
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			Pawn entity = this.GetEntity(t);
			return entity != null && HealthAIUtility.ShouldBeTendedNowByPlayer(entity);
		}

		// Token: 0x06008592 RID: 34194 RVA: 0x0027E8AC File Offset: 0x0027CAAC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn entity = this.GetEntity(t);
			if (entity == null)
			{
				return null;
			}
			Thing thing = HealthAIUtility.FindBestMedicine(pawn, entity, false);
			JobDef tendEntity = JobDefOf.TendEntity;
			LocalTargetInfo targetA = t;
			Thing thing2 = thing;
			return JobMaker.MakeJob(tendEntity, targetA, (thing2 != null) ? thing2 : LocalTargetInfo.Invalid);
		}

		// Token: 0x06008593 RID: 34195 RVA: 0x0027E8F1 File Offset: 0x0027CAF1
		protected override Pawn GetEntity(Thing potentialPlatform)
		{
			return WorkGiver_TendEntity.GetTendableEnityFromPotentialPlatform(potentialPlatform);
		}

		// Token: 0x06008594 RID: 34196 RVA: 0x0027E8FC File Offset: 0x0027CAFC
		public static Pawn GetTendableEnityFromPotentialPlatform(Thing potentialPlatform)
		{
			Building_HoldingPlatform building_HoldingPlatform = potentialPlatform as Building_HoldingPlatform;
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
			if (compHoldingPlatformTarget != null && (compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release || compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Execute))
			{
				return null;
			}
			return heldPawn;
		}
	}
}
