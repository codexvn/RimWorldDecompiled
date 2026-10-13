using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AB3 RID: 10931
	public static class MonumentMarkerUtility
	{
		// Token: 0x0600F396 RID: 62358 RVA: 0x0046C6E0 File Offset: 0x0046A8E0
		public static Building GetFirstAdjacentBuilding(SketchEntity entity, IntVec3 offset, List<Thing> monumentThings, Map map)
		{
			if (entity.IsSameSpawnedOrBlueprintOrFrame(entity.pos + offset, map))
			{
				return null;
			}
			foreach (IntVec3 c in entity.OccupiedRect.MovedBy(offset).ExpandedBy(1).EdgeCells)
			{
				if (c.InBounds(map))
				{
					Building firstBuilding = c.GetFirstBuilding(map);
					if (firstBuilding != null && !monumentThings.Contains(firstBuilding) && (firstBuilding.Faction == null || firstBuilding.Faction == Faction.OfPlayer))
					{
						return firstBuilding;
					}
				}
			}
			return null;
		}
	}
}
