using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003E87 RID: 16007
	public class WorldSelector
	{
		// Token: 0x170036C3 RID: 14019
		// (get) Token: 0x06015E6A RID: 89706 RVA: 0x005583D3 File Offset: 0x005565D3
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x170036C4 RID: 14020
		// (get) Token: 0x06015E6B RID: 89707 RVA: 0x0068D8FF File Offset: 0x0068BAFF
		// (set) Token: 0x06015E6C RID: 89708 RVA: 0x0068D907 File Offset: 0x0068BB07
		public PlanetTile SelectedTile
		{
			get
			{
				return this.selectedTile;
			}
			set
			{
				if (this.selectedTile.Layer != value.Layer)
				{
					this.SelectedLayer = value.Layer;
				}
				this.selectedTile = value;
			}
		}

		// Token: 0x170036C5 RID: 14021
		// (get) Token: 0x06015E6D RID: 89709 RVA: 0x0068D931 File Offset: 0x0068BB31
		// (set) Token: 0x06015E6E RID: 89710 RVA: 0x0068D94C File Offset: 0x0068BB4C
		public PlanetLayer SelectedLayer
		{
			get
			{
				if (this.selectedLayer == null)
				{
					return Find.WorldGrid.Surface;
				}
				return this.selectedLayer;
			}
			set
			{
				if (this.selectedLayer != value)
				{
					this.selectedTile = PlanetTile.Invalid;
					this.SelectedObjects.Clear();
				}
				this.selectedLayer = value;
				if (this.selectedLayer.Def.isSpace)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Orbit, OpportunityType.GoodToKnow);
				}
			}
		}

		// Token: 0x170036C6 RID: 14022
		// (get) Token: 0x06015E6F RID: 89711 RVA: 0x0068D99C File Offset: 0x0068BB9C
		public List<WorldObject> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x170036C7 RID: 14023
		// (get) Token: 0x06015E70 RID: 89712 RVA: 0x0068D9A4 File Offset: 0x0068BBA4
		public WorldObject SingleSelectedObject
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

		// Token: 0x170036C8 RID: 14024
		// (get) Token: 0x06015E71 RID: 89713 RVA: 0x0068D9C2 File Offset: 0x0068BBC2
		public WorldObject FirstSelectedObject
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

		// Token: 0x170036C9 RID: 14025
		// (get) Token: 0x06015E72 RID: 89714 RVA: 0x0068D9DF File Offset: 0x0068BBDF
		public int NumSelectedObjects
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x170036CA RID: 14026
		// (get) Token: 0x06015E73 RID: 89715 RVA: 0x0068D9EC File Offset: 0x0068BBEC
		public bool AnyObjectOrTileSelected
		{
			get
			{
				return this.NumSelectedObjects != 0 || this.selectedTile.Valid;
			}
		}

		// Token: 0x170036CB RID: 14027
		// (get) Token: 0x06015E74 RID: 89716 RVA: 0x0068DA04 File Offset: 0x0068BC04
		public bool AnyCaravanSelected
		{
			get
			{
				for (int i = 0; i < this.selected.Count; i++)
				{
					if (this.selected[i] is Caravan)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06015E75 RID: 89717 RVA: 0x0068DA3D File Offset: 0x0068BC3D
		public void WorldSelectorOnGUI()
		{
			this.HandleWorldClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
		}

		// Token: 0x06015E76 RID: 89718 RVA: 0x0068DA70 File Offset: 0x0068BC70
		private void HandleWorldClicks()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MousePositionOnUIInverted;
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.selected.Count > 0)
				{
					if (this.selected.Count == 1 && this.selected[0] is Caravan)
					{
						Caravan caravan = (Caravan)this.selected[0];
						if (caravan.IsPlayerControlled && !FloatMenuMakerWorld.TryMakeFloatMenu(caravan))
						{
							this.AutoOrderToTile(caravan, GenWorld.MouseTile(false));
						}
					}
					else
					{
						for (int i = 0; i < this.selected.Count; i++)
						{
							Caravan caravan2 = this.selected[i] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled)
							{
								this.AutoOrderToTile(caravan2, GenWorld.MouseTile(false));
							}
						}
					}
					Event.current.Use();
				}
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				if (Event.current.button == 0 && this.dragBox.active)
				{
					this.dragBox.active = false;
					if (!this.dragBox.IsValid)
					{
						this.SelectUnderMouse(true);
					}
					else
					{
						this.SelectInsideDragBox();
					}
				}
				Event.current.Use();
			}
		}

		// Token: 0x06015E77 RID: 89719 RVA: 0x0068DBF3 File Offset: 0x0068BDF3
		public bool IsSelected(WorldObject obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x06015E78 RID: 89720 RVA: 0x0068DC01 File Offset: 0x0068BE01
		public void ClearSelection()
		{
			WorldSelectionDrawer.Clear();
			this.selected.Clear();
			this.selectedTile = PlanetTile.Invalid;
		}

		// Token: 0x06015E79 RID: 89721 RVA: 0x0068DC1E File Offset: 0x0068BE1E
		public void Deselect(WorldObject obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x06015E7A RID: 89722 RVA: 0x0068DC3C File Offset: 0x0068BE3C
		public void Select(WorldObject obj, bool playSound = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.");
				return;
			}
			this.selectedTile = PlanetTile.Invalid;
			if (this.selected.Count >= 80)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				WorldSelectionDrawer.Notify_Selected(obj);
			}
		}

		// Token: 0x06015E7B RID: 89723 RVA: 0x0068DC97 File Offset: 0x0068BE97
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		// Token: 0x06015E7C RID: 89724 RVA: 0x0068DCA5 File Offset: 0x0068BEA5
		private void PlaySelectionSoundFor(WorldObject obj)
		{
			SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
		}

		// Token: 0x06015E7D RID: 89725 RVA: 0x0068DCB4 File Offset: 0x0068BEB4
		private void SelectInsideDragBox()
		{
			if (!this.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			bool flag = false;
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Caravan> list = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
				for (int i = 0; i < list.Count; i++)
				{
					flag = true;
					this.Select(list[i], true);
				}
			}
			if (!flag && Current.ProgramState == ProgramState.Playing)
			{
				List<Thing> list2 = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
				for (int j = 0; j < list2.Count; j++)
				{
					if (!flag)
					{
						CameraJumper.TryJumpAndSelect(list2[j], CameraJumper.MovementMode.Pan);
						flag = true;
					}
					else
					{
						Find.Selector.Select(list2[j], true, true);
					}
				}
			}
			if (!flag)
			{
				List<WorldObject> list3 = WorldObjectSelectionUtility.MultiSelectableWorldObjectsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<WorldObject>();
				if (list3.Any((WorldObject x) => x is Caravan))
				{
					list3.RemoveAll((WorldObject x) => !(x is Caravan));
					if (list3.Any((WorldObject x) => x.Faction == Faction.OfPlayer))
					{
						list3.RemoveAll((WorldObject x) => x.Faction != Faction.OfPlayer);
					}
				}
				for (int k = 0; k < list3.Count; k++)
				{
					flag = true;
					this.Select(list3[k], true);
				}
			}
			if (!flag)
			{
				bool canSelectTile = this.dragBox.Diagonal < 30f;
				this.SelectUnderMouse(canSelectTile);
			}
		}

		// Token: 0x06015E7E RID: 89726 RVA: 0x0068DE78 File Offset: 0x0068C078
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse()
		{
			bool flag;
			bool flag2;
			return this.SelectableObjectsUnderMouse(out flag, out flag2);
		}

		// Token: 0x06015E7F RID: 89727 RVA: 0x0068DE90 File Offset: 0x0068C090
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse(out bool clickedDirectlyOnCaravan, out bool usedColonistBar)
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(mousePositionOnUIInverted);
				if (caravan != null)
				{
					clickedDirectlyOnCaravan = true;
					usedColonistBar = true;
					return Gen.YieldSingle<WorldObject>(caravan);
				}
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			clickedDirectlyOnCaravan = false;
			if (list.Count > 0 && list[0] is Caravan && list[0].DistanceToMouse(UI.MousePositionOnUI) < GenWorldUI.CaravanDirectClickRadius)
			{
				clickedDirectlyOnCaravan = true;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					WorldObject worldObject = list[i];
					if (worldObject is Caravan && worldObject.DistanceToMouse(UI.MousePositionOnUI) > GenWorldUI.CaravanDirectClickRadius)
					{
						list.Remove(worldObject);
					}
				}
			}
			usedColonistBar = false;
			return list;
		}

		// Token: 0x06015E80 RID: 89728 RVA: 0x0068DF4C File Offset: 0x0068C14C
		public static IEnumerable<WorldObject> SelectableObjectsAt(PlanetTile tile)
		{
			foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(tile))
			{
				if (worldObject.SelectableNow)
				{
					yield return worldObject;
				}
			}
			IEnumerator<WorldObject> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06015E81 RID: 89729 RVA: 0x0068DF5C File Offset: 0x0068C15C
		private void SelectUnderMouse(bool canSelectTile = true)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Thing thing = Find.ColonistBar.ColonistOrCorpseAt(UI.MousePositionOnUIInverted);
				Pawn pawn = thing as Pawn;
				if (thing != null && (pawn == null || !pawn.IsCaravanMember()))
				{
					if (thing.Spawned)
					{
						CameraJumper.TryJumpAndSelect(thing, CameraJumper.MovementMode.Pan);
						return;
					}
					CameraJumper.TryJump(thing, CameraJumper.MovementMode.Pan);
					return;
				}
			}
			bool flag;
			bool flag2;
			List<WorldObject> list = this.SelectableObjectsUnderMouse(out flag, out flag2).ToList<WorldObject>();
			if (flag2 || (flag && list.Count >= 2))
			{
				canSelectTile = false;
			}
			if (list.Count == 0)
			{
				if (!this.ShiftIsHeld)
				{
					PlanetTile lhs = this.selectedTile;
					this.ClearSelection();
					if (canSelectTile)
					{
						this.selectedTile = GenWorld.MouseTile(false);
						if (lhs != this.selectedTile && this.selectedTile.Valid)
						{
							SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
							return;
						}
					}
				}
			}
			else
			{
				if (list.FirstOrDefault((WorldObject obj) => this.selected.Contains(obj)) != null)
				{
					if (!this.ShiftIsHeld)
					{
						PlanetTile tile = canSelectTile ? GenWorld.MouseTile(false) : PlanetTile.Invalid;
						this.SelectFirstOrNextFrom(list, tile);
						return;
					}
					using (List<WorldObject>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							WorldObject worldObject = enumerator.Current;
							if (this.selected.Contains(worldObject))
							{
								this.Deselect(worldObject);
							}
						}
						return;
					}
				}
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
				}
				this.Select(list[0], true);
			}
		}

		// Token: 0x06015E82 RID: 89730 RVA: 0x0068E0E8 File Offset: 0x0068C2E8
		public void SelectFirstOrNextAt(PlanetTile tile)
		{
			this.SelectFirstOrNextFrom(WorldSelector.SelectableObjectsAt(tile).ToList<WorldObject>(), tile);
		}

		// Token: 0x06015E83 RID: 89731 RVA: 0x0068E0FC File Offset: 0x0068C2FC
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<WorldObject> list = this.SelectableObjectsUnderMouse().ToList<WorldObject>();
			if (list.Count == 0)
			{
				return;
			}
			Type type = list[0].GetType();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (!(type != allWorldObjects[i].GetType()) && (allWorldObjects[i] == list[0] || allWorldObjects[i].AllMatchingObjectsOnScreenMatchesWith(list[0])) && allWorldObjects[i].VisibleToCameraNow())
				{
					this.Select(allWorldObjects[i], true);
				}
			}
		}

		// Token: 0x06015E84 RID: 89732 RVA: 0x0068E19C File Offset: 0x0068C39C
		private void AutoOrderToTile(Caravan c, PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return;
			}
			if (c.autoJoinable && CaravanExitMapUtility.AnyoneTryingToJoinCaravan(c))
			{
				CaravanExitMapUtility.OpenSomeoneTryingToJoinCaravanDialog(c, delegate
				{
					this.AutoOrderToTileNow(c, tile);
				});
				return;
			}
			this.AutoOrderToTileNow(c, tile);
		}

		// Token: 0x06015E85 RID: 89733 RVA: 0x0068E218 File Offset: 0x0068C418
		private void AutoOrderToTileNow(Caravan c, PlanetTile tile)
		{
			if (!tile.Valid || (tile == c.Tile && !c.pather.Moving))
			{
				return;
			}
			PlanetTile planetTile = CaravanUtility.BestGotoDestNear(tile, c);
			if (planetTile.Valid)
			{
				c.pather.StartPath(planetTile, null, true, true);
				c.gotoMote.OrderedToTile(planetTile);
				SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06015E86 RID: 89734 RVA: 0x0068E284 File Offset: 0x0068C484
		private void SelectFirstOrNextFrom(List<WorldObject> objects, PlanetTile tile)
		{
			int num = objects.FindIndex((WorldObject x) => this.selected.Contains(x));
			PlanetTile planetTile = PlanetTile.Invalid;
			int num2 = -1;
			if (num != -1)
			{
				if (num == objects.Count - 1 || this.selected.Count >= 2)
				{
					if (this.selected.Count >= 2)
					{
						num2 = 0;
					}
					else if (tile.Valid)
					{
						planetTile = tile;
					}
					else
					{
						num2 = 0;
					}
				}
				else
				{
					num2 = num + 1;
				}
			}
			else if (objects.Count == 0)
			{
				planetTile = tile;
			}
			else
			{
				num2 = 0;
			}
			this.ClearSelection();
			if (num2 >= 0)
			{
				this.Select(objects[num2], true);
			}
			this.selectedTile = planetTile;
		}

		// Token: 0x0400F2EC RID: 62188
		public readonly WorldDragBox dragBox = new WorldDragBox();

		// Token: 0x0400F2ED RID: 62189
		private readonly List<WorldObject> selected = new List<WorldObject>();

		// Token: 0x0400F2EE RID: 62190
		private PlanetLayer selectedLayer;

		// Token: 0x0400F2EF RID: 62191
		private PlanetTile selectedTile = PlanetTile.Invalid;

		// Token: 0x0400F2F0 RID: 62192
		private const int MaxNumSelected = 80;

		// Token: 0x0400F2F1 RID: 62193
		private const float MaxDragBoxDiagonalToSelectTile = 30f;
	}
}
