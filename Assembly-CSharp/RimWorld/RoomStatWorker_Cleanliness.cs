using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002174 RID: 8564
	public class RoomStatWorker_Cleanliness : RoomStatWorker
	{
		// Token: 0x0600BAEB RID: 47851 RVA: 0x0035F4B0 File Offset: 0x0035D6B0
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Filth || thing.def.category == ThingCategory.Plant)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Cleanliness, true, -1);
				}
			}
			foreach (IntVec3 c in room.Cells)
			{
				num += c.GetTerrain(room.Map).GetStatValueAbstract(StatDefOf.Cleanliness, null);
			}
			return num / (float)room.CellCount;
		}
	}
}
