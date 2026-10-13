using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002125 RID: 8485
	public class TileMutatorWorker_Wetland : TileMutatorWorker
	{
		// Token: 0x0600B90B RID: 47371 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Wetland(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B90C RID: 47372 RVA: 0x00356CB8 File Offset: 0x00354EB8
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.wetlandNoise = new RidgedMultifractal(0.029999999329447746, 2.0, 2, Rand.Int, QualityMode.High);
			this.wetlandNoise = new Clamp(0.0, 1.0, this.wetlandNoise);
			NoiseDebugUI.StoreNoiseRender(this.wetlandNoise, "wetland");
		}

		// Token: 0x0600B90D RID: 47373 RVA: 0x00356D24 File Offset: 0x00354F24
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (intVec.GetEdifice(map) == null)
				{
					TerrainDef terrain = intVec.GetTerrain(map);
					if (!terrain.IsWater)
					{
						float value = this.wetlandNoise.GetValue(intVec);
						if (value > 0.35f)
						{
							if (terrain.IsRock)
							{
								map.terrainGrid.SetTerrain(intVec, MapGenUtility.MudTerrainAt(intVec, map));
							}
							else
							{
								map.terrainGrid.SetTerrain(intVec, MapGenUtility.ShallowFreshWaterTerrainAt(intVec, map));
							}
						}
						else if (value > 0.015f && MapGenUtility.ShouldGenerateBeachSand(intVec, map))
						{
							map.terrainGrid.SetTerrain(intVec, MapGenUtility.MudTerrainAt(intVec, map));
						}
					}
				}
			}
		}

		// Token: 0x04007FA9 RID: 32681
		private const float RidgedFreq = 0.03f;

		// Token: 0x04007FAA RID: 32682
		private const float RidgedLac = 2f;

		// Token: 0x04007FAB RID: 32683
		private const int RidgedOctaves = 2;

		// Token: 0x04007FAC RID: 32684
		private const float MudThreshold = 0.015f;

		// Token: 0x04007FAD RID: 32685
		private const float WaterThreshold = 0.35f;

		// Token: 0x04007FAE RID: 32686
		private ModuleBase wetlandNoise;
	}
}
