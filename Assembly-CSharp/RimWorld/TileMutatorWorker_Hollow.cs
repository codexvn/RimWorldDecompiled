using System;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020EA RID: 8426
	public class TileMutatorWorker_Hollow : TileMutatorWorker
	{
		// Token: 0x0600B812 RID: 47122 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Hollow(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B813 RID: 47123 RVA: 0x00351424 File Offset: 0x0034F624
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			float num3 = num2 ?? ((float)Rand.Range(0, 360));
			this.hollowNoise = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * TileMutatorWorker_Hollow.HollowRadius, Vector2.zero, 1f, false);
			this.hollowNoise = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * TileMutatorWorker_Hollow.HollowOffset), this.hollowNoise);
			this.hollowNoise = new Rotate(0.0, (double)(num3 + 90f), 0.0, this.hollowNoise);
			this.hollowNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.hollowNoise);
			ModuleBase moduleBase = new DistFromCone(TileMutatorWorker_Hollow.ConeGradient, (float)map.Size.x * TileMutatorWorker_Hollow.ConeSpan);
			moduleBase = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * TileMutatorWorker_Hollow.ConeOffset), moduleBase);
			moduleBase = new Rotate(0.0, (double)(num3 + 90f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.hollowNoise = new SmoothMin(this.hollowNoise, moduleBase, (double)TileMutatorWorker_Hollow.Smoothing);
			moduleBase = new DistFromCone(TileMutatorWorker_Hollow.InnerConeGradient, (float)map.Size.x * TileMutatorWorker_Hollow.ConeSpan);
			moduleBase = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * TileMutatorWorker_Hollow.InnerConeOffset), moduleBase);
			moduleBase = new Rotate(0.0, (double)(num3 + 90f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.hollowNoise = new SmoothMin(this.hollowNoise, moduleBase, (double)TileMutatorWorker_Hollow.Smoothing);
			if (num2 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num2.Value, TileMutatorWorker_Hollow.CoastFalloff, map);
				this.hollowNoise = new SmoothMin(this.hollowNoise, rhs, 0.1);
			}
			NoiseDebugUI.StoreNoiseRender(this.hollowNoise, "Hollow base shape");
			this.hollowNoise = MapNoiseUtility.AddDisplacementNoise(this.hollowNoise, TileMutatorWorker_Hollow.HollowNoiseFreq, TileMutatorWorker_Hollow.HollowNoiseStrength, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.hollowNoise, "Hollow noise", map.Size.ToIntVec2);
		}

		// Token: 0x0600B814 RID: 47124 RVA: 0x00351738 File Offset: 0x0034F938
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.hollowNoise.GetValue(intVec);
				if (value > TileMutatorWorker_Hollow.HollowThreshold)
				{
					elevation[intVec] = value;
				}
			}
		}

		// Token: 0x04007EBD RID: 32445
		private ModuleBase hollowNoise;

		// Token: 0x04007EBE RID: 32446
		[TweakValue("Hollow", 0f, 1f)]
		private static float HollowRadius = 0.25f;

		// Token: 0x04007EBF RID: 32447
		[TweakValue("Hollow", 0f, 0.1f)]
		private static float HollowNoiseFreq = 0.015f;

		// Token: 0x04007EC0 RID: 32448
		[TweakValue("Hollow", 0f, 50f)]
		private static float HollowNoiseStrength = 40f;

		// Token: 0x04007EC1 RID: 32449
		[TweakValue("Hollow", -1f, 1f)]
		private static float HollowOffset = -0.175f;

		// Token: 0x04007EC2 RID: 32450
		[TweakValue("Hollow", -1f, 1f)]
		private static float ConeOffset = 0.025f;

		// Token: 0x04007EC3 RID: 32451
		[TweakValue("Hollow", -1f, 1f)]
		private static float InnerConeOffset = -0.375f;

		// Token: 0x04007EC4 RID: 32452
		[TweakValue("Hollow", 0f, 1f)]
		private static float HollowThreshold = 0.5f;

		// Token: 0x04007EC5 RID: 32453
		[TweakValue("Hollow", 0f, 5f)]
		private static float ConeGradient = 0.5f;

		// Token: 0x04007EC6 RID: 32454
		[TweakValue("Hollow", 0f, 5f)]
		private static float InnerConeGradient = 3f;

		// Token: 0x04007EC7 RID: 32455
		[TweakValue("Hollow", 0f, 1f)]
		private static float ConeSpan = 0.25f;

		// Token: 0x04007EC8 RID: 32456
		[TweakValue("Hollow", 0f, 1f)]
		private static float Smoothing = 0.5f;

		// Token: 0x04007EC9 RID: 32457
		[TweakValue("Hollow", 0f, 1f)]
		private static float CoastFalloff = 0.1f;
	}
}
