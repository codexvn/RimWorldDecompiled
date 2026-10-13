using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003E75 RID: 15989
	public static class WorldFactionsUIUtility
	{
		// Token: 0x06015E01 RID: 89601 RVA: 0x0068B0D4 File Offset: 0x006892D4
		public static void DoWindowContents(Rect rect, List<FactionDef> factions, bool isDefaultFactionCounts)
		{
			WorldFactionsUIUtility.<>c__DisplayClass8_0 CS$<>8__locals1 = new WorldFactionsUIUtility.<>c__DisplayClass8_0();
			CS$<>8__locals1.factions = factions;
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Text.LineHeight);
			Widgets.Label(rect2, "Factions".Translate());
			TooltipHandler.TipRegion(rect2, () => "FactionSelectionDesc".Translate(12), 4534123);
			float num = Text.LineHeight + 4f;
			float num2 = rect.width * 0.050000012f;
			Rect rect3 = new Rect(rect.x + num2, rect.y + num, rect.width * 0.9f, rect.height - num - Text.LineHeight - 28f - WorldFactionsUIUtility.warningHeight);
			Rect outRect = rect3.ContractedBy(4f);
			Rect rect4 = new Rect(outRect.x, outRect.y, outRect.width, WorldFactionsUIUtility.listingHeight);
			Widgets.BeginScrollView(outRect, ref WorldFactionsUIUtility.scrollPosition, rect4, true);
			WorldFactionsUIUtility.listingHeight = 0f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect4.width;
			listing_Standard.Begin(rect4);
			for (int i = 0; i < CS$<>8__locals1.factions.Count; i++)
			{
				if (CS$<>8__locals1.factions[i].displayInFactionSelection)
				{
					listing_Standard.Gap(4f);
					if (WorldFactionsUIUtility.DoRow(listing_Standard.GetRect(24f, 1f), CS$<>8__locals1.factions[i], CS$<>8__locals1.factions, i))
					{
						i--;
					}
					listing_Standard.Gap(4f);
					WorldFactionsUIUtility.listingHeight += 32f;
				}
			}
			listing_Standard.End();
			Widgets.EndScrollView();
			Rect rect5 = new Rect(outRect.x, Mathf.Min(rect3.yMax, outRect.y + WorldFactionsUIUtility.listingHeight + 4f), outRect.width, 28f);
			if (Widgets.ButtonText(rect5, "Add".Translate().CapitalizeFirst() + "...", true, true, true, null) && TutorSystem.AllowAction("ConfiguringWorldFactions"))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionDef factionDef in FactionGenerator.ConfigurableFactions)
				{
					if (factionDef.displayInFactionSelection)
					{
						FactionDef localDef = factionDef;
						string text = localDef.LabelCap;
						Action action = delegate()
						{
							CS$<>8__locals1.factions.Add(localDef);
						};
						AcceptanceReport report = CS$<>8__locals1.<DoWindowContents>g__CanAddFaction|1(localDef);
						if (!report)
						{
							action = null;
							if (!report.Reason.NullOrEmpty())
							{
								text = text + " (" + report.Reason + ")";
							}
						}
						else
						{
							int num3 = CS$<>8__locals1.factions.Count((FactionDef x) => x == localDef);
							if (num3 > 0)
							{
								text = text + " (" + num3.ToString() + ")";
							}
						}
						list.Add(new FloatMenuOption(text, action, localDef.FactionIcon, localDef.DefaultColor, MenuOptionPriority.Default, null, null, 24f, (Rect r) => Widgets.InfoCardButton(r.x, r.y + 3f, localDef), null, true, 0, HorizontalJustification.Left, true)
						{
							tooltip = new TipSignal?(text.AsTipTitle() + "\n" + localDef.Description)
						});
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			float yMax = rect5.yMax;
			bool flag = CS$<>8__locals1.factions.Count((FactionDef x) => !x.hidden) != 0;
			StringBuilder stringBuilder = new StringBuilder();
			if (!flag)
			{
				stringBuilder.AppendLine("FactionsDisabledWarning".Translate());
			}
			else
			{
				if (ModsConfig.RoyaltyActive && !CS$<>8__locals1.factions.Contains(FactionDefOf.Empire))
				{
					stringBuilder.AppendLine("Warning".Translate() + ": " + "FactionDisabledContentWarning".Translate(FactionDefOf.Empire.label));
				}
				if (!CS$<>8__locals1.factions.Contains(FactionDefOf.Mechanoid))
				{
					stringBuilder.AppendLine("Warning".Translate() + ": " + "MechanoidsDisabledContentWarning".Translate(FactionDefOf.Mechanoid.label));
				}
				if (!CS$<>8__locals1.factions.Contains(FactionDefOf.Insect))
				{
					stringBuilder.AppendLine("Warning".Translate() + ": " + "InsectsDisabledContentWarning".Translate(FactionDefOf.Insect.label));
				}
				if (ModsConfig.OdysseyActive)
				{
					if (!CS$<>8__locals1.factions.Contains(FactionDefOf.Mechanoid))
					{
						stringBuilder.AppendLine("Warning".Translate() + ": " + "OdysseyFactionDisabledContentWarning".Translate(FactionDefOf.Mechanoid.label));
					}
					if (!CS$<>8__locals1.factions.Contains(FactionDefOf.Insect))
					{
						stringBuilder.AppendLine("Warning".Translate() + ": " + "OdysseyFactionDisabledContentWarning".Translate(FactionDefOf.Insect.label));
					}
				}
			}
			WorldFactionsUIUtility.warningHeight = 0f;
			if (stringBuilder.Length > 0)
			{
				bool wordWrap = Text.WordWrap;
				string text2 = stringBuilder.ToString().TrimEndNewlines();
				Rect rect6 = new Rect(rect.x, yMax, rect.width, rect.yMax - yMax);
				GUI.color = Color.yellow;
				Text.Font = GameFont.Tiny;
				WorldFactionsUIUtility.warningHeight = Text.CalcHeight(text2, rect6.width);
				Text.WordWrap = true;
				Widgets.Label(rect6, text2);
				Text.WordWrap = wordWrap;
				Text.Font = GameFont.Small;
				GUI.color = Color.white;
			}
		}

		// Token: 0x06015E02 RID: 89602 RVA: 0x0068B760 File Offset: 0x00689960
		public static bool DoRow(Rect rect, FactionDef factionDef, List<FactionDef> factions, int index)
		{
			bool result = false;
			bool flag = true;
			using (List<ScenPart>.Enumerator enumerator = Current.Game.Scenario.parts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.preventRemovalOfFaction == factionDef)
					{
						flag = false;
						break;
					}
				}
			}
			Rect rect2 = new Rect(rect.x, rect.y - 4f, rect.width, rect.height + 8f);
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect2);
			}
			Widgets.BeginGroup(rect);
			WidgetRow widgetRow = new WidgetRow(6f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			GUI.color = factionDef.DefaultColor;
			widgetRow.Icon(factionDef.FactionIcon, null, 0f);
			GUI.color = Color.white;
			widgetRow.Gap(4f);
			Text.Anchor = TextAnchor.MiddleCenter;
			widgetRow.Label(factionDef.LabelCap, -1f, null, -1f);
			Text.Anchor = TextAnchor.UpperLeft;
			if (flag && Widgets.ButtonImage(new Rect(rect.width - 24f - 6f, 0f, 24f, 24f), TexButton.Delete, true, null) && TutorSystem.AllowAction("ConfiguringWorldFactions"))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				factions.RemoveAt(index);
				result = true;
			}
			Widgets.EndGroup();
			if (Mouse.IsOver(rect2))
			{
				string text = factionDef.LabelCap.AsTipTitle() + "\n" + factionDef.Description;
				if (!flag)
				{
					text = text + "\n\n" + "FactionRemovalDisabled".Translate().Colorize(ColorLibrary.RedReadable);
				}
				TooltipHandler.TipRegion(rect2, text);
				Widgets.DrawHighlight(rect2);
			}
			return result;
		}

		// Token: 0x0400F2AF RID: 62127
		private static Vector2 scrollPosition;

		// Token: 0x0400F2B0 RID: 62128
		private static float listingHeight;

		// Token: 0x0400F2B1 RID: 62129
		private static float warningHeight;

		// Token: 0x0400F2B2 RID: 62130
		private const float RowHeight = 24f;

		// Token: 0x0400F2B3 RID: 62131
		private const float AddButtonHeight = 28f;

		// Token: 0x0400F2B4 RID: 62132
		private const int MaxVisibleFactions = 12;

		// Token: 0x0400F2B5 RID: 62133
		private const int MaxVisibleFactionsRecommended = 11;

		// Token: 0x0400F2B6 RID: 62134
		private const float RowMarginX = 6f;
	}
}
