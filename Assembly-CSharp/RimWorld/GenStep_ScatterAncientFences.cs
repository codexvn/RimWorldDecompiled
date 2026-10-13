using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF1 RID: 8177
	public class GenStep_ScatterAncientFences : GenStep_Scatterer
	{
		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x0600B427 RID: 46119 RVA: 0x0033AA77 File Offset: 0x00338C77
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x0600B428 RID: 46120 RVA: 0x0033AA80 File Offset: 0x00338C80
		protected override float GetPlacementFactor(Map map)
		{
			float num = 1f;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.junkDensityFactor;
			}
			return num;
		}

		// Token: 0x0600B429 RID: 46121 RVA: 0x0033AADC File Offset: 0x00338CDC
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient fences"))
			{
				return;
			}
			this.count = Rand.RangeInclusive(1, 3);
			GenStep_ScatterAncientFences.tmpUsedRects.Clear();
			base.Generate(map, parms);
			GenStep_ScatterAncientFences.tmpUsedRects.Clear();
			GenStep_ScatterAncientFences.tmpFenceCells.Clear();
		}

		// Token: 0x0600B42A RID: 46122 RVA: 0x0033AB2C File Offset: 0x00338D2C
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			this.thingToScatter = (Rand.Bool ? ThingDefOf.AncientFence : ThingDefOf.AncientRazorWire);
			int randomInRange = GenStep_ScatterAncientFences.RectSizeRange.RandomInRange;
			CellRect item = CellRect.CenteredOn(loc, randomInRange, randomInRange);
			for (int i = 0; i < GenStep_ScatterAncientFences.tmpUsedRects.Count; i++)
			{
				if (item.Overlaps(GenStep_ScatterAncientFences.tmpUsedRects[i]))
				{
					return false;
				}
			}
			GenStep_ScatterAncientFences.tmpFenceCells.Clear();
			GenStep_ScatterAncientFences.tmpFenceCells.AddRange(item.EdgeCells);
			for (int j = 0; j < GenStep_ScatterAncientFences.tmpFenceCells.Count; j++)
			{
				if (!GenStep_ScatterAncientFences.tmpFenceCells[j].InBounds(map) || GenStep_ScatterAncientFences.tmpFenceCells[j].GetEdifice(map) != null || GenStep_ScatterAncientFences.tmpFenceCells[j].GetRoof(map) != null || GenStep_ScatterAncientFences.tmpFenceCells[j].Impassable(map))
				{
					return false;
				}
				TerrainDef terrain = GenStep_ScatterAncientFences.tmpFenceCells[j].GetTerrain(map);
				if (terrain.IsWater || terrain.IsRoad || !GenConstruct.CanBuildOnTerrain(this.thingToScatter, GenStep_ScatterAncientFences.tmpFenceCells[j], map, Rot4.North, null, null))
				{
					return false;
				}
			}
			GenStep_ScatterAncientFences.tmpUsedRects.Add(item);
			return true;
		}

		// Token: 0x0600B42B RID: 46123 RVA: 0x0033AC84 File Offset: 0x00338E84
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			int num = Rand.Range(0, GenStep_ScatterAncientFences.tmpFenceCells.Count);
			int num2 = Mathf.RoundToInt((float)GenStep_ScatterAncientFences.tmpFenceCells.Count * GenStep_ScatterAncientFences.PerimeterCellsRange.RandomInRange);
			for (int i = 0; i < num2; i++)
			{
				if (!Rand.Chance(0.25f))
				{
					IntVec3 loc2 = GenStep_ScatterAncientFences.tmpFenceCells[(i + num) % GenStep_ScatterAncientFences.tmpFenceCells.Count];
					GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToScatter, null), loc2, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x04007B95 RID: 31637
		private static readonly FloatRange PerimeterCellsRange = new FloatRange(0.2f, 0.4f);

		// Token: 0x04007B96 RID: 31638
		private static readonly IntRange RectSizeRange = new IntRange(6, 12);

		// Token: 0x04007B97 RID: 31639
		private const float SkipChance = 0.25f;

		// Token: 0x04007B98 RID: 31640
		private CellRect rect;

		// Token: 0x04007B99 RID: 31641
		private ThingDef thingToScatter;

		// Token: 0x04007B9A RID: 31642
		private static readonly List<IntVec3> tmpFenceCells = new List<IntVec3>();

		// Token: 0x04007B9B RID: 31643
		private static readonly List<CellRect> tmpUsedRects = new List<CellRect>();
	}
}
