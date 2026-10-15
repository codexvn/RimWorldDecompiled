using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public class SimpleCurveView
{
	[CompilerGenerated]
	private sealed class _003Cget_DebugInputValues_003Ed__5 : IEnumerable<float>, IEnumerable, IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SimpleCurveView _003C_003E4__this;

		private Dictionary<object, float>.ValueCollection.Enumerator _003C_003E7__wrap1;

		float IEnumerator<float>.Current
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
		public _003Cget_DebugInputValues_003Ed__5(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(Dictionary<object, float>.ValueCollection.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SimpleCurveView simpleCurveView = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (simpleCurveView.debugInputValues == null)
					{
						return false;
					}
					_003C_003E7__wrap1 = simpleCurveView.debugInputValues.Values.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					float current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(Dictionary<object, float>.ValueCollection.Enumerator);
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

		[DebuggerHidden]
		IEnumerator<float> IEnumerable<float>.GetEnumerator()
		{
			_003Cget_DebugInputValues_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_DebugInputValues_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<float>)this).GetEnumerator();
		}
	}

	public Rect rect;

	private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

	private const float ResetZoomBuffer = 0.1f;

	private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);

	public IEnumerable<float> DebugInputValues
	{
		[IteratorStateMachine(typeof(_003Cget_DebugInputValues_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_DebugInputValues_003Ed__5(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public void SetDebugInput(object key, float value)
	{
		debugInputValues[key] = value;
	}

	public void ClearDebugInputFrom(object key)
	{
		if (debugInputValues.ContainsKey(key))
		{
			debugInputValues.Remove(key);
		}
	}

	public void SetViewRectAround(SimpleCurve curve)
	{
		if (curve.PointsCount == 0)
		{
			rect = identityRect;
			return;
		}
		rect.xMin = curve.Points.Select((CurvePoint pt) => pt.Loc.x).Min();
		rect.xMax = curve.Points.Select((CurvePoint pt) => pt.Loc.x).Max();
		rect.yMin = curve.Points.Select((CurvePoint pt) => pt.Loc.y).Min();
		rect.yMax = curve.Points.Select((CurvePoint pt) => pt.Loc.y).Max();
		if (Mathf.Approximately(rect.width, 0f))
		{
			rect.width = rect.xMin * 2f;
		}
		if (Mathf.Approximately(rect.height, 0f))
		{
			rect.height = rect.yMin * 2f;
		}
		if (Mathf.Approximately(rect.width, 0f))
		{
			rect.width = 1f;
		}
		if (Mathf.Approximately(rect.height, 0f))
		{
			rect.height = 1f;
		}
		float width = rect.width;
		float height = rect.height;
		rect.xMin -= width * 0.1f;
		rect.xMax += width * 0.1f;
		rect.yMin -= height * 0.1f;
		rect.yMax += height * 0.1f;
	}
}
