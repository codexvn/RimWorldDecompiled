using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class Selector
{
	[CompilerGenerated]
	private sealed class _003CSelectableObjectsAt_003Ed__44 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 c;

		public IntVec3 _003C_003E3__c;

		private Map map;

		public Map _003C_003E3__map;

		private List<Thing> _003CthingList_003E5__2;

		private int _003Ci_003E5__3;

		private Thing _003Ct_003E5__4;

		private IEnumerator<Thing> _003C_003E7__wrap4;

		object IEnumerator<object>.Current
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
		public _003CSelectableObjectsAt_003Ed__44(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CthingList_003E5__2 = null;
			_003Ct_003E5__4 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				Plan plan;
				Zone zone;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CthingList_003E5__2 = c.GetThingList(map);
					_003Ci_003E5__3 = 0;
					goto IL_0118;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap4 = ContainingSelectionUtility.SelectableContainedThings(_003Ct_003E5__4).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00e5;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00e5;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0161;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00e5:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Thing current = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = null;
					goto IL_00ff;
					IL_00ff:
					_003Ct_003E5__4 = null;
					_003Ci_003E5__3++;
					goto IL_0118;
					IL_0161:
					plan = map.planManager.PlanAt(c);
					if (plan != null)
					{
						_003C_003E2__current = plan;
						_003C_003E1__state = 4;
						return true;
					}
					break;
					IL_0118:
					if (_003Ci_003E5__3 < _003CthingList_003E5__2.Count)
					{
						_003Ct_003E5__4 = _003CthingList_003E5__2[_003Ci_003E5__3];
						if (ThingSelectionUtility.SelectableByMapClick(_003Ct_003E5__4))
						{
							_003C_003E2__current = _003Ct_003E5__4;
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_00ff;
					}
					zone = map.zoneManager.ZoneAt(c);
					if (zone != null)
					{
						_003C_003E2__current = zone;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0161;
				}
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			_003CSelectableObjectsAt_003Ed__44 _003CSelectableObjectsAt_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSelectableObjectsAt_003Ed__ = this;
			}
			else
			{
				_003CSelectableObjectsAt_003Ed__ = new _003CSelectableObjectsAt_003Ed__44(0);
			}
			_003CSelectableObjectsAt_003Ed__.c = _003C_003E3__c;
			_003CSelectableObjectsAt_003Ed__.map = _003C_003E3__map;
			return _003CSelectableObjectsAt_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSelectableObjectsUnderMouse_003Ed__43 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Thing> _003CselectableList_003E5__2;

		private int _003Ci_003E5__3;

		object IEnumerator<object>.Current
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
		public _003CSelectableObjectsUnderMouse_003Ed__43(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CselectableList_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			Plan plan;
			Zone zone;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				Thing thing = Find.ColonistBar.ColonistOrCorpseAt(mousePositionOnUIInverted);
				if (thing != null && thing.SpawnedOrAnyParentSpawned)
				{
					_003C_003E2__current = thing;
					_003C_003E1__state = 1;
					return true;
				}
				if (!UI.MouseCell().InBounds(Find.CurrentMap))
				{
					return false;
				}
				TargetingParameters clickParams = new TargetingParameters
				{
					mustBeSelectable = true,
					canTargetPawns = true,
					canTargetBuildings = true,
					canTargetItems = true,
					mapObjectTargetsMustBeAutoAttackable = false
				};
				_003CselectableList_003E5__2 = GenUI.ThingsUnderMouse(UI.MouseMapPosition(), 1f, clickParams);
				if (_003CselectableList_003E5__2.Count > 0 && _003CselectableList_003E5__2[0] is Pawn && (_003CselectableList_003E5__2[0].DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() < 0.4f)
				{
					for (int num = _003CselectableList_003E5__2.Count - 1; num >= 0; num--)
					{
						Thing thing2 = _003CselectableList_003E5__2[num];
						if (thing2.def.category == ThingCategory.Pawn && (thing2.DrawPosHeld.Value - UI.MouseMapPosition()).MagnitudeHorizontal() > 0.4f)
						{
							_003CselectableList_003E5__2.Remove(thing2);
						}
					}
				}
				_003Ci_003E5__3 = 0;
				goto IL_01b1;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				goto IL_01b1;
			case 3:
				_003C_003E1__state = -1;
				goto IL_01f6;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_01f6:
				plan = Find.CurrentMap.planManager.PlanAt(UI.MouseCell());
				if (plan != null)
				{
					_003C_003E2__current = plan;
					_003C_003E1__state = 4;
					return true;
				}
				break;
				IL_01b1:
				if (_003Ci_003E5__3 < _003CselectableList_003E5__2.Count)
				{
					_003C_003E2__current = _003CselectableList_003E5__2[_003Ci_003E5__3];
					_003C_003E1__state = 2;
					return true;
				}
				zone = Find.CurrentMap.zoneManager.ZoneAt(UI.MouseCell());
				if (zone != null)
				{
					_003C_003E2__current = zone;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_01f6;
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
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CSelectableObjectsUnderMouse_003Ed__43(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	public DragBox dragBox = new DragBox();

	public MultiPawnGotoController gotoController = new MultiPawnGotoController();

	private List<object> selected = new List<object>();

	public const float PawnSelectRadius = 1f;

	private const int MaxNumSelected = 200;

	private static readonly List<Pawn> tmpSelectedPawns = new List<Pawn>();

	private static List<Thing> tmp_selectedBoxThings = new List<Thing>();

	private static readonly List<Pawn> tmpDraftedGotoPawns = new List<Pawn>();

	public static bool ShiftIsHeld
	{
		get
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				return Input.GetKey(KeyCode.RightShift);
			}
			return true;
		}
	}

	public List<object> SelectedObjects => selected;

	public List<object> SelectedObjectsListForReading => selected;

	public Thing SingleSelectedThing
	{
		get
		{
			if (selected.Count != 1)
			{
				return null;
			}
			if (selected[0] is Thing)
			{
				return (Thing)selected[0];
			}
			return null;
		}
	}

	public object FirstSelectedObject
	{
		get
		{
			if (selected.Count == 0)
			{
				return null;
			}
			return selected[0];
		}
	}

	public object SingleSelectedObject
	{
		get
		{
			if (selected.Count != 1)
			{
				return null;
			}
			return selected[0];
		}
	}

	public List<Pawn> SelectedPawns
	{
		get
		{
			tmpSelectedPawns.Clear();
			for (int i = 0; i < selected.Count; i++)
			{
				if (selected[i] is Pawn item)
				{
					tmpSelectedPawns.Add(item);
				}
			}
			return tmpSelectedPawns;
		}
	}

	public int NumSelected => selected.Count;

	public Zone SelectedZone
	{
		get
		{
			if (selected.Count == 0)
			{
				return null;
			}
			return selected[0] as Zone;
		}
		set
		{
			ClearSelection();
			if (value != null)
			{
				Select(value);
			}
		}
	}

	public Plan SelectedPlan
	{
		get
		{
			if (selected.Count == 0)
			{
				return null;
			}
			return selected[0] as Plan;
		}
		set
		{
			ClearSelection();
			if (value != null)
			{
				Select(value);
			}
		}
	}

	public bool AnyPawnSelected
	{
		get
		{
			for (int i = 0; i < selected.Count; i++)
			{
				if (selected[i] is Pawn)
				{
					return true;
				}
			}
			return false;
		}
	}

	public void SelectorOnGUI()
	{
		HandleMapClicks();
		if (KeyBindingDefOf.Cancel.KeyDownEvent && selected.Count > 0)
		{
			ClearSelection();
			Event.current.Use();
		}
		if (NumSelected > 0 && Find.MainTabsRoot.OpenTab == null && !WorldRendererUtility.WorldSelected)
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, playSound: false);
		}
	}

	public void SelectorOnGUI_BeforeMainTabs()
	{
		if (gotoController.Active)
		{
			gotoController.OnGUI();
		}
	}

	private void HandleMapClicks()
	{
		if (Event.current.type == EventType.mouseDown)
		{
			if (Event.current.button == 0)
			{
				if (Event.current.clickCount == 1)
				{
					dragBox.active = true;
					dragBox.start = UI.MouseMapPosition();
				}
				if (Event.current.clickCount == 2)
				{
					SelectAllMatchingObjectUnderMouseOnScreen();
				}
				Event.current.Use();
			}
			if (Event.current.button == 1 && SelectedPawns.Any())
			{
				List<FloatMenuOption> list = null;
				FloatMenuContext context = null;
				try
				{
					list = FloatMenuMakerMap.GetOptions(SelectedPawns, UI.MouseMapPosition(), out context);
				}
				catch (Exception ex)
				{
					Log.Error("Error trying to make float menu: " + ex);
				}
				if (!list.NullOrEmpty())
				{
					FloatMenuOption autoTakeOption = FloatMenuMakerMap.GetAutoTakeOption(list);
					if (context.IsMultiselect && context.ValidSelectedPawns.Count() > 1 && list.Count == 1 && list[0].isGoto)
					{
						HandleMultiselectGoto(context);
					}
					else if (autoTakeOption != null)
					{
						autoTakeOption.Chosen(colonistOrdering: true, null);
					}
					else
					{
						string title = ((!context.IsMultiselect) ? context.FirstSelectedPawn.LabelCap : null);
						FloatMenuMap floatMenuMap = new FloatMenuMap(list, title, UI.MouseMapPosition());
						floatMenuMap.givesColonistOrders = true;
						Find.WindowStack.Add(floatMenuMap);
					}
				}
				else if (context != null && context.IsMultiselect)
				{
					HandleMultiselectGoto(context);
				}
				Event.current.Use();
			}
		}
		if (Event.current.rawType == EventType.mouseUp)
		{
			if (Event.current.button == 0)
			{
				if (dragBox.active)
				{
					dragBox.active = false;
					if (!dragBox.IsValid)
					{
						SelectUnderMouse();
					}
					else
					{
						SelectInsideDragBox();
					}
				}
			}
			else if (Event.current.button == 1 && gotoController.Active)
			{
				gotoController.FinalizeInteraction();
			}
			Event.current.Use();
		}
		if (gotoController.Active && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
		{
			gotoController.FinalizeInteraction();
		}
		if (gotoController.Active)
		{
			gotoController.ProcessInputEvents();
		}
	}

	public bool IsSelected(object obj)
	{
		return selected.Contains(obj);
	}

	public void ClearSelection()
	{
		SelectionDrawer.Clear();
		selected.Clear();
		gotoController.Deactivate();
	}

	public void Deselect(object obj)
	{
		DeselectInternal(obj);
	}

	private void DeselectInternal(object obj)
	{
		if (selected.Contains(obj))
		{
			selected.Remove(obj);
		}
	}

	public void Select(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
	{
		SelectInternal(obj, playSound, forceDesignatorDeselect);
	}

	private void SelectInternal(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
	{
		if (obj == null)
		{
			Log.Error("Cannot select null.");
			return;
		}
		Thing thing = obj as Thing;
		if (thing == null && !(obj is Zone) && !(obj is Plan))
		{
			Log.Error($"Tried to select {obj} which is neither a Thing, Zone, or Plan.");
			return;
		}
		if (thing != null && thing.Destroyed)
		{
			Log.Error("Cannot select destroyed thing.");
			return;
		}
		CompSelectProxy compSelectProxy = thing.TryGetComp<CompSelectProxy>();
		if (compSelectProxy != null && compSelectProxy.thingToSelect != null)
		{
			SelectInternal(compSelectProxy.thingToSelect, playSound, forceDesignatorDeselect);
			return;
		}
		if (obj is Pawn p && p.IsWorldPawn())
		{
			Log.Error("Cannot select world pawns.");
			return;
		}
		if (forceDesignatorDeselect)
		{
			Find.DesignatorManager.Deselect();
		}
		if ((SelectedZone != null) ^ !(obj is Zone) ^ !(obj is Plan))
		{
			ClearSelection();
		}
		if (obj is Zone && SelectedZone == null)
		{
			ClearSelection();
		}
		if (obj is Plan && SelectedPlan == null)
		{
			ClearSelection();
		}
		Map map = null;
		if (thing != null)
		{
			map = thing.MapHeld;
		}
		else if (obj is Zone zone)
		{
			map = zone.Map;
		}
		else if (obj is Plan plan)
		{
			map = plan.Map;
		}
		for (int num = selected.Count - 1; num >= 0; num--)
		{
			Map map2 = null;
			if (selected[num] is Thing thing2)
			{
				map2 = thing2.MapHeld;
			}
			else if (selected[num] is Zone zone2)
			{
				map2 = zone2.Map;
			}
			else if (selected[num] is Plan plan2)
			{
				map2 = plan2.Map;
			}
			if (map2 != map)
			{
				Deselect(selected[num]);
			}
		}
		if (selected.Count < 200 && !IsSelected(obj))
		{
			if (map != Find.CurrentMap)
			{
				Current.Game.CurrentMap = map;
				SoundDefOf.MapSelected.PlayOneShotOnCamera();
				IntVec3 cell = thing?.PositionHeld ?? ((Zone)obj).Cells[0];
				Find.CameraDriver.JumpToCurrentMapLoc(cell);
			}
			if (playSound)
			{
				PlaySelectionSoundFor(obj);
			}
			selected.Add(obj);
			thing?.Notify_ThingSelected();
			SelectionDrawer.Notify_Selected(obj);
		}
	}

	public void Notify_DialogOpened()
	{
		dragBox.active = false;
		gotoController.Deactivate();
	}

	private void PlaySelectionSoundFor(object obj)
	{
		if (obj is Pawn && ((Pawn)obj).Faction == Faction.OfPlayer && ((Pawn)obj).RaceProps.Humanlike)
		{
			SoundDefOf.ColonistSelected.PlayOneShotOnCamera();
			return;
		}
		if (obj is Thing || obj is Zone || obj is Plan)
		{
			SoundDefOf.ThingSelected.PlayOneShotOnCamera();
			return;
		}
		Log.Warning($"Can't determine selection sound for {obj}");
	}

	private void SelectInsideDragBox()
	{
		if (!ShiftIsHeld)
		{
			ClearSelection();
		}
		bool selectedSomething = false;
		List<Thing> list = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(dragBox.ScreenRect);
		for (int i = 0; i < list.Count; i++)
		{
			selectedSomething = true;
			Select(list[i]);
		}
		if (selectedSomething)
		{
			return;
		}
		List<Caravan> list2 = Find.ColonistBar.CaravanMembersCaravansInScreenRect(dragBox.ScreenRect);
		for (int j = 0; j < list2.Count; j++)
		{
			if (!selectedSomething)
			{
				CameraJumper.TryJumpAndSelect(list2[j]);
				selectedSomething = true;
			}
			else
			{
				Find.WorldSelector.Select(list2[j]);
			}
		}
		if (selectedSomething)
		{
			return;
		}
		List<Thing> boxThings = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(dragBox.ScreenRect).ToList();
		if (SelectWhere(IsColonist, SelectorUtility.SortInColonistBarOrder) || SelectWhere(IsHumanlike) || SelectWhere(IsResource) || SelectWhere(IsPawn) || SelectWhere((Thing t) => t.def.selectable))
		{
			return;
		}
		foreach (Zone item in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(dragBox.ScreenRect).ToList())
		{
			selectedSomething = true;
			Select(item);
		}
		foreach (Plan item2 in ThingSelectionUtility.MultiSelectablePlansInScreenRectDistinct(dragBox.ScreenRect).ToList())
		{
			selectedSomething = true;
			Select(item2);
		}
		if (!selectedSomething)
		{
			SelectUnderMouse();
		}
		static bool IsColonist(Thing t)
		{
			if (t.def.category == ThingCategory.Pawn && (((Pawn)t).RaceProps.Humanlike || (ModsConfig.BiotechActive && ((Pawn)t).RaceProps.IsMechanoid)))
			{
				return t.Faction == Faction.OfPlayer;
			}
			return false;
		}
		static bool IsHumanlike(Thing t)
		{
			if (t.def.category == ThingCategory.Pawn)
			{
				return ((Pawn)t).RaceProps.Humanlike;
			}
			return false;
		}
		static bool IsPawn(Thing t)
		{
			return t.def.category == ThingCategory.Pawn;
		}
		static bool IsResource(Thing t)
		{
			return t.def.CountAsResource;
		}
		bool SelectWhere(Predicate<Thing> predicate, Action<List<Thing>> postProcessor = null)
		{
			tmp_selectedBoxThings.Clear();
			foreach (Thing item3 in boxThings.Where((Thing t) => predicate(t)))
			{
				tmp_selectedBoxThings.Add(item3);
			}
			if (tmp_selectedBoxThings.Any())
			{
				postProcessor?.Invoke(tmp_selectedBoxThings);
				foreach (Thing tmp_selectedBoxThing in tmp_selectedBoxThings)
				{
					Select(tmp_selectedBoxThing);
					selectedSomething = true;
				}
			}
			tmp_selectedBoxThings.Clear();
			return selectedSomething;
		}
	}

	[IteratorStateMachine(typeof(_003CSelectableObjectsUnderMouse_003Ed__43))]
	private IEnumerable<object> SelectableObjectsUnderMouse()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSelectableObjectsUnderMouse_003Ed__43(-2);
	}

	[IteratorStateMachine(typeof(_003CSelectableObjectsAt_003Ed__44))]
	public static IEnumerable<object> SelectableObjectsAt(IntVec3 c, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSelectableObjectsAt_003Ed__44(-2)
		{
			_003C_003E3__c = c,
			_003C_003E3__map = map
		};
	}

	private void SelectUnderMouse()
	{
		Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(UI.MousePositionOnUIInverted);
		if (caravan != null)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			return;
		}
		List<object> list = SelectableObjectsUnderMouse().ToList();
		list.SortBy((object x) => (!(x is Building_Storage)) ? 1 : 0);
		if (list.Count == 0)
		{
			if (!ShiftIsHeld)
			{
				ClearSelection();
			}
		}
		else if (list.Count == 1)
		{
			object obj2 = list[0];
			if (!ShiftIsHeld)
			{
				ClearSelection();
				Select(obj2);
			}
			else if (!selected.Contains(obj2))
			{
				Select(obj2);
			}
			else
			{
				Deselect(obj2);
			}
		}
		else
		{
			if (list.Count <= 1)
			{
				return;
			}
			object obj3 = list.FirstOrDefault((object obj) => selected.Contains(obj));
			if (obj3 != null)
			{
				if (ShiftIsHeld)
				{
					foreach (object item in list)
					{
						if (selected.Contains(item))
						{
							Deselect(item);
						}
					}
					return;
				}
				int num = list.IndexOf(obj3) + 1;
				if (num >= list.Count)
				{
					num -= list.Count;
				}
				if (obj3 is Thing thing && thing.def.category == ThingCategory.Item && thing.Spawned && thing.Position.GetItemCount(thing.Map) >= 2 && list.All(delegate(object x)
				{
					if (x is Thing thing2 && thing2.def.category != ThingCategory.Item)
					{
						Building obj4 = thing2 as Building;
						if (obj4 == null)
						{
							return false;
						}
						return obj4.MaxItemsInCell > 1;
					}
					return true;
				}))
				{
					if (obj3 != list[0])
					{
						ClearSelection();
						Select(list[0]);
					}
					else if (list[list.Count - 1] is Building { MaxItemsInCell: >1 })
					{
						ClearSelection();
						Select(list[list.Count - 1]);
					}
					else
					{
						ClearSelection();
						Select(list[num]);
					}
				}
				else
				{
					ClearSelection();
					Select(list[num]);
				}
			}
			else
			{
				if (!ShiftIsHeld)
				{
					ClearSelection();
				}
				Select(list[0]);
			}
		}
	}

	public void SelectNextAt(IntVec3 c, Map map)
	{
		if (SelectedObjects.Count != 1)
		{
			Log.Error("Cannot select next at with < or > 1 selected.");
			return;
		}
		List<object> list = SelectableObjectsAt(c, map).ToList();
		int num = list.IndexOf(SingleSelectedThing) + 1;
		if (num >= list.Count)
		{
			num -= list.Count;
		}
		ClearSelection();
		Select(list[num]);
	}

	private void SelectAllMatchingObjectUnderMouseOnScreen()
	{
		List<object> list = SelectableObjectsUnderMouse().ToList();
		if (list.Count == 0)
		{
			return;
		}
		Thing clickedThing = list.FirstOrDefault((object o) => o is Pawn pawn && pawn.Faction == Faction.OfPlayer && !pawn.IsPrisoner) as Thing;
		if (clickedThing == null)
		{
			clickedThing = list.FirstOrDefault((object o) => o is Pawn) as Thing;
		}
		if (clickedThing != null && !clickedThing.Spawned)
		{
			clickedThing = null;
		}
		if (clickedThing == null)
		{
			clickedThing = list.FirstOrDefault(delegate(object o)
			{
				if (!(o is Thing thing))
				{
					return false;
				}
				return !thing.def.neverMultiSelect;
			}) as Thing;
		}
		Rect rect = new Rect(0f, 0f, UI.screenWidth, UI.screenHeight);
		if (clickedThing == null)
		{
			if (!(list.FirstOrDefault((object o) => o is Zone zone && zone.IsMultiselectable) is Zone matchType))
			{
				return;
			}
			{
				foreach (Zone item in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(rect, matchType))
				{
					if (!IsSelected(item))
					{
						Select(item);
					}
				}
				return;
			}
		}
		Building edifice = clickedThing.PositionHeld.GetEdifice(clickedThing.MapHeld);
		if (edifice != null && edifice is Building_Storage building_Storage && building_Storage.GetSlotGroup().HeldThings.Contains(clickedThing))
		{
			Deselect(edifice);
		}
		foreach (Thing item2 in (IEnumerable)ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(rect))
		{
			if (Validator(item2.GetInnerIfMinified()))
			{
				Select(item2);
			}
		}
		bool Validator(Thing t)
		{
			Thing innerIfMinified = clickedThing.GetInnerIfMinified();
			if (IsSelected(t) || t.Faction != innerIfMinified.Faction)
			{
				return false;
			}
			if (innerIfMinified is Pawn pawn2 && t is Pawn pawn3)
			{
				if (pawn3.HostFaction != pawn2.HostFaction)
				{
					return false;
				}
				if (pawn3.mutant?.Def != pawn2.mutant?.Def)
				{
					return false;
				}
				if (!SelectorUtility.IsEquivalentRace(pawn3, pawn2))
				{
					return false;
				}
				return true;
			}
			return t.def == innerIfMinified.def;
		}
	}

	private void HandleMultiselectGoto(FloatMenuContext context)
	{
		tmpDraftedGotoPawns.Clear();
		foreach (Pawn allSelectedPawn in context.allSelectedPawns)
		{
			if ((bool)FloatMenuMakerMap.ShouldGenerateFloatMenuForPawn(allSelectedPawn) && allSelectedPawn.Drafted)
			{
				tmpDraftedGotoPawns.Add(allSelectedPawn);
			}
		}
		if (tmpDraftedGotoPawns.Count == 1)
		{
			if ((bool)FloatMenuOptionProvider_DraftedMove.PawnCanGoto(tmpDraftedGotoPawns[0], context.ClickedCell))
			{
				FloatMenuOptionProvider_DraftedMove.PawnGotoAction(context.ClickedCell, tmpDraftedGotoPawns[0], context.ClickedCell);
			}
		}
		else
		{
			IntVec3 mouseCell = CellFinder.StandableCellNear(context.ClickedCell, context.map, 2.9f);
			if (mouseCell.IsValid)
			{
				gotoController.StartInteraction(mouseCell);
				foreach (Pawn tmpDraftedGotoPawn in tmpDraftedGotoPawns)
				{
					gotoController.AddPawn(tmpDraftedGotoPawn);
				}
			}
		}
		tmpDraftedGotoPawns.Clear();
	}
}
