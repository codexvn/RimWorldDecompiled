using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001610 RID: 5648
	public class WorkGiver_Researcher : WorkGiver_Scanner
	{
		// Token: 0x17001471 RID: 5233
		// (get) Token: 0x06008541 RID: 34113 RVA: 0x0027DCDE File Offset: 0x0027BEDE
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				if (Find.ResearchManager.GetProject(null) == null)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Nothing);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.ResearchBench);
			}
		}

		// Token: 0x17001472 RID: 5234
		// (get) Token: 0x06008542 RID: 34114 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008543 RID: 34115 RVA: 0x0027DCFB File Offset: 0x0027BEFB
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return Find.ResearchManager.GetProject(null) == null;
		}

		// Token: 0x06008544 RID: 34116 RVA: 0x0027DD10 File Offset: 0x0027BF10
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			ResearchProjectDef project = Find.ResearchManager.GetProject(null);
			if (project == null)
			{
				return false;
			}
			Building_ResearchBench building_ResearchBench = t as Building_ResearchBench;
			return building_ResearchBench != null && project.CanBeResearchedAt(building_ResearchBench, false) && pawn.CanReserve(t, 1, -1, null, forced) && (!t.def.hasInteractionCell || pawn.CanReserveSittableOrSpot(t.InteractionCell, forced)) && new HistoryEvent(HistoryEventDefOf.Researching, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
		}

		// Token: 0x06008545 RID: 34117 RVA: 0x0027DD95 File Offset: 0x0027BF95
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Research, t);
		}

		// Token: 0x06008546 RID: 34118 RVA: 0x0027DDA7 File Offset: 0x0027BFA7
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return t.Thing.GetStatValue(StatDefOf.ResearchSpeedFactor, true, -1);
		}
	}
}
