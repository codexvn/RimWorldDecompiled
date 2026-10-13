using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002162 RID: 8546
	public class RoomRoleWorker_DeathrestChamber : RoomRoleWorker
	{
		// Token: 0x0600BAB5 RID: 47797 RVA: 0x0035E6E0 File Offset: 0x0035C8E0
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def == ThingDefOf.DeathrestCasket)
				{
					num += 1f;
				}
				else if (thing.TryGetComp<CompDeathrestBindable>() != null)
				{
					num += 0.5f;
				}
			}
			return num * 100f;
		}

		// Token: 0x0600BAB6 RID: 47798 RVA: 0x0035E741 File Offset: 0x0035C941
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef == ThingDefOf.DeathrestCasket)
			{
				return 100f;
			}
			if (buildingDef.HasComp(typeof(CompDeathrestBindable)))
			{
				return 50f;
			}
			return 0f;
		}
	}
}
