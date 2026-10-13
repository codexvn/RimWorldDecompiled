using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D9B RID: 15771
	[StaticConstructorOnStartup]
	public static class WorldGizmoUtility
	{
		// Token: 0x17003572 RID: 13682
		// (get) Token: 0x060157D0 RID: 88016 RVA: 0x00670377 File Offset: 0x0066E577
		public static Gizmo LastMouseOverGizmo
		{
			get
			{
				return WorldGizmoUtility.lastMouseOverGizmo;
			}
		}

		// Token: 0x060157D1 RID: 88017 RVA: 0x00670380 File Offset: 0x0066E580
		public static void WorldUIOnGUI()
		{
			WorldGizmoUtility.tmpObjectsList.Clear();
			foreach (Gizmo item in Find.WorldGrid.GetGizmos())
			{
				WorldGizmoUtility.tmpObjectsList.Add(item);
			}
			if (!Find.TilePicker.Active)
			{
				WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				Gizmo item2;
				if (WorldGizmoUtility.TryGetCaravanGizmo(out item2))
				{
					WorldGizmoUtility.tmpObjectsList.Add(item2);
				}
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					if (!worldRoutePlanner.Active || selectedObjects[i] is RoutePlannerWaypoint)
					{
						WorldGizmoUtility.tmpObjectsList.Add(selectedObjects[i]);
					}
				}
				if (Find.WorldSelector.SelectedTile != PlanetTile.Invalid)
				{
					WorldGizmoUtility.tmpObjectsList.AddRange(Find.WorldSelector.SelectedTile.Tile.GetGizmos());
				}
			}
			GizmoGridDrawer.DrawGizmoGridFor(WorldGizmoUtility.tmpObjectsList, out WorldGizmoUtility.mouseoverGizmo);
			WorldGizmoUtility.tmpObjectsList.Clear();
		}

		// Token: 0x060157D2 RID: 88018 RVA: 0x006704A4 File Offset: 0x0066E6A4
		public static void WorldUIUpdate()
		{
			WorldGizmoUtility.lastMouseOverGizmo = WorldGizmoUtility.mouseoverGizmo;
			if (WorldGizmoUtility.mouseoverGizmo != null)
			{
				WorldGizmoUtility.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x060157D3 RID: 88019 RVA: 0x006704C4 File Offset: 0x0066E6C4
		public static Gizmo GetJumpToGizmo()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "WorldJumpTo".Translate() + "...";
			command_Action.defaultDesc = "WorldJumpToDesc".Translate();
			command_Action.icon = WorldGizmoUtility.JumpToCommand;
			command_Action.action = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				WorldGizmoUtility.tmpJumpTargets.Clear();
				using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Map m = enumerator.Current;
						if (!WorldGizmoUtility.tmpJumpTargets.Contains(m.Parent) && !m.IsPocketMap)
						{
							string text = m.Parent.LabelCap;
							string str;
							if (GravshipUtility.TryGetNameOfGravshipOnMap(m, out str))
							{
								text = text + " (" + str + ")";
							}
							list.Add(new FloatMenuOption(text, delegate()
							{
								CameraJumper.TryJumpAndSelect(m.Parent, CameraJumper.MovementMode.Pan);
							}, m.Parent.ExpandingIcon, m.Parent.ExpandingIconColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
							WorldGizmoUtility.tmpJumpTargets.Add(m.Parent);
						}
					}
				}
				foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
				{
					foreach (QuestPart questPart in quest.PartsListForReading)
					{
						QuestPart_SpawnWorldObject spawnPart = questPart as QuestPart_SpawnWorldObject;
						if (spawnPart != null && spawnPart.worldObject != null && !spawnPart.worldObject.Destroyed && spawnPart.worldObject.Spawned && !WorldGizmoUtility.tmpJumpTargets.Contains(spawnPart.worldObject))
						{
							list.Add(new FloatMenuOption(spawnPart.worldObject.LabelCap, delegate()
							{
								CameraJumper.TryJumpAndSelect(spawnPart.worldObject, CameraJumper.MovementMode.Pan);
							}, spawnPart.worldObject.ExpandingIcon, spawnPart.worldObject.ExpandingIconColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
							WorldGizmoUtility.tmpJumpTargets.Add(spawnPart.worldObject);
						}
					}
				}
				using (List<Caravan>.Enumerator enumerator4 = Find.WorldObjects.Caravans.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Caravan caravan = enumerator4.Current;
						if (!WorldGizmoUtility.tmpJumpTargets.Contains(caravan))
						{
							list.Add(new FloatMenuOption(caravan.LabelCap, delegate()
							{
								CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
							}, caravan.ExpandingIcon, caravan.ExpandingIconColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
							WorldGizmoUtility.tmpJumpTargets.Add(caravan);
						}
					}
				}
				if (!list.Any<FloatMenuOption>())
				{
					list.Add(new FloatMenuOption("NothingToJumpTo".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			};
			return command_Action;
		}

		// Token: 0x060157D4 RID: 88020 RVA: 0x0067053C File Offset: 0x0066E73C
		public static bool TryGetCaravanGizmo(out Gizmo gizmo)
		{
			gizmo = null;
			if (Find.WorldRoutePlanner.Active)
			{
				return false;
			}
			if (!PlanetLayer.Selected.Def.canFormCaravans)
			{
				return false;
			}
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			PlanetTile tile = Find.WorldSelector.SelectedTile;
			Settlement settlement = null;
			foreach (WorldObject worldObject in selectedObjects)
			{
				if (worldObject is Caravan || worldObject is CaravansBattlefield || worldObject is TravellingTransporters)
				{
					return false;
				}
				Settlement settlement2 = worldObject as Settlement;
				if (settlement2 != null)
				{
					settlement = settlement2;
				}
				FormCaravanComp component = worldObject.GetComponent<FormCaravanComp>();
				if (component != null && component.CanReformNow())
				{
					return false;
				}
				if (!tile.Valid)
				{
					tile = worldObject.Tile;
				}
			}
			WorldGizmoUtility.RefreshSettlements();
			if (WorldGizmoUtility.settlements.Count == 0 || Find.TilePicker.ForGravship)
			{
				return false;
			}
			settlement = (settlement ?? Find.WorldObjects.SettlementAt(tile));
			if (settlement != null)
			{
				if (settlement.Faction != Faction.OfPlayer)
				{
					gizmo = WorldGizmoUtility.SendToTileGizmo(settlement.Tile);
				}
				else if (settlement.Map != null)
				{
					gizmo = WorldGizmoUtility.GetFormCaravanAction("CommandFormCaravan".Translate(), "CommandFormCaravanDesc".Translate(), delegate
					{
						Find.WindowStack.Add(new Dialog_FormCaravan(settlement.Map, false, null, false, null));
					});
				}
				return true;
			}
			MapParent parent = Find.WorldObjects.MapParentAt(tile);
			MapParent parent2 = parent;
			FormCaravanComp formCaravanComp = (parent2 != null) ? parent2.GetComponent<FormCaravanComp>() : null;
			if (formCaravanComp != null && parent.HasMap && parent.Map.mapPawns.ColonistCount > 0 && !formCaravanComp.Reform)
			{
				gizmo = WorldGizmoUtility.GetFormCaravanAction("CommandFormCaravan".Translate(), "CommandFormCaravanDesc".Translate(), delegate
				{
					Dialog_FormCaravan window = new Dialog_FormCaravan(parent.Map, false, null, false, null);
					Find.WindowStack.Add(window);
				});
				return true;
			}
			if (tile.Valid)
			{
				gizmo = WorldGizmoUtility.SendToTileGizmo(tile);
				return true;
			}
			gizmo = WorldGizmoUtility.GetFormCaravanAction("CommandFormCaravan".Translate(), "CommandFormCaravanDesc".Translate(), delegate
			{
				Dialog_FormCaravan window = new Dialog_FormCaravan(WorldGizmoUtility.settlements[0].Map, false, null, false, null);
				Find.WindowStack.Add(window);
			});
			return true;
		}

		// Token: 0x060157D5 RID: 88021 RVA: 0x006707BC File Offset: 0x0066E9BC
		private static Gizmo SendToTileGizmo(PlanetTile tile)
		{
			WorldGizmoUtility.possible.Clear();
			if (!PlanetLayer.Selected.Def.canFormCaravans)
			{
				return null;
			}
			if (!Find.World.Impassable(tile) || Find.WorldObjects.AnySettlementAt(tile))
			{
				for (int i = 0; i < WorldGizmoUtility.settlements.Count; i++)
				{
					if (WorldGizmoUtility.settlements[i].Tile.LayerDef.canFormCaravans && Find.WorldReachability.CanReach(WorldGizmoUtility.settlements[i].Tile, tile))
					{
						WorldGizmoUtility.possible.Add(WorldGizmoUtility.settlements[i]);
					}
				}
			}
			TaggedString taggedString = "CommandSendCaravanDesc".Translate();
			Gizmo formCaravanAction;
			if (WorldGizmoUtility.possible.Count > 1)
			{
				formCaravanAction = WorldGizmoUtility.GetFormCaravanAction("CommandSendCaravanMultiple".Translate(), taggedString, delegate
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					for (int j = 0; j < WorldGizmoUtility.possible.Count; j++)
					{
						MapParent settlement = WorldGizmoUtility.possible[j];
						list.Add(new FloatMenuOption(settlement.LabelCap, delegate()
						{
							WorldGizmoUtility.DialogFromToSettlement(settlement.Map, tile);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					FloatMenu floatMenu = new FloatMenu(list);
					floatMenu.absorbInputAroundWindow = true;
					Find.WindowStack.Add(floatMenu);
				});
			}
			else
			{
				formCaravanAction = WorldGizmoUtility.GetFormCaravanAction("CommandSendCaravan".Translate(), taggedString, delegate
				{
					WorldGizmoUtility.DialogFromToSettlement(WorldGizmoUtility.possible[0].Map, tile);
				});
			}
			if (!WorldGizmoUtility.possible.Any<MapParent>())
			{
				formCaravanAction.Disable("CommandSendCaravanCantReach".Translate());
			}
			return formCaravanAction;
		}

		// Token: 0x060157D6 RID: 88022 RVA: 0x00670910 File Offset: 0x0066EB10
		private static void DialogFromToSettlement(Map origin, PlanetTile tile)
		{
			Dialog_FormCaravan dialog_FormCaravan = new Dialog_FormCaravan(origin, false, null, false, null);
			Find.WindowStack.Add(dialog_FormCaravan);
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			worldRoutePlanner.Start(dialog_FormCaravan);
			worldRoutePlanner.TryAddWaypoint(tile, true);
		}

		// Token: 0x060157D7 RID: 88023 RVA: 0x0067094E File Offset: 0x0066EB4E
		private static Command_Action GetFormCaravanAction(string label, string desc, Action action)
		{
			return new Command_Action
			{
				tutorTag = "FormCaravan",
				defaultLabel = label,
				defaultDesc = desc,
				icon = WorldGizmoUtility.FormCaravanCommand,
				hotKey = KeyBindingDefOf.Misc2,
				action = action
			};
		}

		// Token: 0x060157D8 RID: 88024 RVA: 0x0067098C File Offset: 0x0066EB8C
		private static void RefreshSettlements()
		{
			for (int i = WorldGizmoUtility.settlements.Count - 1; i >= 0; i--)
			{
				if (WorldGizmoUtility.settlements[i] == null)
				{
					WorldGizmoUtility.settlements.RemoveAt(i);
				}
				else if (!Current.Game.Maps.Contains(WorldGizmoUtility.settlements[i].Map))
				{
					WorldGizmoUtility.settlements.RemoveAt(i);
				}
			}
			for (int j = 0; j < Current.Game.Maps.Count; j++)
			{
				Map map = Current.Game.Maps[j];
				if (map.IsPlayerHome && !WorldGizmoUtility.settlements.Contains(map.Parent))
				{
					WorldGizmoUtility.settlements.Add(map.Parent);
				}
			}
		}

		// Token: 0x0400EF1E RID: 61214
		private static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x0400EF1F RID: 61215
		private static readonly Texture2D JumpToCommand = ContentFinder<Texture2D>.Get("UI/Commands/JumpTo", true);

		// Token: 0x0400EF20 RID: 61216
		private static Gizmo mouseoverGizmo;

		// Token: 0x0400EF21 RID: 61217
		private static Gizmo lastMouseOverGizmo;

		// Token: 0x0400EF22 RID: 61218
		private static readonly List<object> tmpObjectsList = new List<object>();

		// Token: 0x0400EF23 RID: 61219
		private static readonly List<MapParent> settlements = new List<MapParent>();

		// Token: 0x0400EF24 RID: 61220
		private static HashSet<WorldObject> tmpJumpTargets = new HashSet<WorldObject>();

		// Token: 0x0400EF25 RID: 61221
		private static readonly List<MapParent> possible = new List<MapParent>();
	}
}
