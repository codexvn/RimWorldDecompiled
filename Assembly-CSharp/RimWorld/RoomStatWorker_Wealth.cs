using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002179 RID: 8569
	public class RoomStatWorker_Wealth : RoomStatWorker
	{
		// Token: 0x0600BAF7 RID: 47863 RVA: 0x0035F884 File Offset: 0x0035DA84
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Plant)
				{
					num += (float)thing.stackCount * thing.MarketValue;
				}
			}
			foreach (IntVec3 c in room.Cells)
			{
				num += c.GetTerrain(room.Map).GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			return num;
		}
	}
}
