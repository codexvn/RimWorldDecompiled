using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013D7 RID: 5079
	public class JobGiver_Nuzzle : ThinkNode_JobGiver
	{
		// Token: 0x06007C31 RID: 31793 RVA: 0x00256644 File Offset: 0x00254844
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (NuzzleUtility.GetNuzzleMTBHours(pawn) <= 0f)
			{
				return null;
			}
			Pawn t;
			if (!(from p in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where !p.NonHumanlikeOrWildMan() && p != pawn && p.Position.InHorDistOf(pawn.Position, 40f) && pawn.GetRoom(RegionType.Set_All) == p.GetRoom(RegionType.Set_All) && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow(false, false, false, false)
			select p).TryRandomElement(out t))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Nuzzle, t);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			job.expiryInterval = 3000;
			return job;
		}

		// Token: 0x040053D1 RID: 21457
		private const float MaxNuzzleDistance = 40f;
	}
}
