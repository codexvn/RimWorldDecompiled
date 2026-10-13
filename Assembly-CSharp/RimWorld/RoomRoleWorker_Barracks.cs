using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200215C RID: 8540
	public class RoomRoleWorker_Barracks : RoomRoleWorker
	{
		// Token: 0x0600BAA0 RID: 47776 RVA: 0x0035E034 File Offset: 0x0035C234
		public override float GetScore(Room room)
		{
			RoomRoleWorker_Barracks.tmpBeds.Clear();
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike && building_Bed.def.building.bed_countsForBedroomOrBarracks)
				{
					if (building_Bed.ForPrisoners)
					{
						RoomRoleWorker_Barracks.tmpBeds.Clear();
						return 0f;
					}
					RoomRoleWorker_Barracks.tmpBeds.Add(building_Bed);
					if (!building_Bed.Medical)
					{
						num++;
					}
				}
			}
			bool flag = RoomRoleWorker_Bedroom.IsBedroom(RoomRoleWorker_Barracks.tmpBeds);
			RoomRoleWorker_Barracks.tmpBeds.Clear();
			if (flag)
			{
				return 0f;
			}
			return (float)num * 100100f;
		}

		// Token: 0x0600BAA1 RID: 47777 RVA: 0x0035E0EC File Offset: 0x0035C2EC
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.building == null || !buildingDef.thingClass.IsAssignableFrom(typeof(Building_Bed)) || !buildingDef.building.bed_humanlike || !buildingDef.building.bed_countsForBedroomOrBarracks)
			{
				return 0f;
			}
			if (room.Role != null)
			{
				RoomRoleWorker worker = room.Role.Worker;
				if (!(worker is RoomRoleWorker_Bedroom) && !(worker is RoomRoleWorker_Barracks) && !(worker is RoomRoleWorker_Hospital))
				{
					return 0f;
				}
			}
			return 100100f;
		}

		// Token: 0x040080AA RID: 32938
		private static List<Building_Bed> tmpBeds = new List<Building_Bed>();
	}
}
