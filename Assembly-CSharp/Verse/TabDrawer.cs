using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000C08 RID: 3080
	public static class TabDrawer
	{
		// Token: 0x06004E96 RID: 20118 RVA: 0x001901A4 File Offset: 0x0018E3A4
		public static TabRecord DrawTabs<T>(Rect baseRect, List<T> tabs, int rows, float? maxTabWidth) where T : TabRecord
		{
			if (rows <= 1)
			{
				return TabDrawer.DrawTabs<T>(baseRect, tabs, 200f);
			}
			int num = Mathf.FloorToInt((float)tabs.Count / (float)rows);
			int num2 = 0;
			TabRecord result = null;
			Rect rect = baseRect;
			baseRect.yMin -= (float)(rows - 1) * 31f;
			Rect rect2 = baseRect;
			rect2.yMax = rect.y;
			Widgets.DrawMenuSection(rect2);
			for (int i = 0; i < rows; i++)
			{
				int num3 = (i == 0) ? (tabs.Count - (rows - 1) * num) : num;
				TabDrawer.tmpTabs.Clear();
				for (int j = num2; j < num2 + num3; j++)
				{
					TabDrawer.tmpTabs.Add(tabs[j]);
				}
				TabRecord tabRecord = TabDrawer.DrawTabs<TabRecord>(baseRect, TabDrawer.tmpTabs, maxTabWidth ?? baseRect.width);
				if (tabRecord != null)
				{
					result = tabRecord;
				}
				baseRect.yMin += 31f;
				num2 += num3;
			}
			TabDrawer.tmpTabs.Clear();
			return result;
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x001902C0 File Offset: 0x0018E4C0
		public static float GetOverflowTabHeight<T>(Rect baseRect, List<T> tabs, float minTabWidth, float maxTabWidth) where T : TabRecord
		{
			int num = Mathf.CeilToInt((float)tabs.Count * minTabWidth / baseRect.width);
			if (num <= 1)
			{
				return 32f;
			}
			return 32f * (float)num - (float)num;
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x001902FC File Offset: 0x0018E4FC
		public static TabRecord DrawTabsOverflow<T>(Rect baseRect, List<T> tabs, float minTabWidth, float maxTabWidth) where T : TabRecord
		{
			int num = Mathf.CeilToInt((float)tabs.Count * minTabWidth / baseRect.width);
			if (num <= 1)
			{
				baseRect.y += 32f;
				T t = TabDrawer.DrawTabs<T>(baseRect, tabs, maxTabWidth);
				baseRect.yMax = baseRect.y;
				return t;
			}
			baseRect.height = 64f;
			int num2 = Mathf.FloorToInt((float)tabs.Count / (float)num);
			int num3 = 0;
			TabRecord result = null;
			for (int i = 0; i < num; i++)
			{
				int num4 = Mathf.Min(tabs.Count - num3, num2);
				if (tabs.Count - num3 - num4 == 1)
				{
					baseRect.xMax += baseRect.width / (float)num2;
					num4++;
				}
				int num5 = num3;
				baseRect.y += 31f;
				TabDrawer.tmpTabs.Clear();
				for (int j = num3; j < num5 + num4; j++)
				{
					TabDrawer.tmpTabs.Add(tabs[j]);
					num3++;
				}
				TabRecord tabRecord = TabDrawer.DrawTabs<TabRecord>(baseRect, TabDrawer.tmpTabs, baseRect.width);
				if (tabRecord != null)
				{
					result = tabRecord;
				}
			}
			TabDrawer.tmpTabs.Clear();
			return result;
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x0019043C File Offset: 0x0018E63C
		public static TTabRecord DrawTabs<TTabRecord>(Rect baseRect, List<TTabRecord> tabs, float maxTabWidth = 200f) where TTabRecord : TabRecord
		{
			TTabRecord ttabRecord = default(TTabRecord);
			TTabRecord ttabRecord2 = tabs.Find((TTabRecord t) => t.Selected);
			float num = baseRect.width + (float)(tabs.Count - 1) * 10f;
			float tabWidth = num / (float)tabs.Count;
			if (tabWidth > maxTabWidth)
			{
				tabWidth = maxTabWidth;
			}
			Rect rect = new Rect(baseRect);
			rect.y -= 32f;
			rect.height = 9999f;
			Widgets.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Small;
			Func<TTabRecord, Rect> func = (TTabRecord tab) => new Rect((float)tabs.IndexOf(tab) * (tabWidth - 10f), 1f, tabWidth, 32f);
			List<TTabRecord> list = tabs.ListFullCopy<TTabRecord>();
			if (ttabRecord2 != null)
			{
				list.Remove(ttabRecord2);
				list.Add(ttabRecord2);
			}
			TabRecord tabRecord = null;
			List<TTabRecord> list2 = list.ListFullCopy<TTabRecord>();
			list2.Reverse();
			for (int i = 0; i < list2.Count; i++)
			{
				TTabRecord ttabRecord3 = list2[i];
				Rect rect2 = func(ttabRecord3);
				if (tabRecord == null && Mouse.IsOver(rect2))
				{
					tabRecord = ttabRecord3;
				}
				MouseoverSounds.DoRegion(rect2, SoundDefOf.Mouseover_Tab);
				if (Mouse.IsOver(rect2) && !ttabRecord3.GetTip().NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect2, ttabRecord3.GetTip());
				}
				if (Widgets.ButtonInvisible(rect2, true))
				{
					ttabRecord = ttabRecord3;
				}
			}
			foreach (TTabRecord ttabRecord4 in list)
			{
				Rect rect3 = func(ttabRecord4);
				ttabRecord4.Draw(rect3);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.EndGroup();
			if (ttabRecord != null && ttabRecord != ttabRecord2)
			{
				SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
				if (ttabRecord.clickedAction != null)
				{
					ttabRecord.clickedAction();
				}
			}
			return ttabRecord;
		}

		// Token: 0x040037E9 RID: 14313
		private const float MaxTabWidth = 200f;

		// Token: 0x040037EA RID: 14314
		public const float TabHeight = 32f;

		// Token: 0x040037EB RID: 14315
		public const float TabHoriztonalOverlap = 10f;

		// Token: 0x040037EC RID: 14316
		private static readonly List<TabRecord> tmpTabs = new List<TabRecord>();
	}
}
