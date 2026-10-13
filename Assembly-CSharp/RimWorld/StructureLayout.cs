using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DelaunatorSharp;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F64 RID: 8036
	public class StructureLayout : IExposable
	{
		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x0600B173 RID: 45427 RVA: 0x0032B264 File Offset: 0x00329464
		public List<LayoutRoom> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x0600B174 RID: 45428 RVA: 0x0032B26C File Offset: 0x0032946C
		public int Width
		{
			get
			{
				return this.roomIds.GetLength(0);
			}
		}

		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x0600B175 RID: 45429 RVA: 0x0032B27A File Offset: 0x0032947A
		public int Height
		{
			get
			{
				return this.roomIds.GetLength(1);
			}
		}

		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x0600B176 RID: 45430 RVA: 0x0032B288 File Offset: 0x00329488
		public int Area
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.rooms.Count; i++)
				{
					num += this.rooms[i].Area;
				}
				return num;
			}
		}

		// Token: 0x0600B177 RID: 45431 RVA: 0x0032B2C2 File Offset: 0x003294C2
		public StructureLayout(LayoutStructureSketch sketch)
		{
			this.sketch = sketch;
		}

		// Token: 0x0600B178 RID: 45432 RVA: 0x0032B2DC File Offset: 0x003294DC
		public StructureLayout(LayoutStructureSketch sketch, CellRect rect)
		{
			this.sketch = sketch;
			this.container = rect;
			this.cellTypes = new RoomLayoutCellType[rect.Width, rect.Height];
			this.roomIds = new int[rect.Width, rect.Height];
			for (int i = 0; i < rect.Width; i++)
			{
				for (int j = 0; j < rect.Height; j++)
				{
					this.roomIds[i, j] = -1;
				}
			}
		}

		// Token: 0x0600B179 RID: 45433 RVA: 0x0032B36C File Offset: 0x0032956C
		public bool HasRoomWithDef(LayoutRoomDef def)
		{
			return this.GetFirstRoomOfDef(def) != null;
		}

		// Token: 0x0600B17A RID: 45434 RVA: 0x0032B378 File Offset: 0x00329578
		public bool TryGetFirstRoomOfDef(LayoutRoomDef def, out LayoutRoom room)
		{
			room = this.GetFirstRoomOfDef(def);
			return room != null;
		}

		// Token: 0x0600B17B RID: 45435 RVA: 0x0032B388 File Offset: 0x00329588
		public LayoutRoom GetFirstRoomOfDef(LayoutRoomDef def)
		{
			foreach (LayoutRoom layoutRoom in this.rooms)
			{
				if (layoutRoom.defs != null && layoutRoom.defs.Contains(def))
				{
					return layoutRoom;
				}
			}
			return null;
		}

		// Token: 0x0600B17C RID: 45436 RVA: 0x0032B3F4 File Offset: 0x003295F4
		public LayoutRoom AddRoom(List<CellRect> rects, LayoutRoomDef requiredDef = null)
		{
			for (int i = 0; i < rects.Count; i++)
			{
				rects[i] = rects[i].ClipInsideRect(this.container);
			}
			LayoutRoom layoutRoom = new LayoutRoom(this.sketch, rects)
			{
				requiredDef = requiredDef
			};
			this.rooms.Add(layoutRoom);
			return layoutRoom;
		}

		// Token: 0x0600B17D RID: 45437 RVA: 0x0032B44F File Offset: 0x0032964F
		public void RemoveRoom(LayoutRoom room)
		{
			this.rooms.Remove(room);
		}

		// Token: 0x0600B17E RID: 45438 RVA: 0x0032B45E File Offset: 0x0032965E
		public void Add(IntVec3 position, RoomLayoutCellType cellType)
		{
			if (this.cellTypes.InBounds(position.x, position.z))
			{
				this.cellTypes[position.x, position.z] = cellType;
			}
		}

		// Token: 0x0600B17F RID: 45439 RVA: 0x0032B494 File Offset: 0x00329694
		public bool IsGoodForHorizontalDoor(IntVec3 p)
		{
			return this.IsWallAt(p + IntVec3.West) && this.IsWallAt(p + IntVec3.East) && !this.IsWallAt(p + IntVec3.North) && !this.IsWallAt(p + IntVec3.South);
		}

		// Token: 0x0600B180 RID: 45440 RVA: 0x0032B4F0 File Offset: 0x003296F0
		public bool IsGoodForVerticalDoor(IntVec3 p)
		{
			return this.IsWallAt(p + IntVec3.North) && this.IsWallAt(p + IntVec3.South) && !this.IsWallAt(p + IntVec3.East) && !this.IsWallAt(p + IntVec3.West);
		}

		// Token: 0x0600B181 RID: 45441 RVA: 0x0032B54C File Offset: 0x0032974C
		public bool IsGoodForDoor(IntVec3 p)
		{
			return this.IsGoodForHorizontalDoor(p) || this.IsGoodForVerticalDoor(p);
		}

		// Token: 0x0600B182 RID: 45442 RVA: 0x0032B560 File Offset: 0x00329760
		public bool IsWallAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Wall;
		}

		// Token: 0x0600B183 RID: 45443 RVA: 0x0032B597 File Offset: 0x00329797
		public bool IsFloorAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Floor;
		}

		// Token: 0x0600B184 RID: 45444 RVA: 0x0032B5CE File Offset: 0x003297CE
		public bool IsDoorAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Door;
		}

		// Token: 0x0600B185 RID: 45445 RVA: 0x0032B605 File Offset: 0x00329805
		public bool IsEmptyAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Empty;
		}

		// Token: 0x0600B186 RID: 45446 RVA: 0x0032B63C File Offset: 0x0032983C
		public bool IsOutside(IntVec3 position)
		{
			return !this.cellTypes.InBounds(position.x, position.z) || this.cellTypes[position.x, position.z] == RoomLayoutCellType.Empty;
		}

		// Token: 0x0600B187 RID: 45447 RVA: 0x0032B673 File Offset: 0x00329873
		public int GetRoomIdAt(IntVec3 position)
		{
			if (!this.roomIds.InBounds(position.x, position.z))
			{
				return -2;
			}
			return this.roomIds[position.x, position.z];
		}

		// Token: 0x0600B188 RID: 45448 RVA: 0x0032B6A8 File Offset: 0x003298A8
		public bool CanRemoveWithoutDisconnection(LayoutRoom room)
		{
			return !this.WouldDisconnectRoomsIfRemoved(room);
		}

		// Token: 0x0600B189 RID: 45449 RVA: 0x0032B6B8 File Offset: 0x003298B8
		public bool TryMinimizeLayoutWithoutDisconnection()
		{
			if (this.rooms.Count == 1)
			{
				return false;
			}
			for (int i = this.rooms.Count - 1; i >= 0; i--)
			{
				if (this.rooms[i].requiredDef == null && !this.WouldDisconnectRoomsIfRemoved(this.rooms[i]))
				{
					this.rooms.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B18A RID: 45450 RVA: 0x0032B724 File Offset: 0x00329924
		private bool WouldDisconnectRoomsIfRemoved(LayoutRoom room)
		{
			StructureLayout.tmpRooms.Clear();
			StructureLayout.tmpRooms.AddRange(this.rooms);
			StructureLayout.tmpRooms.Remove(room);
			StructureLayout.tmpSeenRooms.Clear();
			StructureLayout.tmpRoomQueue.Clear();
			StructureLayout.tmpRoomQueue.Enqueue(StructureLayout.tmpRooms.First<LayoutRoom>());
			while (StructureLayout.tmpRoomQueue.Count > 0)
			{
				LayoutRoom layoutRoom = StructureLayout.tmpRoomQueue.Dequeue();
				StructureLayout.tmpSeenRooms.Add(layoutRoom);
				foreach (LayoutRoom layoutRoom2 in StructureLayout.tmpRooms)
				{
					if (layoutRoom != layoutRoom2 && !StructureLayout.tmpSeenRooms.Contains(layoutRoom2) && layoutRoom.IsAdjacentTo(layoutRoom2, 3))
					{
						StructureLayout.tmpRoomQueue.Enqueue(layoutRoom2);
					}
				}
			}
			int count = StructureLayout.tmpRooms.Count;
			int count2 = StructureLayout.tmpSeenRooms.Count;
			StructureLayout.tmpRooms.Clear();
			StructureLayout.tmpSeenRooms.Clear();
			return count2 != count;
		}

		// Token: 0x0600B18B RID: 45451 RVA: 0x0032B838 File Offset: 0x00329A38
		public bool IsAdjacentToLayoutEdge(LayoutRoom room)
		{
			for (int i = 0; i < room.rects.Count; i++)
			{
				if (room.rects[i].minX == this.container.minX || room.rects[i].maxX == this.container.maxX || room.rects[i].minZ == this.container.minZ || room.rects[i].maxZ == this.container.maxZ)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B18C RID: 45452 RVA: 0x0032B8DC File Offset: 0x00329ADC
		public void FinalizeRooms(bool avoidDoubleWalls = true)
		{
			for (int i = 0; i < 4; i++)
			{
				Rot4 dir = new Rot4(i);
				foreach (LayoutRoom layoutRoom in this.rooms)
				{
					for (int j = 0; j < layoutRoom.rects.Count; j++)
					{
						foreach (IntVec3 intVec in layoutRoom.rects[j].GetEdgeCells(dir))
						{
							IntVec3 facingCell = dir.FacingCell + intVec;
							if (!avoidDoubleWalls || (!this.IsWallAt(facingCell) && !layoutRoom.rects.Any((CellRect r) => r.Contains(facingCell))))
							{
								bool flag = false;
								foreach (CellRect lhs in layoutRoom.rects)
								{
									if (!(lhs == layoutRoom.rects[j]) && lhs.Contains(intVec))
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									this.Add(intVec, RoomLayoutCellType.Wall);
								}
							}
						}
					}
				}
			}
			for (int k = 0; k < this.rooms.Count; k++)
			{
				LayoutRoom layoutRoom2 = this.rooms[k];
				foreach (CellRect cellRect in layoutRoom2.rects)
				{
					foreach (IntVec3 intVec2 in cellRect.Cells)
					{
						this.roomIds[intVec2.x, intVec2.z] = k;
						layoutRoom2.id = k;
						if (!this.IsWallAt(intVec2))
						{
							this.Add(intVec2, RoomLayoutCellType.Floor);
						}
					}
				}
			}
			for (int l = this.container.minX; l < this.container.maxX; l++)
			{
				for (int m = this.container.minZ; m < this.container.maxZ; m++)
				{
					IntVec3 intVec3 = new IntVec3(l, 0, m);
					if (!this.IsWallAt(intVec3) && this.IsFloorAt(intVec3))
					{
						int num = 0;
						foreach (IntVec3 b in GenAdj.CardinalDirections)
						{
							if (this.IsWallAt(intVec3 + b))
							{
								num++;
							}
						}
						int num2 = 0;
						int num3 = 0;
						foreach (IntVec3 b2 in GenAdj.DiagonalDirections)
						{
							if (this.IsWallAt(intVec3 + b2))
							{
								num2++;
							}
							else if (!this.IsFloorAt(intVec3 + b2))
							{
								num3++;
							}
						}
						if (num > 1 && (num2 < 2 || num3 > 0))
						{
							this.Add(intVec3, RoomLayoutCellType.Wall);
						}
					}
				}
			}
		}

		// Token: 0x0600B18D RID: 45453 RVA: 0x0032BCC0 File Offset: 0x00329EC0
		public bool CellWithinRangeOfUndamageableRoom(IntVec3 cell, float range)
		{
			foreach (LayoutRoom layoutRoom in this.Rooms)
			{
				if (layoutRoom.DontDestroyWallsDoors)
				{
					foreach (CellRect cellRect in layoutRoom.rects)
					{
						if (cellRect.ClosestDistanceTo(cell) <= range)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600B18E RID: 45454 RVA: 0x0032BD64 File Offset: 0x00329F64
		public int GetLogicalRoomConnectionCount(LayoutRoom room)
		{
			if (this.neighbours == null || this.neighbours.connections.NullOrEmpty<Vector2, List<Vector2>>())
			{
				return 0;
			}
			int num = 0;
			foreach (KeyValuePair<Vector2, List<Vector2>> keyValuePair in this.neighbours.connections)
			{
				foreach (CellRect cellRect in room.rects)
				{
					if (cellRect.Contains(new IntVec3(Mathf.RoundToInt(keyValuePair.Key.x), 0, Mathf.RoundToInt(keyValuePair.Key.y))))
					{
						num += keyValuePair.Value.Count;
					}
				}
			}
			return num;
		}

		// Token: 0x0600B18F RID: 45455 RVA: 0x0032BE54 File Offset: 0x0032A054
		[return: TupleElementNames(new string[]
		{
			null,
			"a",
			"b"
		})]
		public IEnumerable<ValueTuple<LayoutRoom, CellRect, CellRect>> GetLogicalRoomConnections(LayoutRoom room)
		{
			if (this.neighbours == null || this.neighbours.connections.NullOrEmpty<Vector2, List<Vector2>>())
			{
				yield break;
			}
			Dictionary<CellRect, List<Vector2>> connections = new Dictionary<CellRect, List<Vector2>>();
			foreach (KeyValuePair<Vector2, List<Vector2>> keyValuePair in this.neighbours.connections)
			{
				foreach (CellRect key in room.rects)
				{
					if (key.Contains(new IntVec3(Mathf.RoundToInt(keyValuePair.Key.x), 0, Mathf.RoundToInt(keyValuePair.Key.y))))
					{
						connections[key] = new List<Vector2>();
						connections[key].AddRange(keyValuePair.Value);
					}
				}
			}
			if (connections.Count == 0)
			{
				yield break;
			}
			foreach (LayoutRoom otherRoom in this.Rooms)
			{
				if (otherRoom != room)
				{
					bool found = false;
					foreach (CellRect item in otherRoom.rects)
					{
						foreach (KeyValuePair<CellRect, List<Vector2>> keyValuePair2 in connections)
						{
							CellRect cellRect;
							List<Vector2> list;
							keyValuePair2.Deconstruct(out cellRect, out list);
							CellRect item2 = cellRect;
							foreach (Vector2 vector in list)
							{
								if (item.Contains(new IntVec3(Mathf.RoundToInt(vector.x), 0, Mathf.RoundToInt(vector.y))))
								{
									found = true;
									break;
								}
							}
							if (found)
							{
								yield return new ValueTuple<LayoutRoom, CellRect, CellRect>(otherRoom, item2, item);
								break;
							}
						}
						Dictionary<CellRect, List<Vector2>>.Enumerator enumerator5 = default(Dictionary<CellRect, List<Vector2>>.Enumerator);
						if (found)
						{
							break;
						}
					}
					List<CellRect>.Enumerator enumerator4 = default(List<CellRect>.Enumerator);
					otherRoom = null;
				}
			}
			List<LayoutRoom>.Enumerator enumerator3 = default(List<LayoutRoom>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B190 RID: 45456 RVA: 0x0032BE6C File Offset: 0x0032A06C
		public void RemoveLogicalConnectionsBetweenRooms(LayoutRoom a, LayoutRoom b)
		{
			if (this.neighbours == null || this.neighbours.connections.NullOrEmpty<Vector2, List<Vector2>>())
			{
				return;
			}
			foreach (KeyValuePair<Vector2, List<Vector2>> keyValuePair in this.neighbours.connections)
			{
				Vector2 vector;
				List<Vector2> list;
				keyValuePair.Deconstruct(out vector, out list);
				Vector2 point = vector;
				List<Vector2> connections = list;
				StructureLayout.RemoveLinksBetween(a, b, point, connections);
				StructureLayout.RemoveLinksBetween(b, a, point, connections);
			}
		}

		// Token: 0x0600B191 RID: 45457 RVA: 0x0032BEFC File Offset: 0x0032A0FC
		private static void RemoveLinksBetween(LayoutRoom a, LayoutRoom b, Vector2 point, List<Vector2> connections)
		{
			IntVec3 c = new IntVec3(Mathf.RoundToInt(point.x), 0, Mathf.RoundToInt(point.y));
			foreach (CellRect cellRect in a.rects)
			{
				if (cellRect.Contains(c))
				{
					foreach (Vector2 vector in connections)
					{
						IntVec3 position = new IntVec3(Mathf.RoundToInt(vector.x), 0, Mathf.RoundToInt(vector.y));
						if (b.Contains(position, 0))
						{
							connections.Remove(vector);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600B192 RID: 45458 RVA: 0x0032BFE0 File Offset: 0x0032A1E0
		public bool TryGetRoom(IntVec3 pos, out LayoutRoom room)
		{
			foreach (LayoutRoom layoutRoom in this.Rooms)
			{
				foreach (CellRect cellRect in layoutRoom.rects)
				{
					if (cellRect.Contains(pos))
					{
						room = layoutRoom;
						return true;
					}
				}
			}
			room = null;
			return false;
		}

		// Token: 0x0600B193 RID: 45459 RVA: 0x0032C080 File Offset: 0x0032A280
		public void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.container, "container", default(CellRect), false);
			Scribe_Collections.Look<LayoutRoom>(ref this.rooms, "rooms", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.rooms.Count - 1; i >= 0; i--)
				{
					if (this.rooms[i] == null)
					{
						this.rooms.RemoveAt(i);
					}
					else if (this.rooms[i].sketch == null)
					{
						this.rooms[i].sketch = this.sketch;
					}
				}
			}
		}

		// Token: 0x0400796E RID: 31086
		public LayoutStructureSketch sketch;

		// Token: 0x0400796F RID: 31087
		public CellRect container;

		// Token: 0x04007970 RID: 31088
		private List<LayoutRoom> rooms = new List<LayoutRoom>();

		// Token: 0x04007971 RID: 31089
		public RoomLayoutCellType[,] cellTypes;

		// Token: 0x04007972 RID: 31090
		public int[,] roomIds;

		// Token: 0x04007973 RID: 31091
		public Delaunator delaunator;

		// Token: 0x04007974 RID: 31092
		public RelativeNeighborhoodGraph neighbours;

		// Token: 0x04007975 RID: 31093
		private const int MinAdjacencyForDisconnectedRoom = 3;

		// Token: 0x04007976 RID: 31094
		private static readonly List<LayoutRoom> tmpRooms = new List<LayoutRoom>();

		// Token: 0x04007977 RID: 31095
		private static readonly Queue<LayoutRoom> tmpRoomQueue = new Queue<LayoutRoom>();

		// Token: 0x04007978 RID: 31096
		private static readonly HashSet<LayoutRoom> tmpSeenRooms = new HashSet<LayoutRoom>();
	}
}
