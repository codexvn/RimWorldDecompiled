using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020E3 RID: 8419
	public class TileMutatorWorker_Crevasse : TileMutatorWorker
	{
		// Token: 0x0600B7F8 RID: 47096 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Crevasse(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7F9 RID: 47097 RVA: 0x00350628 File Offset: 0x0034E828
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float num = Rand.Range(0f, 180f);
			this.crevasse = new DistFromAxis((float)map.Size.x * 0.15f);
			this.crevasse = new Rotate(0.0, (double)num, 0.0, this.crevasse);
			this.crevasse = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), this.crevasse);
			this.crevasse = MapNoiseUtility.AddDisplacementNoise(this.crevasse, 0.015f, 20f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.crevasse, "crevasse");
		}

		// Token: 0x0600B7FA RID: 47098 RVA: 0x00350708 File Offset: 0x0034E908
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.crevasse.GetValue(intVec) <= 0.5f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x0600B7FB RID: 47099 RVA: 0x00350774 File Offset: 0x0034E974
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.crevasse.GetValue(intVec);
				if (value > 0.5f)
				{
					if (value > 0.798f)
					{
						map.roofGrid.SetRoof(intVec, RoofDefOf.RoofRockThick);
					}
					else if (value > 0.728f)
					{
						map.roofGrid.SetRoof(intVec, RoofDefOf.RoofRockThin);
					}
					if (intVec.GetEdifice(map) == null)
					{
						GenSpawn.Spawn(ThingDefOf.SolidIce, intVec, map, WipeMode.Vanish);
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Ice);
					}
				}
			}
		}

		// Token: 0x04007E99 RID: 32409
		private ModuleBase crevasse;

		// Token: 0x04007E9A RID: 32410
		private const float CrevasseSpan = 0.15f;

		// Token: 0x04007E9B RID: 32411
		private const float CrevasseThreshold = 0.5f;

		// Token: 0x04007E9C RID: 32412
		private const float CrevasseNoiseFreq = 0.015f;

		// Token: 0x04007E9D RID: 32413
		private const float CrevasseNoiseStrength = 20f;
	}
}
