using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B77 RID: 2935
	public static class EnvironmentStatsDrawer
	{
		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06004B41 RID: 19265 RVA: 0x0017F470 File Offset: 0x0017D670
		private static int DisplayedRoomStatsCount
		{
			get
			{
				int num = 0;
				List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!allDefsListForReading[i].isHidden || DebugViewSettings.showAllRoomStats)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x0017F4B0 File Offset: 0x0017D6B0
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x0017F4CC File Offset: 0x0017D6CC
		private static bool ShouldShowRoomStats()
		{
			if (!Find.PlaySettings.showRoomStats)
			{
				return false;
			}
			if (Find.CurrentMap == null)
			{
				return false;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap) || UI.MouseCell().Fogged(Find.CurrentMap))
			{
				return false;
			}
			Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
			return room != null && room.Role != RoomRoleDefOf.None;
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x0017F53C File Offset: 0x0017D73C
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap) != null;
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x0017F58D File Offset: 0x0017D78D
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				return;
			}
			EnvironmentStatsDrawer.DrawInfoWindow();
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x0017F5AC File Offset: 0x0017D7AC
		private static void DrawInfoWindow()
		{
			Text.Font = GameFont.Small;
			Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(EnvironmentStatsDrawer.ShouldShowBeauty(), EnvironmentStatsDrawer.ShouldShowRoomStats());
			Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.GameUI, delegate
			{
				EnvironmentStatsDrawer.FillWindow(windowRect);
			}, true, false, 1f, null, false);
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x0017F608 File Offset: 0x0017D808
		public static Rect GetWindowRect(bool shouldShowBeauty, bool shouldShowRoomStats)
		{
			Rect result = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 424f, 24f);
			int num = 0;
			if (shouldShowBeauty)
			{
				num++;
				result.height += 25f;
			}
			if (shouldShowRoomStats)
			{
				num++;
				result.height += 23f;
				result.height += (float)EnvironmentStatsDrawer.DisplayedRoomStatsCount * 25f + 23f;
			}
			result.height += 13f * (float)(num - 1);
			result.x += 26f;
			result.y += 26f;
			if (result.xMax > (float)UI.screenWidth)
			{
				result.x -= result.width + 52f;
			}
			if (result.yMax > (float)UI.screenHeight)
			{
				result.y -= result.height + 52f;
			}
			return result;
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x0017F730 File Offset: 0x0017D930
		private static void FillWindow(Rect windowRect)
		{
			EnvironmentStatsDrawer.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.windowRect = windowRect;
			Text.Font = GameFont.Small;
			CS$<>8__locals1.curY = 12f;
			CS$<>8__locals1.dividingLinesSeen = 0;
			if (EnvironmentStatsDrawer.ShouldShowBeauty())
			{
				EnvironmentStatsDrawer.<FillWindow>g__DrawDividingLineIfNecessary|21_0(ref CS$<>8__locals1);
				float beauty = BeautyUtility.AverageBeautyPerceptible(UI.MouseCell(), Find.CurrentMap);
				Rect rect = new Rect(22f, CS$<>8__locals1.curY, CS$<>8__locals1.windowRect.width - 24f - 10f, 100f);
				GUI.color = BeautyDrawer.BeautyColor(beauty, 40f);
				Widgets.Label(rect, "BeautyHere".Translate() + ": " + beauty.ToString("F1"));
				CS$<>8__locals1.curY += 25f;
			}
			if (EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				EnvironmentStatsDrawer.<FillWindow>g__DrawDividingLineIfNecessary|21_0(ref CS$<>8__locals1);
				EnvironmentStatsDrawer.DoRoomInfo(UI.MouseCell().GetRoom(Find.CurrentMap), ref CS$<>8__locals1.curY, CS$<>8__locals1.windowRect);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x0017F834 File Offset: 0x0017DA34
		public static void DrawRoomOverlays()
		{
			if (Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap))
			{
				GenUI.RenderMouseoverBracket();
			}
			if (EnvironmentStatsDrawer.ShouldShowWindowNow() && EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
				if (room != null && room.Role != RoomRoleDefOf.None)
				{
					room.DrawFieldEdges();
				}
			}
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x0017F898 File Offset: 0x0017DA98
		public static void DoRoomInfo(Room room, ref float curY, Rect windowRect)
		{
			Rect rect = new Rect(12f, curY, windowRect.width - 24f, 100f);
			GUI.color = Color.white;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x + 10f, curY, rect.width - 10f, rect.height), room.GetRoomRoleLabel().CapitalizeFirst());
			curY += 30f;
			Text.Font = GameFont.Small;
			Text.WordWrap = false;
			int num = 0;
			bool flag = false;
			for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
			{
				RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
				if (!roomStatDef.isHidden || DebugViewSettings.showAllRoomStats)
				{
					float stat = room.GetStat(roomStatDef);
					RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
					GUI.color = Color.white;
					Rect rect2 = new Rect(rect.x, curY, rect.width, 23f);
					if (num % 2 == 1)
					{
						Widgets.DrawLightHighlight(rect2);
					}
					Rect rect3 = new Rect(rect.x, curY, 10f, 23f);
					if (room.Role.IsStatRelated(roomStatDef))
					{
						flag = true;
						Widgets.Label(rect3, "*");
						GUI.color = EnvironmentStatsDrawer.RelatedStatColor;
					}
					else
					{
						GUI.color = EnvironmentStatsDrawer.UnrelatedStatColor;
					}
					Rect rect4 = new Rect(rect3.xMax, curY, 110f, 23f);
					Widgets.Label(rect4, roomStatDef.LabelCap);
					Rect rect5 = new Rect(rect4.xMax + 35f, curY, 50f, 23f);
					string label = roomStatDef.ScoreToString(stat);
					Widgets.Label(rect5, label);
					Widgets.Label(new Rect(rect5.xMax + 35f, curY, 160f, 23f), (scoreStage == null) ? "" : scoreStage.label.CapitalizeFirst());
					curY += 25f;
					num++;
				}
			}
			if (flag)
			{
				GUI.color = Color.grey;
				Text.Font = GameFont.Tiny;
				Widgets.Label(new Rect(rect.x, curY, rect.width, 23f), "* " + "StatRelatesToCurrentRoom".Translate());
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			Text.WordWrap = true;
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x0017FB1C File Offset: 0x0017DD1C
		[CompilerGenerated]
		internal static void <FillWindow>g__DrawDividingLineIfNecessary|21_0(ref EnvironmentStatsDrawer.<>c__DisplayClass21_0 A_0)
		{
			int dividingLinesSeen = A_0.dividingLinesSeen;
			A_0.dividingLinesSeen = dividingLinesSeen + 1;
			if (A_0.dividingLinesSeen <= 1)
			{
				return;
			}
			A_0.curY += 5f;
			GUI.color = new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(12f, A_0.curY, A_0.windowRect.width - 24f);
			GUI.color = Color.white;
			A_0.curY += 8f;
		}

		// Token: 0x0400358F RID: 13711
		private const float StatLabelColumnWidth = 110f;

		// Token: 0x04003590 RID: 13712
		private const float StatGutterColumnWidth = 10f;

		// Token: 0x04003591 RID: 13713
		private const float ScoreColumnWidth = 50f;

		// Token: 0x04003592 RID: 13714
		private const float ScoreStageLabelColumnWidth = 160f;

		// Token: 0x04003593 RID: 13715
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		// Token: 0x04003594 RID: 13716
		private static readonly Color UnrelatedStatColor = Color.gray;

		// Token: 0x04003595 RID: 13717
		private const float DistFromMouse = 26f;

		// Token: 0x04003596 RID: 13718
		public const float WindowPadding = 12f;

		// Token: 0x04003597 RID: 13719
		private const float LineHeight = 23f;

		// Token: 0x04003598 RID: 13720
		private const float FootnoteHeight = 23f;

		// Token: 0x04003599 RID: 13721
		private const float TitleHeight = 30f;

		// Token: 0x0400359A RID: 13722
		private const float SpaceBetweenLines = 2f;

		// Token: 0x0400359B RID: 13723
		private const float SpaceBetweenColumns = 35f;
	}
}
