using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class ThingSelectionUtility
{
	[CompilerGenerated]
	private sealed class _003CMultiSelectablePlansInScreenRectDistinct_003Ed__6 : IEnumerable<Plan>, IEnumerable, IEnumerator<Plan>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Plan _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rect rect;

		public Rect _003C_003E3__rect;

		private ColorDef match;

		public ColorDef _003C_003E3__match;

		private CellRect.Enumerator _003C_003E7__wrap1;

		private Plan _003Cplan_003E5__3;

		Plan IEnumerator<Plan>.Current
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
		public _003CMultiSelectablePlansInScreenRectDistinct_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cplan_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					yieldedPlans.Add(_003Cplan_003E5__3);
					goto IL_00e6;
				}
				_003C_003E1__state = -1;
				CellRect mapRect = GetMapRect(rect);
				yieldedPlans.Clear();
				_003C_003E1__state = -3;
				_003C_003E7__wrap1 = mapRect.GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_00ed;
				IL_00e6:
				_003Cplan_003E5__3 = null;
				goto IL_00ed;
				IL_00ed:
				while (true)
				{
					if (_003C_003E7__wrap1.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap1.Current;
						if (current.InBounds(Find.CurrentMap))
						{
							_003Cplan_003E5__3 = current.GetPlan(Find.CurrentMap);
							if (_003Cplan_003E5__3 != null && (match == null || _003Cplan_003E5__3.Color == match))
							{
								break;
							}
						}
						continue;
					}
					_003C_003Em__Finally2();
					_003C_003Em__Finally1();
					return false;
				}
				if (!yieldedPlans.Contains(_003Cplan_003E5__3))
				{
					_003C_003E2__current = _003Cplan_003E5__3;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00e6;
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
			yieldedPlans.Clear();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Plan> IEnumerable<Plan>.GetEnumerator()
		{
			_003CMultiSelectablePlansInScreenRectDistinct_003Ed__6 _003CMultiSelectablePlansInScreenRectDistinct_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMultiSelectablePlansInScreenRectDistinct_003Ed__ = this;
			}
			else
			{
				_003CMultiSelectablePlansInScreenRectDistinct_003Ed__ = new _003CMultiSelectablePlansInScreenRectDistinct_003Ed__6(0);
			}
			_003CMultiSelectablePlansInScreenRectDistinct_003Ed__.rect = _003C_003E3__rect;
			_003CMultiSelectablePlansInScreenRectDistinct_003Ed__.match = _003C_003E3__match;
			return _003CMultiSelectablePlansInScreenRectDistinct_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Plan>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMultiSelectableThingsInScreenRectDistinct_003Ed__3 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rect rect;

		public Rect _003C_003E3__rect;

		private CellRect _003CmapRect_003E5__2;

		private Rect _003CrectInWorldSpace_003E5__3;

		private CellRect.Enumerator _003C_003E7__wrap3;

		private List<Thing> _003CcellThings_003E5__5;

		private int _003Ck_003E5__6;

		private Thing _003Ct_003E5__7;

		private IEnumerator<IntVec3> _003C_003E7__wrap7;

		private IEnumerator<Thing> _003C_003E7__wrap8;

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
		public _003CMultiSelectableThingsInScreenRectDistinct_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -6) <= 3u || (uint)(num - 1) <= 1u)
			{
				try
				{
					switch (num)
					{
					case -4:
					case 1:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
						break;
					case -6:
					case -5:
					case 2:
						try
						{
							if (num == -6 || num == 2)
							{
								try
								{
								}
								finally
								{
									_003C_003Em__Finally4();
								}
							}
						}
						finally
						{
							_003C_003Em__Finally3();
						}
						break;
					case -3:
					case -2:
					case -1:
					case 0:
						break;
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CcellThings_003E5__5 = null;
			_003Ct_003E5__7 = null;
			_003C_003E7__wrap7 = null;
			_003C_003E7__wrap8 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				IntVec3 current;
				IntVec3 current2;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CmapRect_003E5__2 = GetMapRect(rect);
					yieldedThings.Clear();
					_003C_003E1__state = -3;
					_003C_003E7__wrap3 = _003CmapRect_003E5__2.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0160;
				case 1:
					_003C_003E1__state = -4;
					yieldedThings.Add(_003Ct_003E5__7);
					goto IL_012c;
				case 2:
					{
						_003C_003E1__state = -6;
						yieldedThings.Add(_003Ct_003E5__7);
						goto IL_02e2;
					}
					IL_0160:
					while (_003C_003E7__wrap3.MoveNext())
					{
						current = _003C_003E7__wrap3.Current;
						if (!current.InBounds(Find.CurrentMap))
						{
							continue;
						}
						goto IL_0084;
					}
					_003C_003Em__Finally2();
					_003CrectInWorldSpace_003E5__3 = GetRectInWorldSpace(rect);
					_003C_003E7__wrap7 = _003CmapRect_003E5__2.ExpandedBy(1).EdgeCells.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0306;
					IL_0306:
					do
					{
						if (_003C_003E7__wrap7.MoveNext())
						{
							current2 = _003C_003E7__wrap7.Current;
							continue;
						}
						_003C_003Em__Finally3();
						_003C_003E7__wrap7 = null;
						_003C_003Em__Finally1();
						return false;
					}
					while (!current2.InBounds(Find.CurrentMap) || current2.GetItemCount(Find.CurrentMap) <= 1);
					_003C_003E7__wrap8 = Find.CurrentMap.thingGrid.ThingsAt(current2).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_02e9;
					IL_0159:
					_003CcellThings_003E5__5 = null;
					goto IL_0160;
					IL_02e9:
					if (_003C_003E7__wrap8.MoveNext())
					{
						_003Ct_003E5__7 = _003C_003E7__wrap8.Current;
						if (_003Ct_003E5__7.def.category == ThingCategory.Item && SelectableByMapClick(_003Ct_003E5__7) && !_003Ct_003E5__7.def.neverMultiSelect && !yieldedThings.Contains(_003Ct_003E5__7))
						{
							Vector3 vector = _003Ct_003E5__7.TrueCenter();
							if (new Rect(vector.x - 0.5f, vector.z - 0.5f, 1f, 1f).Overlaps(_003CrectInWorldSpace_003E5__3))
							{
								_003C_003E2__current = _003Ct_003E5__7;
								_003C_003E1__state = 2;
								return true;
							}
						}
						goto IL_02e2;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap8 = null;
					goto IL_0306;
					IL_02e2:
					_003Ct_003E5__7 = null;
					goto IL_02e9;
					IL_0084:
					_003CcellThings_003E5__5 = Find.CurrentMap.thingGrid.ThingsListAt(current);
					if (_003CcellThings_003E5__5 != null)
					{
						_003Ck_003E5__6 = 0;
						goto IL_0143;
					}
					goto IL_0159;
					IL_012c:
					_003Ct_003E5__7 = null;
					_003Ck_003E5__6++;
					goto IL_0143;
					IL_0143:
					if (_003Ck_003E5__6 < _003CcellThings_003E5__5.Count)
					{
						_003Ct_003E5__7 = _003CcellThings_003E5__5[_003Ck_003E5__6];
						if (SelectableByMapClick(_003Ct_003E5__7) && !_003Ct_003E5__7.def.neverMultiSelect && !yieldedThings.Contains(_003Ct_003E5__7))
						{
							_003C_003E2__current = _003Ct_003E5__7;
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_012c;
					}
					goto IL_0159;
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
			yieldedThings.Clear();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap7 != null)
			{
				_003C_003E7__wrap7.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -5;
			if (_003C_003E7__wrap8 != null)
			{
				_003C_003E7__wrap8.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CMultiSelectableThingsInScreenRectDistinct_003Ed__3 _003CMultiSelectableThingsInScreenRectDistinct_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMultiSelectableThingsInScreenRectDistinct_003Ed__ = this;
			}
			else
			{
				_003CMultiSelectableThingsInScreenRectDistinct_003Ed__ = new _003CMultiSelectableThingsInScreenRectDistinct_003Ed__3(0);
			}
			_003CMultiSelectableThingsInScreenRectDistinct_003Ed__.rect = _003C_003E3__rect;
			return _003CMultiSelectableThingsInScreenRectDistinct_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMultiSelectableZonesInScreenRectDistinct_003Ed__8 : IEnumerable<Zone>, IEnumerable, IEnumerator<Zone>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Zone _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rect rect;

		public Rect _003C_003E3__rect;

		private Zone matchType;

		public Zone _003C_003E3__matchType;

		private CellRect.Enumerator _003C_003E7__wrap1;

		private Zone _003Czone_003E5__3;

		Zone IEnumerator<Zone>.Current
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
		public _003CMultiSelectableZonesInScreenRectDistinct_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Czone_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					yieldedZones.Add(_003Czone_003E5__3);
					goto IL_0100;
				}
				_003C_003E1__state = -1;
				CellRect mapRect = GetMapRect(rect);
				yieldedZones.Clear();
				_003C_003E1__state = -3;
				_003C_003E7__wrap1 = mapRect.GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_0107;
				IL_0100:
				_003Czone_003E5__3 = null;
				goto IL_0107;
				IL_0107:
				while (true)
				{
					if (_003C_003E7__wrap1.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap1.Current;
						if (current.InBounds(Find.CurrentMap))
						{
							_003Czone_003E5__3 = current.GetZone(Find.CurrentMap);
							if (_003Czone_003E5__3 != null && _003Czone_003E5__3.IsMultiselectable && (matchType == null || !(_003Czone_003E5__3.GetType() != matchType.GetType())))
							{
								break;
							}
						}
						continue;
					}
					_003C_003Em__Finally2();
					_003C_003Em__Finally1();
					return false;
				}
				if (!yieldedZones.Contains(_003Czone_003E5__3))
				{
					_003C_003E2__current = _003Czone_003E5__3;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0100;
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
			yieldedZones.Clear();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Zone> IEnumerable<Zone>.GetEnumerator()
		{
			_003CMultiSelectableZonesInScreenRectDistinct_003Ed__8 _003CMultiSelectableZonesInScreenRectDistinct_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMultiSelectableZonesInScreenRectDistinct_003Ed__ = this;
			}
			else
			{
				_003CMultiSelectableZonesInScreenRectDistinct_003Ed__ = new _003CMultiSelectableZonesInScreenRectDistinct_003Ed__8(0);
			}
			_003CMultiSelectableZonesInScreenRectDistinct_003Ed__.rect = _003C_003E3__rect;
			_003CMultiSelectableZonesInScreenRectDistinct_003Ed__.matchType = _003C_003E3__matchType;
			return _003CMultiSelectableZonesInScreenRectDistinct_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Zone>)this).GetEnumerator();
		}
	}

	private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

	private static readonly HashSet<Plan> yieldedPlans = new HashSet<Plan>();

	private static readonly HashSet<Zone> yieldedZones = new HashSet<Zone>();

	private static readonly List<Pawn> tmpColonists = new List<Pawn>();

	public static bool SelectableByMapClick(Thing t)
	{
		if (!t.def.selectable)
		{
			return false;
		}
		if (t is Pawn pawn && pawn.IsHiddenFromPlayer())
		{
			return false;
		}
		Thing spawnedParentOrMe = t.SpawnedParentOrMe;
		if (spawnedParentOrMe == null)
		{
			return false;
		}
		if (spawnedParentOrMe.def.size.x == 1 && spawnedParentOrMe.def.size.z == 1)
		{
			return !spawnedParentOrMe.Position.Fogged(spawnedParentOrMe.Map);
		}
		foreach (IntVec3 item in spawnedParentOrMe.OccupiedRect())
		{
			if (!item.Fogged(spawnedParentOrMe.Map))
			{
				return true;
			}
		}
		return false;
	}

	public static bool SelectableByHotkey(Thing t)
	{
		if (t.def.selectable)
		{
			return t.Spawned;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMultiSelectableThingsInScreenRectDistinct_003Ed__3))]
	public static IEnumerable<Thing> MultiSelectableThingsInScreenRectDistinct(Rect rect)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMultiSelectableThingsInScreenRectDistinct_003Ed__3(-2)
		{
			_003C_003E3__rect = rect
		};
	}

	private static Rect GetRectInWorldSpace(Rect rect)
	{
		Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
		Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
		Vector3 vector = UI.UIToMapPosition(screenLoc);
		Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
		return new Rect(vector.x, vector2.z, vector2.x - vector.x, vector.z - vector2.z);
	}

	[IteratorStateMachine(typeof(_003CMultiSelectablePlansInScreenRectDistinct_003Ed__6))]
	public static IEnumerable<Plan> MultiSelectablePlansInScreenRectDistinct(Rect rect, ColorDef match = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMultiSelectablePlansInScreenRectDistinct_003Ed__6(-2)
		{
			_003C_003E3__rect = rect,
			_003C_003E3__match = match
		};
	}

	[IteratorStateMachine(typeof(_003CMultiSelectableZonesInScreenRectDistinct_003Ed__8))]
	public static IEnumerable<Zone> MultiSelectableZonesInScreenRectDistinct(Rect rect, Zone matchType = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMultiSelectableZonesInScreenRectDistinct_003Ed__8(-2)
		{
			_003C_003E3__rect = rect,
			_003C_003E3__matchType = matchType
		};
	}

	private static CellRect GetMapRect(Rect rect)
	{
		Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
		Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
		Vector3 vector = UI.UIToMapPosition(screenLoc);
		Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
		CellRect result = default(CellRect);
		result.minX = Mathf.FloorToInt(vector.x);
		result.minZ = Mathf.FloorToInt(vector2.z);
		result.maxX = Mathf.FloorToInt(vector2.x);
		result.maxZ = Mathf.FloorToInt(vector.z);
		return result;
	}

	public static void SelectNextColonist()
	{
		tmpColonists.Clear();
		tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(SelectableByHotkey));
		if (tmpColonists.Count == 0)
		{
			return;
		}
		bool worldSelected = WorldRendererUtility.WorldSelected;
		int num = -1;
		for (int num2 = tmpColonists.Count - 1; num2 >= 0; num2--)
		{
			if ((!worldSelected && Find.Selector.IsSelected(tmpColonists[num2])) || (worldSelected && tmpColonists[num2].IsCaravanMember() && Find.WorldSelector.IsSelected(tmpColonists[num2].GetCaravan())))
			{
				num = num2;
				break;
			}
		}
		if (num == -1)
		{
			CameraJumper.TryJumpAndSelect(tmpColonists[0]);
		}
		else
		{
			CameraJumper.TryJumpAndSelect(tmpColonists[(num + 1) % tmpColonists.Count]);
		}
		tmpColonists.Clear();
	}

	public static void SelectPreviousColonist()
	{
		tmpColonists.Clear();
		tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(SelectableByHotkey));
		if (tmpColonists.Count == 0)
		{
			return;
		}
		bool worldSelected = WorldRendererUtility.WorldSelected;
		int num = -1;
		for (int i = 0; i < tmpColonists.Count; i++)
		{
			if ((!worldSelected && Find.Selector.IsSelected(tmpColonists[i])) || (worldSelected && tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(tmpColonists[i].GetCaravan())))
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			CameraJumper.TryJumpAndSelect(tmpColonists[tmpColonists.Count - 1]);
		}
		else
		{
			CameraJumper.TryJumpAndSelect(tmpColonists[GenMath.PositiveMod(num - 1, tmpColonists.Count)]);
		}
		tmpColonists.Clear();
	}
}
