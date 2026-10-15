using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public sealed class RegionGrid : IDisposable
{
	[CompilerGenerated]
	private sealed class _003Cget_AllRegions_003Ed__17 : IEnumerable<Region>, IEnumerable, IEnumerator<Region>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Region _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RegionGrid _003C_003E4__this;

		private int _003Ccount_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003Cget_AllRegions_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RegionGrid regionGrid = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					allRegionsYielded.Add(regionGrid.regionGrid[_003Ci_003E5__3]);
					goto IL_0121;
				}
				_003C_003E1__state = -1;
				if (regionGrid.disposed)
				{
					return false;
				}
				if (!regionGrid.map.regionAndRoomUpdater.Enabled && regionGrid.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				regionGrid.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				allRegionsYielded.Clear();
				_003C_003E1__state = -3;
				_003Ccount_003E5__2 = regionGrid.map.cellIndices.NumGridCells;
				_003Ci_003E5__3 = 0;
				goto IL_0131;
				IL_0121:
				_003Ci_003E5__3++;
				goto IL_0131;
				IL_0131:
				if (_003Ci_003E5__3 < _003Ccount_003E5__2)
				{
					if (regionGrid.regionGrid[_003Ci_003E5__3] != null && regionGrid.regionGrid[_003Ci_003E5__3].valid && !allRegionsYielded.Contains(regionGrid.regionGrid[_003Ci_003E5__3]))
					{
						_003C_003E2__current = regionGrid.regionGrid[_003Ci_003E5__3];
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0121;
				}
				_003C_003Em__Finally1();
				return false;
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
			allRegionsYielded.Clear();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Region> IEnumerable<Region>.GetEnumerator()
		{
			_003Cget_AllRegions_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllRegions_003Ed__17(0)
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
	private sealed class _003Cget_AllRegions_NoRebuild_InvalidAllowed_003Ed__15 : IEnumerable<Region>, IEnumerable, IEnumerator<Region>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Region _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RegionGrid _003C_003E4__this;

		private int _003Ccount_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003Cget_AllRegions_NoRebuild_InvalidAllowed_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RegionGrid regionGrid = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					allRegionsYielded.Add(regionGrid.regionGrid[_003Ci_003E5__3]);
					goto IL_00cc;
				}
				_003C_003E1__state = -1;
				if (regionGrid.disposed)
				{
					return false;
				}
				allRegionsYielded.Clear();
				_003C_003E1__state = -3;
				_003Ccount_003E5__2 = regionGrid.map.cellIndices.NumGridCells;
				_003Ci_003E5__3 = 0;
				goto IL_00dc;
				IL_00dc:
				if (_003Ci_003E5__3 < _003Ccount_003E5__2)
				{
					if (regionGrid.regionGrid[_003Ci_003E5__3] != null && !allRegionsYielded.Contains(regionGrid.regionGrid[_003Ci_003E5__3]))
					{
						_003C_003E2__current = regionGrid.regionGrid[_003Ci_003E5__3];
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00cc;
				}
				_003C_003Em__Finally1();
				return false;
				IL_00cc:
				_003Ci_003E5__3++;
				goto IL_00dc;
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
			allRegionsYielded.Clear();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Region> IEnumerable<Region>.GetEnumerator()
		{
			_003Cget_AllRegions_NoRebuild_InvalidAllowed_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllRegions_NoRebuild_InvalidAllowed_003Ed__15(0)
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

	private readonly Map map;

	private Region[] regionGrid;

	private int curCleanIndex;

	private bool disposed;

	public List<District> allDistricts = new List<District>();

	private List<Room> allRooms = new List<Room>();

	private Dictionary<int, Room> roomLookup = new Dictionary<int, Room>();

	private static HashSet<Region> allRegionsYielded = new HashSet<Region>();

	private const int CleanSquaresPerFrame = 16;

	private HashSet<Region> drawnRegions = new HashSet<Region>();

	public IReadOnlyList<Room> AllRooms => allRooms;

	public IReadOnlyDictionary<int, Room> RoomLookup => roomLookup;

	public Region[] DirectGrid
	{
		get
		{
			if (!map.regionAndRoomUpdater.Enabled && map.regionAndRoomUpdater.AnythingToRebuild)
			{
				Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.");
			}
			map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			return regionGrid;
		}
	}

	public IEnumerable<Region> AllRegions_NoRebuild_InvalidAllowed
	{
		[IteratorStateMachine(typeof(_003Cget_AllRegions_NoRebuild_InvalidAllowed_003Ed__15))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllRegions_NoRebuild_InvalidAllowed_003Ed__15(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<Region> AllRegions
	{
		[IteratorStateMachine(typeof(_003Cget_AllRegions_003Ed__17))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllRegions_003Ed__17(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public RegionGrid(Map map)
	{
		this.map = map;
		regionGrid = new Region[map.cellIndices.NumGridCells];
	}

	public Region GetValidRegionAt(IntVec3 c)
	{
		if (disposed)
		{
			return null;
		}
		if (!c.InBounds(map))
		{
			IntVec3 intVec = c;
			Log.Error("Tried to get valid region out of bounds at " + intVec.ToString());
			return null;
		}
		if (!map.regionAndRoomUpdater.Enabled && map.regionAndRoomUpdater.AnythingToRebuild)
		{
			IntVec3 intVec = c;
			Log.Warning("Trying to get valid region at " + intVec.ToString() + " but RegionAndRoomUpdater is disabled. The result may be incorrect.");
		}
		map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
		Region region = regionGrid[map.cellIndices.CellToIndex(c)];
		if (region != null && region.valid)
		{
			return region;
		}
		return null;
	}

	public Region GetValidRegionAt_NoRebuild(IntVec3 c)
	{
		if (disposed)
		{
			return null;
		}
		if (!c.InBounds(map))
		{
			IntVec3 intVec = c;
			Log.Error("Tried to get valid region out of bounds at " + intVec.ToString());
			return null;
		}
		Region region = regionGrid[map.cellIndices.CellToIndex(c)];
		if (region != null && region.valid)
		{
			return region;
		}
		return null;
	}

	public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
	{
		if (disposed)
		{
			return null;
		}
		return regionGrid[map.cellIndices.CellToIndex(c)];
	}

	public void SetRegionAt(IntVec3 c, Region reg)
	{
		regionGrid[map.cellIndices.CellToIndex(c)] = reg;
	}

	public void RoomAdded(Room room)
	{
		allRooms.Add(room);
		roomLookup[room.ID] = room;
	}

	public void RoomRemoved(Room room)
	{
		allRooms.Remove(room);
		roomLookup.Remove(room.ID);
	}

	public void UpdateClean()
	{
		if (disposed)
		{
			return;
		}
		for (int i = 0; i < 16; i++)
		{
			if (curCleanIndex >= regionGrid.Length)
			{
				curCleanIndex = 0;
			}
			Region region = regionGrid[curCleanIndex];
			if (region != null && !region.valid)
			{
				regionGrid[curCleanIndex] = null;
			}
			curCleanIndex++;
		}
	}

	public void DebugDraw()
	{
		if (map != Find.CurrentMap || disposed)
		{
			return;
		}
		if (DebugViewSettings.drawRegionTraversal)
		{
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			currentViewRect.ClipInsideMap(map);
			foreach (IntVec3 item in currentViewRect)
			{
				Region validRegionAt = GetValidRegionAt(item);
				if (validRegionAt != null && !drawnRegions.Contains(validRegionAt))
				{
					validRegionAt.DebugDraw();
					drawnRegions.Add(validRegionAt);
				}
			}
			drawnRegions.Clear();
		}
		IntVec3 intVec = UI.MouseCell();
		if (intVec.InBounds(map))
		{
			if (DebugViewSettings.drawDistricts)
			{
				intVec.GetDistrict(map)?.DebugDraw();
			}
			if (DebugViewSettings.drawRooms)
			{
				intVec.GetRoom(map)?.DebugDraw();
			}
			if (DebugViewSettings.drawRegions || DebugViewSettings.drawRegionLinks || DebugViewSettings.drawRegionThings)
			{
				GetRegionAt_NoRebuild_InvalidAllowed(intVec)?.DebugDrawMouseover();
			}
		}
	}

	public void Dispose()
	{
		regionGrid = null;
		allDistricts.Clear();
		allRooms.Clear();
		roomLookup.Clear();
		allRegionsYielded.Clear();
		disposed = true;
	}
}
