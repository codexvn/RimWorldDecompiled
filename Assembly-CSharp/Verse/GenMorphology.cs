using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000CFF RID: 3327
	public static class GenMorphology
	{
		// Token: 0x06005534 RID: 21812 RVA: 0x001B7864 File Offset: 0x001B5A64
		public static void Erode(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			GenMorphology.cellsSet.Clear();
			GenMorphology.cellsSet.AddRange(cells);
			GenMorphology.tmpEdgeCells.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 item = cells[i] + cardinalDirections[j];
					if (!GenMorphology.cellsSet.Contains(item))
					{
						GenMorphology.tmpEdgeCells.Add(cells[i]);
						break;
					}
				}
			}
			if (!GenMorphology.tmpEdgeCells.Any<IntVec3>())
			{
				return;
			}
			GenMorphology.tmpOutput.Clear();
			Predicate<IntVec3> passCheck;
			if (extraPredicate != null)
			{
				passCheck = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				passCheck = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x));
			}
			map.floodFiller.FloodFill(IntVec3.Invalid, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				if (traversalDist >= count)
				{
					GenMorphology.tmpOutput.Add(cell);
				}
				return false;
			}, int.MaxValue, false, GenMorphology.tmpEdgeCells);
			cells.Clear();
			cells.AddRange(GenMorphology.tmpOutput);
		}

		// Token: 0x06005535 RID: 21813 RVA: 0x001B7994 File Offset: 0x001B5B94
		public static void Dilate(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			FloodFiller floodFiller = map.floodFiller;
			IntVec3 invalid = IntVec3.Invalid;
			Predicate<IntVec3> passCheck = extraPredicate;
			if (extraPredicate == null && (passCheck = GenMorphology.<>c.<>9__4_0) == null)
			{
				passCheck = (GenMorphology.<>c.<>9__4_0 = ((IntVec3 x) => true));
			}
			floodFiller.FloodFill(invalid, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				if (traversalDist > count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					cells.Add(cell);
				}
				return false;
			}, int.MaxValue, false, cells);
		}

		// Token: 0x06005536 RID: 21814 RVA: 0x001B7A0B File Offset: 0x001B5C0B
		public static void Open(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Erode(cells, count, map, null);
			GenMorphology.Dilate(cells, count, map, null);
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x001B7A1F File Offset: 0x001B5C1F
		public static void Close(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Dilate(cells, count, map, null);
			GenMorphology.Erode(cells, count, map, null);
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x001B7A33 File Offset: 0x001B5C33
		public static void GenerateNaturalPatch(Map map, CellRect rect, GenMorphology.GenPatchParms? parms = null, Predicate<IntVec3> validator = null)
		{
			GenMorphology.GenerateNaturalPatch(map, new List<CellRect>
			{
				rect
			}, parms, validator);
		}

		// Token: 0x06005539 RID: 21817 RVA: 0x001B7A4C File Offset: 0x001B5C4C
		public static void GenerateNaturalPatch(Map map, List<CellRect> rects, GenMorphology.GenPatchParms? parms = null, Predicate<IntVec3> validator = null)
		{
			GenMorphology.GenPatchParms genPatchParms = parms ?? GenMorphology.GenPatchParms.Default;
			ModuleBase lhs = new Perlin((double)genPatchParms.macroFrequency, 1.25, 0.5, 6, map.ConstantRandSeed, QualityMode.Medium);
			ModuleBase rhs = new Perlin((double)genPatchParms.microFrequency, 2.0, 0.5, 6, map.ConstantRandSeed + 1, QualityMode.Medium);
			ModuleBase moduleBase = new Blend(lhs, rhs, new Const((double)genPatchParms.blendMacroMicro));
			moduleBase = new ScaleBias(0.5, 0.6000000238418579, moduleBase);
			ModuleBase moduleBase2 = new DistFromPointRects(rects);
			moduleBase2 = new Clamp(0.0, 1.0, moduleBase2);
			moduleBase2 = new ScaleBias(2.200000047683716, -0.5, moduleBase2);
			moduleBase = new Blend(moduleBase2, moduleBase, new Const((double)genPatchParms.blendFalloff));
			foreach (CellRect cellRect in rects)
			{
				foreach (IntVec3 intVec in cellRect.Cells)
				{
					if (GenMorphology.visited.Add(intVec) && intVec.InBounds(map) && (validator == null || validator(intVec)))
					{
						float value = moduleBase.GetValue(intVec);
						if (genPatchParms.terrainDef != null && value >= genPatchParms.terrainThreshold)
						{
							map.terrainGrid.SetTerrain(intVec, genPatchParms.terrainDef);
						}
						if (genPatchParms.edificeDef != null && value >= genPatchParms.edificeThreshold)
						{
							GenSpawn.Spawn(genPatchParms.edificeDef, intVec, map, WipeMode.Vanish);
						}
						if (genPatchParms.roofDef != null && value >= genPatchParms.roofThreshold)
						{
							map.roofGrid.SetRoof(intVec, genPatchParms.roofDef);
						}
					}
				}
			}
			GenMorphology.visited.Clear();
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x001B7C6C File Offset: 0x001B5E6C
		private static float ValueAt(ModuleBase noise, IntVec3 cell, List<CellRect> rects)
		{
			float value = rects.Min(delegate(CellRect rect)
			{
				IntVec3 b = rect.ClosestCellTo(cell);
				return (cell - b).Magnitude;
			});
			float num = Mathf.Clamp01(Mathf.InverseLerp(0f, 12f, value));
			return noise.GetValue(cell) + 1f - 2f * num;
		}

		// Token: 0x04003D7B RID: 15739
		private static HashSet<IntVec3> tmpOutput = new HashSet<IntVec3>();

		// Token: 0x04003D7C RID: 15740
		private static HashSet<IntVec3> cellsSet = new HashSet<IntVec3>();

		// Token: 0x04003D7D RID: 15741
		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();

		// Token: 0x04003D7E RID: 15742
		private static readonly HashSet<IntVec3> visited = new HashSet<IntVec3>();

		// Token: 0x02000D00 RID: 3328
		public struct GenPatchParms
		{
			// Token: 0x17000D4D RID: 3405
			// (get) Token: 0x0600553C RID: 21820 RVA: 0x001B7CF3 File Offset: 0x001B5EF3
			public static GenMorphology.GenPatchParms Default
			{
				get
				{
					return GenMorphology.GenPatchParms.For(ThingDefOf.Slate, ThingDefOf.Slate.building.naturalTerrain);
				}
			}

			// Token: 0x0600553D RID: 21821 RVA: 0x001B7D0E File Offset: 0x001B5F0E
			public static GenMorphology.GenPatchParms For(ThingDef edifice)
			{
				return GenMorphology.GenPatchParms.For(edifice, null);
			}

			// Token: 0x0600553E RID: 21822 RVA: 0x001B7D17 File Offset: 0x001B5F17
			public static GenMorphology.GenPatchParms For(TerrainDef terrain)
			{
				return GenMorphology.GenPatchParms.For(null, terrain);
			}

			// Token: 0x0600553F RID: 21823 RVA: 0x001B7D20 File Offset: 0x001B5F20
			public static GenMorphology.GenPatchParms For(ThingDef edifice, TerrainDef terrain)
			{
				return new GenMorphology.GenPatchParms
				{
					edificeDef = edifice,
					terrainDef = terrain,
					roofDef = RoofDefOf.RoofRockThin,
					macroFrequency = 0.05f,
					microFrequency = 0.08f,
					blendMacroMicro = 0.01f,
					blendFalloff = 0.4f,
					terrainThreshold = 0.45f,
					edificeThreshold = 0.5f,
					roofThreshold = 0.55f
				};
			}

			// Token: 0x04003D7F RID: 15743
			public ThingDef edificeDef;

			// Token: 0x04003D80 RID: 15744
			public TerrainDef terrainDef;

			// Token: 0x04003D81 RID: 15745
			public RoofDef roofDef;

			// Token: 0x04003D82 RID: 15746
			public float macroFrequency;

			// Token: 0x04003D83 RID: 15747
			public float microFrequency;

			// Token: 0x04003D84 RID: 15748
			public float blendMacroMicro;

			// Token: 0x04003D85 RID: 15749
			public float blendFalloff;

			// Token: 0x04003D86 RID: 15750
			public float terrainThreshold;

			// Token: 0x04003D87 RID: 15751
			public float edificeThreshold;

			// Token: 0x04003D88 RID: 15752
			public float roofThreshold;
		}
	}
}
