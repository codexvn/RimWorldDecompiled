using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002169 RID: 8553
	public class RoomRoleWorker_Playroom : RoomRoleWorker
	{
		// Token: 0x0600BAC9 RID: 47817 RVA: 0x0035EC0C File Offset: 0x0035CE0C
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
				Thing thing = containedAndAdjacentThings[i];
				Building_Bed building_Bed = thing as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					return 0f;
				}
				if (thing.GetStatValue(StatDefOf.BabyPlayGainFactor, true, -1) > 1f)
				{
					num++;
				}
			}
			return (float)num * 8f;
		}

		// Token: 0x0600BACA RID: 47818 RVA: 0x0035EC8D File Offset: 0x0035CE8D
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (buildingDef.GetStatValueAbstract(StatDefOf.BabyPlayGainFactor, null) > 1f)
			{
				return 8f;
			}
			return 0f;
		}
	}
}
