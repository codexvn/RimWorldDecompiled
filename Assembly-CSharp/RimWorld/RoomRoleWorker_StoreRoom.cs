using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200216E RID: 8558
	public class RoomRoleWorker_StoreRoom : RoomRoleWorker
	{
		// Token: 0x0600BAD7 RID: 47831 RVA: 0x0035EFE0 File Offset: 0x0035D1E0
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Storage)
				{
					num++;
				}
			}
			return 1f * (float)num;
		}

		// Token: 0x0600BAD8 RID: 47832 RVA: 0x0035F021 File Offset: 0x0035D221
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.thingClass.IsAssignableFrom(typeof(Building_Storage)))
			{
				return 1f;
			}
			return 0f;
		}
	}
}
