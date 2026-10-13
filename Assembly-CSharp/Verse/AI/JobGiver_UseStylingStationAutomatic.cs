using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000EAF RID: 3759
	public class JobGiver_UseStylingStationAutomatic : ThinkNode_JobGiver
	{
		// Token: 0x06005EA3 RID: 24227 RVA: 0x001E0D44 File Offset: 0x001DEF44
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (Find.TickManager.TicksGame < pawn.style.nextStyleChangeAttemptTick)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.StylingStation), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, false);
			if (thing == null)
			{
				pawn.style.ResetNextStyleChangeAttemptTick();
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.UseStylingStationAutomatic, thing);
		}
	}
}
