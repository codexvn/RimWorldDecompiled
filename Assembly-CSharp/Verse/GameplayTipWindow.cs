using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000139 RID: 313
	public class GameplayTipWindow
	{
		// Token: 0x0600099E RID: 2462 RVA: 0x0002D880 File Offset: 0x0002BA80
		public static void DrawWindow(Vector2 offset, bool useWindowStack)
		{
			if (GameplayTipWindow.allTipsCached == null)
			{
				GameplayTipWindow.allTipsCached = DefDatabase<TipSetDef>.AllDefsListForReading.SelectMany(delegate(TipSetDef set)
				{
					if (SteamDeck.IsSteamDeck && set == TipSetDefOf.GameplayTips)
					{
						return set.tips.Skip(11);
					}
					return set.tips;
				}).InRandomOrder(null).ToList<string>();
			}
			Rect rect = new Rect(offset.x, offset.y, GameplayTipWindow.WindowSize.x, GameplayTipWindow.WindowSize.y);
			if (useWindowStack)
			{
				Find.WindowStack.ImmediateWindow(62893997, rect, WindowLayer.Super, delegate
				{
					GameplayTipWindow.DrawContents(rect.AtZero());
				}, true, false, 1f, null, false);
				return;
			}
			Widgets.DrawShadowAround(rect);
			Widgets.DrawWindowBackground(rect);
			GameplayTipWindow.DrawContents(rect);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0002D950 File Offset: 0x0002BB50
		private static void DrawContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			if (Time.realtimeSinceStartup - GameplayTipWindow.lastTimeUpdatedTooltip > 17.5f || GameplayTipWindow.lastTimeUpdatedTooltip < 0f)
			{
				GameplayTipWindow.currentTipIndex = (GameplayTipWindow.currentTipIndex + 1) % GameplayTipWindow.allTipsCached.Count;
				GameplayTipWindow.lastTimeUpdatedTooltip = Time.realtimeSinceStartup;
			}
			Rect rect2 = rect;
			rect2.x += GameplayTipWindow.TextMargin.x;
			rect2.width -= GameplayTipWindow.TextMargin.x * 2f;
			rect2.y += GameplayTipWindow.TextMargin.y;
			rect2.height -= GameplayTipWindow.TextMargin.y * 2f;
			Widgets.Label(rect2, GameplayTipWindow.allTipsCached[GameplayTipWindow.currentTipIndex]);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002DA31 File Offset: 0x0002BC31
		public static void ResetTipTimer()
		{
			GameplayTipWindow.lastTimeUpdatedTooltip = -1f;
		}

		// Token: 0x040004D2 RID: 1234
		private static List<string> allTipsCached;

		// Token: 0x040004D3 RID: 1235
		private static float lastTimeUpdatedTooltip = -1f;

		// Token: 0x040004D4 RID: 1236
		private static int currentTipIndex = 0;

		// Token: 0x040004D5 RID: 1237
		public const float tipUpdateInterval = 17.5f;

		// Token: 0x040004D6 RID: 1238
		public static readonly Vector2 WindowSize = new Vector2(776f, 60f);

		// Token: 0x040004D7 RID: 1239
		private static readonly Vector2 TextMargin = new Vector2(15f, 8f);

		// Token: 0x040004D8 RID: 1240
		private const int InterfaceTipsCount = 11;
	}
}
