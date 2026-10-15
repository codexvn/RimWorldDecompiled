using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;

namespace Verse;

public struct CellRect : IEquatable<CellRect>, IEnumerable<IntVec3>, IEnumerable
{
	public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private CellRect ir;

		private int x;

		private int z;

		public IntVec3 Current => new IntVec3(x, 0, z);

		object IEnumerator.Current => new IntVec3(x, 0, z);

		public Enumerator(CellRect ir)
		{
			this.ir = ir;
			x = ir.minX - 1;
			z = ir.minZ;
		}

		public bool MoveNext()
		{
			x++;
			if (x > ir.maxX)
			{
				x = ir.minX;
				z++;
			}
			if (z > ir.maxZ)
			{
				return false;
			}
			return true;
		}

		public void Reset()
		{
			x = ir.minX - 1;
			z = ir.minZ;
		}

		void IDisposable.Dispose()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CDifferenceCells_003Ed__128 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private CellRect container;

		public CellRect _003C_003E3__container;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private int _003Cz_003E5__2;

		private int _003Cx_003E5__3;

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
		public _003CDifferenceCells_003Ed__128(int _003C_003E1__state)
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
				goto IL_0076;
			}
			_003C_003E1__state = -1;
			_003Cz_003E5__2 = container.minZ;
			goto IL_00a9;
			IL_0086:
			if (_003Cx_003E5__3 <= container.maxX)
			{
				IntVec3 c = new IntVec3(_003Cx_003E5__3, 0, _003Cz_003E5__2);
				if (!_003C_003E4__this.Contains(c))
				{
					_003C_003E2__current = c;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0076;
			}
			_003Cz_003E5__2++;
			goto IL_00a9;
			IL_0076:
			_003Cx_003E5__3++;
			goto IL_0086;
			IL_00a9:
			if (_003Cz_003E5__2 <= container.maxZ)
			{
				_003Cx_003E5__3 = container.minX;
				goto IL_0086;
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
			_003CDifferenceCells_003Ed__128 _003CDifferenceCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CDifferenceCells_003Ed__ = this;
			}
			else
			{
				_003CDifferenceCells_003Ed__ = new _003CDifferenceCells_003Ed__128(0);
			}
			_003CDifferenceCells_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			_003CDifferenceCells_003Ed__.container = _003C_003E3__container;
			return _003CDifferenceCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateRectanglesCovering_003Ed__143 : IEnumerable<CellRect>, IEnumerable, IEnumerator<CellRect>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private CellRect _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private Predicate<IntVec3> validator;

		public Predicate<IntVec3> _003C_003E3__validator;

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
		public _003CEnumerateRectanglesCovering_003Ed__143(int _003C_003E1__state)
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
				tmpUnvistedCells.Clear();
				foreach (IntVec3 cell in _003C_003E4__this.Cells)
				{
					if (validator(cell))
					{
						tmpUnvistedCells.Add(cell);
					}
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (tmpUnvistedCells.Count > 0)
			{
				IntVec3 item = tmpUnvistedCells.First();
				tmpUnvistedCells.Remove(item);
				int i = item.x;
				int num;
				for (num = item.z; tmpUnvistedCells.Contains(new IntVec3(i + 1, 0, num)); i++)
				{
				}
				while (true)
				{
					bool flag = false;
					for (int j = item.x; j <= i; j++)
					{
						if (!tmpUnvistedCells.Contains(new IntVec3(j, 0, num + 1)))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
					num++;
				}
				CellRect cellRect = new CellRect(item.x, item.z, i - item.x + 1, num - item.z + 1);
				foreach (IntVec3 cell2 in cellRect.Cells)
				{
					tmpUnvistedCells.Remove(cell2);
				}
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
			_003CEnumerateRectanglesCovering_003Ed__143 _003CEnumerateRectanglesCovering_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateRectanglesCovering_003Ed__ = this;
			}
			else
			{
				_003CEnumerateRectanglesCovering_003Ed__ = new _003CEnumerateRectanglesCovering_003Ed__143(0);
			}
			_003CEnumerateRectanglesCovering_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			_003CEnumerateRectanglesCovering_003Ed__.validator = _003C_003E3__validator;
			return _003CEnumerateRectanglesCovering_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CellRect>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetCellsOnEdge_003Ed__82 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private int _003Cx_003E5__2;

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
		public _003CGetCellsOnEdge_003Ed__82(int _003C_003E1__state)
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
				if (_003C_003E4__this.IsEmpty)
				{
					return false;
				}
				if (rot == Rot4.North)
				{
					_003Cx_003E5__2 = _003C_003E4__this.minX;
					goto IL_009b;
				}
				if (rot == Rot4.East)
				{
					_003Cx_003E5__2 = _003C_003E4__this.minZ;
					goto IL_0115;
				}
				if (rot == Rot4.South)
				{
					_003Cx_003E5__2 = _003C_003E4__this.minX;
					goto IL_018f;
				}
				if (!(rot == Rot4.West))
				{
					break;
				}
				_003Cx_003E5__2 = _003C_003E4__this.minZ;
				goto IL_0206;
			case 1:
				_003C_003E1__state = -1;
				_003Cx_003E5__2++;
				goto IL_009b;
			case 2:
				_003C_003E1__state = -1;
				_003Cx_003E5__2++;
				goto IL_0115;
			case 3:
				_003C_003E1__state = -1;
				_003Cx_003E5__2++;
				goto IL_018f;
			case 4:
				{
					_003C_003E1__state = -1;
					_003Cx_003E5__2++;
					goto IL_0206;
				}
				IL_0206:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxZ)
				{
					_003C_003E2__current = new IntVec3(_003C_003E4__this.minX, 0, _003Cx_003E5__2);
					_003C_003E1__state = 4;
					return true;
				}
				break;
				IL_018f:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003C_003E4__this.minZ);
					_003C_003E1__state = 3;
					return true;
				}
				break;
				IL_0115:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxZ)
				{
					_003C_003E2__current = new IntVec3(_003C_003E4__this.maxX, 0, _003Cx_003E5__2);
					_003C_003E1__state = 2;
					return true;
				}
				break;
				IL_009b:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003C_003E4__this.maxZ);
					_003C_003E1__state = 1;
					return true;
				}
				break;
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
			_003CGetCellsOnEdge_003Ed__82 _003CGetCellsOnEdge_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCellsOnEdge_003Ed__ = this;
			}
			else
			{
				_003CGetCellsOnEdge_003Ed__ = new _003CGetCellsOnEdge_003Ed__82(0);
			}
			_003CGetCellsOnEdge_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			_003CGetCellsOnEdge_003Ed__.rot = _003C_003E3__rot;
			return _003CGetCellsOnEdge_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetCenterCellsOnEdge_003Ed__79 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private int range;

		public int _003C_003E3__range;

		private int _003Ci_003E5__2;

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
		public _003CGetCenterCellsOnEdge_003Ed__79(int _003C_003E1__state)
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
				goto IL_006d;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_007d;
			IL_006d:
			_003Ci_003E5__2++;
			goto IL_007d;
			IL_007d:
			if (_003Ci_003E5__2 < range * 2 + 1)
			{
				IntVec3 centerCellOnEdge = _003C_003E4__this.GetCenterCellOnEdge(rot, _003Ci_003E5__2 - range);
				if (centerCellOnEdge.IsValid && _003C_003E4__this.Contains(centerCellOnEdge))
				{
					_003C_003E2__current = centerCellOnEdge;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_006d;
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
			_003CGetCenterCellsOnEdge_003Ed__79 _003CGetCenterCellsOnEdge_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCenterCellsOnEdge_003Ed__ = this;
			}
			else
			{
				_003CGetCenterCellsOnEdge_003Ed__ = new _003CGetCenterCellsOnEdge_003Ed__79(0);
			}
			_003CGetCenterCellsOnEdge_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			_003CGetCenterCellsOnEdge_003Ed__.rot = _003C_003E3__rot;
			_003CGetCenterCellsOnEdge_003Ed__.range = _003C_003E3__range;
			return _003CGetCenterCellsOnEdge_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetEdgeCells_003Ed__102 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rot4 dir;

		public Rot4 _003C_003E3__dir;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private int _003Cx_003E5__2;

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
		public _003CGetEdgeCells_003Ed__102(int _003C_003E1__state)
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
				if (dir == Rot4.North)
				{
					_003Cx_003E5__2 = _003C_003E4__this.minX;
					goto IL_008c;
				}
				if (dir == Rot4.South)
				{
					_003Cx_003E5__2 = _003C_003E4__this.minX;
					goto IL_0106;
				}
				if (dir == Rot4.West)
				{
					_003Cx_003E5__2 = _003C_003E4__this.minZ;
					goto IL_0180;
				}
				if (!(dir == Rot4.East))
				{
					break;
				}
				_003Cx_003E5__2 = _003C_003E4__this.minZ;
				goto IL_01f7;
			case 1:
				_003C_003E1__state = -1;
				_003Cx_003E5__2++;
				goto IL_008c;
			case 2:
				_003C_003E1__state = -1;
				_003Cx_003E5__2++;
				goto IL_0106;
			case 3:
				_003C_003E1__state = -1;
				_003Cx_003E5__2++;
				goto IL_0180;
			case 4:
				{
					_003C_003E1__state = -1;
					_003Cx_003E5__2++;
					goto IL_01f7;
				}
				IL_0106:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003C_003E4__this.minZ);
					_003C_003E1__state = 2;
					return true;
				}
				break;
				IL_008c:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003C_003E4__this.maxZ);
					_003C_003E1__state = 1;
					return true;
				}
				break;
				IL_01f7:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxZ)
				{
					_003C_003E2__current = new IntVec3(_003C_003E4__this.maxX, 0, _003Cx_003E5__2);
					_003C_003E1__state = 4;
					return true;
				}
				break;
				IL_0180:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxZ)
				{
					_003C_003E2__current = new IntVec3(_003C_003E4__this.minX, 0, _003Cx_003E5__2);
					_003C_003E1__state = 3;
					return true;
				}
				break;
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
			_003CGetEdgeCells_003Ed__102 _003CGetEdgeCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetEdgeCells_003Ed__ = this;
			}
			else
			{
				_003CGetEdgeCells_003Ed__ = new _003CGetEdgeCells_003Ed__102(0);
			}
			_003CGetEdgeCells_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			_003CGetEdgeCells_003Ed__.dir = _003C_003E3__dir;
			return _003CGetEdgeCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CUnionCells_003Ed__127 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private CellRect container;

		public CellRect _003C_003E3__container;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private int _003Cz_003E5__2;

		private int _003Cx_003E5__3;

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
		public _003CUnionCells_003Ed__127(int _003C_003E1__state)
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
				goto IL_0076;
			}
			_003C_003E1__state = -1;
			_003Cz_003E5__2 = container.minZ;
			goto IL_00a9;
			IL_0086:
			if (_003Cx_003E5__3 <= container.maxX)
			{
				IntVec3 c = new IntVec3(_003Cx_003E5__3, 0, _003Cz_003E5__2);
				if (_003C_003E4__this.Contains(c))
				{
					_003C_003E2__current = c;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0076;
			}
			_003Cz_003E5__2++;
			goto IL_00a9;
			IL_0076:
			_003Cx_003E5__3++;
			goto IL_0086;
			IL_00a9:
			if (_003Cz_003E5__2 <= container.maxZ)
			{
				_003Cx_003E5__3 = container.minX;
				goto IL_0086;
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
			_003CUnionCells_003Ed__127 _003CUnionCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CUnionCells_003Ed__ = this;
			}
			else
			{
				_003CUnionCells_003Ed__ = new _003CUnionCells_003Ed__127(0);
			}
			_003CUnionCells_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			_003CUnionCells_003Ed__.container = _003C_003E3__container;
			return _003CUnionCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AdjacentCells_003Ed__48 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

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
		public _003Cget_AdjacentCells_003Ed__48(int _003C_003E1__state)
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
					if (_003C_003E4__this.IsEmpty)
					{
						return false;
					}
					_003C_003E7__wrap1 = _003C_003E4__this.AdjacentCellsCardinal.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0090;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0090;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new IntVec3(_003C_003E4__this.maxX + 1, 0, _003C_003E4__this.minZ - 1);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new IntVec3(_003C_003E4__this.minX - 1, 0, _003C_003E4__this.maxZ + 1);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new IntVec3(_003C_003E4__this.maxX + 1, 0, _003C_003E4__this.maxZ + 1);
					_003C_003E1__state = 5;
					return true;
				case 5:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0090:
					if (_003C_003E7__wrap1.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new IntVec3(_003C_003E4__this.minX - 1, 0, _003C_003E4__this.minZ - 1);
					_003C_003E1__state = 2;
					return true;
				}
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
			_003Cget_AdjacentCells_003Ed__48 _003Cget_AdjacentCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_AdjacentCells_003Ed__ = this;
			}
			else
			{
				_003Cget_AdjacentCells_003Ed__ = new _003Cget_AdjacentCells_003Ed__48(0);
			}
			_003Cget_AdjacentCells_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_AdjacentCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AdjacentCellsCardinal_003Ed__46 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private int _003Cx_003E5__2;

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
		public _003Cget_AdjacentCellsCardinal_003Ed__46(int _003C_003E1__state)
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
				if (_003C_003E4__this.IsEmpty)
				{
					return false;
				}
				_003Cx_003E5__2 = _003C_003E4__this.minX;
				goto IL_00ba;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003C_003E4__this.maxZ + 1);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003Cx_003E5__2++;
				goto IL_00ba;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new IntVec3(_003C_003E4__this.maxX + 1, 0, _003Cx_003E5__2);
				_003C_003E1__state = 4;
				return true;
			case 4:
				{
					_003C_003E1__state = -1;
					_003Cx_003E5__2++;
					break;
				}
				IL_00ba:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003C_003E4__this.minZ - 1);
					_003C_003E1__state = 1;
					return true;
				}
				_003Cx_003E5__2 = _003C_003E4__this.minZ;
				break;
			}
			if (_003Cx_003E5__2 <= _003C_003E4__this.maxZ)
			{
				_003C_003E2__current = new IntVec3(_003C_003E4__this.minX - 1, 0, _003Cx_003E5__2);
				_003C_003E1__state = 3;
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
			_003Cget_AdjacentCellsCardinal_003Ed__46 _003Cget_AdjacentCellsCardinal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_AdjacentCellsCardinal_003Ed__ = this;
			}
			else
			{
				_003Cget_AdjacentCellsCardinal_003Ed__ = new _003Cget_AdjacentCellsCardinal_003Ed__46(0);
			}
			_003Cget_AdjacentCellsCardinal_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_AdjacentCellsCardinal_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Cells2D_003Ed__38 : IEnumerable<IntVec2>, IEnumerable, IEnumerator<IntVec2>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec2 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private int _003Cz_003E5__2;

		private int _003Cx_003E5__3;

		IntVec2 IEnumerator<IntVec2>.Current
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
		public _003Cget_Cells2D_003Ed__38(int _003C_003E1__state)
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
				_003Cx_003E5__3++;
				goto IL_0074;
			}
			_003C_003E1__state = -1;
			_003Cz_003E5__2 = _003C_003E4__this.minZ;
			goto IL_0097;
			IL_0074:
			if (_003Cx_003E5__3 <= _003C_003E4__this.maxX)
			{
				_003C_003E2__current = new IntVec2(_003Cx_003E5__3, _003Cz_003E5__2);
				_003C_003E1__state = 1;
				return true;
			}
			_003Cz_003E5__2++;
			goto IL_0097;
			IL_0097:
			if (_003Cz_003E5__2 <= _003C_003E4__this.maxZ)
			{
				_003Cx_003E5__3 = _003C_003E4__this.minX;
				goto IL_0074;
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
		IEnumerator<IntVec2> IEnumerable<IntVec2>.GetEnumerator()
		{
			_003Cget_Cells2D_003Ed__38 _003Cget_Cells2D_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_Cells2D_003Ed__ = this;
			}
			else
			{
				_003Cget_Cells2D_003Ed__ = new _003Cget_Cells2D_003Ed__38(0);
			}
			_003Cget_Cells2D_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_Cells2D_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec2>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Cells_003Ed__36 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private int _003Cz_003E5__2;

		private int _003Cx_003E5__3;

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
		public _003Cget_Cells_003Ed__36(int _003C_003E1__state)
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
				_003Cx_003E5__3++;
				goto IL_0075;
			}
			_003C_003E1__state = -1;
			_003Cz_003E5__2 = _003C_003E4__this.minZ;
			goto IL_0098;
			IL_0075:
			if (_003Cx_003E5__3 <= _003C_003E4__this.maxX)
			{
				_003C_003E2__current = new IntVec3(_003Cx_003E5__3, 0, _003Cz_003E5__2);
				_003C_003E1__state = 1;
				return true;
			}
			_003Cz_003E5__2++;
			goto IL_0098;
			IL_0098:
			if (_003Cz_003E5__2 <= _003C_003E4__this.maxZ)
			{
				_003Cx_003E5__3 = _003C_003E4__this.minX;
				goto IL_0075;
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
			_003Cget_Cells_003Ed__36 _003Cget_Cells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_Cells_003Ed__ = this;
			}
			else
			{
				_003Cget_Cells_003Ed__ = new _003Cget_Cells_003Ed__36(0);
			}
			_003Cget_Cells_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_Cells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Corners_003Ed__19 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

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
		public _003Cget_Corners_003Ed__19(int _003C_003E1__state)
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
				if (_003C_003E4__this.IsEmpty)
				{
					return false;
				}
				_003C_003E2__current = new IntVec3(_003C_003E4__this.minX, 0, _003C_003E4__this.minZ);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (_003C_003E4__this.Height > 1)
				{
					_003C_003E2__current = new IntVec3(_003C_003E4__this.minX, 0, _003C_003E4__this.maxZ);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_00eb;
			case 2:
				_003C_003E1__state = -1;
				if (_003C_003E4__this.Width > 1)
				{
					_003C_003E2__current = new IntVec3(_003C_003E4__this.maxX, 0, _003C_003E4__this.maxZ);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00eb;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00eb;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00eb:
				if (_003C_003E4__this.Width > 1)
				{
					_003C_003E2__current = new IntVec3(_003C_003E4__this.maxX, 0, _003C_003E4__this.minZ);
					_003C_003E1__state = 4;
					return true;
				}
				break;
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
			_003Cget_Corners_003Ed__19 _003Cget_Corners_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_Corners_003Ed__ = this;
			}
			else
			{
				_003Cget_Corners_003Ed__ = new _003Cget_Corners_003Ed__19(0);
			}
			_003Cget_Corners_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_Corners_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_EdgeCells_003Ed__40 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

		private int _003Cx_003E5__2;

		private int _003Cz_003E5__3;

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
		public _003Cget_EdgeCells_003Ed__40(int _003C_003E1__state)
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
				if (_003C_003E4__this.IsEmpty)
				{
					return false;
				}
				_003Cx_003E5__2 = _003C_003E4__this.minX;
				_003Cz_003E5__3 = _003C_003E4__this.minZ;
				goto IL_0095;
			case 1:
				_003C_003E1__state = -1;
				_003Cx_003E5__2++;
				goto IL_0095;
			case 2:
				_003C_003E1__state = -1;
				_003Cz_003E5__3++;
				goto IL_0102;
			case 3:
				_003C_003E1__state = -1;
				_003Cx_003E5__2--;
				goto IL_016f;
			case 4:
				{
					_003C_003E1__state = -1;
					_003Cz_003E5__3--;
					break;
				}
				IL_0102:
				if (_003Cz_003E5__3 <= _003C_003E4__this.maxZ)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003Cz_003E5__3);
					_003C_003E1__state = 2;
					return true;
				}
				_003Cz_003E5__3--;
				_003Cx_003E5__2--;
				goto IL_016f;
				IL_0095:
				if (_003Cx_003E5__2 <= _003C_003E4__this.maxX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003Cz_003E5__3);
					_003C_003E1__state = 1;
					return true;
				}
				_003Cx_003E5__2--;
				_003Cz_003E5__3++;
				goto IL_0102;
				IL_016f:
				if (_003Cx_003E5__2 >= _003C_003E4__this.minX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003Cz_003E5__3);
					_003C_003E1__state = 3;
					return true;
				}
				_003Cx_003E5__2++;
				_003Cz_003E5__3--;
				break;
			}
			if (_003Cz_003E5__3 > _003C_003E4__this.minZ)
			{
				_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003Cz_003E5__3);
				_003C_003E1__state = 4;
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
			_003Cget_EdgeCells_003Ed__40 _003Cget_EdgeCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_EdgeCells_003Ed__ = this;
			}
			else
			{
				_003Cget_EdgeCells_003Ed__ = new _003Cget_EdgeCells_003Ed__40(0);
			}
			_003Cget_EdgeCells_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_EdgeCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_EdgeCellsNoCorners_003Ed__42 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CellRect _003C_003E4__this;

		public CellRect _003C_003E3___003C_003E4__this;

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
		public _003Cget_EdgeCellsNoCorners_003Ed__42(int _003C_003E1__state)
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
					_003C_003E7__wrap1 = _003C_003E4__this.EdgeCells.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap1.Current;
					if (!_003C_003E4__this.IsCorner(current))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
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
			_003Cget_EdgeCellsNoCorners_003Ed__42 _003Cget_EdgeCellsNoCorners_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_EdgeCellsNoCorners_003Ed__ = this;
			}
			else
			{
				_003Cget_EdgeCellsNoCorners_003Ed__ = new _003Cget_EdgeCellsNoCorners_003Ed__42(0);
			}
			_003Cget_EdgeCellsNoCorners_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_EdgeCellsNoCorners_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	public int minX;

	public int maxX;

	public int minZ;

	public int maxZ;

	private static readonly HashSet<IntVec3> tmpUnvistedCells = new HashSet<IntVec3>();

	public static CellRect Empty => new CellRect(0, 0, 0, 0);

	public readonly bool IsEmpty
	{
		get
		{
			if (Width > 0)
			{
				return Height <= 0;
			}
			return true;
		}
	}

	public readonly int Area => Width * Height;

	public int Width
	{
		readonly get
		{
			if (minX > maxX)
			{
				return 0;
			}
			return maxX - minX + 1;
		}
		set
		{
			maxX = minX + Mathf.Max(value, 0) - 1;
		}
	}

	public int Height
	{
		readonly get
		{
			if (minZ > maxZ)
			{
				return 0;
			}
			return maxZ - minZ + 1;
		}
		set
		{
			maxZ = minZ + Mathf.Max(value, 0) - 1;
		}
	}

	public IntVec2 Size => new IntVec2(Width, Height);

	public readonly IEnumerable<IntVec3> Corners
	{
		[IteratorStateMachine(typeof(_003Cget_Corners_003Ed__19))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Corners_003Ed__19(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public IntVec3 Min
	{
		get
		{
			return new IntVec3(minX, 0, minZ);
		}
		set
		{
			minX = value.x;
			minZ = value.z;
		}
	}

	public IntVec3 Max
	{
		get
		{
			return new IntVec3(maxX, 0, maxZ);
		}
		set
		{
			maxX = value.x;
			maxZ = value.z;
		}
	}

	public IntVec3 RandomCell => new IntVec3(Rand.RangeInclusive(minX, maxX), 0, Rand.RangeInclusive(minZ, maxZ));

	public IntVec3 CenterCell => new IntVec3(minX + Width / 2, 0, minZ + Height / 2);

	public Vector3 CenterVector3 => new Vector3((float)minX + (float)Width / 2f, 0f, (float)minZ + (float)Height / 2f);

	public Vector3 RandomVector3 => new Vector3(Rand.Range(minX, (float)maxX + 1f), 0f, Rand.Range(minZ, (float)maxZ + 1f));

	public readonly IEnumerable<IntVec3> Cells
	{
		[IteratorStateMachine(typeof(_003Cget_Cells_003Ed__36))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Cells_003Ed__36(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public readonly IEnumerable<IntVec2> Cells2D
	{
		[IteratorStateMachine(typeof(_003Cget_Cells2D_003Ed__38))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Cells2D_003Ed__38(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public readonly IEnumerable<IntVec3> EdgeCells
	{
		[IteratorStateMachine(typeof(_003Cget_EdgeCells_003Ed__40))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_EdgeCells_003Ed__40(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public IEnumerable<IntVec3> EdgeCellsNoCorners
	{
		[IteratorStateMachine(typeof(_003Cget_EdgeCellsNoCorners_003Ed__42))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_EdgeCellsNoCorners_003Ed__42(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public int EdgeCellsCount
	{
		get
		{
			if (Area == 0)
			{
				return 0;
			}
			if (Area == 1)
			{
				return 1;
			}
			return Width * 2 + (Height - 2) * 2;
		}
	}

	public IEnumerable<IntVec3> AdjacentCellsCardinal
	{
		[IteratorStateMachine(typeof(_003Cget_AdjacentCellsCardinal_003Ed__46))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AdjacentCellsCardinal_003Ed__46(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public IEnumerable<IntVec3> AdjacentCells
	{
		[IteratorStateMachine(typeof(_003Cget_AdjacentCells_003Ed__48))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AdjacentCells_003Ed__48(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public readonly bool AreSidesEqualOrGreater(int width, int height)
	{
		if (Width >= width)
		{
			return Height >= height;
		}
		return false;
	}

	public IntVec3 FarthestPoint(IntVec3 startingPoint, Rot4 direction)
	{
		if (!Contains(startingPoint))
		{
			throw new ArgumentException(startingPoint.ToString());
		}
		return direction.AsInt switch
		{
			0 => new IntVec3(startingPoint.x, 0, maxZ), 
			1 => new IntVec3(maxX, 0, startingPoint.z), 
			2 => new IntVec3(startingPoint.x, 0, minZ), 
			3 => new IntVec3(minX, 0, startingPoint.z), 
			_ => throw new ArgumentException(direction.ToString()), 
		};
	}

	public static bool operator ==(CellRect lhs, CellRect rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(CellRect lhs, CellRect rhs)
	{
		return !(lhs == rhs);
	}

	public CellRect(int minX, int minZ, int width, int height)
	{
		this.minX = minX;
		this.minZ = minZ;
		maxX = minX + width - 1;
		maxZ = minZ + height - 1;
	}

	public static CellRect WholeMap(Map map)
	{
		return new CellRect(0, 0, map.Size.x, map.Size.z);
	}

	public static CellRect FromLimits(int minX, int minZ, int maxX, int maxZ)
	{
		CellRect result = default(CellRect);
		result.minX = Mathf.Min(minX, maxX);
		result.minZ = Mathf.Min(minZ, maxZ);
		result.maxX = Mathf.Max(maxX, minX);
		result.maxZ = Mathf.Max(maxZ, minZ);
		return result;
	}

	public static CellRect FromLimits(IntVec3 first, IntVec3 second)
	{
		CellRect result = default(CellRect);
		result.minX = Mathf.Min(first.x, second.x);
		result.minZ = Mathf.Min(first.z, second.z);
		result.maxX = Mathf.Max(first.x, second.x);
		result.maxZ = Mathf.Max(first.z, second.z);
		return result;
	}

	public static CellRect FromCellList(IEnumerable<IntVec3> cells)
	{
		CellRect cellRect = default(CellRect);
		cellRect.minX = int.MaxValue;
		cellRect.minZ = int.MaxValue;
		CellRect result = cellRect;
		foreach (IntVec3 cell in cells)
		{
			if (cell.x < result.minX)
			{
				result.minX = cell.x;
			}
			if (cell.z < result.minZ)
			{
				result.minZ = cell.z;
			}
			if (cell.x > result.maxX)
			{
				result.maxX = cell.x;
			}
			if (cell.z > result.maxZ)
			{
				result.maxZ = cell.z;
			}
		}
		return result;
	}

	public static CellRect CenteredOn(IntVec3 center, int radius)
	{
		CellRect result = default(CellRect);
		result.minX = center.x - radius;
		result.maxX = center.x + radius;
		result.minZ = center.z - radius;
		result.maxZ = center.z + radius;
		return result;
	}

	public static CellRect CenteredOn(IntVec3 center, int width, int height)
	{
		CellRect cellRect = default(CellRect);
		cellRect.minX = center.x - width / 2;
		cellRect.minZ = center.z - height / 2;
		CellRect result = cellRect;
		result.maxX = result.minX + width - 1;
		result.maxZ = result.minZ + height - 1;
		return result;
	}

	public static CellRect CenteredOn(IntVec3 center, IntVec2 size)
	{
		return CenteredOn(center, size.x, size.z);
	}

	public static CellRect ViewRect(Map map)
	{
		if (Current.ProgramState != ProgramState.Playing || Find.CurrentMap != map || WorldRendererUtility.WorldSelected)
		{
			return Empty;
		}
		return Find.CameraDriver.CurrentViewRect;
	}

	public static CellRect SingleCell(IntVec3 c)
	{
		return new CellRect(c.x, c.z, 1, 1);
	}

	public bool InBounds(Map map)
	{
		if (minX >= 0 && minZ >= 0 && maxX < map.Size.x)
		{
			return maxZ < map.Size.z;
		}
		return false;
	}

	public bool FullyContainedWithin(CellRect within)
	{
		CellRect cellRect = this;
		cellRect.ClipInsideRect(within);
		return this == cellRect;
	}

	public bool Overlaps(CellRect other)
	{
		if (IsEmpty || other.IsEmpty)
		{
			return false;
		}
		if (minX <= other.maxX && maxX >= other.minX && maxZ >= other.minZ)
		{
			return minZ <= other.maxZ;
		}
		return false;
	}

	public bool OverlapsCardinal(CellRect other)
	{
		if (IsEmpty || other.IsEmpty)
		{
			return false;
		}
		CellRect other2 = this;
		other2.minZ++;
		other2.maxZ--;
		CellRect other3 = this;
		other3.minX++;
		other3.maxX--;
		if (!other.Overlaps(other2))
		{
			return other.Overlaps(other3);
		}
		return true;
	}

	public bool Overlaps(Bounds other)
	{
		if (IsEmpty || other.size == Vector3.zero)
		{
			return false;
		}
		if ((float)minX <= other.max.x && (float)maxX >= other.min.x && (float)maxZ >= other.min.z)
		{
			return (float)minZ <= other.max.z;
		}
		return false;
	}

	public Bounds ToBounds()
	{
		Bounds result = default(Bounds);
		result.center = CenterVector3;
		result.size = new Vector3(Width, 1f, Height);
		return result;
	}

	public bool IsOnEdge(IntVec3 c)
	{
		if ((c.x != minX || c.z < minZ || c.z > maxZ) && (c.x != maxX || c.z < minZ || c.z > maxZ) && (c.z != minZ || c.x < minX || c.x > maxX))
		{
			if (c.z == maxZ && c.x >= minX)
			{
				return c.x <= maxX;
			}
			return false;
		}
		return true;
	}

	public bool IsOnEdge(IntVec3 c, Rot4 rot)
	{
		if (rot == Rot4.West)
		{
			if (c.x == minX && c.z >= minZ)
			{
				return c.z <= maxZ;
			}
			return false;
		}
		if (rot == Rot4.East)
		{
			if (c.x == maxX && c.z >= minZ)
			{
				return c.z <= maxZ;
			}
			return false;
		}
		if (rot == Rot4.South)
		{
			if (c.z == minZ && c.x >= minX)
			{
				return c.x <= maxX;
			}
			return false;
		}
		if (c.z == maxZ && c.x >= minX)
		{
			return c.x <= maxX;
		}
		return false;
	}

	public bool IsOnEdge(IntVec3 c, int edgeWidth)
	{
		if (!Contains(c))
		{
			return false;
		}
		if (c.x >= minX + edgeWidth && c.z >= minZ + edgeWidth && c.x < maxX + 1 - edgeWidth)
		{
			return c.z >= maxZ + 1 - edgeWidth;
		}
		return true;
	}

	public bool IsCorner(IntVec3 c)
	{
		if ((c.x != minX || c.z != minZ) && (c.x != maxX || c.z != minZ) && (c.x != minX || c.z != maxZ))
		{
			if (c.x == maxX)
			{
				return c.z == maxZ;
			}
			return false;
		}
		return true;
	}

	public IntVec3 ClosestCorner(IntVec3 cell)
	{
		IntVec3 corner = GetCorner(Rot4.North);
		IntVec3 corner2 = GetCorner(Rot4.East);
		IntVec3 corner3 = GetCorner(Rot4.South);
		IntVec3 corner4 = GetCorner(Rot4.West);
		IntVec3 intVec = cell - corner;
		IntVec3 intVec2 = cell - corner2;
		IntVec3 intVec3 = cell - corner3;
		IntVec3 intVec4 = cell - corner4;
		return GenMath.MinBy(corner, intVec.SqrMagnitude, corner2, intVec2.SqrMagnitude, corner3, intVec3.SqrMagnitude, corner4, intVec4.SqrMagnitude);
	}

	public Rot4 GetClosestEdge(IntVec3 c)
	{
		int num = Mathf.Abs(c.x - minX);
		int num2 = Mathf.Abs(c.x - maxX);
		int num3 = Mathf.Abs(c.z - maxZ);
		int num4 = Mathf.Abs(c.z - minZ);
		return GenMath.MinBy(Rot4.West, num, Rot4.East, num2, Rot4.North, num3, Rot4.South, num4);
	}

	public Rot4 GetEdgeCellRot(IntVec3 c)
	{
		if (c.x == maxX && c.z >= minZ && c.z <= maxZ)
		{
			return Rot4.East;
		}
		if (c.x == minX && c.z >= minZ && c.z <= maxZ)
		{
			return Rot4.West;
		}
		if (c.z == maxZ && c.x >= minX && c.x <= maxX)
		{
			return Rot4.North;
		}
		if (c.z == minZ && c.x >= minX && c.x <= maxX)
		{
			return Rot4.South;
		}
		Log.Error($"Attempted to get edge cell rotation but cell was not on an edge, cell: {c}, rect: {ToString()}");
		return Rot4.North;
	}

	public bool IsDiagonalCorner(IntVec3 c)
	{
		if (c.x == minX || c.x == maxX)
		{
			if (c.z != minZ)
			{
				return c.z == maxZ;
			}
			return true;
		}
		return false;
	}

	public IntVec3 GetCenterCellOnEdge(Rot4 rot)
	{
		return GetCellOnEdge(rot, CenterCell);
	}

	public IntVec3 GetCellOnEdge(Rot4 rot, IntVec3 point)
	{
		if (rot == Rot4.North)
		{
			return new IntVec3(point.x, point.y, maxZ);
		}
		if (rot == Rot4.East)
		{
			return new IntVec3(maxX, point.y, point.z);
		}
		if (rot == Rot4.South)
		{
			return new IntVec3(point.x, point.y, minZ);
		}
		if (rot == Rot4.West)
		{
			return new IntVec3(minX, point.y, point.z);
		}
		return IntVec3.Invalid;
	}

	public IntVec3 GetCenterCellOnEdge(Rot4 rot, int offset)
	{
		if (rot == Rot4.North)
		{
			return new IntVec3(CenterCell.x + offset, CenterCell.y, maxZ);
		}
		if (rot == Rot4.East)
		{
			return new IntVec3(maxX, CenterCell.y, CenterCell.z + offset);
		}
		if (rot == Rot4.South)
		{
			return new IntVec3(CenterCell.x + offset, CenterCell.y, minZ);
		}
		if (rot == Rot4.West)
		{
			return new IntVec3(minX, CenterCell.y, CenterCell.z + offset);
		}
		return IntVec3.Invalid;
	}

	[IteratorStateMachine(typeof(_003CGetCenterCellsOnEdge_003Ed__79))]
	public IEnumerable<IntVec3> GetCenterCellsOnEdge(Rot4 rot, int range)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCenterCellsOnEdge_003Ed__79(-2)
		{
			_003C_003E3___003C_003E4__this = this,
			_003C_003E3__rot = rot,
			_003C_003E3__range = range
		};
	}

	public IEnumerable<IntVec3> GetCellsCenterOutOnEdge(Rot4 rot)
	{
		return GetCenterCellsOnEdge(rot, GetSideLength(rot));
	}

	public int GetSideLength(Rot4 rot)
	{
		if (rot == Rot4.North || rot == Rot4.South)
		{
			return Width;
		}
		return Height;
	}

	[IteratorStateMachine(typeof(_003CGetCellsOnEdge_003Ed__82))]
	public IEnumerable<IntVec3> GetCellsOnEdge(Rot4 rot)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCellsOnEdge_003Ed__82(-2)
		{
			_003C_003E3___003C_003E4__this = this,
			_003C_003E3__rot = rot
		};
	}

	public CellRect ClipInsideMap(Map map)
	{
		if (minX < 0)
		{
			minX = 0;
		}
		if (minZ < 0)
		{
			minZ = 0;
		}
		if (maxX > map.Size.x - 1)
		{
			maxX = map.Size.x - 1;
		}
		if (maxZ > map.Size.z - 1)
		{
			maxZ = map.Size.z - 1;
		}
		return this;
	}

	public CellRect ClipInsideRect(CellRect otherRect)
	{
		if (minX < otherRect.minX)
		{
			minX = otherRect.minX;
		}
		if (maxX > otherRect.maxX)
		{
			maxX = otherRect.maxX;
		}
		if (minZ < otherRect.minZ)
		{
			minZ = otherRect.minZ;
		}
		if (maxZ > otherRect.maxZ)
		{
			maxZ = otherRect.maxZ;
		}
		return this;
	}

	public Rect ToScreenRect(Camera camera)
	{
		Vector3 vector = camera.WorldToScreenPoint(Min.ToVector3());
		Vector3 vector2 = camera.WorldToScreenPoint(Max.ToVector3() + new Vector3(1f, 0f, 1f));
		return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
	}

	public (CellRect bottom, CellRect up) SplitVertical(int separation = 0)
	{
		IntVec3 centerCell = CenterCell;
		CellRect item = this;
		CellRect item2 = this;
		item.maxZ = centerCell.z - separation;
		item2.minZ = centerCell.z + separation;
		return (bottom: item, up: item2);
	}

	public (CellRect left, CellRect right) SplitHorizontal(int separation = 0)
	{
		IntVec3 centerCell = CenterCell;
		CellRect item = this;
		CellRect item2 = this;
		item.maxX = centerCell.x - separation;
		item2.minX = centerCell.x + separation;
		return (left: item, right: item2);
	}

	public (CellRect leftBottom, CellRect leftUp, CellRect rightBottom, CellRect rightUp) Subdivide(int separation)
	{
		var (cellRect, cellRect2) = SplitHorizontal(separation);
		var (item, item2) = cellRect.SplitVertical(separation);
		var (item3, item4) = cellRect2.SplitVertical(separation);
		return (leftBottom: item, leftUp: item2, rightBottom: item3, rightUp: item4);
	}

	public List<CellRect> Subdivide(int times, int separation)
	{
		List<CellRect> list = new List<CellRect> { this };
		for (int i = 0; i < times; i++)
		{
			int count = list.Count;
			for (int j = 0; j < count; j++)
			{
				var (item, item2, item3, item4) = list[j].Subdivide(separation);
				list.Add(item);
				list.Add(item2);
				list.Add(item3);
				list.Add(item4);
			}
			list.RemoveRange(0, count);
		}
		return list;
	}

	public void SubdivideToMaxLength(int size, List<CellRect> rects)
	{
		rects.Clear();
		rects.Add(this);
		for (int i = 0; i < rects.Count; i++)
		{
			CellRect cellRect = rects[i];
			while (cellRect.Width > size)
			{
				rects.RemoveAt(i);
				(CellRect left, CellRect right) tuple = cellRect.SplitHorizontal(1);
				CellRect item = tuple.left;
				CellRect item2 = tuple.right;
				cellRect = item;
				rects.Add(item);
				rects.Add(item2);
			}
			while (cellRect.Height > size)
			{
				rects.RemoveAt(i);
				(CellRect bottom, CellRect up) tuple2 = cellRect.SplitVertical(1);
				CellRect item3 = tuple2.bottom;
				CellRect item4 = tuple2.up;
				cellRect = item3;
				rects.Add(item3);
				rects.Add(item4);
			}
		}
	}

	public bool TryContractToRemove(IntVec3 point, out CellRect rect)
	{
		rect = this;
		IntVec3 centerCell = CenterCell;
		if (point == centerCell)
		{
			if (Width < 3 && Height < 3)
			{
				rect = default(CellRect);
				return false;
			}
			if (Width > Height)
			{
				if (minX < point.x - 1)
				{
					rect.maxX = point.x - 1;
				}
				else
				{
					rect.minX = point.x + 1;
				}
			}
			else if (minZ < point.z - 1)
			{
				rect.maxZ = point.z - 1;
			}
			else
			{
				rect.minZ = point.z + 1;
			}
			return true;
		}
		if (point.x <= maxX && point.x >= minX)
		{
			if (point.x >= centerCell.x)
			{
				rect.maxX = point.x - 1;
			}
			else
			{
				rect.minX = point.x + 1;
			}
		}
		if (point.z <= maxZ && point.z >= minZ)
		{
			if (point.z >= centerCell.z)
			{
				rect.maxZ = point.z - 1;
			}
			else
			{
				rect.minZ = point.z + 1;
			}
		}
		return true;
	}

	public readonly bool Contains(IntVec3 c)
	{
		if (c.x >= minX && c.x <= maxX && c.z >= minZ)
		{
			return c.z <= maxZ;
		}
		return false;
	}

	public readonly bool InlineWith(CellRect other)
	{
		if (minX != other.minX || maxX != other.maxX)
		{
			if (minZ == other.minZ)
			{
				return maxZ == other.maxZ;
			}
			return false;
		}
		return true;
	}

	public IntVec3 GetCorner(Rot4 rot, bool clockwise = true)
	{
		if (rot == Rot4.North)
		{
			return new IntVec3(clockwise ? maxX : minX, 0, maxZ);
		}
		if (rot == Rot4.East)
		{
			return new IntVec3(clockwise ? maxX : minX, 0, minZ);
		}
		if (rot == Rot4.South)
		{
			return new IntVec3(clockwise ? minX : maxX, 0, minZ);
		}
		if (rot == Rot4.West)
		{
			return new IntVec3(clockwise ? minX : maxX, 0, maxZ);
		}
		return IntVec3.Invalid;
	}

	public void GetInternalCorners(out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
	{
		BL = new IntVec3(minX, 0, minZ);
		TL = new IntVec3(minX, 0, maxZ);
		TR = new IntVec3(maxX, 0, maxZ);
		BR = new IntVec3(maxX, 0, minZ);
	}

	public void GetAdjacentCorners(out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
	{
		BL = new IntVec3(minX - 1, 0, minZ - 1);
		TL = new IntVec3(minX - 1, 0, maxZ + 1);
		TR = new IntVec3(maxX + 1, 0, maxZ + 1);
		BR = new IntVec3(maxX + 1, 0, minZ - 1);
	}

	public float ClosestDistSquaredTo(IntVec3 c)
	{
		if (Contains(c))
		{
			return 0f;
		}
		if (c.x < minX)
		{
			if (c.z < minZ)
			{
				return (c - new IntVec3(minX, 0, minZ)).LengthHorizontalSquared;
			}
			if (c.z > maxZ)
			{
				return (c - new IntVec3(minX, 0, maxZ)).LengthHorizontalSquared;
			}
			return (minX - c.x) * (minX - c.x);
		}
		if (c.x > maxX)
		{
			if (c.z < minZ)
			{
				return (c - new IntVec3(maxX, 0, minZ)).LengthHorizontalSquared;
			}
			if (c.z > maxZ)
			{
				return (c - new IntVec3(maxX, 0, maxZ)).LengthHorizontalSquared;
			}
			return (c.x - maxX) * (c.x - maxX);
		}
		if (c.z < minZ)
		{
			return (minZ - c.z) * (minZ - c.z);
		}
		return (c.z - maxZ) * (c.z - maxZ);
	}

	public float ClosestDistanceTo(IntVec3 c)
	{
		return ClosestCellTo(c).DistanceTo(c);
	}

	public float DistanceToEdge(IntVec3 v)
	{
		return Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), Width - v.x - 1), Height - v.z - 1), 0);
	}

	public IntVec3 ClosestCellTo(IntVec3 c)
	{
		if (Contains(c))
		{
			return c;
		}
		if (c.x < minX)
		{
			if (c.z < minZ)
			{
				return new IntVec3(minX, 0, minZ);
			}
			if (c.z > maxZ)
			{
				return new IntVec3(minX, 0, maxZ);
			}
			return new IntVec3(minX, 0, c.z);
		}
		if (c.x > maxX)
		{
			if (c.z < minZ)
			{
				return new IntVec3(maxX, 0, minZ);
			}
			if (c.z > maxZ)
			{
				return new IntVec3(maxX, 0, maxZ);
			}
			return new IntVec3(maxX, 0, c.z);
		}
		if (c.z < minZ)
		{
			return new IntVec3(c.x, 0, minZ);
		}
		return new IntVec3(c.x, 0, maxZ);
	}

	public bool InNoBuildEdgeArea(Map map)
	{
		if (IsEmpty)
		{
			return false;
		}
		if (minX >= 10 && minZ >= 10 && maxX < map.Size.x - 10)
		{
			return maxZ >= map.Size.z - 10;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetEdgeCells_003Ed__102))]
	public IEnumerable<IntVec3> GetEdgeCells(Rot4 dir)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetEdgeCells_003Ed__102(-2)
		{
			_003C_003E3___003C_003E4__this = this,
			_003C_003E3__dir = dir
		};
	}

	public readonly CellRect GetEdgeRect(Rot4 rot)
	{
		if (rot == Rot4.North)
		{
			return FromLimits(new IntVec3(minX, 0, maxZ), new IntVec3(maxX, 0, maxZ));
		}
		if (rot == Rot4.South)
		{
			return FromLimits(new IntVec3(minX, 0, minZ), new IntVec3(maxX, 0, minZ));
		}
		if (rot == Rot4.West)
		{
			return FromLimits(new IntVec3(minX, 0, minZ), new IntVec3(minX, 0, maxZ));
		}
		return FromLimits(new IntVec3(maxX, 0, minZ), new IntVec3(maxX, 0, maxZ));
	}

	public bool TryFindRandomInnerRectTouchingEdge(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
	{
		if (Width < size.x || Height < size.z)
		{
			rect = Empty;
			return false;
		}
		if (size.x <= 0 || size.z <= 0 || IsEmpty)
		{
			rect = Empty;
			return false;
		}
		CellRect cellRect = this;
		cellRect.maxX -= size.x - 1;
		cellRect.maxZ -= size.z - 1;
		if (cellRect.EdgeCells.Where(delegate(IntVec3 x)
		{
			if (predicate == null)
			{
				return true;
			}
			CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
			return predicate(obj);
		}).TryRandomElement(out var result))
		{
			rect = new CellRect(result.x, result.z, size.x, size.z);
			return true;
		}
		rect = Empty;
		return false;
	}

	public readonly bool TryFindRandomInnerRectTouchingCorner(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
	{
		if (Width < size.x || Height < size.z)
		{
			rect = Empty;
			return false;
		}
		if (size.x <= 0 || size.z <= 0 || IsEmpty)
		{
			rect = Empty;
			return false;
		}
		CellRect cellRect = this;
		cellRect.maxX -= size.x - 1;
		cellRect.maxZ -= size.z - 1;
		if (cellRect.Corners.Where(delegate(IntVec3 x)
		{
			if (predicate == null)
			{
				return true;
			}
			CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
			return predicate(obj);
		}).TryRandomElement(out var result))
		{
			rect = new CellRect(result.x, result.z, size.x, size.z);
			return true;
		}
		rect = Empty;
		return false;
	}

	public readonly bool TryFindRandomCell(out IntVec3 cell, Predicate<IntVec3> validator = null)
	{
		foreach (IntVec3 item in Cells.InRandomOrder())
		{
			if (validator == null || validator(item))
			{
				cell = item;
				return true;
			}
		}
		cell = IntVec3.Invalid;
		return false;
	}

	public readonly bool TryFindRandomInnerRect(IntVec2 size, out CellRect rect, Predicate<CellRect> validator = null)
	{
		if (Width < size.x || Height < size.z)
		{
			rect = Empty;
			return false;
		}
		if (size.x <= 0 || size.z <= 0 || IsEmpty)
		{
			rect = Empty;
			return false;
		}
		CellRect cellRect = this;
		cellRect.maxX -= size.x - 1;
		cellRect.maxZ -= size.z - 1;
		if (cellRect.Cells.Where(delegate(IntVec3 x)
		{
			if (validator == null)
			{
				return true;
			}
			CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
			return validator(obj);
		}).TryRandomElement(out var result))
		{
			rect = new CellRect(result.x, result.z, size.x, size.z);
			return true;
		}
		rect = Empty;
		return false;
	}

	public readonly bool TryFindNearestInnerRectTo(IntVec2 size, IntVec3 position, out CellRect rect)
	{
		if (Width < size.x || Height < size.z)
		{
			rect = Empty;
			return false;
		}
		if (size.x <= 0 || size.z <= 0 || IsEmpty)
		{
			rect = Empty;
			return false;
		}
		CellRect cellRect = this;
		cellRect.maxX -= size.x - 1;
		cellRect.maxZ -= size.z - 1;
		CellRect cellRect2 = Empty;
		float num = float.MaxValue;
		foreach (IntVec3 cell in cellRect.Cells)
		{
			CellRect cellRect3 = new CellRect(cell.x, cell.z, size.x, size.z);
			int num2 = position.DistanceToSquared(cellRect3.CenterCell);
			if ((float)num2 < num)
			{
				cellRect2 = cellRect3;
				num = num2;
			}
		}
		rect = cellRect2;
		return true;
	}

	public readonly CellRect ExpandedBy(int dist)
	{
		CellRect result = this;
		result.minX -= dist;
		result.minZ -= dist;
		result.maxX += dist;
		result.maxZ += dist;
		return result;
	}

	public readonly CellRect ExpandedBy(int x, int z)
	{
		CellRect result = this;
		result.minX -= x;
		result.minZ -= z;
		result.maxX += x;
		result.maxZ += z;
		return result;
	}

	public readonly CellRect ExpandedBy(IntVec3 offset)
	{
		CellRect result = this;
		result.minX -= offset.x;
		result.minZ -= offset.z;
		result.maxX += offset.x;
		result.maxZ += offset.z;
		return result;
	}

	public readonly CellRect MaxExpandedBy(int dist)
	{
		CellRect result = this;
		result.maxX += dist;
		result.maxZ += dist;
		return result;
	}

	public readonly CellRect MaxExpandedBy(int x, int z)
	{
		CellRect result = this;
		result.maxX += x;
		result.maxZ += z;
		return result;
	}

	public readonly CellRect MaxExpandedBy(IntVec3 offset)
	{
		CellRect result = this;
		result.maxX += offset.x;
		result.maxZ += offset.z;
		return result;
	}

	public readonly CellRect MinExpandedBy(int dist)
	{
		CellRect result = this;
		result.minX -= dist;
		result.minZ -= dist;
		return result;
	}

	public readonly CellRect MinExpandedBy(int x, int z)
	{
		CellRect result = this;
		result.minX -= x;
		result.minZ -= z;
		return result;
	}

	public readonly CellRect MinExpandedBy(IntVec3 offset)
	{
		CellRect result = this;
		result.minX -= offset.x;
		result.minZ -= offset.z;
		return result;
	}

	public readonly CellRect ContractedBy(int x, int z)
	{
		return ExpandedBy(new IntVec3(-x, 0, -z));
	}

	public readonly CellRect ContractedBy(IntVec3 dist)
	{
		return ExpandedBy(-dist);
	}

	public readonly CellRect ContractedBy(int dist)
	{
		return ExpandedBy(-dist);
	}

	public CellRect ContractedBy(int x, int z, int negativeX, int negativeZ)
	{
		CellRect result = this;
		result.minX += x;
		result.minZ += z;
		result.maxX -= negativeX;
		result.maxZ -= negativeZ;
		return result;
	}

	public CellRect MovedBy(int x, int z)
	{
		return MovedBy(new IntVec3(x, 0, z));
	}

	public CellRect MovedBy(IntVec2 offset)
	{
		return MovedBy(offset.ToIntVec3);
	}

	public CellRect MovedBy(IntVec3 offset)
	{
		CellRect result = this;
		result.minX += offset.x;
		result.minZ += offset.z;
		result.maxX += offset.x;
		result.maxZ += offset.z;
		return result;
	}

	public CellRect Encapsulate(CellRect otherRect)
	{
		CellRect result = this;
		result.minX = Mathf.Min(minX, otherRect.minX);
		result.minZ = Mathf.Min(minZ, otherRect.minZ);
		result.maxX = Mathf.Max(maxX, otherRect.maxX);
		result.maxZ = Mathf.Max(maxZ, otherRect.maxZ);
		return result;
	}

	public CellRect Encapsulate(IntVec3 point)
	{
		CellRect result = this;
		result.minX = Mathf.Min(minX, point.x);
		result.minZ = Mathf.Min(minZ, point.z);
		result.maxX = Mathf.Max(maxX, point.x);
		result.maxZ = Mathf.Max(maxZ, point.z);
		return result;
	}

	[IteratorStateMachine(typeof(_003CUnionCells_003Ed__127))]
	public IEnumerable<IntVec3> UnionCells(CellRect container)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CUnionCells_003Ed__127(-2)
		{
			_003C_003E3___003C_003E4__this = this,
			_003C_003E3__container = container
		};
	}

	[IteratorStateMachine(typeof(_003CDifferenceCells_003Ed__128))]
	public IEnumerable<IntVec3> DifferenceCells(CellRect container)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CDifferenceCells_003Ed__128(-2)
		{
			_003C_003E3___003C_003E4__this = this,
			_003C_003E3__container = container
		};
	}

	public int IndexOf(IntVec3 location)
	{
		return location.x - minX + (location.z - minZ) * Width;
	}

	public void DebugDraw()
	{
		float y = AltitudeLayer.MetaOverlays.AltitudeFor();
		Vector3 vector = new Vector3(minX, y, minZ);
		Vector3 vector2 = new Vector3(minX, y, maxZ + 1);
		Vector3 vector3 = new Vector3(maxX + 1, y, maxZ + 1);
		Vector3 vector4 = new Vector3(maxX + 1, y, minZ);
		GenDraw.DrawLineBetween(vector, vector2);
		GenDraw.DrawLineBetween(vector2, vector3);
		GenDraw.DrawLineBetween(vector3, vector4);
		GenDraw.DrawLineBetween(vector4, vector);
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public override string ToString()
	{
		return "(" + minX + "," + minZ + "," + maxX + "," + maxZ + ")";
	}

	public void Reset()
	{
		this = default(CellRect);
	}

	public static CellRect FromString(string str)
	{
		str = str.TrimStart('(');
		str = str.TrimEnd(')');
		string[] array = str.Split(',');
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		int num = Convert.ToInt32(array[0], invariantCulture);
		int num2 = Convert.ToInt32(array[1], invariantCulture);
		int num3 = Convert.ToInt32(array[2], invariantCulture);
		int num4 = Convert.ToInt32(array[3], invariantCulture);
		return new CellRect(num, num2, num3 - num + 1, num4 - num2 + 1);
	}

	public override int GetHashCode()
	{
		return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, minX), maxX), minZ), maxZ);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is CellRect other))
		{
			return false;
		}
		return Equals(other);
	}

	public bool Equals(CellRect other)
	{
		if (minX == other.minX && maxX == other.maxX && minZ == other.minZ)
		{
			return maxZ == other.maxZ;
		}
		return false;
	}

	public static CellRect FromCell(IntVec3 cell)
	{
		return new CellRect(cell.x, cell.z, 1, 1);
	}

	[IteratorStateMachine(typeof(_003CEnumerateRectanglesCovering_003Ed__143))]
	public readonly IEnumerable<CellRect> EnumerateRectanglesCovering(Predicate<IntVec3> validator)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateRectanglesCovering_003Ed__143(-2)
		{
			_003C_003E3___003C_003E4__this = this,
			_003C_003E3__validator = validator
		};
	}
}
