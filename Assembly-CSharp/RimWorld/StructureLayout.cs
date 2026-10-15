using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DelaunatorSharp;
using UnityEngine;
using Verse;

namespace RimWorld;

public class StructureLayout : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetLogicalRoomConnections_003Ed__43 : IEnumerable<(LayoutRoom, CellRect a, CellRect b)>, IEnumerable, IEnumerator<(LayoutRoom, CellRect a, CellRect b)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (LayoutRoom, CellRect a, CellRect b) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StructureLayout _003C_003E4__this;

		private LayoutRoom room;

		public LayoutRoom _003C_003E3__room;

		private Dictionary<CellRect, List<Vector2>> _003Cconnections_003E5__2;

		private List<LayoutRoom>.Enumerator _003C_003E7__wrap2;

		private LayoutRoom _003CotherRoom_003E5__4;

		private bool _003Cfound_003E5__5;

		private List<CellRect>.Enumerator _003C_003E7__wrap5;

		private Dictionary<CellRect, List<Vector2>>.Enumerator _003C_003E7__wrap6;

		(LayoutRoom, CellRect a, CellRect b) IEnumerator<(LayoutRoom, CellRect, CellRect)>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetLogicalRoomConnections_003Ed__43(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -5) <= 2u || num == 1)
			{
				try
				{
					if ((uint)(num - -5) <= 1u || num == 1)
					{
						try
						{
							if (num == -5 || num == 1)
							{
								try
								{
								}
								finally
								{
									_003C_003Em__Finally3();
								}
							}
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cconnections_003E5__2 = null;
			_003C_003E7__wrap2 = default(List<LayoutRoom>.Enumerator);
			_003CotherRoom_003E5__4 = null;
			_003C_003E7__wrap5 = default(List<CellRect>.Enumerator);
			_003C_003E7__wrap6 = default(Dictionary<CellRect, List<Vector2>>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				StructureLayout structureLayout = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -5;
					goto IL_0286;
				}
				_003C_003E1__state = -1;
				if (structureLayout.neighbours == null || structureLayout.neighbours.connections.NullOrEmpty())
				{
					return false;
				}
				_003Cconnections_003E5__2 = new Dictionary<CellRect, List<Vector2>>();
				foreach (KeyValuePair<Vector2, List<Vector2>> connection in structureLayout.neighbours.connections)
				{
					foreach (CellRect rect in room.rects)
					{
						if (rect.Contains(new IntVec3(Mathf.RoundToInt(connection.Key.x), 0, Mathf.RoundToInt(connection.Key.y))))
						{
							_003Cconnections_003E5__2[rect] = new List<Vector2>();
							_003Cconnections_003E5__2[rect].AddRange(connection.Value);
						}
					}
				}
				if (_003Cconnections_003E5__2.Count == 0)
				{
					return false;
				}
				_003C_003E7__wrap2 = structureLayout.Rooms.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_02c9;
				IL_02b0:
				_003C_003Em__Finally2();
				_003C_003E7__wrap5 = default(List<CellRect>.Enumerator);
				_003CotherRoom_003E5__4 = null;
				goto IL_02c9;
				IL_0286:
				_003C_003Em__Finally3();
				_003C_003E7__wrap6 = default(Dictionary<CellRect, List<Vector2>>.Enumerator);
				if (!_003Cfound_003E5__5)
				{
					goto IL_02a0;
				}
				goto IL_02b0;
				IL_02a0:
				if (_003C_003E7__wrap5.MoveNext())
				{
					CellRect current3 = _003C_003E7__wrap5.Current;
					_003C_003E7__wrap6 = _003Cconnections_003E5__2.GetEnumerator();
					_003C_003E1__state = -5;
					while (_003C_003E7__wrap6.MoveNext())
					{
						var (item, list2) = (KeyValuePair<CellRect, List<Vector2>>)(ref _003C_003E7__wrap6.Current);
						foreach (Vector2 item2 in list2)
						{
							if (current3.Contains(new IntVec3(Mathf.RoundToInt(item2.x), 0, Mathf.RoundToInt(item2.y))))
							{
								_003Cfound_003E5__5 = true;
								break;
							}
						}
						if (_003Cfound_003E5__5)
						{
							_003C_003E2__current = (_003CotherRoom_003E5__4, a: item, b: current3);
							_003C_003E1__state = 1;
							return true;
						}
					}
					goto IL_0286;
				}
				goto IL_02b0;
				IL_02c9:
				do
				{
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003CotherRoom_003E5__4 = _003C_003E7__wrap2.Current;
						continue;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = default(List<LayoutRoom>.Enumerator);
					return false;
				}
				while (_003CotherRoom_003E5__4 == room);
				_003Cfound_003E5__5 = false;
				_003C_003E7__wrap5 = _003CotherRoom_003E5__4.rects.GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_02a0;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap5/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			((IDisposable)_003C_003E7__wrap6/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(LayoutRoom, CellRect a, CellRect b)> IEnumerable<(LayoutRoom, CellRect, CellRect)>.GetEnumerator()
		{
			_003CGetLogicalRoomConnections_003Ed__43 _003CGetLogicalRoomConnections_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetLogicalRoomConnections_003Ed__ = this;
			}
			else
			{
				_003CGetLogicalRoomConnections_003Ed__ = new _003CGetLogicalRoomConnections_003Ed__43(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetLogicalRoomConnections_003Ed__.room = _003C_003E3__room;
			return _003CGetLogicalRoomConnections_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(LayoutRoom, CellRect, CellRect)>)this).GetEnumerator();
		}
	}

	public LayoutStructureSketch sketch;

	public CellRect container;

	private List<LayoutRoom> rooms = new List<LayoutRoom>();

	public RoomLayoutCellType[,] cellTypes;

	public int[,] roomIds;

	public Delaunator delaunator;

	public RelativeNeighborhoodGraph neighbours;

	private const int MinAdjacencyForDisconnectedRoom = 3;

	private static readonly List<LayoutRoom> tmpRooms = new List<LayoutRoom>();

	private static readonly Queue<LayoutRoom> tmpRoomQueue = new Queue<LayoutRoom>();

	private static readonly HashSet<LayoutRoom> tmpSeenRooms = new HashSet<LayoutRoom>();

	public List<LayoutRoom> Rooms => rooms;

	public int Width => roomIds.GetLength(0);

	public int Height => roomIds.GetLength(1);

	public int Area
	{
		get
		{
			int num = 0;
			for (int i = 0; i < rooms.Count; i++)
			{
				num += rooms[i].Area;
			}
			return num;
		}
	}

	public StructureLayout(LayoutStructureSketch sketch)
	{
		this.sketch = sketch;
	}

	public StructureLayout(LayoutStructureSketch sketch, CellRect rect)
	{
		this.sketch = sketch;
		container = rect;
		cellTypes = new RoomLayoutCellType[rect.Width, rect.Height];
		roomIds = new int[rect.Width, rect.Height];
		for (int i = 0; i < rect.Width; i++)
		{
			for (int j = 0; j < rect.Height; j++)
			{
				roomIds[i, j] = -1;
			}
		}
	}

	public bool HasRoomWithDef(LayoutRoomDef def)
	{
		return GetFirstRoomOfDef(def) != null;
	}

	public bool TryGetFirstRoomOfDef(LayoutRoomDef def, out LayoutRoom room)
	{
		room = GetFirstRoomOfDef(def);
		return room != null;
	}

	public LayoutRoom GetFirstRoomOfDef(LayoutRoomDef def)
	{
		foreach (LayoutRoom room in rooms)
		{
			if (room.defs != null && room.defs.Contains(def))
			{
				return room;
			}
		}
		return null;
	}

	public LayoutRoom AddRoom(List<CellRect> rects, LayoutRoomDef requiredDef = null)
	{
		for (int i = 0; i < rects.Count; i++)
		{
			rects[i] = rects[i].ClipInsideRect(container);
		}
		LayoutRoom layoutRoom = new LayoutRoom(sketch, rects)
		{
			requiredDef = requiredDef
		};
		rooms.Add(layoutRoom);
		return layoutRoom;
	}

	public void RemoveRoom(LayoutRoom room)
	{
		rooms.Remove(room);
	}

	public void Add(IntVec3 position, RoomLayoutCellType cellType)
	{
		if (cellTypes.InBounds(position.x, position.z))
		{
			cellTypes[position.x, position.z] = cellType;
		}
	}

	public bool IsGoodForHorizontalDoor(IntVec3 p)
	{
		if (IsWallAt(p + IntVec3.West) && IsWallAt(p + IntVec3.East) && !IsWallAt(p + IntVec3.North))
		{
			return !IsWallAt(p + IntVec3.South);
		}
		return false;
	}

	public bool IsGoodForVerticalDoor(IntVec3 p)
	{
		if (IsWallAt(p + IntVec3.North) && IsWallAt(p + IntVec3.South) && !IsWallAt(p + IntVec3.East))
		{
			return !IsWallAt(p + IntVec3.West);
		}
		return false;
	}

	public bool IsGoodForDoor(IntVec3 p)
	{
		if (!IsGoodForHorizontalDoor(p))
		{
			return IsGoodForVerticalDoor(p);
		}
		return true;
	}

	public bool IsWallAt(IntVec3 position)
	{
		if (cellTypes.InBounds(position.x, position.z))
		{
			return cellTypes[position.x, position.z] == RoomLayoutCellType.Wall;
		}
		return false;
	}

	public bool IsFloorAt(IntVec3 position)
	{
		if (cellTypes.InBounds(position.x, position.z))
		{
			return cellTypes[position.x, position.z] == RoomLayoutCellType.Floor;
		}
		return false;
	}

	public bool IsDoorAt(IntVec3 position)
	{
		if (cellTypes.InBounds(position.x, position.z))
		{
			return cellTypes[position.x, position.z] == RoomLayoutCellType.Door;
		}
		return false;
	}

	public bool IsEmptyAt(IntVec3 position)
	{
		if (cellTypes.InBounds(position.x, position.z))
		{
			return cellTypes[position.x, position.z] == RoomLayoutCellType.Empty;
		}
		return false;
	}

	public bool IsOutside(IntVec3 position)
	{
		if (cellTypes.InBounds(position.x, position.z))
		{
			return cellTypes[position.x, position.z] == RoomLayoutCellType.Empty;
		}
		return true;
	}

	public int GetRoomIdAt(IntVec3 position)
	{
		if (!roomIds.InBounds(position.x, position.z))
		{
			return -2;
		}
		return roomIds[position.x, position.z];
	}

	public bool CanRemoveWithoutDisconnection(LayoutRoom room)
	{
		if (WouldDisconnectRoomsIfRemoved(room))
		{
			return false;
		}
		return true;
	}

	public bool TryMinimizeLayoutWithoutDisconnection()
	{
		if (rooms.Count == 1)
		{
			return false;
		}
		for (int num = rooms.Count - 1; num >= 0; num--)
		{
			if (rooms[num].requiredDef == null && !WouldDisconnectRoomsIfRemoved(rooms[num]))
			{
				rooms.RemoveAt(num);
				return true;
			}
		}
		return false;
	}

	private bool WouldDisconnectRoomsIfRemoved(LayoutRoom room)
	{
		tmpRooms.Clear();
		tmpRooms.AddRange(rooms);
		tmpRooms.Remove(room);
		tmpSeenRooms.Clear();
		tmpRoomQueue.Clear();
		tmpRoomQueue.Enqueue(tmpRooms.First());
		while (tmpRoomQueue.Count > 0)
		{
			LayoutRoom layoutRoom = tmpRoomQueue.Dequeue();
			tmpSeenRooms.Add(layoutRoom);
			foreach (LayoutRoom tmpRoom in tmpRooms)
			{
				if (layoutRoom != tmpRoom && !tmpSeenRooms.Contains(tmpRoom) && layoutRoom.IsAdjacentTo(tmpRoom, 3))
				{
					tmpRoomQueue.Enqueue(tmpRoom);
				}
			}
		}
		int count = tmpRooms.Count;
		int count2 = tmpSeenRooms.Count;
		tmpRooms.Clear();
		tmpSeenRooms.Clear();
		return count2 != count;
	}

	public bool IsAdjacentToLayoutEdge(LayoutRoom room)
	{
		for (int i = 0; i < room.rects.Count; i++)
		{
			if (room.rects[i].minX == container.minX || room.rects[i].maxX == container.maxX || room.rects[i].minZ == container.minZ || room.rects[i].maxZ == container.maxZ)
			{
				return true;
			}
		}
		return false;
	}

	public void FinalizeRooms(bool avoidDoubleWalls = true)
	{
		for (int i = 0; i < 4; i++)
		{
			Rot4 dir = new Rot4(i);
			foreach (LayoutRoom room in rooms)
			{
				for (int j = 0; j < room.rects.Count; j++)
				{
					foreach (IntVec3 edgeCell in room.rects[j].GetEdgeCells(dir))
					{
						IntVec3 facingCell = dir.FacingCell + edgeCell;
						if (avoidDoubleWalls && (IsWallAt(facingCell) || room.rects.Any((CellRect r) => r.Contains(facingCell))))
						{
							continue;
						}
						bool flag = false;
						foreach (CellRect rect in room.rects)
						{
							if (!(rect == room.rects[j]) && rect.Contains(edgeCell))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							Add(edgeCell, RoomLayoutCellType.Wall);
						}
					}
				}
			}
		}
		for (int k = 0; k < rooms.Count; k++)
		{
			LayoutRoom layoutRoom = rooms[k];
			foreach (CellRect rect2 in layoutRoom.rects)
			{
				foreach (IntVec3 cell in rect2.Cells)
				{
					roomIds[cell.x, cell.z] = k;
					layoutRoom.id = k;
					if (!IsWallAt(cell))
					{
						Add(cell, RoomLayoutCellType.Floor);
					}
				}
			}
		}
		for (int l = container.minX; l < container.maxX; l++)
		{
			for (int m = container.minZ; m < container.maxZ; m++)
			{
				IntVec3 intVec = new IntVec3(l, 0, m);
				if (IsWallAt(intVec) || !IsFloorAt(intVec))
				{
					continue;
				}
				int num = 0;
				IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
				foreach (IntVec3 intVec2 in cardinalDirections)
				{
					if (IsWallAt(intVec + intVec2))
					{
						num++;
					}
				}
				int num2 = 0;
				int num3 = 0;
				cardinalDirections = GenAdj.DiagonalDirections;
				foreach (IntVec3 intVec3 in cardinalDirections)
				{
					if (IsWallAt(intVec + intVec3))
					{
						num2++;
					}
					else if (!IsFloorAt(intVec + intVec3))
					{
						num3++;
					}
				}
				if (num > 1 && (num2 < 2 || num3 > 0))
				{
					Add(intVec, RoomLayoutCellType.Wall);
				}
			}
		}
	}

	public bool CellWithinRangeOfUndamageableRoom(IntVec3 cell, float range)
	{
		foreach (LayoutRoom room in Rooms)
		{
			if (!room.DontDestroyWallsDoors)
			{
				continue;
			}
			foreach (CellRect rect in room.rects)
			{
				if (rect.ClosestDistanceTo(cell) <= range)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetLogicalRoomConnectionCount(LayoutRoom room)
	{
		if (neighbours == null || neighbours.connections.NullOrEmpty())
		{
			return 0;
		}
		int num = 0;
		foreach (KeyValuePair<Vector2, List<Vector2>> connection in neighbours.connections)
		{
			foreach (CellRect rect in room.rects)
			{
				if (rect.Contains(new IntVec3(Mathf.RoundToInt(connection.Key.x), 0, Mathf.RoundToInt(connection.Key.y))))
				{
					num += connection.Value.Count;
				}
			}
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CGetLogicalRoomConnections_003Ed__43))]
	public IEnumerable<(LayoutRoom, CellRect a, CellRect b)> GetLogicalRoomConnections(LayoutRoom room)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetLogicalRoomConnections_003Ed__43(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__room = room
		};
	}

	public void RemoveLogicalConnectionsBetweenRooms(LayoutRoom a, LayoutRoom b)
	{
		if (neighbours == null || neighbours.connections.NullOrEmpty())
		{
			return;
		}
		foreach (var (point, connections) in neighbours.connections)
		{
			RemoveLinksBetween(a, b, point, connections);
			RemoveLinksBetween(b, a, point, connections);
		}
	}

	private static void RemoveLinksBetween(LayoutRoom a, LayoutRoom b, Vector2 point, List<Vector2> connections)
	{
		IntVec3 c = new IntVec3(Mathf.RoundToInt(point.x), 0, Mathf.RoundToInt(point.y));
		foreach (CellRect rect in a.rects)
		{
			if (!rect.Contains(c))
			{
				continue;
			}
			foreach (Vector2 connection in connections)
			{
				IntVec3 position = new IntVec3(Mathf.RoundToInt(connection.x), 0, Mathf.RoundToInt(connection.y));
				if (b.Contains(position))
				{
					connections.Remove(connection);
					break;
				}
			}
		}
	}

	public bool TryGetRoom(IntVec3 pos, out LayoutRoom room)
	{
		foreach (LayoutRoom room2 in Rooms)
		{
			foreach (CellRect rect in room2.rects)
			{
				if (rect.Contains(pos))
				{
					room = room2;
					return true;
				}
			}
		}
		room = null;
		return false;
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref container, "container");
		Scribe_Collections.Look(ref rooms, "rooms", LookMode.Deep);
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		for (int num = rooms.Count - 1; num >= 0; num--)
		{
			if (rooms[num] == null)
			{
				rooms.RemoveAt(num);
			}
			else if (rooms[num].sketch == null)
			{
				rooms[num].sketch = sketch;
			}
		}
	}
}
