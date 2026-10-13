using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200211C RID: 8476
	public class TileMutatorWorker_Stockpile : TileMutatorWorker
	{
		// Token: 0x0600B8F1 RID: 47345 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Stockpile(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B8F2 RID: 47346 RVA: 0x003564B0 File Offset: 0x003546B0
		public override string GetLabel(PlanetTile tile)
		{
			return ("Stockpile" + TileMutatorWorker_Stockpile.GetStockpileType(tile).ToString()).Translate();
		}

		// Token: 0x0600B8F3 RID: 47347 RVA: 0x003564E8 File Offset: 0x003546E8
		public override void GeneratePostFog(Map map)
		{
			TileMutatorWorker_Stockpile.<>c__DisplayClass4_0 CS$<>8__locals1 = new TileMutatorWorker_Stockpile.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			SketchResolveParams parms = default(SketchResolveParams);
			parms.sketch = new Sketch();
			CS$<>8__locals1.sketch = SketchGen.Generate(SketchResolverDefOf.AncientHatch, parms);
			CellRect item;
			if (!MapGenUtility.TryGetRandomClearRect(CS$<>8__locals1.sketch.OccupiedSize.x, CS$<>8__locals1.sketch.OccupiedSize.z, out item, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<GeneratePostFog>g__Validator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1))
			{
				IntVec3 center;
				if (!CellFinder.TryFindRandomCell(CS$<>8__locals1.map, (IntVec3 c) => base.<GeneratePostFog>g__Validator|0(CellRect.CenteredOn(c, CS$<>8__locals1.sketch.OccupiedSize)), out center))
				{
					return;
				}
				item = CellRect.CenteredOn(center, CS$<>8__locals1.sketch.OccupiedSize);
			}
			List<Thing> list = new List<Thing>();
			CS$<>8__locals1.sketch.Spawn(CS$<>8__locals1.map, item.Min, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, true, false, true, list, false, false, null, null, null);
			CS$<>8__locals1.usedRects.Add(item);
			(list.FirstOrDefault((Thing t) => t.def == ThingDefOf.AncientHatch) as AncientHatch).stockpileType = TileMutatorWorker_Stockpile.GetStockpileType(CS$<>8__locals1.map.Tile);
		}

		// Token: 0x0600B8F4 RID: 47348 RVA: 0x00356626 File Offset: 0x00354826
		private static TileMutatorWorker_Stockpile.StockpileType GetStockpileType(PlanetTile tile)
		{
			Rand.PushState(tile.tileId);
			TileMutatorWorker_Stockpile.StockpileType result = TileMutatorWorker_Stockpile.GeneratableStockpileTypes.RandomElement<TileMutatorWorker_Stockpile.StockpileType>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04007F8B RID: 32651
		public static readonly List<TileMutatorWorker_Stockpile.StockpileType> GeneratableStockpileTypes = new List<TileMutatorWorker_Stockpile.StockpileType>
		{
			TileMutatorWorker_Stockpile.StockpileType.Medicine,
			TileMutatorWorker_Stockpile.StockpileType.Chemfuel,
			TileMutatorWorker_Stockpile.StockpileType.Component,
			TileMutatorWorker_Stockpile.StockpileType.Weapons,
			TileMutatorWorker_Stockpile.StockpileType.Drugs
		};

		// Token: 0x0200211D RID: 8477
		public enum StockpileType
		{
			// Token: 0x04007F8D RID: 32653
			Medicine,
			// Token: 0x04007F8E RID: 32654
			Chemfuel,
			// Token: 0x04007F8F RID: 32655
			Component,
			// Token: 0x04007F90 RID: 32656
			Weapons,
			// Token: 0x04007F91 RID: 32657
			Gravcore,
			// Token: 0x04007F92 RID: 32658
			Drugs
		}
	}
}
