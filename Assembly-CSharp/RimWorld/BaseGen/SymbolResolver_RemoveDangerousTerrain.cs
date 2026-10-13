using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003933 RID: 14643
	public class SymbolResolver_RemoveDangerousTerrain : SymbolResolver
	{
		// Token: 0x060142AD RID: 82605 RVA: 0x00602350 File Offset: 0x00600550
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 c in rp.rect)
			{
				if (terrainGrid.TerrainAt(c).dangerous)
				{
					terrainGrid.SetTerrain(c, map.BiomeAt(c).TerrainForAffordance(TerrainAffordanceDefOf.Heavy));
				}
			}
		}
	}
}
