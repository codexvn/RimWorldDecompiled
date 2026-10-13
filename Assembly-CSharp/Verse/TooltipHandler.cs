using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000BDE RID: 3038
	public static class TooltipHandler
	{
		// Token: 0x06004DC3 RID: 19907 RVA: 0x0018B31C File Offset: 0x0018951C
		public static void ClearTooltipsFrom(Rect rect)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.dyingTips.Clear();
				foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
				{
					if (keyValuePair.Value.lastTriggerFrame == TooltipHandler.frame)
					{
						TooltipHandler.dyingTips.Add(keyValuePair.Key);
					}
				}
				for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
				{
					TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
				}
			}
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x0018B3DC File Offset: 0x001895DC
		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x0018B3EB File Offset: 0x001895EB
		public static void TipRegionByKey(Rect rect, string key)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate());
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x0018B40E File Offset: 0x0018960E
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1));
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x0018B432 File Offset: 0x00189632
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2));
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x0018B457 File Offset: 0x00189657
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2, arg3));
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x0018B480 File Offset: 0x00189680
		public static void TipRegion(Rect rect, TipSignal tip)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			if (tip.textGetter == null && tip.text.NullOrEmpty())
			{
				return;
			}
			if (SteamDeck.KeyboardShowing)
			{
				return;
			}
			if (DebugViewSettings.drawTooltipEdges)
			{
				Widgets.DrawBox(rect, 1, null);
			}
			if (!TooltipHandler.activeTips.ContainsKey(tip.uniqueId))
			{
				ActiveTip value = new ActiveTip(tip);
				TooltipHandler.activeTips.Add(tip.uniqueId, value);
				TooltipHandler.activeTips[tip.uniqueId].firstTriggerTime = (double)Time.realtimeSinceStartup;
			}
			TooltipHandler.activeTips[tip.uniqueId].lastTriggerFrame = TooltipHandler.frame;
			TooltipHandler.activeTips[tip.uniqueId].signal.text = tip.text;
			TooltipHandler.activeTips[tip.uniqueId].signal.textGetter = tip.textGetter;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x0018B577 File Offset: 0x00189777
		public static void DoTooltipGUI()
		{
			if (CellInspectorDrawer.active)
			{
				return;
			}
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.Repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x0018B5A4 File Offset: 0x001897A4
		private static void DrawActiveTips()
		{
			if (TooltipHandler.activeTips.Count == 0)
			{
				return;
			}
			TooltipHandler.drawingTips.Clear();
			foreach (ActiveTip activeTip in TooltipHandler.activeTips.Values)
			{
				if ((double)Time.realtimeSinceStartup > activeTip.firstTriggerTime + (double)activeTip.signal.delay)
				{
					TooltipHandler.drawingTips.Add(activeTip);
				}
			}
			if (TooltipHandler.drawingTips.Any<ActiveTip>())
			{
				TooltipHandler.drawingTips.SortStable(TooltipHandler.compareTooltipsByPriorityCached);
				Vector2 pos = TooltipHandler.CalculateInitialTipPosition(TooltipHandler.drawingTips);
				for (int i = 0; i < TooltipHandler.drawingTips.Count; i++)
				{
					pos.y += TooltipHandler.drawingTips[i].DrawTooltip(pos);
					pos.y += 2f;
				}
				TooltipHandler.drawingTips.Clear();
			}
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x0018B6A4 File Offset: 0x001898A4
		private static void CleanActiveTooltips()
		{
			TooltipHandler.dyingTips.Clear();
			foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
			{
				if (keyValuePair.Value.lastTriggerFrame != TooltipHandler.frame)
				{
					TooltipHandler.dyingTips.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
			{
				TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
			}
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x0018B748 File Offset: 0x00189948
		private static Vector2 CalculateInitialTipPosition(List<ActiveTip> drawingTips)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < drawingTips.Count; i++)
			{
				Rect tipRect = drawingTips[i].TipRect;
				num += tipRect.height;
				num2 = Mathf.Max(num2, tipRect.width);
				if (i != drawingTips.Count - 1)
				{
					num += 2f;
				}
			}
			return GenUI.GetMouseAttachedWindowPos(num2, num);
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x0018B7B4 File Offset: 0x001899B4
		private static int CompareTooltipsByPriority(ActiveTip A, ActiveTip B)
		{
			int num = (int)(-(int)A.signal.priority);
			int value = (int)(-(int)B.signal.priority);
			return num.CompareTo(value);
		}

		// Token: 0x04003738 RID: 14136
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		// Token: 0x04003739 RID: 14137
		private static int frame = 0;

		// Token: 0x0400373A RID: 14138
		private static List<int> dyingTips = new List<int>(32);

		// Token: 0x0400373B RID: 14139
		private const float SpaceBetweenTooltips = 2f;

		// Token: 0x0400373C RID: 14140
		private static List<ActiveTip> drawingTips = new List<ActiveTip>();

		// Token: 0x0400373D RID: 14141
		private static Func<ActiveTip, ActiveTip, int> compareTooltipsByPriorityCached = new Func<ActiveTip, ActiveTip, int>(TooltipHandler.CompareTooltipsByPriority);
	}
}
