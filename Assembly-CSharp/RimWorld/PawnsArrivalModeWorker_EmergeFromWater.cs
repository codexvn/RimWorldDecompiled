using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200239E RID: 9118
	public class PawnsArrivalModeWorker_EmergeFromWater : PawnsArrivalModeWorker
	{
		// Token: 0x0600C574 RID: 50548 RVA: 0x003971F0 File Offset: 0x003953F0
		public override bool CanUseWith(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			SurfaceTile surfaceTile;
			if (map.TileInfo.Isnt(out surfaceTile))
			{
				return false;
			}
			bool flag = !surfaceTile.Rivers.NullOrEmpty<SurfaceTile.RiverLink>();
			bool isCoastal = map.TileInfo.IsCoastal;
			return flag || isCoastal;
		}

		// Token: 0x0600C575 RID: 50549 RVA: 0x00397238 File Offset: 0x00395438
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			if (pawns.Empty<Pawn>())
			{
				return;
			}
			List<IntVec3> list = new List<IntVec3>();
			List<IntVec3> list2 = new List<IntVec3>();
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 item = PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.RandomElement<IntVec3>();
				PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.Remove(item);
				(this.IsBigSpawn(pawns[i]) ? list2 : list).Add(item);
				pawns[i].stances.stunner.StunFor(PawnsArrivalModeWorker_EmergeFromWater.StunRange.RandomInRangeSeeded(pawns[i].ThingID.GetHashCode()).SecondsToTicks(), pawns[i], true, false, false);
			}
			float num = 8f / (float)pawns.Count;
			SpawnRequest spawnRequest = new SpawnRequest((from p in pawns
			where !this.IsBigSpawn(p)
			select p).Cast<Thing>().ToList<Thing>(), list, 1, num, null);
			SpawnRequest spawnRequest2 = new SpawnRequest((from p in pawns
			where this.IsBigSpawn(p)
			select p).Cast<Thing>().ToList<Thing>(), list2, 1, num * 2.7f, null);
			spawnRequest.spawnSound = SoundDefOf.EmergeFromWater;
			spawnRequest.spawnEffect = EffecterDefOf.PawnEmergeFromWater;
			spawnRequest.preSpawnEffect = EffecterDefOf.WaterMist;
			spawnRequest.preSpawnEffecterOffsetTicks = -40;
			spawnRequest.initialDelay = 40;
			spawnRequest.lord = parms.lord;
			spawnRequest2.spawnSound = SoundDefOf.EmergeFromWater;
			spawnRequest2.spawnEffect = EffecterDefOf.PawnEmergeFromWaterLarge;
			spawnRequest2.preSpawnEffect = EffecterDefOf.WaterMist;
			spawnRequest2.preSpawnEffecterOffsetTicks = -40;
			spawnRequest2.initialDelay = 40;
			spawnRequest2.lord = parms.lord;
			Find.CurrentMap.deferredSpawner.AddRequest(spawnRequest, true);
			Find.CurrentMap.deferredSpawner.AddRequest(spawnRequest2, true);
		}

		// Token: 0x0600C576 RID: 50550 RVA: 0x003973F4 File Offset: 0x003955F4
		private bool IsBigSpawn(Pawn pawn)
		{
			return pawn.BodySize > 1.5f;
		}

		// Token: 0x0600C577 RID: 50551 RVA: 0x00397404 File Offset: 0x00395604
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = 20;
			for (;;)
			{
				IntVec3 spawnCenter = parms.spawnCenter;
				if (!(parms.spawnCenter == IntVec3.Invalid) || num <= 0)
				{
					return false;
				}
				PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.Clear();
				parms.spawnCenter = CellFinderLoose.RandomCellWith((IntVec3 cell) => (cell.GetTerrain(map).IsRiver || cell.GetTerrain(map).IsOcean) && cell.Walkable(map) && map.reachability.CanReachColony(cell), map, 1000);
				if (parms.spawnCenter != IntVec3.Invalid)
				{
					map.floodFiller.FloodFill(parms.spawnCenter, (IntVec3 cell) => cell.GetTerrain(map).IsWater && cell.Walkable(map) && cell.InHorDistOf(parms.spawnCenter, 30f), delegate(IntVec3 cell)
					{
						PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.Add(cell);
					}, int.MaxValue, false, null);
					if (PawnsArrivalModeWorker_EmergeFromWater.WaterBodyCells.Count > 50)
					{
						break;
					}
				}
				num--;
			}
			return true;
		}

		// Token: 0x040086AC RID: 34476
		private const int LargeBodyOfWaterThreshold = 50;

		// Token: 0x040086AD RID: 34477
		private const float MaxSpawnRadius = 30f;

		// Token: 0x040086AE RID: 34478
		private const float SpawnDurationSeconds = 8f;

		// Token: 0x040086AF RID: 34479
		private const int SpawnDelayTicks = 40;

		// Token: 0x040086B0 RID: 34480
		public static readonly FloatRange StunRange = new FloatRange(2f, 4f);

		// Token: 0x040086B1 RID: 34481
		private static readonly HashSet<IntVec3> WaterBodyCells = new HashSet<IntVec3>();
	}
}
