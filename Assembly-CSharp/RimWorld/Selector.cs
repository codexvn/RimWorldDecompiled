using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020033E2 RID: 13282
	public class Selector
	{
		// Token: 0x170030DB RID: 12507
		// (get) Token: 0x06012AB2 RID: 76466 RVA: 0x005583D3 File Offset: 0x005565D3
		public static bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x170030DC RID: 12508
		// (get) Token: 0x06012AB3 RID: 76467 RVA: 0x00586837 File Offset: 0x00584A37
		public List<object> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x170030DD RID: 12509
		// (get) Token: 0x06012AB4 RID: 76468 RVA: 0x00586837 File Offset: 0x00584A37
		public List<object> SelectedObjectsListForReading
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x170030DE RID: 12510
		// (get) Token: 0x06012AB5 RID: 76469 RVA: 0x0058683F File Offset: 0x00584A3F
		public Thing SingleSelectedThing
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				if (this.selected[0] is Thing)
				{
					return (Thing)this.selected[0];
				}
				return null;
			}
		}

		// Token: 0x170030DF RID: 12511
		// (get) Token: 0x06012AB6 RID: 76470 RVA: 0x00586877 File Offset: 0x00584A77
		public object FirstSelectedObject
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		// Token: 0x170030E0 RID: 12512
		// (get) Token: 0x06012AB7 RID: 76471 RVA: 0x00586894 File Offset: 0x00584A94
		public object SingleSelectedObject
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		// Token: 0x170030E1 RID: 12513
		// (get) Token: 0x06012AB8 RID: 76472 RVA: 0x005868B4 File Offset: 0x00584AB4
		public List<Pawn> SelectedPawns
		{
			get
			{
				Selector.tmpSelectedPawns.Clear();
				for (int i = 0; i < this.selected.Count; i++)
				{
					Pawn pawn = this.selected[i] as Pawn;
					if (pawn != null)
					{
						Selector.tmpSelectedPawns.Add(pawn);
					}
				}
				return Selector.tmpSelectedPawns;
			}
		}

		// Token: 0x170030E2 RID: 12514
		// (get) Token: 0x06012AB9 RID: 76473 RVA: 0x00586906 File Offset: 0x00584B06
		public int NumSelected
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x170030E3 RID: 12515
		// (get) Token: 0x06012ABA RID: 76474 RVA: 0x00586913 File Offset: 0x00584B13
		// (set) Token: 0x06012ABB RID: 76475 RVA: 0x00586935 File Offset: 0x00584B35
		public Zone SelectedZone
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0] as Zone;
			}
			set
			{
				this.ClearSelection();
				if (value != null)
				{
					this.Select(value, true, true);
				}
			}
		}

		// Token: 0x170030E4 RID: 12516
		// (get) Token: 0x06012ABC RID: 76476 RVA: 0x00586949 File Offset: 0x00584B49
		// (set) Token: 0x06012ABD RID: 76477 RVA: 0x00586935 File Offset: 0x00584B35
		public Plan SelectedPlan
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0] as Plan;
			}
			set
			{
				this.ClearSelection();
				if (value != null)
				{
					this.Select(value, true, true);
				}
			}
		}

		// Token: 0x170030E5 RID: 12517
		// (get) Token: 0x06012ABE RID: 76478 RVA: 0x0058696C File Offset: 0x00584B6C
		public bool AnyPawnSelected
		{
			get
			{
				for (int i = 0; i < this.selected.Count; i++)
				{
					if (this.selected[i] is Pawn)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06012ABF RID: 76479 RVA: 0x005869A8 File Offset: 0x00584BA8
		public void SelectorOnGUI()
		{
			this.HandleMapClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
			if (this.NumSelected > 0 && Find.MainTabsRoot.OpenTab == null && !WorldRendererUtility.WorldSelected)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, false);
			}
		}

		// Token: 0x06012AC0 RID: 76480 RVA: 0x00586A11 File Offset: 0x00584C11
		public void SelectorOnGUI_BeforeMainTabs()
		{
			if (this.gotoController.Active)
			{
				this.gotoController.OnGUI();
			}
		}

		// Token: 0x06012AC1 RID: 76481 RVA: 0x00586A2C File Offset: 0x00584C2C
		private void HandleMapClicks()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MouseMapPosition();
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.SelectedPawns.Any<Pawn>())
				{
					List<FloatMenuOption> list = null;
					FloatMenuContext floatMenuContext = null;
					try
					{
						list = FloatMenuMakerMap.GetOptions(this.SelectedPawns, UI.MouseMapPosition(), out floatMenuContext);
					}
					catch (Exception ex)
					{
						string str = "Error trying to make float menu: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					if (!list.NullOrEmpty<FloatMenuOption>())
					{
						FloatMenuOption autoTakeOption = FloatMenuMakerMap.GetAutoTakeOption(list);
						if (floatMenuContext.IsMultiselect && floatMenuContext.ValidSelectedPawns.Count<Pawn>() > 1 && list.Count == 1 && list[0].isGoto)
						{
							this.HandleMultiselectGoto(floatMenuContext);
						}
						else if (autoTakeOption != null)
						{
							autoTakeOption.Chosen(true, null);
						}
						else
						{
							string title = (!floatMenuContext.IsMultiselect) ? floatMenuContext.FirstSelectedPawn.LabelCap : null;
							FloatMenuMap floatMenuMap = new FloatMenuMap(list, title, UI.MouseMapPosition());
							floatMenuMap.givesColonistOrders = true;
							Find.WindowStack.Add(floatMenuMap);
						}
					}
					else if (floatMenuContext != null && floatMenuContext.IsMultiselect)
					{
						this.HandleMultiselectGoto(floatMenuContext);
					}
					Event.current.Use();
				}
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				if (Event.current.button == 0)
				{
					if (this.dragBox.active)
					{
						this.dragBox.active = false;
						if (!this.dragBox.IsValid)
						{
							this.SelectUnderMouse();
						}
						else
						{
							this.SelectInsideDragBox();
						}
					}
				}
				else if (Event.current.button == 1 && this.gotoController.Active)
				{
					this.gotoController.FinalizeInteraction();
				}
				Event.current.Use();
			}
			if (this.gotoController.Active && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
			{
				this.gotoController.FinalizeInteraction();
			}
			if (this.gotoController.Active)
			{
				this.gotoController.ProcessInputEvents();
			}
		}

		// Token: 0x06012AC2 RID: 76482 RVA: 0x00586C70 File Offset: 0x00584E70
		public bool IsSelected(object obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x06012AC3 RID: 76483 RVA: 0x00586C7E File Offset: 0x00584E7E
		public void ClearSelection()
		{
			SelectionDrawer.Clear();
			this.selected.Clear();
			this.gotoController.Deactivate();
		}

		// Token: 0x06012AC4 RID: 76484 RVA: 0x00586C9B File Offset: 0x00584E9B
		public void Deselect(object obj)
		{
			this.DeselectInternal(obj);
		}

		// Token: 0x06012AC5 RID: 76485 RVA: 0x00586CA4 File Offset: 0x00584EA4
		private void DeselectInternal(object obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x06012AC6 RID: 76486 RVA: 0x00586CC1 File Offset: 0x00584EC1
		public void Select(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
		{
			this.SelectInternal(obj, playSound, forceDesignatorDeselect);
		}

		// Token: 0x06012AC7 RID: 76487 RVA: 0x00586CCC File Offset: 0x00584ECC
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
				Log.Error(string.Format("Tried to select {0} which is neither a Thing, Zone, or Plan.", obj));
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
				this.SelectInternal(compSelectProxy.thingToSelect, playSound, forceDesignatorDeselect);
				return;
			}
			Pawn pawn = obj as Pawn;
			if (pawn != null && pawn.IsWorldPawn())
			{
				Log.Error("Cannot select world pawns.");
				return;
			}
			if (forceDesignatorDeselect)
			{
				Find.DesignatorManager.Deselect();
			}
			if (this.SelectedZone != null ^ !(obj is Zone) ^ !(obj is Plan))
			{
				this.ClearSelection();
			}
			if (obj is Zone && this.SelectedZone == null)
			{
				this.ClearSelection();
			}
			if (obj is Plan && this.SelectedPlan == null)
			{
				this.ClearSelection();
			}
			Map map = null;
			if (thing != null)
			{
				map = thing.MapHeld;
			}
			else
			{
				Zone zone = obj as Zone;
				if (zone != null)
				{
					map = zone.Map;
				}
				else
				{
					Plan plan = obj as Plan;
					if (plan != null)
					{
						map = plan.Map;
					}
				}
			}
			for (int i = this.selected.Count - 1; i >= 0; i--)
			{
				Map map2 = null;
				Thing thing2 = this.selected[i] as Thing;
				if (thing2 != null)
				{
					map2 = thing2.MapHeld;
				}
				else
				{
					Zone zone2 = this.selected[i] as Zone;
					if (zone2 != null)
					{
						map2 = zone2.Map;
					}
					else
					{
						Plan plan2 = this.selected[i] as Plan;
						if (plan2 != null)
						{
							map2 = plan2.Map;
						}
					}
				}
				if (map2 != map)
				{
					this.Deselect(this.selected[i]);
				}
			}
			if (this.selected.Count >= 200)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (map != Find.CurrentMap)
				{
					Current.Game.CurrentMap = map;
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					IntVec3 cell = (thing != null) ? thing.PositionHeld : ((Zone)obj).Cells[0];
					Find.CameraDriver.JumpToCurrentMapLoc(cell);
				}
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				if (thing != null)
				{
					thing.Notify_ThingSelected();
				}
				SelectionDrawer.Notify_Selected(obj);
			}
		}

		// Token: 0x06012AC8 RID: 76488 RVA: 0x00586F2F File Offset: 0x0058512F
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
			this.gotoController.Deactivate();
		}

		// Token: 0x06012AC9 RID: 76489 RVA: 0x00586F48 File Offset: 0x00585148
		private void PlaySelectionSoundFor(object obj)
		{
			if (obj is Pawn && ((Pawn)obj).Faction == Faction.OfPlayer && ((Pawn)obj).RaceProps.Humanlike)
			{
				SoundDefOf.ColonistSelected.PlayOneShotOnCamera(null);
				return;
			}
			if (obj is Thing || obj is Zone || obj is Plan)
			{
				SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
				return;
			}
			Log.Warning(string.Format("Can't determine selection sound for {0}", obj));
		}

		// Token: 0x06012ACA RID: 76490 RVA: 0x00586FC4 File Offset: 0x005851C4
		private void SelectInsideDragBox()
		{
			Selector.<>c__DisplayClass42_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			if (!Selector.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			CS$<>8__locals1.selectedSomething = false;
			List<Thing> list = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
			for (int i = 0; i < list.Count; i++)
			{
				CS$<>8__locals1.selectedSomething = true;
				this.Select(list[i], true, true);
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			List<Caravan> list2 = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
			for (int j = 0; j < list2.Count; j++)
			{
				if (!CS$<>8__locals1.selectedSomething)
				{
					CameraJumper.TryJumpAndSelect(list2[j], CameraJumper.MovementMode.Pan);
					CS$<>8__locals1.selectedSomething = true;
				}
				else
				{
					Find.WorldSelector.Select(list2[j], true);
				}
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			CS$<>8__locals1.boxThings = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<Thing>();
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0(new Predicate<Thing>(Selector.<SelectInsideDragBox>g__IsColonist|42_1), new Action<List<Thing>>(SelectorUtility.SortInColonistBarOrder), ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0(new Predicate<Thing>(Selector.<SelectInsideDragBox>g__IsHumanlike|42_2), null, ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0(new Predicate<Thing>(Selector.<SelectInsideDragBox>g__IsResource|42_3), null, ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0(new Predicate<Thing>(Selector.<SelectInsideDragBox>g__IsPawn|42_4), null, ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0((Thing t) => t.def.selectable, null, ref CS$<>8__locals1))
			{
				return;
			}
			foreach (Zone obj in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(this.dragBox.ScreenRect, null).ToList<Zone>())
			{
				CS$<>8__locals1.selectedSomething = true;
				this.Select(obj, true, true);
			}
			foreach (Plan obj2 in ThingSelectionUtility.MultiSelectablePlansInScreenRectDistinct(this.dragBox.ScreenRect, null).ToList<Plan>())
			{
				CS$<>8__locals1.selectedSomething = true;
				this.Select(obj2, true, true);
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			this.SelectUnderMouse();
		}

		// Token: 0x06012ACB RID: 76491 RVA: 0x00587224 File Offset: 0x00585424
		private IEnumerable<object> SelectableObjectsUnderMouse()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			Thing thing = Find.ColonistBar.ColonistOrCorpseAt(mousePositionOnUIInverted);
			if (thing != null && thing.SpawnedOrAnyParentSpawned)
			{
				yield return thing;
				yield break;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap))
			{
				yield break;
			}
			TargetingParameters clickParams = new TargetingParameters
			{
				mustBeSelectable = true,
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = false
			};
			List<Thing> selectableList = GenUI.ThingsUnderMouse(UI.MouseMapPosition(), 1f, clickParams, null);
			if (selectableList.Count > 0 && selectableList[0] is Pawn && (selectableList[0].DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() < 0.4f)
			{
				for (int j = selectableList.Count - 1; j >= 0; j--)
				{
					Thing thing2 = selectableList[j];
					if (thing2.def.category == ThingCategory.Pawn && (thing2.DrawPosHeld.Value - UI.MouseMapPosition()).MagnitudeHorizontal() > 0.4f)
					{
						selectableList.Remove(thing2);
					}
				}
			}
			int num;
			for (int i = 0; i < selectableList.Count; i = num + 1)
			{
				yield return selectableList[i];
				num = i;
			}
			Zone zone = Find.CurrentMap.zoneManager.ZoneAt(UI.MouseCell());
			if (zone != null)
			{
				yield return zone;
			}
			Plan plan = Find.CurrentMap.planManager.PlanAt(UI.MouseCell());
			if (plan != null)
			{
				yield return plan;
			}
			yield break;
		}

		// Token: 0x06012ACC RID: 76492 RVA: 0x0058722D File Offset: 0x0058542D
		public static IEnumerable<object> SelectableObjectsAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				Thing t = thingList[i];
				if (ThingSelectionUtility.SelectableByMapClick(t))
				{
					yield return t;
					foreach (Thing thing in ContainingSelectionUtility.SelectableContainedThings(t))
					{
						yield return thing;
					}
					IEnumerator<Thing> enumerator = null;
				}
				t = null;
				num = i;
			}
			Zone zone = map.zoneManager.ZoneAt(c);
			if (zone != null)
			{
				yield return zone;
			}
			Plan plan = map.planManager.PlanAt(c);
			if (plan != null)
			{
				yield return plan;
			}
			yield break;
			yield break;
		}

		// Token: 0x06012ACD RID: 76493 RVA: 0x00587244 File Offset: 0x00585444
		private void SelectUnderMouse()
		{
			Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(UI.MousePositionOnUIInverted);
			if (caravan != null)
			{
				CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
				return;
			}
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			list.SortBy(delegate(object x)
			{
				if (!(x is Building_Storage))
				{
					return 1;
				}
				return 0;
			});
			if (list.Count == 0)
			{
				if (!Selector.ShiftIsHeld)
				{
					this.ClearSelection();
					return;
				}
			}
			else if (list.Count == 1)
			{
				object obj4 = list[0];
				if (!Selector.ShiftIsHeld)
				{
					this.ClearSelection();
					this.Select(obj4, true, true);
					return;
				}
				if (!this.selected.Contains(obj4))
				{
					this.Select(obj4, true, true);
					return;
				}
				this.Deselect(obj4);
				return;
			}
			else if (list.Count > 1)
			{
				object obj2 = list.FirstOrDefault((object obj) => this.selected.Contains(obj));
				if (obj2 != null)
				{
					if (!Selector.ShiftIsHeld)
					{
						int num = list.IndexOf(obj2) + 1;
						if (num >= list.Count)
						{
							num -= list.Count;
						}
						Thing thing = obj2 as Thing;
						if (thing != null && thing.def.category == ThingCategory.Item && thing.Spawned && thing.Position.GetItemCount(thing.Map) >= 2)
						{
							if (list.All(delegate(object x)
							{
								Thing thing2 = x as Thing;
								if (thing2 != null && thing2.def.category != ThingCategory.Item)
								{
									Building building2 = thing2 as Building;
									return building2 != null && building2.MaxItemsInCell > 1;
								}
								return true;
							}))
							{
								if (obj2 != list[0])
								{
									this.ClearSelection();
									this.Select(list[0], true, true);
									return;
								}
								List<object> list2 = list;
								Building building = list2[list2.Count - 1] as Building;
								if (building != null && building.MaxItemsInCell > 1)
								{
									this.ClearSelection();
									List<object> list3 = list;
									this.Select(list3[list3.Count - 1], true, true);
									return;
								}
								this.ClearSelection();
								this.Select(list[num], true, true);
								return;
							}
						}
						this.ClearSelection();
						this.Select(list[num], true, true);
						return;
					}
					using (List<object>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj3 = enumerator.Current;
							if (this.selected.Contains(obj3))
							{
								this.Deselect(obj3);
							}
						}
						return;
					}
				}
				if (!Selector.ShiftIsHeld)
				{
					this.ClearSelection();
				}
				this.Select(list[0], true, true);
			}
		}

		// Token: 0x06012ACE RID: 76494 RVA: 0x005874BC File Offset: 0x005856BC
		public void SelectNextAt(IntVec3 c, Map map)
		{
			if (this.SelectedObjects.Count != 1)
			{
				Log.Error("Cannot select next at with < or > 1 selected.");
				return;
			}
			List<object> list = Selector.SelectableObjectsAt(c, map).ToList<object>();
			int num = list.IndexOf(this.SingleSelectedThing) + 1;
			if (num >= list.Count)
			{
				num -= list.Count;
			}
			this.ClearSelection();
			this.Select(list[num], true, true);
		}

		// Token: 0x06012ACF RID: 76495 RVA: 0x00587528 File Offset: 0x00585728
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			Selector.<>c__DisplayClass47_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			if (list.Count == 0)
			{
				return;
			}
			CS$<>8__locals1.clickedThing = (list.FirstOrDefault(delegate(object o)
			{
				Pawn pawn = o as Pawn;
				return pawn != null && pawn.Faction == Faction.OfPlayer && !pawn.IsPrisoner;
			}) as Thing);
			if (CS$<>8__locals1.clickedThing == null)
			{
				CS$<>8__locals1.clickedThing = (list.FirstOrDefault((object o) => o is Pawn) as Thing);
			}
			if (CS$<>8__locals1.clickedThing != null && !CS$<>8__locals1.clickedThing.Spawned)
			{
				CS$<>8__locals1.clickedThing = null;
			}
			if (CS$<>8__locals1.clickedThing == null)
			{
				CS$<>8__locals1.clickedThing = (list.FirstOrDefault(delegate(object o)
				{
					Thing thing2 = o as Thing;
					return thing2 != null && !thing2.def.neverMultiSelect;
				}) as Thing);
			}
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			if (CS$<>8__locals1.clickedThing != null)
			{
				Building edifice = CS$<>8__locals1.clickedThing.PositionHeld.GetEdifice(CS$<>8__locals1.clickedThing.MapHeld);
				if (edifice != null)
				{
					Building_Storage building_Storage = edifice as Building_Storage;
					if (building_Storage != null && building_Storage.GetSlotGroup().HeldThings.Contains(CS$<>8__locals1.clickedThing))
					{
						this.Deselect(edifice);
					}
				}
				foreach (object obj in ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(rect))
				{
					Thing thing = (Thing)obj;
					if (this.<SelectAllMatchingObjectUnderMouseOnScreen>g__Validator|47_3(thing.GetInnerIfMinified(), ref CS$<>8__locals1))
					{
						this.Select(thing, true, true);
					}
				}
				return;
			}
			Zone zone = list.FirstOrDefault(delegate(object o)
			{
				Zone zone2 = o as Zone;
				return zone2 != null && zone2.IsMultiselectable;
			}) as Zone;
			if (zone == null)
			{
				return;
			}
			foreach (Zone obj2 in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(rect, zone))
			{
				if (!this.IsSelected(obj2))
				{
					this.Select(obj2, true, true);
				}
			}
		}

		// Token: 0x06012AD0 RID: 76496 RVA: 0x00587770 File Offset: 0x00585970
		private void HandleMultiselectGoto(FloatMenuContext context)
		{
			Selector.tmpDraftedGotoPawns.Clear();
			foreach (Pawn pawn in context.allSelectedPawns)
			{
				if (FloatMenuMakerMap.ShouldGenerateFloatMenuForPawn(pawn) && pawn.Drafted)
				{
					Selector.tmpDraftedGotoPawns.Add(pawn);
				}
			}
			if (Selector.tmpDraftedGotoPawns.Count == 1)
			{
				if (FloatMenuOptionProvider_DraftedMove.PawnCanGoto(Selector.tmpDraftedGotoPawns[0], context.ClickedCell))
				{
					FloatMenuOptionProvider_DraftedMove.PawnGotoAction(context.ClickedCell, Selector.tmpDraftedGotoPawns[0], context.ClickedCell);
				}
			}
			else
			{
				IntVec3 mouseCell = CellFinder.StandableCellNear(context.ClickedCell, context.map, 2.9f, null);
				if (mouseCell.IsValid)
				{
					this.gotoController.StartInteraction(mouseCell);
					foreach (Pawn pawn2 in Selector.tmpDraftedGotoPawns)
					{
						this.gotoController.AddPawn(pawn2);
					}
				}
			}
			Selector.tmpDraftedGotoPawns.Clear();
		}

		// Token: 0x06012AD3 RID: 76499 RVA: 0x005878FC File Offset: 0x00585AFC
		[CompilerGenerated]
		private bool <SelectInsideDragBox>g__SelectWhere|42_0(Predicate<Thing> predicate, Action<List<Thing>> postProcessor = null, ref Selector.<>c__DisplayClass42_0 A_3)
		{
			Selector.tmp_selectedBoxThings.Clear();
			IEnumerable<Thing> boxThings = A_3.boxThings;
			Func<Thing, bool> <>9__6;
			Func<Thing, bool> predicate2;
			if ((predicate2 = <>9__6) == null)
			{
				predicate2 = (<>9__6 = ((Thing t) => predicate(t)));
			}
			foreach (Thing item in boxThings.Where(predicate2))
			{
				Selector.tmp_selectedBoxThings.Add(item);
			}
			if (Selector.tmp_selectedBoxThings.Any<Thing>())
			{
				if (postProcessor != null)
				{
					postProcessor(Selector.tmp_selectedBoxThings);
				}
				foreach (Thing obj in Selector.tmp_selectedBoxThings)
				{
					this.Select(obj, true, true);
					A_3.selectedSomething = true;
				}
			}
			Selector.tmp_selectedBoxThings.Clear();
			return A_3.selectedSomething;
		}

		// Token: 0x06012AD4 RID: 76500 RVA: 0x00587A00 File Offset: 0x00585C00
		[CompilerGenerated]
		internal static bool <SelectInsideDragBox>g__IsColonist|42_1(Thing t)
		{
			return t.def.category == ThingCategory.Pawn && (((Pawn)t).RaceProps.Humanlike || (ModsConfig.BiotechActive && ((Pawn)t).RaceProps.IsMechanoid)) && t.Faction == Faction.OfPlayer;
		}

		// Token: 0x06012AD5 RID: 76501 RVA: 0x00587A55 File Offset: 0x00585C55
		[CompilerGenerated]
		internal static bool <SelectInsideDragBox>g__IsHumanlike|42_2(Thing t)
		{
			return t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike;
		}

		// Token: 0x06012AD6 RID: 76502 RVA: 0x00587A77 File Offset: 0x00585C77
		[CompilerGenerated]
		internal static bool <SelectInsideDragBox>g__IsResource|42_3(Thing t)
		{
			return t.def.CountAsResource;
		}

		// Token: 0x06012AD7 RID: 76503 RVA: 0x00587A84 File Offset: 0x00585C84
		[CompilerGenerated]
		internal static bool <SelectInsideDragBox>g__IsPawn|42_4(Thing t)
		{
			return t.def.category == ThingCategory.Pawn;
		}

		// Token: 0x06012AD9 RID: 76505 RVA: 0x00587A94 File Offset: 0x00585C94
		[CompilerGenerated]
		private bool <SelectAllMatchingObjectUnderMouseOnScreen>g__Validator|47_3(Thing t, ref Selector.<>c__DisplayClass47_0 A_2)
		{
			Thing innerIfMinified = A_2.clickedThing.GetInnerIfMinified();
			if (this.IsSelected(t) || t.Faction != innerIfMinified.Faction)
			{
				return false;
			}
			Pawn pawn = innerIfMinified as Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = t as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.HostFaction != pawn.HostFaction)
					{
						return false;
					}
					Pawn_MutantTracker mutant = pawn2.mutant;
					MutantDef mutantDef = (mutant != null) ? mutant.Def : null;
					Pawn_MutantTracker mutant2 = pawn.mutant;
					return mutantDef == ((mutant2 != null) ? mutant2.Def : null) && SelectorUtility.IsEquivalentRace(pawn2, pawn);
				}
			}
			return t.def == innerIfMinified.def;
		}

		// Token: 0x0400C1E9 RID: 49641
		public DragBox dragBox = new DragBox();

		// Token: 0x0400C1EA RID: 49642
		public MultiPawnGotoController gotoController = new MultiPawnGotoController();

		// Token: 0x0400C1EB RID: 49643
		private List<object> selected = new List<object>();

		// Token: 0x0400C1EC RID: 49644
		public const float PawnSelectRadius = 1f;

		// Token: 0x0400C1ED RID: 49645
		private const int MaxNumSelected = 200;

		// Token: 0x0400C1EE RID: 49646
		private static readonly List<Pawn> tmpSelectedPawns = new List<Pawn>();

		// Token: 0x0400C1EF RID: 49647
		private static List<Thing> tmp_selectedBoxThings = new List<Thing>();

		// Token: 0x0400C1F0 RID: 49648
		private static readonly List<Pawn> tmpDraftedGotoPawns = new List<Pawn>();
	}
}
