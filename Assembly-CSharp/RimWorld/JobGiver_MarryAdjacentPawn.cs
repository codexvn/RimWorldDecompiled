using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001449 RID: 5193
	public class JobGiver_MarryAdjacentPawn : ThinkNode_JobGiver
	{
		// Token: 0x06007DC7 RID: 32199 RVA: 0x0025E264 File Offset: 0x0025C464
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			Predicate<Thing> <>9__0;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = pawn.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(pawn.Map))
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					Predicate<Thing> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((Thing x) => x is Pawn && this.CanMarry(pawn, (Pawn)x)));
					}
					Thing thing = thingList.Find(match);
					if (thing != null)
					{
						return JobMaker.MakeJob(JobDefOf.MarryAdjacentPawn, thing);
					}
				}
			}
			return null;
		}

		// Token: 0x06007DC8 RID: 32200 RVA: 0x0025E324 File Offset: 0x0025C524
		private bool CanMarry(Pawn pawn, Pawn toMarry)
		{
			return !toMarry.Drafted && pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, toMarry);
		}
	}
}
