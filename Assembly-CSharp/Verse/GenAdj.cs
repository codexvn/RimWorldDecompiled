using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public static class GenAdj
{
	[CompilerGenerated]
	private sealed class _003CCellsAdjacent8Way_003Ed__16 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TargetInfo pack;

		public TargetInfo _003C_003E3__pack;

		private IEnumerator<IntVec3> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003CCellsAdjacent8Way_003Ed__16(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (pack.HasThing)
					{
						_003C_003E7__wrap1 = CellsAdjacent8Way(pack.Thing).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0082;
					}
					_003Ci_003E5__3 = 0;
					goto IL_00ef;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					{
						_003C_003E1__state = -1;
						_003Ci_003E5__3++;
						goto IL_00ef;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					break;
					IL_00ef:
					if (_003Ci_003E5__3 < 8)
					{
						_003C_003E2__current = pack.Cell + AdjacentCells[_003Ci_003E5__3];
						_003C_003E1__state = 2;
						return true;
					}
					break;
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
			_003CCellsAdjacent8Way_003Ed__16 _003CCellsAdjacent8Way_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCellsAdjacent8Way_003Ed__ = this;
			}
			else
			{
				_003CCellsAdjacent8Way_003Ed__ = new _003CCellsAdjacent8Way_003Ed__16(0);
			}
			_003CCellsAdjacent8Way_003Ed__.pack = _003C_003E3__pack;
			return _003CCellsAdjacent8Way_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCellsAdjacent8Way_003Ed__18 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 thingCenter;

		public IntVec3 _003C_003E3__thingCenter;

		private IntVec2 thingSize;

		public IntVec2 _003C_003E3__thingSize;

		private Rot4 thingRot;

		public Rot4 _003C_003E3__thingRot;

		private int _003CminX_003E5__2;

		private int _003CmaxX_003E5__3;

		private int _003CminZ_003E5__4;

		private int _003CmaxZ_003E5__5;

		private IntVec3 _003Ccur_003E5__6;

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
		public _003CCellsAdjacent8Way_003Ed__18(int _003C_003E1__state)
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
				AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
				_003CminX_003E5__2 = thingCenter.x - (thingSize.x - 1) / 2 - 1;
				_003CmaxX_003E5__3 = _003CminX_003E5__2 + thingSize.x + 1;
				_003CminZ_003E5__4 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
				_003CmaxZ_003E5__5 = _003CminZ_003E5__4 + thingSize.z + 1;
				_003Ccur_003E5__6 = new IntVec3(_003CminX_003E5__2 - 1, 0, _003CminZ_003E5__4);
				goto IL_00d5;
			case 1:
				_003C_003E1__state = -1;
				if (_003Ccur_003E5__6.x < _003CmaxX_003E5__3)
				{
					goto IL_00d5;
				}
				goto IL_0114;
			case 2:
				_003C_003E1__state = -1;
				if (_003Ccur_003E5__6.z < _003CmaxZ_003E5__5)
				{
					goto IL_0114;
				}
				goto IL_0153;
			case 3:
				_003C_003E1__state = -1;
				if (_003Ccur_003E5__6.x > _003CminX_003E5__2)
				{
					goto IL_0153;
				}
				goto IL_0192;
			case 4:
				{
					_003C_003E1__state = -1;
					if (_003Ccur_003E5__6.z <= _003CminZ_003E5__4 + 1)
					{
						return false;
					}
					goto IL_0192;
				}
				IL_0192:
				_003Ccur_003E5__6.z--;
				_003C_003E2__current = _003Ccur_003E5__6;
				_003C_003E1__state = 4;
				return true;
				IL_0153:
				_003Ccur_003E5__6.x--;
				_003C_003E2__current = _003Ccur_003E5__6;
				_003C_003E1__state = 3;
				return true;
				IL_0114:
				_003Ccur_003E5__6.z++;
				_003C_003E2__current = _003Ccur_003E5__6;
				_003C_003E1__state = 2;
				return true;
				IL_00d5:
				_003Ccur_003E5__6.x++;
				_003C_003E2__current = _003Ccur_003E5__6;
				_003C_003E1__state = 1;
				return true;
			}
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
			_003CCellsAdjacent8Way_003Ed__18 _003CCellsAdjacent8Way_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCellsAdjacent8Way_003Ed__ = this;
			}
			else
			{
				_003CCellsAdjacent8Way_003Ed__ = new _003CCellsAdjacent8Way_003Ed__18(0);
			}
			_003CCellsAdjacent8Way_003Ed__.thingCenter = _003C_003E3__thingCenter;
			_003CCellsAdjacent8Way_003Ed__.thingRot = _003C_003E3__thingRot;
			_003CCellsAdjacent8Way_003Ed__.thingSize = _003C_003E3__thingSize;
			return _003CCellsAdjacent8Way_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCellsAdjacent8WayAndInside_003Ed__22 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing thing;

		public Thing _003C_003E3__thing;

		private int _003CminZ_003E5__2;

		private int _003CmaxX_003E5__3;

		private int _003CmaxZ_003E5__4;

		private int _003Ci_003E5__5;

		private int _003Cj_003E5__6;

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
		public _003CCellsAdjacent8WayAndInside_003Ed__22(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Cj_003E5__6++;
				goto IL_00f3;
			}
			_003C_003E1__state = -1;
			IntVec3 center = thing.Position;
			IntVec2 size = thing.def.size;
			Rot4 rotation = thing.Rotation;
			AdjustForRotation(ref center, ref size, rotation);
			int num2 = center.x - (size.x - 1) / 2 - 1;
			_003CminZ_003E5__2 = center.z - (size.z - 1) / 2 - 1;
			_003CmaxX_003E5__3 = num2 + size.x + 1;
			_003CmaxZ_003E5__4 = _003CminZ_003E5__2 + size.z + 1;
			_003Ci_003E5__5 = num2;
			goto IL_0113;
			IL_00f3:
			if (_003Cj_003E5__6 <= _003CmaxZ_003E5__4)
			{
				_003C_003E2__current = new IntVec3(_003Ci_003E5__5, 0, _003Cj_003E5__6);
				_003C_003E1__state = 1;
				return true;
			}
			_003Ci_003E5__5++;
			goto IL_0113;
			IL_0113:
			if (_003Ci_003E5__5 <= _003CmaxX_003E5__3)
			{
				_003Cj_003E5__6 = _003CminZ_003E5__2;
				goto IL_00f3;
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
			_003CCellsAdjacent8WayAndInside_003Ed__22 _003CCellsAdjacent8WayAndInside_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCellsAdjacent8WayAndInside_003Ed__ = this;
			}
			else
			{
				_003CCellsAdjacent8WayAndInside_003Ed__ = new _003CCellsAdjacent8WayAndInside_003Ed__22(0);
			}
			_003CCellsAdjacent8WayAndInside_003Ed__.thing = _003C_003E3__thing;
			return _003CCellsAdjacent8WayAndInside_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCellsAdjacentAlongEdge_003Ed__21 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private IntVec2 size;

		public IntVec2 _003C_003E3__size;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private int dir;

		public int _003C_003E3__dir;

		private int _003CminX_003E5__2;

		private int _003CmaxX_003E5__3;

		private int _003CminZ_003E5__4;

		private int _003CmaxZ_003E5__5;

		private int _003Cx_003E5__6;

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
		public _003CCellsAdjacentAlongEdge_003Ed__21(int _003C_003E1__state)
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
				AdjustForRotation(ref center, ref size, rot);
				_003CminX_003E5__2 = center.x - (size.x - 1) / 2;
				_003CmaxX_003E5__3 = _003CminX_003E5__2 + size.x;
				_003CminZ_003E5__4 = center.z - (size.z - 1) / 2;
				_003CmaxZ_003E5__5 = _003CminZ_003E5__4 + size.z;
				if (dir == 0)
				{
					_003Cx_003E5__6 = _003CminX_003E5__2;
					goto IL_010d;
				}
				goto IL_011b;
			case 1:
				_003C_003E1__state = -1;
				_003Cx_003E5__6++;
				goto IL_010d;
			case 2:
				_003C_003E1__state = -1;
				_003Cx_003E5__6++;
				goto IL_0176;
			case 3:
				_003C_003E1__state = -1;
				_003Cx_003E5__6++;
				goto IL_01df;
			case 4:
				{
					_003C_003E1__state = -1;
					_003Cx_003E5__6++;
					goto IL_0248;
				}
				IL_010d:
				if (_003Cx_003E5__6 < _003CmaxX_003E5__3)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__6, center.y, _003CmaxZ_003E5__5 + 1);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_011b;
				IL_01df:
				if (_003Cx_003E5__6 < _003CmaxX_003E5__3)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__6, center.y, _003CminZ_003E5__4 - 1);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_01ed;
				IL_011b:
				if (dir == 1)
				{
					_003Cx_003E5__6 = _003CminZ_003E5__4;
					goto IL_0176;
				}
				goto IL_0184;
				IL_0248:
				if (_003Cx_003E5__6 < _003CmaxZ_003E5__5)
				{
					_003C_003E2__current = new IntVec3(_003CminX_003E5__2 - 1, center.y, _003Cx_003E5__6);
					_003C_003E1__state = 4;
					return true;
				}
				break;
				IL_0176:
				if (_003Cx_003E5__6 < _003CmaxZ_003E5__5)
				{
					_003C_003E2__current = new IntVec3(_003CmaxX_003E5__3 + 1, center.y, _003Cx_003E5__6);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0184;
				IL_01ed:
				if (dir != 3)
				{
					break;
				}
				_003Cx_003E5__6 = _003CminZ_003E5__4;
				goto IL_0248;
				IL_0184:
				if (dir == 2)
				{
					_003Cx_003E5__6 = _003CminX_003E5__2;
					goto IL_01df;
				}
				goto IL_01ed;
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
			_003CCellsAdjacentAlongEdge_003Ed__21 _003CCellsAdjacentAlongEdge_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCellsAdjacentAlongEdge_003Ed__ = this;
			}
			else
			{
				_003CCellsAdjacentAlongEdge_003Ed__ = new _003CCellsAdjacentAlongEdge_003Ed__21(0);
			}
			_003CCellsAdjacentAlongEdge_003Ed__.center = _003C_003E3__center;
			_003CCellsAdjacentAlongEdge_003Ed__.rot = _003C_003E3__rot;
			_003CCellsAdjacentAlongEdge_003Ed__.size = _003C_003E3__size;
			_003CCellsAdjacentAlongEdge_003Ed__.dir = _003C_003E3__dir;
			return _003CCellsAdjacentAlongEdge_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCellsAdjacentCardinal_003Ed__20 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private IntVec2 size;

		public IntVec2 _003C_003E3__size;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private int _003CminX_003E5__2;

		private int _003CmaxX_003E5__3;

		private int _003CminZ_003E5__4;

		private int _003CmaxZ_003E5__5;

		private IntVec3 _003Ccur_003E5__6;

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
		public _003CCellsAdjacentCardinal_003Ed__20(int _003C_003E1__state)
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
				AdjustForRotation(ref center, ref size, rot);
				_003CminX_003E5__2 = center.x - (size.x - 1) / 2 - 1;
				_003CmaxX_003E5__3 = _003CminX_003E5__2 + size.x + 1;
				_003CminZ_003E5__4 = center.z - (size.z - 1) / 2 - 1;
				_003CmaxZ_003E5__5 = _003CminZ_003E5__4 + size.z + 1;
				_003Ccur_003E5__6 = new IntVec3(_003CminX_003E5__2, 0, _003CminZ_003E5__4);
				goto IL_00d3;
			case 1:
				_003C_003E1__state = -1;
				if (_003Ccur_003E5__6.x < _003CmaxX_003E5__3 - 1)
				{
					goto IL_00d3;
				}
				_003Ccur_003E5__6.x++;
				goto IL_0124;
			case 2:
				_003C_003E1__state = -1;
				if (_003Ccur_003E5__6.z < _003CmaxZ_003E5__5 - 1)
				{
					goto IL_0124;
				}
				_003Ccur_003E5__6.z++;
				goto IL_0175;
			case 3:
				_003C_003E1__state = -1;
				if (_003Ccur_003E5__6.x > _003CminX_003E5__2 + 1)
				{
					goto IL_0175;
				}
				_003Ccur_003E5__6.x--;
				goto IL_01c6;
			case 4:
				{
					_003C_003E1__state = -1;
					if (_003Ccur_003E5__6.z <= _003CminZ_003E5__4 + 1)
					{
						return false;
					}
					goto IL_01c6;
				}
				IL_0124:
				_003Ccur_003E5__6.z++;
				_003C_003E2__current = _003Ccur_003E5__6;
				_003C_003E1__state = 2;
				return true;
				IL_01c6:
				_003Ccur_003E5__6.z--;
				_003C_003E2__current = _003Ccur_003E5__6;
				_003C_003E1__state = 4;
				return true;
				IL_00d3:
				_003Ccur_003E5__6.x++;
				_003C_003E2__current = _003Ccur_003E5__6;
				_003C_003E1__state = 1;
				return true;
				IL_0175:
				_003Ccur_003E5__6.x--;
				_003C_003E2__current = _003Ccur_003E5__6;
				_003C_003E1__state = 3;
				return true;
			}
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
			_003CCellsAdjacentCardinal_003Ed__20 _003CCellsAdjacentCardinal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCellsAdjacentCardinal_003Ed__ = this;
			}
			else
			{
				_003CCellsAdjacentCardinal_003Ed__ = new _003CCellsAdjacentCardinal_003Ed__20(0);
			}
			_003CCellsAdjacentCardinal_003Ed__.center = _003C_003E3__center;
			_003CCellsAdjacentCardinal_003Ed__.rot = _003C_003E3__rot;
			_003CCellsAdjacentCardinal_003Ed__.size = _003C_003E3__size;
			return _003CCellsAdjacentCardinal_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCellsOccupiedBy_003Ed__14 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing t;

		public Thing _003C_003E3__t;

		private IEnumerator<IntVec3> _003C_003E7__wrap1;

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
		public _003CCellsOccupiedBy_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (t.def.size.x == 1 && t.def.size.z == 1)
					{
						_003C_003E2__current = t.Position;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E7__wrap1 = CellsOccupiedBy(t.Position, t.Rotation, t.def.size).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00e8;
				case 1:
					_003C_003E1__state = -1;
					break;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_00e8;
					}
					IL_00e8:
					if (_003C_003E7__wrap1.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					break;
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
			_003CCellsOccupiedBy_003Ed__14 _003CCellsOccupiedBy_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCellsOccupiedBy_003Ed__ = this;
			}
			else
			{
				_003CCellsOccupiedBy_003Ed__ = new _003CCellsOccupiedBy_003Ed__14(0);
			}
			_003CCellsOccupiedBy_003Ed__.t = _003C_003E3__t;
			return _003CCellsOccupiedBy_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCellsOccupiedBy_003Ed__15 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private IntVec2 size;

		public IntVec2 _003C_003E3__size;

		private Rot4 rotation;

		public Rot4 _003C_003E3__rotation;

		private int _003CminZ_003E5__2;

		private int _003CmaxX_003E5__3;

		private int _003CmaxZ_003E5__4;

		private int _003Ci_003E5__5;

		private int _003Cj_003E5__6;

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
		public _003CCellsOccupiedBy_003Ed__15(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Cj_003E5__6++;
				goto IL_00ec;
			}
			_003C_003E1__state = -1;
			AdjustForRotation(ref center, ref size, rotation);
			int num2 = center.x - (size.x - 1) / 2;
			_003CminZ_003E5__2 = center.z - (size.z - 1) / 2;
			_003CmaxX_003E5__3 = num2 + size.x - 1;
			_003CmaxZ_003E5__4 = _003CminZ_003E5__2 + size.z - 1;
			_003Ci_003E5__5 = num2;
			goto IL_010a;
			IL_00ec:
			if (_003Cj_003E5__6 <= _003CmaxZ_003E5__4)
			{
				_003C_003E2__current = new IntVec3(_003Ci_003E5__5, 0, _003Cj_003E5__6);
				_003C_003E1__state = 1;
				return true;
			}
			_003Ci_003E5__5++;
			goto IL_010a;
			IL_010a:
			if (_003Ci_003E5__5 <= _003CmaxX_003E5__3)
			{
				_003Cj_003E5__6 = _003CminZ_003E5__2;
				goto IL_00ec;
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
			_003CCellsOccupiedBy_003Ed__15 _003CCellsOccupiedBy_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCellsOccupiedBy_003Ed__ = this;
			}
			else
			{
				_003CCellsOccupiedBy_003Ed__ = new _003CCellsOccupiedBy_003Ed__15(0);
			}
			_003CCellsOccupiedBy_003Ed__.center = _003C_003E3__center;
			_003CCellsOccupiedBy_003Ed__.rotation = _003C_003E3__rotation;
			_003CCellsOccupiedBy_003Ed__.size = _003C_003E3__size;
			return _003CCellsOccupiedBy_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	public static IntVec3[] CardinalDirections;

	public static IntVec3[] CardinalDirectionsAndInside;

	public static IntVec3[] CardinalDirectionsAround;

	public static IntVec3[] DiagonalDirections;

	public static IntVec3[] DiagonalDirectionsAround;

	public static IntVec3[] AdjacentCells;

	public static IntVec3[] AdjacentCellsAndInside;

	public static IntVec3[] AdjacentCellsAround;

	public static IntVec3[] AdjacentCellsAroundBottom;

	public static IntVec3[] AdjacentCellsAndInsideForUV;

	private static List<IntVec3> adjRandomOrderList;

	private static List<IntVec3> validCells;

	static GenAdj()
	{
		CardinalDirections = new IntVec3[4];
		CardinalDirectionsAndInside = new IntVec3[5];
		CardinalDirectionsAround = new IntVec3[4];
		DiagonalDirections = new IntVec3[4];
		DiagonalDirectionsAround = new IntVec3[4];
		AdjacentCells = new IntVec3[8];
		AdjacentCellsAndInside = new IntVec3[9];
		AdjacentCellsAround = new IntVec3[8];
		AdjacentCellsAroundBottom = new IntVec3[9];
		AdjacentCellsAndInsideForUV = new IntVec3[9];
		validCells = new List<IntVec3>();
		SetupAdjacencyTables();
	}

	private static void SetupAdjacencyTables()
	{
		CardinalDirections[0] = new IntVec3(0, 0, 1);
		CardinalDirections[1] = new IntVec3(1, 0, 0);
		CardinalDirections[2] = new IntVec3(0, 0, -1);
		CardinalDirections[3] = new IntVec3(-1, 0, 0);
		CardinalDirectionsAndInside[0] = new IntVec3(0, 0, 1);
		CardinalDirectionsAndInside[1] = new IntVec3(1, 0, 0);
		CardinalDirectionsAndInside[2] = new IntVec3(0, 0, -1);
		CardinalDirectionsAndInside[3] = new IntVec3(-1, 0, 0);
		CardinalDirectionsAndInside[4] = new IntVec3(0, 0, 0);
		CardinalDirectionsAround[0] = new IntVec3(0, 0, -1);
		CardinalDirectionsAround[1] = new IntVec3(-1, 0, 0);
		CardinalDirectionsAround[2] = new IntVec3(0, 0, 1);
		CardinalDirectionsAround[3] = new IntVec3(1, 0, 0);
		DiagonalDirections[0] = new IntVec3(-1, 0, -1);
		DiagonalDirections[1] = new IntVec3(-1, 0, 1);
		DiagonalDirections[2] = new IntVec3(1, 0, 1);
		DiagonalDirections[3] = new IntVec3(1, 0, -1);
		DiagonalDirectionsAround[0] = new IntVec3(-1, 0, -1);
		DiagonalDirectionsAround[1] = new IntVec3(-1, 0, 1);
		DiagonalDirectionsAround[2] = new IntVec3(1, 0, 1);
		DiagonalDirectionsAround[3] = new IntVec3(1, 0, -1);
		AdjacentCells[0] = new IntVec3(0, 0, 1);
		AdjacentCells[1] = new IntVec3(1, 0, 0);
		AdjacentCells[2] = new IntVec3(0, 0, -1);
		AdjacentCells[3] = new IntVec3(-1, 0, 0);
		AdjacentCells[4] = new IntVec3(1, 0, -1);
		AdjacentCells[5] = new IntVec3(1, 0, 1);
		AdjacentCells[6] = new IntVec3(-1, 0, 1);
		AdjacentCells[7] = new IntVec3(-1, 0, -1);
		AdjacentCellsAndInside[0] = new IntVec3(0, 0, 1);
		AdjacentCellsAndInside[1] = new IntVec3(1, 0, 0);
		AdjacentCellsAndInside[2] = new IntVec3(0, 0, -1);
		AdjacentCellsAndInside[3] = new IntVec3(-1, 0, 0);
		AdjacentCellsAndInside[4] = new IntVec3(1, 0, -1);
		AdjacentCellsAndInside[5] = new IntVec3(1, 0, 1);
		AdjacentCellsAndInside[6] = new IntVec3(-1, 0, 1);
		AdjacentCellsAndInside[7] = new IntVec3(-1, 0, -1);
		AdjacentCellsAndInside[8] = new IntVec3(0, 0, 0);
		AdjacentCellsAround[0] = new IntVec3(0, 0, 1);
		AdjacentCellsAround[1] = new IntVec3(1, 0, 1);
		AdjacentCellsAround[2] = new IntVec3(1, 0, 0);
		AdjacentCellsAround[3] = new IntVec3(1, 0, -1);
		AdjacentCellsAround[4] = new IntVec3(0, 0, -1);
		AdjacentCellsAround[5] = new IntVec3(-1, 0, -1);
		AdjacentCellsAround[6] = new IntVec3(-1, 0, 0);
		AdjacentCellsAround[7] = new IntVec3(-1, 0, 1);
		AdjacentCellsAroundBottom[0] = new IntVec3(0, 0, -1);
		AdjacentCellsAroundBottom[1] = new IntVec3(-1, 0, -1);
		AdjacentCellsAroundBottom[2] = new IntVec3(-1, 0, 0);
		AdjacentCellsAroundBottom[3] = new IntVec3(-1, 0, 1);
		AdjacentCellsAroundBottom[4] = new IntVec3(0, 0, 1);
		AdjacentCellsAroundBottom[5] = new IntVec3(1, 0, 1);
		AdjacentCellsAroundBottom[6] = new IntVec3(1, 0, 0);
		AdjacentCellsAroundBottom[7] = new IntVec3(1, 0, -1);
		AdjacentCellsAroundBottom[8] = new IntVec3(0, 0, 0);
		AdjacentCellsAndInsideForUV[0] = new IntVec3(0, 0, -1);
		AdjacentCellsAndInsideForUV[1] = new IntVec3(-1, 0, -1);
		AdjacentCellsAndInsideForUV[2] = new IntVec3(-1, 0, 0);
		AdjacentCellsAndInsideForUV[3] = new IntVec3(-1, 0, 1);
		AdjacentCellsAndInsideForUV[4] = new IntVec3(0, 0, 1);
		AdjacentCellsAndInsideForUV[5] = new IntVec3(1, 0, 1);
		AdjacentCellsAndInsideForUV[6] = new IntVec3(1, 0, 0);
		AdjacentCellsAndInsideForUV[7] = new IntVec3(1, 0, -1);
		AdjacentCellsAndInsideForUV[8] = new IntVec3(0, 0, 0);
	}

	public static List<IntVec3> AdjacentCells8WayRandomized()
	{
		if (adjRandomOrderList == null)
		{
			adjRandomOrderList = new List<IntVec3>();
			for (int i = 0; i < 8; i++)
			{
				adjRandomOrderList.Add(AdjacentCells[i]);
			}
		}
		adjRandomOrderList.Shuffle();
		return adjRandomOrderList;
	}

	[IteratorStateMachine(typeof(_003CCellsOccupiedBy_003Ed__14))]
	public static IEnumerable<IntVec3> CellsOccupiedBy(Thing t)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCellsOccupiedBy_003Ed__14(-2)
		{
			_003C_003E3__t = t
		};
	}

	[IteratorStateMachine(typeof(_003CCellsOccupiedBy_003Ed__15))]
	public static IEnumerable<IntVec3> CellsOccupiedBy(IntVec3 center, Rot4 rotation, IntVec2 size)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCellsOccupiedBy_003Ed__15(-2)
		{
			_003C_003E3__center = center,
			_003C_003E3__rotation = rotation,
			_003C_003E3__size = size
		};
	}

	[IteratorStateMachine(typeof(_003CCellsAdjacent8Way_003Ed__16))]
	public static IEnumerable<IntVec3> CellsAdjacent8Way(TargetInfo pack)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCellsAdjacent8Way_003Ed__16(-2)
		{
			_003C_003E3__pack = pack
		};
	}

	public static IEnumerable<IntVec3> CellsAdjacent8Way(Thing t)
	{
		return CellsAdjacent8Way(t.Position, t.Rotation, t.def.size);
	}

	[IteratorStateMachine(typeof(_003CCellsAdjacent8Way_003Ed__18))]
	public static IEnumerable<IntVec3> CellsAdjacent8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCellsAdjacent8Way_003Ed__18(-2)
		{
			_003C_003E3__thingCenter = thingCenter,
			_003C_003E3__thingRot = thingRot,
			_003C_003E3__thingSize = thingSize
		};
	}

	public static IEnumerable<IntVec3> CellsAdjacentCardinal(Thing t)
	{
		return CellsAdjacentCardinal(t.Position, t.Rotation, t.def.size);
	}

	[IteratorStateMachine(typeof(_003CCellsAdjacentCardinal_003Ed__20))]
	public static IEnumerable<IntVec3> CellsAdjacentCardinal(IntVec3 center, Rot4 rot, IntVec2 size)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCellsAdjacentCardinal_003Ed__20(-2)
		{
			_003C_003E3__center = center,
			_003C_003E3__rot = rot,
			_003C_003E3__size = size
		};
	}

	[IteratorStateMachine(typeof(_003CCellsAdjacentAlongEdge_003Ed__21))]
	public static IEnumerable<IntVec3> CellsAdjacentAlongEdge(IntVec3 center, Rot4 rot, IntVec2 size, int dir)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCellsAdjacentAlongEdge_003Ed__21(-2)
		{
			_003C_003E3__center = center,
			_003C_003E3__rot = rot,
			_003C_003E3__size = size,
			_003C_003E3__dir = dir
		};
	}

	[IteratorStateMachine(typeof(_003CCellsAdjacent8WayAndInside_003Ed__22))]
	public static IEnumerable<IntVec3> CellsAdjacent8WayAndInside(this Thing thing)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCellsAdjacent8WayAndInside_003Ed__22(-2)
		{
			_003C_003E3__thing = thing
		};
	}

	public static void GetAdjacentCorners(LocalTargetInfo target, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
	{
		if (target.HasThing)
		{
			GetAdjacentCorners(target.Thing.OccupiedRect(), out BL, out TL, out TR, out BR);
		}
		else
		{
			GetAdjacentCorners(CellRect.SingleCell(target.Cell), out BL, out TL, out TR, out BR);
		}
	}

	private static void GetAdjacentCorners(CellRect rect, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
	{
		BL = new IntVec3(rect.minX - 1, 0, rect.minZ - 1);
		TL = new IntVec3(rect.minX - 1, 0, rect.maxZ + 1);
		TR = new IntVec3(rect.maxX + 1, 0, rect.maxZ + 1);
		BR = new IntVec3(rect.maxX + 1, 0, rect.minZ - 1);
	}

	public static IntVec3 RandomAdjacentCell8Way(this IntVec3 root)
	{
		return root + AdjacentCells[Rand.RangeInclusive(0, 7)];
	}

	public static IntVec3 RandomAdjacentCellCardinal(this IntVec3 root)
	{
		return root + CardinalDirections[Rand.RangeInclusive(0, 3)];
	}

	public static IntVec3 RandomAdjacentCell8Way(this Thing t)
	{
		CellRect cellRect = t.OccupiedRect();
		CellRect cellRect2 = cellRect.ExpandedBy(1);
		IntVec3 randomCell;
		do
		{
			randomCell = cellRect2.RandomCell;
		}
		while (cellRect.Contains(randomCell));
		return randomCell;
	}

	public static IntVec3 RandomAdjacentCellCardinal(this Thing t)
	{
		CellRect cellRect = t.OccupiedRect();
		IntVec3 randomCell = cellRect.RandomCell;
		if (Rand.Value < 0.5f)
		{
			if (Rand.Value < 0.5f)
			{
				randomCell.x = cellRect.minX - 1;
			}
			else
			{
				randomCell.x = cellRect.maxX + 1;
			}
		}
		else if (Rand.Value < 0.5f)
		{
			randomCell.z = cellRect.minZ - 1;
		}
		else
		{
			randomCell.z = cellRect.maxZ + 1;
		}
		return randomCell;
	}

	public static bool TryFindRandomAdjacentCell8WayWithRoom(Thing t, out IntVec3 result)
	{
		return TryFindRandomAdjacentCell8WayWithRoom(t.Position, t.Rotation, t.def.size, t.MapHeld, out result);
	}

	public static bool TryFindRandomAdjacentCell8WayWithRoom(IntVec3 center, Rot4 rot, IntVec2 size, Map map, out IntVec3 result)
	{
		AdjustForRotation(ref center, ref size, rot);
		validCells.Clear();
		foreach (IntVec3 item in CellsAdjacent8Way(center, rot, size))
		{
			if (item.InBounds(map) && item.GetRoom(map) != null)
			{
				validCells.Add(item);
			}
		}
		return validCells.TryRandomElement(out result);
	}

	public static bool AdjacentTo8WayOrInside(this IntVec3 me, LocalTargetInfo other)
	{
		if (other.HasThing)
		{
			return me.AdjacentTo8WayOrInside(other.Thing);
		}
		return me.AdjacentTo8WayOrInside(other.Cell);
	}

	public static bool AdjacentTo8Way(this IntVec3 me, IntVec3 other)
	{
		int num = me.x - other.x;
		int num2 = me.z - other.z;
		if (num == 0 && num2 == 0)
		{
			return false;
		}
		if (num < 0)
		{
			num *= -1;
		}
		if (num2 < 0)
		{
			num2 *= -1;
		}
		if (num <= 1)
		{
			return num2 <= 1;
		}
		return false;
	}

	public static bool AdjacentTo8WayOrInside(this IntVec3 me, IntVec3 other)
	{
		int num = me.x - other.x;
		int num2 = me.z - other.z;
		if (num < 0)
		{
			num *= -1;
		}
		if (num2 < 0)
		{
			num2 *= -1;
		}
		if (num <= 1)
		{
			return num2 <= 1;
		}
		return false;
	}

	public static bool IsAdjacentToCardinalOrInside(this IntVec3 me, CellRect other)
	{
		if (other.IsEmpty)
		{
			return false;
		}
		CellRect cellRect = other.ExpandedBy(1);
		if (cellRect.Contains(me))
		{
			return !cellRect.IsCorner(me);
		}
		return false;
	}

	public static bool IsAdjacentToCardinalOrInside(this Thing t1, Thing t2)
	{
		return IsAdjacentToCardinalOrInside(t1.OccupiedRect(), t2.OccupiedRect());
	}

	public static bool IsAdjacentToCardinalOrInside(CellRect rect1, CellRect rect2)
	{
		if (rect1.IsEmpty || rect2.IsEmpty)
		{
			return false;
		}
		CellRect cellRect = rect1.ExpandedBy(1);
		int minX = cellRect.minX;
		int maxX = cellRect.maxX;
		int minZ = cellRect.minZ;
		int maxZ = cellRect.maxZ;
		int i = minX;
		int num = minZ;
		for (; i <= maxX; i++)
		{
			if (rect2.Contains(new IntVec3(i, 0, num)) && (i != minX || num != minZ) && (i != minX || num != maxZ) && (i != maxX || num != minZ) && (i != maxX || num != maxZ))
			{
				return true;
			}
		}
		i--;
		for (num++; num <= maxZ; num++)
		{
			if (rect2.Contains(new IntVec3(i, 0, num)) && (i != minX || num != minZ) && (i != minX || num != maxZ) && (i != maxX || num != minZ) && (i != maxX || num != maxZ))
			{
				return true;
			}
		}
		num--;
		for (i--; i >= minX; i--)
		{
			if (rect2.Contains(new IntVec3(i, 0, num)) && (i != minX || num != minZ) && (i != minX || num != maxZ) && (i != maxX || num != minZ) && (i != maxX || num != maxZ))
			{
				return true;
			}
		}
		i++;
		for (num--; num > minZ; num--)
		{
			if (rect2.Contains(new IntVec3(i, 0, num)) && (i != minX || num != minZ) && (i != minX || num != maxZ) && (i != maxX || num != minZ) && (i != maxX || num != maxZ))
			{
				return true;
			}
		}
		return false;
	}

	public static bool AdjacentTo8WayOrInside(this IntVec3 root, Thing t)
	{
		return root.AdjacentTo8WayOrInside(t.Position, t.Rotation, t.def.size);
	}

	public static bool AdjacentTo8WayOrInside(this IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
	{
		AdjustForRotation(ref center, ref size, rot);
		int num = center.x - (size.x - 1) / 2 - 1;
		int num2 = center.z - (size.z - 1) / 2 - 1;
		int num3 = num + size.x + 1;
		int num4 = num2 + size.z + 1;
		if (root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4)
		{
			return true;
		}
		return false;
	}

	public static bool AdjacentTo8WayOrInside(this Thing a, Thing b)
	{
		return AdjacentTo8WayOrInside(a.OccupiedRect(), b.OccupiedRect());
	}

	public static bool AdjacentTo8WayOrInside(CellRect rect1, CellRect rect2)
	{
		if (rect1.IsEmpty || rect2.IsEmpty)
		{
			return false;
		}
		return rect1.ExpandedBy(1).Overlaps(rect2);
	}

	public static bool IsInside(this IntVec3 root, Thing t)
	{
		return IsInside(root, t.Position, t.Rotation, t.def.size);
	}

	public static bool IsInside(IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
	{
		AdjustForRotation(ref center, ref size, rot);
		int num = center.x - (size.x - 1) / 2;
		int num2 = center.z - (size.z - 1) / 2;
		int num3 = num + size.x - 1;
		int num4 = num2 + size.z - 1;
		if (root.x >= num && root.x <= num3 && root.z >= num2)
		{
			return root.z <= num4;
		}
		return false;
	}

	public static CellRect OccupiedRect(this Thing t)
	{
		return OccupiedRect(t.Position, t.Rotation, t.def.size);
	}

	public static CellRect OccupiedDrawRect(this Thing t)
	{
		Vector2 drawSize = t.DrawSize;
		return OccupiedRect(t.Position, t.Rotation, new IntVec2(Mathf.CeilToInt(drawSize.x), Mathf.CeilToInt(drawSize.y)));
	}

	public static Bounds DrawBounds(this Thing t)
	{
		Vector3 center = t.TrueCenter();
		Vector3 size = t.Rotation.AsQuat * t.def.size.ToVector3();
		return new Bounds(center, size);
	}

	public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, int width, int height)
	{
		return OccupiedRect(center, rot, new IntVec2(width, height));
	}

	public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, IntVec2 size)
	{
		AdjustForRotation(ref center, ref size, rot);
		return new CellRect(center.x - (size.x - 1) / 2, center.z - (size.z - 1) / 2, size.x, size.z);
	}

	public static CellRect RectAbout(this IntVec3 center, int width, int height)
	{
		return OccupiedRect(center, Rot4.North, new IntVec2(width, height));
	}

	public static CellRect RectAbout(this IntVec3 center, int width, int height, Rot4 rot)
	{
		return OccupiedRect(center, rot, new IntVec2(width, height));
	}

	public static CellRect RectAbout(this IntVec3 center, IntVec2 size)
	{
		return OccupiedRect(center, Rot4.North, size);
	}

	public static CellRect RectAbout(this IntVec3 center, IntVec2 size, Rot4 rot)
	{
		return OccupiedRect(center, rot, size);
	}

	public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 rot)
	{
		AdjustForRotation(ref center, ref size, Rot4.North, rot);
	}

	public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 reference, Rot4 rot)
	{
		if (size.x == 1 && size.z == 1)
		{
			return;
		}
		int x2;
		int z2;
		if (rot.IsHorizontal)
		{
			ref int x = ref size.x;
			ref int z = ref size.z;
			z2 = size.z;
			x2 = size.x;
			x = z2;
			z = x2;
		}
		x2 = reference.AsInt;
		z2 = rot.AsInt;
		(int, int) tuple;
		switch (x2)
		{
		case 0:
			switch (z2)
			{
			case 1:
				break;
			case 2:
				goto IL_00dd;
			case 3:
				goto IL_00e8;
			default:
				goto IL_0156;
			}
			tuple = (0, -1);
			break;
		case 1:
			switch (z2)
			{
			case 1:
				break;
			case 2:
				goto IL_00fe;
			case 3:
				goto IL_0109;
			default:
				goto IL_0156;
			}
			tuple = (-1, 0);
			break;
		case 2:
			switch (z2)
			{
			case 1:
				break;
			case 2:
				goto IL_011f;
			case 3:
				goto IL_012a;
			default:
				goto IL_0156;
			}
			tuple = (0, 1);
			break;
		case 3:
			switch (z2)
			{
			case 1:
				break;
			case 2:
				goto IL_0140;
			case 3:
				goto IL_014b;
			default:
				goto IL_0156;
			}
			tuple = (1, 0);
			break;
		default:
			goto IL_0156;
			IL_014b:
			tuple = (0, -1);
			break;
			IL_0140:
			tuple = (1, -1);
			break;
			IL_0156:
			tuple = (0, 0);
			break;
			IL_012a:
			tuple = (1, 0);
			break;
			IL_011f:
			tuple = (1, 1);
			break;
			IL_00e8:
			tuple = (-1, 0);
			break;
			IL_0109:
			tuple = (0, 1);
			break;
			IL_00fe:
			tuple = (-1, 1);
			break;
			IL_00dd:
			tuple = (-1, -1);
			break;
		}
		var (num, num2) = tuple;
		if (size.x % 2 == 0)
		{
			center.x += num;
		}
		if (size.z % 2 == 0)
		{
			center.z += num2;
		}
	}
}
