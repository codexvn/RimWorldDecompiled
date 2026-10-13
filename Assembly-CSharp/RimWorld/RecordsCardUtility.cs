using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003235 RID: 12853
	public static class RecordsCardUtility
	{
		// Token: 0x17002EBE RID: 11966
		// (get) Token: 0x0601210F RID: 73999 RVA: 0x005486B4 File Offset: 0x005468B4
		private static List<RecordDef> AllRecordsSorted
		{
			get
			{
				if (RecordsCardUtility.allRecordsSorted == null)
				{
					RecordsCardUtility.allRecordsSorted = (from x in DefDatabase<RecordDef>.AllDefsListForReading
					orderby x.displayOrder
					select x).ToList<RecordDef>();
				}
				return RecordsCardUtility.allRecordsSorted;
			}
		}

		// Token: 0x06012110 RID: 74000 RVA: 0x00548700 File Offset: 0x00546900
		public static void DrawRecordsCard(Rect rect, Pawn pawn)
		{
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, RecordsCardUtility.listHeight);
			Widgets.BeginScrollView(rect, ref RecordsCardUtility.scrollPosition, rect2, true);
			Rect leftRect = rect2;
			leftRect.width *= 0.5f;
			Rect rightRect = rect2;
			rightRect.x = leftRect.xMax;
			rightRect.width = rect2.width - rightRect.x;
			leftRect.xMax -= 6f;
			rightRect.xMin += 6f;
			float a = RecordsCardUtility.DrawTimeRecords(leftRect, pawn);
			float b = RecordsCardUtility.DrawMiscRecords(rightRect, pawn);
			RecordsCardUtility.listHeight = Mathf.Max(a, b) + 100f;
			Widgets.EndScrollView();
		}

		// Token: 0x06012111 RID: 74001 RVA: 0x005487CC File Offset: 0x005469CC
		private static float DrawTimeRecords(Rect leftRect, Pawn pawn)
		{
			List<RecordDef> list = RecordsCardUtility.AllRecordsSorted;
			float num = 0f;
			Widgets.BeginGroup(leftRect);
			Widgets.ListSeparator(ref num, leftRect.width, "TimeRecordsCategory".Translate());
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].type == RecordType.Time)
				{
					num += RecordsCardUtility.DrawRecord(8f, num, leftRect.width - 8f, list[i], pawn);
				}
			}
			Widgets.EndGroup();
			return num;
		}

		// Token: 0x06012112 RID: 74002 RVA: 0x00548850 File Offset: 0x00546A50
		private static float DrawMiscRecords(Rect rightRect, Pawn pawn)
		{
			List<RecordDef> list = RecordsCardUtility.AllRecordsSorted;
			float num = 0f;
			Widgets.BeginGroup(rightRect);
			Widgets.ListSeparator(ref num, rightRect.width, "MiscRecordsCategory".Translate());
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].type == RecordType.Int || list[i].type == RecordType.Float)
				{
					num += RecordsCardUtility.DrawRecord(8f, num, rightRect.width - 8f, list[i], pawn);
				}
			}
			Widgets.EndGroup();
			return num;
		}

		// Token: 0x06012113 RID: 74003 RVA: 0x005488E4 File Offset: 0x00546AE4
		private static float DrawRecord(float x, float y, float width, RecordDef record, Pawn pawn)
		{
			float num = width * 0.45f;
			string text;
			if (record.type == RecordType.Time)
			{
				text = pawn.records.GetAsInt(record).ToStringTicksToPeriod(true, false, true, true, false);
			}
			else
			{
				text = pawn.records.GetValue(record).ToString("0.##");
			}
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(text, num));
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			Rect rect2 = rect;
			rect2.width -= num;
			Widgets.Label(rect2, record.LabelCap);
			Rect rect3 = rect;
			rect3.x = rect2.xMax;
			rect3.width = num;
			Widgets.Label(rect3, text);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => record.description, record.GetHashCode()));
			}
			return rect.height;
		}

		// Token: 0x0400BB8F RID: 48015
		private static Vector2 scrollPosition;

		// Token: 0x0400BB90 RID: 48016
		private static float listHeight;

		// Token: 0x0400BB91 RID: 48017
		private const float RecordsLeftPadding = 8f;

		// Token: 0x0400BB92 RID: 48018
		private static List<RecordDef> allRecordsSorted;
	}
}
