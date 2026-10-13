using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020FA RID: 8442
	public class TileMutatorWorker_ToxicLake : TileMutatorWorker_Lake
	{
		// Token: 0x0600B852 RID: 47186 RVA: 0x0034E5E7 File Offset: 0x0034C7E7
		public TileMutatorWorker_ToxicLake(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B853 RID: 47187 RVA: 0x00352900 File Offset: 0x00350B00
		protected override void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			if (this.GenerateDeepWater && valAt > 0.75f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.ToxicWaterDeep);
				return;
			}
			if (valAt > 0.5f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.ToxicWaterShallow);
				return;
			}
			if (valAt > 0.45f && MapGenUtility.ShouldGenerateBeachSand(cell, map))
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.LakeshoreTerrainAt(cell, map));
			}
		}
	}
}
