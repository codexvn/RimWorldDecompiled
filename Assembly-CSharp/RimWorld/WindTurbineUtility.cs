using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class WindTurbineUtility
{
	[CompilerGenerated]
	private sealed class _003CCalculateWindCells_003Ed__0 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private CellRect _003CrectA_003E5__2;

		private CellRect _003CrectB_003E5__3;

		private int _003Cz_003E5__4;

		private int _003Cx_003E5__5;

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
		public _003CCalculateWindCells_003Ed__0(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003CrectA_003E5__2 = default(CellRect);
				_003CrectB_003E5__3 = default(CellRect);
				int num = 0;
				int num2;
				int num3;
				if (rot == Rot4.North || rot == Rot4.East)
				{
					num2 = 9;
					num3 = 5;
				}
				else
				{
					num2 = 5;
					num3 = 9;
					num = -1;
				}
				if (rot.IsHorizontal)
				{
					_003CrectA_003E5__2.minX = center.x + 2 + num;
					_003CrectA_003E5__2.maxX = center.x + 2 + num2 + num;
					_003CrectB_003E5__3.minX = center.x - 1 - num3 + num;
					_003CrectB_003E5__3.maxX = center.x - 1 + num;
					_003CrectB_003E5__3.minZ = (_003CrectA_003E5__2.minZ = center.z - 3);
					_003CrectB_003E5__3.maxZ = (_003CrectA_003E5__2.maxZ = center.z + 3);
				}
				else
				{
					_003CrectA_003E5__2.minZ = center.z + 2 + num;
					_003CrectA_003E5__2.maxZ = center.z + 2 + num2 + num;
					_003CrectB_003E5__3.minZ = center.z - 1 - num3 + num;
					_003CrectB_003E5__3.maxZ = center.z - 1 + num;
					_003CrectB_003E5__3.minX = (_003CrectA_003E5__2.minX = center.x - 3);
					_003CrectB_003E5__3.maxX = (_003CrectA_003E5__2.maxX = center.x + 3);
				}
				_003Cz_003E5__4 = _003CrectA_003E5__2.minZ;
				goto IL_0280;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Cx_003E5__5++;
				goto IL_025b;
			case 2:
				{
					_003C_003E1__state = -1;
					_003Cx_003E5__5++;
					goto IL_02f6;
				}
				IL_0280:
				if (_003Cz_003E5__4 <= _003CrectA_003E5__2.maxZ)
				{
					_003Cx_003E5__5 = _003CrectA_003E5__2.minX;
					goto IL_025b;
				}
				_003Cz_003E5__4 = _003CrectB_003E5__3.minZ;
				goto IL_031b;
				IL_02f6:
				if (_003Cx_003E5__5 <= _003CrectB_003E5__3.maxX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__5, 0, _003Cz_003E5__4);
					_003C_003E1__state = 2;
					return true;
				}
				_003Cz_003E5__4++;
				goto IL_031b;
				IL_031b:
				if (_003Cz_003E5__4 <= _003CrectB_003E5__3.maxZ)
				{
					_003Cx_003E5__5 = _003CrectB_003E5__3.minX;
					goto IL_02f6;
				}
				return false;
				IL_025b:
				if (_003Cx_003E5__5 <= _003CrectA_003E5__2.maxX)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__5, 0, _003Cz_003E5__4);
					_003C_003E1__state = 1;
					return true;
				}
				_003Cz_003E5__4++;
				goto IL_0280;
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
			_003CCalculateWindCells_003Ed__0 _003CCalculateWindCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCalculateWindCells_003Ed__ = this;
			}
			else
			{
				_003CCalculateWindCells_003Ed__ = new _003CCalculateWindCells_003Ed__0(0);
			}
			_003CCalculateWindCells_003Ed__.center = _003C_003E3__center;
			_003CCalculateWindCells_003Ed__.rot = _003C_003E3__rot;
			return _003CCalculateWindCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CCalculateWindCells_003Ed__0))]
	public static IEnumerable<IntVec3> CalculateWindCells(IntVec3 center, Rot4 rot, IntVec2 size)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCalculateWindCells_003Ed__0(-2)
		{
			_003C_003E3__center = center,
			_003C_003E3__rot = rot
		};
	}
}
