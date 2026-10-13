using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015BE RID: 5566
	public class WorkGiver_ExtractBioferrite : WorkGiver_EntityOnPlatform
	{
		// Token: 0x0600838F RID: 33679 RVA: 0x00277AE8 File Offset: 0x00275CE8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && this.GetEntity(t) != null;
		}

		// Token: 0x06008390 RID: 33680 RVA: 0x00277B60 File Offset: 0x00275D60
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (this.GetEntity(t) == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.ExtractBioferrite, t);
		}

		// Token: 0x06008391 RID: 33681 RVA: 0x00277B80 File Offset: 0x00275D80
		protected override Pawn GetEntity(Thing potentialPlatform)
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
			if (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.extractBioferrite)
			{
				return null;
			}
			if (heldPawn.health.hediffSet.HasHediff(HediffDefOf.BioferriteExtracted, false))
			{
				JobFailReason.Is("BioferriteAlreadyExtracted".Translate(), "ExtractBioferrite".Translate(heldPawn.LabelShort));
				return null;
			}
			if (CompProducesBioferrite.BioferritePerDay(heldPawn) <= 0f)
			{
				return null;
			}
			return heldPawn;
		}
	}
}
