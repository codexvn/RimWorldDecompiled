using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014DA RID: 5338
	public class JoyGiver_InteractBuildingSitAdjacent : JoyGiver_InteractBuilding
	{
		// Token: 0x06007F8C RID: 32652 RVA: 0x00266260 File Offset: 0x00264460
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Clear();
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.AddRange(GenAdjFast.AdjacentCellsCardinal(t));
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Shuffle<IntVec3>();
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < JoyGiver_InteractBuildingSitAdjacent.tmpCells.Count; j++)
				{
					IntVec3 intVec = JoyGiver_InteractBuildingSitAdjacent.tmpCells[j];
					if (!intVec.IsForbidden(pawn) && pawn.CanReserveSittableOrSpot(intVec, false))
					{
						if (i == 0)
						{
							Building edifice = intVec.GetEdifice(pawn.Map);
							if (edifice == null || !edifice.def.building.isSittable)
							{
								goto IL_95;
							}
						}
						return JobMaker.MakeJob(this.def.jobDef, t, intVec);
					}
					IL_95:;
				}
				if (this.def.requireChair)
				{
					break;
				}
			}
			return null;
		}

		// Token: 0x04005558 RID: 21848
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
