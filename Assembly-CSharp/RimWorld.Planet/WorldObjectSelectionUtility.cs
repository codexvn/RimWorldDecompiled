using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public static class WorldObjectSelectionUtility
{
	[CompilerGenerated]
	private sealed class _003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__0 : IEnumerable<WorldObject>, IEnumerable, IEnumerator<WorldObject>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private WorldObject _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rect rect;

		public Rect _003C_003E3__rect;

		private List<WorldObject> _003CallObjects_003E5__2;

		private int _003Ci_003E5__3;

		WorldObject IEnumerator<WorldObject>.Current
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
		public _003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CallObjects_003E5__2 = null;
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
				_003CallObjects_003E5__2 = Find.WorldObjects.AllWorldObjects;
				_003Ci_003E5__3 = 0;
				goto IL_0137;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0127;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0127;
				}
				IL_0137:
				if (_003Ci_003E5__3 < _003CallObjects_003E5__2.Count)
				{
					if (!_003CallObjects_003E5__2[_003Ci_003E5__3].NeverMultiSelect && !_003CallObjects_003E5__2[_003Ci_003E5__3].HiddenBehindTerrainNow())
					{
						if (ExpandableWorldObjectsUtility.IsExpanded(_003CallObjects_003E5__2[_003Ci_003E5__3]))
						{
							if (rect.Overlaps(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(_003CallObjects_003E5__2[_003Ci_003E5__3])))
							{
								_003C_003E2__current = _003CallObjects_003E5__2[_003Ci_003E5__3];
								_003C_003E1__state = 1;
								return true;
							}
						}
						else if (rect.Contains(_003CallObjects_003E5__2[_003Ci_003E5__3].ScreenPos()))
						{
							_003C_003E2__current = _003CallObjects_003E5__2[_003Ci_003E5__3];
							_003C_003E1__state = 2;
							return true;
						}
					}
					goto IL_0127;
				}
				return false;
				IL_0127:
				_003Ci_003E5__3++;
				goto IL_0137;
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
		IEnumerator<WorldObject> IEnumerable<WorldObject>.GetEnumerator()
		{
			_003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__0 _003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__ = this;
			}
			else
			{
				_003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__ = new _003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__0(0);
			}
			_003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__.rect = _003C_003E3__rect;
			return _003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<WorldObject>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__0))]
	public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMultiSelectableWorldObjectsInScreenRectDistinct_003Ed__0(-2)
		{
			_003C_003E3__rect = rect
		};
	}

	public static bool HiddenBehindTerrainNow(this WorldObject o)
	{
		if (!WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos))
		{
			if (!o.VisibleInBackground)
			{
				return o.Tile.Layer != PlanetLayer.Selected;
			}
			return false;
		}
		return true;
	}

	public static Vector2 ScreenPos(this WorldObject o)
	{
		return GenWorldUI.WorldToUIPosition(o.DrawPos);
	}

	public static bool VisibleToCameraNow(this WorldObject o)
	{
		if (!WorldRendererUtility.WorldSelected)
		{
			return false;
		}
		if (o.HiddenBehindTerrainNow())
		{
			return false;
		}
		Vector2 point = o.ScreenPos();
		return new Rect(0f, 0f, UI.screenWidth, UI.screenHeight).Contains(point);
	}

	public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
	{
		Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
		int worldLayerMask = WorldCameraManager.WorldLayerMask;
		if (Physics.Raycast(ray, out var hitInfo, 1500f, worldLayerMask))
		{
			return Vector3.Distance(hitInfo.point, o.DrawPos);
		}
		return Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude;
	}
}
