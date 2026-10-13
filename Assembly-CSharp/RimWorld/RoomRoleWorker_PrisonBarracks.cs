using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200216A RID: 8554
	public class RoomRoleWorker_PrisonBarracks : RoomRoleWorker
	{
		// Token: 0x0600BACC RID: 47820 RVA: 0x0035ECBC File Offset: 0x0035CEBC
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
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
			if (num2 + num <= 1)
			{
				return 0f;
			}
			return (float)num2 * 100100f + (float)num * 50001f;
		}

		// Token: 0x0600BACD RID: 47821 RVA: 0x0035ED4C File Offset: 0x0035CF4C
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.building == null || !buildingDef.thingClass.IsAssignableFrom(typeof(Building_Bed)) || !buildingDef.building.bed_humanlike)
			{
				return 0f;
			}
			if (room.Role != null)
			{
				RoomRoleWorker worker = room.Role.Worker;
				if (worker is RoomRoleWorker_PrisonCell || worker is RoomRoleWorker_PrisonBarracks)
				{
					if (!buildingDef.building.bed_defaultMedical)
					{
						return 100100f;
					}
					return 50001f;
				}
			}
			return 0f;
		}
	}
}
