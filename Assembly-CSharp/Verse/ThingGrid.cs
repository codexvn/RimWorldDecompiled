using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public sealed class ThingGrid
{
	[CompilerGenerated]
	private sealed class _003CThingsAt_003Ed__7 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 c;

		public IntVec3 _003C_003E3__c;

		public ThingGrid _003C_003E4__this;

		private List<Thing> _003Clist_003E5__2;

		private int _003Ci_003E5__3;

		Thing IEnumerator<Thing>.Current
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
		public _003CThingsAt_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clist_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ThingGrid thingGrid = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!c.InBounds(thingGrid.map))
				{
					return false;
				}
				_003Clist_003E5__2 = thingGrid.thingGrid[thingGrid.map.cellIndices.CellToIndex(c)];
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003Clist_003E5__2.Count)
			{
				_003C_003E2__current = _003Clist_003E5__2[_003Ci_003E5__3];
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CThingsAt_003Ed__7 _003CThingsAt_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CThingsAt_003Ed__ = this;
			}
			else
			{
				_003CThingsAt_003Ed__ = new _003CThingsAt_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CThingsAt_003Ed__.c = _003C_003E3__c;
			return _003CThingsAt_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private Map map;

	private List<Thing>[] thingGrid;

	private static readonly List<Thing> EmptyThingList = new List<Thing>();

	public ThingGrid(Map map)
	{
		this.map = map;
		CellIndices cellIndices = map.cellIndices;
		thingGrid = new List<Thing>[cellIndices.NumGridCells];
		for (int i = 0; i < cellIndices.NumGridCells; i++)
		{
			thingGrid[i] = new List<Thing>(4);
		}
	}

	public void Register(Thing t)
	{
		if (t.def.size.x == 1 && t.def.size.z == 1)
		{
			RegisterInCell(t, t.Position);
			return;
		}
		CellRect cellRect = t.OccupiedRect();
		for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
		{
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				RegisterInCell(t, new IntVec3(j, 0, i));
			}
		}
	}

	private void RegisterInCell(Thing t, IntVec3 c)
	{
		if (!c.InBounds(map))
		{
			string obj = t?.ToString();
			IntVec3 intVec = c;
			Log.Warning(obj + " tried to register out of bounds at " + intVec.ToString() + ". Destroying.");
			t.Destroy();
		}
		else
		{
			thingGrid[map.cellIndices.CellToIndex(c)].Add(t);
		}
	}

	public void Deregister(Thing t, bool doEvenIfDespawned = false)
	{
		if (!t.Spawned && !doEvenIfDespawned)
		{
			return;
		}
		if (t.def.size.x == 1 && t.def.size.z == 1)
		{
			DeregisterInCell(t, t.Position);
			return;
		}
		CellRect cellRect = t.OccupiedRect();
		for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
		{
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				DeregisterInCell(t, new IntVec3(j, 0, i));
			}
		}
	}

	private void DeregisterInCell(Thing t, IntVec3 c)
	{
		if (!c.InBounds(map))
		{
			string obj = t?.ToString();
			IntVec3 intVec = c;
			Log.Error(obj + " tried to de-register out of bounds at " + intVec.ToString());
			return;
		}
		int num = map.cellIndices.CellToIndex(c);
		if (thingGrid[num].Contains(t))
		{
			thingGrid[num].Remove(t);
		}
	}

	[IteratorStateMachine(typeof(_003CThingsAt_003Ed__7))]
	public IEnumerable<Thing> ThingsAt(IntVec3 c)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CThingsAt_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__c = c
		};
	}

	public List<Thing> ThingsListAt(IntVec3 c)
	{
		if (!c.InBounds(map))
		{
			IntVec3 intVec = c;
			Log.ErrorOnce("Got ThingsListAt out of bounds: " + intVec.ToString(), 495287);
			return EmptyThingList;
		}
		return thingGrid[map.cellIndices.CellToIndex(c)];
	}

	public List<Thing> ThingsListAtFast(IntVec3 c)
	{
		return thingGrid[map.cellIndices.CellToIndex(c)];
	}

	public List<Thing> ThingsListAtFast(int index)
	{
		return thingGrid[index];
	}

	public bool CellContains(IntVec3 c, ThingCategory cat)
	{
		return ThingAt(c, cat) != null;
	}

	public Thing ThingAt(IntVec3 c, ThingCategory cat)
	{
		if (!c.InBounds(map))
		{
			return null;
		}
		List<Thing> list = thingGrid[map.cellIndices.CellToIndex(c)];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].def.category == cat)
			{
				return list[i];
			}
		}
		return null;
	}

	public bool CellContains(IntVec3 c, ThingDef def)
	{
		return ThingAt(c, def) != null;
	}

	public Thing ThingAt(IntVec3 c, ThingDef def)
	{
		if (!c.InBounds(map))
		{
			return null;
		}
		List<Thing> list = thingGrid[map.cellIndices.CellToIndex(c)];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].def == def)
			{
				return list[i];
			}
		}
		return null;
	}

	public T ThingAt<T>(IntVec3 c) where T : Thing
	{
		if (!c.InBounds(map))
		{
			return null;
		}
		List<Thing> list = thingGrid[map.cellIndices.CellToIndex(c)];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] is T result)
			{
				return result;
			}
		}
		return null;
	}
}
