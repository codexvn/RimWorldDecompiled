using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003353 RID: 13139
	public static class DateReadout
	{
		// Token: 0x17003022 RID: 12322
		// (get) Token: 0x06012772 RID: 75634 RVA: 0x00571CD7 File Offset: 0x0056FED7
		public static float Height
		{
			get
			{
				return (float)(48 + (DateReadout.SeasonLabelVisible ? 26 : 0));
			}
		}

		// Token: 0x17003023 RID: 12323
		// (get) Token: 0x06012773 RID: 75635 RVA: 0x00571CE9 File Offset: 0x0056FEE9
		private static bool SeasonLabelVisible
		{
			get
			{
				return !WorldRendererUtility.WorldSelected && Find.CurrentMap != null;
			}
		}

		// Token: 0x06012774 RID: 75636 RVA: 0x00571CFC File Offset: 0x0056FEFC
		static DateReadout()
		{
			DateReadout.Reset();
		}

		// Token: 0x06012775 RID: 75637 RVA: 0x00571D3C File Offset: 0x0056FF3C
		public static void Reset()
		{
			DateReadout.dateString = null;
			DateReadout.dateStringDay = -1;
			DateReadout.dateStringSeason = Season.Undefined;
			DateReadout.dateStringQuadrum = Quadrum.Undefined;
			DateReadout.dateStringYear = -1;
			DateReadout.fastHourStrings24h.Clear();
			for (int i = 0; i < 24; i++)
			{
				DateReadout.fastHourStrings24h.Add(i.ToString() + "LetterHour".Translate());
			}
			DateReadout.fastHourStrings12h.Clear();
			for (int j = 0; j < 24; j++)
			{
				TaggedString taggedString = (j >= 12) ? "PM".Translate() : "AM".Translate();
				string item = (j == 0) ? string.Format("12 {0}", taggedString) : ((j > 12) ? string.Format("{0} {1}", j - 12, taggedString) : string.Format("{0} {1}", j, taggedString));
				DateReadout.fastHourStrings12h.Add(item);
			}
			DateReadout.seasonsCached.Clear();
			int length = Enum.GetValues(typeof(Season)).Length;
			for (int k = 0; k < length; k++)
			{
				Season season = (Season)k;
				DateReadout.seasonsCached.Add((season == Season.Undefined) ? "" : season.LabelCap());
			}
		}

		// Token: 0x06012776 RID: 75638 RVA: 0x00571E80 File Offset: 0x00570080
		public static void DateOnGUI(Rect dateRect)
		{
			Vector2 vector;
			if (WorldRendererUtility.WorldSelected && Find.WorldSelector.SelectedTile.Valid)
			{
				vector = Find.WorldGrid.LongLatOf(Find.WorldSelector.SelectedTile);
			}
			else if (WorldRendererUtility.WorldSelected && Find.WorldSelector.NumSelectedObjects > 0)
			{
				vector = Find.WorldGrid.LongLatOf(Find.WorldSelector.FirstSelectedObject.Tile);
			}
			else
			{
				if (Find.CurrentMap == null)
				{
					return;
				}
				vector = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
			}
			int index = GenDate.HourInteger((long)Find.TickManager.TicksAbs, vector.x);
			int num = GenDate.DayOfTwelfth((long)Find.TickManager.TicksAbs, vector.x);
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, vector);
			Quadrum quadrum = GenDate.Quadrum((long)Find.TickManager.TicksAbs, vector.x);
			int num2 = GenDate.Year((long)Find.TickManager.TicksAbs, vector.x);
			string text = DateReadout.SeasonLabelVisible ? DateReadout.seasonsCached[(int)season] : "";
			if (num != DateReadout.dateStringDay || season != DateReadout.dateStringSeason || quadrum != DateReadout.dateStringQuadrum || num2 != DateReadout.dateStringYear)
			{
				DateReadout.dateString = GenDate.DateReadoutStringAt((long)Find.TickManager.TicksAbs, vector);
				DateReadout.dateStringDay = num;
				DateReadout.dateStringSeason = season;
				DateReadout.dateStringQuadrum = quadrum;
				DateReadout.dateStringYear = num2;
			}
			Text.Font = GameFont.Small;
			float num3 = Mathf.Max(Mathf.Max(Text.CalcSize(Prefs.TwelveHourClockMode ? DateReadout.fastHourStrings12h[index] : DateReadout.fastHourStrings24h[index]).x, Text.CalcSize(DateReadout.dateString).x), Text.CalcSize(text).x) + 7f;
			dateRect.xMin = dateRect.xMax - num3;
			if (Mouse.IsOver(dateRect))
			{
				Widgets.DrawHighlight(dateRect);
			}
			Widgets.BeginGroup(dateRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = dateRect.AtZero();
			rect.xMax -= 7f;
			if (Prefs.TwelveHourClockMode)
			{
				Widgets.Label(rect, DateReadout.fastHourStrings12h[index]);
			}
			else
			{
				Widgets.Label(rect, DateReadout.fastHourStrings24h[index]);
			}
			rect.yMin += 26f;
			Widgets.Label(rect, DateReadout.dateString);
			rect.yMin += 26f;
			if (!text.NullOrEmpty())
			{
				Widgets.Label(rect, text);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.EndGroup();
			if (Mouse.IsOver(dateRect))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum2 = (Quadrum)i;
					stringBuilder.AppendLine(quadrum2.Label() + " - " + quadrum2.GetSeason(vector.y).LabelCap());
				}
				TaggedString taggedString = "DateReadoutTip".Translate(GenDate.DaysPassed, 15, season.LabelCap(), 15, GenDate.Quadrum((long)GenTicks.TicksAbs, vector.x).Label(), stringBuilder.ToString());
				TooltipHandler.TipRegion(dateRect, new TipSignal(taggedString, 86423));
			}
		}

		// Token: 0x0400BFCD RID: 49101
		private static string dateString;

		// Token: 0x0400BFCE RID: 49102
		private static int dateStringDay = -1;

		// Token: 0x0400BFCF RID: 49103
		private static Season dateStringSeason = Season.Undefined;

		// Token: 0x0400BFD0 RID: 49104
		private static Quadrum dateStringQuadrum = Quadrum.Undefined;

		// Token: 0x0400BFD1 RID: 49105
		private static int dateStringYear = -1;

		// Token: 0x0400BFD2 RID: 49106
		private static readonly List<string> fastHourStrings24h = new List<string>();

		// Token: 0x0400BFD3 RID: 49107
		private static readonly List<string> fastHourStrings12h = new List<string>();

		// Token: 0x0400BFD4 RID: 49108
		private static readonly List<string> seasonsCached = new List<string>();

		// Token: 0x0400BFD5 RID: 49109
		private const float DateRightPadding = 7f;
	}
}
