using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200215D RID: 8541
	public class RoomRoleWorker_Bedroom : RoomRoleWorker
	{
		// Token: 0x0600BAA4 RID: 47780 RVA: 0x0035E17C File Offset: 0x0035C37C
		public override float GetScore(Room room)
		{
			RoomRoleWorker_Bedroom.tmpBeds.Clear();
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num = 0;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike && building_Bed.def.building.bed_countsForBedroomOrBarracks)
				{
					if (building_Bed.Medical || building_Bed.ForPrisoners)
					{
						RoomRoleWorker_Bedroom.tmpBeds.Clear();
						return 0f;
					}
					num++;
					RoomRoleWorker_Bedroom.tmpBeds.Add(building_Bed);
				}
			}
			if (num == 0)
			{
				RoomRoleWorker_Bedroom.tmpBeds.Clear();
				return 0f;
			}
			bool flag = RoomRoleWorker_Bedroom.IsBedroom(RoomRoleWorker_Bedroom.tmpBeds);
			RoomRoleWorker_Bedroom.tmpBeds.Clear();
			if (!flag)
			{
				return 0f;
			}
			return 100000f;
		}

		// Token: 0x0600BAA5 RID: 47781 RVA: 0x0035E244 File Offset: 0x0035C444
		public static bool IsBedroom(List<Building_Bed> beds)
		{
			RoomRoleWorker_Bedroom.children.Clear();
			RoomRoleWorker_Bedroom.adults.Clear();
			bool result = RoomRoleWorker_Bedroom.IsBedroomHelper(beds);
			RoomRoleWorker_Bedroom.children.Clear();
			RoomRoleWorker_Bedroom.adults.Clear();
			return result;
		}

		// Token: 0x0600BAA6 RID: 47782 RVA: 0x0035E274 File Offset: 0x0035C474
		private static bool IsBedroomHelper(List<Building_Bed> beds)
		{
			List<Pawn> list = null;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (Building_Bed building_Bed in beds)
			{
				List<Pawn> ownersForReading = building_Bed.OwnersForReading;
				if (ownersForReading.NullOrEmpty<Pawn>() && building_Bed.def.building.bed_emptyCountsForBarracks)
				{
					num++;
				}
				else
				{
					if (ownersForReading.Count > 0)
					{
						num2++;
					}
					bool flag = false;
					foreach (Pawn pawn in ownersForReading)
					{
						if (pawn.DevelopmentalStage.Juvenile())
						{
							RoomRoleWorker_Bedroom.children.Add(pawn);
						}
						else
						{
							RoomRoleWorker_Bedroom.adults.Add(pawn);
							if (list == null)
							{
								list = pawn.GetLoveCluster();
							}
							if (!list.Contains(pawn))
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						num3++;
					}
				}
			}
			if (num == 1 && num2 == 0)
			{
				return true;
			}
			if (num == 0 && num2 == 1)
			{
				return true;
			}
			if (num > 0)
			{
				return false;
			}
			if (RoomRoleWorker_Bedroom.adults.NullOrEmpty<Pawn>())
			{
				return true;
			}
			if (num3 > 0)
			{
				return false;
			}
			foreach (Pawn pawn2 in RoomRoleWorker_Bedroom.children)
			{
				Pawn mother = pawn2.GetMother();
				Pawn father = pawn2.GetFather();
				if (!RoomRoleWorker_Bedroom.adults.Any((Pawn adult) => adult == mother || adult == father))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BAA7 RID: 47783 RVA: 0x0035E438 File Offset: 0x0035C638
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.building == null || !buildingDef.thingClass.IsAssignableFrom(typeof(Building_Bed)) || !buildingDef.building.bed_humanlike || !buildingDef.building.bed_countsForBedroomOrBarracks)
			{
				return 0f;
			}
			if (room.Role != null)
			{
				RoomRoleWorker worker = room.Role.Worker;
				if (worker is RoomRoleWorker_Barracks || worker is RoomRoleWorker_Hospital || worker is RoomRoleWorker_PrisonCell || worker is RoomRoleWorker_PrisonBarracks)
				{
					return 0f;
				}
			}
			if (room.Role != null && room.Role.Worker is RoomRoleWorker_Bedroom)
			{
				return -100000f;
			}
			return 100000f;
		}

		// Token: 0x040080AB RID: 32939
		private static List<Building_Bed> tmpBeds = new List<Building_Bed>();

		// Token: 0x040080AC RID: 32940
		private static List<Pawn> children = new List<Pawn>();

		// Token: 0x040080AD RID: 32941
		private static List<Pawn> adults = new List<Pawn>();
	}
}
