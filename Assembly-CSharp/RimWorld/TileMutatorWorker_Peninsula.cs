using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200210D RID: 8461
	public class TileMutatorWorker_Peninsula : TileMutatorWorker_Coast
	{
		// Token: 0x0600B89B RID: 47259 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_Peninsula(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B89C RID: 47260 RVA: 0x00353F08 File Offset: 0x00352108
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			float coastAngle = this.GetCoastAngle(map.Tile);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.6f);
			moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
			moduleBase = new Multiply(moduleBase, new CutOff(true, false));
			ModuleBase moduleBase2 = new DistFromAxis((float)map.Size.x * 0.6f);
			moduleBase2 = new Rotate(0.0, 90.0, 0.0, moduleBase2);
			moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
			moduleBase2 = new Multiply(moduleBase2, new CutOff());
			this.coastNoise = new Add(moduleBase, moduleBase2);
			this.coastNoise = new Translate((double)((float)map.Size.x * 0.1f), 0.0, 0.0, this.coastNoise);
			this.coastNoise = new Rotate(0.0, (double)coastAngle, 0.0, this.coastNoise);
			this.coastNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.coastNoise);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "Peninsula shape", map.Size.ToIntVec2);
			this.coastNoise = MapNoiseUtility.AddDisplacementNoise(this.coastNoise, 0.006f, 30f, 2, -1);
			this.coastNoise = MapNoiseUtility.AddDisplacementNoise(this.coastNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "Peninsula", map.Size.ToIntVec2);
		}

		// Token: 0x04007F40 RID: 32576
		private const float PeninsulaRadius = 0.6f;

		// Token: 0x04007F41 RID: 32577
		private const float PeninsulaOffset = 0.1f;
	}
}
