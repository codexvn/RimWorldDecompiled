using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class DoorUtility
{
	[CompilerGenerated]
	private sealed class _003CWallRequirementCells_003Ed__0 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private IntVec3 pos;

		public IntVec3 _003C_003E3__pos;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private CellRect _003Crect_003E5__2;

		private int _003Cmax_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003CWallRequirementCells_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (def.size == IntVec2.One)
				{
					return false;
				}
				_003Crect_003E5__2 = GenAdj.OccupiedRect(IntVec3.Zero, def.defaultPlacingRot, def.size);
				_003Cmax_003E5__3 = (def.defaultPlacingRot.IsHorizontal ? _003Crect_003E5__2.Width : _003Crect_003E5__2.Height);
				_003Ci_003E5__4 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = pos + new IntVec3(_003Crect_003E5__2.maxX + 1, 0, _003Crect_003E5__2.minZ + _003Ci_003E5__4).RotatedBy(rot);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__4++;
				break;
			}
			if (_003Ci_003E5__4 < _003Cmax_003E5__3)
			{
				_003C_003E2__current = pos + new IntVec3(_003Crect_003E5__2.minX - 1, 0, _003Crect_003E5__2.minZ + _003Ci_003E5__4).RotatedBy(rot);
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CWallRequirementCells_003Ed__0 _003CWallRequirementCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CWallRequirementCells_003Ed__ = this;
			}
			else
			{
				_003CWallRequirementCells_003Ed__ = new _003CWallRequirementCells_003Ed__0(0);
			}
			_003CWallRequirementCells_003Ed__.def = _003C_003E3__def;
			_003CWallRequirementCells_003Ed__.pos = _003C_003E3__pos;
			_003CWallRequirementCells_003Ed__.rot = _003C_003E3__rot;
			return _003CWallRequirementCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CWallRequirementCells_003Ed__0))]
	public static IEnumerable<IntVec3> WallRequirementCells(ThingDef def, IntVec3 pos, Rot4 rot)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWallRequirementCells_003Ed__0(-2)
		{
			_003C_003E3__def = def,
			_003C_003E3__pos = pos,
			_003C_003E3__rot = rot
		};
	}

	public static bool EncapsulatingWallAt(IntVec3 cell, Map map, bool includeUnbuilt = false)
	{
		if (!cell.InBounds(map))
		{
			return false;
		}
		if (includeUnbuilt)
		{
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is Blueprint blueprint && blueprint.def.entityDefToBuild is ThingDef { Fillage: FillCategory.Full, IsDoor: false })
				{
					return true;
				}
				if (thingList[i] is Frame frame && frame.BuildDef.Fillage == FillCategory.Full && !frame.BuildDef.IsDoor)
				{
					return true;
				}
			}
		}
		Building edifice = cell.GetEdifice(map);
		if (edifice == null || edifice.def.Fillage != FillCategory.Full || edifice.def.IsDoor)
		{
			return false;
		}
		return true;
	}

	public static Rot4 DoorRotationAt(IntVec3 loc, Map map, bool preferFences)
	{
		int num = 0;
		int num2 = 0 + AlignQualityAgainst(loc, IntVec3.East, map, preferFences) + AlignQualityAgainst(loc, IntVec3.West, map, preferFences);
		num += AlignQualityAgainst(loc, IntVec3.North, map, preferFences);
		num += AlignQualityAgainst(loc, IntVec3.South, map, preferFences);
		if (num2 >= num)
		{
			return Rot4.North;
		}
		return Rot4.East;
	}

	private static int AlignQualityAgainst(IntVec3 c, IntVec3 offset, Map map, bool preferFences)
	{
		IntVec3 c2 = c + offset;
		if (!c2.InBounds(map))
		{
			return 0;
		}
		if (!c2.WalkableByNormal(map))
		{
			return 9;
		}
		List<Thing> thingList = c2.GetThingList(map);
		for (int i = 0; i < thingList.Count; i++)
		{
			Thing thing = thingList[i];
			if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
			{
				if ((c - offset).GetDoor(map) == null)
				{
					return 1;
				}
				return 5;
			}
			if (thing.def.IsFence)
			{
				if (!preferFences)
				{
					return 1;
				}
				return 10;
			}
			Thing thing2 = thing as Blueprint;
			if (thing2 == null)
			{
				continue;
			}
			if (thing2.def.entityDefToBuild.passability == Traversability.Impassable)
			{
				return 9;
			}
			if (thing2.def.entityDefToBuild is ThingDef { IsFence: not false })
			{
				if (!preferFences)
				{
					return 1;
				}
				return 10;
			}
			if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
			{
				return 1;
			}
		}
		return 0;
	}
}
