using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000B74 RID: 2932
	public static class CellInspectorDrawer
	{
		// Token: 0x06004B2C RID: 19244 RVA: 0x0017E429 File Offset: 0x0017C629
		public static void Update()
		{
			if (!KeyBindingDefOf.ShowCellInspector.IsDown)
			{
				CellInspectorDrawer.active = false;
				return;
			}
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TileInspector, KnowledgeAmount.TinyInteraction);
			CellInspectorDrawer.active = true;
			if (CellInspectorDrawer.ShouldShow() && !WorldRendererUtility.WorldSelected)
			{
				GenUI.RenderMouseoverBracket();
			}
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x0017E464 File Offset: 0x0017C664
		public static void OnGUI()
		{
			if (!CellInspectorDrawer.ShouldShow() || Mouse.IsInputBlockedNow)
			{
				return;
			}
			Rect rect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 336f, (float)CellInspectorDrawer.numLines * 24f + 24f);
			CellInspectorDrawer.numLines = 0;
			rect.x += 26f;
			rect.y += 26f;
			if (rect.xMax > (float)UI.screenWidth)
			{
				rect.x -= rect.width + 52f;
			}
			if (rect.yMax > (float)UI.screenHeight)
			{
				rect.y -= rect.height + 52f;
			}
			Find.WindowStack.ImmediateWindow(62348, rect, WindowLayer.Super, new Action(CellInspectorDrawer.FillWindow), true, false, 1f, null, false);
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x0017E562 File Offset: 0x0017C762
		private static void FillWindow()
		{
			if (!CellInspectorDrawer.ShouldShow())
			{
				return;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			if (WorldRendererUtility.WorldSelected)
			{
				CellInspectorDrawer.DrawWorldInspector();
			}
			else
			{
				CellInspectorDrawer.DrawMapInspector();
			}
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x0017E5A0 File Offset: 0x0017C7A0
		private static void DrawMapInspector()
		{
			IntVec3 intVec = UI.MouseCell();
			List<Thing> list = intVec.GetThingList(Find.CurrentMap).Where(delegate(Thing thing)
			{
				if (thing.def.category != ThingCategory.Mote && thing.def.category != ThingCategory.Filth && thing.def.category != ThingCategory.Ethereal)
				{
					Pawn pawn = thing as Pawn;
					return pawn == null || !pawn.IsHiddenFromPlayer();
				}
				return false;
			}).ToList<Thing>();
			if (list.Any<Thing>())
			{
				foreach (Thing thing2 in list)
				{
					CellInspectorDrawer.DrawThingRow(thing2);
				}
			}
			IEnumerable<string> enumerable = from thing in intVec.GetThingList(Find.CurrentMap)
			where thing.def.category == ThingCategory.Filth
			select thing into filth
			select filth.def.label;
			if (enumerable.Any<string>())
			{
				CellInspectorDrawer.DrawRow("Filth_Label".Translate(), enumerable.ToCommaList(false, false).CapitalizeFirst().Truncate(170f, null));
			}
			if (list.Any<Thing>() || enumerable.Any<string>())
			{
				CellInspectorDrawer.DrawDivider();
			}
			Room room = intVec.GetRoom(Find.CurrentMap);
			if (room != null && room.Role != RoomRoleDefOf.None)
			{
				CellInspectorDrawer.DrawHeader(room.GetRoomRoleLabel().CapitalizeFirst());
				foreach (RoomStatDef roomStatDef in DefDatabase<RoomStatDef>.AllDefsListForReading)
				{
					if (!roomStatDef.isHidden || DebugViewSettings.showAllRoomStats)
					{
						float stat = room.GetStat(roomStatDef);
						RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
						CellInspectorDrawer.DrawRow(roomStatDef.LabelCap, (scoreStage == null) ? "" : (scoreStage.label.CapitalizeFirst() + " (" + roomStatDef.ScoreToString(stat) + ")"));
					}
				}
				CellInspectorDrawer.DrawDivider();
			}
			TerrainDef terrain = intVec.GetTerrain(Find.CurrentMap);
			bool flag = intVec.IsPolluted(Find.CurrentMap);
			float fertility = intVec.GetFertility(Find.CurrentMap);
			float temperature = intVec.GetTemperature(Find.CurrentMap);
			float value = Find.CurrentMap.glowGrid.GroundGlowAt(intVec, false, false);
			Zone zone = intVec.GetZone(Find.CurrentMap);
			float depth = Find.CurrentMap.snowGrid.GetDepth(intVec);
			object obj = ModsConfig.OdysseyActive ? Find.CurrentMap.sandGrid.GetDepth(intVec) : 0f;
			WeatherBuildupCategory buildupCategory = WeatherBuildupUtility.GetBuildupCategory(depth);
			object obj2 = obj;
			WeatherBuildupCategory buildupCategory2 = WeatherBuildupUtility.GetBuildupCategory(obj2);
			RoofDef roof = intVec.GetRoof(Find.CurrentMap);
			byte b = Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.BlindSmoke);
			byte b2 = Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.ToxGas);
			byte b3 = Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.RotStink);
			byte b4 = Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.DeadlifeDust);
			float num = BeautyUtility.AverageBeautyPerceptible(intVec, Find.CurrentMap);
			if (ModsConfig.OdysseyActive)
			{
				WaterBody waterBody = Find.CurrentMap.waterBodyTracker.WaterBodyAt(intVec);
				if (waterBody != null && waterBody.HasFish)
				{
					IEnumerable<ThingDef> commonFishIncludingExtras = waterBody.CommonFishIncludingExtras;
					IEnumerable<ThingDef> uncommonFish = waterBody.UncommonFish;
					string info = (from x in commonFishIncludingExtras.Concat(uncommonFish)
					select x.label).ToCommaList(false, false).CapitalizeFirst();
					CellInspectorDrawer.DrawRow("Fish".Translate(), info);
				}
			}
			CellInspectorDrawer.DrawRow("Beauty_Label".Translate(), num.ToString("F1"));
			if (zone != null)
			{
				CellInspectorDrawer.DrawRow("Zone_Label".Translate(), zone.label);
			}
			if (roof != null)
			{
				CellInspectorDrawer.DrawRow("Roof_Label".Translate(), roof.LabelCap);
			}
			CellInspectorDrawer.DrawRow("Terrain_Label".Translate(), flag ? "PollutedTerrain".Translate(terrain.label).CapitalizeFirst() : terrain.LabelCap);
			if (depth > 0.03f)
			{
				CellInspectorDrawer.DrawRow("Snow_Label".Translate(), WeatherBuildupUtility.GetSnowDescription(buildupCategory).CapitalizeFirst());
			}
			if (obj2 != 0.03f)
			{
				CellInspectorDrawer.DrawRow("Sand_Label".Translate(), WeatherBuildupUtility.GetSandDescription(buildupCategory2).CapitalizeFirst());
			}
			CellInspectorDrawer.DrawRow("WalkSpeed_Label".Translate(), GenPath.SpeedPercentString((float)Mathf.Max(terrain.pathCost, WeatherBuildupUtility.MovementTicksAddOn(buildupCategory))));
			if ((double)fertility > 0.0001)
			{
				CellInspectorDrawer.DrawRow("Fertility_Label".Translate(), fertility.ToStringPercent());
			}
			CellInspectorDrawer.DrawRow("Temperature_Label".Translate(), temperature.ToStringTemperature("F0"));
			CellInspectorDrawer.DrawRow("LightLevel_Label".Translate(), MouseoverUtility.GetGlowLabelByValue(value));
			if (b > 0)
			{
				CellInspectorDrawer.DrawRow(GasType.BlindSmoke.GetLabel().CapitalizeFirst(), ((float)b / 255f).ToStringPercent("F0"));
			}
			if (b2 > 0)
			{
				CellInspectorDrawer.DrawRow(GasType.ToxGas.GetLabel().CapitalizeFirst(), ((float)b2 / 255f).ToStringPercent("F0"));
			}
			if (b3 > 0)
			{
				CellInspectorDrawer.DrawRow(GasType.RotStink.GetLabel().CapitalizeFirst(), ((float)b3 / 255f).ToStringPercent("F0"));
			}
			if (b4 > 0)
			{
				CellInspectorDrawer.DrawRow(GasType.DeadlifeDust.GetLabel().CapitalizeFirst(), ((float)b4 / 255f).ToStringPercent("F0"));
			}
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x0017EB54 File Offset: 0x0017CD54
		private static void DrawWorldInspector()
		{
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			PlanetTile mouseTileIndex = GenWorld.MouseTile(false);
			Tile tile = Find.WorldGrid[mouseTileIndex];
			foreach (WorldObject worldObject in list)
			{
				CellInspectorDrawer.DrawHeader(worldObject.LabelCap);
				WorldObject worldObject2 = worldObject;
				Settlement settlement = worldObject2 as Settlement;
				if (settlement == null)
				{
					Caravan caravan = worldObject2 as Caravan;
					if (caravan != null)
					{
						CellInspectorDrawer.DrawRow("CaravanColonists_Label".Translate(), caravan.pawns.Count((Pawn pawn) => pawn.IsColonist).ToString());
						if (caravan.pather.Moving)
						{
							if (!caravan.pather.MovingNow)
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), CaravanBedUtility.AppendUsingBedsLabel("CaravanResting".Translate(), caravan.beds.GetUsedBedCount()));
							}
							else if (caravan.pather.ArrivalAction != null)
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), caravan.pather.ArrivalAction.ReportString);
							}
							else
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), "CaravanTraveling".Translate());
							}
							float num = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(caravan, true) / 60000f;
							CellInspectorDrawer.DrawRow("CaravanTTD_Label".Translate(), num.ToString("0.#"));
						}
						else
						{
							Settlement settlement2 = CaravanVisitUtility.SettlementVisitedNow(caravan);
							if (settlement2 != null)
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), "CaravanVisiting".Translate(settlement2.Label));
							}
							else
							{
								CellInspectorDrawer.DrawRow("CaravanStatus_Label".Translate(), "CaravanWaiting".Translate());
							}
						}
					}
				}
				else if (settlement.Faction != null)
				{
					CellInspectorDrawer.DrawRow("Faction_Label".Translate(), settlement.Faction.Name);
					if (settlement.Faction != Faction.OfPlayer)
					{
						if (settlement.Faction.Hidden)
						{
							CellInspectorDrawer.DrawRow("Relationship_Label".Translate(), settlement.Faction.PlayerRelationKind.GetLabelCap());
						}
						else
						{
							CellInspectorDrawer.DrawRow("Relationship_Label".Translate(), settlement.Faction.PlayerRelationKind.GetLabelCap() + " (" + settlement.Faction.PlayerGoodwill.ToStringWithSign() + ")");
						}
					}
				}
				CellInspectorDrawer.DrawDivider();
			}
			CellInspectorDrawer.DrawRow("Biome_Label".Translate(), tile.PrimaryBiome.LabelCap);
			if (tile.Mutators.Any<TileMutatorDef>())
			{
				CellInspectorDrawer.DrawRow("Features_Label".Translate(), (from mutator in tile.Mutators
				select mutator.Label(mouseTileIndex)).ToCommaList(false, false).CapitalizeFirst().Truncate(170f, null));
			}
			if (!tile.PrimaryBiome.impassable)
			{
				CellInspectorDrawer.DrawRow("Hilliness_Label".Translate(), tile.hilliness.GetLabelCap());
			}
			SurfaceTile surfaceTile = tile as SurfaceTile;
			if (surfaceTile != null)
			{
				if (surfaceTile.Roads != null)
				{
					CellInspectorDrawer.DrawRow("Road_Label".Translate(), (from rl in surfaceTile.Roads
					select rl.road).MaxBy((RoadDef road) => road.priority).LabelCap);
				}
				if (surfaceTile.Rivers != null)
				{
					CellInspectorDrawer.DrawRow("River_Label".Translate(), surfaceTile.Rivers[0].river.LabelCap);
				}
			}
			if (!Find.World.Impassable(mouseTileIndex))
			{
				string info = (WorldPathGrid.CalculatedMovementDifficultyAt(mouseTileIndex, false, null, null) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(mouseTileIndex, PlanetTile.Invalid, null)).ToString("0.#");
				CellInspectorDrawer.DrawRow("MovementDifficulty_Label".Translate(), info);
			}
			if (ModsConfig.BiotechActive && tile.pollution > 0f)
			{
				CellInspectorDrawer.DrawRow("Pollution_Label".Translate(), GenWorld.GetPollutionDescription(tile.pollution) + " (" + tile.pollution.ToStringPercent() + ")");
			}
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x0017F060 File Offset: 0x0017D260
		private static bool ShouldShow()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (WorldRendererUtility.WorldSelected)
			{
				if (!GenWorld.MouseTile(false).Valid)
				{
					return false;
				}
			}
			else if (Find.CurrentMap != null && (!UI.MouseCell().InBounds(Find.CurrentMap) || UI.MouseCell().Fogged(Find.CurrentMap)))
			{
				return false;
			}
			return CellInspectorDrawer.active;
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x0017F0C0 File Offset: 0x0017D2C0
		private static void DrawThingRow(Thing thing)
		{
			float num = (float)CellInspectorDrawer.numLines * 24f;
			CompSelectProxy compSelectProxy = thing.TryGetComp<CompSelectProxy>();
			if (compSelectProxy != null && compSelectProxy.thingToSelect != null)
			{
				thing = compSelectProxy.thingToSelect;
			}
			List<object> selectedObjects = Find.Selector.SelectedObjects;
			Rect rect = new Rect(12f, num + 12f, 312f, 24f);
			if (selectedObjects.Contains(thing))
			{
				Widgets.DrawHighlight(rect);
			}
			else if (CellInspectorDrawer.numLines % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			rect = new Rect(24f, num + 12f + 1f, 22f, 22f);
			float num2;
			float num3;
			Vector2 vector;
			Color color;
			Material material;
			if (thing is Blueprint || thing is Frame)
			{
				Widgets.DefIcon(rect, thing.def, null, 1f, null, false, null, null, null, 1f);
			}
			else if (thing is Pawn || thing is Corpse)
			{
				Widgets.ThingIcon(rect.ExpandedBy(5f), thing, 1f, null, false, 1f, false);
			}
			else if (Widgets.GetIconFor(thing, new Vector2(rect.width, rect.height), null, false, out num2, out num3, out vector, out color, out material) != BaseContent.BadTex)
			{
				Widgets.ThingIcon(rect, thing, 1f, null, false, 1f, false);
			}
			rect = new Rect(58f, num + 12f, 370f, 24f);
			Widgets.LabelEllipses(rect, thing.LabelMouseover);
			CellInspectorDrawer.numLines++;
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x0017F268 File Offset: 0x0017D468
		private static void DrawRow(string label, string info)
		{
			float num = (float)CellInspectorDrawer.numLines * 24f;
			Rect rect = new Rect(12f, num + 12f, 312f, 24f);
			if (CellInspectorDrawer.numLines % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			GUI.color = Color.gray;
			rect = new Rect(24f, num + 12f, 130f, 24f);
			Widgets.Label(rect, label);
			GUI.color = Color.white;
			rect = new Rect(154f, num + 12f, 170f, 24f);
			Widgets.LabelEllipses(rect, info);
			CellInspectorDrawer.numLines++;
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x0017F318 File Offset: 0x0017D518
		private static void DrawHeader(string text)
		{
			float num = (float)CellInspectorDrawer.numLines * 24f;
			Rect rect = new Rect(12f, num + 12f - 8f, 312f, 28f);
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, text);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			CellInspectorDrawer.numLines++;
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x0017F380 File Offset: 0x0017D580
		private static void DrawDivider()
		{
			float num = (float)CellInspectorDrawer.numLines * 24f;
			GUI.color = Color.gray;
			Widgets.DrawLineHorizontal(0f, num + 12f + 12f, 336f);
			GUI.color = Color.white;
			CellInspectorDrawer.numLines++;
		}

		// Token: 0x0400357C RID: 13692
		private static int numLines;

		// Token: 0x0400357D RID: 13693
		private const float DistFromMouse = 26f;

		// Token: 0x0400357E RID: 13694
		private const float LabelColumnWidth = 130f;

		// Token: 0x0400357F RID: 13695
		private const float InfoColumnWidth = 170f;

		// Token: 0x04003580 RID: 13696
		private const float WindowPadding = 12f;

		// Token: 0x04003581 RID: 13697
		private const float ColumnPadding = 12f;

		// Token: 0x04003582 RID: 13698
		private const float LineHeight = 24f;

		// Token: 0x04003583 RID: 13699
		private const float ThingIconSize = 22f;

		// Token: 0x04003584 RID: 13700
		private const float WindowWidth = 336f;

		// Token: 0x04003585 RID: 13701
		public static bool active;
	}
}
