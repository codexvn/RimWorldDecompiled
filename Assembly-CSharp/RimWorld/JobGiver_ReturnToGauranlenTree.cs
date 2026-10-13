using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200157E RID: 5502
	public class JobGiver_ReturnToGauranlenTree : ThinkNode_JobGiver
	{
		// Token: 0x06008277 RID: 33399 RVA: 0x00273284 File Offset: 0x00271484
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (pawn.connections == null || pawn.connections.ConnectedThings.NullOrEmpty<Thing>())
			{
				return null;
			}
			foreach (Thing thing in pawn.connections.ConnectedThings)
			{
				CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && compTreeConnection.ShouldReturnToTree(pawn) && pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return JobMaker.MakeJob(JobDefOf.ReturnToGauranlenTree, thing);
				}
			}
			return null;
		}
	}
}
