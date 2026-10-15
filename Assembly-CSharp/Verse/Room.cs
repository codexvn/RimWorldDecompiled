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

public class Room
{
	[CompilerGenerated]
	private sealed class _003CContainedThings_003Ed__111 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Room _003C_003E4__this;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private List<Region> _003Cregions_003E5__2;

		private int _003Ci_003E5__3;

		private List<Thing> _003Cthings_003E5__4;

		private int _003Cj_003E5__5;

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
		public _003CContainedThings_003Ed__111(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cregions_003E5__2 = null;
			_003Cthings_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Room room = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00b9;
			}
			_003C_003E1__state = -1;
			room.uniqueContainedThingsOfDef.Clear();
			_003Cregions_003E5__2 = room.Regions;
			_003Ci_003E5__3 = 0;
			goto IL_00f3;
			IL_00c9:
			if (_003Cj_003E5__5 < _003Cthings_003E5__4.Count)
			{
				if (room.uniqueContainedThingsOfDef.Add(_003Cthings_003E5__4[_003Cj_003E5__5]))
				{
					_003C_003E2__current = _003Cthings_003E5__4[_003Cj_003E5__5];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b9;
			}
			_003Cthings_003E5__4 = null;
			int num2 = _003Ci_003E5__3 + 1;
			_003Ci_003E5__3 = num2;
			goto IL_00f3;
			IL_00b9:
			num2 = _003Cj_003E5__5 + 1;
			_003Cj_003E5__5 = num2;
			goto IL_00c9;
			IL_00f3:
			if (_003Ci_003E5__3 < _003Cregions_003E5__2.Count)
			{
				_003Cthings_003E5__4 = _003Cregions_003E5__2[_003Ci_003E5__3].ListerThings.ThingsOfDef(def);
				_003Cj_003E5__5 = 0;
				goto IL_00c9;
			}
			room.uniqueContainedThingsOfDef.Clear();
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
			_003CContainedThings_003Ed__111 _003CContainedThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CContainedThings_003Ed__ = this;
			}
			else
			{
				_003CContainedThings_003Ed__ = new _003CContainedThings_003Ed__111(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CContainedThings_003Ed__.def = _003C_003E3__def;
			return _003CContainedThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CContainedThings_003Ed__112<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : Thing
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Room _003C_003E4__this;

		private List<Region>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<T> _003C_003E7__wrap2;

		T IEnumerator<T>.Current
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
		public _003CContainedThings_003Ed__112(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
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
			_003C_003E7__wrap1 = default(List<Region>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Room room = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_00b7;
				}
				_003C_003E1__state = -1;
				room.uniqueContainedThingsOfDef.Clear();
				_003C_003E7__wrap1 = room.Regions.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00d1;
				IL_00b7:
				while (_003C_003E7__wrap2.MoveNext())
				{
					T current = _003C_003E7__wrap2.Current;
					if (room.uniqueContainedThingsOfDef.Add(current))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				goto IL_00d1;
				IL_00d1:
				if (_003C_003E7__wrap1.MoveNext())
				{
					Region current2 = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = current2.ListerThings.GetThingsOfType<T>().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00b7;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Region>.Enumerator);
				room.uniqueContainedThingsOfDef.Clear();
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CContainedThings_003Ed__112<T> result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CContainedThings_003Ed__112<T>(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CContainedThingsList_003Ed__113 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<ThingDef> thingDefs;

		public IEnumerable<ThingDef> _003C_003E3__thingDefs;

		public Room _003C_003E4__this;

		private IEnumerator<ThingDef> _003C_003E7__wrap1;

		private IEnumerator<Thing> _003C_003E7__wrap2;

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
		public _003CContainedThingsList_003Ed__113(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
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
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Room room = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_008e;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = thingDefs.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00a8;
				IL_008e:
				if (_003C_003E7__wrap2.MoveNext())
				{
					Thing current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				goto IL_00a8;
				IL_00a8:
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current2 = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = room.ContainedThings(current2).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_008e;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CContainedThingsList_003Ed__113 _003CContainedThingsList_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CContainedThingsList_003Ed__ = this;
			}
			else
			{
				_003CContainedThingsList_003Ed__ = new _003CContainedThingsList_003Ed__113(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CContainedThingsList_003Ed__.thingDefs = _003C_003E3__thingDefs;
			return _003CContainedThingsList_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_BorderCells_003Ed__68 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Room _003C_003E4__this;

		private IEnumerator<IntVec3> _003C_003E7__wrap1;

		private IntVec3 _003Cc_003E5__3;

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
		public _003Cget_BorderCells_003Ed__68(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Room room = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00cf;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = room.Cells.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00ed;
				IL_00e1:
				if (_003Ci_003E5__4 < 8)
				{
					IntVec3 intVec = _003Cc_003E5__3 + GenAdj.AdjacentCells[_003Ci_003E5__4];
					Region region = (_003Cc_003E5__3 + GenAdj.AdjacentCells[_003Ci_003E5__4]).GetRegion(room.Map);
					if (region == null || region.Room != room)
					{
						_003C_003E2__current = intVec;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00cf;
				}
				goto IL_00ed;
				IL_00cf:
				int num2 = _003Ci_003E5__4 + 1;
				_003Ci_003E5__4 = num2;
				goto IL_00e1;
				IL_00ed:
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003Cc_003E5__3 = _003C_003E7__wrap1.Current;
					_003Ci_003E5__4 = 0;
					goto IL_00e1;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
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
			_003Cget_BorderCells_003Ed__68 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_BorderCells_003Ed__68(0)
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
	private sealed class _003Cget_BorderCellsCardinal_003Ed__70 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Room _003C_003E4__this;

		private IEnumerator<IntVec3> _003C_003E7__wrap1;

		private IntVec3 _003Cc_003E5__3;

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
		public _003Cget_BorderCellsCardinal_003Ed__70(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Room room = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00c0;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = room.Cells.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00db;
				IL_00d2:
				if (_003Ci_003E5__4 < 4)
				{
					IntVec3 intVec = _003Cc_003E5__3 + GenAdj.CardinalDirections[_003Ci_003E5__4];
					Region region = intVec.GetRegion(room.Map);
					if (intVec.InBounds(room.Map) && (region == null || region.Room != room))
					{
						_003C_003E2__current = intVec;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00c0;
				}
				goto IL_00db;
				IL_00c0:
				int num2 = _003Ci_003E5__4 + 1;
				_003Ci_003E5__4 = num2;
				goto IL_00d2;
				IL_00db:
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003Cc_003E5__3 = _003C_003E7__wrap1.Current;
					_003Ci_003E5__4 = 0;
					goto IL_00d2;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
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
			_003Cget_BorderCellsCardinal_003Ed__70 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_BorderCellsCardinal_003Ed__70(0)
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
	private sealed class _003Cget_Cells_003Ed__51 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Room _003C_003E4__this;

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
		public _003Cget_Cells_003Ed__51(int _003C_003E1__state)
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
				Room room = _003C_003E4__this;
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
				if (_003Ci_003E5__2 < room.districts.Count)
				{
					_003C_003E7__wrap2 = room.districts[_003Ci_003E5__2].Cells.GetEnumerator();
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
			_003Cget_Cells_003Ed__51 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Cells_003Ed__51(0)
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
	private sealed class _003Cget_ContainedBeds_003Ed__82 : IEnumerable<Building_Bed>, IEnumerable, IEnumerator<Building_Bed>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Building_Bed _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Room _003C_003E4__this;

		private List<Thing> _003Cthings_003E5__2;

		private int _003Ci_003E5__3;

		Building_Bed IEnumerator<Building_Bed>.Current
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
		public _003Cget_ContainedBeds_003Ed__82(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cthings_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Room room = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0064;
			}
			_003C_003E1__state = -1;
			_003Cthings_003E5__2 = room.ContainedAndAdjacentThings;
			_003Ci_003E5__3 = 0;
			goto IL_0074;
			IL_0064:
			_003Ci_003E5__3++;
			goto IL_0074;
			IL_0074:
			if (_003Ci_003E5__3 < _003Cthings_003E5__2.Count)
			{
				if (_003Cthings_003E5__2[_003Ci_003E5__3] is Building_Bed building_Bed)
				{
					_003C_003E2__current = building_Bed;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0064;
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
		IEnumerator<Building_Bed> IEnumerable<Building_Bed>.GetEnumerator()
		{
			_003Cget_ContainedBeds_003Ed__82 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ContainedBeds_003Ed__82(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Building_Bed>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Owners_003Ed__80 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Room _003C_003E4__this;

		private IEnumerator<Building_Bed> _003C_003E7__wrap1;

		private List<Pawn> _003CassignedPawns_003E5__3;

		private int _003Ci_003E5__4;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_Owners_003Ed__80(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003CassignedPawns_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Room room = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					_003Ci_003E5__4++;
					goto IL_0176;
				}
				_003C_003E1__state = -1;
				if (room.TouchesMapEdge)
				{
					return false;
				}
				if (room.IsHuge)
				{
					return false;
				}
				if (room.Role != RoomRoleDefOf.Bedroom && room.Role != RoomRoleDefOf.PrisonCell && room.Role != RoomRoleDefOf.Barracks && room.Role != RoomRoleDefOf.PrisonBarracks)
				{
					return false;
				}
				IEnumerable<Building_Bed> enumerable = room.ContainedBeds.Where((Building_Bed x) => x.def.building.bed_humanlike);
				if (enumerable.Count() > 1 && (room.Role == RoomRoleDefOf.Barracks || room.Role == RoomRoleDefOf.PrisonBarracks) && enumerable.Where((Building_Bed b) => b.OwnersForReading.Any()).Count() > 1)
				{
					return false;
				}
				_003C_003E7__wrap1 = enumerable.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_0190;
				IL_0176:
				if (_003Ci_003E5__4 < _003CassignedPawns_003E5__3.Count)
				{
					_003C_003E2__current = _003CassignedPawns_003E5__3[_003Ci_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				_003CassignedPawns_003E5__3 = null;
				goto IL_0190;
				IL_0190:
				if (_003C_003E7__wrap1.MoveNext())
				{
					Building_Bed current = _003C_003E7__wrap1.Current;
					_003CassignedPawns_003E5__3 = current.OwnersForReading;
					_003Ci_003E5__4 = 0;
					goto IL_0176;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_Owners_003Ed__80 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Owners_003Ed__80(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public int ID = -1;

	private List<District> districts = new List<District>();

	private RoomTempTracker tempTracker;

	private int cachedOpenRoofCount = -1;

	private int cachedExposedCount = -1;

	private int cachedExposedThreshold;

	private int cachedCellCount = -1;

	private bool isPrisonCell;

	private bool statsAndRoleDirty = true;

	private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

	private RoomRoleDef role;

	private List<IntVec3> cachedBorderCells = new List<IntVec3>();

	private long cachedBorderCellsTick = -1L;

	private float vacuum;

	public bool vacuumOverlayDrawn;

	public int vacuumOverlayRects;

	private static int nextRoomID;

	private const int RegionCountHuge = 60;

	private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;

	public const float MinVaccum = -0.1f;

	private const int MaxRegionsToAssignRoomRole = 60;

	private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

	private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

	public const string VacuumChangedSignal = "Vacuum";

	private List<Region> tmpRegions = new List<Region>();

	private readonly HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

	private readonly List<Thing> uniqueContainedThings = new List<Thing>();

	private readonly HashSet<Thing> uniqueContainedThingsOfDef = new HashSet<Thing>();

	private static List<IntVec3> fields = new List<IntVec3>();

	public List<District> Districts => districts;

	public Map Map
	{
		get
		{
			if (!districts.Any())
			{
				return null;
			}
			return districts[0].Map;
		}
	}

	public int DistrictCount => districts.Count;

	public RoomTempTracker TempTracker => tempTracker;

	public float Temperature
	{
		get
		{
			return tempTracker.Temperature;
		}
		set
		{
			tempTracker.Temperature = value;
		}
	}

	public bool UsesOutdoorTemperature
	{
		get
		{
			if (!TouchesMapEdge)
			{
				return OpenRoofCount >= Mathf.CeilToInt((float)CellCount * 0.25f);
			}
			return true;
		}
	}

	public bool ExposedToSpace
	{
		get
		{
			if (Map.Biome.inVacuum)
			{
				if (!TouchesMapEdge)
				{
					return ExposedCountStopAt(1) > 0;
				}
				return true;
			}
			return false;
		}
	}

	public bool Dereferenced => RegionCount == 0;

	public bool IsHuge => RegionCount > 60;

	public bool IsPrisonCell => isPrisonCell;

	public float Vacuum
	{
		get
		{
			if (!Map.Biome.inVacuum)
			{
				return 0f;
			}
			if (ExposedToSpace)
			{
				return vacuum = 1f;
			}
			if (IsDoorway)
			{
				if (!Map.regionAndRoomUpdater.Enabled)
				{
					return 0f;
				}
				float num = 0f;
				Region region = Regions[0];
				foreach (RegionLink link in region.links)
				{
					Room room = link.GetOtherRegion(region)?.Room;
					if (room != null)
					{
						num += room.UnsanitizedVacuum;
					}
				}
				if (Mathf.Approximately(num, 0f) || region.links.Count == 0)
				{
					return 0f;
				}
				num /= (float)region.links.Count;
				return vacuum = ((num <= 0.01f) ? 0f : num);
			}
			if (!(vacuum <= 0.01f))
			{
				return vacuum;
			}
			return 0f;
		}
		set
		{
			vacuum = Mathf.Clamp(value, -0.1f, 1f);
			foreach (Thing uniqueContainedThing in uniqueContainedThings)
			{
				if (uniqueContainedThing is ThingWithComps thingWithComps)
				{
					thingWithComps.BroadcastCompSignal("Vacuum");
				}
			}
		}
	}

	internal float UnsanitizedVacuum => vacuum;

	public IEnumerable<IntVec3> Cells
	{
		[IteratorStateMachine(typeof(_003Cget_Cells_003Ed__51))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Cells_003Ed__51(-2)
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
				for (int i = 0; i < districts.Count; i++)
				{
					cachedCellCount += districts[i].CellCount;
				}
			}
			return cachedCellCount;
		}
	}

	public Region FirstRegion
	{
		get
		{
			for (int i = 0; i < districts.Count; i++)
			{
				List<Region> regions = districts[i].Regions;
				if (regions.Count > 0)
				{
					return regions[0];
				}
			}
			return null;
		}
	}

	public List<Region> Regions
	{
		get
		{
			tmpRegions.Clear();
			for (int i = 0; i < districts.Count; i++)
			{
				List<Region> regions = districts[i].Regions;
				for (int j = 0; j < regions.Count; j++)
				{
					tmpRegions.Add(regions[j]);
				}
			}
			return tmpRegions;
		}
	}

	public int RegionCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < districts.Count; i++)
			{
				num += districts[i].RegionCount;
			}
			return num;
		}
	}

	public CellRect ExtentsClose
	{
		get
		{
			CellRect result = new CellRect(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
			foreach (Region region in Regions)
			{
				if (region.extentsClose.minX < result.minX)
				{
					result.minX = region.extentsClose.minX;
				}
				if (region.extentsClose.minZ < result.minZ)
				{
					result.minZ = region.extentsClose.minZ;
				}
				if (region.extentsClose.maxX > result.maxX)
				{
					result.maxX = region.extentsClose.maxX;
				}
				if (region.extentsClose.maxZ > result.maxZ)
				{
					result.maxZ = region.extentsClose.maxZ;
				}
			}
			return result;
		}
	}

	public int OpenRoofCount
	{
		get
		{
			if (cachedOpenRoofCount == -1)
			{
				cachedOpenRoofCount = OpenRoofCountStopAt(int.MaxValue);
			}
			return cachedOpenRoofCount;
		}
	}

	public IEnumerable<IntVec3> BorderCells
	{
		[IteratorStateMachine(typeof(_003Cget_BorderCells_003Ed__68))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_BorderCells_003Ed__68(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<IntVec3> BorderCellsCardinal
	{
		[IteratorStateMachine(typeof(_003Cget_BorderCellsCardinal_003Ed__70))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_BorderCellsCardinal_003Ed__70(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<IntVec3> BorderCellsCached
	{
		get
		{
			if (GenTicks.TicksGame - cachedBorderCellsTick > 120)
			{
				cachedBorderCellsTick = GenTicks.TicksGame;
				cachedBorderCells.Clear();
				cachedBorderCells.AddRange(BorderCells);
			}
			return cachedBorderCells;
		}
	}

	public bool TouchesMapEdge
	{
		get
		{
			for (int i = 0; i < districts.Count; i++)
			{
				if (districts[i].TouchesMapEdge)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool PsychologicallyOutdoors
	{
		get
		{
			if (OpenRoofCountStopAt(300) >= 300)
			{
				return true;
			}
			if (TouchesMapEdge && (float)OpenRoofCount / (float)CellCount >= 0.5f)
			{
				return true;
			}
			return false;
		}
	}

	public bool OutdoorsForWork
	{
		get
		{
			if (OpenRoofCountStopAt(101) > 100 || (float)OpenRoofCount > (float)CellCount * 0.25f)
			{
				return true;
			}
			return false;
		}
	}

	public IEnumerable<Pawn> Owners
	{
		[IteratorStateMachine(typeof(_003Cget_Owners_003Ed__80))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Owners_003Ed__80(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<Building_Bed> ContainedBeds
	{
		[IteratorStateMachine(typeof(_003Cget_ContainedBeds_003Ed__82))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ContainedBeds_003Ed__82(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool Fogged
	{
		get
		{
			if (RegionCount == 0)
			{
				return false;
			}
			return FirstRegion.AnyCell.Fogged(Map);
		}
	}

	public bool IsDoorway
	{
		get
		{
			if (districts.Count == 1)
			{
				return districts[0].IsDoorway;
			}
			return false;
		}
	}

	public Building_Door Door
	{
		get
		{
			if (!IsDoorway)
			{
				return null;
			}
			return districts[0].Regions[0].door;
		}
	}

	public List<Thing> ContainedAndAdjacentThings
	{
		get
		{
			uniqueContainedThingsSet.Clear();
			uniqueContainedThings.Clear();
			List<Region> regions = Regions;
			for (int i = 0; i < regions.Count; i++)
			{
				List<Thing> allThings = regions[i].ListerThings.AllThings;
				if (allThings == null)
				{
					continue;
				}
				for (int j = 0; j < allThings.Count; j++)
				{
					Thing item = allThings[j];
					if (uniqueContainedThingsSet.Add(item))
					{
						uniqueContainedThings.Add(item);
					}
				}
			}
			uniqueContainedThingsSet.Clear();
			return uniqueContainedThings;
		}
	}

	public RoomRoleDef Role
	{
		get
		{
			if (statsAndRoleDirty)
			{
				UpdateRoomStatsAndRole();
			}
			return role;
		}
	}

	public bool AnyPassable
	{
		get
		{
			for (int i = 0; i < districts.Count; i++)
			{
				if (districts[i].Passable)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool ProperRoom
	{
		get
		{
			if (TouchesMapEdge)
			{
				return false;
			}
			for (int i = 0; i < districts.Count; i++)
			{
				if (districts[i].RegionType == RegionType.Normal)
				{
					return true;
				}
			}
			return false;
		}
	}

	private int OpenRoofCountStopAt(int threshold)
	{
		if (cachedOpenRoofCount != -1)
		{
			return cachedOpenRoofCount;
		}
		int num = 0;
		for (int i = 0; i < districts.Count; i++)
		{
			num += districts[i].OpenRoofCountStopAt(threshold);
			if (num >= threshold)
			{
				return num;
			}
			threshold -= num;
		}
		return num;
	}

	private int ExposedCountStopAt(int threshold)
	{
		if (cachedExposedThreshold > 0 && cachedExposedThreshold <= threshold)
		{
			return cachedExposedCount;
		}
		cachedExposedThreshold = threshold;
		int num = 0;
		for (int i = 0; i < districts.Count; i++)
		{
			num += districts[i].ExposedVacuumCount(threshold);
			if (num >= threshold)
			{
				return cachedExposedCount = num;
			}
			threshold -= num;
		}
		return cachedExposedCount = num;
	}

	public static Room MakeNew(Map map)
	{
		Room room = new Room();
		room.ID = nextRoomID;
		room.tempTracker = new RoomTempTracker(room, map);
		map.regionAndRoomUpdater.roomLookup[room.ID] = room;
		nextRoomID++;
		return room;
	}

	public void AddDistrict(District district)
	{
		if (districts.Contains(district))
		{
			Log.Error("Tried to add the same district twice to Room. district=" + district?.ToString() + ", room=" + this);
			return;
		}
		districts.Add(district);
		if (districts.Count == 1)
		{
			Map.regionGrid.RoomAdded(this);
		}
	}

	public void RemoveDistrict(District district)
	{
		if (!districts.Contains(district))
		{
			Log.Error("Tried to remove district from Room but this district is not here. district=" + district?.ToString() + ", room=" + this);
			return;
		}
		Map map = Map;
		districts.Remove(district);
		tmpRegions.Clear();
		if (districts.Count == 0)
		{
			map?.regionGrid.RoomRemoved(this);
		}
		statsAndRoleDirty = true;
	}

	public bool PushHeat(float energy)
	{
		if (UsesOutdoorTemperature)
		{
			return false;
		}
		Temperature += energy / (float)CellCount;
		return true;
	}

	public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
	{
		if (th.def.category == ThingCategory.Mote || th.def.category == ThingCategory.Projectile || th.def.category == ThingCategory.Ethereal || th.def.category == ThingCategory.Pawn)
		{
			return;
		}
		if (IsDoorway)
		{
			List<Region> regions = districts[0].Regions;
			for (int i = 0; i < regions[0].links.Count; i++)
			{
				Region otherRegion = regions[0].links[i].GetOtherRegion(regions[0]);
				if (otherRegion != null && !otherRegion.IsDoorway)
				{
					otherRegion.Room.Notify_ContainedThingSpawnedOrDespawned(th);
				}
			}
		}
		statsAndRoleDirty = true;
	}

	public void Notify_TerrainChanged()
	{
		cachedExposedCount = -1;
		cachedExposedThreshold = 0;
		statsAndRoleDirty = true;
	}

	public void Notify_BedTypeChanged()
	{
		statsAndRoleDirty = true;
	}

	public void Notify_RoofChanged()
	{
		cachedOpenRoofCount = -1;
		cachedExposedCount = -1;
		cachedExposedThreshold = 0;
		tempTracker.RoofChanged();
	}

	public void Notify_RoomShapeChanged()
	{
		cachedCellCount = -1;
		cachedOpenRoofCount = -1;
		cachedExposedCount = -1;
		cachedExposedThreshold = 0;
		if (Dereferenced)
		{
			isPrisonCell = false;
			statsAndRoleDirty = true;
			return;
		}
		tempTracker.RoomChanged();
		if (Current.ProgramState == ProgramState.Playing && !Fogged && !GravshipPlacementUtility.placingGravship)
		{
			Map.autoBuildRoofAreaSetter.TryGenerateAreaFor(this);
		}
		isPrisonCell = false;
		if (Building_Bed.RoomCanBePrisonCell(this))
		{
			List<Thing> containedAndAdjacentThings = ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Bed { ForPrisoners: not false })
				{
					isPrisonCell = true;
					break;
				}
			}
		}
		List<Thing> list = Map.listerThings.ThingsOfDef(ThingDefOf.NutrientPasteDispenser);
		for (int j = 0; j < list.Count; j++)
		{
			list[j].Notify_ColorChanged();
		}
		if (Current.ProgramState == ProgramState.Playing && isPrisonCell)
		{
			foreach (Building_Bed containedBed in ContainedBeds)
			{
				containedBed.ForPrisoners = true;
			}
		}
		statsAndRoleDirty = true;
	}

	public bool ContainsCell(IntVec3 cell)
	{
		if (Map != null)
		{
			return cell.GetRoom(Map) == this;
		}
		return false;
	}

	public bool ContainsThing(ThingDef def)
	{
		List<Region> regions = Regions;
		for (int i = 0; i < regions.Count; i++)
		{
			if (regions[i].ListerThings.ThingsOfDef(def).Any())
			{
				return true;
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CContainedThings_003Ed__111))]
	public IEnumerable<Thing> ContainedThings(ThingDef def)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CContainedThings_003Ed__111(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__def = def
		};
	}

	[IteratorStateMachine(typeof(_003CContainedThings_003Ed__112<>))]
	public IEnumerable<T> ContainedThings<T>() where T : Thing
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CContainedThings_003Ed__112<T>(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CContainedThingsList_003Ed__113))]
	public IEnumerable<Thing> ContainedThingsList(IEnumerable<ThingDef> thingDefs)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CContainedThingsList_003Ed__113(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__thingDefs = thingDefs
		};
	}

	public int ThingCount(ThingDef def)
	{
		uniqueContainedThingsOfDef.Clear();
		List<Region> regions = Regions;
		int num = 0;
		for (int i = 0; i < regions.Count; i++)
		{
			List<Thing> list = regions[i].ListerThings.ThingsOfDef(def);
			for (int j = 0; j < list.Count; j++)
			{
				if (uniqueContainedThingsOfDef.Add(list[j]))
				{
					num += list[j].stackCount;
				}
			}
		}
		uniqueContainedThingsOfDef.Clear();
		return num;
	}

	public float GetStat(RoomStatDef roomStat)
	{
		if (statsAndRoleDirty)
		{
			UpdateRoomStatsAndRole();
		}
		if (stats == null)
		{
			return roomStat.roomlessScore;
		}
		return stats[roomStat];
	}

	public void DrawFieldEdges()
	{
		fields.Clear();
		fields.AddRange(Cells);
		Color color = (isPrisonCell ? PrisonFieldColor : NonPrisonFieldColor);
		color.a = Pulser.PulseBrightness(1f, 0.6f);
		GenDraw.DrawFieldEdges(fields, color, null);
		fields.Clear();
	}

	public RoomRoleDef GetRoomRoleIfBuildingPlaced(ThingDef buildingDef)
	{
		if (statsAndRoleDirty)
		{
			UpdateRoomStatsAndRole();
		}
		if (!ProperRoom || RegionCount > 60)
		{
			return RoomRoleDefOf.None;
		}
		return DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this) + x.Worker.GetScoreDeltaIfBuildingPlaced(this, buildingDef));
	}

	private void UpdateRoomStatsAndRole()
	{
		statsAndRoleDirty = false;
		if (ProperRoom && RegionCount <= 60)
		{
			if (stats == null)
			{
				stats = new DefMap<RoomStatDef, float>();
			}
			foreach (RoomStatDef item in DefDatabase<RoomStatDef>.AllDefs.OrderByDescending((RoomStatDef x) => x.updatePriority))
			{
				stats[item] = item.Worker.GetScore(this);
			}
			role = DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this));
		}
		else
		{
			stats = null;
			role = RoomRoleDefOf.None;
		}
	}

	public string GetRoomRoleLabel()
	{
		Pawn pawn = null;
		Pawn pawn2 = null;
		foreach (Pawn owner in Owners)
		{
			if (pawn == null)
			{
				pawn = owner;
			}
			else
			{
				pawn2 = owner;
			}
		}
		TaggedString taggedString = ((pawn == null) ? ((TaggedString)Role.PostProcessedLabel(this)) : ((pawn2 != null && pawn2 != pawn) ? "CouplesRoom".Translate(pawn.LabelShort, pawn2.LabelShort, Role.label, pawn.Named("PAWN1"), pawn2.Named("PAWN2")) : "SomeonesRoom".Translate(pawn.LabelShort, Role.label, pawn.Named("PAWN"))));
		return taggedString;
	}

	public string DebugString()
	{
		return "Room ID=" + ID + "\n  first cell=" + Cells.FirstOrDefault().ToString() + "\n  DistrictCount=" + DistrictCount + "\n  RegionCount=" + RegionCount + "\n  CellCount=" + CellCount + "\n  OpenRoofCount=" + OpenRoofCount + "\n  PsychologicallyOutdoors=" + PsychologicallyOutdoors + "\n  OutdoorsForWork=" + OutdoorsForWork + "\n  WellEnclosed=" + ProperRoom + "\n  " + tempTracker.DebugString() + (DebugViewSettings.writeRoomRoles ? ("\n" + DebugRolesString()) : "");
	}

	private string DebugRolesString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (var (value, value2) in from x in DefDatabase<RoomRoleDef>.AllDefs
			select (score: x.Worker.GetScore(this), role: x) into tuple
			orderby tuple.score descending
			select tuple)
		{
			stringBuilder.AppendLine($"{value}: {value2}");
		}
		return stringBuilder.ToString();
	}

	internal void DebugDraw()
	{
		int num = Gen.HashCombineInt(GetHashCode(), 1948571531);
		foreach (IntVec3 cell in Cells)
		{
			CellRenderer.RenderCell(cell, (float)num * 0.01f);
		}
		tempTracker.DebugDraw();
	}

	public override string ToString()
	{
		return "Room(roomID=" + ID + ", first=" + Cells.FirstOrDefault().ToString() + ", RegionsCount=" + RegionCount + ")";
	}

	public override int GetHashCode()
	{
		return Gen.HashCombineInt(ID, 1538478891);
	}
}
