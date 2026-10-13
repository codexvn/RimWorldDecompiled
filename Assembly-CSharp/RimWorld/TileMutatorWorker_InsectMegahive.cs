using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020F1 RID: 8433
	public class TileMutatorWorker_InsectMegahive : TileMutatorWorker
	{
		// Token: 0x0600B82B RID: 47147 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_InsectMegahive(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B82C RID: 47148 RVA: 0x00351FEC File Offset: 0x003501EC
		public override void GeneratePostFog(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			IntVec3 entranceCell;
			if (!TileMutatorWorker_InsectMegahive.TryGetEntranceCell(map, out entranceCell))
			{
				return;
			}
			TileMutatorWorker_InsectMegahive.GenerateMegahiveEntrance(map, entranceCell, false);
		}

		// Token: 0x0600B82D RID: 47149 RVA: 0x00352014 File Offset: 0x00350214
		public static void GenerateMegahiveEntrance(Map map, IntVec3 entranceCell, bool spawnGravcore = false)
		{
			foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(entranceCell, map, 150, (IntVec3 _) => true))
			{
				Building edifice = intVec.GetEdifice(map);
				if (edifice != null)
				{
					edifice.Destroy(DestroyMode.Vanish);
					map.fogGrid.FloodUnfogAdjacent(edifice, false);
				}
				map.terrainGrid.SetTerrain(intVec, GenStep_RocksFromGrid.RockDefAt(intVec).building.naturalTerrain);
				map.terrainGrid.SetTerrain(intVec, TerrainDefOf.InsectSludge);
				if (Rand.Chance(0.2f))
				{
					GenSpawn.Spawn(ThingDefOf.Filth_Slime, intVec, map, WipeMode.Vanish);
				}
			}
			(GenSpawn.Spawn(ThingDefOf.InsectLairEntrance, entranceCell, map, WipeMode.Vanish) as InsectLairEntrance).spawnGravcore = spawnGravcore;
			MapGenerator.UsedRects.Add(GenAdj.OccupiedRect(entranceCell, Rot4.North, 8, 8));
			int randomInRange = TileMutatorWorker_InsectMegahive.NumHivesRange.RandomInRange;
			Predicate<IntVec3> <>9__1;
			for (int i = 0; i < randomInRange; i++)
			{
				Map map2 = map;
				int squareRadius = 6;
				Predicate<IntVec3> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.Hive, c, map, null, true)));
				}
				IntVec3 spawnCell;
				if (CellFinder.TryFindRandomCellNear(entranceCell, map2, squareRadius, validator, out spawnCell, -1))
				{
					HiveUtility.SpawnHive(spawnCell, map, WipeMode.VanishOrMoveAside, true, false, true, false, true, false, true);
				}
			}
			FloodFillerFog.FloodUnfog(entranceCell, map);
		}

		// Token: 0x0600B82E RID: 47150 RVA: 0x003521CC File Offset: 0x003503CC
		public static bool TryGetEntranceCell(Map map, out IntVec3 entranceCell)
		{
			return CellFinder.TryFindRandomCell(map, (IntVec3 cell) => GenSpawn.CanSpawnAt(ThingDefOf.InsectLairEntrance, cell, map, null, true) && cell.Roofed(map) && !cell.Fogged(map), out entranceCell) || CellFinder.TryFindRandomCell(map, (IntVec3 cell) => GenSpawn.CanSpawnAt(ThingDefOf.InsectLairEntrance, cell, map, null, true) && !cell.Fogged(map), out entranceCell);
		}

		// Token: 0x04007EEB RID: 32491
		private static readonly IntRange NumHivesRange = new IntRange(1, 2);
	}
}
