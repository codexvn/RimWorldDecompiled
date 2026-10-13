using System;
using System.Collections.Generic;
using DelaunatorSharp;
using RimWorld;
using UnityEngine;
using Verse;

// Token: 0x02000028 RID: 40
public class LayoutWorker_Labyrinth : LayoutWorker
{
	// Token: 0x06000094 RID: 148 RVA: 0x000052FD File Offset: 0x000034FD
	public LayoutWorker_Labyrinth(LayoutDef def) : base(def)
	{
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00005308 File Offset: 0x00003508
	protected override LayoutSketch GenerateSketch(StructureGenParams parms)
	{
		if (!ModLister.CheckAnomaly("Labyrinth"))
		{
			return null;
		}
		LayoutSketch layoutSketch = new LayoutSketch
		{
			wall = ThingDefOf.GrayWall,
			door = ThingDefOf.GrayDoor,
			floor = TerrainDefOf.GraySurface,
			defaultAffordanceTerrain = TerrainDefOf.GraySurface,
			wallStuff = ThingDefOf.LabyrinthMatter,
			doorStuff = ThingDefOf.LabyrinthMatter
		};
		using (new ProfilerBlock("Generate Labyrinth"))
		{
			layoutSketch.structureLayout = this.GenerateLabyrinth(parms);
		}
		return layoutSketch;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x000053A8 File Offset: 0x000035A8
	private StructureLayout GenerateLabyrinth(StructureGenParams parms)
	{
		CellRect cellRect = new CellRect(0, 0, parms.size.x, parms.size.z);
		StructureLayout structureLayout = new StructureLayout(parms.sketch, cellRect);
		LayoutWorker_Labyrinth.PlaceObeliskRoom(cellRect, structureLayout);
		using (new ProfilerBlock("Scatter L Rooms"))
		{
			LayoutWorker_Labyrinth.ScatterLRooms(cellRect, structureLayout);
		}
		using (new ProfilerBlock("Scatter Square Rooms"))
		{
			LayoutWorker_Labyrinth.ScatterSquareRooms(cellRect, structureLayout);
		}
		using (new ProfilerBlock("Generate Graphs"))
		{
			LayoutWorker_Labyrinth.GenerateGraphs(structureLayout);
		}
		structureLayout.FinalizeRooms(false);
		using (new ProfilerBlock("Create Doors"))
		{
			LayoutWorker_Labyrinth.CreateDoors(structureLayout);
		}
		using (new ProfilerBlock("Create Corridors"))
		{
			LayoutWorker_Labyrinth.CreateCorridorsAStar(structureLayout);
		}
		using (new ProfilerBlock("Fill Empty Spaces"))
		{
			LayoutWorker_Labyrinth.FillEmptySpaces(structureLayout);
		}
		return structureLayout;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000550C File Offset: 0x0000370C
	private static void FillEmptySpaces(StructureLayout layout)
	{
		HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
		foreach (IntVec3 intVec in layout.container.Cells)
		{
			if (layout.IsEmptyAt(intVec) && !hashSet.Contains(intVec))
			{
				foreach (IntVec3 position in GenAdjFast.AdjacentCells8Way(intVec))
				{
					if (layout.IsWallAt(position))
					{
						hashSet.Add(intVec);
						break;
					}
				}
			}
		}
		foreach (IntVec3 position2 in hashSet)
		{
			layout.Add(position2, RoomLayoutCellType.Wall);
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00005600 File Offset: 0x00003800
	private static void GenerateGraphs(StructureLayout layout)
	{
		List<Vector2> list = new List<Vector2>();
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			Vector3 vector = Vector3.zero;
			foreach (CellRect cellRect in layoutRoom.rects)
			{
				vector += cellRect.CenterVector3;
			}
			vector /= (float)layoutRoom.rects.Count;
			list.Add(new Vector2(vector.x, vector.z));
		}
		layout.delaunator = new Delaunator(list.ToArray());
		layout.neighbours = new RelativeNeighborhoodGraph(layout.delaunator);
	}

	// Token: 0x06000099 RID: 153 RVA: 0x000056F0 File Offset: 0x000038F0
	private static void PlaceObeliskRoom(CellRect size, StructureLayout layout)
	{
		int minX = Rand.Range(0, size.Width - 19);
		int minZ = Rand.Range(0, size.Height - 19);
		CellRect item = new CellRect(minX, minZ, 19, 19);
		LayoutRoom layoutRoom = layout.AddRoom(new List<CellRect>
		{
			item
		}, null);
		layoutRoom.requiredDef = LayoutRoomDefOf.LabyrinthObelisk;
		layoutRoom.entryCells = new List<IntVec3>();
		layoutRoom.entryCells.AddRange(item.GetCenterCellsOnEdge(Rot4.North, 2));
		layoutRoom.entryCells.AddRange(item.GetCenterCellsOnEdge(Rot4.East, 2));
		layoutRoom.entryCells.AddRange(item.GetCenterCellsOnEdge(Rot4.South, 2));
		layoutRoom.entryCells.AddRange(item.GetCenterCellsOnEdge(Rot4.West, 2));
	}

	// Token: 0x0600009A RID: 154 RVA: 0x000057B4 File Offset: 0x000039B4
	private static void ScatterLRooms(CellRect size, StructureLayout layout)
	{
		int randomInRange = LayoutWorker_Labyrinth.LShapeRoomRange.RandomInRange;
		int num = 0;
		int num2 = 0;
		while (num2 < 100 && num < randomInRange)
		{
			int randomInRange2 = LayoutWorker_Labyrinth.RoomSizeRange.RandomInRange;
			int randomInRange3 = LayoutWorker_Labyrinth.RoomSizeRange.RandomInRange;
			int minX = Rand.Range(0, size.Width - randomInRange2);
			int minZ = Rand.Range(0, size.Height - randomInRange3);
			int randomInRange4 = LayoutWorker_Labyrinth.LShapeRoomRange.RandomInRange;
			int randomInRange5 = LayoutWorker_Labyrinth.LShapeRoomRange.RandomInRange;
			while (Mathf.Abs(randomInRange4 - randomInRange2) <= 2)
			{
				randomInRange4 = LayoutWorker_Labyrinth.LShapeRoomRange.RandomInRange;
			}
			while (Mathf.Abs(randomInRange5 - randomInRange3) <= 2)
			{
				randomInRange5 = LayoutWorker_Labyrinth.LShapeRoomRange.RandomInRange;
			}
			CellRect cellRect = new CellRect(minX, minZ, randomInRange2, randomInRange3);
			CellRect cellRect2;
			if (Rand.Bool)
			{
				cellRect2 = new CellRect(cellRect.maxX, cellRect.maxZ - randomInRange5 + 1, randomInRange4, randomInRange5);
			}
			else
			{
				cellRect2 = new CellRect(cellRect.minX - randomInRange4, cellRect.minZ, randomInRange4 + 1, randomInRange5);
			}
			if (cellRect2.Width >= 4 && cellRect2.Height >= 4 && size.FullyContainedWithin(cellRect2) && !LayoutWorker_Labyrinth.OverlapsWithAnyRoom(layout, cellRect) && !LayoutWorker_Labyrinth.OverlapsWithAnyRoom(layout, cellRect2))
			{
				layout.AddRoom(new List<CellRect>
				{
					cellRect,
					cellRect2
				}, null);
				num++;
			}
			num2++;
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0000592C File Offset: 0x00003B2C
	private static void ScatterSquareRooms(CellRect size, StructureLayout layout)
	{
		int randomInRange = LayoutWorker_Labyrinth.RoomRange.RandomInRange;
		int num = 0;
		int num2 = 0;
		while (num2 < 300 && num < randomInRange)
		{
			int randomInRange2 = LayoutWorker_Labyrinth.RoomSizeRange.RandomInRange;
			int randomInRange3 = LayoutWorker_Labyrinth.RoomSizeRange.RandomInRange;
			int minX = Rand.Range(0, size.Width - randomInRange2);
			int minZ = Rand.Range(0, size.Height - randomInRange3);
			CellRect cellRect = new CellRect(minX, minZ, randomInRange2, randomInRange3);
			if (!LayoutWorker_Labyrinth.OverlapsWithAnyRoom(layout, cellRect))
			{
				layout.AddRoom(new List<CellRect>
				{
					cellRect
				}, null);
				num++;
			}
			num2++;
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x000059D4 File Offset: 0x00003BD4
	private static void CreateCorridorsAStar(StructureLayout layout)
	{
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			foreach (ValueTuple<LayoutRoom, CellRect, CellRect> valueTuple in layout.GetLogicalRoomConnections(layoutRoom))
			{
				LayoutRoom item = valueTuple.Item1;
				if (!layoutRoom.connections.Contains(item))
				{
					LayoutWorker_Labyrinth.ConnectRooms(layout, layoutRoom, item);
				}
			}
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00005A74 File Offset: 0x00003C74
	private static void ConnectRooms(StructureLayout layout, LayoutRoom a, LayoutRoom b)
	{
		PriorityQueue<ValueTuple<IntVec3, IntVec3>, int> priorityQueue = new PriorityQueue<ValueTuple<IntVec3, IntVec3>, int>();
		foreach (CellRect cellRect in a.rects)
		{
			foreach (CellRect cellRect2 in b.rects)
			{
				IEnumerable<IntVec3> entryCells = a.entryCells;
				foreach (IntVec3 intVec in (entryCells ?? cellRect.EdgeCells))
				{
					if (!cellRect.IsCorner(intVec) && !cellRect2.Contains(intVec))
					{
						Rot4 closestEdge = cellRect.GetClosestEdge(intVec);
						entryCells = b.entryCells;
						foreach (IntVec3 intVec2 in (entryCells ?? cellRect2.EdgeCells))
						{
							if (!cellRect2.IsCorner(intVec2) && !cellRect.Contains(intVec2))
							{
								Rot4 closestEdge2 = cellRect2.GetClosestEdge(intVec2);
								int num = (intVec2 - intVec).LengthManhattan;
								RotationDirection relativeRotation = Rot4.GetRelativeRotation(closestEdge, closestEdge2);
								if (closestEdge == Rot4.East && intVec2.x < cellRect.maxX)
								{
									num += 4;
								}
								else if (closestEdge == Rot4.West && intVec2.x > cellRect.minX)
								{
									num += 4;
								}
								if (closestEdge == Rot4.North && intVec2.z < cellRect.maxZ)
								{
									num += 4;
								}
								else if (closestEdge == Rot4.South && intVec2.z > cellRect.minZ)
								{
									num += 4;
								}
								if (relativeRotation == RotationDirection.Clockwise || relativeRotation == RotationDirection.Counterclockwise)
								{
									num++;
								}
								else if (relativeRotation == RotationDirection.None)
								{
									num += 2;
								}
								priorityQueue.Enqueue(new ValueTuple<IntVec3, IntVec3>(intVec, intVec2), num);
							}
						}
					}
				}
			}
		}
		ValueTuple<IntVec3, IntVec3> valueTuple;
		int num2;
		while (priorityQueue.TryDequeue(out valueTuple, out num2))
		{
			IntVec3 item = valueTuple.Item1;
			IntVec3 item2 = valueTuple.Item2;
			List<IntVec3> list;
			if (LayoutWorker_Labyrinth.TryGetPath(layout, item, item2, num2 * 2, out list))
			{
				IntVec3 intVec3 = item2 - item;
				if (Mathf.Max(Mathf.Abs(intVec3.x), Mathf.Abs(intVec3.z)) <= 4)
				{
					layout.Add(item, RoomLayoutCellType.Floor);
					layout.Add(item2, RoomLayoutCellType.Floor);
					LayoutWorker_Labyrinth.InflatePath(layout, list, 1);
					int index = 1;
					if (list.Count == 1 || !layout.IsGoodForDoor(list[index]))
					{
						index = 0;
					}
					if (a.requiredDef == LayoutRoomDefOf.LabyrinthObelisk)
					{
						layout.Add(item, RoomLayoutCellType.Door);
					}
					else if (b.requiredDef == LayoutRoomDefOf.LabyrinthObelisk)
					{
						layout.Add(item2, RoomLayoutCellType.Door);
					}
					else
					{
						layout.Add(list[index], RoomLayoutCellType.Door);
					}
				}
				else
				{
					layout.Add(item, RoomLayoutCellType.Door);
					layout.Add(item2, RoomLayoutCellType.Door);
					LayoutWorker_Labyrinth.InflatePath(layout, list, Mathf.Min(Mathf.Max(1, Mathf.CeilToInt((float)list.Count / 3f)), 3));
				}
				a.connections.Add(b);
				b.connections.Add(a);
				return;
			}
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00005E38 File Offset: 0x00004038
	private static void InflatePath(StructureLayout layout, List<IntVec3> cells, int levels)
	{
		Queue<IntVec3> queue = new Queue<IntVec3>();
		HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
		IntVec3 intVec = cells[0];
		IntVec3 last = cells.GetLast<IntVec3>();
		IntVec3 intVec2 = new IntVec3(Mathf.Min(intVec.x, last.x), 0, Mathf.Min(intVec.z, last.z));
		IntVec3 intVec3 = new IntVec3(Mathf.Max(intVec.x, last.x), 0, Mathf.Max(intVec.z, last.z));
		CellRect cellRect = new CellRect
		{
			minX = intVec2.x,
			minZ = intVec2.z,
			maxX = intVec3.x,
			maxZ = intVec3.z
		};
		cellRect = cellRect.ExpandedBy(levels);
		using (List<IntVec3>.Enumerator enumerator = cells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				IntVec3 intVec4 = enumerator.Current;
				if (layout.IsEmptyAt(intVec4))
				{
					queue.Enqueue(intVec4);
					break;
				}
			}
			goto IL_19B;
		}
		IL_103:
		IntVec3 intVec5 = queue.Dequeue();
		bool flag = cellRect.IsOnEdge(intVec5) && !cells.Contains(intVec5);
		layout.Add(intVec5, (!flag) ? RoomLayoutCellType.Floor : RoomLayoutCellType.Wall);
		if (!flag)
		{
			foreach (IntVec3 intVec6 in LayoutWorker_Labyrinth.Neighbours8Way(layout, intVec5))
			{
				if (layout.IsEmptyAt(intVec6) && cellRect.Contains(intVec6) && !hashSet.Contains(intVec6))
				{
					queue.Enqueue(intVec6);
					hashSet.Add(intVec6);
				}
			}
		}
		IL_19B:
		if (queue.Count == 0)
		{
			bool flag2;
			do
			{
				flag2 = false;
				foreach (IntVec3 intVec7 in hashSet)
				{
					if (!layout.IsWallAt(intVec7) && !cells.Contains(intVec7) && LayoutWorker_Labyrinth.CountAdjacentWalls(layout, intVec7) == 3)
					{
						layout.Add(intVec7, RoomLayoutCellType.Wall);
						flag2 = true;
					}
				}
			}
			while (flag2);
			foreach (IntVec3 intVec8 in cellRect.ExpandedBy(1).EdgeCells)
			{
				LayoutRoom layoutRoom;
				if (intVec8.x > 0 && intVec8.z > 0 && intVec8.x < layout.Width && intVec8.z < layout.Height && !layout.TryGetRoom(intVec8, out layoutRoom) && layout.IsEmptyAt(intVec8))
				{
					layout.Add(intVec8, RoomLayoutCellType.Wall);
				}
			}
			return;
		}
		goto IL_103;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00006108 File Offset: 0x00004308
	private static int CountAdjacentWalls(StructureLayout layout, IntVec3 cell)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			IntVec3 intVec = cell + new Rot4(i).FacingCell;
			if (intVec.x > 0 && intVec.z > 0 && intVec.x < layout.Width && intVec.z < layout.Height && layout.IsWallAt(intVec))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00006174 File Offset: 0x00004374
	private static List<IntVec3> ReconstructPath(Dictionary<IntVec3, IntVec3> from, IntVec3 current)
	{
		List<IntVec3> list = new List<IntVec3>
		{
			current
		};
		while (from.ContainsKey(current))
		{
			current = from[current];
			list.Add(current);
		}
		list.Reverse();
		return list;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x000061B0 File Offset: 0x000043B0
	private static void ResetPathVars()
	{
		LayoutWorker_Labyrinth.openSet.Clear();
		LayoutWorker_Labyrinth.cameFrom.Clear();
		LayoutWorker_Labyrinth.gScore.Clear();
		LayoutWorker_Labyrinth.fScore.Clear();
		LayoutWorker_Labyrinth.toEnqueue.Clear();
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x000061E4 File Offset: 0x000043E4
	private static bool TryGetPath(StructureLayout layout, IntVec3 start, IntVec3 goal, int max, out List<IntVec3> path)
	{
		LayoutWorker_Labyrinth.ResetPathVars();
		LayoutWorker_Labyrinth.gScore.Add(start, 0);
		LayoutWorker_Labyrinth.fScore.Add(start, LayoutWorker_Labyrinth.Heuristic(start, goal));
		LayoutWorker_Labyrinth.openSet.Enqueue(start, LayoutWorker_Labyrinth.fScore[start]);
		Comparison<IntVec3> <>9__0;
		while (LayoutWorker_Labyrinth.openSet.Count != 0)
		{
			IntVec3 intVec = LayoutWorker_Labyrinth.openSet.Dequeue();
			if (intVec == goal)
			{
				path = LayoutWorker_Labyrinth.ReconstructPath(LayoutWorker_Labyrinth.cameFrom, intVec);
				LayoutWorker_Labyrinth.ResetPathVars();
				return true;
			}
			LayoutWorker_Labyrinth.toEnqueue.Clear();
			foreach (IntVec3 intVec2 in LayoutWorker_Labyrinth.Neighbours(layout, intVec, goal))
			{
				if (intVec2 == goal)
				{
					LayoutWorker_Labyrinth.cameFrom[intVec2] = intVec;
					path = LayoutWorker_Labyrinth.ReconstructPath(LayoutWorker_Labyrinth.cameFrom, intVec2);
					LayoutWorker_Labyrinth.ResetPathVars();
					return true;
				}
				int num = LayoutWorker_Labyrinth.gScore[intVec] + 1;
				if (num > max)
				{
					break;
				}
				if (!LayoutWorker_Labyrinth.gScore.ContainsKey(intVec2) || num < LayoutWorker_Labyrinth.gScore[intVec2])
				{
					LayoutWorker_Labyrinth.cameFrom[intVec2] = intVec;
					LayoutWorker_Labyrinth.gScore[intVec2] = num;
					LayoutWorker_Labyrinth.fScore[intVec2] = num + LayoutWorker_Labyrinth.Heuristic(intVec2, goal);
					LayoutWorker_Labyrinth.toEnqueue.Add(intVec2);
				}
			}
			List<IntVec3> list = LayoutWorker_Labyrinth.toEnqueue;
			Comparison<IntVec3> comparison;
			if ((comparison = <>9__0) == null)
			{
				comparison = (<>9__0 = delegate(IntVec3 x, IntVec3 z)
				{
					if (x == z)
					{
						return 0;
					}
					IntVec3 intVec4 = x - start;
					if (intVec4.x == 0 || intVec4.z == 0)
					{
						return -1;
					}
					intVec4 = x - goal;
					if (intVec4.x == 0 || intVec4.z == 0)
					{
						return -1;
					}
					intVec4 = z - start;
					if (intVec4.x == 0 || intVec4.z == 0)
					{
						return 1;
					}
					intVec4 = z - goal;
					if (intVec4.x == 0 || intVec4.z == 0)
					{
						return 1;
					}
					return 0;
				});
			}
			list.Sort(comparison);
			foreach (IntVec3 intVec3 in LayoutWorker_Labyrinth.toEnqueue)
			{
				LayoutWorker_Labyrinth.openSet.Enqueue(intVec3, LayoutWorker_Labyrinth.fScore[intVec3]);
			}
		}
		LayoutWorker_Labyrinth.ResetPathVars();
		path = null;
		return false;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00006420 File Offset: 0x00004620
	private static IEnumerable<IntVec3> Neighbours8Way(StructureLayout layout, IntVec3 cell)
	{
		foreach (IntVec3 b in GenAdj.AdjacentCellsAround)
		{
			IntVec3 intVec = cell + b;
			if (intVec.x > 0 && intVec.z > 0 && intVec.x < layout.Width && intVec.z < layout.Height && layout.IsEmptyAt(intVec))
			{
				yield return intVec;
			}
		}
		IntVec3[] array = null;
		yield break;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00006437 File Offset: 0x00004637
	private static IEnumerable<IntVec3> Neighbours(StructureLayout layout, IntVec3 cell, IntVec3 goal)
	{
		int num;
		for (int i = 0; i < 4; i = num + 1)
		{
			IntVec3 intVec = cell + new Rot4(i).FacingCell;
			if (intVec.x > 0 && intVec.z > 0 && intVec.x < layout.Width && intVec.z < layout.Height && (!(intVec != goal) || layout.IsEmptyAt(intVec)))
			{
				yield return intVec;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00006458 File Offset: 0x00004658
	private static int Heuristic(IntVec3 pos, IntVec3 goal)
	{
		return (goal - pos).LengthManhattan;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00006474 File Offset: 0x00004674
	private static bool OverlapsWithAnyRoom(StructureLayout layout, CellRect rect)
	{
		foreach (LayoutRoom layoutRoom in layout.Rooms)
		{
			foreach (CellRect cellRect in layoutRoom.rects)
			{
				if (cellRect.Overlaps(rect.ContractedBy(1)))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00006514 File Offset: 0x00004714
	private static void CreateDoors(StructureLayout layout)
	{
		LayoutWorker_Labyrinth.tmpCells.Clear();
		LayoutWorker_Labyrinth.tmpCells.AddRange(layout.container.Cells.InRandomOrder(null));
		for (int i = 0; i < LayoutWorker_Labyrinth.tmpCells.Count; i++)
		{
			IntVec3 intVec = LayoutWorker_Labyrinth.tmpCells[i];
			if (layout.IsWallAt(intVec))
			{
				if (layout.IsGoodForHorizontalDoor(intVec))
				{
					LayoutWorker_Labyrinth.TryConnectAdjacentRooms(layout, intVec, IntVec3.North);
				}
				if (layout.IsGoodForVerticalDoor(intVec))
				{
					LayoutWorker_Labyrinth.TryConnectAdjacentRooms(layout, intVec, IntVec3.East);
				}
			}
		}
		LayoutWorker_Labyrinth.tmpCells.Clear();
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000065A4 File Offset: 0x000047A4
	private static void TryConnectAdjacentRooms(StructureLayout layout, IntVec3 p, IntVec3 dir)
	{
		LayoutRoom layoutRoom;
		if (!layout.TryGetRoom(p + dir, out layoutRoom))
		{
			return;
		}
		LayoutRoom layoutRoom2;
		if (!layout.TryGetRoom(p - dir, out layoutRoom2))
		{
			return;
		}
		if (layoutRoom.connections.Contains(layoutRoom2))
		{
			return;
		}
		bool flag = false;
		using (IEnumerator<ValueTuple<LayoutRoom, CellRect, CellRect>> enumerator = layout.GetLogicalRoomConnections(layoutRoom).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Item1 == layoutRoom2)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		if (layoutRoom.entryCells != null && !layoutRoom.entryCells.Contains(p))
		{
			return;
		}
		if (layoutRoom2.entryCells != null && !layoutRoom2.entryCells.Contains(p))
		{
			return;
		}
		layout.Add(p, RoomLayoutCellType.Door);
		layoutRoom.connections.Add(layoutRoom2);
		layoutRoom2.connections.Add(layoutRoom);
	}

	// Token: 0x04000043 RID: 67
	private static readonly IntRange RoomSizeRange = new IntRange(8, 12);

	// Token: 0x04000044 RID: 68
	private static readonly IntRange LShapeRoomRange = new IntRange(6, 12);

	// Token: 0x04000045 RID: 69
	private static readonly IntRange RoomRange = new IntRange(32, 48);

	// Token: 0x04000046 RID: 70
	private const int CorridorInflation = 3;

	// Token: 0x04000047 RID: 71
	private const int ObeliskRoomSize = 19;

	// Token: 0x04000048 RID: 72
	private static readonly PriorityQueue<IntVec3, int> openSet = new PriorityQueue<IntVec3, int>();

	// Token: 0x04000049 RID: 73
	private static readonly Dictionary<IntVec3, IntVec3> cameFrom = new Dictionary<IntVec3, IntVec3>();

	// Token: 0x0400004A RID: 74
	private static readonly Dictionary<IntVec3, int> gScore = new Dictionary<IntVec3, int>();

	// Token: 0x0400004B RID: 75
	private static readonly Dictionary<IntVec3, int> fScore = new Dictionary<IntVec3, int>();

	// Token: 0x0400004C RID: 76
	private static readonly List<IntVec3> toEnqueue = new List<IntVec3>();

	// Token: 0x0400004D RID: 77
	private static readonly List<IntVec3> tmpCells = new List<IntVec3>();
}
