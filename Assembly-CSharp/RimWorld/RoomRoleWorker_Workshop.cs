using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002171 RID: 8561
	public class RoomRoleWorker_Workshop : RoomRoleWorker
	{
		// Token: 0x0600BAE1 RID: 47841 RVA: 0x0035F150 File Offset: 0x0035D350
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				BuildingProperties building = containedAndAdjacentThings[i].def.building;
				if (((building != null) ? building.workTableRoomRole : null) == RoomRoleDefOf.Workshop)
				{
					num++;
				}
			}
			return 27f * (float)num;
		}

		// Token: 0x0600BAE2 RID: 47842 RVA: 0x0035F1A7 File Offset: 0x0035D3A7
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			BuildingProperties building = buildingDef.building;
			if (((building != null) ? building.workTableRoomRole : null) == RoomRoleDefOf.Workshop)
			{
				return 27f;
			}
			return 0f;
		}
	}
}
