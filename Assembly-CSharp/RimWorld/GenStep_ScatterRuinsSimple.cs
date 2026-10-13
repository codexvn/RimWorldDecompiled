using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002003 RID: 8195
	public class GenStep_ScatterRuinsSimple : GenStep_Scatterer
	{
		// Token: 0x17001B81 RID: 7041
		// (get) Token: 0x0600B48B RID: 46219 RVA: 0x0033CABF File Offset: 0x0033ACBF
		public override int SeedPart
		{
			get
			{
				return 1348417666;
			}
		}

		// Token: 0x0600B48C RID: 46220 RVA: 0x0033CAC6 File Offset: 0x0033ACC6
		protected virtual CellRect EffectiveRectAt(IntVec3 c)
		{
			return CellRect.CenteredOn(c, this.randomSize, this.randomSize);
		}

		// Token: 0x0600B48D RID: 46221 RVA: 0x0033CADA File Offset: 0x0033ACDA
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(this.ruinSizeChanceCurve));
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x0600B48E RID: 46222 RVA: 0x0033CAFC File Offset: 0x0033ACFC
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			if (this.mustBeStandable && !c.Standable(map))
			{
				return false;
			}
			if (!this.CanPlaceAncientBuildingInRange(this.EffectiveRectAt(c).ClipInsideMap(map), map))
			{
				return false;
			}
			List<CellRect> list;
			if (!this.ignoreUsedRects && MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				CellRect cellRect = this.EffectiveRectAt(c);
				foreach (CellRect cellRect2 in list)
				{
					if (cellRect.Overlaps(cellRect2.ExpandedBy(this.usedRectsPadding)))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600B48F RID: 46223 RVA: 0x0033CBCC File Offset: 0x0033ADCC
		protected bool CanPlaceAncientBuildingInRange(CellRect rect, Map map)
		{
			if (MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Any((CellRect x) => x.Overlaps(rect)))
			{
				return false;
			}
			foreach (IntVec3 c in rect.Cells)
			{
				if (!this.canBeOnEdge && !c.InBounds(map))
				{
					return false;
				}
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
				if (terrainDef.HasTag("River") || terrainDef.HasTag("Road"))
				{
					return false;
				}
				if (!GenConstruct.CanBuildOnTerrain(ThingDefOf.Wall, c, map, Rot4.North, null, null))
				{
					return false;
				}
				Building edifice = c.GetEdifice(map);
				if (edifice != null && edifice.def.IsBuildingArtificial)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B490 RID: 46224 RVA: 0x0033CCCC File Offset: 0x0033AECC
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CellRect cellRect = this.EffectiveRectAt(c).ClipInsideMap(map);
			if (this.CanPlaceAncientBuildingInRange(cellRect, map))
			{
				SketchResolveParams parms2 = default(SketchResolveParams);
				parms2.sketch = new Sketch();
				parms2.monumentSize = new IntVec2?(new IntVec2(cellRect.Width, cellRect.Height));
				parms2.destroyChanceExp = new float?(this.destroyChanceExp);
				SketchGen.Generate(SketchResolverDefOf.MonumentRuin, parms2).Spawn(map, cellRect.CenterCell, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, false, delegate(SketchEntity entity, IntVec3 cell)
				{
					if (this.clearSurroundingArea)
					{
						foreach (IntVec3 b in GenAdj.CardinalDirectionsAndInside)
						{
							if ((cell + b).InBounds(map))
							{
								Building edifice = (cell + b).GetEdifice(map);
								if (edifice != null && !edifice.Position.CloseToEdge(map, 3) && edifice.def.building.isNaturalRock)
								{
									edifice.Destroy(DestroyMode.Vanish);
								}
							}
						}
					}
					bool result = false;
					foreach (IntVec3 b2 in entity.OccupiedRect.AdjacentCells)
					{
						IntVec3 c2 = cell + b2;
						if (c2.InBounds(map))
						{
							Building edifice2 = c2.GetEdifice(map);
							if (edifice2 == null || !edifice2.def.building.isNaturalRock)
							{
								result = true;
								break;
							}
						}
					}
					return result;
				}, null, null);
				if (!this.ignoreUsedRects)
				{
					orGenerateVar.Add(cellRect);
				}
			}
		}

		// Token: 0x04007BC7 RID: 31687
		private int randomSize;

		// Token: 0x04007BC8 RID: 31688
		private SimpleCurve ruinSizeChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(6f, 0f),
				true
			},
			{
				new CurvePoint(6.001f, 4f),
				true
			},
			{
				new CurvePoint(10f, 1f),
				true
			},
			{
				new CurvePoint(30f, 0f),
				true
			}
		};

		// Token: 0x04007BC9 RID: 31689
		private bool clearSurroundingArea;

		// Token: 0x04007BCA RID: 31690
		private float destroyChanceExp = 1.32f;

		// Token: 0x04007BCB RID: 31691
		private bool mustBeStandable;

		// Token: 0x04007BCC RID: 31692
		private bool canBeOnEdge;

		// Token: 0x04007BCD RID: 31693
		private bool ignoreUsedRects;

		// Token: 0x04007BCE RID: 31694
		private int usedRectsPadding = 2;
	}
}
