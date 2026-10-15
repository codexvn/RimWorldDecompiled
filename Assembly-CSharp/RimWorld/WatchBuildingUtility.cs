using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public static class WatchBuildingUtility
{
	[CompilerGenerated]
	private sealed class _003CCalculateWatchCells_003Ed__0 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private Map map;

		public Map _003C_003E3__map;

		private List<int> _003CallowedDirections_003E5__2;

		private int _003Ci_003E5__3;

		private CellRect.Enumerator _003C_003E7__wrap3;

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
		public _003CCalculateWatchCells_003Ed__0(int _003C_003E1__state)
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
			_003CallowedDirections_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00c3;
				}
				_003C_003E1__state = -1;
				_003CallowedDirections_003E5__2 = CalculateAllowedDirections(def, rot);
				_003Ci_003E5__3 = 0;
				goto IL_00e8;
				IL_00c3:
				while (_003C_003E7__wrap3.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap3.Current;
					if (EverPossibleToWatchFrom(current, center, map, bedAllowed: true, def))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003Ci_003E5__3++;
				goto IL_00e8;
				IL_00e8:
				if (_003Ci_003E5__3 < _003CallowedDirections_003E5__2.Count)
				{
					_003C_003E7__wrap3 = GetWatchCellRect(def, center, rot, _003CallowedDirections_003E5__2[_003Ci_003E5__3]).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00c3;
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
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CCalculateWatchCells_003Ed__0 _003CCalculateWatchCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCalculateWatchCells_003Ed__ = this;
			}
			else
			{
				_003CCalculateWatchCells_003Ed__ = new _003CCalculateWatchCells_003Ed__0(0);
			}
			_003CCalculateWatchCells_003Ed__.def = _003C_003E3__def;
			_003CCalculateWatchCells_003Ed__.center = _003C_003E3__center;
			_003CCalculateWatchCells_003Ed__.rot = _003C_003E3__rot;
			_003CCalculateWatchCells_003Ed__.map = _003C_003E3__map;
			return _003CCalculateWatchCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private static List<int> allowedDirections = new List<int>();

	[IteratorStateMachine(typeof(_003CCalculateWatchCells_003Ed__0))]
	public static IEnumerable<IntVec3> CalculateWatchCells(ThingDef def, IntVec3 center, Rot4 rot, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCalculateWatchCells_003Ed__0(-2)
		{
			_003C_003E3__def = def,
			_003C_003E3__center = center,
			_003C_003E3__rot = rot,
			_003C_003E3__map = map
		};
	}

	public static bool TryFindBestWatchCell(Thing toWatch, Pawn pawn, bool desireSit, out IntVec3 result, out Building chair)
	{
		List<int> list = CalculateAllowedDirections(toWatch.def, toWatch.Rotation);
		list.Shuffle();
		IntVec3 intVec = IntVec3.Invalid;
		for (int i = 0; i < list.Count; i++)
		{
			CellRect watchCellRect = GetWatchCellRect(toWatch.def, toWatch.Position, toWatch.Rotation, list[i]);
			IntVec3 centerCell = watchCellRect.CenterCell;
			int num = watchCellRect.Area * 4;
			for (int j = 0; j < num; j++)
			{
				IntVec3 intVec2 = centerCell + GenRadial.RadialPattern[j];
				if (!watchCellRect.Contains(intVec2))
				{
					continue;
				}
				bool flag = false;
				Building building = null;
				if (EverPossibleToWatchFrom(intVec2, toWatch.Position, toWatch.Map, bedAllowed: false, toWatch.def) && !intVec2.IsForbidden(pawn) && pawn.CanReserveSittableOrSpot(intVec2) && pawn.Map.pawnDestinationReservationManager.CanReserve(intVec2, pawn))
				{
					if (desireSit)
					{
						building = intVec2.GetEdifice(pawn.Map);
						if (building != null && building.def.building.isSittable && pawn.CanReserve(building))
						{
							flag = true;
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					if (!desireSit || !(building.Rotation != new Rot4(list[i]).Opposite))
					{
						result = intVec2;
						chair = building;
						return true;
					}
					intVec = intVec2;
				}
			}
		}
		if (intVec.IsValid)
		{
			result = intVec;
			chair = intVec.GetEdifice(pawn.Map);
			return true;
		}
		result = IntVec3.Invalid;
		chair = null;
		return false;
	}

	public static bool CanWatchFromBed(Pawn pawn, Building_Bed bed, Thing toWatch)
	{
		if (!EverPossibleToWatchFrom(pawn.Position, toWatch.Position, pawn.Map, bedAllowed: true, toWatch.def))
		{
			return false;
		}
		if (toWatch.def.rotatable)
		{
			Rot4 rotation = bed.Rotation;
			CellRect cellRect = toWatch.OccupiedRect();
			if (rotation == Rot4.North && cellRect.maxZ < pawn.Position.z)
			{
				return false;
			}
			if (rotation == Rot4.South && cellRect.minZ > pawn.Position.z)
			{
				return false;
			}
			if (rotation == Rot4.East && cellRect.maxX < pawn.Position.x)
			{
				return false;
			}
			if (rotation == Rot4.West && cellRect.minX > pawn.Position.x)
			{
				return false;
			}
		}
		List<int> list = CalculateAllowedDirections(toWatch.def, toWatch.Rotation);
		for (int i = 0; i < list.Count; i++)
		{
			if (GetWatchCellRect(toWatch.def, toWatch.Position, toWatch.Rotation, list[i]).Contains(pawn.Position))
			{
				return true;
			}
		}
		return false;
	}

	private static CellRect GetWatchCellRect(ThingDef def, IntVec3 center, Rot4 rot, int watchRot)
	{
		Rot4 rot2 = new Rot4(watchRot);
		if (def.building == null)
		{
			def = def.entityDefToBuild as ThingDef;
		}
		CellRect result;
		if (rot2.IsHorizontal)
		{
			int num = center.x + GenAdj.CardinalDirections[watchRot].x * def.building.watchBuildingStandDistanceRange.min;
			int num2 = center.x + GenAdj.CardinalDirections[watchRot].x * def.building.watchBuildingStandDistanceRange.max;
			int num3 = center.z + def.building.watchBuildingStandRectWidth / 2;
			int num4 = center.z - def.building.watchBuildingStandRectWidth / 2;
			if (def.building.watchBuildingStandRectWidth % 2 == 0)
			{
				if (rot2 == Rot4.West)
				{
					num4++;
				}
				else
				{
					num3--;
				}
			}
			result = new CellRect(Mathf.Min(num, num2), num4, Mathf.Abs(num - num2) + 1, num3 - num4 + 1);
		}
		else
		{
			int num5 = center.z + GenAdj.CardinalDirections[watchRot].z * def.building.watchBuildingStandDistanceRange.min;
			int num6 = center.z + GenAdj.CardinalDirections[watchRot].z * def.building.watchBuildingStandDistanceRange.max;
			int num7 = center.x + def.building.watchBuildingStandRectWidth / 2;
			int num8 = center.x - def.building.watchBuildingStandRectWidth / 2;
			if (def.building.watchBuildingStandRectWidth % 2 == 0)
			{
				if (rot2 == Rot4.North)
				{
					num8++;
				}
				else
				{
					num7--;
				}
			}
			result = new CellRect(num8, Mathf.Min(num5, num6), num7 - num8 + 1, Mathf.Abs(num5 - num6) + 1);
		}
		return result;
	}

	private static bool EverPossibleToWatchFrom(IntVec3 watchCell, IntVec3 buildingCenter, Map map, bool bedAllowed, ThingDef def)
	{
		if (!watchCell.InBounds(map))
		{
			return false;
		}
		Room room = ((def.building != null && def.building.watchBuildingInSameRoom) ? buildingCenter.GetRoom(map) : null);
		if ((room == null || room.ContainsCell(watchCell)) && (watchCell.Standable(map) || (bedAllowed && watchCell.GetEdifice(map) is Building_Bed)))
		{
			return GenSight.LineOfSight(buildingCenter, watchCell, map, skipFirstCell: true);
		}
		return false;
	}

	private static List<int> CalculateAllowedDirections(ThingDef toWatchDef, Rot4 toWatchRot)
	{
		allowedDirections.Clear();
		if (toWatchDef.rotatable)
		{
			allowedDirections.Add(toWatchRot.AsInt);
		}
		else
		{
			allowedDirections.Add(0);
			allowedDirections.Add(1);
			allowedDirections.Add(2);
			allowedDirections.Add(3);
		}
		return allowedDirections;
	}
}
