using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld.Planet
{
	// Token: 0x02003E81 RID: 16001
	[StaticConstructorOnStartup]
	public class WorldRoutePlanner
	{
		// Token: 0x170036BC RID: 14012
		// (get) Token: 0x06015E43 RID: 89667 RVA: 0x0068C5EF File Offset: 0x0068A7EF
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170036BD RID: 14013
		// (get) Token: 0x06015E44 RID: 89668 RVA: 0x0068C5F7 File Offset: 0x0068A7F7
		public bool FormingCaravan
		{
			get
			{
				return this.Active && this.currentFormCaravanDialog != null;
			}
		}

		// Token: 0x170036BE RID: 14014
		// (get) Token: 0x06015E45 RID: 89669 RVA: 0x0068C60C File Offset: 0x0068A80C
		private bool ShouldStop
		{
			get
			{
				return !this.active || !WorldRendererUtility.WorldSelected || (Current.ProgramState == ProgramState.Playing && Find.TickManager.CurTimeSpeed != TimeSpeed.Paused);
			}
		}

		// Token: 0x170036BF RID: 14015
		// (get) Token: 0x06015E46 RID: 89670 RVA: 0x0068C638 File Offset: 0x0068A838
		private int CaravanTicksPerMove
		{
			get
			{
				CaravanTicksPerMoveUtility.CaravanInfo? caravanInfo = this.CaravanInfo;
				if (caravanInfo != null && caravanInfo.Value.pawns.Any<Pawn>())
				{
					return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.Value, null);
				}
				return 3464;
			}
		}

		// Token: 0x170036C0 RID: 14016
		// (get) Token: 0x06015E47 RID: 89671 RVA: 0x0068C67C File Offset: 0x0068A87C
		private CaravanTicksPerMoveUtility.CaravanInfo? CaravanInfo
		{
			get
			{
				if (this.currentFormCaravanDialog != null)
				{
					return this.caravanInfoFromFormCaravanDialog;
				}
				Caravan caravanAtTheFirstWaypoint = this.CaravanAtTheFirstWaypoint;
				if (caravanAtTheFirstWaypoint != null)
				{
					return new CaravanTicksPerMoveUtility.CaravanInfo?(new CaravanTicksPerMoveUtility.CaravanInfo(caravanAtTheFirstWaypoint));
				}
				return null;
			}
		}

		// Token: 0x170036C1 RID: 14017
		// (get) Token: 0x06015E48 RID: 89672 RVA: 0x0068C6B7 File Offset: 0x0068A8B7
		private Caravan CaravanAtTheFirstWaypoint
		{
			get
			{
				if (!this.waypoints.Any<RoutePlannerWaypoint>())
				{
					return null;
				}
				return Find.WorldObjects.PlayerControlledCaravanAt(this.waypoints[0].Tile);
			}
		}

		// Token: 0x06015E49 RID: 89673 RVA: 0x0068C6E4 File Offset: 0x0068A8E4
		public void Start(PlanetLayer planetLayer)
		{
			if (this.active)
			{
				this.Stop();
			}
			this.layer = planetLayer;
			this.active = true;
			if (Current.ProgramState == ProgramState.Playing)
			{
				AmbientSoundManager.EnsureWorldAmbientSoundCreated();
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				Find.TickManager.Pause();
			}
		}

		// Token: 0x06015E4A RID: 89674 RVA: 0x0068C734 File Offset: 0x0068A934
		public void Start(Dialog_FormCaravan formCaravanDialog)
		{
			if (this.active)
			{
				this.Stop();
			}
			this.currentFormCaravanDialog = formCaravanDialog;
			this.caravanInfoFromFormCaravanDialog = new CaravanTicksPerMoveUtility.CaravanInfo?(new CaravanTicksPerMoveUtility.CaravanInfo(formCaravanDialog));
			formCaravanDialog.choosingRoute = true;
			Find.WindowStack.TryRemove(formCaravanDialog, false);
			this.Start(formCaravanDialog.CurrentTile.Layer);
			this.TryAddWaypoint(formCaravanDialog.CurrentTile, true);
			this.cantRemoveFirstWaypoint = true;
		}

		// Token: 0x06015E4B RID: 89675 RVA: 0x0068C7A4 File Offset: 0x0068A9A4
		public void Stop()
		{
			this.active = false;
			this.layer = null;
			for (int i = 0; i < this.waypoints.Count; i++)
			{
				this.waypoints[i].Destroy();
			}
			this.waypoints.Clear();
			this.cachedTicksToWaypoint.Clear();
			if (this.currentFormCaravanDialog != null)
			{
				this.currentFormCaravanDialog.Notify_NoLongerChoosingRoute();
			}
			this.caravanInfoFromFormCaravanDialog = null;
			this.currentFormCaravanDialog = null;
			this.cantRemoveFirstWaypoint = false;
			this.ReleasePaths();
		}

		// Token: 0x06015E4C RID: 89676 RVA: 0x0068C830 File Offset: 0x0068AA30
		public void WorldRoutePlannerUpdate()
		{
			if (this.active && this.ShouldStop)
			{
				this.Stop();
			}
			if (!this.active)
			{
				return;
			}
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].DrawPath(null);
			}
		}

		// Token: 0x06015E4D RID: 89677 RVA: 0x0068C884 File Offset: 0x0068AA84
		public void WorldRoutePlannerOnGUI()
		{
			if (!this.active)
			{
				return;
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				if (this.currentFormCaravanDialog != null)
				{
					Find.WindowStack.Add(this.currentFormCaravanDialog);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				this.Stop();
				Event.current.Use();
				return;
			}
			GenUI.DrawMouseAttachment(WorldRoutePlanner.MouseAttachment);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				Caravan caravan = Find.WorldSelector.SelectableObjectsUnderMouse().FirstOrDefault<WorldObject>() as Caravan;
				PlanetTile tile = (caravan != null) ? caravan.Tile : GenWorld.MouseTile(true);
				if (tile.Valid)
				{
					RoutePlannerWaypoint waypoint = this.MostRecentWaypointAt(tile);
					if (waypoint != null)
					{
						RoutePlannerWaypoint waypoint2 = waypoint;
						List<RoutePlannerWaypoint> list = this.waypoints;
						if (waypoint2 == list[list.Count - 1])
						{
							this.TryRemoveWaypoint(waypoint, true);
						}
						else
						{
							List<FloatMenuOption> list2 = new List<FloatMenuOption>();
							list2.Add(new FloatMenuOption("AddWaypoint".Translate(), delegate()
							{
								this.TryAddWaypoint(tile, true);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							list2.Add(new FloatMenuOption("RemoveWaypoint".Translate(), delegate()
							{
								this.TryRemoveWaypoint(waypoint, true);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							Find.WindowStack.Add(new FloatMenu(list2));
						}
					}
					else
					{
						this.TryAddWaypoint(tile, true);
					}
					Event.current.Use();
				}
			}
			this.DoRouteDetailsBox();
			if (this.DoChooseRouteButton())
			{
				return;
			}
			this.DoTileTooltips();
		}

		// Token: 0x06015E4E RID: 89678 RVA: 0x0068CA5C File Offset: 0x0068AC5C
		private void DoRouteDetailsBox()
		{
			WorldRoutePlanner.<>c__DisplayClass32_0 CS$<>8__locals1 = new WorldRoutePlanner.<>c__DisplayClass32_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.rect = new Rect(((float)UI.screenWidth - WorldRoutePlanner.BottomWindowSize.x) / 2f, (float)UI.screenHeight - WorldRoutePlanner.BottomWindowSize.y - 45f, WorldRoutePlanner.BottomWindowSize.x, WorldRoutePlanner.BottomWindowSize.y);
			if (Current.ProgramState == ProgramState.Entry)
			{
				WorldRoutePlanner.<>c__DisplayClass32_0 CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals2.rect.y = CS$<>8__locals2.rect.y - 22f;
			}
			Find.WindowStack.ImmediateWindow(1373514241, CS$<>8__locals1.rect, WindowLayer.Dialog, delegate
			{
				if (!CS$<>8__locals1.<>4__this.active)
				{
					return;
				}
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				float num = 6f;
				if (CS$<>8__locals1.<>4__this.waypoints.Count >= 2)
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerEstTimeToFinalDest".Translate(CS$<>8__locals1.<>4__this.GetTicksToWaypoint(CS$<>8__locals1.<>4__this.waypoints.Count - 1).ToStringTicksToDays("0.#")));
				}
				else if (CS$<>8__locals1.<>4__this.cantRemoveFirstWaypoint)
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerAddOneOrMoreWaypoints".Translate());
				}
				else
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerAddTwoOrMoreWaypoints".Translate());
				}
				num += 20f;
				if (CS$<>8__locals1.<>4__this.CaravanInfo == null || !CS$<>8__locals1.<>4__this.CaravanInfo.Value.pawns.Any<Pawn>())
				{
					GUI.color = new Color(0.8f, 0.6f, 0.6f);
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerUsingAverageTicksPerMoveWarning".Translate());
				}
				else if (CS$<>8__locals1.<>4__this.currentFormCaravanDialog == null && CS$<>8__locals1.<>4__this.CaravanAtTheFirstWaypoint != null)
				{
					GUI.color = Color.gray;
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerUsingTicksPerMoveOfCaravan".Translate(CS$<>8__locals1.<>4__this.CaravanAtTheFirstWaypoint.LabelCap));
				}
				num += 20f;
				GUI.color = Color.gray;
				Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), SteamDeck.IsSteamDeckInNonKeyboardMode ? "RoutePlannerPressRMBToAddAndRemoveWaypointsController".Translate() : "RoutePlannerPressRMBToAddAndRemoveWaypoints".Translate());
				num += 20f;
				if (CS$<>8__locals1.<>4__this.currentFormCaravanDialog != null)
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), SteamDeck.IsSteamDeckInNonKeyboardMode ? "RoutePlannerPressEscapeToReturnToCaravanFormationDialogController".Translate().Resolve().AdjustedForKeys(null, true) : "RoutePlannerPressEscapeToReturnToCaravanFormationDialog".Translate().Resolve());
				}
				else
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), SteamDeck.IsSteamDeckInNonKeyboardMode ? "RoutePlannerPressEscapeToExitController".Translate().Resolve().AdjustedForKeys(null, true) : "RoutePlannerPressEscapeToExit".Translate().Resolve());
				}
				num += 20f;
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f, null, false);
		}

		// Token: 0x06015E4F RID: 89679 RVA: 0x0068CB0C File Offset: 0x0068AD0C
		private bool DoChooseRouteButton()
		{
			if (this.currentFormCaravanDialog == null || this.waypoints.Count < 2)
			{
				return false;
			}
			if (Widgets.ButtonText(new Rect(((float)UI.screenWidth - WorldRoutePlanner.BottomButtonSize.x) / 2f, (float)UI.screenHeight - WorldRoutePlanner.BottomWindowSize.y - 45f - 10f - WorldRoutePlanner.BottomButtonSize.y, WorldRoutePlanner.BottomButtonSize.x, WorldRoutePlanner.BottomButtonSize.y), "Accept".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(this.currentFormCaravanDialog);
				this.currentFormCaravanDialog.Notify_ChoseRoute(this.waypoints[1].Tile);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				this.Stop();
				return true;
			}
			return false;
		}

		// Token: 0x06015E50 RID: 89680 RVA: 0x0068CBEC File Offset: 0x0068ADEC
		private void DoTileTooltips()
		{
			if (Mouse.IsInputBlockedNow)
			{
				return;
			}
			PlanetTile planetTile = GenWorld.MouseTile(true);
			if (!planetTile.Valid)
			{
				return;
			}
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (this.paths[i].NodesReversed.Contains(planetTile))
				{
					string str = this.GetTileTip(planetTile, i);
					Text.Font = GameFont.Small;
					Vector2 vector = Text.CalcSize(str);
					vector.x += 20f;
					vector.y += 20f;
					Vector2 mouseAttachedWindowPos = GenUI.GetMouseAttachedWindowPos(vector.x, vector.y);
					Rect rect = new Rect(mouseAttachedWindowPos, vector);
					Find.WindowStack.ImmediateWindow(1859615246, rect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Widgets.Label(rect.AtZero().ContractedBy(10f), str);
					}, true, false, 1f, null, false);
					return;
				}
			}
		}

		// Token: 0x06015E51 RID: 89681 RVA: 0x0068CCE0 File Offset: 0x0068AEE0
		private string GetTileTip(PlanetTile tile, int pathIndex)
		{
			int num = this.paths[pathIndex].NodesReversed.IndexOf(tile);
			PlanetTile planetTile;
			if (num > 0)
			{
				planetTile = this.paths[pathIndex].NodesReversed[num - 1];
			}
			else if (pathIndex < this.paths.Count - 1 && this.paths[pathIndex + 1].NodeCount >= 2)
			{
				planetTile = this.paths[pathIndex + 1].NodesReversed[this.paths[pathIndex + 1].NodeCount - 2];
			}
			else
			{
				planetTile = PlanetTile.Invalid;
			}
			int num2 = this.cachedTicksToWaypoint[pathIndex] + CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.paths[pathIndex].FirstNode, tile, this.paths[pathIndex], 0f, this.CaravanTicksPerMove, GenTicks.TicksAbs + this.cachedTicksToWaypoint[pathIndex]);
			int num3 = GenTicks.TicksAbs + num2;
			StringBuilder stringBuilder = new StringBuilder();
			if (num2 != 0)
			{
				stringBuilder.AppendLine("EstimatedTimeToTile".Translate(num2.ToStringTicksToDays("0.##")));
			}
			stringBuilder.AppendLine("ForagedFoodAmount".Translate() + ": " + Find.WorldGrid[tile].PrimaryBiome.forageability.ToStringPercent());
			stringBuilder.Append(VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(tile, num3));
			if (planetTile.Valid)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = new StringBuilder();
				float num4 = WorldPathGrid.CalculatedMovementDifficultyAt(planetTile, false, new int?(num3), stringBuilder2);
				float roadMovementDifficultyMultiplier = Find.WorldGrid.GetRoadMovementDifficultyMultiplier(tile, planetTile, stringBuilder2);
				stringBuilder.Append("TileMovementDifficulty".Translate() + ":\n" + stringBuilder2.ToString().Indented("  "));
				stringBuilder.AppendLine();
				stringBuilder.Append("  = ");
				stringBuilder.Append((num4 * roadMovementDifficultyMultiplier).ToString("0.#"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06015E52 RID: 89682 RVA: 0x0068CF0C File Offset: 0x0068B10C
		public void DoRoutePlannerButton(ref float curBaseY)
		{
			float num = 57f;
			float num2 = 33f;
			Rect rect = new Rect((float)UI.screenWidth - 10f - num, curBaseY - 10f - num2, num, num2);
			if (Widgets.ButtonImage(rect, WorldRoutePlanner.ButtonTex, Color.white, new Color(0.8f, 0.8f, 0.8f), true, null))
			{
				if (this.active)
				{
					this.Stop();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				else
				{
					this.Start(Find.WorldSelector.SelectedLayer);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegionByKey(rect, "RoutePlannerButtonTip");
			curBaseY -= num2 + 20f;
		}

		// Token: 0x06015E53 RID: 89683 RVA: 0x0068CFB8 File Offset: 0x0068B1B8
		public int GetTicksToWaypoint(int index)
		{
			return this.cachedTicksToWaypoint[index];
		}

		// Token: 0x06015E54 RID: 89684 RVA: 0x0068CFC8 File Offset: 0x0068B1C8
		public void TryAddWaypoint(PlanetTile tile, bool playSound = true)
		{
			if (tile.Layer != this.layer)
			{
				Messages.Message("MessageCantAddWaypointBecauseUnreachable".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (Find.World.Impassable(tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseImpassable".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.waypoints.Any<RoutePlannerWaypoint>())
			{
				WorldReachability worldReachability = Find.WorldReachability;
				List<RoutePlannerWaypoint> list = this.waypoints;
				if (!worldReachability.CanReach(list[list.Count - 1].Tile, tile))
				{
					Messages.Message("MessageCantAddWaypointBecauseUnreachable".Translate(), MessageTypeDefOf.RejectInput, false);
					return;
				}
			}
			if (this.waypoints.Count >= 25)
			{
				Messages.Message("MessageCantAddWaypointBecauseLimit".Translate(25), MessageTypeDefOf.RejectInput, false);
				return;
			}
			RoutePlannerWaypoint routePlannerWaypoint = (RoutePlannerWaypoint)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.RoutePlannerWaypoint);
			routePlannerWaypoint.Tile = tile;
			Find.WorldObjects.Add(routePlannerWaypoint);
			this.waypoints.Add(routePlannerWaypoint);
			this.RecreatePaths();
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06015E55 RID: 89685 RVA: 0x0068D0E8 File Offset: 0x0068B2E8
		public void TryRemoveWaypoint(RoutePlannerWaypoint point, bool playSound = true)
		{
			if (this.cantRemoveFirstWaypoint && this.waypoints.Any<RoutePlannerWaypoint>() && point == this.waypoints[0])
			{
				Messages.Message("MessageCantRemoveWaypointBecauseFirst".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			point.Destroy();
			this.waypoints.Remove(point);
			for (int i = this.waypoints.Count - 1; i >= 1; i--)
			{
				if (this.waypoints[i].Tile == this.waypoints[i - 1].Tile)
				{
					this.waypoints[i].Destroy();
					this.waypoints.RemoveAt(i);
				}
			}
			this.RecreatePaths();
			if (playSound)
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06015E56 RID: 89686 RVA: 0x0068D1BC File Offset: 0x0068B3BC
		private void ReleasePaths()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].ReleaseToPool();
			}
			this.paths.Clear();
		}

		// Token: 0x06015E57 RID: 89687 RVA: 0x0068D1FC File Offset: 0x0068B3FC
		private void RecreatePaths()
		{
			this.ReleasePaths();
			if (this.waypoints.NullOrEmpty<RoutePlannerWaypoint>())
			{
				return;
			}
			WorldPathing pather = this.waypoints[0].Tile.Layer.Pather;
			for (int i = 1; i < this.waypoints.Count; i++)
			{
				this.paths.Add(pather.FindPath(this.waypoints[i - 1].Tile, this.waypoints[i].Tile, null, null));
			}
			this.cachedTicksToWaypoint.Clear();
			int num = 0;
			int caravanTicksPerMove = this.CaravanTicksPerMove;
			for (int j = 0; j < this.waypoints.Count; j++)
			{
				if (j == 0)
				{
					this.cachedTicksToWaypoint.Add(0);
				}
				else
				{
					num += CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.waypoints[j - 1].Tile, this.waypoints[j].Tile, this.paths[j - 1], 0f, caravanTicksPerMove, GenTicks.TicksAbs + num);
					this.cachedTicksToWaypoint.Add(num);
				}
			}
		}

		// Token: 0x06015E58 RID: 89688 RVA: 0x0068D328 File Offset: 0x0068B528
		private RoutePlannerWaypoint MostRecentWaypointAt(PlanetTile tile)
		{
			for (int i = this.waypoints.Count - 1; i >= 0; i--)
			{
				if (this.waypoints[i].Tile == tile)
				{
					return this.waypoints[i];
				}
			}
			return null;
		}

		// Token: 0x0400F2CF RID: 62159
		private bool active;

		// Token: 0x0400F2D0 RID: 62160
		private PlanetLayer layer;

		// Token: 0x0400F2D1 RID: 62161
		private CaravanTicksPerMoveUtility.CaravanInfo? caravanInfoFromFormCaravanDialog;

		// Token: 0x0400F2D2 RID: 62162
		private Dialog_FormCaravan currentFormCaravanDialog;

		// Token: 0x0400F2D3 RID: 62163
		private bool cantRemoveFirstWaypoint;

		// Token: 0x0400F2D4 RID: 62164
		private readonly List<WorldPath> paths = new List<WorldPath>();

		// Token: 0x0400F2D5 RID: 62165
		private readonly List<int> cachedTicksToWaypoint = new List<int>();

		// Token: 0x0400F2D6 RID: 62166
		public readonly List<RoutePlannerWaypoint> waypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x0400F2D7 RID: 62167
		private const int MaxCount = 25;

		// Token: 0x0400F2D8 RID: 62168
		private static readonly Texture2D ButtonTex = ContentFinder<Texture2D>.Get("UI/Misc/WorldRoutePlanner", true);

		// Token: 0x0400F2D9 RID: 62169
		private static readonly Texture2D MouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/WaypointMouseAttachment", true);

		// Token: 0x0400F2DA RID: 62170
		private static readonly Vector2 BottomWindowSize = new Vector2(500f, 95f);

		// Token: 0x0400F2DB RID: 62171
		private static readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400F2DC RID: 62172
		private const float BottomWindowBotMargin = 45f;

		// Token: 0x0400F2DD RID: 62173
		private const float BottomWindowEntryExtraBotMargin = 22f;
	}
}
