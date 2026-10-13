using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020E2 RID: 8418
	public class TileMutatorWorker_Cove : TileMutatorWorker_Coast
	{
		// Token: 0x0600B7F5 RID: 47093 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_Cove(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7F6 RID: 47094 RVA: 0x00350350 File Offset: 0x0034E550
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float coastAngle = this.GetCoastAngle(map.Tile);
			this.coveNoise = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * 0.5f, Vector2.zero, 1f, false);
			this.coveNoise = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * -0.2f), this.coveNoise);
			this.coveNoise = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, this.coveNoise);
			this.coveNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.coveNoise);
			float num = Rand.Range(-0.1f, 0.1f);
			ModuleBase moduleBase = new DistFromCone(5f, (float)map.Size.x * 0.1f);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) * num), 0.0, 0.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			moduleBase = new ScaleBias(0.25, 0.0, moduleBase);
			this.coveNoise = new SmoothMin(this.coveNoise, moduleBase, 0.2);
			NoiseDebugUI.StoreNoiseRender(this.coveNoise, "cove shape");
			this.coveNoise = MapNoiseUtility.AddDisplacementNoise(this.coveNoise, 0.003f, 30f, 2, -1);
			this.coveNoise = MapNoiseUtility.AddDisplacementNoise(this.coveNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.coveNoise, "cove noise");
			this.coveNoise = new Max(this.coveNoise, new Const((double)this.MaxForDeepWater));
			this.coastNoise = new SmoothMin(this.coveNoise, this.coastNoise, 0.5);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "cove & coast");
		}

		// Token: 0x0600B7F7 RID: 47095 RVA: 0x003505B4 File Offset: 0x0034E7B4
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.coveNoise.GetValue(intVec) < 0.4f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x04007E8E RID: 32398
		private ModuleBase coveNoise;

		// Token: 0x04007E8F RID: 32399
		private const float CoveRadius = 0.5f;

		// Token: 0x04007E90 RID: 32400
		private const float CoveOffset = -0.2f;

		// Token: 0x04007E91 RID: 32401
		private const float CoveMacroNoiseFrequency = 0.003f;

		// Token: 0x04007E92 RID: 32402
		private const float CoveMacroNoiseStrength = 30f;

		// Token: 0x04007E93 RID: 32403
		private const float EntranceGradient = 5f;

		// Token: 0x04007E94 RID: 32404
		private const float EntranceSpan = 0.1f;

		// Token: 0x04007E95 RID: 32405
		private const float EntranceScale = 0.25f;

		// Token: 0x04007E96 RID: 32406
		private const float EntranceBias = 0f;

		// Token: 0x04007E97 RID: 32407
		private const float EntranceXOffset = 0.1f;

		// Token: 0x04007E98 RID: 32408
		private const float ElevationThreshold = 0.4f;
	}
}
