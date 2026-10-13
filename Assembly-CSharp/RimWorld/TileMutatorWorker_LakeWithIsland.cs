using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020F7 RID: 8439
	public class TileMutatorWorker_LakeWithIsland : TileMutatorWorker_Lake
	{
		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x0600B846 RID: 47174 RVA: 0x003526B9 File Offset: 0x003508B9
		protected override float LakeRadius
		{
			get
			{
				return 0.8f;
			}
		}

		// Token: 0x17001BFF RID: 7167
		// (get) Token: 0x0600B847 RID: 47175 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool GenerateDeepWater
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B848 RID: 47176 RVA: 0x0034E5E7 File Offset: 0x0034C7E7
		public TileMutatorWorker_LakeWithIsland(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B849 RID: 47177 RVA: 0x003526C0 File Offset: 0x003508C0
		public override void Init(Map map)
		{
			base.Init(map);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.4f);
			moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.waterInfo.lakeCenter.x), 0.0, (double)(-(double)map.waterInfo.lakeCenter.z), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.006f, 40f, 2, map.Tile.tileId);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 15f, 4, map.Tile.tileId);
			this.lakeNoise = new Min(this.lakeNoise, moduleBase);
		}

		// Token: 0x04007EFB RID: 32507
		private const float LakeIslandNoiseFrequency = 0.015f;

		// Token: 0x04007EFC RID: 32508
		private const float LakeIslandNoiseStrength = 15f;

		// Token: 0x04007EFD RID: 32509
		private const float LakeIslandMacroNoiseFrequency = 0.006f;

		// Token: 0x04007EFE RID: 32510
		private const float LakeIslandMacroNoiseStrength = 40f;

		// Token: 0x04007EFF RID: 32511
		private const float IslandRadius = 0.4f;
	}
}
