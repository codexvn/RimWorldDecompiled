using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003354 RID: 13140
	public class GlobalControls
	{
		// Token: 0x06012777 RID: 75639 RVA: 0x005721D0 File Offset: 0x005703D0
		public void GlobalControlsOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			float num = (float)UI.screenWidth - 200f;
			float num2 = (float)UI.screenHeight;
			num2 -= 35f;
			GenUI.DrawTextWinterShadow(new Rect((float)(UI.screenWidth - 270), (float)(UI.screenHeight - 450), 270f, 450f));
			num2 -= 4f;
			GlobalControlsUtility.DoPlaySettings(this.rowVisibility, false, ref num2);
			num2 -= 4f;
			GlobalControlsUtility.DoTimespeedControls(num, 200f, ref num2);
			num2 -= 4f;
			GlobalControlsUtility.DoDate(num, 200f, ref num2);
			if (!Find.CurrentMap.IsPocketMap)
			{
				Rect rect = new Rect(num - 22f, num2 - 26f, 230f, 26f);
				Find.CurrentMap.weatherManager.DoWeatherGUI(rect);
				num2 -= rect.height;
			}
			Rect rect2 = new Rect(num - 100f, num2 - 26f, 293f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, GlobalControls.TemperatureString());
			Text.Anchor = TextAnchor.UpperLeft;
			num2 -= 26f;
			float num3 = 154f;
			float num4 = Find.CurrentMap.gameConditionManager.TotalHeightAt(num3);
			Rect rect3 = new Rect((float)UI.screenWidth - num3, num2 - num4, num3, num4);
			Find.CurrentMap.gameConditionManager.DoConditionsUI(rect3);
			num2 -= rect3.height;
			if (DebugViewSettings.showMemoryInfo)
			{
				GlobalControlsUtility.DrawMemoryInfo(num, 200f, ref num2);
			}
			if (DebugViewSettings.showTpsCounter)
			{
				GlobalControlsUtility.DrawTpsCounter(num, 200f, ref num2);
			}
			if (DebugViewSettings.showFpsCounter)
			{
				GlobalControlsUtility.DrawFpsCounter(num, 200f, ref num2);
			}
			if (Prefs.ShowRealtimeClock)
			{
				GlobalControlsUtility.DoRealtimeClock(num, 200f, ref num2);
			}
			MapParent parent = Find.CurrentMap.Parent;
			TimedDetectionRaids timedDetectionRaids = (parent != null) ? parent.GetComponent<TimedDetectionRaids>() : null;
			if (timedDetectionRaids != null && timedDetectionRaids.NextRaidCountdownActiveAndVisible)
			{
				Rect rect4 = new Rect(num, num2 - 26f, 193f, 26f);
				Text.Anchor = TextAnchor.MiddleRight;
				GlobalControls.DoCountdownTimer(rect4, timedDetectionRaids);
				Text.Anchor = TextAnchor.UpperLeft;
				num2 -= 26f;
			}
			num2 -= 10f;
			Find.LetterStack.LettersOnGUI(num2);
		}

		// Token: 0x06012778 RID: 75640 RVA: 0x005723F8 File Offset: 0x005705F8
		private static string TemperatureString()
		{
			IntVec3 intVec = UI.MouseCell();
			IntVec3 c = intVec;
			Room room = intVec.GetRoom(Find.CurrentMap);
			if (room == null)
			{
				for (int i = 0; i < 9; i++)
				{
					IntVec3 intVec2 = intVec + GenAdj.AdjacentCellsAndInside[i];
					if (intVec2.InBounds(Find.CurrentMap))
					{
						Room room2 = intVec2.GetRoom(Find.CurrentMap);
						if (room2 != null && ((!room2.PsychologicallyOutdoors && !room2.UsesOutdoorTemperature) || (!room2.PsychologicallyOutdoors && (room == null || room.PsychologicallyOutdoors)) || (room2.PsychologicallyOutdoors && room == null)))
						{
							c = intVec2;
							room = room2;
						}
					}
				}
			}
			if (room == null && intVec.InBounds(Find.CurrentMap))
			{
				Building edifice = intVec.GetEdifice(Find.CurrentMap);
				if (edifice != null)
				{
					foreach (IntVec3 intVec3 in edifice.OccupiedRect().ExpandedBy(1).ClipInsideMap(Find.CurrentMap))
					{
						room = intVec3.GetRoom(Find.CurrentMap);
						if (room != null && !room.PsychologicallyOutdoors)
						{
							c = intVec3;
							break;
						}
					}
				}
			}
			string text;
			if (c.InBounds(Find.CurrentMap) && !c.Fogged(Find.CurrentMap) && room != null && !room.PsychologicallyOutdoors)
			{
				if (room.OpenRoofCount == 0)
				{
					text = "Indoors".Translate();
				}
				else
				{
					if (GlobalControls.indoorsUnroofedStringCachedRoofCount != room.OpenRoofCount)
					{
						GlobalControls.indoorsUnroofedStringCached = "IndoorsUnroofed".Translate() + " (" + room.OpenRoofCount.ToStringCached() + ")";
						GlobalControls.indoorsUnroofedStringCachedRoofCount = room.OpenRoofCount;
					}
					text = GlobalControls.indoorsUnroofedStringCached;
				}
			}
			else
			{
				text = "Outdoors".Translate().CapitalizeFirst();
			}
			float num = (room == null || c.Fogged(Find.CurrentMap)) ? Find.CurrentMap.mapTemperature.OutdoorTemp : room.Temperature;
			int num2 = Mathf.RoundToInt(GenTemperature.CelsiusTo(GlobalControls.cachedTemperatureStringForTemperature, Prefs.TemperatureMode));
			int num3 = Mathf.RoundToInt(GenTemperature.CelsiusTo(num, Prefs.TemperatureMode));
			if (GlobalControls.cachedTemperatureStringForLabel != text || num2 != num3 || GlobalControls.cachedTemperatureDisplayMode != Prefs.TemperatureMode)
			{
				GlobalControls.cachedTemperatureStringForLabel = text;
				GlobalControls.cachedTemperatureStringForTemperature = num;
				GlobalControls.cachedTemperatureString = text + " " + num.ToStringTemperature("F0");
				GlobalControls.cachedTemperatureDisplayMode = Prefs.TemperatureMode;
			}
			return GlobalControls.cachedTemperatureString;
		}

		// Token: 0x06012779 RID: 75641 RVA: 0x00572694 File Offset: 0x00570894
		private static void DoCountdownTimer(Rect rect, TimedDetectionRaids timedForcedExit)
		{
			string detectionCountdownTimeLeftString = timedForcedExit.DetectionCountdownTimeLeftString;
			string text = "CaravanDetectedRaidCountdown".Translate(detectionCountdownTimeLeftString);
			float x = Text.CalcSize(text).x;
			Rect rect2 = new Rect(rect.xMax - x, rect.y, x, rect.height);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			TooltipHandler.TipRegionByKey(rect2, "CaravanDetectedRaidCountdownTip", detectionCountdownTimeLeftString);
			Widgets.Label(rect2, text);
		}

		// Token: 0x0400BFD6 RID: 49110
		public const float Width = 200f;

		// Token: 0x0400BFD7 RID: 49111
		private WidgetRow rowVisibility = new WidgetRow();

		// Token: 0x0400BFD8 RID: 49112
		private static string indoorsUnroofedStringCached;

		// Token: 0x0400BFD9 RID: 49113
		private static int indoorsUnroofedStringCachedRoofCount = -1;

		// Token: 0x0400BFDA RID: 49114
		private static string cachedTemperatureString;

		// Token: 0x0400BFDB RID: 49115
		private static string cachedTemperatureStringForLabel;

		// Token: 0x0400BFDC RID: 49116
		private static float cachedTemperatureStringForTemperature;

		// Token: 0x0400BFDD RID: 49117
		private static TemperatureDisplayMode cachedTemperatureDisplayMode;
	}
}
