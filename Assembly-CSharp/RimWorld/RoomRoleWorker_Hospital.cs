using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002164 RID: 8548
	public class RoomRoleWorker_Hospital : RoomRoleWorker
	{
		// Token: 0x0600BABB RID: 47803 RVA: 0x0035E7E8 File Offset: 0x0035C9E8
		public override float GetScore(Room room)
		{
			bool flag = false;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					if (building_Bed.ForPrisoners)
					{
						return 0f;
					}
					if (building_Bed.Medical)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return 0f;
			}
			return 100000f;
		}

		// Token: 0x0600BABC RID: 47804 RVA: 0x0035E85C File Offset: 0x0035CA5C
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (room.Role != null && room.Role.Worker is RoomRoleWorker_Hospital)
			{
				return 0f;
			}
			if (buildingDef.building != null && buildingDef.building.bed_humanlike && buildingDef.building.bed_defaultMedical)
			{
				return 100000f;
			}
			return 0f;
		}
	}
}
