using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Utility;

namespace RimWorld
{
	// Token: 0x02001F44 RID: 8004
	public static class RoomLayoutGenerator
	{
		// Token: 0x0600B109 RID: 45321 RVA: 0x00328674 File Offset: 0x00326874
		public static StructureLayout GenerateRandomLayout(LayoutStructureSketch sketch, CellRect container, int minRoomWidth = 6, int minRoomHeight = 6, float areaPrunePercent = 0.2f, bool canRemoveRooms = false, bool generateDoors = true, LayoutRoomDef corridor = null, int corridorExpansion = 2, IntRange? maxMergeRoomsRange = null, CorridorShape corridorShapes = CorridorShape.All, bool canDisconnectRooms = true, bool singleRoom = false)
		{
			return RoomLayoutGenerator.GenerateRoomLayout(new RoomLayoutParams
			{
				sketch = sketch,
				container = container,
				areaPrunePercent = areaPrunePercent,
				minRoomHeight = Mathf.Max(minRoomWidth, 3),
				minRoomWidth = Mathf.Max(minRoomHeight, 3),
				canRemoveRooms = canRemoveRooms,
				generateDoors = generateDoors,
				corridor = corridor,
				corridorExpansion = corridorExpansion,
				maxMergeRoomsRange = maxMergeRoomsRange,
				corridorShapes = corridorShapes,
				canDisconnectRooms = canDisconnectRooms,
				singleRoom = singleRoom
			});
		}

		// Token: 0x0600B10A RID: 45322 RVA: 0x0032870C File Offset: 0x0032690C
		public static StructureLayout GenerateRandomLayout(CellRect container, int minRoomWidth = 6, int minRoomHeight = 6, float areaPrunePercent = 0.2f, bool canRemoveRooms = false, bool generateDoors = true, LayoutRoomDef corridor = null, int corridorExpansion = 2, IntRange? maxMergeRoomsRange = null, CorridorShape corridorShapes = CorridorShape.All, bool canDisconnectRooms = true, bool singleRoom = false)
		{
			return RoomLayoutGenerator.GenerateRoomLayout(new RoomLayoutParams
			{
				container = container,
				areaPrunePercent = areaPrunePercent,
				minRoomHeight = Mathf.Max(minRoomWidth, 3),
				minRoomWidth = Mathf.Max(minRoomHeight, 3),
				canRemoveRooms = canRemoveRooms,
				generateDoors = generateDoors,
				corridor = corridor,
				corridorExpansion = corridorExpansion,
				maxMergeRoomsRange = maxMergeRoomsRange,
				corridorShapes = corridorShapes,
				canDisconnectRooms = canDisconnectRooms,
				singleRoom = singleRoom
			});
		}

