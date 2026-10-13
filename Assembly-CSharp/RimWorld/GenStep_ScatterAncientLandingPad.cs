using System;
using RimWorld.BaseGen;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF2 RID: 8178
	public class GenStep_ScatterAncientLandingPad : GenStep_Scatterer
	{
		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x0600B42E RID: 46126 RVA: 0x0033AD40 File Offset: 0x00338F40
		public override int SeedPart
		{
			get
			{
				return 1872954345;
			}
		}

		// Token: 0x0600B42F RID: 46127 RVA: 0x0033AD47 File Offset: 0x00338F47
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient landing"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(GenStep_ScatterAncientLandingPad.SizeChanceCurve));
			base.Generate(map, parms);
		}

		// Token: 0x0600B430 RID: 46128 RVA: 0x0033AD84 File Offset: 0x00338F84
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(loc, this.randomSize, this.randomSize);
			return this.CanPlaceAt(rect, map);
		}

		// Token: 0x0600B431 RID: 46129 RVA: 0x0033ADC0 File Offset: 0x00338FC0
		private bool CanPlaceAt(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.ExpandedBy(2))
			{
				if (!c.InBounds(map))
				{
					return false;
				}
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
				if (terrainDef.IsWater || terrainDef.IsRoad)
				{
					return false;
				}
				if (c.GetEdifice(map) != null)
				{
					return false;
				}
				if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Light))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B432 RID: 46130 RVA: 0x0033AE68 File Offset: 0x00339068
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.filthDensity = new FloatRange?(new FloatRange(0.025f, 0.05f));
			resolveParams.filthDef = ThingDefOf.Filth_Ash;
			resolveParams.rect = CellRect.CenteredOn(loc, this.randomSize, this.randomSize);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			BaseGen.Generate();
			SketchResolveParams parms2 = default(SketchResolveParams);
			parms2.landingPadSize = new IntVec2?(new IntVec2(this.randomSize, this.randomSize));
			parms2.sketch = new Sketch();
			SketchGen.Generate(SketchResolverDefOf.AncientLandingPad, parms2).Spawn(map, loc - new IntVec3(this.randomSize / 2, 0, this.randomSize / 2), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, false, null, null, null);
		}

		// Token: 0x04007B9C RID: 31644
		private static readonly SimpleCurve SizeChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(8f, 0f),
				true
			},
			{
				new CurvePoint(12f, 4f),
				true
			},
			{
				new CurvePoint(18f, 0f),
				true
			}
		};

		// Token: 0x04007B9D RID: 31645
		private const int Gap = 2;

		// Token: 0x04007B9E RID: 31646
		private int randomSize;
	}
}
