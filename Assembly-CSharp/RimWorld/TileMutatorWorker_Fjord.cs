using System;
using RimWorld.Planet;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020E5 RID: 8421
	public class TileMutatorWorker_Fjord : TileMutatorWorker_Coast
	{
		// Token: 0x0600B7FF RID: 47103 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_Fjord(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B800 RID: 47104 RVA: 0x0034CD58 File Offset: 0x0034AF58
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x0600B801 RID: 47105 RVA: 0x003509F0 File Offset: 0x0034EBF0
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float coastAngle = this.GetCoastAngle(map.Tile);
			this.fjordNoise = new DistFromAxis((float)map.Size.x * 0.25f);
			this.fjordNoise = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, this.fjordNoise);
			this.fjordNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.fjordNoise);
			this.fjordNoise = new Max(this.fjordNoise, new Const((double)this.MaxForDeepWater));
			ModuleBase moduleBase = new DistFromCone(2f, (float)map.Size.x * 0.35f);
			moduleBase = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * -0.5f), moduleBase);
			moduleBase = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.fjordNoise = new SmoothMin(this.fjordNoise, moduleBase, 0.5);
			NoiseDebugUI.StoreNoiseRender(this.fjordNoise, "fjord shape");
			this.fjordNoise = MapNoiseUtility.AddDisplacementNoise(this.fjordNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.fjordNoise, "fjord");
			this.coastNoise = new SmoothMin(this.fjordNoise, this.coastNoise, 0.5);
		}

		// Token: 0x0600B802 RID: 47106 RVA: 0x00350BC0 File Offset: 0x0034EDC0
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.fjordNoise.GetValue(intVec);
				if (value < this.MaxForSand)
				{
					elevation[intVec] = 0f;
				}
				else
				{
					MapGenFloatGrid mapGenFloatGrid = elevation;
					IntVec3 c = intVec;
					mapGenFloatGrid[c] += value + -1f;
				}
			}
		}

		// Token: 0x04007EAA RID: 32426
		private const float FjordSpan = 0.25f;

		// Token: 0x04007EAB RID: 32427
		private const float ConeGradient = 2f;

		// Token: 0x04007EAC RID: 32428
		private const float ConeSpan = 0.35f;

		// Token: 0x04007EAD RID: 32429
		private const float ConeOffset = -0.5f;

		// Token: 0x04007EAE RID: 32430
		private const float ElevationOffset = -1f;

		// Token: 0x04007EAF RID: 32431
		private ModuleBase fjordNoise;
	}
}
