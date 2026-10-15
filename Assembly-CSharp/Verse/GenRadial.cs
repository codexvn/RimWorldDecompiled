using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public static class GenRadial
{
	[CompilerGenerated]
	private sealed class _003CRadialCellsAround_003Ed__16 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private float radius;

		public float _003C_003E3__radius;

		private bool useCenter;

		public bool _003C_003E3__useCenter;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private int _003CnumSquares_003E5__2;

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
		public _003CRadialCellsAround_003Ed__16(int _003C_003E1__state)
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
				_003CnumSquares_003E5__2 = NumCellsInRadius(radius);
				_003Ci_003E5__3 = ((!useCenter) ? 1 : 0);
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003CnumSquares_003E5__2)
			{
				_003C_003E2__current = RadialPattern[_003Ci_003E5__3] + center;
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
			_003CRadialCellsAround_003Ed__16 _003CRadialCellsAround_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRadialCellsAround_003Ed__ = this;
			}
			else
			{
				_003CRadialCellsAround_003Ed__ = new _003CRadialCellsAround_003Ed__16(0);
			}
			_003CRadialCellsAround_003Ed__.center = _003C_003E3__center;
			_003CRadialCellsAround_003Ed__.radius = _003C_003E3__radius;
			_003CRadialCellsAround_003Ed__.useCenter = _003C_003E3__useCenter;
			return _003CRadialCellsAround_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRadialCellsAround_003Ed__17 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private float maxRadius;

		public float _003C_003E3__maxRadius;

		private float minRadius;

		public float _003C_003E3__minRadius;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private int _003CnumSquares_003E5__2;

		private float _003CminRadiusSquared_003E5__3;

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
		public _003CRadialCellsAround_003Ed__17(int _003C_003E1__state)
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
				goto IL_0093;
			}
			_003C_003E1__state = -1;
			_003CnumSquares_003E5__2 = NumCellsInRadius(maxRadius);
			_003CminRadiusSquared_003E5__3 = minRadius * minRadius;
			_003Ci_003E5__4 = 0;
			goto IL_00a3;
			IL_0093:
			_003Ci_003E5__4++;
			goto IL_00a3;
			IL_00a3:
			if (_003Ci_003E5__4 < _003CnumSquares_003E5__2)
			{
				if ((float)RadialPattern[_003Ci_003E5__4].LengthHorizontalSquared >= _003CminRadiusSquared_003E5__3)
				{
					_003C_003E2__current = RadialPattern[_003Ci_003E5__4] + center;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0093;
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
			_003CRadialCellsAround_003Ed__17 _003CRadialCellsAround_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRadialCellsAround_003Ed__ = this;
			}
			else
			{
				_003CRadialCellsAround_003Ed__ = new _003CRadialCellsAround_003Ed__17(0);
			}
			_003CRadialCellsAround_003Ed__.center = _003C_003E3__center;
			_003CRadialCellsAround_003Ed__.minRadius = _003C_003E3__minRadius;
			_003CRadialCellsAround_003Ed__.maxRadius = _003C_003E3__maxRadius;
			return _003CRadialCellsAround_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRadialDistinctThingsAround_003Ed__18 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private float radius;

		public float _003C_003E3__radius;

		private bool useCenter;

		public bool _003C_003E3__useCenter;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private Map map;

		public Map _003C_003E3__map;

		private int _003CnumCells_003E5__2;

		private HashSet<Thing> _003CreturnedThings_003E5__3;

		private int _003Ci_003E5__4;

		private List<Thing> _003CthingList_003E5__5;

		private int _003Cj_003E5__6;

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
		public _003CRadialDistinctThingsAround_003Ed__18(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CreturnedThings_003E5__3 = null;
			_003CthingList_003E5__5 = null;
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
				goto IL_0101;
			}
			_003C_003E1__state = -1;
			_003CnumCells_003E5__2 = NumCellsInRadius(radius);
			_003CreturnedThings_003E5__3 = null;
			_003Ci_003E5__4 = ((!useCenter) ? 1 : 0);
			goto IL_013e;
			IL_012e:
			_003Ci_003E5__4++;
			goto IL_013e;
			IL_0111:
			if (_003Cj_003E5__6 < _003CthingList_003E5__5.Count)
			{
				Thing thing = _003CthingList_003E5__5[_003Cj_003E5__6];
				if (thing.def.size.x > 1 || thing.def.size.z > 1)
				{
					if (_003CreturnedThings_003E5__3 == null)
					{
						_003CreturnedThings_003E5__3 = new HashSet<Thing>();
					}
					if (!_003CreturnedThings_003E5__3.Add(thing))
					{
						goto IL_0101;
					}
				}
				_003C_003E2__current = thing;
				_003C_003E1__state = 1;
				return true;
			}
			_003CthingList_003E5__5 = null;
			goto IL_012e;
			IL_013e:
			if (_003Ci_003E5__4 < _003CnumCells_003E5__2)
			{
				IntVec3 c = RadialPattern[_003Ci_003E5__4] + center;
				if (c.InBounds(map))
				{
					_003CthingList_003E5__5 = c.GetThingList(map);
					_003Cj_003E5__6 = 0;
					goto IL_0111;
				}
				goto IL_012e;
			}
			return false;
			IL_0101:
			_003Cj_003E5__6++;
			goto IL_0111;
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
			_003CRadialDistinctThingsAround_003Ed__18 _003CRadialDistinctThingsAround_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRadialDistinctThingsAround_003Ed__ = this;
			}
			else
			{
				_003CRadialDistinctThingsAround_003Ed__ = new _003CRadialDistinctThingsAround_003Ed__18(0);
			}
			_003CRadialDistinctThingsAround_003Ed__.center = _003C_003E3__center;
			_003CRadialDistinctThingsAround_003Ed__.map = _003C_003E3__map;
			_003CRadialDistinctThingsAround_003Ed__.radius = _003C_003E3__radius;
			_003CRadialDistinctThingsAround_003Ed__.useCenter = _003C_003E3__useCenter;
			return _003CRadialDistinctThingsAround_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRadialPatternInRadius_003Ed__15 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private float radius;

		public float _003C_003E3__radius;

		private int _003CnumSquares_003E5__2;

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
		public _003CRadialPatternInRadius_003Ed__15(int _003C_003E1__state)
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
				_003CnumSquares_003E5__2 = NumCellsInRadius(radius);
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003CnumSquares_003E5__2)
			{
				_003C_003E2__current = RadialPattern[_003Ci_003E5__3];
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
			_003CRadialPatternInRadius_003Ed__15 _003CRadialPatternInRadius_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRadialPatternInRadius_003Ed__ = this;
			}
			else
			{
				_003CRadialPatternInRadius_003Ed__ = new _003CRadialPatternInRadius_003Ed__15(0);
			}
			_003CRadialPatternInRadius_003Ed__.radius = _003C_003E3__radius;
			return _003CRadialPatternInRadius_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	public static IntVec3[] ManualRadialPattern;

	public static IntVec3[] RadialPattern;

	public static float[] RadialPatternRadii;

	private static int[] LengthSquaredToIndexArray;

	private const int RadialPatternCount = 20000;

	private const int MAX_RADIUS = 80;

	private static readonly List<IntVec3> tmpCells;

	private static bool working;

	public static float MaxRadialPatternRadius => RadialPatternRadii[^1];

	static GenRadial()
	{
		ManualRadialPattern = new IntVec3[49];
		RadialPattern = new IntVec3[20000];
		RadialPatternRadii = new float[20000];
		tmpCells = new List<IntVec3>();
		SetupManualRadialPattern();
		SetupRadialPattern();
	}

	private static void SetupManualRadialPattern()
	{
		ManualRadialPattern[0] = new IntVec3(0, 0, 0);
		ManualRadialPattern[1] = new IntVec3(0, 0, -1);
		ManualRadialPattern[2] = new IntVec3(1, 0, 0);
		ManualRadialPattern[3] = new IntVec3(0, 0, 1);
		ManualRadialPattern[4] = new IntVec3(-1, 0, 0);
		ManualRadialPattern[5] = new IntVec3(1, 0, -1);
		ManualRadialPattern[6] = new IntVec3(1, 0, 1);
		ManualRadialPattern[7] = new IntVec3(-1, 0, 1);
		ManualRadialPattern[8] = new IntVec3(-1, 0, -1);
		ManualRadialPattern[9] = new IntVec3(2, 0, 0);
		ManualRadialPattern[10] = new IntVec3(-2, 0, 0);
		ManualRadialPattern[11] = new IntVec3(0, 0, 2);
		ManualRadialPattern[12] = new IntVec3(0, 0, -2);
		ManualRadialPattern[13] = new IntVec3(2, 0, 1);
		ManualRadialPattern[14] = new IntVec3(2, 0, -1);
		ManualRadialPattern[15] = new IntVec3(-2, 0, 1);
		ManualRadialPattern[16] = new IntVec3(-2, 0, -1);
		ManualRadialPattern[17] = new IntVec3(-1, 0, 2);
		ManualRadialPattern[18] = new IntVec3(1, 0, 2);
		ManualRadialPattern[19] = new IntVec3(-1, 0, -2);
		ManualRadialPattern[20] = new IntVec3(1, 0, -2);
		ManualRadialPattern[21] = new IntVec3(2, 0, 2);
		ManualRadialPattern[22] = new IntVec3(-2, 0, -2);
		ManualRadialPattern[23] = new IntVec3(2, 0, -2);
		ManualRadialPattern[24] = new IntVec3(-2, 0, 2);
		ManualRadialPattern[25] = new IntVec3(3, 0, 0);
		ManualRadialPattern[26] = new IntVec3(0, 0, 3);
		ManualRadialPattern[27] = new IntVec3(-3, 0, 0);
		ManualRadialPattern[28] = new IntVec3(0, 0, -3);
		ManualRadialPattern[29] = new IntVec3(3, 0, 1);
		ManualRadialPattern[30] = new IntVec3(-3, 0, -1);
		ManualRadialPattern[31] = new IntVec3(1, 0, 3);
		ManualRadialPattern[32] = new IntVec3(-1, 0, -3);
		ManualRadialPattern[33] = new IntVec3(-3, 0, 1);
		ManualRadialPattern[34] = new IntVec3(3, 0, -1);
		ManualRadialPattern[35] = new IntVec3(-1, 0, 3);
		ManualRadialPattern[36] = new IntVec3(1, 0, -3);
		ManualRadialPattern[37] = new IntVec3(3, 0, 2);
		ManualRadialPattern[38] = new IntVec3(-3, 0, -2);
		ManualRadialPattern[39] = new IntVec3(2, 0, 3);
		ManualRadialPattern[40] = new IntVec3(-2, 0, -3);
		ManualRadialPattern[41] = new IntVec3(-3, 0, 2);
		ManualRadialPattern[42] = new IntVec3(3, 0, -2);
		ManualRadialPattern[43] = new IntVec3(-2, 0, 3);
		ManualRadialPattern[44] = new IntVec3(2, 0, -3);
		ManualRadialPattern[45] = new IntVec3(3, 0, 3);
		ManualRadialPattern[46] = new IntVec3(3, 0, -3);
		ManualRadialPattern[47] = new IntVec3(-3, 0, 3);
		ManualRadialPattern[48] = new IntVec3(-3, 0, -3);
	}

	private static void SetupRadialPattern()
	{
		List<IntVec3> list = new List<IntVec3>();
		for (int i = -80; i < 80; i++)
		{
			for (int j = -80; j < 80; j++)
			{
				list.Add(new IntVec3(i, 0, j));
			}
		}
		list.Sort(delegate(IntVec3 a, IntVec3 b)
		{
			float num = a.LengthHorizontalSquared;
			float num2 = b.LengthHorizontalSquared;
			if (num < num2)
			{
				return -1;
			}
			return (num != num2) ? 1 : 0;
		});
		for (int k = 0; k < 20000; k++)
		{
			RadialPattern[k] = list[k];
			RadialPatternRadii[k] = list[k].LengthHorizontal;
		}
		BuildLengthSquaredIndex();
	}

	private static void BuildLengthSquaredIndex()
	{
		int num = 6400;
		LengthSquaredToIndexArray = new int[num + 1];
		for (int i = 0; i <= num; i++)
		{
			LengthSquaredToIndexArray[i] = -1;
		}
		for (int j = 0; j < 20000; j++)
		{
			int lengthHorizontalSquared = RadialPattern[j].LengthHorizontalSquared;
			if (LengthSquaredToIndexArray[lengthHorizontalSquared] == -1)
			{
				LengthSquaredToIndexArray[lengthHorizontalSquared] = j;
			}
		}
		int num2 = 0;
		for (int k = 0; k <= num; k++)
		{
			if (LengthSquaredToIndexArray[k] != -1)
			{
				num2 = LengthSquaredToIndexArray[k];
			}
			else
			{
				LengthSquaredToIndexArray[k] = num2;
			}
		}
	}

	public static int NumCellsToFillForRadius_ManualRadialPattern(int radius)
	{
		switch (radius)
		{
		case 0:
			return 1;
		case 1:
			return 9;
		case 2:
			return 21;
		case 3:
			return 37;
		default:
			Log.Error("NumSquares radius error");
			return 0;
		}
	}

	public static int NumCellsInRadius(float radius)
	{
		if (radius >= MaxRadialPatternRadius)
		{
			Log.Error($"Not enough squares to get to radius {radius}. Max is {MaxRadialPatternRadius}");
			return 20000;
		}
		float num = radius + float.Epsilon;
		int num2 = (int)Math.Floor(num * num);
		int num3 = 6400;
		if (num2 > num3)
		{
			num2 = num3;
		}
		for (int i = LengthSquaredToIndexArray[num2]; i < 20000; i++)
		{
			if (RadialPatternRadii[i] > num)
			{
				return i;
			}
		}
		return 20000;
	}

	public static float RadiusOfNumCells(int numCells)
	{
		return RadialPatternRadii[numCells];
	}

	[IteratorStateMachine(typeof(_003CRadialPatternInRadius_003Ed__15))]
	public static IEnumerable<IntVec3> RadialPatternInRadius(float radius)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRadialPatternInRadius_003Ed__15(-2)
		{
			_003C_003E3__radius = radius
		};
	}

	[IteratorStateMachine(typeof(_003CRadialCellsAround_003Ed__16))]
	public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float radius, bool useCenter)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRadialCellsAround_003Ed__16(-2)
		{
			_003C_003E3__center = center,
			_003C_003E3__radius = radius,
			_003C_003E3__useCenter = useCenter
		};
	}

	[IteratorStateMachine(typeof(_003CRadialCellsAround_003Ed__17))]
	public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float minRadius, float maxRadius)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRadialCellsAround_003Ed__17(-2)
		{
			_003C_003E3__center = center,
			_003C_003E3__minRadius = minRadius,
			_003C_003E3__maxRadius = maxRadius
		};
	}

	[IteratorStateMachine(typeof(_003CRadialDistinctThingsAround_003Ed__18))]
	public static IEnumerable<Thing> RadialDistinctThingsAround(IntVec3 center, Map map, float radius, bool useCenter)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRadialDistinctThingsAround_003Ed__18(-2)
		{
			_003C_003E3__center = center,
			_003C_003E3__map = map,
			_003C_003E3__radius = radius,
			_003C_003E3__useCenter = useCenter
		};
	}

	public static void ProcessEquidistantCells(IntVec3 center, float radius, Func<List<IntVec3>, bool> processor, Map map = null)
	{
		if (working)
		{
			Log.Error("Nested calls to ProcessEquidistantCells() are not allowed.");
			return;
		}
		tmpCells.Clear();
		working = true;
		try
		{
			float num = -1f;
			int num2 = NumCellsInRadius(radius);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 intVec = center + RadialPattern[i];
				if (map != null && !intVec.InBounds(map))
				{
					continue;
				}
				float num3 = intVec.DistanceToSquared(center);
				if (Mathf.Abs(num3 - num) > 0.0001f)
				{
					if (tmpCells.Any() && processor(tmpCells))
					{
						return;
					}
					num = num3;
					tmpCells.Clear();
				}
				tmpCells.Add(intVec);
			}
			if (tmpCells.Any())
			{
				processor(tmpCells);
			}
		}
		finally
		{
			tmpCells.Clear();
			working = false;
		}
	}
}
