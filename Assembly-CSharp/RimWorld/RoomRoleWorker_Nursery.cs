using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002168 RID: 8552
	public class RoomRoleWorker_Nursery : RoomRoleWorker
	{
		// Token: 0x0600BAC6 RID: 47814 RVA: 0x0035EAA0 File Offset: 0x0035CCA0
		public override float GetScore(Room room)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike && !building_Bed.Medical)
				{
					if (building_Bed.ForPrisoners || building_Bed.def.building.bed_maxBodySize >= LifeStageDefOf.HumanlikeChild.bodySizeFactor)
					{
						return 0f;
					}
					num++;
				}
			}
			if (num < 2)
			{
				return 0f;
			}
			return (float)num * 100200f;
		}

		// Token: 0x0600BAC7 RID: 47815 RVA: 0x0035EB40 File Offset: 0x0035CD40
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (room.Role != null)
			{
				RoomRoleWorker worker = room.Role.Worker;
				if (worker is RoomRoleWorker_Bedroom || worker is RoomRoleWorker_Barracks || worker is RoomRoleWorker_Hospital || worker is RoomRoleWorker_PrisonCell || worker is RoomRoleWorker_PrisonBarracks)
				{
					return 0f;
				}
			}
			if (buildingDef.building == null || !buildingDef.building.bed_humanlike)
			{
				return 0f;
			}
			bool flag = room.Role != null && room.Role.Worker is RoomRoleWorker_Nursery;
			if (buildingDef.building.bed_maxBodySize >= LifeStageDefOf.HumanlikeChild.bodySizeFactor)
			{
				if (!flag)
				{
					return 0f;
				}
				return -100200f;
			}
			else
			{
				if (!flag)
				{
					return 100200f;
				}
				return 0f;
			}
		}

		// Token: 0x040080B1 RID: 32945
		private const int MinBabyBeds = 2;
	}
}
