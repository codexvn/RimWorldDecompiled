using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002163 RID: 8547
	public class RoomRoleWorker_DiningRoom : RoomRoleWorker
	{
		// Token: 0x0600BAB8 RID: 47800 RVA: 0x0035E770 File Offset: 0x0035C970
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building && thing.def.surfaceType == SurfaceType.Eat)
				{
					num++;
				}
			}
			return (float)num * 12f;
		}

		// Token: 0x0600BAB9 RID: 47801 RVA: 0x0035E7C7 File Offset: 0x0035C9C7
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.category == ThingCategory.Building && buildingDef.surfaceType == SurfaceType.Eat)
			{
				return 12f;
			}
			return 0f;
		}
	}
}
