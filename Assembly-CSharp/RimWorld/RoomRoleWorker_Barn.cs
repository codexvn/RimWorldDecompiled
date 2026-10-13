using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200215B RID: 8539
	public class RoomRoleWorker_Barn : RoomRoleWorker
	{
		// Token: 0x0600BA9D RID: 47773 RVA: 0x0035DFB4 File Offset: 0x0035C1B4
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && !building_Bed.def.building.bed_humanlike)
				{
					num++;
				}
			}
			return (float)num * 7.6f;
		}

		// Token: 0x0600BA9E RID: 47774 RVA: 0x0035E009 File Offset: 0x0035C209
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.building == null || !buildingDef.building.bed_humanlike)
			{
				return 0f;
			}
			return 7.6f;
		}
	}
}
