using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020F9 RID: 8441
	public class TileMutatorWorker_DryLake : TileMutatorWorker_Lake
	{
		// Token: 0x17001C02 RID: 7170
		// (get) Token: 0x0600B84E RID: 47182 RVA: 0x0034FBFB File Offset: 0x0034DDFB
		protected override float LakeRadius
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x17001C03 RID: 7171
		// (get) Token: 0x0600B84F RID: 47183 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool GenerateDeepWater
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B850 RID: 47184 RVA: 0x0034E5E7 File Offset: 0x0034C7E7
		public TileMutatorWorker_DryLake(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B851 RID: 47185 RVA: 0x003528B4 File Offset: 0x00350AB4
		protected override void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			if (valAt > 0.5f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.DryLakeBed);
				return;
			}
			if (valAt > 0.45f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.Sand);
			}
		}
	}
}
