using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02003381 RID: 13185
	[StaticConstructorOnStartup]
	public static class InspectPaneUtility
	{
		// Token: 0x060128A4 RID: 75940 RVA: 0x005784DA File Offset: 0x005766DA
		public static void Reset()
		{
			InspectPaneUtility.truncatedLabelsCached.Clear();
		}

		// Token: 0x060128A5 RID: 75941 RVA: 0x005784E8 File Offset: 0x005766E8
		public static float PaneWidthFor(IInspectPane pane)
		{
			if (pane == null)
			{
				return 432f;
			}
			InspectPaneUtility.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.visible = 0;
			if (pane.CurTabs != null)
			{
				IList list = pane.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						InspectPaneUtility.<PaneWidthFor>g__Process|9_0((InspectTabBase)list[i], ref CS$<>8__locals1);
					}
				}
				else
				{
					foreach (InspectTabBase tab in pane.CurTabs)
					{
						InspectPaneUtility.<PaneWidthFor>g__Process|9_0(tab, ref CS$<>8__locals1);
					}
				}
			}
			return 72f * (float)Mathf.Max(6, CS$<>8__locals1.visible);
		}

		// Token: 0x060128A6 RID: 75942 RVA: 0x00578598 File Offset: 0x00576798
		public static Vector2 PaneSizeFor(IInspectPane pane)
		{
			return new Vector2(InspectPaneUtility.PaneWidthFor(pane), 165f);
		}

		// Token: 0x060128A7 RID: 75943 RVA: 0x005785AC File Offset: 0x005767AC
		public static bool CanInspectTogether(object A, object B)
		{
			Thing thing = A as Thing;
			Thing thing2 = B as Thing;
			return thing != null && thing2 != null && thing.def.category != ThingCategory.Pawn && thing.def == thing2.def;
		}

		// Token: 0x060128A8 RID: 75944 RVA: 0x005785F0 File Offset: 0x005767F0
		public static string AdjustedLabelFor(List<object> selected, Rect rect)
		{
			IRenameable renameable = selected[0] as IRenameable;
			string text;
			if (renameable != null)
			{
				if (selected.Count == 1)
				{
					text = renameable.InspectLabel;
				}
				else
				{
					string baseLabel = renameable.BaseLabel;
					bool flag = true;
					for (int i = 1; i < selected.Count; i++)
					{
						IRenameable renameable2 = selected[i] as IRenameable;
						if (renameable2 != null && renameable2.BaseLabel != baseLabel)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						text = renameable.BaseLabel;
					}
					else
					{
						text = "VariousLabel".Translate();
					}
					text = text + " x" + selected.Count.ToString();
				}
			}
			else
			{
				InspectPaneUtility.selectedThings.Clear();
				for (int j = 0; j < selected.Count; j++)
				{
					Thing thing = selected[j] as Thing;
					if (thing != null)
					{
						InspectPaneUtility.selectedThings.Add(thing.GetInnerIfMinified());
					}
				}
				if (InspectPaneUtility.selectedThings.Count == 1)
				{
					text = InspectPaneUtility.selectedThings[0].LabelCap;
				}
				else if (InspectPaneUtility.selectedThings.Count > 1)
				{
					string b = InspectPaneUtility.PluralLabelFor(InspectPaneUtility.selectedThings[0]);
					bool flag2 = true;
					for (int k = 1; k < InspectPaneUtility.selectedThings.Count; k++)
					{
						if (InspectPaneUtility.PluralLabelFor(InspectPaneUtility.selectedThings[k]) != b)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						text = InspectPaneUtility.PluralLabelFor(InspectPaneUtility.selectedThings[0]).CapitalizeFirst();
					}
					else
					{
						text = "VariousLabel".Translate();
					}
					int num = 0;
					for (int l = 0; l < InspectPaneUtility.selectedThings.Count; l++)
					{
						num += InspectPaneUtility.selectedThings[l].stackCount;
					}
					text = text + " x" + num.ToStringCached();
				}
				else
				{
					text = "?";
				}
				InspectPaneUtility.selectedThings.Clear();
			}
			string result;
			using (new TextBlock(GameFont.Medium))
			{
				result = text.Truncate(rect.width, InspectPaneUtility.truncatedLabelsCached);
			}
			return result;
		}

		// Token: 0x060128A9 RID: 75945 RVA: 0x00578830 File Offset: 0x00576A30
		private static string PluralLabelFor(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.IsMutant)
			{
				return pawn.mutant.Def.label;
			}
			return thing.def.label;
		}

		// Token: 0x060128AA RID: 75946 RVA: 0x0057886B File Offset: 0x00576A6B
		public static void ExtraOnGUI(IInspectPane pane)
		{
			if (pane.AnythingSelected)
			{
				if (KeyBindingDefOf.SelectNextInCell.KeyDownEvent)
				{
					pane.SelectNextInCell();
				}
				InspectPaneUtility.DoTabs(pane);
			}
		}

		// Token: 0x060128AB RID: 75947 RVA: 0x00578890 File Offset: 0x00576A90
		public static void UpdateTabs(IInspectPane pane)
		{
			InspectPaneUtility.<>c__DisplayClass16_0 CS$<>8__locals1;
			CS$<>8__locals1.pane = pane;
			CS$<>8__locals1.tabUpdated = false;
			if (CS$<>8__locals1.pane.CurTabs != null)
			{
				IList list = CS$<>8__locals1.pane.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						InspectPaneUtility.<UpdateTabs>g__Update|16_0((InspectTabBase)list[i], ref CS$<>8__locals1);
					}
				}
				else
				{
					foreach (InspectTabBase tab in CS$<>8__locals1.pane.CurTabs)
					{
						InspectPaneUtility.<UpdateTabs>g__Update|16_0(tab, ref CS$<>8__locals1);
					}
				}
			}
			if (!CS$<>8__locals1.tabUpdated)
			{
				CS$<>8__locals1.pane.CloseOpenTab();
			}
		}

		// Token: 0x060128AC RID: 75948 RVA: 0x0057894C File Offset: 0x00576B4C
		public static void InspectPaneOnGUI(Rect inRect, IInspectPane pane)
		{
			pane.RecentHeight = 165f;
			if (pane.AnythingSelected)
			{
				try
				{
					Rect rect = inRect.ContractedBy(12f);
					rect.yMin -= 4f;
					rect.yMax += 6f;
					Widgets.BeginGroup(rect);
					float num = 0f;
					if (pane.ShouldShowSelectNextInCellButton)
					{
						Rect rect2 = new Rect(rect.width - 24f, 0f, 24f, 24f);
						MouseoverSounds.DoRegion(rect2);
						if (Widgets.ButtonImage(rect2, TexButton.SelectOverlappingNext, true, null))
						{
							pane.SelectNextInCell();
						}
						num += 24f;
						if (SteamDeck.IsSteamDeckInNonKeyboardMode)
						{
							TooltipHandler.TipRegionByKey(rect2, "SelectNextInSquareTipController");
						}
						else
						{
							TooltipHandler.TipRegionByKey(rect2, "SelectNextInSquareTip", KeyBindingDefOf.SelectNextInCell.MainKeyLabel);
						}
					}
					pane.DoInspectPaneButtons(rect, ref num);
					Rect rect3 = new Rect(0f, 0f, rect.width - num, 50f);
					string label = pane.GetLabel(rect3);
					rect3.width += 300f;
					using (new TextBlock(GameFont.Medium, TextAnchor.UpperLeft))
					{
						Widgets.Label(rect3, label);
					}
					if (pane.ShouldShowPaneContents)
					{
						Rect rect4 = rect.AtZero();
						rect4.yMin += 26f;
						pane.DoPaneContents(rect4);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception doing inspect pane: " + ex.ToString());
				}
				finally
				{
					Widgets.EndGroup();
				}
			}
		}

		// Token: 0x060128AD RID: 75949 RVA: 0x00578B30 File Offset: 0x00576D30
		private static void DoTabs(IInspectPane pane)
		{
			InspectPaneUtility.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.pane = pane;
			try
			{
				InspectPaneUtility.<>c__DisplayClass18_1 CS$<>8__locals2;
				CS$<>8__locals2.tabsTopY = CS$<>8__locals1.pane.PaneTopY - 30f;
				CS$<>8__locals2.curTabX = InspectPaneUtility.PaneWidthFor(CS$<>8__locals1.pane) - 72f;
				CS$<>8__locals2.leftEdge = 0f;
				CS$<>8__locals2.drewOpen = false;
				if (CS$<>8__locals1.pane.CurTabs != null)
				{
					IList list = CS$<>8__locals1.pane.CurTabs as IList;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							InspectPaneUtility.<DoTabs>g__Do|18_0((InspectTabBase)list[i], ref CS$<>8__locals1, ref CS$<>8__locals2);
						}
					}
					else
					{
						foreach (InspectTabBase tab in CS$<>8__locals1.pane.CurTabs)
						{
							InspectPaneUtility.<DoTabs>g__Do|18_0(tab, ref CS$<>8__locals1, ref CS$<>8__locals2);
						}
					}
				}
				if (CS$<>8__locals2.drewOpen)
				{
					GUI.DrawTexture(new Rect(0f, CS$<>8__locals2.tabsTopY, CS$<>8__locals2.leftEdge, 30f), InspectPaneUtility.InspectTabButtonFillTex);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 742783);
			}
		}

		// Token: 0x060128AE RID: 75950 RVA: 0x00578C6C File Offset: 0x00576E6C
		private static bool IsOpen(InspectTabBase tab, IInspectPane pane)
		{
			return tab.GetType() == pane.OpenTabType;
		}

		// Token: 0x060128AF RID: 75951 RVA: 0x00578C80 File Offset: 0x00576E80
		private static void ToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (InspectPaneUtility.IsOpen(tab, pane) || (tab == null && pane.OpenTabType == null))
			{
				pane.OpenTabType = null;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				return;
			}
			tab.OnOpen();
			pane.OpenTabType = tab.GetType();
			SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
		}

		// Token: 0x060128B0 RID: 75952 RVA: 0x00578CD8 File Offset: 0x00576ED8
		public static InspectTabBase OpenTab(Type inspectTabType)
		{
			InspectPaneUtility.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.inspectTabType = inspectTabType;
			MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			CS$<>8__locals1.tab = null;
			if (mainTabWindow_Inspect.CurTabs != null)
			{
				IList list = mainTabWindow_Inspect.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (InspectPaneUtility.<OpenTab>g__Find|21_0((InspectTabBase)list[i], ref CS$<>8__locals1))
						{
							break;
						}
					}
				}
				else
				{
					using (IEnumerator<InspectTabBase> enumerator = mainTabWindow_Inspect.CurTabs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (InspectPaneUtility.<OpenTab>g__Find|21_0(enumerator.Current, ref CS$<>8__locals1))
							{
								break;
							}
						}
					}
				}
			}
			if (CS$<>8__locals1.tab != null)
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, true);
				}
				if (!InspectPaneUtility.IsOpen(CS$<>8__locals1.tab, mainTabWindow_Inspect))
				{
					InspectPaneUtility.ToggleTab(CS$<>8__locals1.tab, mainTabWindow_Inspect);
				}
			}
			return CS$<>8__locals1.tab;
		}

		// Token: 0x060128B1 RID: 75953 RVA: 0x00578DD8 File Offset: 0x00576FD8
		private static void InterfaceToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (TutorSystem.TutorialMode && !InspectPaneUtility.IsOpen(tab, pane) && !TutorSystem.AllowAction("ITab-" + tab.tutorTag + "-Open"))
			{
				return;
			}
			InspectPaneUtility.ToggleTab(tab, pane);
		}

		// Token: 0x060128B3 RID: 75955 RVA: 0x00578E4C File Offset: 0x0057704C
		[CompilerGenerated]
		internal static void <PaneWidthFor>g__Process|9_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass9_0 A_1)
		{
			if (tab.IsVisible && !tab.Hidden)
			{
				int visible = A_1.visible;
				A_1.visible = visible + 1;
			}
		}

		// Token: 0x060128B4 RID: 75956 RVA: 0x00578E79 File Offset: 0x00577079
		[CompilerGenerated]
		internal static void <UpdateTabs>g__Update|16_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass16_0 A_1)
		{
			if (!tab.IsVisible)
			{
				return;
			}
			if (tab.GetType() == A_1.pane.OpenTabType)
			{
				tab.TabUpdate();
				A_1.tabUpdated = true;
			}
		}

		// Token: 0x060128B5 RID: 75957 RVA: 0x00578EAC File Offset: 0x005770AC
		[CompilerGenerated]
		internal static void <DoTabs>g__Do|18_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass18_0 A_1, ref InspectPaneUtility.<>c__DisplayClass18_1 A_2)
		{
			if (!tab.IsVisible)
			{
				return;
			}
			bool flag = tab.GetType() == A_1.pane.OpenTabType;
			if (!tab.Hidden)
			{
				Rect rect = new Rect(A_2.curTabX, A_2.tabsTopY, 72f, 30f);
				A_2.leftEdge = A_2.curTabX;
				Text.Font = GameFont.Small;
				if (Widgets.ButtonText(rect, tab.labelKey.Translate(), true, true, true, null))
				{
					InspectPaneUtility.InterfaceToggleTab(tab, A_1.pane);
				}
				if (!flag && !tab.TutorHighlightTagClosed.NullOrEmpty())
				{
					UIHighlighter.HighlightOpportunity(rect, tab.TutorHighlightTagClosed);
				}
				A_2.curTabX -= 72f;
			}
			if (flag)
			{
				tab.DoTabGUI();
				A_1.pane.RecentHeight = 700f;
				A_2.drewOpen = true;
			}
		}

		// Token: 0x060128B6 RID: 75958 RVA: 0x00578F91 File Offset: 0x00577191
		[CompilerGenerated]
		internal static bool <OpenTab>g__Find|21_0(InspectTabBase t, ref InspectPaneUtility.<>c__DisplayClass21_0 A_1)
		{
			if (A_1.inspectTabType.IsAssignableFrom(t.GetType()))
			{
				A_1.tab = t;
				return true;
			}
			return false;
		}

		// Token: 0x0400C091 RID: 49297
		private static Dictionary<string, string> truncatedLabelsCached = new Dictionary<string, string>();

		// Token: 0x0400C092 RID: 49298
		public const float TabWidth = 72f;

		// Token: 0x0400C093 RID: 49299
		public const float TabHeight = 30f;

		// Token: 0x0400C094 RID: 49300
		private static readonly Texture2D InspectTabButtonFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.07450981f, 0.08627451f, 0.105882354f, 1f));

		// Token: 0x0400C095 RID: 49301
		public const float CornerButtonsSize = 24f;

		// Token: 0x0400C096 RID: 49302
		public const float PaneInnerMargin = 12f;

		// Token: 0x0400C097 RID: 49303
		public const float PaneHeight = 165f;

		// Token: 0x0400C098 RID: 49304
		private const int TabMinimum = 6;

		// Token: 0x0400C099 RID: 49305
		private static List<Thing> selectedThings = new List<Thing>();
	}
}
