using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E6F RID: 15983
	public class WITab_Orbit : WITab
	{
		// Token: 0x170036A0 RID: 13984
		// (get) Token: 0x06015DD4 RID: 89556 RVA: 0x0068A03C File Offset: 0x0068823C
		public override bool IsVisible
		{
			get
			{
				return base.SelPlanetTile.Valid && base.SelPlanetTile.LayerDef == PlanetLayerDefOf.Orbit;
			}
		}

		// Token: 0x06015DD5 RID: 89557 RVA: 0x0068A070 File Offset: 0x00688270
		public WITab_Orbit()
		{
			this.size = WITab_Orbit.WinSize;
			this.labelKey = "TabOrbit";
			this.tutorTag = "Orbit";
		}

		// Token: 0x06015DD6 RID: 89558 RVA: 0x0068A09C File Offset: 0x0068829C
		protected override void FillTab()
		{
			Rect outRect = new Rect(0f, 0f, WITab_Orbit.WinSize.x, WITab_Orbit.WinSize.y).ContractedBy(10f);
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

		// Token: 0x06015DD7 RID: 89559 RVA: 0x0068A184 File Offset: 0x00688384
		private void DrawScrollContents(Listing_Standard listing, Rect infoRect)
		{
			Tile selTile = base.SelTile;
			listing.Label(selTile.PrimaryBiome.description, -1f, null);
			listing.Gap(8f);
			listing.GapLine(12f);
			if (!selTile.PrimaryBiome.implemented)
			{
				listing.Label(string.Format("{0} {1}", selTile.PrimaryBiome.LabelCap, "BiomeNotImplemented".Translate()), -1f, null);
			}
			WITab_Orbit.ListOrbitalDetails(listing, selTile, base.SelPlanetTile);
			listing.GapLine(12f);
			WITab_Orbit.ListMiscDetails(listing, selTile, base.SelPlanetTile);
			this.lastDrawnHeight = infoRect.y + listing.CurHeight;
		}

		// Token: 0x06015DD8 RID: 89560 RVA: 0x0068A254 File Offset: 0x00688454
		private static void ListOrbitalDetails(Listing_Standard listing, Tile ws, PlanetTile tile)
		{
			listing.LabelDouble("Elevation".Translate(), ws.Layer.Def.elevationString.Formatted(ws.elevation.ToString("F0")), null);
			listing.LabelDouble("AvgTemp".Translate(), GenTemperature.GetAverageTemperatureLabel(tile), null);
		}

		// Token: 0x06015DD9 RID: 89561 RVA: 0x0068A2C4 File Offset: 0x006884C4
		private static void ListMiscDetails(Listing_Standard listing, Tile ws, PlanetTile tile)
		{
			listing.LabelDouble("TimeZone".Translate(), GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(tile).x).ToStringWithSign(), null);
			if (Prefs.DevMode)
			{
				listing.LabelDouble("Debug world tile ID", tile.ToString(), null);
			}
		}

		// Token: 0x0400F29A RID: 62106
		private Vector2 scrollPosition;

		// Token: 0x0400F29B RID: 62107
		private float lastDrawnHeight;

		// Token: 0x0400F29C RID: 62108
		private static readonly Vector2 WinSize = new Vector2(440f, 540f);
	}
}
