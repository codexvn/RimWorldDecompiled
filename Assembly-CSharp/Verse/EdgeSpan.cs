using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public struct EdgeSpan
{
	[CompilerGenerated]
	private sealed class _003Cget_Cells_003Ed__6 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public EdgeSpan _003C_003E4__this;

		public EdgeSpan _003C_003E3___003C_003E4__this;

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
		public _003Cget_Cells_003Ed__6(int _003C_003E1__state)
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
				_003Ci_003E5__2 = 0;
				goto IL_00e1;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00d1;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_00d1;
				}
				IL_00e1:
				if (_003Ci_003E5__2 < _003C_003E4__this.length)
				{
					if (_003C_003E4__this.dir == SpanDirection.North)
					{
						_003C_003E2__current = new IntVec3(_003C_003E4__this.root.x, 0, _003C_003E4__this.root.z + _003Ci_003E5__2);
						_003C_003E1__state = 1;
						return true;
					}
					if (_003C_003E4__this.dir == SpanDirection.East)
					{
						_003C_003E2__current = new IntVec3(_003C_003E4__this.root.x + _003Ci_003E5__2, 0, _003C_003E4__this.root.z);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d1;
				}
				return false;
				IL_00d1:
				_003Ci_003E5__2++;
				goto IL_00e1;
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
			_003Cget_Cells_003Ed__6 _003Cget_Cells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_Cells_003Ed__ = this;
			}
			else
			{
				_003Cget_Cells_003Ed__ = new _003Cget_Cells_003Ed__6(0);
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

	public IntVec3 root;

	public SpanDirection dir;

	public int length;

	public bool IsValid => length > 0;

	public IEnumerable<IntVec3> Cells
	{
		[IteratorStateMachine(typeof(_003Cget_Cells_003Ed__6))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Cells_003Ed__6(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public override string ToString()
	{
		string[] obj = new string[7] { "(root=", null, null, null, null, null, null };
		IntVec3 intVec = root;
		obj[1] = intVec.ToString();
		obj[2] = ", dir=";
		obj[3] = dir.ToString();
		obj[4] = " + length=";
		obj[5] = length.ToString();
		obj[6] = ")";
		return string.Concat(obj);
	}

	public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
	{
		this.root = root;
		this.dir = dir;
		this.length = length;
	}

	public ulong UniqueHashCode()
	{
		ulong num = root.UniqueHashCode();
		if (dir == SpanDirection.East)
		{
			num += 17592186044416L;
		}
		return num + (ulong)(281474976710656L * length);
	}
}
