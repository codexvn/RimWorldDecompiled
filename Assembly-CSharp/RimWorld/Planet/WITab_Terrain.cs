using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E71 RID: 15985
	public class WITab_Terrain : WITab
	{
		// Token: 0x170036A3 RID: 13987
		// (get) Token: 0x06015DE0 RID: 89568 RVA: 0x0068A494 File Offset: 0x00688694
		public override bool IsVisible
		{
			get
			{
				return base.SelPlanetTile.Valid && base.SelPlanetTile.Tile.OnSurface;
			}
		}

		// Token: 0x06015DE1 RID: 89569 RVA: 0x0068A4C6 File Offset: 0x006886C6
		public WITab_Terrain()
		{
			this.size = WITab_Terrain.WinSize;
			this.labelKey = "TabTerrain";
			this.tutorTag = "Terrain";
		}

		// Token: 0x06015DE2 RID: 89570 RVA: 0x0068A4F0 File Offset: 0x006886F0
		protected override void FillTab()
		{
			Rect outRect = new Rect(0f, 0f, WITab_Terrain.WinSize.x, WITab_Terrain.WinSize.y).ContractedBy(10f);
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, base.SelTile.PrimaryBiome.LabelCap);
			Rect rect3 = rect2;
			rect3.yMin += 35f;
			rect3.height = 99999f;
			Text.Font = GameFont.Small;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.verticalSpacing = 0f;
			listing_Standard.Begin(rect3);
			this.DrawScrollContents(listing_Standard, rect3);
			listing_Standard.End();
			Widgets.EndScrollView();
		}

		// Token: 0x06015DE3 RID: 89571 RVA: 0x0068A5D8 File Offset: 0x006887D8
		private void DrawScrollContents(Listing_Standard listing, Rect infoRect)
		{
			Tile selTile = base.SelTile;
			PlanetTile selPlanetTile = base.SelPlanetTile;
			listing.Label(selTile.PrimaryBiome.description, -1f, null);
			listing.Gap(8f);
			listing.GapLine(12f);
			if (!selTile.PrimaryBiome.implemented)
			{
				listing.Label(string.Format("{0} {1}", selTile.PrimaryBiome.LabelCap, "BiomeNotImplemented".Translate()), -1f, null);
			}
			WITab_Terrain.ListGeometricDetails(listing, selTile, selPlanetTile);
			listing.GapLine(12f);
			WITab_Terrain.ListTemperatureDetails(listing, selPlanetTile, selTile);
			WITab_Terrain.ListPollutionDetails(listing, selPlanetTile);
			listing.GapLine(12f);
			WITab_Terrain.ListMiscDetails(listing, selTile, selPlanetTile);
			this.lastDrawnHeight = infoRect.y + listing.CurHeight;
		}

		// Token: 0x06015DE4 RID: 89572 RVA: 0x0068A6C0 File Offset: 0x006888C0
		private static void ListMiscDetails(Listing_Standard listing, Tile ws, PlanetTile tile)
		{
			listing.LabelDouble("AverageDiseaseFrequency".Translate(), string.Format("{0:F1} {1}", 60f / ws.PrimaryBiome.diseaseMtbDays, "PerYear".Translate()), null);
			listing.LabelDouble("TimeZone".Translate(), GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(tile).x).ToStringWithSign(), null);
			if (Prefs.DevMode)
			{
				listing.LabelDouble("Debug world tile ID", tile.ToString(), null);
			}
		}

		// Token: 0x06015DE5 RID: 89573 RVA: 0x0068A764 File Offset: 0x00688964
		private static void ListPollutionDetails(Listing_Standard listing, PlanetTile tileID)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			listing.GapLine(12f);
			listing.LabelDouble("TilePollution".Translate(), Find.WorldGrid[tileID].pollution.ToStringPercent(), "TerrainPollutionTip".Translate());
			string text = "";
			foreach (IGrouping<float, CurvePoint> grouping in from p in WorldPollutionUtility.NearbyPollutionOverDistanceCurve
			group p by p.y)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (grouping.Count<CurvePoint>() > 1)
				{
					CurvePoint curvePoint = grouping.MinBy((CurvePoint p) => p.x);
					CurvePoint curvePoint2 = grouping.MaxBy((CurvePoint p) => p.x);
					text += string.Format(" - {0}-{1} {2}, {3}x {4}", new object[]
					{
						curvePoint.x,
						curvePoint2.x,
						"NearbyPollutionTilesAway".Translate(),
						grouping.Key,
						"PollutionLower".Translate()
					});
				}
				else
				{
					text += string.Format(" - {0} {1}, {2}x {3}", new object[]
					{
						grouping.First<CurvePoint>().x,
						"NearbyPollutionTilesAway".Translate(),
						grouping.Key,
						"PollutionLower".Translate()
					});
				}
			}
			TaggedString taggedString = "NearbyPollutionTip".Translate(4, text);
			float num = WorldPollutionUtility.CalculateNearbyPollutionScore(tileID);
			if (num >= GameConditionDefOf.NoxiousHaze.minNearbyPollution)
			{
				float value = GameConditionDefOf.NoxiousHaze.mtbOverNearbyPollutionCurve.Evaluate(num);
				taggedString += "\n\n" + "NoxiousHazeInterval".Translate(value);
			}
			else
			{
				taggedString += "\n\n" + "NoxiousHazeNeverOccurring".Translate();
			}
			listing.LabelDouble("TilePollutionNearby".Translate(), WorldPollutionUtility.CalculateNearbyPollutionScore(tileID).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute), taggedString);
		}

		// Token: 0x06015DE6 RID: 89574 RVA: 0x0068AA14 File Offset: 0x00688C14
		private static void ListTemperatureDetails(Listing_Standard listing, PlanetTile tile, Tile ws)
		{
			listing.LabelDouble("AvgTemp".Translate(), GenTemperature.GetAverageTemperatureLabel(tile), null);
			string rightLabel = WITab_Terrain.cachedGrowingQuadrumsDescription;
			if (WITab_Terrain.cachedGrowingQuadrumsTile != tile)
			{
				rightLabel = (WITab_Terrain.cachedGrowingQuadrumsDescription = Zone_Growing.GrowingQuadrumsDescription(tile, null));
				WITab_Terrain.cachedGrowingQuadrumsTile = tile;
			}
			listing.LabelDouble("OutdoorGrowingPeriod".Translate(), rightLabel, null);
			listing.LabelDouble("Rainfall".Translate(), ws.rainfall.ToString("F0") + "mm", null);
			if (ws.PrimaryBiome.foragedFood != null && ws.PrimaryBiome.forageability > 0f)
			{
				listing.LabelDouble("Forageability".Translate(), ws.PrimaryBiome.forageability.ToStringPercent() + " (" + ws.PrimaryBiome.foragedFood.label + ")", null);
			}
			else
			{
				listing.LabelDouble("Forageability".Translate(), "0%", null);
			}
			listing.LabelDouble("AnimalsCanGrazeNow".Translate(), VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsNowAt(tile) ? "Yes".Translate() : "No".Translate(), null);
		}

		// Token: 0x06015DE7 RID: 89575 RVA: 0x0068AB68 File Offset: 0x00688D68
		private static void ListGeometricDetails(Listing_Standard listing, Tile ws, PlanetTile tile)
		{
			if (ws.HillinessLabel != Hilliness.Undefined)
			{
				listing.LabelDouble("Terrain".Translate(), ws.HillinessLabel.GetLabelCap(), null);
			}
			SurfaceTile surfaceTile = ws as SurfaceTile;
			if (surfaceTile != null)
			{
				if (surfaceTile.Roads != null)
				{
					listing.LabelDouble("Road".Translate(), (from roadlink in surfaceTile.Roads
					select roadlink.road.label).Distinct<string>().ToCommaList(true, false).CapitalizeFirst(), null);
				}
				if (surfaceTile.Rivers != null)
				{
					listing.LabelDouble("River".Translate(), surfaceTile.Rivers.MaxBy((SurfaceTile.RiverLink riverlink) => riverlink.river.degradeThreshold).river.LabelCap, null);
				}
			}
			if (!Find.World.Impassable(tile))
			{
				StringBuilder stringBuilder = new StringBuilder();
				PlanetTile tile2 = tile;
				bool perceivedStatic = false;
				StringBuilder explanation = stringBuilder;
				string rightLabel = (WorldPathGrid.CalculatedMovementDifficultyAt(tile2, perceivedStatic, null, explanation) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(tile, PlanetTile.Invalid, stringBuilder)).ToString("0.#");
				if (WorldPathGrid.WillWinterEverAffectMovementDifficulty(tile) && WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(tile, null, null) < 2f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append(" (");
					stringBuilder.Append("MovementDifficultyOffsetInWinter".Translate(string.Format("+{0:0.#}", 2f)));
					stringBuilder.Append(")");
				}
				listing.LabelDouble("MovementDifficulty".Translate(), rightLabel, stringBuilder.ToString());
			}
			if (ws.PrimaryBiome.canBuildBase)
			{
				listing.LabelDouble("StoneTypesHere".Translate(), (from rt in Find.World.NaturalRockTypesIn(tile)
				select rt.label).ToCommaList(true, false).CapitalizeFirst(), null);
			}
			listing.LabelDouble("Elevation".Translate(), ws.Layer.Def.elevationString.Formatted(ws.elevation.ToString("F0")), null);
			if (ModsConfig.OdysseyActive && ws.Landmark != null)
			{
				listing.LabelDouble("Landmark".Translate(), ws.Landmark.name, ws.Landmark.def.description);
			}
			if (ws.Mutators.Any<TileMutatorDef>())
			{
				IOrderedEnumerable<TileMutatorDef> source = from m in ws.Mutators
				orderby -m.displayPriority
				select m;
				listing.LabelDouble("TileMutators".Translate(), (from m in source
				select m.Label(tile)).ToCommaList(false, false).CapitalizeFirst(), (from m in source
				select m.Label(tile).Colorize(ColoredText.TipSectionTitleColor).CapitalizeFirst() + "\n" + m.Description(tile)).ToStringList("\n\n"));
			}
		}

		// Token: 0x0400F29E RID: 62110
		private Vector2 scrollPosition;

		// Token: 0x0400F29F RID: 62111
		private float lastDrawnHeight;

		// Token: 0x0400F2A0 RID: 62112
		private static string cachedGrowingQuadrumsDescription;

		// Token: 0x0400F2A1 RID: 62113
		private static PlanetTile cachedGrowingQuadrumsTile;

		// Token: 0x0400F2A2 RID: 62114
		private static readonly Vector2 WinSize = new Vector2(440f, 540f);
	}
}
