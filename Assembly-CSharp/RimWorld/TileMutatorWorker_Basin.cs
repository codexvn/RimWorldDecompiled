using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020D3 RID: 8403
	public class TileMutatorWorker_Basin : TileMutatorWorker_Lake
	{
		// Token: 0x17001BE9 RID: 7145
		// (get) Token: 0x0600B7AC RID: 47020 RVA: 0x0034E5D8 File Offset: 0x0034C7D8
		protected override float LakeRadius
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x0600B7AD RID: 47021 RVA: 0x0034E5DF File Offset: 0x0034C7DF
		protected override IntVec3 GetLakeCenter(Map map)
		{
			return map.Center;
		}

		// Token: 0x0600B7AE RID: 47022 RVA: 0x0034E5E7 File Offset: 0x0034C7E7
		public TileMutatorWorker_Basin(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7AF RID: 47023 RVA: 0x0034E5F0 File Offset: 0x0034C7F0
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			Rot4 rot = Rot4.Random;
			if (map.TileInfo.IsCoastal)
			{
				rot = Find.World.CoastDirectionAt(map.Tile).Opposite;
			}
			float angle = rot.AsAngle + (float)Rand.Range(-30, 30);
			this.basinNoise = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * 0.35f, map.Center.ToVector2(), 1f, false);
			List<float> list = new List<float>();
			list.Add(angle);
			ModuleBase moduleBase = new DistFromCone(5f, (float)map.Size.x * 0.05f);
			moduleBase = new Rotate(0.0, (double)(360f - angle), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.basinNoise = new SmoothMin(this.basinNoise, moduleBase, 0.2);
			int randomInRange = TileMutatorWorker_Basin.AdditionalEntranceCountRange.RandomInRange;
			Predicate<float> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				int num = 100;
				for (;;)
				{
					List<float> list2 = list;
					Predicate<float> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((float ua) => Mathf.Abs(Mathf.DeltaAngle(ua, angle)) < 70f));
					}
					if (!list2.Any(predicate) || num-- <= 0)
					{
						break;
					}
					angle = Rand.Range(0f, 360f);
				}
				list.Add(angle);
				moduleBase = new DistFromCone(5f, (float)map.Size.x * 0.02f);
				moduleBase = new Rotate(0.0, (double)angle, 0.0, moduleBase);
				moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
				this.basinNoise = new SmoothMin(this.basinNoise, moduleBase, 0.2);
			}
			NoiseDebugUI.StoreNoiseRender(this.basinNoise, "Basin base shape");
			float? num2 = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num3 = (num2 != null) ? num2 : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			if (num3 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num3.Value, 0.1f, map);
				this.basinNoise = new SmoothMin(this.basinNoise, rhs, 0.1);
			}
			this.basinNoise = MapNoiseUtility.AddDisplacementNoise(this.basinNoise, 0.015f, 35f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.basinNoise, "Basin");
		}

		// Token: 0x0600B7B0 RID: 47024 RVA: 0x0034E8D8 File Offset: 0x0034CAD8
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.basinNoise.GetValue(intVec);
				if (value > 0.5f)
				{
					elevation[intVec] = value;
				}
			}
			base.GeneratePostElevationFertility(map);
		}

		// Token: 0x04007E29 RID: 32297
		private ModuleBase basinNoise;

		// Token: 0x04007E2A RID: 32298
		private const float BasinRadius = 0.35f;

		// Token: 0x04007E2B RID: 32299
		private const float EntranceGradient = 5f;

		// Token: 0x04007E2C RID: 32300
		private const float EntranceSpan = 0.05f;

		// Token: 0x04007E2D RID: 32301
		private const float AdditionalEntranceSpan = 0.02f;

		// Token: 0x04007E2E RID: 32302
		private const float BasinNoiseFreq = 0.015f;

		// Token: 0x04007E2F RID: 32303
		private const float BasinNoiseStrength = 35f;

		// Token: 0x04007E30 RID: 32304
		private const float BasinThreshold = 0.5f;

		// Token: 0x04007E31 RID: 32305
		private const float MinAngleBetweenEntrances = 70f;

		// Token: 0x04007E32 RID: 32306
		private const float CoastFalloff = 0.1f;

		// Token: 0x04007E33 RID: 32307
		private static readonly IntRange AdditionalEntranceCountRange = new IntRange(1, 2);
	}
}