		// Token: 0x0600B10B RID: 45323 RVA: 0x0032879C File Offset: 0x0032699C
		private static StructureLayout GenerateRoomLayout(RoomLayoutParams parms)
		{
			StructureLayout structureLayout = new StructureLayout(parms.sketch, parms.container);
			if (parms.singleRoom)
			{
				structureLayout.AddRoom(new List<CellRect>
				{
					parms.container
				}, null);
			}
			else
			{
				if (parms.corridor == null)
				{
					RoomLayoutGenerator.SplitRandom(parms.container, RoomLayoutGenerator.tmpRoomRects, parms.minRoomWidth, parms.minRoomHeight);
					RoomLayoutGenerator.MergeRandom(RoomLayoutGenerator.tmpRoomRects, RoomLayoutGenerator.tmpMergedRoomRects, parms.maxMergeRoomsRange ?? RoomLayoutGenerator.DefaultMaxMergedRoomsRange, 5);
					using (List<List<CellRect>>.Enumerator enumerator = RoomLayoutGenerator.tmpMergedRoomRects.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							List<CellRect> rects = enumerator.Current;
							structureLayout.AddRoom(rects, null);
						}
						goto IL_1B2;
					}
				}
				RoomLayoutGenerator.SplitCorridor(parms.container, RoomLayoutGenerator.tmpCorridors, RoomLayoutGenerator.tmpSpaces, parms.corridorExpansion, parms.minRoomWidth, parms.minRoomHeight, parms.corridorShapes);
				RoomLayoutGenerator.MergeAddCorridors(parms, structureLayout);
				foreach (CellRect rectToSplit in RoomLayoutGenerator.tmpSpaces)
				{
					RoomLayoutGenerator.SplitRandom(rectToSplit, RoomLayoutGenerator.tmpRoomRects, parms.minRoomWidth, parms.minRoomHeight);
					RoomLayoutGenerator.MergeRandom(RoomLayoutGenerator.tmpRoomRects, RoomLayoutGenerator.tmpMergedRoomRects, parms.maxMergeRoomsRange ?? RoomLayoutGenerator.DefaultMaxMergedRoomsRange, 5);
					foreach (List<CellRect> rects2 in RoomLayoutGenerator.tmpMergedRoomRects)
					{
						structureLayout.AddRoom(rects2, null);
					}
				}
			}
			IL_1B2:
			float num = (float)structureLayout.Area - parms.areaPrunePercent * (float)structureLayout.Area;
			int num2 = 100;
			int num3 = 0;
			int num4 = structureLayout.Rooms.Count / 3;
			while (parms.canRemoveRooms && structureLayout.Rooms.Count > 4 && num3 < num4 && (float)structureLayout.Area > num && num2-- > 0)
			{
				LayoutRoom layoutRoom = structureLayout.Rooms.RandomElement<LayoutRoom>();
				if (layoutRoom.requiredDef == null && (parms.canDisconnectRooms || structureLayout.CanRemoveWithoutDisconnection(layoutRoom)))
				{
					structureLayout.RemoveRoom(layoutRoom);
					num3++;
				}
			}
			num2 = 0;
			while ((float)structureLayout.Area > num && num2-- > 0)
			{
				structureLayout.TryMinimizeLayoutWithoutDisconnection();
			}
			structureLayout.FinalizeRooms(true);
			if (parms.generateDoors)
			{
				RoomLayoutGenerator.CreateDoors(structureLayout, parms.entranceCount);
			}
			return structureLayout;
		}

		// Token: 0x0600B10C RID: 45324 RVA: 0x00328A4C File Offset: 0x00326C4C
		private static void MergeAddCorridors(RoomLayoutParams parms, StructureLayout layout)
		{
			while (RoomLayoutGenerator.tmpCorridors.Any<CellRect>())
			{
				List<CellRect> list = new List<CellRect>
				{
					RoomLayoutGenerator.tmpCorridors[0]
				};
				RoomLayoutGenerator.tmpCorridors.RemoveAt(0);
				for (int i = RoomLayoutGenerator.tmpCorridors.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					for (int j = 0; j < list.Count; j++)
					{
						if (RoomLayoutGenerator.tmpCorridors[i].Overlaps(list[j]))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list.Add(RoomLayoutGenerator.tmpCorridors[i]);
						RoomLayoutGenerator.tmpCorridors.RemoveAt(i);
					}
				}
				LayoutRoom layoutRoom = layout.AddRoom(list, null);
				layoutRoom.requiredDef = parms.corridor;
				layoutRoom.noExteriorDoors = true;
			}
		}

		// Token: 0x0600B10D RID: 45325 RVA: 0x00328B14 File Offset: 0x00326D14
		private static void CreateDoors(StructureLayout layout, int entranceCount)
		{
			HashSet<int> hashSet = new HashSet<int>();
			RoomLayoutGenerator.tmpCells.Clear();
			RoomLayoutGenerator.tmpCells.AddRange(layout.container.Cells.InRandomOrder(null));
			for (int i = 0; i < RoomLayoutGenerator.tmpCells.Count; i++)
			{
				IntVec3 intVec = RoomLayoutGenerator.tmpCells[i];
				if (layout.IsWallAt(intVec))
				{
					if (layout.IsGoodForHorizontalDoor(intVec))
					{
						int roomIdAt = layout.GetRoomIdAt(intVec + IntVec3.North);
						int roomIdAt2 = layout.GetRoomIdAt(intVec + IntVec3.South);
						bool flag = layout.IsOutside(intVec + IntVec3.North) || layout.IsOutside(intVec + IntVec3.South);
						int item = Gen.HashOrderless(roomIdAt, roomIdAt2);
						if (hashSet.Contains(item) || (flag && entranceCount <= 0))
						{
							goto IL_16A;
						}
						layout.Add(intVec, RoomLayoutCellType.Door);
						hashSet.Add(item);
						if (flag)
						{
							entranceCount--;
						}
					}
					if (layout.IsGoodForVerticalDoor(intVec))
					{
						int roomIdAt3 = layout.GetRoomIdAt(intVec + IntVec3.East);
						int roomIdAt4 = layout.GetRoomIdAt(intVec + IntVec3.West);
						bool flag2 = layout.IsOutside(intVec + IntVec3.East) || layout.IsOutside(intVec + IntVec3.West);
						int item2 = Gen.HashOrderless(roomIdAt3, roomIdAt4);
						if (!hashSet.Contains(item2) && (!flag2 || entranceCount > 0))
						{
							layout.Add(intVec, RoomLayoutCellType.Door);
							hashSet.Add(item2);
							if (flag2)
							{
								entranceCount--;
							}
						}
					}
				}
				IL_16A:;
			}
			RoomLayoutGenerator.tmpCells.Clear();
		}

