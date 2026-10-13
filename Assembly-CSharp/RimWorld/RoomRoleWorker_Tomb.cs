using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002170 RID: 8560
	public class RoomRoleWorker_Tomb : RoomRoleWorker
	{
		// Token: 0x0600BADE RID: 47838 RVA: 0x0035F0E8 File Offset: 0x0035D2E8
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Sarcophagus)
				{
					num++;
				}
			}
			return 50f * (float)num;
		}

		// Token: 0x0600BADF RID: 47839 RVA: 0x0035F129 File Offset: 0x0035D329
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (!buildingDef.thingClass.IsAssignableFrom(typeof(Building_Sarcophagus)))
			{
				return 0f;
			}
			return 50f;
		}
	}
}
