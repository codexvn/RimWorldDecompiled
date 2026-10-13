using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002039 RID: 8249
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		// Token: 0x0600B547 RID: 46407 RVA: 0x00341DF4 File Offset: 0x0033FFF4
		public override void Place(Map map, IntVec3 tile, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			while (tile.Impassable(map))
			{
				foreach (Thing thing in tile.GetThingList(map))
				{
					if (thing.def.passability == Traversability.Impassable)
					{
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
				}
			}
		}
	}
}
