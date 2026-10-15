using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class AttachPointTracker
{
	[CompilerGenerated]
	private sealed class _003CPointTypes_003Ed__11 : IEnumerable<AttachPointType>, IEnumerable, IEnumerator<AttachPointType>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private AttachPointType _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AttachPointTracker _003C_003E4__this;

		private int min;

		public int _003C_003E3__min;

		private int max;

		public int _003C_003E3__max;

		private Dictionary<AttachPointType, AttachPoint>.Enumerator _003C_003E7__wrap1;

		AttachPointType IEnumerator<AttachPointType>.Current
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
		public _003CPointTypes_003Ed__11(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(Dictionary<AttachPointType, AttachPoint>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				AttachPointTracker attachPointTracker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					attachPointTracker.TryCacheKindDefOverrides();
					_003C_003E7__wrap1 = attachPointTracker.cache.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					KeyValuePair<AttachPointType, AttachPoint> current = _003C_003E7__wrap1.Current;
					int key = (int)current.Key;
					if (key >= min && key <= max)
					{
						_003C_003E2__current = current.Key;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(Dictionary<AttachPointType, AttachPoint>.Enumerator);
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
		IEnumerator<AttachPointType> IEnumerable<AttachPointType>.GetEnumerator()
		{
			_003CPointTypes_003Ed__11 _003CPointTypes_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPointTypes_003Ed__ = this;
			}
			else
			{
				_003CPointTypes_003Ed__ = new _003CPointTypes_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CPointTypes_003Ed__.min = _003C_003E3__min;
			_003CPointTypes_003Ed__.max = _003C_003E3__max;
			return _003CPointTypes_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<AttachPointType>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPoints_003Ed__12 : IEnumerable<AttachPoint>, IEnumerable, IEnumerator<AttachPoint>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private AttachPoint _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AttachPointTracker _003C_003E4__this;

		private int min;

		public int _003C_003E3__min;

		private int max;

		public int _003C_003E3__max;

		private Dictionary<AttachPointType, AttachPoint>.Enumerator _003C_003E7__wrap1;

		AttachPoint IEnumerator<AttachPoint>.Current
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
		public _003CPoints_003Ed__12(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(Dictionary<AttachPointType, AttachPoint>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				AttachPointTracker attachPointTracker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = attachPointTracker.cache.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					KeyValuePair<AttachPointType, AttachPoint> current = _003C_003E7__wrap1.Current;
					int key = (int)current.Key;
					if (key >= min && key <= max)
					{
						_003C_003E2__current = current.Value;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(Dictionary<AttachPointType, AttachPoint>.Enumerator);
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
		IEnumerator<AttachPoint> IEnumerable<AttachPoint>.GetEnumerator()
		{
			_003CPoints_003Ed__12 _003CPoints_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPoints_003Ed__ = this;
			}
			else
			{
				_003CPoints_003Ed__ = new _003CPoints_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CPoints_003Ed__.min = _003C_003E3__min;
			_003CPoints_003Ed__.max = _003C_003E3__max;
			return _003CPoints_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<AttachPoint>)this).GetEnumerator();
		}
	}

	private ThingWithComps parent;

	private Dictionary<AttachPointType, AttachPoint> cache;

	private bool didReadOverrides;

	public string ThingId => parent.ThingID;

	public AttachPointTracker(List<AttachPoint> points, ThingWithComps parent)
	{
		this.parent = parent;
		cache = new Dictionary<AttachPointType, AttachPoint>();
		foreach (AttachPoint point in points)
		{
			cache[point.type] = point;
		}
		cache[AttachPointType.RootNone] = new AttachPoint
		{
			offset = Vector3.zero,
			type = AttachPointType.RootNone
		};
	}

	public void Add(AttachPointTracker other, bool overwrite = true)
	{
		foreach (KeyValuePair<AttachPointType, AttachPoint> item in other.cache)
		{
			if (overwrite || !cache.ContainsKey(item.Key))
			{
				cache[item.Key] = item.Value;
			}
		}
	}

	private void TryCacheKindDefOverrides()
	{
		if (didReadOverrides || !(parent is Pawn pawn))
		{
			return;
		}
		if (pawn.TryGetAlternate(out var ag, out var _) && ag.attachPoints != null)
		{
			foreach (AttachPoint attachPoint in ag.attachPoints)
			{
				cache[attachPoint.type] = attachPoint;
			}
		}
		GraphicData graphicData = pawn.Drawer.renderer.BodyGraphic?.data;
		if (graphicData == null)
		{
			return;
		}
		if (graphicData.attachPoints != null && graphicData.attachPoints.Count > 0)
		{
			foreach (AttachPoint attachPoint2 in graphicData.attachPoints)
			{
				cache[attachPoint2.type] = attachPoint2;
			}
		}
		didReadOverrides = true;
	}

	public Vector3 GetRotatedOffset(AttachPointType type)
	{
		return GetRotatedOffset(type, parent.Rotation);
	}

	public Vector3 GetRotatedOffset(AttachPointType type, Rot4 rot)
	{
		Vector3 offset = GetPointOrDefault(type).offset;
		if (parent is Pawn pawn)
		{
			offset *= pawn.ageTracker.CurLifeStage.attachPointScaleFactor;
		}
		return offset.RotatedBy(rot);
	}

	public Vector3 GetWorldPos(AttachPointType type)
	{
		return parent.DrawPos + GetRotatedOffset(type);
	}

	[IteratorStateMachine(typeof(_003CPointTypes_003Ed__11))]
	public IEnumerable<AttachPointType> PointTypes(int min = 0, int max = int.MaxValue)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPointTypes_003Ed__11(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__min = min,
			_003C_003E3__max = max
		};
	}

	[IteratorStateMachine(typeof(_003CPoints_003Ed__12))]
	public IEnumerable<AttachPoint> Points(int min = 0, int max = int.MaxValue)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPoints_003Ed__12(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__min = min,
			_003C_003E3__max = max
		};
	}

	public AttachPoint GetPointOrDefault(AttachPointType type)
	{
		if (TryGetPoint(type, out var pt))
		{
			return pt;
		}
		return cache[AttachPointType.RootNone];
	}

	public bool TryGetPoint(AttachPointType type, out AttachPoint pt)
	{
		return cache.TryGetValue(type, out pt);
	}
}
