using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001573 RID: 5491
	public abstract class JobGiver_CreateAndEnterDryadHolder : ThinkNode_JobGiver
	{
		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x06008244 RID: 33348
		public abstract JobDef JobDef { get; }

		// Token: 0x06008245 RID: 33349 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ExtraValidator(Pawn pawn, CompTreeConnection connectionComp)
		{
			return false;
		}

		// Token: 0x06008246 RID: 33350 RVA: 0x00272BD4 File Offset: 0x00270DD4
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
			Predicate<IntVec3> <>9__0;
			foreach (Thing thing in pawn.connections.ConnectedThings)
			{
				CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && this.ExtraValidator(pawn, compTreeConnection) && pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					IntVec3 position = thing.Position;
					Map map = pawn.Map;
					int squareRadius = 4;
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, pawn.Map, null)));
					}
					IntVec3 intVec;
					if (CellFinder.TryFindRandomCellNear(position, map, squareRadius, validator, out intVec, -1))
					{
						return JobMaker.MakeJob(this.JobDef, thing);
					}
				}
			}
			return null;
		}

		// Token: 0x0400568A RID: 22154
		public const int SquareRadius = 4;
	}
}
