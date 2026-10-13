using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013D3 RID: 5075
	public class JobGiver_Mate : ThinkNode_JobGiver
	{
		// Token: 0x06007C29 RID: 31785 RVA: 0x0025641C File Offset: 0x0025461C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.gender != Gender.Male || pawn.Sterile() || pawn.RaceProps.disableMating)
			{
				return null;
			}
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn3 = t as Pawn;
				return !pawn3.Downed && pawn3.CanCasuallyInteractNow(false, false, false, false) && !pawn3.IsForbidden(pawn) && pawn3.Faction == pawn.Faction && PawnUtility.FertileMateTarget(pawn, pawn3);
			};
			foreach (ThingDef singleDef in pawn.RaceProps.canCrossBreedWith.OrElseEmptyEnumerable<ThingDef>().Prepend(pawn.def))
			{
				Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(singleDef), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 30f, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (pawn2 != null)
				{
					return JobMaker.MakeJob(JobDefOf.Mate, pawn2);
				}
			}
			return null;
		}
	}
}
