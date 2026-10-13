using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002160 RID: 8544
	public class RoomRoleWorker_Classroom : RoomRoleWorker
	{
		// Token: 0x0600BAAF RID: 47791 RVA: 0x0035E578 File Offset: 0x0035C778
		public override float GetScore(Room room)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num = 0;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.IsBed && thing.def.building.bed_humanlike)
				{
					return 0f;
				}
				if (thing.def == ThingDefOf.SchoolDesk || thing.def == ThingDefOf.Blackboard)
				{
					num++;
				}
			}
			return (float)num * 8f;
		}

		// Token: 0x0600BAB0 RID: 47792 RVA: 0x0035E5FF File Offset: 0x0035C7FF
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (buildingDef == ThingDefOf.SchoolDesk || buildingDef == ThingDefOf.Blackboard)
			{
				return 8f;
			}
			return 0f;
		}

		// Token: 0x040080B0 RID: 32944
		private const int MinBabyBeds = 2;
	}
}
