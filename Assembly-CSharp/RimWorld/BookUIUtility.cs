using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200386A RID: 14442
	public static class BookUIUtility
	{
		// Token: 0x06013F6D RID: 81773 RVA: 0x005ECB40 File Offset: 0x005EAD40
		public static void DrawTitle(Rect rect, Book book)
		{
			Rect position = rect;
			position.width = position.height;
			GUI.DrawTexture(position, book.def.uiIcon);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.Font = GameFont.Medium;
			Rect rect2 = rect;
			rect2.x += rect.height + 10f;
			rect2.width -= rect.height + 10f;
			Widgets.LabelFit(rect2, book.LabelCap);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06013F6E RID: 81774 RVA: 0x005ECBCB File Offset: 0x005EADCB
		public static void DrawBookDescPanel(Rect rect, Book book, ref Vector2 scroll)
		{
			Widgets.LabelScrollable(rect, book.FlavorUI, ref scroll, false, true, false);
		}

		// Token: 0x06013F6F RID: 81775 RVA: 0x005ECBE0 File Offset: 0x005EADE0
		public static void DrawBookInfoPanel(Rect rect, Book book)
		{
			float y = rect.y;
			BookUIUtility.DrawBenefits(rect, ref y, book);
			BookUIUtility.DrawDangers(rect, ref y, book);
			Rect rect2 = rect;
			rect2.y = y;
			rect2.yMax = rect.yMax;
			BookUIUtility.DrawHyperlinks(rect2, ref y, book);
		}

		// Token: 0x06013F70 RID: 81776 RVA: 0x005ECC28 File Offset: 0x005EAE28
		private static void DrawDangers(Rect rect, ref float y, Book book)
		{
			if (book.MentalBreakChancePerHour > 0f)
			{
				BookUIUtility.DrawSubheader(rect, ref y, "Dangers".Translate());
				y += 10f;
				Widgets.Label(rect, ref y, string.Format("- {0}: {1}", "BookMentalBreak".Translate(), "PerHour".Translate(book.MentalBreakChancePerHour.ToStringPercent("0.0"))), default(TipSignal));
				y += 10f;
			}
		}

		// Token: 0x06013F71 RID: 81777 RVA: 0x005ECCBC File Offset: 0x005EAEBC
		private static void DrawSubheader(Rect rect, ref float y, string title)
		{
			Rect position = new Rect
			{
				x = rect.x,
				y = y,
				xMax = rect.xMax,
				yMax = rect.yMax
			};
			GUI.BeginGroup(position);
			float num = 0f;
			Widgets.ListSeparator(ref num, position.width, title);
			y += num;
			GUI.EndGroup();
		}

		// Token: 0x06013F72 RID: 81778 RVA: 0x005ECD2C File Offset: 0x005EAF2C
		private static void DrawBenefits(Rect rect, ref float y, Book book)
		{
			bool flag = false;
			using (IEnumerator<BookOutcomeDoer> enumerator = book.BookComp.Doers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!string.IsNullOrEmpty(enumerator.Current.GetBenefitsString(null)))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			BookUIUtility.DrawSubheader(rect, ref y, "Benefits".Translate());
			y += 10f;
			foreach (BookOutcomeDoer bookOutcomeDoer in book.BookComp.Doers)
			{
				string benefitsString = bookOutcomeDoer.GetBenefitsString(null);
				if (!string.IsNullOrEmpty(benefitsString))
				{
					Widgets.Label(rect, ref y, benefitsString, default(TipSignal));
				}
			}
			y += 10f;
		}

		// Token: 0x06013F73 RID: 81779 RVA: 0x005ECE10 File Offset: 0x005EB010
		private static void DrawHyperlinks(Rect rect, ref float y, Book book)
		{
			Color normalOptionColor = Widgets.NormalOptionColor;
			float num = 0f;
			foreach (Dialog_InfoCard.Hyperlink hyperlink in book.GetHyperlinks().Reverse<Dialog_InfoCard.Hyperlink>())
			{
				float num2 = Text.CalcHeight(hyperlink.Label, rect.width);
				Rect rect2 = rect;
				rect2.y = rect.yMax - num2 - num - 4f;
				rect2.height = num2;
				TaggedString taggedString = "ViewHyperlink".Translate(hyperlink.Label);
				Widgets.HyperlinkWithIcon(rect2, hyperlink, taggedString, 2f, 6f, new Color?(normalOptionColor), false, null);
				num += num2;
				y += num2;
			}
		}

		// Token: 0x0400DC72 RID: 56434
		private const float KeyValSplit = 140f;

		// Token: 0x0400DC73 RID: 56435
		private const float LineHeight = 25f;

		// Token: 0x0400DC74 RID: 56436
		private const float HyperlinkBaseGap = 4f;
	}
}
