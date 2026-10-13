using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EDE RID: 7902
	public class RoomContents_InnerCourtyard : RoomContentsWorker
	{
		// Token: 0x0600AF05 RID: 44805 RVA: 0x0031EE50 File Offset: 0x0031D050
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			CellRect rect = room.rects[0];
			foreach (IntVec3 cell in rect.ContractedBy(3).EdgeCells)
			{
				if (!RoomContents_InnerCourtyard.ContainedWithinOtherRect(rect, cell, room, 1))
				{
					RoomContents_InnerCourtyard.SpawnWall(cell, map, room);
				}
			}
			foreach (IntVec3 intVec in rect.ContractedBy(4).Cells)
			{
				if (!RoomContents_InnerCourtyard.ContainedWithinOtherRect(rect, intVec, room, 1))
				{
					TerrainDef newTerr = map.Biome.inVacuum ? TerrainDefOf.Space : TerrainDefOf.PackedDirt;
					map.terrainGrid.SetTerrain(intVec, newTerr);
					map.roofGrid.SetRoof(intVec, null);
				}
			}
			RoomContents_InnerCourtyard.TryDivideRoom(map, room, rect);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AF06 RID: 44806 RVA: 0x0031EF58 File Offset: 0x0031D158
		private static void TryDivideRoom(Map map, LayoutRoom room, CellRect rect)
		{
			CellRect cellRect = rect.ContractedBy(3);
			for (int i = 0; i < 2; i++)
			{
				Rot4 rot = (i == 0) ? Rot4.North : Rot4.East;
				Rot4 rot2 = rot.Rotated(RotationDirection.Clockwise);
				if (cellRect.GetSideLength(rot) > 8)
				{
					IntVec3 intVec = cellRect.GetCenterCellOnEdge(rot.Opposite);
					CellRect cellRect2 = CellRect.FromLimits(cellRect.GetCenterCellOnEdge(rot2.Opposite), cellRect.GetCenterCellOnEdge(rot2));
					while (cellRect.Contains(intVec))
					{
						map.terrainGrid.SetTerrain(intVec, room.sketch.layoutSketch.floor);
						map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
						Building edifice = intVec.GetEdifice(map);
						if (edifice != null)
						{
							edifice.Destroy(DestroyMode.Vanish);
						}
						for (int j = 0; j < 2; j++)
						{
							IntVec3 intVec2 = intVec + ((j == 0) ? rot2 : rot2.Opposite).FacingCell;
							if (!cellRect2.Contains(intVec2) || cellRect.GetSideLength(rot2) <= 8)
							{
								RoomContents_InnerCourtyard.SpawnWall(intVec2, map, room);
							}
						}
						intVec += rot.FacingCell;
					}
				}
			}
		}

		// Token: 0x0600AF07 RID: 44807 RVA: 0x0031F08B File Offset: 0x0031D28B
		protected override bool IsValidCellBase(ThingDef thing, ThingDef stuff, IntVec3 cell, LayoutRoom room, Map map)
		{
			return map.roofGrid.RoofAt(cell) != null && base.IsValidCellBase(thing, stuff, cell, room, map);
		}

		// Token: 0x0600AF08 RID: 44808 RVA: 0x0031F0AB File Offset: 0x0031D2AB
		private static void SpawnWall(IntVec3 cell, Map map, LayoutRoom room)
		{
			map.terrainGrid.SetTerrain(cell, room.sketch.layoutSketch.floor);
			map.roofGrid.SetRoof(cell, RoofDefOf.RoofConstructed);
			GenSpawn.Spawn(ThingDefOf.AncientFortifiedWall, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600AF09 RID: 44809 RVA: 0x0031F0E8 File Offset: 0x0031D2E8
		private static bool ContainedWithinOtherRect(CellRect rect, IntVec3 cell, LayoutRoom room, int contractedBy)
		{
			return rect.EdgeCells.Contains(cell);
		}
	}
}
