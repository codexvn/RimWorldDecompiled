using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class BoolGrid : IExposable
{
	[CompilerGenerated]
	private sealed class _003Cget_ActiveCells_003Ed__13 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BoolGrid _003C_003E4__this;

		private int _003CyieldedCount_003E5__2;

		private bool _003CcanSetMinPossibleTrueIndex_003E5__3;

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
		public _003Cget_ActiveCells_003Ed__13(int _003C_003E1__state)
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
			BoolGrid boolGrid = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003CyieldedCount_003E5__2++;
				if (_003CyieldedCount_003E5__2 >= boolGrid.trueCountInt)
				{
					return false;
				}
				goto IL_00dc;
			}
			_003C_003E1__state = -1;
			if (boolGrid.trueCountInt == 0)
			{
				return false;
			}
			_003CyieldedCount_003E5__2 = 0;
			_003CcanSetMinPossibleTrueIndex_003E5__3 = boolGrid.minPossibleTrueIndexDirty;
			int num2 = ((!boolGrid.minPossibleTrueIndexDirty) ? boolGrid.minPossibleTrueIndexCached : 0);
			_003Ci_003E5__4 = num2;
			goto IL_00ec;
			IL_00ec:
			if (_003Ci_003E5__4 < boolGrid.arr.Length)
			{
				if (boolGrid.arr[_003Ci_003E5__4])
				{
					if (_003CcanSetMinPossibleTrueIndex_003E5__3 && boolGrid.minPossibleTrueIndexDirty)
					{
						_003CcanSetMinPossibleTrueIndex_003E5__3 = false;
						boolGrid.minPossibleTrueIndexDirty = false;
						boolGrid.minPossibleTrueIndexCached = _003Ci_003E5__4;
					}
					_003C_003E2__current = CellIndicesUtility.IndexToCell(_003Ci_003E5__4, boolGrid.width);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00dc;
			}
			return false;
			IL_00dc:
			_003Ci_003E5__4++;
			goto IL_00ec;
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
			_003Cget_ActiveCells_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ActiveCells_003Ed__13(0)
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

	private bool[] arr;

	private int trueCountInt;

	private int width;

	private int height;

	private int minPossibleTrueIndexCached = -1;

	private bool minPossibleTrueIndexDirty;

	public int TrueCount => trueCountInt;

	public int Width => width;

	public int Height => height;

	public IEnumerable<IntVec3> ActiveCells
	{
		[IteratorStateMachine(typeof(_003Cget_ActiveCells_003Ed__13))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ActiveCells_003Ed__13(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool this[int index]
	{
		get
		{
			return arr[index];
		}
		set
		{
			Set(index, value);
		}
	}

	public bool this[IntVec3 c]
	{
		get
		{
			return arr[CellIndicesUtility.CellToIndex(c, width)];
		}
		set
		{
			Set(c, value);
		}
	}

	public bool this[int x, int z]
	{
		get
		{
			return arr[CellIndicesUtility.CellToIndex(x, z, width)];
		}
		set
		{
			Set(CellIndicesUtility.CellToIndex(x, z, width), value);
		}
	}

	public BoolGrid()
	{
	}

	public BoolGrid(Map map)
	{
		ClearAndResizeTo(map);
	}

	public BoolGrid(int x, int z)
	{
		ClearAndResizeTo(x, z);
	}

	public bool MapSizeMatches(Map map)
	{
		if (width == map.Size.x)
		{
			return height == map.Size.z;
		}
		return false;
	}

	public void ClearAndResizeTo(Map map)
	{
		if (MapSizeMatches(map))
		{
			if (arr != null)
			{
				Clear();
			}
		}
		else
		{
			ClearAndResizeTo(map.Size.x, map.Size.z);
		}
	}

	public void ClearAndResizeTo(int x, int z)
	{
		if (width == x && height == z)
		{
			if (arr != null)
			{
				Clear();
			}
			return;
		}
		width = x;
		height = z;
		arr = new bool[width * height];
		trueCountInt = 0;
		minPossibleTrueIndexCached = -1;
		minPossibleTrueIndexDirty = false;
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref trueCountInt, "trueCount", 0);
		Scribe_Values.Look(ref width, "mapSizeX", 0);
		Scribe_Values.Look(ref height, "mapSizeZ", 0);
		DataExposeUtility.LookBoolArray(ref arr, width * height, "arr");
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			minPossibleTrueIndexDirty = true;
		}
	}

	public void Clear()
	{
		Array.Clear(arr, 0, arr.Length);
		trueCountInt = 0;
		minPossibleTrueIndexCached = -1;
		minPossibleTrueIndexDirty = false;
	}

	public virtual void Set(IntVec3 c, bool value)
	{
		Set(CellIndicesUtility.CellToIndex(c, width), value);
	}

	public virtual void Set(int index, bool value)
	{
		if (arr[index] == value)
		{
			return;
		}
		arr[index] = value;
		if (value)
		{
			trueCountInt++;
			if (trueCountInt == 1 || index < minPossibleTrueIndexCached)
			{
				minPossibleTrueIndexCached = index;
			}
		}
		else
		{
			trueCountInt--;
			if (index == minPossibleTrueIndexCached)
			{
				minPossibleTrueIndexDirty = true;
			}
		}
	}

	public void Invert()
	{
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i] = !arr[i];
		}
		trueCountInt = arr.Length - trueCountInt;
		minPossibleTrueIndexDirty = true;
	}
}
