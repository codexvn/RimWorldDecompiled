using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020020CA RID: 8394
	public abstract class TileMutatorWorker_AncientVent : TileMutatorWorker
	{
		// Token: 0x17001BE3 RID: 7139
		// (get) Token: 0x0600B78C RID: 46988
		protected abstract ThingDef AncientVentDef { get; }

		// Token: 0x0600B78D RID: 46989 RVA: 0x0034DD80 File Offset: 0x0034BF80
		public TileMutatorWorker_AncientVent(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B78E RID: 46990 RVA: 0x0034DD94 File Offset: 0x0034BF94
		public override void GenerateNonCriticalStructures(Map map)
		{
			TileMutatorWorker_AncientVent.<>c__DisplayClass8_0 CS$<>8__locals1 = new TileMutatorWorker_AncientVent.<>c__DisplayClass8_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.numToGenerate = TileMutatorWorker_AncientVent.numVentsRange.RandomInRange;
			this.positionsToGenerate.Clear();
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			while (this.positionsToGenerate.Count < this.numToGenerate)
			{
				IntVec3 item;
				if (!CellFinder.TryFindRandomCell(CS$<>8__locals1.map, new Predicate<IntVec3>(CS$<>8__locals1.<GenerateNonCriticalStructures>g__Validator|0), out item))
				{
					Log.Warning(string.Format("{0} failed to find any valid cells to place at least one vent. Placed {1} vents, wanted to place {2}.", "TileMutatorWorker_AncientVent", this.positionsToGenerate.Count, this.numToGenerate));
					break;
				}
				this.positionsToGenerate.Add(item);
			}
			TerrainGrid terrainGrid = CS$<>8__locals1.map.terrainGrid;
			IntVec3 c;
			this.positionsToGenerate.RemoveWhere(delegate(IntVec3 p)
			{
				IEnumerable<IntVec3> source = GenRadial.RadialCellsAround(p, 0f, 8.9f);
				Func<IntVec3, bool> predicate;
				if ((predicate = CS$<>8__locals1.<>9__4) == null)
				{
					predicate = (CS$<>8__locals1.<>9__4 = ((IntVec3 c) => CS$<>8__locals1.map.terrainGrid.TerrainAt(c).IsWater));
				}
				return source.Any(predicate);
			});
			foreach (IntVec3 center in this.positionsToGenerate)
			{
				using (IEnumerator<IntVec3> enumerator2 = GenRadial.RadialCellsAround(center, 0f, 8.9f).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						c = enumerator2.Current;
						if (c.InBounds(CS$<>8__locals1.map) && c.GetEdifice(CS$<>8__locals1.map) == null && !Rand.Chance(0f))
						{
							terrainGrid.SetTerrain(c, TerrainDefOf.AncientMegastructure);
						}
					}
				}
			}
			foreach (IntVec3 loc in this.positionsToGenerate)
			{
				Thing thing = ThingMaker.MakeThing(this.AncientVentDef, null);
				GenSpawn.Spawn(thing, loc, CS$<>8__locals1.map, WipeMode.Vanish);
				CS$<>8__locals1.usedRects.Add(thing.OccupiedRect());
			}
		}

		// Token: 0x04007E14 RID: 32276
		private const float RadiusToClear = 8.9f;

		// Token: 0x04007E15 RID: 32277
		private const float ChanceNoMegaStructure = 0f;

		// Token: 0x04007E16 RID: 32278
		private static readonly IntRange numVentsRange = new IntRange(5, 9);

		// Token: 0x04007E17 RID: 32279
		private int numToGenerate;

		// Token: 0x04007E18 RID: 32280
		private List<IntVec3> positionsToGenerate = new List<IntVec3>();
	}
}
