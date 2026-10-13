using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EE5 RID: 7909
	public class RoomContents_NarrowHalls : RoomContentsWorker
	{
		// Token: 0x0600AF29 RID: 44841 RVA: 0x0031F991 File Offset: 0x0031DB91
		public override void PreFillRooms(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContents_NarrowHalls.ComputePathways(this.GetPathwayList(room), map, room);
		}

		// Token: 0x0600AF2A RID: 44842 RVA: 0x0031F9A4 File Offset: 0x0031DBA4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			CellRect rect = room.rects[0];
			List<IntVec3> pathwayList = this.GetPathwayList(room);
			this.SpawnPathways(pathwayList, map, room);
			RoomContents_NarrowHalls.FillSpace(pathwayList, map, rect);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AF2B RID: 44843 RVA: 0x0031F9E4 File Offset: 0x0031DBE4
		private List<IntVec3> GetPathwayList(LayoutRoom room)
		{
			List<IntVec3> list;
			if (!RoomContents_NarrowHalls.pathwayLists.TryGetValue(room.GetHashCode(), out list))
			{
				list = new List<IntVec3>();
				RoomContents_NarrowHalls.pathwayLists[room.GetHashCode()] = list;
			}
			return list;
		}

		// Token: 0x0600AF2C RID: 44844 RVA: 0x0031FA1D File Offset: 0x0031DC1D
		public override void PostFillRooms(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.PostFillRooms(map, room, faction, threatPoints);
			RoomContents_NarrowHalls.pathwayLists.Clear();
		}

		// Token: 0x0600AF2D RID: 44845 RVA: 0x0031FA34 File Offset: 0x0031DC34
		protected override bool CanRemoveWall(IntVec3 cell, Map map, LayoutRoom room)
		{
			foreach (KeyValuePair<int, List<IntVec3>> keyValuePair in RoomContents_NarrowHalls.pathwayLists)
			{
				int num;
				List<IntVec3> list;
				keyValuePair.Deconstruct(out num, out list);
				foreach (IntVec3 other in list)
				{
					if (cell.AdjacentToDiagonal(other))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600AF2E RID: 44846 RVA: 0x0031FAD8 File Offset: 0x0031DCD8
		protected override bool IsValidCellBase(ThingDef thing, ThingDef stuff, IntVec3 cell, LayoutRoom room, Map map)
		{
			return cell.Roofed(map) && base.IsValidCellBase(thing, stuff, cell, room, map);
		}

		// Token: 0x0600AF2F RID: 44847 RVA: 0x0031FAF3 File Offset: 0x0031DCF3
		protected override bool IsValidWallAttachmentCell(LayoutWallAttatchmentParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			return this.GetPathwayList(room).Contains(cell) && base.IsValidWallAttachmentCell(parms, cell, rot, room, map);
		}

		// Token: 0x0600AF30 RID: 44848 RVA: 0x0031FB14 File Offset: 0x0031DD14
		private static void ComputePathways(List<IntVec3> pathway, Map map, LayoutRoom room)
		{
			pathway.Clear();
			List<IntVec3> list = new List<IntVec3>();
			CellRect rect = room.rects[0];
			foreach (IntVec3 intVec in rect.EdgeCells)
			{
				if (intVec.GetDoor(map) != null || intVec.GetEdifice(map) == null)
				{
					list.Add(intVec);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec2 = list[i];
				Rot4 opposite = rect.GetClosestEdge(intVec2).Opposite;
				IntVec3 pos = intVec2 + opposite.FacingCell;
				IntVec3 intVec3 = IntVec3.Invalid;
				int num = 0;
				foreach (IntVec3 intVec4 in list)
				{
					if (!(intVec2 == intVec4))
					{
						Rot4 closestEdge = rect.GetClosestEdge(intVec4);
						if (opposite.IsHorizontal != closestEdge.IsHorizontal)
						{
							int num2 = opposite.IsHorizontal ? (intVec4.x - intVec2.x) : (intVec4.z - intVec2.z);
							num2 = Mathf.Abs(num2);
							if (num2 > num)
							{
								num = num2;
								intVec3 = (opposite.IsHorizontal ? new IntVec3(intVec4.x, 0, intVec2.z) : new IntVec3(intVec2.x, 0, intVec4.z));
							}
						}
					}
				}
				if (intVec3 == IntVec3.Invalid)
				{
					break;
				}
				RoomContents_NarrowHalls.WalkPathway(rect, pos, pathway, intVec3, opposite);
			}
			if (pathway.Empty<IntVec3>())
			{
				RoomContents_NarrowHalls.BackupPathways(list, rect, pathway);
			}
		}

		// Token: 0x0600AF31 RID: 44849 RVA: 0x0031FCE8 File Offset: 0x0031DEE8
		private static void BackupPathways(List<IntVec3> doors, CellRect rect, List<IntVec3> pathway)
		{
			List<IntVec3> list = new List<IntVec3>();
			for (int i = 0; i < doors.Count; i++)
			{
				IntVec3 intVec = doors[i];
				Rot4 dir = rect.GetClosestEdge(intVec).Opposite;
				IntVec3 intVec2 = intVec + dir.FacingCell;
				IntVec3 centerCell = rect.CenterCell;
				IntVec3 intVec3 = dir.IsHorizontal ? new IntVec3(centerCell.x, 0, intVec2.z) : new IntVec3(intVec2.x, 0, centerCell.z);
				if (!list.Contains(intVec3))
				{
					list.Add(intVec3);
				}
				RoomContents_NarrowHalls.WalkPathway(rect, intVec2, pathway, intVec3, dir);
				for (int j = 0; j < list.Count - 1; j++)
				{
					IntVec3 intVec4 = list[j];
					IntVec3 intVec5 = list[j + 1];
					if (!(intVec4 == intVec5))
					{
						if (intVec4.x == intVec5.x)
						{
							dir = ((intVec4.z < intVec5.z) ? Rot4.North : Rot4.South);
						}
						else
						{
							dir = ((intVec4.x < intVec5.x) ? Rot4.East : Rot4.West);
						}
						intVec2 = intVec4 + dir.FacingCell;
						RoomContents_NarrowHalls.WalkPathway(rect, intVec2, pathway, intVec3, dir);
					}
				}
			}
		}

		// Token: 0x0600AF32 RID: 44850 RVA: 0x0031FE44 File Offset: 0x0031E044
		private void SpawnPathways(List<IntVec3> pathway, Map map, LayoutRoom room)
		{
			int num = 5;
			foreach (IntVec3 intVec in pathway)
			{
				base.SpawnRoof(map, intVec);
				foreach (IntVec3 b in GenAdj.AdjacentCells)
				{
					IntVec3 intVec2 = intVec + b;
					base.SpawnRoof(map, intVec2);
					if (!pathway.Contains(intVec2) && intVec2.GetFirstBuilding(map) == null && !room.rects[0].EdgeCells.Contains(intVec2))
					{
						RoomContents_NarrowHalls.SpawnWall(map, intVec2);
					}
				}
				if (--num <= 0)
				{
					num = 5;
					Thing thing;
					RoomGenUtility.TrySpawnWallAttatchment(room.sketch.layoutSketch.WallLampThing, intVec, map, out thing, true, null, null, null);
				}
			}
		}

		// Token: 0x0600AF33 RID: 44851 RVA: 0x0031FF34 File Offset: 0x0031E134
		private static void FillSpace(List<IntVec3> pathway, Map map, CellRect rect)
		{
			CellRect cellRect = rect.ContractedBy(1).ContractedBy(RoomContents_NarrowHalls.PlatformRange.RandomInRange, RoomContents_NarrowHalls.PlatformRange.RandomInRange, RoomContents_NarrowHalls.PlatformRange.RandomInRange, RoomContents_NarrowHalls.PlatformRange.RandomInRange);
			foreach (IntVec3 intVec in rect.ContractedBy(1).Cells)
			{
				if (!pathway.Contains(intVec) && intVec.GetFirstBuilding(map) == null)
				{
					if (cellRect.Contains(intVec))
					{
						TerrainDef newTerr = map.Biome.inVacuum ? TerrainDefOf.Space : TerrainDefOf.PackedDirt;
						map.terrainGrid.SetTerrain(intVec, newTerr);
					}
					map.roofGrid.SetRoof(intVec, null);
				}
			}
		}

		// Token: 0x0600AF34 RID: 44852 RVA: 0x00320024 File Offset: 0x0031E224
		private static void SpawnWall(Map map, IntVec3 cell)
		{
			GenSpawn.Spawn(ThingDefOf.AncientFortifiedWall, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600AF35 RID: 44853 RVA: 0x00320034 File Offset: 0x0031E234
		private static void WalkPathway(CellRect rect, IntVec3 pos, List<IntVec3> pathway, IntVec3 end, Rot4 dir)
		{
			while (rect.ContractedBy(1).Contains(pos))
			{
				pathway.Add(pos);
				if (pos == end)
				{
					break;
				}
				pos += dir.FacingCell;
			}
		}

		// Token: 0x04007851 RID: 30801
		private const int LightEachCells = 5;

		// Token: 0x04007852 RID: 30802
		private static readonly IntRange PlatformRange = new IntRange(1, 2);

		// Token: 0x04007853 RID: 30803
		private static readonly Dictionary<int, List<IntVec3>> pathwayLists = new Dictionary<int, List<IntVec3>>();
	}
}
