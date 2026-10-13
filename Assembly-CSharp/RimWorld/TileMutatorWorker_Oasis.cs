using System;
using LudeonTK;
using Verse;

namespace RimWorld
{
	// Token: 0x02002109 RID: 8457
	public class TileMutatorWorker_Oasis : TileMutatorWorker_Lake
	{
		// Token: 0x17001C0B RID: 7179
		// (get) Token: 0x0600B88D RID: 47245 RVA: 0x00353C2C File Offset: 0x00351E2C
		protected override float LakeRadius
		{
			get
			{
				return TileMutatorWorker_Oasis.LakeRadiusInt;
			}
		}

		// Token: 0x17001C0C RID: 7180
		// (get) Token: 0x0600B88E RID: 47246 RVA: 0x00353C33 File Offset: 0x00351E33
		protected override float LakeFalloffDecay
		{
			get
			{
				return TileMutatorWorker_Oasis.LakeFalloffDecayInt;
			}
		}

		// Token: 0x0600B88F RID: 47247 RVA: 0x0034E5E7 File Offset: 0x0034C7E7
		public TileMutatorWorker_Oasis(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B890 RID: 47248 RVA: 0x00353C3A File Offset: 0x00351E3A
		protected override IntVec3 GetLakeCenter(Map map)
		{
			return CellFinder.RandomNotEdgeCell(map.Size.x / 2, map);
		}

		// Token: 0x0600B891 RID: 47249 RVA: 0x00353C50 File Offset: 0x00351E50
		protected override void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(cell);
			if (valAt > 0.75f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.DeepFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.57f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.ShallowFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.45f && !terrainDef.IsWater)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.SoilRich);
				return;
			}
			if (valAt > 0.3f && !terrainDef.IsWater)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.Soil);
			}
		}

		// Token: 0x04007F36 RID: 32566
		private new const float DeepWaterThreshold = 0.75f;

		// Token: 0x04007F37 RID: 32567
		private new const float WaterThreshold = 0.57f;

		// Token: 0x04007F38 RID: 32568
		private const float RichSoilThreshold = 0.45f;

		// Token: 0x04007F39 RID: 32569
		private const float SoilThreshold = 0.3f;

		// Token: 0x04007F3A RID: 32570
		[TweakValue("Oasis", 0f, 1f)]
		private static float LakeRadiusInt = 0.4f;

		// Token: 0x04007F3B RID: 32571
		[TweakValue("Oasis", 0f, 5f)]
		private static float LakeFalloffDecayInt = 2f;
	}
}
