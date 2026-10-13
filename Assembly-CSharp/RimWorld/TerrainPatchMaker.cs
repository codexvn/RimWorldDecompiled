using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200200F RID: 8207
	public class TerrainPatchMaker
	{
		// Token: 0x0600B4BA RID: 46266 RVA: 0x0033DAF0 File Offset: 0x0033BCF0
		private void Init(Map map)
		{
			this.noise = new Perlin((double)this.perlinFrequency, (double)this.perlinLacunarity, (double)this.perlinPersistence, this.perlinOctaves, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			NoiseDebugUI.RenderSize = new IntVec2(map.Size.x, map.Size.z);
			NoiseDebugUI.StoreNoiseRender(this.noise, "TerrainPatchMaker " + this.thresholds[0].terrain.defName);
			this.currentlyInitializedForMap = map;
		}

		// Token: 0x0600B4BB RID: 46267 RVA: 0x0033DB81 File Offset: 0x0033BD81
		public void Cleanup()
		{
			this.noise = null;
			this.currentlyInitializedForMap = null;
		}

		// Token: 0x0600B4BC RID: 46268 RVA: 0x0033DB94 File Offset: 0x0033BD94
		public TerrainDef TerrainAt(IntVec3 c, Map map, float fertility)
		{
			if (fertility < this.minFertility || fertility > this.maxFertility)
			{
				return null;
			}
			if (this.noise != null && map != this.currentlyInitializedForMap)
			{
				this.Cleanup();
			}
			if (this.noise == null)
			{
				this.Init(map);
			}
			if (this.minSize > 0)
			{
				int count = 0;
				map.floodFiller.FloodFill(c, (IntVec3 x) => TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(x)) != null, delegate(IntVec3 x)
				{
					int count = count;
					count++;
					return count >= this.minSize;
				}, int.MaxValue, false, null);
				if (count < this.minSize)
				{
					return null;
				}
			}
			return TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(c));
		}

		// Token: 0x04007BE9 RID: 31721
		private Map currentlyInitializedForMap;

		// Token: 0x04007BEA RID: 31722
		public List<TerrainThreshold> thresholds = new List<TerrainThreshold>();

		// Token: 0x04007BEB RID: 31723
		public float perlinFrequency = 0.01f;

		// Token: 0x04007BEC RID: 31724
		public float perlinLacunarity = 2f;

		// Token: 0x04007BED RID: 31725
		public float perlinPersistence = 0.5f;

		// Token: 0x04007BEE RID: 31726
		public int perlinOctaves = 6;

		// Token: 0x04007BEF RID: 31727
		public float minFertility = -999f;

		// Token: 0x04007BF0 RID: 31728
		public float maxFertility = 999f;

		// Token: 0x04007BF1 RID: 31729
		public int minSize;

		// Token: 0x04007BF2 RID: 31730
		public bool isPond;

		// Token: 0x04007BF3 RID: 31731
		[Unsaved(false)]
		private ModuleBase noise;
	}
}
