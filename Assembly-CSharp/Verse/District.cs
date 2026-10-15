using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public sealed class District
{
	[CompilerGenerated]
	private sealed class _003Cget_Cells_003Ed__37 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public District _003C_003E4__this;

		private int _003Ci_003E5__2;

		private IEnumerator<IntVec3> _003C_003E7__wrap2;

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
		public _003Cget_Cells_003Ed__37(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				District district = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_007d;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00a9;
				IL_007d:
				if (_003C_003E7__wrap2.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				_003Ci_003E5__2++;
				goto IL_00a9;
				IL_00a9:
				if (_003Ci_003E5__2 < district.regions.Count)
				{
					_003C_003E7__wrap2 = district.regions[_003Ci_003E5__2].Cells.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007d;
				}
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003Cget_Cells_003Ed__37 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Cells_003Ed__37(0)
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

	public sbyte mapIndex = -1;

	private Room roomInt;

	private List<Region> regions = new List<Region>();

	public int ID = -16161616;

	public int lastChangeTick = -1;

	private int numRegionsTouchingMapEdge;

	private int cachedOpenRoofCount = -1;

	private int cachedExposedCount = -1;

	private IEnumerator<IntVec3> cachedOpenRoofState;

	private IEnumerator<IntVec3> cachedExposedState;

	private int cachedCellCount = -1;

	public int newOrReusedRoomIndex = -1;

	private static int nextDistrictID;

	private HashSet<District> uniqueNeighborsSet = new HashSet<District>();

	private List<District> uniqueNeighbors = new List<District>();

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

	public RegionType RegionType
	{
		get
		{
			if (!regions.Any())
			{
				return RegionType.None;
			}
			return regions[0].type;
		}
	}

	public List<Region> Regions => regions;

	public int RegionCount => regions.Count;

	public bool TouchesMapEdge => numRegionsTouchingMapEdge > 0;

	public bool Passable => RegionType.Passable();

	public bool IsDoorway
	{
		get
		{
			if (regions.Count == 1)
			{
				return regions[0].IsDoorway;
			}
			return false;
		}
	}

	public Room Room
	{
		get
		{
			return roomInt;
		}
		set
		{
			if (value != roomInt)
			{
				if (roomInt != null)
				{
					roomInt.RemoveDistrict(this);
				}
				roomInt = value;
				if (roomInt != null)
				{
					roomInt.AddDistrict(this);
				}
			}
		}
	}

	public int CellCount
	{
		get
		{
			if (cachedCellCount == -1)
			{
				cachedCellCount = 0;
				for (int i = 0; i < regions.Count; i++)
				{
					cachedCellCount += regions[i].CellCount;
				}
			}
			return cachedCellCount;
		}
	}

	public List<District> Neighbors
	{
		get
		{
			uniqueNeighborsSet.Clear();
			uniqueNeighbors.Clear();
			for (int i = 0; i < regions.Count; i++)
			{
				foreach (Region neighbor in regions[i].Neighbors)
				{
					if (uniqueNeighborsSet.Add(neighbor.District) && neighbor.District != this)
					{
						uniqueNeighbors.Add(neighbor.District);
					}
				}
			}
			uniqueNeighborsSet.Clear();
			return uniqueNeighbors;
		}
	}

	public IEnumerable<IntVec3> Cells
	{
		[IteratorStateMachine(typeof(_003Cget_Cells_003Ed__37))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Cells_003Ed__37(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public static District MakeNew(Map map)
	{
		District result = new District
		{
			mapIndex = (sbyte)map.Index,
			ID = nextDistrictID
		};
		nextDistrictID++;
		return result;
	}

	public void AddRegion(Region r)
	{
		if (regions.Contains(r))
		{
			Log.Error("Tried to add the same region twice to District. region=" + r?.ToString() + ", district=" + this);
			return;
		}
		regions.Add(r);
		if (r.touchesMapEdge)
		{
			numRegionsTouchingMapEdge++;
		}
		if (regions.Count == 1)
		{
			Map.regionGrid.allDistricts.Add(this);
		}
	}

	public void RemoveRegion(Region r)
	{
		if (!regions.Contains(r))
		{
			Log.Error("Tried to remove region from District but this region is not here. region=" + r?.ToString() + ", district=" + this);
			return;
		}
		regions.Remove(r);
		if (r.touchesMapEdge)
		{
			numRegionsTouchingMapEdge--;
		}
		if (regions.Count == 0)
		{
			Room = null;
			cachedOpenRoofCount = -1;
			cachedExposedCount = -1;
			cachedOpenRoofState?.Dispose();
			cachedOpenRoofState = null;
			cachedExposedState?.Dispose();
			cachedExposedState = null;
			Map?.regionGrid.allDistricts.Remove(this);
		}
	}

	public void Notify_MyMapRemoved()
	{
		mapIndex = -1;
	}

	public void Notify_TerrainChanged()
	{
		cachedExposedCount = -1;
		cachedExposedState?.Dispose();
		cachedExposedState = null;
		Room.Notify_TerrainChanged();
	}

	public void Notify_RoofChanged()
	{
		cachedOpenRoofCount = -1;
		cachedExposedCount = -1;
		cachedOpenRoofState?.Dispose();
		cachedOpenRoofState = null;
		cachedExposedState?.Dispose();
		cachedExposedState = null;
		Room.Notify_RoofChanged();
	}

	public void Notify_RoomShapeOrContainedBedsChanged()
	{
		cachedCellCount = -1;
		cachedOpenRoofCount = -1;
		cachedExposedCount = -1;
		cachedOpenRoofState?.Dispose();
		cachedOpenRoofState = null;
		cachedExposedState?.Dispose();
		cachedExposedState = null;
		AnimalPenConnectedDistrictsCalculator.InvalidateDistrictCache(this);
		lastChangeTick = Find.TickManager.TicksGame;
		FacilitiesUtility.NotifyFacilitiesAboutChangedLOSBlockers(regions);
	}

	public void DecrementMapIndex()
	{
		if (mapIndex <= 0)
		{
			Log.Warning("Tried to decrement map index for district " + ID + ", but mapIndex=" + mapIndex);
		}
		else
		{
			mapIndex--;
		}
	}

	public int OpenRoofCountStopAt(int threshold)
	{
		if (cachedOpenRoofCount == -1 && cachedOpenRoofState == null)
		{
			cachedOpenRoofCount = 0;
			cachedOpenRoofState = Cells.GetEnumerator();
		}
		if (cachedOpenRoofCount < threshold && cachedOpenRoofState != null)
		{
			RoofGrid roofGrid = Map.roofGrid;
			while (cachedOpenRoofCount < threshold && cachedOpenRoofState.MoveNext())
			{
				if (!roofGrid.Roofed(cachedOpenRoofState.Current))
				{
					cachedOpenRoofCount++;
				}
			}
			if (cachedOpenRoofCount < threshold)
			{
				cachedOpenRoofState.Dispose();
				cachedOpenRoofState = null;
			}
		}
		return cachedOpenRoofCount;
	}

	public int ExposedVacuumCount(int threshold)
	{
		if (cachedExposedCount == -1 && cachedExposedState == null)
		{
			cachedExposedCount = 0;
			cachedExposedState = Cells.GetEnumerator();
		}
		if (cachedExposedCount < threshold && cachedExposedState != null)
		{
			RoofGrid roofGrid = Map.roofGrid;
			TerrainGrid terrainGrid = Map.terrainGrid;
			while (cachedExposedCount < threshold && cachedExposedState.MoveNext())
			{
				IntVec3 current = cachedExposedState.Current;
				if (!roofGrid.Roofed(current) || terrainGrid.TerrainAt(current).exposesToVacuum)
				{
					cachedExposedCount++;
				}
			}
			if (cachedExposedCount < threshold)
			{
				cachedExposedState.Dispose();
				cachedExposedState = null;
			}
		}
		return cachedExposedCount;
	}

	internal void DebugDraw()
	{
		int hashCode = GetHashCode();
		foreach (IntVec3 cell in Cells)
		{
			CellRenderer.RenderCell(cell, (float)hashCode * 0.01f);
		}
	}

	internal string DebugString()
	{
		return "District ID=" + ID + "\n  first cell=" + Cells.FirstOrDefault().ToString() + "\n  RegionCount=" + RegionCount + "\n  RegionType=" + RegionType.ToString() + "\n  CellCount=" + CellCount + "\n  OpenRoofCount=" + OpenRoofCountStopAt(int.MaxValue) + "\n  numRegionsTouchingMapEdge=" + numRegionsTouchingMapEdge + "\n  lastChangeTick=" + lastChangeTick + "\n  Room=" + ((Room != null) ? Room.ID.ToString() : "null");
	}

	public override string ToString()
	{
		return "District(districtID=" + ID + ", first=" + Cells.FirstOrDefault().ToString() + ", RegionsCount=" + RegionCount + ", lastChangeTick=" + lastChangeTick + ")";
	}

	public override int GetHashCode()
	{
		return Gen.HashCombineInt(ID, 1538478890);
	}
}
