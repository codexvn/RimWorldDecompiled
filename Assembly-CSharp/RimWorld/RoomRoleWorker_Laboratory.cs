using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002166 RID: 8550
	public class RoomRoleWorker_Laboratory : RoomRoleWorker
	{
		// Token: 0x0600BAC1 RID: 47809 RVA: 0x0035EA18 File Offset: 0x0035CC18
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				BuildingProperties building = containedAndAdjacentThings[i].def.building;
				if (((building != null) ? building.workTableRoomRole : null) == RoomRoleDefOf.Laboratory)
				{
					num++;
				}
			}
			return 60f * (float)num;
		}

		// Token: 0x0600BAC2 RID: 47810 RVA: 0x0035EA77 File Offset: 0x0035CC77
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			BuildingProperties building = buildingDef.building;
			if (((building != null) ? building.workTableRoomRole : null) == RoomRoleDefOf.Laboratory)
			{
				return 60f;
			}
			return 0f;
		}
	}
}
