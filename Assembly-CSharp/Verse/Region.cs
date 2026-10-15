using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse;

public sealed class Region
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass58_0
	{
		public HashSet<IntVec3> toRemove;

		internal bool _003CEnumerateRectangleCover_003Eb__0(IntVec3 x)
		{
			return toRemove.Contains(x);
		}
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateRectangleCover_003Ed__58 : IEnumerable<CellRect>, IEnumerable, IEnumerator<CellRect>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private CellRect _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Region _003C_003E4__this;

		private List<IntVec3> _003CuncoveredCells_003E5__2;

		CellRect IEnumerator<CellRect>.Current
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
		public _003CEnumerateRectangleCover_003Ed__58(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CuncoveredCells_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Region region = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CuncoveredCells_003E5__2 = new List<IntVec3>(region.Cells);
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003CuncoveredCells_003E5__2.Count > 0)
			{
				_003C_003Ec__DisplayClass58_0 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass58_0();
				IntVec3 intVec = _003CuncoveredCells_003E5__2.PopFront();
				int i = intVec.x;
				int num2;
				for (num2 = intVec.z; _003CuncoveredCells_003E5__2.Contains(new IntVec3(i + 1, 0, num2)); i++)
				{
				}
				while (true)
				{
					bool flag = false;
					for (int j = intVec.x; j <= i; j++)
					{
						if (!_003CuncoveredCells_003E5__2.Contains(new IntVec3(j, 0, num2 + 1)))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
					num2++;
				}
				CellRect cellRect = new CellRect(intVec.x, intVec.z, i - intVec.x + 1, num2 - intVec.z + 1);
				CS_0024_003C_003E8__locals2.toRemove = new HashSet<IntVec3>(cellRect.Cells);
				_003CuncoveredCells_003E5__2.RemoveAll((IntVec3 x) => CS_0024_003C_003E8__locals2.toRemove.Contains(x));
				_003C_003E2__current = cellRect;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<CellRect> IEnumerable<CellRect>.GetEnumerator()
		{
			_003CEnumerateRectangleCover_003Ed__58 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateRectangleCover_003Ed__58(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CellRect>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Cells_003Ed__31 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Region _003C_003E4__this;

		private RegionGrid _003Cregions_003E5__2;

		private int _003Cz_003E5__3;

		private int _003Cx_003E5__4;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003Cget_Cells_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cregions_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Region region = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0092;
			}
			_003C_003E1__state = -1;
			_003Cregions_003E5__2 = region.Map.regionGrid;
			_003Cz_003E5__3 = region.extentsClose.minZ;
			goto IL_00c5;
			IL_00a2:
			if (_003Cx_003E5__4 <= region.extentsClose.maxX)
			{
				IntVec3 c = new IntVec3(_003Cx_003E5__4, 0, _003Cz_003E5__3);
				if (_003Cregions_003E5__2.GetRegionAt_NoRebuild_InvalidAllowed(c) == region)
				{
					_003C_003E2__current = c;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0092;
			}
			_003Cz_003E5__3++;
			goto IL_00c5;
			IL_0092:
			_003Cx_003E5__4++;
			goto IL_00a2;
			IL_00c5:
			if (_003Cz_003E5__3 <= region.extentsClose.maxZ)
			{
				_003Cx_003E5__4 = region.extentsClose.minX;
				goto IL_00a2;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003Cget_Cells_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Cells_003Ed__31(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Neighbors_003Ed__35 : IEnumerable<Region>, IEnumerable, IEnumerator<Region>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Region _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Region _003C_003E4__this;

		private int _003Cli_003E5__2;

		private RegionLink _003Clink_003E5__3;

		private int _003Cri_003E5__4;

		Region IEnumerator<Region>.Current
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
		public _003Cget_Neighbors_003Ed__35(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clink_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Region region = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00b7;
			}
			_003C_003E1__state = -1;
			_003Cli_003E5__2 = 0;
			goto IL_00ea;
			IL_00c7:
			if (_003Cri_003E5__4 < 2)
			{
				if (_003Clink_003E5__3.regions[_003Cri_003E5__4] != null && _003Clink_003E5__3.regions[_003Cri_003E5__4] != region && _003Clink_003E5__3.regions[_003Cri_003E5__4].valid)
				{
					_003C_003E2__current = _003Clink_003E5__3.regions[_003Cri_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b7;
			}
			_003Clink_003E5__3 = null;
			_003Cli_003E5__2++;
			goto IL_00ea;
			IL_00b7:
			_003Cri_003E5__4++;
			goto IL_00c7;
			IL_00ea:
			if (_003Cli_003E5__2 < region.links.Count)
			{
				_003Clink_003E5__3 = region.links[_003Cli_003E5__2];
				_003Cri_003E5__4 = 0;
				goto IL_00c7;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Region> IEnumerable<Region>.GetEnumerator()
		{
			_003Cget_Neighbors_003Ed__35 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Neighbors_003Ed__35(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Region>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_NeighborsOfSameType_003Ed__37 : IEnumerable<Region>, IEnumerable, IEnumerator<Region>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Region _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Region _003C_003E4__this;

		private int _003Cli_003E5__2;

		private RegionLink _003Clink_003E5__3;

		private int _003Cri_003E5__4;

		Region IEnumerator<Region>.Current
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
		public _003Cget_NeighborsOfSameType_003Ed__37(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clink_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Region region = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00d9;
			}
			_003C_003E1__state = -1;
			_003Cli_003E5__2 = 0;
			goto IL_010c;
			IL_00e9:
			if (_003Cri_003E5__4 < 2)
			{
				if (_003Clink_003E5__3.regions[_003Cri_003E5__4] != null && _003Clink_003E5__3.regions[_003Cri_003E5__4] != region && _003Clink_003E5__3.regions[_003Cri_003E5__4].type == region.type && _003Clink_003E5__3.regions[_003Cri_003E5__4].valid)
				{
					_003C_003E2__current = _003Clink_003E5__3.regions[_003Cri_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00d9;
			}
			_003Clink_003E5__3 = null;
			_003Cli_003E5__2++;
			goto IL_010c;
			IL_00d9:
			_003Cri_003E5__4++;
			goto IL_00e9;
			IL_010c:
			if (_003Cli_003E5__2 < region.links.Count)
			{
				_003Clink_003E5__3 = region.links[_003Cli_003E5__2];
				_003Cri_003E5__4 = 0;
				goto IL_00e9;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Region> IEnumerable<Region>.GetEnumerator()
		{
			_003Cget_NeighborsOfSameType_003Ed__37 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_NeighborsOfSameType_003Ed__37(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Region>)this).GetEnumerator();
		}
	}

	public RegionType type = RegionType.Normal;

	public int id = -1;

	public sbyte mapIndex = -1;

	private District districtInt;

	public List<RegionLink> links = new List<RegionLink>();

	public CellRect extentsClose;

	public CellRect extentsLimit;

	public Building_Door door;

	private int precalculatedHashCode;

	public bool touchesMapEdge;

	private int cachedCellCount = -1;

	public bool valid = true;

	private readonly ListerThings listerThings = new ListerThings(ListerThingsUse.Region);

	public readonly uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

	public uint reachedIndex;

	public int newRegionGroupIndex = -1;

	private Dictionary<Area, AreaOverlap> cachedAreaOverlaps;

	public int mark;

	private readonly Dictionary<Pawn, Danger> cachedDangers = new Dictionary<Pawn, Danger>();

	private int cachedDangersForTick;

	private float cachedBaseDesiredPlantsCount;

	private int cachedBaseDesiredPlantsCountForTick = -999999;

	private static readonly Dictionary<Pawn, FloatRange> cachedSafeTemperatureRanges = new Dictionary<Pawn, FloatRange>();

	private static int cachedSafeTemperatureRangesForTick;

	private int debug_makeTick = -1000;

	private int debug_lastTraverseTick = -1000;

	private static int nextId = 1;

	public const int GridSize = 12;

	public Map Map
	{
		get
		{
			if (mapIndex >= 0)
			{
				return Find.Maps[mapIndex];
			}
			return null;
		}
	}

	public IEnumerable<IntVec3> Cells
	{
		[IteratorStateMachine(typeof(_003Cget_Cells_003Ed__31))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Cells_003Ed__31(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public int CellCount
	{
		get
		{
			if (cachedCellCount == -1)
			{
				cachedCellCount = 0;
				RegionGrid regionGrid = Map.regionGrid;
				for (int i = extentsClose.minZ; i <= extentsClose.maxZ; i++)
				{
					for (int j = extentsClose.minX; j <= extentsClose.maxX; j++)
					{
						IntVec3 c = new IntVec3(j, 0, i);
						if (regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c) == this)
						{
							cachedCellCount++;
						}
					}
				}
			}
			return cachedCellCount;
		}
	}

	public IEnumerable<Region> Neighbors
	{
		[IteratorStateMachine(typeof(_003Cget_Neighbors_003Ed__35))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Neighbors_003Ed__35(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<Region> NeighborsOfSameType
	{
		[IteratorStateMachine(typeof(_003Cget_NeighborsOfSameType_003Ed__37))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_NeighborsOfSameType_003Ed__37(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Room Room => District?.Room;

	public District District
	{
		get
		{
			return districtInt;
		}
		set
		{
			if (value != districtInt)
			{
				districtInt?.RemoveRegion(this);
				districtInt = value;
				districtInt?.AddRegion(this);
			}
		}
	}

	public IntVec3 RandomCell
	{
		get
		{
			Map map = Map;
			CellIndices cellIndices = map.cellIndices;
			Region[] directGrid = map.regionGrid.DirectGrid;
			for (int i = 0; i < 1000; i++)
			{
				IntVec3 randomCell = extentsClose.RandomCell;
				if (directGrid[cellIndices.CellToIndex(randomCell)] == this)
				{
					return randomCell;
				}
			}
			return AnyCell;
		}
	}

	public IntVec3 AnyCell
	{
		get
		{
			Map map = Map;
			CellIndices cellIndices = map.cellIndices;
			Region[] directGrid = map.regionGrid.DirectGrid;
			foreach (IntVec3 item in extentsClose)
			{
				if (directGrid[cellIndices.CellToIndex(item)] == this)
				{
					return item;
				}
			}
			Log.Error("Couldn't find any cell in region " + ToString());
			return extentsClose.RandomCell;
		}
	}

	public string DebugString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("id: " + id);
			stringBuilder.AppendLine("mapIndex: " + mapIndex);
			stringBuilder.AppendLine("links count: " + links.Count);
			stringBuilder.AppendLine("type: " + type);
			stringBuilder.AppendLine("touchesMapEdge: " + touchesMapEdge);
			foreach (RegionLink link in links)
			{
				stringBuilder.AppendLine("  --" + link);
			}
			stringBuilder.AppendLine("valid: " + valid);
			stringBuilder.AppendLine("makeTick: " + debug_makeTick);
			stringBuilder.AppendLine("districtID: " + ((District != null) ? District.ID.ToString() : "null district!"));
			stringBuilder.AppendLine("roomID: " + ((Room != null) ? Room.ID.ToString() : "null room!"));
			CellRect cellRect = extentsClose;
			stringBuilder.AppendLine("extentsClose: " + cellRect.ToString());
			cellRect = extentsLimit;
			stringBuilder.AppendLine("extentsLimit: " + cellRect.ToString());
			stringBuilder.AppendLine("ListerThings:");
			if (listerThings.AllThings != null)
			{
				for (int i = 0; i < listerThings.AllThings.Count; i++)
				{
					stringBuilder.AppendLine("  --" + listerThings.AllThings[i]);
				}
			}
			return stringBuilder.ToString();
		}
	}

	public bool DebugIsNew => debug_makeTick > Find.TickManager.TicksGame - 60;

	public ListerThings ListerThings => listerThings;

	public bool IsDoorway => door != null;

	private Region()
	{
	}

	public static Region MakeNewUnfilled(IntVec3 root, Map map)
	{
		Region obj = new Region
		{
			debug_makeTick = Find.TickManager.TicksGame,
			id = nextId
		};
		nextId++;
		obj.mapIndex = (sbyte)map.Index;
		obj.precalculatedHashCode = Gen.HashCombineInt(obj.id, 1295813358);
		obj.extentsClose.minX = root.x;
		obj.extentsClose.maxX = root.x;
		obj.extentsClose.minZ = root.z;
		obj.extentsClose.maxZ = root.z;
		obj.extentsLimit.minX = root.x - root.x % 12;
		obj.extentsLimit.maxX = root.x + 12 - (root.x + 12) % 12 - 1;
		obj.extentsLimit.minZ = root.z - root.z % 12;
		obj.extentsLimit.maxZ = root.z + 12 - (root.z + 12) % 12 - 1;
		obj.extentsLimit.ClipInsideMap(map);
		return obj;
	}

	public bool Allows(TraverseParms tp, bool isDestination)
	{
		if (tp.mode != TraverseMode.PassAllDestroyableThings && tp.mode != TraverseMode.PassAllDestroyablePlayerOwnedThings && tp.mode != TraverseMode.PassAllDestroyableThingsNotWater && !type.Passable())
		{
			return false;
		}
		if ((int)tp.maxDanger < 3 && tp.pawn != null)
		{
			Danger danger = DangerFor(tp.pawn);
			if (isDestination || danger == Danger.Deadly)
			{
				Region region = tp.pawn.GetRegion(RegionType.Set_All);
				if ((region == null || (int)danger > (int)region.DangerFor(tp.pawn)) && (int)danger > (int)tp.maxDanger)
				{
					return false;
				}
			}
		}
		bool flag = type == RegionType.Fence && tp.fenceBlocked && !tp.canBashFences;
		switch (tp.mode)
		{
		case TraverseMode.ByPawn:
			if (door != null)
			{
				if (tp.pawn == null)
				{
					if (!door.FreePassage && !door.Open)
					{
						return tp.canBashDoors;
					}
					return true;
				}
				if (tp.pawn.TryGetAvoidGrid(out var grid))
				{
					foreach (IntVec3 item in door.OccupiedRect())
					{
						if (grid.Grid[Map.cellIndices.CellToIndex(item)] == byte.MaxValue && item.GetRegion(Map) == this)
						{
							return false;
						}
					}
				}
				if (tp.pawn.HostileTo(door))
				{
					if (!door.CanPhysicallyPass(tp.pawn))
					{
						return tp.canBashDoors;
					}
					return true;
				}
				if (door.CanPhysicallyPass(tp.pawn))
				{
					return !door.IsForbiddenToPass(tp.pawn);
				}
				return false;
			}
			return !flag;
		case TraverseMode.NoPassClosedDoors:
		case TraverseMode.NoPassClosedDoorsOrWater:
			if (door == null || door.FreePassage)
			{
				return !flag;
			}
			return false;
		case TraverseMode.PassDoors:
			return !flag;
		case TraverseMode.PassAllDestroyableThings:
			return true;
		case TraverseMode.PassAllDestroyablePlayerOwnedThings:
			if (door != null)
			{
				return door.Faction != Faction.OfPlayer;
			}
			return true;
		case TraverseMode.PassAllDestroyableThingsNotWater:
			return true;
		default:
			throw new NotImplementedException();
		}
	}

	[IteratorStateMachine(typeof(_003CEnumerateRectangleCover_003Ed__58))]
	public IEnumerable<CellRect> EnumerateRectangleCover()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateRectangleCover_003Ed__58(-2)
		{
			_003C_003E4__this = this
		};
	}

	public Danger DangerFor(Pawn p)
	{
		if (Current.ProgramState == ProgramState.Playing)
		{
			Danger value;
			if (cachedDangersForTick != GenTicks.TicksGame)
			{
				cachedDangers.Clear();
				cachedDangersForTick = GenTicks.TicksGame;
			}
			else if (cachedDangers.TryGetValue(p, out value))
			{
				return value;
			}
		}
		Room room = Room;
		float temperature = room.Temperature;
		FloatRange value2;
		if (Current.ProgramState == ProgramState.Playing)
		{
			if (cachedSafeTemperatureRangesForTick != GenTicks.TicksGame)
			{
				cachedSafeTemperatureRanges.Clear();
				cachedSafeTemperatureRangesForTick = GenTicks.TicksGame;
			}
			if (!cachedSafeTemperatureRanges.TryGetValue(p, out value2))
			{
				value2 = p.SafeTemperatureRange();
				cachedSafeTemperatureRanges.Add(p, value2);
			}
		}
		else
		{
			value2 = p.SafeTemperatureRange();
		}
		Danger danger = (value2.Includes(temperature) ? Danger.None : (value2.ExpandedBy(80f).Includes(temperature) ? Danger.Some : Danger.Deadly));
		if (room.Vacuum > 0.5f && p.ConcernedByVacuum)
		{
			danger = Danger.Deadly;
		}
		cachedDangers[p] = danger;
		return danger;
	}

	public float GetBaseDesiredPlantsCount(bool allowCache = true)
	{
		int ticksGame = Find.TickManager.TicksGame;
		if (allowCache && ticksGame - cachedBaseDesiredPlantsCountForTick < 2500)
		{
			return cachedBaseDesiredPlantsCount;
		}
		cachedBaseDesiredPlantsCount = 0f;
		Map map = Map;
		foreach (IntVec3 cell in Cells)
		{
			cachedBaseDesiredPlantsCount += map.wildPlantSpawner.GetBaseDesiredPlantsCountAt(cell);
		}
		cachedBaseDesiredPlantsCountForTick = ticksGame;
		return cachedBaseDesiredPlantsCount;
	}

	public AreaOverlap OverlapWith(Area a)
	{
		if (a.TrueCount == 0)
		{
			return AreaOverlap.None;
		}
		if (Map != a.Map)
		{
			return AreaOverlap.None;
		}
		if (cachedAreaOverlaps == null)
		{
			cachedAreaOverlaps = new Dictionary<Area, AreaOverlap>();
		}
		if (!cachedAreaOverlaps.TryGetValue(a, out var value))
		{
			int num = 0;
			int num2 = 0;
			foreach (IntVec3 cell in Cells)
			{
				num2++;
				if (a[cell])
				{
					num++;
				}
			}
			value = ((num != 0) ? ((num == num2) ? AreaOverlap.Entire : AreaOverlap.Partial) : AreaOverlap.None);
			cachedAreaOverlaps.Add(a, value);
		}
		return value;
	}

	public void Notify_AreaChanged(Area a)
	{
		if (cachedAreaOverlaps != null && cachedAreaOverlaps.ContainsKey(a))
		{
			cachedAreaOverlaps.Remove(a);
		}
	}

	public void DecrementMapIndex()
	{
		if (mapIndex <= 0)
		{
			Log.Warning("Tried to decrement map index for region " + id + ", but mapIndex=" + mapIndex);
		}
		else
		{
			mapIndex--;
		}
	}

	public void Notify_MyMapRemoved()
	{
		listerThings.Clear();
		mapIndex = -1;
	}

	public static void ClearStaticData()
	{
		cachedSafeTemperatureRanges.Clear();
	}

	public override string ToString()
	{
		string text = ((door == null) ? "null" : door.ToString());
		return "Region(id=" + id + ", mapIndex=" + mapIndex + ", center=" + extentsClose.CenterCell.ToString() + ", links=" + links.Count + ", cells=" + CellCount + ", touchesMapEdge=" + touchesMapEdge + ((door != null) ? (", portal=" + text) : null) + ")";
	}

	public void DebugDraw()
	{
		if (DebugViewSettings.drawRegionTraversal && Find.TickManager.TicksGame < debug_lastTraverseTick + 60)
		{
			float a = 1f - (float)(Find.TickManager.TicksGame - debug_lastTraverseTick) / 60f;
			GenDraw.DrawFieldEdges(Cells.ToList(), new Color(0f, 0f, 1f, a), null);
		}
	}

	public void DebugDrawMouseover()
	{
		int num = Mathf.RoundToInt(Time.realtimeSinceStartup * 2f) % 2;
		if (DebugViewSettings.drawRegions)
		{
			GenDraw.DrawFieldEdges(Cells.ToList(), DebugColor(), null);
			foreach (Region neighbor in Neighbors)
			{
				GenDraw.DrawFieldEdges(neighbor.Cells.ToList(), Color.grey, null);
			}
		}
		if (DebugViewSettings.drawRegionLinks)
		{
			foreach (RegionLink link in links)
			{
				if (num != 1)
				{
					continue;
				}
				List<IntVec3> list = link.span.Cells.ToList();
				Material mat = DebugSolidColorMats.MaterialOf(Color.magenta * new Color(1f, 1f, 1f, 0.25f));
				foreach (IntVec3 item in list)
				{
					CellRenderer.RenderCell(item, mat);
				}
				GenDraw.DrawFieldEdges(list, Color.white, null);
			}
		}
		if (!DebugViewSettings.drawRegionThings)
		{
			return;
		}
		foreach (Thing allThing in listerThings.AllThings)
		{
			CellRenderer.RenderSpot(allThing.TrueCenter(), (float)(allThing.thingIDNumber % 256) / 256f);
		}
	}

	private Color DebugColor()
	{
		if (!valid)
		{
			return Color.red;
		}
		if (DebugIsNew)
		{
			return Color.yellow;
		}
		return Color.green;
	}

	public void Debug_Notify_Traversed()
	{
		debug_lastTraverseTick = Find.TickManager.TicksGame;
	}

	public override int GetHashCode()
	{
		return precalculatedHashCode;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (!(obj is Region region))
		{
			return false;
		}
		return region.id == id;
	}
}
