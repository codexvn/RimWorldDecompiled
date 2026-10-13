using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020033F9 RID: 13305
	[StaticConstructorOnStartup]
	public class MainTabWindow_History : MainTabWindow
	{
		// Token: 0x17003104 RID: 12548
		// (get) Token: 0x06012B5B RID: 76635 RVA: 0x00589E1F File Offset: 0x0058801F
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 640f);
			}
		}

		// Token: 0x06012B5C RID: 76636 RVA: 0x00589E30 File Offset: 0x00588030
		public override void PreOpen()
		{
			base.PreOpen();
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("Graph".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Graph;
			}, () => MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Graph));
			this.tabs.Add(new TabRecord("Messages".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Messages;
			}, () => MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Messages));
			this.tabs.Add(new TabRecord("Statistics".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Statistics;
			}, () => MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Statistics));
			this.historyAutoRecorderGroup = Find.History.Groups().FirstOrDefault<HistoryAutoRecorderGroup>();
			if (this.historyAutoRecorderGroup != null)
			{
				this.graphSection = new FloatRange(0f, (float)Find.TickManager.TicksGame / 60000f);
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].wealthWatcher.ForceRecount(false);
			}
			MainTabWindow_History.quickSearchWidget.Reset();
		}

		// Token: 0x06012B5D RID: 76637 RVA: 0x00589FD8 File Offset: 0x005881D8
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin += 45f;
			TabDrawer.DrawTabs<TabRecord>(rect2, this.tabs, 200f);
			switch (MainTabWindow_History.curTab)
			{
			case MainTabWindow_History.HistoryTab.Graph:
				this.DoGraphPage(rect2);
				return;
			case MainTabWindow_History.HistoryTab.Messages:
				this.DoMessagesPage(rect2);
				return;
			case MainTabWindow_History.HistoryTab.Statistics:
				this.DoStatisticsPage(rect2);
				return;
			default:
				return;
			}
		}

		// Token: 0x06012B5E RID: 76638 RVA: 0x0058A03C File Offset: 0x0058823C
		private void DoStatisticsPage(Rect rect)
		{
			rect.yMin += 17f;
			Widgets.BeginGroup(rect);
			StringBuilder stringBuilder = new StringBuilder();
			TimeSpan timeSpan = new TimeSpan(0, 0, (int)Find.GameInfo.RealPlayTimeInteracting);
			stringBuilder.AppendLine("Playtime".Translate() + ": " + timeSpan.Days.ToString() + "LetterDay".Translate() + " " + timeSpan.Hours.ToString() + "LetterHour".Translate() + " " + timeSpan.Minutes.ToString() + "LetterMinute".Translate() + " " + timeSpan.Seconds.ToString() + "LetterSecond".Translate());
			stringBuilder.AppendLine("Storyteller".Translate() + ": " + Find.Storyteller.def.LabelCap);
			stringBuilder.AppendLine("Difficulty".Translate() + ": " + Find.Storyteller.difficultyDef.LabelCap);
			if (Find.CurrentMap != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("ThisMapColonyWealthTotal".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthTotal.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthItems".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthItems.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthBuildings".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthBuildings.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthColonistsAndTameAnimals".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthPawns.ToString("F0"));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("NumThreatBigs".Translate() + ": " + Find.StoryWatcher.statsRecord.numThreatBigs.ToString());
			stringBuilder.AppendLine("NumEnemyRaids".Translate() + ": " + Find.StoryWatcher.statsRecord.numRaidsEnemy.ToString());
			stringBuilder.AppendLine();
			if (Find.CurrentMap != null)
			{
				stringBuilder.AppendLine("ThisMapDamageTaken".Translate() + ": " + Find.CurrentMap.damageWatcher.DamageTakenEver.ToString());
			}
			stringBuilder.AppendLine("ColonistsKilled".Translate() + ": " + Find.StoryWatcher.statsRecord.colonistsKilled.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("ColonistsLaunched".Translate() + ": " + Find.StoryWatcher.statsRecord.colonistsLaunched.ToString());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(new Rect(0f, 0f, 400f, 400f), stringBuilder.ToString());
			Widgets.EndGroup();
		}

		// Token: 0x06012B5F RID: 76639 RVA: 0x0058A448 File Offset: 0x00588648
		private void DoMessagesPage(Rect rect)
		{
			Rect rect2 = rect;
			rect.yMin += 10f;
			Widgets.CheckboxLabeled(new Rect(rect.x, rect.y, 200f, 30f), "ShowLetters".Translate(), ref MainTabWindow_History.showLetters, false, null, null, true, false);
			Widgets.CheckboxLabeled(new Rect(rect.x + 200f, rect.y, 200f, 30f), "ShowMessages".Translate(), ref MainTabWindow_History.showMessages, false, null, null, true, false);
			rect.yMin += 40f;
			bool flag = false;
			Rect outRect = rect;
			Rect viewRect = new Rect(0f, 0f, outRect.width / 2f - 16f, this.messagesLastHeight);
			List<IArchivable> archivablesListForReading = Find.Archive.ArchivablesListForReading;
			Rect rect3 = new Rect(rect.x + rect.width / 2f + 10f, rect.y, rect.width / 2f - 10f - 16f, rect.height);
			this.displayedMessageIndex = -1;
			MainTabWindow_History.quickSearchWidget.noResultsMatched = !archivablesListForReading.Any<IArchivable>();
			Widgets.BeginScrollView(outRect, ref this.messagesScrollPos, viewRect, true);
			float num = 0f;
			for (int i = archivablesListForReading.Count - 1; i >= 0; i--)
			{
				if ((MainTabWindow_History.showLetters || (!(archivablesListForReading[i] is Letter) && !(archivablesListForReading[i] is ArchivedDialog))) && (MainTabWindow_History.showMessages || !(archivablesListForReading[i] is Message)))
				{
					flag = true;
					if (i > this.displayedMessageIndex)
					{
						this.displayedMessageIndex = i;
					}
					if (num + 30f >= this.messagesScrollPos.y && num <= this.messagesScrollPos.y + outRect.height)
					{
						this.DoArchivableRow(new Rect(0f, num, viewRect.width - 5f, 30f), archivablesListForReading[i], i);
					}
					num += 30f;
				}
			}
			this.messagesLastHeight = num;
			Widgets.EndScrollView();
			if (flag)
			{
				if (this.displayedMessageIndex >= 0)
				{
					TaggedString label = archivablesListForReading[this.displayedMessageIndex].ArchivedTooltip.TruncateHeight(rect3.width - 10f, rect3.height - 10f, this.truncationCache);
					Widgets.Label(rect3.ContractedBy(5f), label);
				}
			}
			else
			{
				Widgets.NoneLabel(rect.yMin + 3f, rect.width, "(" + "NoMessages".Translate() + ")");
			}
			Rect rect4 = new Rect(rect2.x + MainTabWindow_History.SearchBarOffset.x, rect2.y + MainTabWindow_History.SearchBarOffset.y - Window.QuickSearchSize.y - 10f, Window.QuickSearchSize.x, Window.QuickSearchSize.y);
			MainTabWindow_History.quickSearchWidget.OnGUI(rect4, new Action(this.Notify_CommonSearchChanged), null);
		}

		// Token: 0x06012B60 RID: 76640 RVA: 0x0058A79C File Offset: 0x0058899C
		private void DoArchivableRow(Rect rect, IArchivable archivable, int index)
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Rect rect2 = rect;
			bool flag = MainTabWindow_History.quickSearchWidget.filter.Active && MainTabWindow_History.quickSearchWidget.filter.Matches(archivable.ArchivedLabel);
			if (flag)
			{
				Widgets.DrawTextHighlight(rect, 0f, null);
				if (MainTabWindow_History.quickSearchWidget.filter.Active && MainTabWindow_History.quickSearchWidget.CurrentlyFocused())
				{
					this.displayedMessageIndex = index;
				}
			}
			else if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect3 = rect2;
			rect3.width = 30f;
			rect2.xMin += 35f;
			float num;
			if (Find.Archive.IsPinned(archivable))
			{
				num = 1f;
			}
			else if (Mouse.IsOver(rect3))
			{
				num = 0.25f;
			}
			else
			{
				num = 0f;
			}
			Rect position = new Rect(rect3.x + (rect3.width - 22f) / 2f, rect3.y + (rect3.height - 22f) / 2f, 22f, 22f).Rounded();
			if (num > 0f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(position, MainTabWindow_History.PinTex);
			}
			else
			{
				GUI.color = MainTabWindow_History.PinOutlineColor;
				GUI.DrawTexture(position, MainTabWindow_History.PinOutlineTex);
			}
			GUI.color = Color.white;
			Rect rect4 = rect2;
			Rect outerRect = rect2;
			outerRect.width = 30f;
			rect2.xMin += 35f;
			Texture archivedIcon = archivable.ArchivedIcon;
			if (archivedIcon != null)
			{
				GUI.color = archivable.ArchivedIconColor;
				Widgets.DrawTextureFitted(outerRect, archivedIcon, 0.8f, 1f);
				GUI.color = Color.white;
			}
			Rect rect5 = rect2;
			rect5.width = 90f;
			rect2.xMin += 95f;
			Vector2 location = (Find.CurrentMap != null) ? Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile) : default(Vector2);
			GUI.color = new Color(0.75f, 0.75f, 0.75f);
			string str = GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(archivable.CreatedTicksGame), location);
			Widgets.Label(rect5, str.Truncate(rect5.width, null));
			GUI.color = Color.white;
			Rect rect6 = rect2;
			if (!flag)
			{
				GUI.color = Color.gray;
			}
			Widgets.Label(rect6, archivable.ArchivedLabel.Truncate(rect6.width, null));
			GenUI.ResetLabelAlign();
			Text.WordWrap = true;
			GUI.color = Color.white;
			TooltipHandler.TipRegionByKey(rect3, "PinArchivableTip", 200);
			if (Mouse.IsOver(rect4))
			{
				this.displayedMessageIndex = index;
			}
			if (Widgets.ButtonInvisible(rect3, true))
			{
				if (Find.Archive.IsPinned(archivable))
				{
					Find.Archive.Unpin(archivable);
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				else
				{
					Find.Archive.Pin(archivable);
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
			}
			if (Widgets.ButtonInvisible(rect4, true))
			{
				if (Event.current.button == 1)
				{
					LookTargets lookTargets = archivable.LookTargets;
					if (CameraJumper.CanJump(lookTargets.TryGetPrimaryTarget()))
					{
						CameraJumper.TryJumpAndSelect(lookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
						Find.MainTabsRoot.EscapeCurrentTab(true);
						return;
					}
				}
				else
				{
					archivable.OpenArchived();
				}
			}
		}

		// Token: 0x06012B61 RID: 76641 RVA: 0x0058AB08 File Offset: 0x00588D08
		private void DoGraphPage(Rect rect)
		{
			rect.yMin += 17f;
			Widgets.BeginGroup(rect);
			Rect graphRect = new Rect(0f, 0f, rect.width, 450f);
			Rect legendRect = new Rect(0f, graphRect.yMax, rect.width / 2f, 40f);
			Rect rect2 = new Rect(0f, legendRect.yMax, rect.width, 40f);
			if (this.historyAutoRecorderGroup != null)
			{
				MainTabWindow_History.marks.Clear();
				List<Tale> allTalesListForReading = Find.TaleManager.AllTalesListForReading;
				for (int i = 0; i < allTalesListForReading.Count; i++)
				{
					Tale tale = allTalesListForReading[i];
					if (tale.def.type == TaleType.PermanentHistorical && !tale.hidden)
					{
						float x = (float)GenDate.TickAbsToGame(tale.date) / 60000f;
						MainTabWindow_History.marks.Add(new CurveMark(x, tale.ShortSummary, tale.def.historyGraphColor));
					}
				}
				this.historyAutoRecorderGroup.DrawGraph(graphRect, legendRect, this.graphSection, MainTabWindow_History.marks);
			}
			Text.Font = GameFont.Small;
			float num = (float)Find.TickManager.TicksGame / 60000f;
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width, legendRect.yMin, 110f, 40f), "Last30Days".Translate(), true, true, true, null))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 30f), num);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 110f + 4f, legendRect.yMin, 110f, 40f), "Last100Days".Translate(), true, true, true, null))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 100f), num);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 228f, legendRect.yMin, 110f, 40f), "Last300Days".Translate(), true, true, true, null))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 300f), num);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 342f, legendRect.yMin, 110f, 40f), "AllDays".Translate(), true, true, true, null))
			{
				this.graphSection = new FloatRange(0f, num);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect2.x, rect2.y, 110f, 40f), "SelectGraph".Translate(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				List<HistoryAutoRecorderGroup> list2 = Find.History.Groups();
				for (int j = 0; j < list2.Count; j++)
				{
					HistoryAutoRecorderGroup groupLocal = list2[j];
					if (!groupLocal.def.devModeOnly || Prefs.DevMode)
					{
						list.Add(new FloatMenuOption(groupLocal.def.LabelCap, delegate()
						{
							this.historyAutoRecorderGroup = groupLocal;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				FloatMenu window = new FloatMenu(list, "SelectGraph".Translate(), false);
				Find.WindowStack.Add(window);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HistoryTab, KnowledgeAmount.Total);
			}
			Widgets.EndGroup();
		}

		// Token: 0x06012B62 RID: 76642 RVA: 0x0058AF36 File Offset: 0x00589136
		public override void Notify_ClickOutsideWindow()
		{
			MainTabWindow_History.quickSearchWidget.Unfocus();
		}

		// Token: 0x0400C241 RID: 49729
		private HistoryAutoRecorderGroup historyAutoRecorderGroup;

		// Token: 0x0400C242 RID: 49730
		private FloatRange graphSection;

		// Token: 0x0400C243 RID: 49731
		private Vector2 messagesScrollPos;

		// Token: 0x0400C244 RID: 49732
		private float messagesLastHeight;

		// Token: 0x0400C245 RID: 49733
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x0400C246 RID: 49734
		private int displayedMessageIndex;

		// Token: 0x0400C247 RID: 49735
		private static QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400C248 RID: 49736
		private static MainTabWindow_History.HistoryTab curTab = MainTabWindow_History.HistoryTab.Graph;

		// Token: 0x0400C249 RID: 49737
		private static bool showLetters = true;

		// Token: 0x0400C24A RID: 49738
		private static bool showMessages;

		// Token: 0x0400C24B RID: 49739
		private const float MessagesRowHeight = 30f;

		// Token: 0x0400C24C RID: 49740
		private const float PinColumnSize = 30f;

		// Token: 0x0400C24D RID: 49741
		private const float PinSize = 22f;

		// Token: 0x0400C24E RID: 49742
		private const float IconColumnSize = 30f;

		// Token: 0x0400C24F RID: 49743
		private const float DateSize = 90f;

		// Token: 0x0400C250 RID: 49744
		private const float SpaceBetweenColumns = 5f;

		// Token: 0x0400C251 RID: 49745
		private static readonly Vector2 SearchBarOffset = new Vector2(720f, 8f);

		// Token: 0x0400C252 RID: 49746
		private static readonly Texture2D PinTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin", true);

		// Token: 0x0400C253 RID: 49747
		private static readonly Texture2D PinOutlineTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin-Outline", true);

		// Token: 0x0400C254 RID: 49748
		private static readonly Color PinOutlineColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);

		// Token: 0x0400C255 RID: 49749
		private Dictionary<string, string> truncationCache = new Dictionary<string, string>();

		// Token: 0x0400C256 RID: 49750
		private static List<CurveMark> marks = new List<CurveMark>();

		// Token: 0x020033FA RID: 13306
		private enum HistoryTab : byte
		{
			// Token: 0x0400C258 RID: 49752
			Graph,
			// Token: 0x0400C259 RID: 49753
			Messages,
			// Token: 0x0400C25A RID: 49754
			Statistics
		}
	}
}
