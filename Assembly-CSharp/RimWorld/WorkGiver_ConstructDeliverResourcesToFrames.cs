using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001566 RID: 5478
	public class WorkGiver_ConstructDeliverResourcesToFrames : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x060081F0 RID: 33264 RVA: 0x00271CB0 File Offset: 0x0026FEB0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x060081F1 RID: 33265 RVA: 0x00271CBC File Offset: 0x0026FEBC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Frame frame = t as Frame;
			if (frame == null)
			{
				return null;
			}
			if (!GenConstruct.CanTouchTargetFromValidCell(frame, pawn))
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(frame, pawn, this.def.workType, forced, JobDefOf.HaulToContainer))
			{
				return null;
			}
			return base.ResourceDeliverJobFor(pawn, frame, true, forced);
		}
	}
}
