using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002012 RID: 8210
	public class GenStep_ReserveGravshipArea : GenStep
	{
		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x0600B4C4 RID: 46276 RVA: 0x0033DD5C File Offset: 0x0033BF5C
		public override int SeedPart
		{
			get
			{
				return 2031751232;
			}
		}

		// Token: 0x0600B4C5 RID: 46277 RVA: 0x0033DD64 File Offset: 0x0033BF64
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Gravship gravship = parms.gravship;
			if (gravship == null)
			{
				return;
			}
			bool flag;
			if (MapGenerator.TryGetVar<bool>("GravshipSpawnSet", out flag) && flag)
			{
				return;
			}
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			HashSet<IntVec3> cellsAdjacentToSubstructure = GravshipPlacementUtility.GetCellsAdjacentToSubstructure(gravship.OccupiedRects, 2);
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			for (int i = 0; i < cellsAdjacentToSubstructure.Count / 10; i++)
			{
				List<IntVec3> list = GridShapeMaker.IrregularLumpRelative(Rand.Range(50, 180));
				IntVec3 b = cellsAdjacentToSubstructure.RandomElement<IntVec3>();
				foreach (IntVec3 a in list)
				{
					hashSet.Add(a + b);
				}
			}
			HashSet<IntVec3> hashSet2 = new HashSet<IntVec3>();
			hashSet2.AddRange(cellsAdjacentToSubstructure);
			hashSet2.AddRange(hashSet);
			List<IntVec3> list2 = new List<IntVec3>();
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in gravship.ThrusterPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				thing2.def.GetCompProperties<CompProperties_GravshipThruster>().GetExclusionZone(data2.local, data2.rotation, ref list2);
				hashSet2.AddRange(list2);
			}
			bool enabled = map.regionAndRoomUpdater.Enabled;
			map.regionAndRoomUpdater.Enabled = true;
			if (!MapGenerator.PlayerStartSpotValid)
			{
				GenStep_ReserveGravshipArea.SetStartSpot(map, hashSet2, orGenerateVar);
			}
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			foreach (CellRect cellRect in gravship.OccupiedRects)
			{
				orGenerateVar.Add(cellRect.MovedBy(playerStartSpot).ExpandedBy(2));
			}
			GravshipPlacementUtility.ClearArea(map, playerStartSpot, cellsAdjacentToSubstructure, GravshipPlacementUtility.ClearMode.AllButNonTreePlants);
			GravshipPlacementUtility.ClearArea(map, playerStartSpot, hashSet, GravshipPlacementUtility.ClearMode.NaturalRockOnly);
			map.regionAndRoomUpdater.Enabled = enabled;
		}

		// Token: 0x0600B4C6 RID: 46278 RVA: 0x0033DF6C File Offset: 0x0033C16C
		public static void SetStartSpot(Map map, HashSet<IntVec3> cells, List<CellRect> usedRects)
		{
			int i = 0;
			int num = int.MaxValue;
			IntVec3 intVec = IntVec3.Invalid;
			while (i < 50)
			{
				IntVec3 intVec2 = CellFinderLoose.TryFindCentralCell(map, 2, 30, null, true);
				if (intVec2.IsValid)
				{
					int num2 = GenStep_ReserveGravshipArea.CostToPlaceAt(intVec2, cells, map, usedRects);
					if (num2 < num)
					{
						num = num2;
						intVec = intVec2;
						if (num == 0)
						{
							break;
						}
					}
				}
				i++;
			}
			if (num > cells.Count * 50)
			{
				Predicate<IntVec3> <>9__0;
				for (i = 0; i < 100; i++)
				{
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 c) => !usedRects.Any((CellRect r) => r.Contains(c))));
					}
					IntVec3 intVec3;
					if (CellFinder.TryFindRandomCell(map, validator, out intVec3))
					{
						int num3 = GenStep_ReserveGravshipArea.CostToPlaceAt(intVec3, cells, map, usedRects);
						if (num3 < num)
						{
							num = num3;
							intVec = intVec3;
							if (num == 0)
							{
								break;
							}
						}
					}
				}
			}
			if (intVec != IntVec3.Invalid)
			{
				MapGenerator.PlayerStartSpot = intVec;
			}
		}

		// Token: 0x0600B4C7 RID: 46279 RVA: 0x0033E04C File Offset: 0x0033C24C
		private static int CostToPlaceAt(IntVec3 center, HashSet<IntVec3> cells, Map map, List<CellRect> usedRects)
		{
			int num = 0;
			int num2 = 0;
			foreach (IntVec3 b in cells)
			{
				IntVec3 cell = center + b;
				if (!cell.InBounds(map) || cell.InNoBuildEdgeArea(map) || map.landingBlockers.Any((CellRect x) => x.Contains(cell)))
				{
					return int.MaxValue;
				}
				if (usedRects.Any((CellRect x) => x.Contains(cell)))
				{
					num2 += 100000;
				}
				if (cell.GetRoof(map) != null)
				{
					num2 += 20;
				}
				if (cell.Fogged(map))
				{
					num2 += 20;
				}
				Building edifice = cell.GetEdifice(map);
				if (edifice != null)
				{
					num2 += (edifice.def.building.isNaturalRock ? 80 : 40);
				}
				else
				{
					TerrainDef terrain = cell.GetTerrain(map);
					if (terrain == TerrainDefOf.Space)
					{
						num++;
						num2 += 5;
					}
					else if (terrain.passability == Traversability.Impassable)
					{
						num2 += 200;
					}
					else if (terrain.dangerous)
					{
						num2 += 80;
					}
					else if (terrain.IsWater)
					{
						num2 += 60;
					}
					else if (!cell.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy) && !cell.GetAffordances(map).Contains(TerrainAffordanceDefOf.Medium) && !cell.GetAffordances(map).Contains(TerrainAffordanceDefOf.Light))
					{
						num2 += 30;
					}
				}
			}
			float num3 = (float)num / (float)cells.Count;
			if (num == cells.Count)
			{
				num2 += 10000;
			}
			else if (num3 > 0.9f)
			{
				num2 += 1000;
			}
			return num2;
		}

		// Token: 0x04007BF7 RID: 31735
		private const int ClearEdgeRadius = 2;
	}
}
