using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable, IExposable
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__32 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SimpleCurve _003C_003E4__this;

		private string prefix;

		public string _003C_003E3__prefix;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__32(int _003C_003E1__state)
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
			SimpleCurve simpleCurve = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				for (int i = 0; i < simpleCurve.points.Count - 1; i++)
				{
					if (simpleCurve.points[i + 1].x < simpleCurve.points[i].x)
					{
						_003C_003E2__current = prefix + ": points are out of order";
						_003C_003E1__state = 1;
						return true;
					}
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__32 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__32(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.prefix = _003C_003E3__prefix;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__19 : IEnumerator<CurvePoint>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private CurvePoint _003C_003E2__current;

		public SimpleCurve _003C_003E4__this;

		private List<CurvePoint>.Enumerator _003C_003E7__wrap1;

		CurvePoint IEnumerator<CurvePoint>.Current
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
		public _003CGetEnumerator_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
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
			_003C_003E7__wrap1 = default(List<CurvePoint>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SimpleCurve simpleCurve = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = simpleCurve.points.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					CurvePoint current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<CurvePoint>.Enumerator);
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	private List<CurvePoint> points = new List<CurvePoint>();

	[Unsaved(false)]
	private SimpleCurveView view;

	private static Comparison<CurvePoint> CurvePointsComparer = delegate(CurvePoint a, CurvePoint b)
	{
		if (a.x < b.x)
		{
			return -1;
		}
		return (b.x < a.x) ? 1 : 0;
	};

	public int PointsCount => points.Count;

	public List<CurvePoint> Points => points;

	public bool HasView => view != null;

	public SimpleCurveView View
	{
		get
		{
			if (view == null)
			{
				view = new SimpleCurveView();
				view.SetViewRectAround(this);
			}
			return view;
		}
	}

	public float MinY => points.Min((CurvePoint point) => point.y);

	public float MaxY => points.Max((CurvePoint point) => point.y);

	public CurvePoint this[int i]
	{
		get
		{
			return points[i];
		}
		set
		{
			points[i] = value;
		}
	}

	public SimpleCurve(IEnumerable<CurvePoint> points)
	{
		SetPoints(points);
	}

	public SimpleCurve()
	{
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref points, "points", LookMode.Value);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__19))]
	public IEnumerator<CurvePoint> GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetEnumerator_003Ed__19(0)
		{
			_003C_003E4__this = this
		};
	}

	public void SetPoints(IEnumerable<CurvePoint> newPoints)
	{
		points.Clear();
		foreach (CurvePoint newPoint in newPoints)
		{
			points.Add(newPoint);
		}
		SortPoints();
	}

	public void Add(float x, float y, bool sort = true)
	{
		CurvePoint newPoint = new CurvePoint(x, y);
		Add(newPoint, sort);
	}

	public void Add(CurvePoint newPoint, bool sort = true)
	{
		points.Add(newPoint);
		if (sort)
		{
			SortPoints();
		}
	}

	public void SortPoints()
	{
		points.Sort(CurvePointsComparer);
	}

	public float ClampToCurve(float value)
	{
		if (points.Count == 0)
		{
			Log.Error("Clamping a value to an empty SimpleCurve.");
			return value;
		}
		return Mathf.Clamp(value, points[0].y, points[points.Count - 1].y);
	}

	public void RemovePointNear(CurvePoint point)
	{
		for (int i = 0; i < points.Count; i++)
		{
			if ((points[i].Loc - point.Loc).sqrMagnitude < 0.001f)
			{
				points.RemoveAt(i);
				break;
			}
		}
	}

	public float Evaluate(float x)
	{
		if (points.Count == 0)
		{
			Log.Error("Evaluating a SimpleCurve with no points.");
			return 0f;
		}
		if (x <= points[0].x)
		{
			return points[0].y;
		}
		if (x >= points[points.Count - 1].x)
		{
			return points[points.Count - 1].y;
		}
		CurvePoint curvePoint = points[0];
		CurvePoint curvePoint2 = points[points.Count - 1];
		for (int i = 0; i < points.Count; i++)
		{
			if (x <= points[i].x)
			{
				curvePoint2 = points[i];
				if (i > 0)
				{
					curvePoint = points[i - 1];
				}
				break;
			}
		}
		float t = (x - curvePoint.x) / (curvePoint2.x - curvePoint.x);
		return Mathf.Lerp(curvePoint.y, curvePoint2.y, t);
	}

	public float EvaluateInverted(float y)
	{
		if (points.Count == 0)
		{
			Log.Error("Evaluating a SimpleCurve with no points.");
			return 0f;
		}
		if (points.Count == 1)
		{
			return points[0].x;
		}
		for (int i = 0; i < points.Count - 1; i++)
		{
			if ((y >= points[i].y && y <= points[i + 1].y) || (y <= points[i].y && y >= points[i + 1].y))
			{
				if (y == points[i].y)
				{
					return points[i].x;
				}
				if (y == points[i + 1].y)
				{
					return points[i + 1].x;
				}
				return GenMath.LerpDouble(points[i].y, points[i + 1].y, points[i].x, points[i + 1].x, y);
			}
		}
		if (y < points[0].y)
		{
			float result = 0f;
			float num = 0f;
			for (int j = 0; j < points.Count; j++)
			{
				if (j == 0 || points[j].y < num)
				{
					num = points[j].y;
					result = points[j].x;
				}
			}
			return result;
		}
		float result2 = 0f;
		float num2 = 0f;
		for (int k = 0; k < points.Count; k++)
		{
			if (k == 0 || points[k].y > num2)
			{
				num2 = points[k].y;
				result2 = points[k].x;
			}
		}
		return result2;
	}

	public float PeriodProbabilityFromCumulative(float startX, float span)
	{
		if (points.Count < 2)
		{
			return 0f;
		}
		if (points[0].y != 0f)
		{
			Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.");
		}
		float num = Evaluate(startX + span) - Evaluate(startX);
		if (num < 0f)
		{
			Log.Error("PeriodicProbability got negative probability from " + this?.ToString() + ": slope should never be negative.");
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__32))]
	public IEnumerable<string> ConfigErrors(string prefix)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__32(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__prefix = prefix
		};
	}

	public static SimpleCurve Empty()
	{
		return new SimpleCurve { new CurvePoint(0f, 0f) };
	}

	public AnimationCurve ToAnimationCurve()
	{
		AnimationCurve animationCurve = new AnimationCurve();
		foreach (CurvePoint point in Points)
		{
			animationCurve.AddKey(point.x, point.y);
		}
		return animationCurve;
	}

	public static SimpleCurve FromAnimationCurve(AnimationCurve curve)
	{
		SimpleCurve simpleCurve = new SimpleCurve();
		UnityEngine.Keyframe[] keys = curve.keys;
		for (int i = 0; i < keys.Length; i++)
		{
			UnityEngine.Keyframe keyframe = keys[i];
			simpleCurve.Points.Add(new CurvePoint(keyframe.time, keyframe.value));
		}
		return simpleCurve;
	}
}
