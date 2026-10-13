using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200205A RID: 8282
	public class GenStep_FrozenTerraformer : GenStep_BaseRuins
	{
		// Token: 0x17001BCC RID: 7116
		// (get) Token: 0x0600B5E9 RID: 46569 RVA: 0x00344256 File Offset: 0x00342456
		public override int SeedPart
		{
			get
			{
				return 9871234;
			}
		}

		// Token: 0x17001BCD RID: 7117
		// (get) Token: 0x0600B5EA RID: 46570 RVA: 0x0034425D File Offset: 0x0034245D
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientRuinsFrozenTerraformer;
			}
		}

		// Token: 0x17001BCE RID: 7118
		// (get) Token: 0x0600B5EB RID: 46571 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override Faction Faction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600B5EC RID: 46572 RVA: 0x00344264 File Offset: 0x00342464
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Frozen Terraformer"))
			{
				return;
			}
			this.iceNoise = new Perlin(0.009999999776482582, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.3f * 2f);
			moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.iceNoise = new Blend(this.iceNoise, moduleBase, new Const(0.699999988079071));
			MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (intVec.GetEdifice(map) == null)
				{
					float value = this.iceNoise.GetValue(intVec);
					if (value > 0.4f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Ice);
					}
					if (value > 0.5f)
					{
						GenSpawn.Spawn(ThingDefOf.SolidIce, intVec, map, WipeMode.Vanish);
					}
				}
			}
			CellRect cellRect = CellRect.CenteredOn(map.Center, 20);
			this.GenerateAndSpawn(cellRect, map, parms, this.LayoutDef);
			MapGenerator.SetVar<CellRect>("SpawnRect", cellRect);
		}

		// Token: 0x0600B5ED RID: 46573 RVA: 0x003443DC File Offset: 0x003425DC
		public override void PostMapInitialized(Map map, GenStepParams parms)
		{
			BaseGenUtility.ScatterSentryDronesInMap(GenStep_FrozenTerraformer.SentryCountFromPointsCurve, map, Faction.OfAncientsHostile, parms);
		}

		// Token: 0x04007CC2 RID: 31938
		private ModuleBase iceNoise;

		// Token: 0x04007CC3 RID: 31939
		private const float NoiseFrequency = 0.01f;

		// Token: 0x04007CC4 RID: 31940
		private const float FalloffStrength = 0.7f;

		// Token: 0x04007CC5 RID: 31941
		private const float IceRadius = 0.3f;

		// Token: 0x04007CC6 RID: 31942
		private const float IceThreshold = 0.5f;

		// Token: 0x04007CC7 RID: 31943
		private const float IceTerrainThreshold = 0.4f;

		// Token: 0x04007CC8 RID: 31944
		private const int BuildingSize = 40;

		// Token: 0x04007CC9 RID: 31945
		private static readonly SimpleCurve SentryCountFromPointsCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(100f, 2f),
			new CurvePoint(1000f, 8f),
			new CurvePoint(5000f, 16f)
		});
	}
}
