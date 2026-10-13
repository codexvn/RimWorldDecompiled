using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203E RID: 8254
	public class GenStep_FrozenRuins : GenStep_BaseRuins
	{
		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x0600B55D RID: 46429 RVA: 0x00342311 File Offset: 0x00340511
		public override int SeedPart
		{
			get
			{
				return 964521;
			}
		}

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x0600B55E RID: 46430 RVA: 0x003423CD File Offset: 0x003405CD
		protected override int RegionSize
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x17001B9D RID: 7069
		// (get) Token: 0x0600B55F RID: 46431 RVA: 0x003423D1 File Offset: 0x003405D1
		protected override FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(0.01f, 0.05f);
			}
		}

		// Token: 0x17001B9E RID: 7070
		// (get) Token: 0x0600B560 RID: 46432 RVA: 0x003423E2 File Offset: 0x003405E2
		protected override FloatRange MergeRange
		{
			get
			{
				return new FloatRange(0.05f, 0.15f);
			}
		}

		// Token: 0x17001B9F RID: 7071
		// (get) Token: 0x0600B561 RID: 46433 RVA: 0x000E9647 File Offset: 0x000E7847
		protected override int MoveRangeLimit
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001BA0 RID: 7072
		// (get) Token: 0x0600B562 RID: 46434 RVA: 0x000E9647 File Offset: 0x000E7847
		protected override int ContractLimit
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001BA1 RID: 7073
		// (get) Token: 0x0600B563 RID: 46435 RVA: 0x002A9950 File Offset: 0x002A7B50
		protected override int MinRegionSize
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17001BA2 RID: 7074
		// (get) Token: 0x0600B564 RID: 46436 RVA: 0x003423F3 File Offset: 0x003405F3
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientRuinsGlacier;
			}
		}

		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x0600B565 RID: 46437 RVA: 0x00342320 File Offset: 0x00340520
		protected override Faction Faction
		{
			get
			{
				return Faction.OfAncientsHostile;
			}
		}

		// Token: 0x0600B566 RID: 46438 RVA: 0x003423FA File Offset: 0x003405FA
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Frozen Ruins"))
			{
				return;
			}
			base.Generate(map, parms);
		}

		// Token: 0x0600B567 RID: 46439 RVA: 0x00342414 File Offset: 0x00340614
		public override void GenerateRuins(Map map, GenStepParams parms, FloatRange mapFillPercentRange)
		{
			base.GenerateRuins(map, parms, mapFillPercentRange);
			GenStep_FrozenRuins.EncaseStructuresInIce(map, this.structureSketches, null);
		}

		// Token: 0x0600B568 RID: 46440 RVA: 0x00342440 File Offset: 0x00340640
		public static void EncaseStructuresInIce(Map map, List<LayoutStructureSketch> structureSketches, GenMorphology.GenPatchParms? patchParms = null)
		{
			GenStep_FrozenRuins.<>c__DisplayClass21_0 CS$<>8__locals1 = new GenStep_FrozenRuins.<>c__DisplayClass21_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.structureSketches = structureSketches;
			foreach (LayoutStructureSketch layoutStructureSketch in CS$<>8__locals1.structureSketches)
			{
				if (layoutStructureSketch.structureLayout != null)
				{
					List<CellRect> rects = (from r in layoutStructureSketch.structureLayout.Rooms.SelectMany((LayoutRoom room) => room.rects)
					select r.ExpandedBy(24)).ToList<CellRect>();
					GenMorphology.GenPatchParms value = patchParms.GetValueOrDefault();
					if (patchParms == null)
					{
						value = GenMorphology.GenPatchParms.For(ThingDefOf.SolidIce, TerrainDefOf.Ice);
						patchParms = new GenMorphology.GenPatchParms?(value);
					}
					GenMorphology.GenerateNaturalPatch(CS$<>8__locals1.map, rects, patchParms, new Predicate<IntVec3>(CS$<>8__locals1.<EncaseStructuresInIce>g__Validator|2));
					foreach (LayoutRoom layoutRoom in layoutStructureSketch.structureLayout.Rooms)
					{
						foreach (IntVec3 c in layoutRoom.Cells)
						{
							if (CS$<>8__locals1.map.roofGrid.RoofAt(c) == null)
							{
								CS$<>8__locals1.map.roofGrid.SetRoof(c, RoofDefOf.RoofRockThin);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B569 RID: 46441 RVA: 0x00342618 File Offset: 0x00340818
		protected override bool IsValidRect(CellRect rect, Map map)
		{
			if (!base.IsValidRect(rect, map))
			{
				return false;
			}
			if (map.TileInfo.Mutators.Contains(TileMutatorDefOf.Crevasse))
			{
				foreach (IntVec3 c in rect.Cells)
				{
					Building edifice = c.GetEdifice(map);
					if (((edifice != null) ? edifice.def : null) != ThingDefOf.SolidIce)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x04007C8A RID: 31882
		private const int CellRange = 24;
	}
}
