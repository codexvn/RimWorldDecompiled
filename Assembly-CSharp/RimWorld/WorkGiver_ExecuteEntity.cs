using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015BD RID: 5565
	public class WorkGiver_ExecuteEntity : WorkGiver_EntityOnPlatform
	{
		// Token: 0x0600838B RID: 33675 RVA: 0x00277AE8 File Offset: 0x00275CE8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && this.GetEntity(t) != null;
		}

		// Token: 0x0600838C RID: 33676 RVA: 0x00277B08 File Offset: 0x00275D08
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ExecuteEntity, t);
		}

		// Token: 0x0600838D RID: 33677 RVA: 0x00277B1C File Offset: 0x00275D1C
		protected override Pawn GetEntity(Thing potentialPlatform)
		{
			Building_HoldingPlatform building_HoldingPlatform = potentialPlatform as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return null;
			}
			Pawn heldPawn = building_HoldingPlatform.HeldPawn;
			CompHoldingPlatformTarget compHoldingPlatformTarget = (heldPawn != null) ? heldPawn.TryGetComp<CompHoldingPlatformTarget>() : null;
			if (compHoldingPlatformTarget == null || compHoldingPlatformTarget.containmentMode != EntityContainmentMode.Execute)
			{
				return null;
			}
			return heldPawn;
		}
	}
}
