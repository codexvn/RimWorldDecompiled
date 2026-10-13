using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200216C RID: 8556
	public class RoomRoleWorker_RecRoom : RoomRoleWorker
	{
		// Token: 0x0600BAD2 RID: 47826 RVA: 0x0035EEC0 File Offset: 0x0035D0C0
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building)
				{
					List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].countsForRecRoom && allDefsListForReading[j].thingDefs != null && allDefsListForReading[j].thingDefs.Contains(thing.def))
						{
							num++;
							break;
						}
					}
				}
			}
			return (float)num * 7f;
		}

		// Token: 0x0600BAD3 RID: 47827 RVA: 0x0035EF68 File Offset: 0x0035D168
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.category != ThingCategory.Building)
			{
				return 0f;
			}
			List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].countsForRecRoom && allDefsListForReading[i].thingDefs != null && allDefsListForReading[i].thingDefs.Contains(buildingDef))
				{
					return 7f;
				}
			}
			return 0f;
		}
	}
}
