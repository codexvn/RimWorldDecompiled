using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200216B RID: 8555
	public class RoomRoleWorker_PrisonCell : RoomRoleWorker
	{
		// Token: 0x0600BACF RID: 47823 RVA: 0x0035EDD0 File Offset: 0x0035CFD0
		public override float GetScore(Room room)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					if (!building_Bed.ForPrisoners)
					{
						return 0f;
					}
					if (building_Bed.Medical)
					{
						num2++;
					}
					else
					{
						num++;
					}
				}
			}
			if (num == 1)
			{
				return 170000f;
			}
			if (num2 == 1)
			{
				return 100000f;
			}
			return 0f;
		}

		// Token: 0x0600BAD0 RID: 47824 RVA: 0x0035EE5C File Offset: 0x0035D05C
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (room.Role == null || !buildingDef.thingClass.IsAssignableFrom(typeof(Building_Bed)) || !(room.Role.Worker is RoomRoleWorker_PrisonCell))
			{
				return 0f;
			}
			if (buildingDef.building == null || !buildingDef.building.bed_humanlike)
			{
				return 0f;
			}
			return -170000f;
		}
	}
}