		// Token: 0x0600B10E RID: 45326 RVA: 0x00328CAC File Offset: 0x00326EAC
		private static void SplitCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, int expansion, int minRoomWidth, int minRoomHeight, CorridorShape shapes)
		{
			RoomLayoutGenerator.tmpCorridors.Clear();
			RoomLayoutGenerator.tmpShapes.Clear();
			spaces.Clear();
			foreach (Enum @enum in shapes.GetBitFlags())
			{
				CorridorShape item = (CorridorShape)@enum;
				RoomLayoutGenerator.tmpShapes.Add(item);
			}
			IntVec3 xExpansion = new IntVec3(expansion, 0, 0);
			IntVec3 zExpansion = new IntVec3(0, 0, expansion);
			int xRange = Mathf.Max(rect.Width / 2 - minRoomWidth - expansion - 1, 0);
			int zRange = Mathf.Max(rect.Height / 2 - minRoomHeight - expansion - 1, 0);
			float num;
			if (rect.Width >= rect.Height)
			{
				num = (float)rect.Height / (float)rect.Width;
			}
			else
			{
				num = (float)rect.Width / (float)rect.Height;
			}
			if (RoomLayoutGenerator.tmpShapes.Contains(CorridorShape.H) && (num < 0.5f || rect.Width - expansion * 2 - minRoomWidth * 3 - 2 < 0 || rect.Height - expansion * 2 - minRoomHeight * 3 - 2 < 0))
			{
				RoomLayoutGenerator.tmpShapes.Remove(CorridorShape.H);
			}
			if (RoomLayoutGenerator.tmpShapes.Empty<CorridorShape>())
			{
				Log.Error("Attempted to spawn a corridor with no valid options.");
				return;
			}
			CorridorShape corridorShape = RoomLayoutGenerator.tmpShapes.RandomElement<CorridorShape>();
			if (corridorShape == CorridorShape.Straight)
			{
				RoomLayoutGenerator.GenerateStraightCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			if (corridorShape == CorridorShape.Cross)
			{
				RoomLayoutGenerator.GenerateCrossCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			if (corridorShape == CorridorShape.T)
			{
				RoomLayoutGenerator.GenerateTCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			if (corridorShape == CorridorShape.H)
			{
				RoomLayoutGenerator.GenerateHCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			if (corridorShape == CorridorShape.AsymmetricCross)
			{
				RoomLayoutGenerator.GenerateAsymmetricCrossCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			Log.Error(string.Format("Unhanlded corridor shape {0}", corridorShape));
		}

		// Token: 0x0600B10F RID: 45327 RVA: 0x00328E84 File Offset: 0x00327084
		private static void GenerateStraightCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.East;
			if (rect.Height > rect.Width)
			{
				rot = Rot4.North;
			}
			int offset = (rot == Rot4.East) ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange);
			IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			IntVec3 centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			cellRect = cellRect.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
		}

		// Token: 0x0600B110 RID: 45328 RVA: 0x00328F38 File Offset: 0x00327138
		private static void GenerateCrossCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.East;
			int offset = Rand.Range(-zRange, zRange);
			IntVec3 intVec = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			IntVec3 intVec2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			cellRect = cellRect.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
			rot = Rot4.North;
			IntVec3 point = cellRect.CenterCell + new IntVec3(Rand.Range(-xRange, xRange), 0, 0);
			intVec = rect.GetCellOnEdge(rot.Opposite, point);
			intVec2 = rect.GetCellOnEdge(rot, point);
			CellRect cellRect2 = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, cellRect.minZ);
			CellRect cellRect3 = CellRect.FromLimits(intVec.x, cellRect.maxZ, intVec2.x, intVec2.z);
			cellRect2 = cellRect2.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			cellRect3 = cellRect3.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect2);
			corridors.Add(cellRect3);
			RoomLayoutGenerator.DivideRectsByCorridor(spaces[spaces[0].Overlaps(cellRect2) ? 0 : 1], cellRect2, spaces, rot);
			RoomLayoutGenerator.DivideRectsByCorridor(spaces[spaces[0].Overlaps(cellRect3) ? 0 : 1], cellRect3, spaces, rot);
			spaces.RemoveAt(0);
			spaces.RemoveAt(0);
		}

		// Token: 0x0600B111 RID: 45329 RVA: 0x003290E0 File Offset: 0x003272E0
		private static void GenerateAsymmetricCrossCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.West;
			if (rect.Height > rect.Width)
			{
				rot = Rot4.North;
			}
			int offset = rot.IsHorizontal ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange);
			IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			IntVec3 centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			cellRect = cellRect.ExpandedBy(rot.IsHorizontal ? zExpansion : xExpansion);
			corridors.Add(cellRect);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
			rot = rot.Rotated(RotationDirection.Clockwise);
			offset = (rot.IsHorizontal ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange));
			centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect2;
			if (rot.IsHorizontal)
			{
				cellRect2 = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, cellRect.minX, centerCellOnEdge2.z);
			}
			else
			{
				cellRect2 = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, cellRect.minZ);
			}
			offset = (rot.IsHorizontal ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange));
			centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect3;
			if (rot.IsHorizontal)
			{
				cellRect3 = CellRect.FromLimits(cellRect.maxX, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			}
			else
			{
				cellRect3 = CellRect.FromLimits(centerCellOnEdge.x, cellRect.maxZ, centerCellOnEdge2.x, centerCellOnEdge2.z);
			}
			cellRect2 = cellRect2.ExpandedBy(rot.IsHorizontal ? zExpansion : xExpansion);
			cellRect3 = cellRect3.ExpandedBy(rot.IsHorizontal ? zExpansion : xExpansion);
			corridors.Add(cellRect2);
			corridors.Add(cellRect3);
			RoomLayoutGenerator.DivideRectsByCorridor(spaces[spaces[0].Overlaps(cellRect2) ? 0 : 1], cellRect2, spaces, rot);
			RoomLayoutGenerator.DivideRectsByCorridor(spaces[spaces[0].Overlaps(cellRect3) ? 0 : 1], cellRect3, spaces, rot);
			spaces.RemoveAt(0);
			spaces.RemoveAt(0);
		}

		// Token: 0x0600B112 RID: 45330 RVA: 0x00329338 File Offset: 0x00327538
		private static void GenerateTCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.East;
			if (rect.Height > rect.Width)
			{
				rot = Rot4.North;
			}
			int offset = (rot == Rot4.East) ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange);
			IntVec3 intVec = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			IntVec3 intVec2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			cellRect = cellRect.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
			int i = Mathf.RoundToInt(cellRect.CenterCell.DistanceTo(intVec2)) - ((rot == Rot4.East) ? cellRect.Height : cellRect.Width) - ((rot == Rot4.East) ? zRange : xRange);
			if (Rand.Bool)
			{
				rot = rot.Opposite;
			}
			IntVec3 point = cellRect.CenterCell + rot.FacingCell * i;
			rot = rot.Rotated(RotationDirection.Clockwise);
			intVec = rect.GetCellOnEdge(rot.Opposite, point);
			intVec2 = rect.GetCellOnEdge(rot, point);
			CellRect cellRect2 = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			cellRect2 = cellRect2.ExpandedBy(rot.IsHorizontal ? zExpansion : xExpansion);
			if (rot.IsVertical && cellRect2.CenterCell.x >= rect.CenterCell.x)
			{
				cellRect.maxX = cellRect2.minX;
			}
			else if (rot.IsVertical)
			{
				cellRect.minX = cellRect2.maxX;
			}
			else if (cellRect2.CenterCell.z >= rect.CenterCell.z)
			{
				cellRect.maxZ = cellRect2.minZ;
			}
			else
			{
				cellRect.minZ = cellRect2.maxZ;
			}
			corridors.Add(cellRect);
			corridors.Add(cellRect2);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect2, spaces, rot);
			CellRect value = spaces[0];
			CellRect value2 = spaces[1];
			if (rot.IsVertical && cellRect2.CenterCell.x >= rect.CenterCell.x)
			{
				value.maxX = cellRect2.minX;
				value2.maxX = cellRect2.minX;
				spaces.RemoveAt(2);
			}
			else if (!rot.IsVertical && cellRect2.CenterCell.z >= rect.CenterCell.z)
			{
				value.maxZ = cellRect2.minZ;
				value2.maxZ = cellRect2.minZ;
				spaces.RemoveAt(3);
			}
			else if (rot.IsVertical)
			{
				value.minX = cellRect2.maxX;
				value2.minX = cellRect2.maxX;
				spaces.RemoveAt(2);
			}
			else
			{
				value.minZ = cellRect2.maxZ;
				value2.minZ = cellRect2.maxZ;
				spaces.RemoveAt(3);
			}
			spaces[0] = value;
			spaces[1] = value2;
		}

		// Token: 0x0600B113 RID: 45331 RVA: 0x00329658 File Offset: 0x00327858
		private static void GenerateHCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.East;
			if (rect.Height > rect.Width)
			{
				rot = Rot4.North;
			}
			int num = (rot == Rot4.East) ? zRange : xRange;
			IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, -num);
			IntVec3 centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, -num);
			CellRect cellRect = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			cellRect = cellRect.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
			centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, num);
			centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, num);
			CellRect cellRect2 = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			cellRect2 = cellRect2.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect2);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect2, spaces, rot);
			rot = ((rot == Rot4.East) ? Rot4.North : Rot4.East);
			CellRect cellRect3;
			if (rot == Rot4.East)
			{
				int num2 = cellRect.CenterCell.z + Rand.Range(-zRange, zRange);
				cellRect3 = CellRect.FromLimits(cellRect.maxX, num2, cellRect2.minX, num2);
			}
			else
			{
				int num3 = cellRect.CenterCell.x + Rand.Range(-xRange, xRange);
				cellRect3 = CellRect.FromLimits(num3, cellRect.maxZ, num3, cellRect2.minZ);
			}
			cellRect3 = cellRect3.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect3);
			spaces.RemoveAt(0);
			spaces.RemoveAt(2);
			if (rot == Rot4.East)
			{
				rect.minX = cellRect.maxX;
				rect.maxX = cellRect2.minX;
			}
			else
			{
				rect.minZ = cellRect.maxZ;
				rect.maxZ = cellRect2.minZ;
			}
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect3, spaces, rot);
		}

		// Token: 0x0600B114 RID: 45332 RVA: 0x00329874 File Offset: 0x00327A74
		private static void DivideRectsByCorridor(CellRect rect, CellRect corridor, List<CellRect> spaces, Rot4 corridorRot)
		{
			CellRect item = rect;
			CellRect item2 = rect;
			if (!rect.Overlaps(corridor))
			{
				string format = "Tried to divide rects by corridor but rect and corridor are not overlapping. rect: {0}, corridor: {1}, rot: {2}, \nspaces:\n{3}";
				object[] array = new object[4];
				array[0] = rect;
				array[1] = corridor;
				array[2] = corridorRot;
				array[3] = (from r in spaces
				select r.ToString()).ToLineList(null, false);
				Log.Warning(string.Format(format, array));
			}
			if (corridorRot == Rot4.North)
			{
				item.minX = corridor.maxX;
				item2.maxX = corridor.minX;
			}
			else if (corridorRot == Rot4.East)
			{
				item.minZ = corridor.maxZ;
				item2.maxZ = corridor.minZ;
			}
			else if (corridorRot == Rot4.South)
			{
				item.maxX = corridor.minX;
				item2.minX = corridor.maxX;
			}
			else
			{
				item.maxZ = corridor.minZ;
				item2.minZ = corridor.maxZ;
			}
			spaces.Add(item);
			spaces.Add(item2);
		}

		// Token: 0x0600B115 RID: 45333 RVA: 0x00329994 File Offset: 0x00327B94
		private static void SplitRandom(CellRect rectToSplit, List<CellRect> rooms, int minWidth, int minHeight)
		{
			RoomLayoutGenerator.<>c__DisplayClass19_0 CS$<>8__locals1;
			CS$<>8__locals1.minHeight = minHeight;
			CS$<>8__locals1.minWidth = minWidth;
			rooms.Clear();
			Queue<CellRect> queue = new Queue<CellRect>();
			queue.Enqueue(rectToSplit);
			while (queue.Count > 0)
			{
				CellRect cellRect = queue.Dequeue();
				if (!RoomLayoutGenerator.<SplitRandom>g__CanSplit|19_0(cellRect, ref CS$<>8__locals1))
				{
					rooms.Add(cellRect);
				}
				else if (cellRect.Width > cellRect.Height)
				{
					int num = Rand.Range(CS$<>8__locals1.minWidth, cellRect.Width - CS$<>8__locals1.minWidth);
					CellRect item = new CellRect(cellRect.minX, cellRect.minZ, num, cellRect.Height);
					CellRect item2 = new CellRect(cellRect.minX - 1 + num, cellRect.minZ, cellRect.Width - num + 1, cellRect.Height);
					queue.Enqueue(item);
					queue.Enqueue(item2);
				}
				else
				{
					int num2 = Rand.Range(CS$<>8__locals1.minHeight, cellRect.Height - CS$<>8__locals1.minHeight);
					CellRect item3 = new CellRect(cellRect.minX, cellRect.minZ - 1 + num2, cellRect.Width, cellRect.Height - num2 + 1);
					CellRect item4 = new CellRect(cellRect.minX, cellRect.minZ, cellRect.Width, num2);
					queue.Enqueue(item3);
					queue.Enqueue(item4);
				}
			}
		}

		// Token: 0x0600B116 RID: 45334 RVA: 0x00329AE4 File Offset: 0x00327CE4
		private static void MergeRandom(List<CellRect> rects, List<List<CellRect>> mergedRects, IntRange maxMergedRooms, int minAdjacenyScore = 5)
		{
			mergedRects.Clear();
			rects.Shuffle<CellRect>();
			for (int i = 0; i < rects.Count; i++)
			{
				CellRect cellRect = rects[i];
				if (!RoomLayoutGenerator.UsedInMerge(mergedRects, cellRect))
				{
					List<CellRect> list = new List<CellRect>
					{
						cellRect
					};
					int num = Math.Max(1, maxMergedRooms.RandomInRange);
					int num2 = 0;
					while (num2 < rects.Count && list.Count < num)
					{
						CellRect cellRect2 = rects[num2];
						if (!(cellRect == cellRect2) && !RoomLayoutGenerator.UsedInMerge(mergedRects, cellRect2) && cellRect.GetAdjacencyScore(cellRect2) >= minAdjacenyScore)
						{
							list.Add(cellRect2);
						}
						num2++;
					}
					mergedRects.Add(list);
				}
			}
		}

		// Token: 0x0600B117 RID: 45335 RVA: 0x00329B98 File Offset: 0x00327D98
		private static bool UsedInMerge(List<List<CellRect>> mergedRects, CellRect rect)
		{
			for (int i = 0; i < mergedRects.Count; i++)
			{
				for (int j = 0; j < mergedRects[i].Count; j++)
				{
					if (mergedRects[i][j] == rect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B119 RID: 45337 RVA: 0x00329C3D File Offset: 0x00327E3D
		[CompilerGenerated]
		internal static bool <SplitRandom>g__CanSplit|19_0(CellRect r, ref RoomLayoutGenerator.<>c__DisplayClass19_0 A_1)
		{
			return r.Height > 2 * A_1.minHeight || r.Width > 2 * A_1.minWidth;
		}

		// Token: 0x0400793C RID: 31036
		private static readonly IntRange DefaultMaxMergedRoomsRange = new IntRange(1, 3);

		// Token: 0x0400793D RID: 31037
		private static readonly List<CellRect> tmpRoomRects = new List<CellRect>();

		// Token: 0x0400793E RID: 31038
		private static readonly List<CellRect> tmpCorridors = new List<CellRect>();

		// Token: 0x0400793F RID: 31039
		private static readonly List<CellRect> tmpSpaces = new List<CellRect>();

		// Token: 0x04007940 RID: 31040
		private static readonly List<List<CellRect>> tmpMergedRoomRects = new List<List<CellRect>>();

		// Token: 0x04007941 RID: 31041
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04007942 RID: 31042
		private static readonly List<CorridorShape> tmpShapes = new List<CorridorShape>();
	}
}
