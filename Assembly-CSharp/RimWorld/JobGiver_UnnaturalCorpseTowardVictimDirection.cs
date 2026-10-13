using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001474 RID: 5236
	public class JobGiver_UnnaturalCorpseTowardVictimDirection : ThinkNode_JobGiver
	{
		// Token: 0x06007E3E RID: 32318 RVA: 0x002602A4 File Offset: 0x0025E4A4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForAwoken(pawn, out unnaturalCorpseTracker))
			{
				return null;
			}
			Pawn haunted = unnaturalCorpseTracker.Haunted;
			if (haunted.DestroyedOrNull())
			{
				return null;
			}
			Vector3 normalized = (haunted.PositionHeld - pawn.Position).ToVector3().normalized;
			IntVec3 ideal = pawn.Position + (normalized * 10f).ToIntVec3();
			IntVec3 c;
			if (this.TryGetNearbyCell(pawn, ideal, out c))
			{
				return JobMaker.MakeJob(JobDefOf.Goto, c);
			}
			return null;
		}

		// Token: 0x06007E3F RID: 32319 RVA: 0x00260340 File Offset: 0x0025E540
		private bool TryGetNearbyCell(Pawn pawn, IntVec3 ideal, out IntVec3 cell)
		{
			return RCellFinder.TryFindRandomCellNearWith(ideal, (IntVec3 x) => x.Standable(pawn.Map) && pawn.CanReach(pawn.Position, x, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn), pawn.Map, out cell, 5, int.MaxValue);
		}

		// Token: 0x040054B7 RID: 21687
		private const float LookAheadDist = 10f;
	}
}
