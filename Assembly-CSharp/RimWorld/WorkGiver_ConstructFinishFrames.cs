using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001567 RID: 5479
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x060081F3 RID: 33267 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060081F4 RID: 33268 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x060081F5 RID: 33269 RVA: 0x00271CB0 File Offset: 0x0026FEB0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x060081F6 RID: 33270 RVA: 0x00271D28 File Offset: 0x0026FF28
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
			if (!frame.IsCompleted())
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
			if (!GenConstruct.CanConstruct(frame, pawn, true, forced, null))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.FinishFrame, frame);
		}
	}
}
