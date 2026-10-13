using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02002050 RID: 8272
	public class GenStep_AncientStockpile_Gravcore : GenStep
	{
		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x0600B5BB RID: 46523 RVA: 0x00343779 File Offset: 0x00341979
		public override int SeedPart
		{
			get
			{
				return 341298741;
			}
		}

		// Token: 0x0600B5BC RID: 46524 RVA: 0x00343780 File Offset: 0x00341980
		public override void Generate(Map map, GenStepParams parms)
		{
			for (int i = 0; i < 3; i++)
			{
				this.GenerateHatch(map, i == 0);
			}
		}

		// Token: 0x0600B5BD RID: 46525 RVA: 0x003437A4 File Offset: 0x003419A4
		private void GenerateHatch(Map map, bool spawnGravcore = false)
		{
			GenStep_AncientStockpile_Gravcore.<>c__DisplayClass7_0 CS$<>8__locals1 = new GenStep_AncientStockpile_Gravcore.<>c__DisplayClass7_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			SketchResolveParams parms = default(SketchResolveParams);
			parms.sketch = new Sketch();
			CS$<>8__locals1.sketch = SketchGen.Generate(SketchResolverDefOf.AncientHatch, parms);
			CellRect item;
			if (!MapGenUtility.TryGetRandomClearRect(CS$<>8__locals1.sketch.OccupiedSize.x, CS$<>8__locals1.sketch.OccupiedSize.z, out item, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<GenerateHatch>g__Validator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1))
			{
				IntVec3 center;
				if (!CellFinder.TryFindRandomCell(CS$<>8__locals1.map, (IntVec3 cell) => base.<GenerateHatch>g__Validator|0(CellRect.CenteredOn(cell, CS$<>8__locals1.sketch.OccupiedSize)), out center))
				{
					return;
				}
				item = CellRect.CenteredOn(center, CS$<>8__locals1.sketch.OccupiedSize);
			}
			GenStep_AncientStockpile_Gravcore.spawnedThings.Clear();
			CS$<>8__locals1.sketch.Spawn(CS$<>8__locals1.map, item.Min, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, true, false, true, GenStep_AncientStockpile_Gravcore.spawnedThings, false, false, null, null, null);
			CS$<>8__locals1.usedRects.Add(item);
			AncientHatch ancientHatch = GenStep_AncientStockpile_Gravcore.spawnedThings.FirstOrDefault((Thing t) => t.def == ThingDefOf.AncientHatch) as AncientHatch;
			if (spawnGravcore)
			{
				ancientHatch.stockpileType = TileMutatorWorker_Stockpile.StockpileType.Gravcore;
			}
			else
			{
				ancientHatch.layout = LayoutDefOf.AncientStockpileFake;
			}
			this.hatches.Add(ancientHatch);
		}

		// Token: 0x04007CAE RID: 31918
		private List<Thing> hatches = new List<Thing>();

		// Token: 0x04007CAF RID: 31919
		private const int NumHatches = 3;

		// Token: 0x04007CB0 RID: 31920
		private const int MinDistanceBetweenHatches = 20;

		// Token: 0x04007CB1 RID: 31921
		private static List<Thing> spawnedThings = new List<Thing>();
	}
}
