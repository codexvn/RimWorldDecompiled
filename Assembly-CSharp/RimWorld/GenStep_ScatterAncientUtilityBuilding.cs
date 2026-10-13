using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF7 RID: 8183
	public class GenStep_ScatterAncientUtilityBuilding : GenStep_Scatterer
	{
		// Token: 0x17001B79 RID: 7033
		// (get) Token: 0x0600B450 RID: 46160 RVA: 0x0033AD40 File Offset: 0x00338F40
		public override int SeedPart
		{
			get
			{
				return 1872954345;
			}
		}

		// Token: 0x0600B451 RID: 46161 RVA: 0x0033B839 File Offset: 0x00339A39
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient outdoor building"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(GenStep_ScatterAncientUtilityBuilding.SizeChanceCurve));
			base.Generate(map, parms);
		}

		// Token: 0x0600B452 RID: 46162 RVA: 0x0033B874 File Offset: 0x00339A74
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			if (!loc.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			CellRect rect = new CellRect(loc.x, loc.z, this.randomSize, this.randomSize);
			return this.CanPlaceAt(rect, map);
		}

		// Token: 0x0600B453 RID: 46163 RVA: 0x0033B8CC File Offset: 0x00339ACC
		private bool CanPlaceAt(CellRect rect, Map map)
		{
			if (MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Any((CellRect ur) => ur.Overlaps(rect)))
			{
				return false;
			}
			foreach (IntVec3 c in rect.Cells)
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
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable)
					{
						return false;
					}
				}
				if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B454 RID: 46164 RVA: 0x0033B9DC File Offset: 0x00339BDC
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			SketchResolveParams parms2 = default(SketchResolveParams);
			parms2.utilityBuildingSize = new IntVec2?(new IntVec2(this.randomSize, this.randomSize));
			parms2.sketch = new Sketch();
			SketchGen.Generate(SketchResolverDefOf.AncientUtilityBuilding, parms2).Spawn(map, loc, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, false, null, null, null);
		}

		// Token: 0x04007BA9 RID: 31657
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

		// Token: 0x04007BAA RID: 31658
		private int randomSize;
	}
}
