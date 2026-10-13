using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200215F RID: 8543
	public class RoomRoleWorker_CeremonialChamber : RoomRoleWorker
	{
		// Token: 0x0600BAAC RID: 47788 RVA: 0x0035E51C File Offset: 0x0035C71C
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def == ThingDefOf.PsychicRitualSpot)
				{
					num += 200f;
				}
			}
			return num;
		}

		// Token: 0x0600BAAD RID: 47789 RVA: 0x0035E563 File Offset: 0x0035C763
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef == ThingDefOf.PsychicRitualSpot)
			{
				return 200f;
			}
			return 0f;
		}
	}
}
