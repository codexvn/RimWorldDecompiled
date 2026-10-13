using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CCA RID: 15562
	public static class GenWorldClosest
	{
		// Token: 0x0601527D RID: 86653 RVA: 0x00654FA4 File Offset: 0x006531A4
		public static bool TryFindClosestTile(PlanetTile rootTile, Predicate<PlanetTile> predicate, out PlanetTile foundTile, int maxTilesToScan = 2147483647, bool canSearchThroughImpassable = true)
		{
			PlanetTile foundTileLocal = PlanetTile.Invalid;
			rootTile.Layer.Filler.FloodFill(rootTile, (PlanetTile x) => canSearchThroughImpassable || !Find.World.Impassable(x), delegate(PlanetTile t)
			{
				bool flag = predicate(t);
				if (flag)
				{
					foundTileLocal = t;
				}
				return flag;
			}, maxTilesToScan, null);
			foundTile = foundTileLocal;
			return foundTileLocal.Valid;
		}

		// Token: 0x0601527E RID: 86654 RVA: 0x00655014 File Offset: 0x00653214
		public static bool TryFindClosestPassableTile(PlanetTile rootTile, out PlanetTile foundTile)
		{
			return GenWorldClosest.TryFindClosestTile(rootTile, (PlanetTile x) => !Find.World.Impassable(x), out foundTile, int.MaxValue, true);
		}
	}
}
