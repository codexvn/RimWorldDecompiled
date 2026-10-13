using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003413 RID: 13331
	[StaticConstructorOnStartup]
	public class MainTabWindow_Research : MainTabWindow
	{
		// Token: 0x17003124 RID: 12580
		// (get) Token: 0x06012C17 RID: 76823 RVA: 0x0058F94B File Offset: 0x0058DB4B
		// (set) Token: 0x06012C18 RID: 76824 RVA: 0x0058F954 File Offset: 0x0058DB54
		public ResearchTabDef CurTab
		{
			get
			{
				return this.curTabInt;
			}
			set
			{
				if (value == this.curTabInt)
				{
					return;
				}
				this.curTabInt = value;
				Vector2 vector = this.ViewSize(this.CurTab);
				this.rightViewWidth = vector.x;
				this.rightScrollPosition = Vector2.zero;
			}
		}

		// Token: 0x17003125 RID: 12581
		// (get) Token: 0x06012C19 RID: 76825 RVA: 0x0058F998 File Offset: 0x0058DB98
		private MainTabWindow_Research.ResearchTabRecord CurTabRecord
		{
			get
			{
				foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord in this.tabs)
				{
					if (researchTabRecord.def == this.CurTab)
					{
						return researchTabRecord;
					}
				}
				return null;
			}
		}

		// Token: 0x17003126 RID: 12582
		// (get) Token: 0x06012C1A RID: 76826 RVA: 0x0058F9FC File Offset: 0x0058DBFC
		private bool ColonistsHaveResearchBench
		{
			get
			{
				bool result = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].listerBuildings.ColonistsHaveResearchBench())
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x17003127 RID: 12583
		// (get) Token: 0x06012C1B RID: 76827 RVA: 0x0058FA3C File Offset: 0x0058DC3C
		public List<ResearchProjectDef> VisibleResearchProjects
		{
			get
			{
				if (this.cachedVisibleResearchProjects == null)
				{
					this.cachedVisibleResearchProjects = new List<ResearchProjectDef>(from d in DefDatabase<ResearchProjectDef>.AllDefsListForReading
					where Find.Storyteller.difficulty.AllowedBy(d.hideWhen) || Find.ResearchManager.IsCurrentProject(d)
					select d);
				}
				return this.cachedVisibleResearchProjects;
			}
		}

		// Token: 0x17003128 RID: 12584
		// (get) Token: 0x06012C1C RID: 76828 RVA: 0x0058FA8C File Offset: 0x0058DC8C
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 initialSize = base.InitialSize;
				float b = (float)(UI.screenHeight - 35);
				float num = 0f;
				foreach (ResearchTabDef tab in DefDatabase<ResearchTabDef>.AllDefs)
				{
					num = Mathf.Max(num, this.ViewSize(tab).y);
				}
				float num2 = Mathf.Max(270f, (float)UI.screenWidth * 0.22f);
				float overflowTabHeight = TabDrawer.GetOverflowTabHeight<MainTabWindow_Research.ResearchTabRecord>(new Rect(num2, 0f, (float)UI.screenWidth - num2 - 200f - 4f, 0f), this.tabs, 100f, 200f);
				float b2 = this.Margin + 10f + overflowTabHeight + 10f + num + 10f + 10f + this.Margin;
				float a = Mathf.Max(initialSize.y, b2);
				initialSize.y = Mathf.Min(a, b);
				return initialSize;
			}
		}

		// Token: 0x06012C1D RID: 76829 RVA: 0x0058FBA0 File Offset: 0x0058DDA0
		private Vector2 ViewSize(ResearchTabDef tab)
		{
			List<ResearchProjectDef> visibleResearchProjects = this.VisibleResearchProjects;
			float num = 0f;
			float num2 = 0f;
			Text.Font = GameFont.Small;
			float num3 = 0f;
			if (ModsConfig.AnomalyActive && tab == ResearchTabDefOf.Anomaly)
			{
				num3 = 14f;
			}
			for (int i = 0; i < visibleResearchProjects.Count; i++)
			{
				ResearchProjectDef researchProjectDef = visibleResearchProjects[i];
				if (researchProjectDef.tab == tab)
				{
					Rect rect = new Rect(0f, 0f, 140f, 0f);
					Widgets.LabelCacheHeight(ref rect, this.GetLabelWithNewlineCached(this.GetLabel(researchProjectDef)), false, false);
					num = Mathf.Max(num, this.PosX(researchProjectDef) + 140f + num3);
					num2 = Mathf.Max(num2, this.PosY(researchProjectDef) + rect.height);
				}
			}
			return new Vector2(num + 20f + 4f, num2 + 20f + 4f);
		}

		// Token: 0x06012C1E RID: 76830 RVA: 0x0058FC8C File Offset: 0x0058DE8C
		public override void PreOpen()
		{
			base.PreOpen();
			this.UpdateSelectedProject(Find.ResearchManager);
			this.scrollPositioner.Arm(true);
			this.cachedVisibleResearchProjects = null;
			this.cachedUnlockedDefsGroupedByPrerequisites = null;
			this.quickSearchWidget.Reset();
			if (this.CurTab == null)
			{
				this.CurTab = ((this.selectedProject != null) ? this.selectedProject.tab : ResearchTabDefOf.Main);
			}
			this.UpdateSearchResults();
		}

		// Token: 0x06012C1F RID: 76831 RVA: 0x0058FCFD File Offset: 0x0058DEFD
		public void Select(ResearchProjectDef project)
		{
			this.CurTab = project.tab;
			this.selectedProject = project;
		}

		// Token: 0x06012C20 RID: 76832 RVA: 0x0058FD14 File Offset: 0x0058DF14
		private void UpdateSelectedProject(ResearchManager researchManager)
		{
			if (ModsConfig.AnomalyActive && this.curTabInt == ResearchTabDefOf.Anomaly)
			{
				this.selectedProject = null;
				using (List<ResearchManager.KnowledgeCategoryProject>.Enumerator enumerator = researchManager.CurrentAnomalyKnowledgeProjects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject = enumerator.Current;
						if (knowledgeCategoryProject.project != null)
						{
							this.selectedProject = knowledgeCategoryProject.project;
							break;
						}
					}
					return;
				}
			}
			this.selectedProject = researchManager.GetProject(null);
		}

		// Token: 0x06012C21 RID: 76833 RVA: 0x0058FDA0 File Offset: 0x0058DFA0
		public override void DoWindowContents(Rect inRect)
		{
			this.windowRect.width = (float)UI.screenWidth;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			float width = Mathf.Max(270f, inRect.width * 0.22f);
			Rect leftOutRect = new Rect(0f, 0f, width, inRect.height);
			Rect searchRect = new Rect(inRect.xMax - 200f, 0f, 200f, 24f);
			Rect rightOutRect = new Rect(leftOutRect.xMax + 10f, 0f, inRect.width - leftOutRect.width - 10f, inRect.height);
			this.DrawSearchRect(searchRect);
			this.DrawLeftRect(leftOutRect);
			this.DrawRightRect(rightOutRect, searchRect.x - 4f);
		}

		// Token: 0x06012C22 RID: 76834 RVA: 0x0058FE75 File Offset: 0x0058E075
		private void DrawSearchRect(Rect searchRect)
		{
			this.quickSearchWidget.OnGUI(searchRect, new Action(this.UpdateSearchResults), null);
		}

		// Token: 0x06012C23 RID: 76835 RVA: 0x0058FE90 File Offset: 0x0058E090
		private void DrawLeftRect(Rect leftOutRect)
		{
			Widgets.BeginGroup(leftOutRect);
			if (this.selectedProject != null)
			{
				this.DrawProjectInfo(leftOutRect);
			}
			else
			{
				this.DrawCurrentTabInfo(leftOutRect);
			}
			Widgets.EndGroup();
		}

		// Token: 0x06012C24 RID: 76836 RVA: 0x0058FEC4 File Offset: 0x0058E0C4
		private void DrawCurrentTabInfo(Rect rect)
		{
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.leftScrollViewHeight);
			Widgets.BeginScrollView(outRect, ref this.leftScrollPosition, viewRect, true);
			float num = 0f;
			Text.Font = GameFont.Medium;
			GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
			Rect rect2 = new Rect(0f, num, viewRect.width - 0f, 50f);
			Widgets.LabelCacheHeight(ref rect2, this.curTabInt.generalTitle.CapitalizeFirst(), true, false);
			GenUI.ResetLabelAlign();
			Text.Font = GameFont.Small;
			num += rect2.height;
			Rect rect3 = new Rect(0f, num, viewRect.width, 0f);
			Widgets.LabelCacheHeight(ref rect3, this.curTabInt.generalDescription, true, false);
			num += rect3.height;
			this.leftScrollViewHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x06012C25 RID: 76837 RVA: 0x0058FFC4 File Offset: 0x0058E1C4
		private void DrawProjectInfo(Rect rect)
		{
			int num = (ModsConfig.AnomalyActive && this.curTabInt == ResearchTabDefOf.Anomaly) ? 2 : 1;
			float num2 = (num > 1) ? (75f * (float)num) : 100f;
			Rect rect2 = rect;
			rect2.yMin = rect.yMax - num2;
			rect2.yMax = rect.yMax;
			Rect rect3 = rect2;
			Rect rect4 = rect2;
			rect4.y = rect2.y - 30f;
			rect4.height = 28f;
			rect2 = rect2.ContractedBy(10f);
			rect2.y += 5f;
			Text.Font = GameFont.Medium;
			string key = (num > 1) ? "ActiveProjectPlural" : "ActiveProject";
			Widgets.Label(rect4, key.Translate());
			Text.Font = GameFont.Small;
			Rect startButRect = new Rect
			{
				y = rect4.y - 55f - 10f,
				height = 55f,
				x = rect.center.x - rect.width / 4f,
				width = rect.width / 2f + 20f
			};
			Widgets.DrawMenuSection(rect3);
			if (ModsConfig.AnomalyActive && this.curTabInt == ResearchTabDefOf.Anomaly)
			{
				Rect rect5 = rect2;
				rect5.height = rect2.height / 2f;
				Rect rect6 = rect5;
				rect6.yMin = rect2.yMax - rect5.height;
				rect6.yMax = rect2.yMax;
				ResearchProjectDef project = Find.ResearchManager.GetProject(KnowledgeCategoryDefOf.Basic);
				ResearchProjectDef project2 = Find.ResearchManager.GetProject(KnowledgeCategoryDefOf.Advanced);
				if (project == null && project2 == null)
				{
					using (new TextBlock(TextAnchor.MiddleCenter))
					{
						Widgets.Label(rect2, "NoProjectSelected".Translate());
						goto IL_285;
					}
				}
				float prefixWidth = DefDatabase<KnowledgeCategoryDef>.AllDefs.Max((KnowledgeCategoryDef x) => Text.CalcSize(x.LabelCap + ":").x);
				this.DrawProjectProgress(rect5, project, KnowledgeCategoryDefOf.Basic.LabelCap, prefixWidth);
				this.DrawProjectProgress(rect6, project2, KnowledgeCategoryDefOf.Advanced.LabelCap, prefixWidth);
			}
			else
			{
				ResearchProjectDef project3 = Find.ResearchManager.GetProject(null);
				if (project3 == null)
				{
					using (new TextBlock(TextAnchor.MiddleCenter))
					{
						Widgets.Label(rect2, "NoProjectSelected".Translate());
						goto IL_285;
					}
				}
				this.DrawProjectProgress(rect2, project3, null, 75f);
			}
			IL_285:
			this.DrawStartButton(startButRect);
			if (Prefs.DevMode && !Find.ResearchManager.IsCurrentProject(this.selectedProject) && !this.selectedProject.IsFinished)
			{
				Text.Font = GameFont.Tiny;
				if (Widgets.ButtonText(new Rect(rect.xMax - 120f, rect4.y, 120f, 25f), "Debug: Finish now", true, true, true, null))
				{
					Find.ResearchManager.SetCurrentProject(this.selectedProject);
					Find.ResearchManager.FinishProject(this.selectedProject, false, null, true);
				}
				Text.Font = GameFont.Small;
			}
			if (Prefs.DevMode && !this.selectedProject.TechprintRequirementMet)
			{
				Text.Font = GameFont.Tiny;
				if (Widgets.ButtonText(new Rect(rect.xMax - 300f, rect4.y, 170f, 25f), "Debug: Apply techprint", true, true, true, null))
				{
					Find.ResearchManager.ApplyTechprint(this.selectedProject, null);
					SoundDefOf.TechprintApplied.PlayOneShotOnCamera(null);
				}
				Text.Font = GameFont.Small;
			}
			float y = 0f;
			this.DrawProjectPrimaryInfo(rect, ref y);
			this.DrawProjectScrollView(new Rect(0f, y, rect.width, 0f)
			{
				yMax = startButRect.yMin - 10f
			});
		}

		// Token: 0x06012C26 RID: 76838 RVA: 0x005903CC File Offset: 0x0058E5CC
		private void DrawProjectPrimaryInfo(Rect rect, ref float y)
		{
			using (new TextBlock(GameFont.Medium, TextAnchor.MiddleLeft))
			{
				Widgets.Label(new Rect(0f, y, rect.width - 0f, 50f), ref y, this.selectedProject.LabelCap, default(TipSignal));
			}
			y += 10f;
			string text = this.selectedProject.Description;
			if (ModsConfig.AnomalyActive && this.selectedProject.knowledgeCategory != null)
			{
				text = text + "\n\n" + "AnomalyResearchDescriptionHelpText".Translate().Colorize(ColoredText.SubtleGrayColor);
			}
			Widgets.Label(0f, ref y, rect.width, text, default(TipSignal));
			y += 10f;
			Widgets.DrawLineHorizontal(rect.x - 8f, y, rect.width, Color.gray);
			y += 10f;
			if (ModsConfig.AnomalyActive && this.selectedProject.knowledgeCategory != null)
			{
				Widgets.Label(0f, ref y, rect.width, "KnowledgeCategory".Translate() + ": " + this.selectedProject.knowledgeCategory.LabelCap, default(TipSignal));
			}
			Rect rect2 = new Rect(0f, y, rect.width, 500f);
			this.DrawTechprintInfo(rect2, ref y);
		}

		// Token: 0x06012C27 RID: 76839 RVA: 0x00590558 File Offset: 0x0058E758
		private void DrawProjectScrollView(Rect rect)
		{
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.leftScrollViewHeight);
			float num = 3f;
			Widgets.BeginScrollView(rect, ref this.leftScrollPosition, viewRect, true);
			if (this.selectedProject.techLevel > Faction.OfPlayer.def.techLevel)
			{
				float num2 = this.selectedProject.CostFactor(Faction.OfPlayer.def.techLevel);
				Rect rect2 = new Rect(0f, num, viewRect.width, 0f);
				string text = "TechLevelTooLow".Translate(Faction.OfPlayer.def.techLevel.ToStringHuman(), this.selectedProject.techLevel.ToStringHuman(), (1f / num2).ToStringPercent());
				if (num2 != 1f)
				{
					text += " " + "ResearchCostComparison".Translate(this.selectedProject.Cost.ToString("F0"), this.selectedProject.CostApparent.ToString("F0"));
				}
				Widgets.LabelCacheHeight(ref rect2, text, true, false);
				num += rect2.height;
			}
			Rect rect3 = new Rect(0f, num, viewRect.width, 0f);
			this.DrawResearchPrerequisites(rect3, ref num, this.selectedProject);
			Rect rect4 = new Rect(0f, num, viewRect.width, 500f);
			num += this.DrawResearchBenchRequirements(this.selectedProject, rect4);
			Rect rect5 = new Rect(0f, num, viewRect.width, 500f);
			num += this.DrawStudyRequirements(this.selectedProject, rect5);
			Rect rect6 = new Rect(0f, num, viewRect.width, 500f);
			num += this.DrawInspectionRequirements(this.selectedProject, rect6);
			Rect rect7 = new Rect(0f, num, viewRect.width, 500f);
			Rect visibleRect = new Rect(0f, this.leftScrollPosition.y, viewRect.width, rect.height);
			num += this.DrawUnlockableHyperlinks(rect7, visibleRect, this.selectedProject);
			num += this.DrawCustomUnlockables(new Rect(0f, num, viewRect.width, 500f), this.selectedProject);
			Rect rect8 = new Rect(0f, num, viewRect.width, 500f);
			num += this.DrawContentSource(rect8, this.selectedProject);
			num += 3f;
			this.leftScrollViewHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x06012C28 RID: 76840 RVA: 0x00590818 File Offset: 0x0058EA18
		private void DrawStartButton(Rect startButRect)
		{
			if (this.selectedProject.CanStartNow && !Find.ResearchManager.IsCurrentProject(this.selectedProject))
			{
				if (Widgets.ButtonText(startButRect, "Research".Translate(), true, true, true, null))
				{
					this.AttemptBeginResearch(this.selectedProject);
				}
				return;
			}
			if (Find.ResearchManager.IsCurrentProject(this.selectedProject))
			{
				if (Widgets.ButtonText(startButRect, "StopResearch".Translate(), true, true, true, null))
				{
					Find.ResearchManager.StopProject(this.selectedProject);
				}
				return;
			}
			MainTabWindow_Research.lockedReasons.Clear();
			string text;
			if (this.selectedProject.IsFinished)
			{
				text = "Finished".Translate();
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else if (Find.ResearchManager.IsCurrentProject(this.selectedProject))
			{
				text = "InProgress".Translate();
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				if (!this.selectedProject.PrerequisitesCompleted)
				{
					MainTabWindow_Research.lockedReasons.Add("PrerequisitesNotCompleted".Translate());
				}
				if (!this.selectedProject.TechprintRequirementMet)
				{
					MainTabWindow_Research.lockedReasons.Add("InsufficientTechprintsApplied".Translate(this.selectedProject.TechprintsApplied, this.selectedProject.TechprintCount));
				}
				if ((!ModsConfig.AnomalyActive || this.curTabInt != ResearchTabDefOf.Anomaly) && !this.selectedProject.PlayerHasAnyAppropriateResearchBench)
				{
					MainTabWindow_Research.lockedReasons.Add("MissingRequiredResearchFacilities".Translate());
				}
				if (!this.selectedProject.PlayerMechanitorRequirementMet)
				{
					MainTabWindow_Research.lockedReasons.Add("MissingRequiredMechanitor".Translate());
				}
				if (!this.selectedProject.InspectionRequirementsMet)
				{
					MainTabWindow_Research.lockedReasons.Add("MissingGravEngineInspection".Translate());
				}
				if (!this.selectedProject.AnalyzedThingsRequirementsMet)
				{
					for (int i = 0; i < this.selectedProject.requiredAnalyzed.Count; i++)
					{
						MainTabWindow_Research.lockedReasons.Add("NotStudied".Translate(this.selectedProject.requiredAnalyzed[i].LabelCap));
					}
				}
				if (MainTabWindow_Research.lockedReasons.NullOrEmpty<string>())
				{
					Log.ErrorOnce("Research " + this.selectedProject.defName + " locked but no reasons given", this.selectedProject.GetHashCode() ^ 100543441);
				}
				text = "Locked".Translate();
			}
			Widgets.DrawHighlight(startButRect);
			startButRect = startButRect.ContractedBy(4f);
			string text2 = text;
			if (!MainTabWindow_Research.lockedReasons.NullOrEmpty<string>())
			{
				text2 = text2 + ":\n" + MainTabWindow_Research.lockedReasons.ToLineList("  ");
			}
			Vector2 vector = Text.CalcSize(text2);
			if (vector.x > startButRect.width || vector.y > startButRect.height)
			{
				TooltipHandler.TipRegion(startButRect.ExpandedBy(4f), text2);
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				text = text2;
			}
			Widgets.Label(startButRect, text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06012C29 RID: 76841 RVA: 0x00590B48 File Offset: 0x0058ED48
		private void DrawProjectProgress(Rect rect, ResearchProjectDef project, string prefixTitle = null, float prefixWidth = 75f)
		{
			Rect rect2 = rect;
			if (!string.IsNullOrEmpty(prefixTitle))
			{
				Rect rect3 = rect2;
				rect3.width = prefixWidth;
				rect2.xMin = rect3.xMax + 10f;
				using (new TextBlock(TextAnchor.MiddleLeft))
				{
					Widgets.Label(rect3, prefixTitle + ":");
				}
			}
			if (project == null)
			{
				using (new TextBlock(TextAnchor.MiddleCenter))
				{
					Widgets.Label(rect2, "NoProjectSelected".Translate());
				}
				return;
			}
			rect2 = rect2.ContractedBy(15f);
			Widgets.FillableBar(rect2, project.ProgressPercent, MainTabWindow_Research.ResearchBarFillTex, MainTabWindow_Research.ResearchBarBGTex, true);
			Text.Anchor = TextAnchor.MiddleCenter;
			string label = project.ProgressApparentString + " / " + project.CostApparent.ToString("F0");
			Widgets.Label(rect2, label);
			Rect rect4 = rect2;
			rect4.y = rect2.y - 22f;
			rect4.height = 22f;
			float x = Text.CalcSize(project.LabelCap).x;
			Widgets.Label(rect4, project.LabelCap.Truncate(rect4.width, null));
			if (x > rect4.width)
			{
				TooltipHandler.TipRegion(rect4, project.LabelCap);
				Widgets.DrawHighlightIfMouseover(rect4);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06012C2A RID: 76842 RVA: 0x00590CBC File Offset: 0x0058EEBC
		private void AttemptBeginResearch(ResearchProjectDef projectToStart)
		{
			List<ValueTuple<BuildableDef, List<string>>> list = this.ComputeUnlockedDefsThatHaveMissingMemes(projectToStart);
			if (!list.Any<ValueTuple<BuildableDef, List<string>>>())
			{
				this.DoBeginResearch(projectToStart);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ResearchProjectHasDefsWithMissingMemes".Translate(projectToStart.LabelCap)).Append(":");
			stringBuilder.AppendLine();
			foreach (ValueTuple<BuildableDef, List<string>> valueTuple in list)
			{
				BuildableDef item = valueTuple.Item1;
				List<string> item2 = valueTuple.Item2;
				stringBuilder.AppendLine();
				stringBuilder.Append("  - ").Append(item.LabelCap.Colorize(ColoredText.NameColor)).Append(" (").Append(item2.ToCommaList(false, false)).Append(")");
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), delegate()
			{
				this.DoBeginResearch(projectToStart);
			}, false, null, WindowLayer.Dialog));
			SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
		}

		// Token: 0x06012C2B RID: 76843 RVA: 0x00590E04 File Offset: 0x0058F004
		private List<ValueTuple<BuildableDef, List<string>>> ComputeUnlockedDefsThatHaveMissingMemes(ResearchProjectDef project)
		{
			this.cachedDefsWithMissingMemes.Clear();
			if (!ModsConfig.IdeologyActive)
			{
				return this.cachedDefsWithMissingMemes;
			}
			FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
			if (((ideos != null) ? ideos.PrimaryIdeo : null) == null)
			{
				return this.cachedDefsWithMissingMemes;
			}
			foreach (Def def in project.UnlockedDefs)
			{
				BuildableDef buildableDef = def as BuildableDef;
				if (buildableDef != null && !buildableDef.canGenerateDefaultDesignator)
				{
					List<string> list = null;
					foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefsListForReading)
					{
						if (!Faction.OfPlayer.ideos.HasAnyIdeoWithMeme(memeDef) && memeDef.AllDesignatorBuildables.Contains(buildableDef))
						{
							if (list == null)
							{
								list = new List<string>();
							}
							list.Add(memeDef.LabelCap);
						}
					}
					if (list != null)
					{
						this.cachedDefsWithMissingMemes.Add(new ValueTuple<BuildableDef, List<string>>(buildableDef, list));
					}
				}
			}
			return this.cachedDefsWithMissingMemes;
		}

		// Token: 0x06012C2C RID: 76844 RVA: 0x00590F3C File Offset: 0x0058F13C
		private void DoBeginResearch(ResearchProjectDef projectToStart)
		{
			SoundDefOf.ResearchStart.PlayOneShotOnCamera(null);
			Find.ResearchManager.SetCurrentProject(projectToStart);
			TutorSystem.Notify_Event("StartResearchProject");
			if ((!ModsConfig.AnomalyActive || projectToStart.knowledgeCategory == null) && !this.ColonistsHaveResearchBench)
			{
				Messages.Message("MessageResearchMenuWithoutBench".Translate(), MessageTypeDefOf.CautionInput, true);
			}
		}

		// Token: 0x06012C2D RID: 76845 RVA: 0x00590F9F File Offset: 0x0058F19F
		private float CoordToPixelsX(float x)
		{
			return x * 190f;
		}

		// Token: 0x06012C2E RID: 76846 RVA: 0x00590FA8 File Offset: 0x0058F1A8
		private float CoordToPixelsY(float y)
		{
			return y * 100f;
		}

		// Token: 0x06012C2F RID: 76847 RVA: 0x00590FB1 File Offset: 0x0058F1B1
		private float PixelsToCoordX(float x)
		{
			return x / 190f;
		}

		// Token: 0x06012C30 RID: 76848 RVA: 0x00590FBA File Offset: 0x0058F1BA
		private float PixelsToCoordY(float y)
		{
			return y / 100f;
		}

		// Token: 0x06012C31 RID: 76849 RVA: 0x00590FC3 File Offset: 0x0058F1C3
		private float PosX(ResearchProjectDef d)
		{
			return this.CoordToPixelsX(d.ResearchViewX);
		}

		// Token: 0x06012C32 RID: 76850 RVA: 0x00590FD1 File Offset: 0x0058F1D1
		private float PosY(ResearchProjectDef d)
		{
			return this.CoordToPixelsY(d.ResearchViewY);
		}

		// Token: 0x06012C33 RID: 76851 RVA: 0x00590FE0 File Offset: 0x0058F1E0
		public override void PostOpen()
		{
			base.PostOpen();
			this.tabs.Clear();
			using (IEnumerator<ResearchTabDef> enumerator = DefDatabase<ResearchTabDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ResearchTabDef tabDef = enumerator.Current;
					this.tabs.Add(new MainTabWindow_Research.ResearchTabRecord(tabDef, tabDef.LabelCap, delegate()
					{
						this.CurTab = tabDef;
						this.UpdateSelectedProject(Find.ResearchManager);
					}, () => this.CurTab == tabDef));
				}
			}
		}

		// Token: 0x06012C34 RID: 76852 RVA: 0x00591088 File Offset: 0x0058F288
		private void DrawRightRect(Rect rightOutRect, float maxTabX)
		{
			Rect baseRect = rightOutRect;
			baseRect.xMax = maxTabX;
			rightOutRect.yMin += TabDrawer.GetOverflowTabHeight<MainTabWindow_Research.ResearchTabRecord>(baseRect, this.tabs, 100f, 200f);
			Widgets.DrawMenuSection(rightOutRect);
			TabDrawer.DrawTabsOverflow<MainTabWindow_Research.ResearchTabRecord>(baseRect, this.tabs, 100f, 200f);
			if (Prefs.DevMode)
			{
				Rect rect = rightOutRect;
				rect.yMax = rect.yMin + 20f;
				rect.xMin = rect.xMax - 80f;
				Rect butRect = rect.RightPartPixels(30f);
				rect = rect.LeftPartPixels(rect.width - 30f);
				Widgets.CheckboxLabeled(rect, "Edit", ref this.editMode, false, null, null, false, false);
				if (Widgets.ButtonImageFitted(butRect, TexButton.Copy))
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (ResearchProjectDef researchProjectDef in from def in this.VisibleResearchProjects
					where def.Debug_IsPositionModified()
					select def)
					{
						stringBuilder.AppendLine(researchProjectDef.defName);
						stringBuilder.AppendLine(string.Format("  <researchViewX>{0:F2}</researchViewX>", researchProjectDef.ResearchViewX));
						stringBuilder.AppendLine(string.Format("  <researchViewY>{0:F2}</researchViewY>", researchProjectDef.ResearchViewY));
						stringBuilder.AppendLine();
					}
					GUIUtility.systemCopyBuffer = stringBuilder.ToString();
					Messages.Message("Modified data copied to clipboard.", MessageTypeDefOf.SituationResolved, false);
				}
			}
			else
			{
				this.editMode = false;
			}
			bool flag = false;
			Rect rect2 = rightOutRect.ContractedBy(1f);
			Rect rect3 = new Rect(0f, 0f, Mathf.Max(this.rightViewWidth + 20f, rect2.width), rightOutRect.height - 16f);
			Rect rect4 = rect3.ContractedBy(20f);
			Rect rect5 = rect3;
			rect5.height = rightOutRect.height;
			this.scrollPositioner.ClearInterestRects();
			if (Find.ResearchManager.TabInfoVisible(this.CurTab))
			{
				Widgets.ScrollHorizontal(rect2, ref this.rightScrollPosition, rect3, 20f);
				Widgets.BeginScrollView(rect2, ref this.rightScrollPosition, rect3, true);
				if (this.CurTab == ResearchTabDefOf.Anomaly)
				{
					Color color = GUI.color;
					GUI.color = new Color(1f, 1f, 1f, 0.15f);
					GUI.DrawTexture(rect5.LeftPartPixels(830f), MainTabWindow_Research.BasicBackgroundTex.Texture, ScaleMode.StretchToFill);
					GUI.DrawTexture(rect5.RightPartPixels(rect5.width - 830f + 1f), MainTabWindow_Research.AdvancedBackgroundTex.Texture, ScaleMode.StretchToFill);
					GUI.color = color;
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnomalyResearch, OpportunityType.GoodToKnow);
				}
				Widgets.BeginGroup(rect4);
				this.ListProjects(rect4, ref flag);
				Widgets.EndGroup();
				Widgets.EndScrollView();
				this.scrollPositioner.ScrollHorizontally(ref this.rightScrollPosition, rect2.size);
				if (this.editMode)
				{
					if (!flag && Input.GetMouseButtonDown(0))
					{
						this.draggingTabs.Clear();
					}
					if (!this.draggingTabs.NullOrEmpty<ResearchProjectDef>())
					{
						if (Input.GetMouseButtonUp(0))
						{
							for (int i = 0; i < this.draggingTabs.Count; i++)
							{
								this.draggingTabs[i].Debug_SnapPositionData();
							}
							return;
						}
						if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && Event.current.type == EventType.Layout)
						{
							for (int j = 0; j < this.draggingTabs.Count; j++)
							{
								this.draggingTabs[j].Debug_ApplyPositionDelta(new Vector2(this.PixelsToCoordX(Event.current.delta.x), this.PixelsToCoordY(Event.current.delta.y)));
							}
							return;
						}
					}
				}
			}
			else
			{
				Widgets.BeginGroup(rect2);
				using (new TextBlock(TextAnchor.MiddleCenter, ColoredText.SubtleGrayColor))
				{
					Widgets.Label(rect2.AtZero(), "ResearchNotDiscovered".Translate());
				}
				Widgets.EndGroup();
			}
		}

		// Token: 0x06012C35 RID: 76853 RVA: 0x005914CC File Offset: 0x0058F6CC
		private void ListProjects(Rect rightInRect, ref bool elementClicked)
		{
			List<ResearchProjectDef> visibleResearchProjects = this.VisibleResearchProjects;
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			for (int i = 0; i < 2; i++)
			{
				foreach (ResearchProjectDef researchProjectDef in visibleResearchProjects)
				{
					if (researchProjectDef.tab == this.CurTab)
					{
						float num = 0f;
						if (ModsConfig.AnomalyActive && researchProjectDef.knowledgeCategory != null)
						{
							num = 14f;
						}
						start.x = this.PosX(researchProjectDef);
						start.y = this.PosY(researchProjectDef) + 25f;
						for (int j = 0; j < researchProjectDef.prerequisites.CountAllowNull<ResearchProjectDef>(); j++)
						{
							ResearchProjectDef researchProjectDef2 = researchProjectDef.prerequisites[j];
							if (researchProjectDef2 != null && researchProjectDef2.tab == this.CurTab)
							{
								end.x = this.PosX(researchProjectDef2) + 140f + num;
								end.y = this.PosY(researchProjectDef2) + 25f;
								if (this.selectedProject == researchProjectDef || this.selectedProject == researchProjectDef2)
								{
									if (i == 1)
									{
										Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
									}
								}
								else if (i == 0)
								{
									Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
								}
							}
						}
					}
				}
			}
			Rect other = new Rect(this.rightScrollPosition.x, this.rightScrollPosition.y, rightInRect.width, rightInRect.height).ExpandedBy(10f);
			using (List<ResearchProjectDef>.Enumerator enumerator = visibleResearchProjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ResearchProjectDef project = enumerator.Current;
					if (project.tab == this.CurTab)
					{
						float num2 = 0f;
						if (ModsConfig.AnomalyActive && project.knowledgeCategory != null)
						{
							num2 = 14f;
						}
						Rect rect = new Rect(this.PosX(project), this.PosY(project), 140f, 50f);
						rect.xMax += num2;
						string label = this.GetLabel(project);
						Rect rect2 = rect;
						rect2.xMin += num2;
						Widgets.LabelCacheHeight(ref rect2, label, true, false);
						Rect rect3 = rect2;
						rect3.y = rect2.yMax - 4f;
						Widgets.LabelCacheHeight(ref rect3, " ", true, false);
						rect.yMax = rect3.yMax;
						bool flag = this.quickSearchWidget.filter.Active && !this.matchingProjects.Contains(project);
						bool flag2 = this.quickSearchWidget.filter.Active && this.matchingProjects.Contains(project);
						if (flag2 || this.selectedProject == project)
						{
							this.scrollPositioner.RegisterInterestRect(rect);
						}
						if (project.IsHidden)
						{
							label = string.Format("({0})", "UnknownResearch".Translate());
						}
						if (rect.Overlaps(other))
						{
							Color color = Widgets.NormalOptionColor;
							Color color2 = TexUI.OtherActiveResearchColor;
							Color windowBGFillColor = Widgets.WindowBGFillColor;
							Color color3 = default(Color);
							bool flag3 = !project.IsFinished && !project.CanStartNow;
							bool flag4 = false;
							if (project.IsHidden)
							{
								color2 = TexUI.HiddenResearchColor;
								color = MainTabWindow_Research.HiddenProjectLabelColor;
							}
							else if (Find.ResearchManager.IsCurrentProject(project))
							{
								color2 = TexUI.ActiveResearchColor;
								color = MainTabWindow_Research.ActiveProjectLabelColor;
							}
							else if (project.IsFinished)
							{
								color2 = TexUI.FinishedResearchColor;
							}
							else if (flag3)
							{
								color2 = TexUI.LockedResearchColor;
							}
							if (flag3)
							{
								color = MainTabWindow_Research.ProjectWithMissingPrerequisiteLabelColor;
							}
							if (this.editMode && this.draggingTabs.Contains(project))
							{
								color3 = Color.yellow;
							}
							else if (this.selectedProject == project)
							{
								color2 += TexUI.HighlightBgResearchColor;
								color3 = TexUI.BorderResearchSelectedColor;
								color = TexUI.BorderResearchSelectedColor;
								flag4 = true;
							}
							else if (Find.ResearchManager.IsCurrentProject(project))
							{
								color3 = TexUI.BorderResearchingColor;
								flag4 = true;
							}
							else
							{
								color3 = TexUI.DefaultBorderResearchColor;
							}
							if (this.selectedProject != null)
							{
								if (project.prerequisites.NotNullAndContains(this.selectedProject) || project.hiddenPrerequisites.NotNullAndContains(this.selectedProject))
								{
									color3 = TexUI.HighlightLineResearchColor;
								}
								if (this.selectedProject.prerequisites.NotNullAndContains(project) || this.selectedProject.hiddenPrerequisites.NotNullAndContains(project))
								{
									color3 = (project.IsFinished ? TexUI.HighlightLineResearchColor : TexUI.DependencyOutlineResearchColor);
								}
							}
							Color color4 = project.TechprintRequirementMet ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor;
							Color color5 = project.AnalyzedThingsRequirementsMet ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor;
							if (flag)
							{
								color = this.NoMatchTint(color);
								color2 = this.NoMatchTint(color2);
								color3 = this.NoMatchTint(color3);
								color4 = this.NoMatchTint(color4);
								color5 = this.NoMatchTint(color5);
							}
							if (flag2)
							{
								Widgets.DrawStrongHighlight(rect.ExpandedBy(4f), null);
							}
							int num3 = flag4 ? 2 : 1;
							if (Widgets.CustomButtonText(ref rect, "", color2, color, color3, windowBGFillColor, false, (float)num3, true, true, project.ProgressPercent) && !project.IsHidden)
							{
								SoundDefOf.Click.PlayOneShotOnCamera(null);
								this.selectedProject = project;
							}
							Color color6 = GUI.color;
							TextAnchor anchor = Text.Anchor;
							if (ModsConfig.AnomalyActive && project.knowledgeCategory != null)
							{
								Rect position = rect;
								position.x += 4f;
								position.width = 14f;
								GUI.color = project.knowledgeCategory.color;
								GUI.DrawTexture(position, ContentFinder<Texture2D>.Get(project.knowledgeCategory.texPath, true), ScaleMode.ScaleToFit);
							}
							GUI.color = color;
							Text.Anchor = TextAnchor.UpperCenter;
							Widgets.Label(rect2, label);
							if (!project.IsHidden)
							{
								this.DrawBottomRow(rect3, project, color4, color5);
								if (Mouse.IsOver(rect) && !this.editMode)
								{
									Widgets.DrawLightHighlight(rect);
									TooltipHandler.TipRegion(rect, () => project.GetTip(), project.GetHashCode() ^ 91727);
								}
							}
							GUI.color = color6;
							Text.Anchor = anchor;
							if (this.editMode && Mouse.IsOver(rect2) && Input.GetMouseButtonDown(0))
							{
								elementClicked = true;
								if (Input.GetKey(KeyCode.LeftShift))
								{
									if (!this.draggingTabs.Contains(project))
									{
										this.draggingTabs.Add(project);
									}
								}
								else if (!Input.GetKey(KeyCode.LeftControl) && !this.draggingTabs.Contains(project))
								{
									this.draggingTabs.Clear();
									this.draggingTabs.Add(project);
								}
								if (Input.GetKey(KeyCode.LeftControl) && this.draggingTabs.Contains(project))
								{
									this.draggingTabs.Remove(project);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06012C36 RID: 76854 RVA: 0x00591CCC File Offset: 0x0058FECC
		private void DrawBottomRow(Rect rect, ResearchProjectDef project, Color techprintColor, Color studiedColor)
		{
			Color color = GUI.color;
			TextAnchor anchor = Text.Anchor;
			int num = 1;
			if (project.TechprintCount > 0)
			{
				num++;
			}
			if (project.RequiredAnalyzedThingCount > 0)
			{
				num++;
			}
			float num2 = rect.width / (float)num;
			Rect rect2 = rect;
			rect2.x = rect.x;
			rect2.width = num2;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect2, project.CostApparent.ToString());
			rect2.x += num2;
			if (project.TechprintCount > 0)
			{
				string text = this.GetTechprintsInfoCached(project.TechprintsApplied, project.TechprintCount);
				Vector2 vector = Text.CalcSize(text);
				Rect rect3 = rect2;
				rect3.xMin = rect2.xMax - vector.x - 10f;
				Rect rect4 = rect2;
				rect4.width = rect4.height;
				rect4.x = rect3.x - rect4.width;
				GUI.color = techprintColor;
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				GUI.DrawTexture(rect4.ContractedBy(4f), MainTabWindow_Research.TechprintRequirementTex.Texture);
				rect2.x += num2;
			}
			if (project.RequiredAnalyzedThingCount > 0)
			{
				string text2 = this.GetTechprintsInfoCached(project.AnalyzedThingsCompleted, project.RequiredAnalyzedThingCount);
				Vector2 vector2 = Text.CalcSize(text2);
				Rect rect5 = rect2;
				rect5.xMin = rect2.xMax - vector2.x - 10f;
				Rect rect6 = rect2;
				rect6.width = rect6.height;
				rect6.x = rect5.x - rect6.width;
				GUI.color = studiedColor;
				Widgets.Label(rect5, text2);
				GUI.color = Color.white;
				GUI.DrawTexture(rect6.ContractedBy(4f), MainTabWindow_Research.StudyRequirementTex.Texture);
			}
			GUI.color = color;
			Text.Anchor = anchor;
		}

		// Token: 0x06012C37 RID: 76855 RVA: 0x00591EAE File Offset: 0x005900AE
		private Color NoMatchTint(Color color)
		{
			return Color.Lerp(color, MainTabWindow_Research.NoMatchTintColor, 0.4f);
		}

		// Token: 0x06012C38 RID: 76856 RVA: 0x00591EC0 File Offset: 0x005900C0
		private void DrawResearchPrerequisites(Rect rect, ref float y, ResearchProjectDef project)
		{
			if (project.prerequisites.NullOrEmpty<ResearchProjectDef>() && project.hiddenPrerequisites.NullOrEmpty<ResearchProjectDef>())
			{
				return;
			}
			Widgets.Label(rect, ref y, string.Format("{0}:", "Prerequisites".Translate()), default(TipSignal));
			rect.xMin += 6f;
			foreach (ResearchProjectDef researchProjectDef in project.prerequisites.OrElseEmptyEnumerable<ResearchProjectDef>())
			{
				this.SetPrerequisiteStatusColor(researchProjectDef.IsFinished, project);
				Widgets.Label(rect, ref y, string.Format("- {0}", researchProjectDef.LabelCap), default(TipSignal));
			}
			if (project.hiddenPrerequisites != null)
			{
				foreach (ResearchProjectDef researchProjectDef2 in project.hiddenPrerequisites)
				{
					this.SetPrerequisiteStatusColor(researchProjectDef2.IsFinished, project);
					Widgets.Label(rect, ref y, string.Format("- {0}", researchProjectDef2.LabelCap), default(TipSignal));
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x06012C39 RID: 76857 RVA: 0x00592018 File Offset: 0x00590218
		private string GetLabelWithNewlineCached(string label)
		{
			if (!MainTabWindow_Research.labelsWithNewlineCached.ContainsKey(label))
			{
				MainTabWindow_Research.labelsWithNewlineCached.Add(label, label + "\n");
			}
			return MainTabWindow_Research.labelsWithNewlineCached[label];
		}

		// Token: 0x06012C3A RID: 76858 RVA: 0x00592048 File Offset: 0x00590248
		private string GetTechprintsInfoCached(int applied, int total)
		{
			Pair<int, int> key = new Pair<int, int>(applied, total);
			if (!MainTabWindow_Research.techprintsInfoCached.ContainsKey(key))
			{
				MainTabWindow_Research.techprintsInfoCached.Add(key, applied.ToString() + " / " + total.ToString());
			}
			return MainTabWindow_Research.techprintsInfoCached[key];
		}

		// Token: 0x06012C3B RID: 76859 RVA: 0x0059209C File Offset: 0x0059029C
		private float DrawResearchBenchRequirements(ResearchProjectDef project, Rect rect)
		{
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			if (project.requiredResearchBuilding != null)
			{
				bool present = false;
				Predicate<Building> <>9__0;
				foreach (Map map in Find.Maps)
				{
					List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
					Predicate<Building> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((Building x) => x.def == project.requiredResearchBuilding));
					}
					if (allBuildingsColonist.Find(match) != null)
					{
						present = true;
						break;
					}
				}
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBench".Translate() + ":", true, false);
				rect.xMin += 6f;
				rect.yMin += rect.height;
				this.SetPrerequisiteStatusColor(present, project);
				rect.height = Text.CalcHeight(project.requiredResearchBuilding.LabelCap, rect.width - 24f - 6f);
				Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(project.requiredResearchBuilding, -1), null, 2f, 6f, null, false, null);
				rect.yMin += rect.height + 4f;
				GUI.color = Color.white;
				rect.xMin = xMin;
			}
			if (!project.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBenchFacilities".Translate() + ":", true, false);
				rect.yMin += rect.height;
				Building_ResearchBench building_ResearchBench = this.FindBenchFulfillingMostRequirements(project.requiredResearchBuilding, project.requiredResearchFacilities);
				CompAffectedByFacilities bestMatchingBench = null;
				if (building_ResearchBench != null)
				{
					bestMatchingBench = building_ResearchBench.TryGetComp<CompAffectedByFacilities>();
				}
				rect.xMin += 6f;
				foreach (ThingDef requiredFacility in project.requiredResearchFacilities)
				{
					this.DrawResearchBenchFacilityRequirement(requiredFacility, bestMatchingBench, project, ref rect);
					rect.yMin += rect.height;
				}
				rect.yMin += 4f;
			}
			GUI.color = Color.white;
			rect.xMin = xMin;
			return rect.yMin - yMin;
		}

		// Token: 0x06012C3C RID: 76860 RVA: 0x00592358 File Offset: 0x00590558
		private float DrawStudyRequirements(ResearchProjectDef project, Rect rect)
		{
			float yMin = rect.yMin;
			if (project.RequiredAnalyzedThingCount > 0)
			{
				Widgets.LabelCacheHeight(ref rect, "StudyRequirements".Translate() + ":", true, false);
				rect.xMin += 6f;
				rect.yMin += rect.height;
				foreach (ThingDef thingDef in project.requiredAnalyzed)
				{
					Rect rect2 = new Rect(rect.x, rect.yMin, rect.width, 24f);
					Color? color = null;
					if (this.quickSearchWidget.filter.Active)
					{
						if (this.MatchesUnlockedDef(thingDef))
						{
							Widgets.DrawTextHighlight(rect2, 4f, null);
						}
						else
						{
							color = new Color?(this.NoMatchTint(Widgets.NormalOptionColor));
						}
					}
					Dialog_InfoCard.Hyperlink hyperlink = new Dialog_InfoCard.Hyperlink(thingDef, -1);
					Widgets.HyperlinkWithIcon(rect2, hyperlink, null, 2f, 6f, color, false, this.LabelSuffixForUnlocked(thingDef));
					rect.yMin += 24f;
				}
			}
			return rect.yMin - yMin;
		}

		// Token: 0x06012C3D RID: 76861 RVA: 0x005924B8 File Offset: 0x005906B8
		private float DrawInspectionRequirements(ResearchProjectDef project, Rect rect)
		{
			float yMin = rect.yMin;
			if (ModsConfig.OdysseyActive && project.requireGravEngineInspected)
			{
				Widgets.LabelCacheHeight(ref rect, "RequiresInspectionof".Translate() + ":", true, false);
				rect.yMin += 24f;
				rect.xMin += 6f;
				Rect rect2 = new Rect(rect.x, rect.yMin, rect.width, 24f);
				Widgets.HyperlinkWithIcon(rect2, new Dialog_InfoCard.Hyperlink(ThingDefOf.GravEngine, -1), null, 2f, 6f, null, false, null);
				rect.yMin += rect2.height;
			}
			return rect.yMin - yMin;
		}

		// Token: 0x06012C3E RID: 76862 RVA: 0x00592590 File Offset: 0x00590790
		private float DrawUnlockableHyperlinks(Rect rect, Rect visibleRect, ResearchProjectDef project)
		{
			List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> list = this.UnlockedDefsGroupedByPrerequisites(project);
			if (list.NullOrEmpty<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>())
			{
				return 0f;
			}
			float yMin = rect.yMin;
			float x = rect.x;
			foreach (Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>> pair in list)
			{
				ResearchPrerequisitesUtility.UnlockedHeader first = pair.First;
				rect.x = x;
				if (!first.unlockedBy.Any<ResearchProjectDef>())
				{
					Widgets.LabelCacheHeight(ref rect, "Unlocks".Translate() + ":", true, false);
				}
				else
				{
					Widgets.LabelCacheHeight(ref rect, "UnlockedWith".Translate() + " " + this.HeaderLabel(first) + ":", true, false);
				}
				rect.x += 6f;
				rect.yMin += rect.height;
				foreach (Def def in pair.Second)
				{
					Rect rect2 = new Rect(rect.x, rect.yMin, rect.width, 24f);
					if (visibleRect.Overlaps(rect2))
					{
						Color? color = null;
						if (this.quickSearchWidget.filter.Active)
						{
							if (this.MatchesUnlockedDef(def))
							{
								Widgets.DrawTextHighlight(rect2, 4f, null);
							}
							else
							{
								color = new Color?(this.NoMatchTint(Widgets.NormalOptionColor));
							}
						}
						Dialog_InfoCard.Hyperlink hyperlink = new Dialog_InfoCard.Hyperlink(def, -1);
						Widgets.HyperlinkWithIcon(rect2, hyperlink, null, 2f, 6f, color, false, this.LabelSuffixForUnlocked(def));
					}
					rect.yMin += 24f;
				}
			}
			return rect.yMin - yMin;
		}

		// Token: 0x06012C3F RID: 76863 RVA: 0x005927BC File Offset: 0x005909BC
		private float DrawCustomUnlockables(Rect rect, ResearchProjectDef project)
		{
			if (project.customUnlockTexts.NullOrEmpty<string>())
			{
				return 0f;
			}
			float yMin = rect.yMin;
			for (int i = 0; i < project.customUnlockTexts.Count; i++)
			{
				if (i == 0)
				{
					Widgets.LabelCacheHeight(ref rect, "Unlocks".Translate() + ":", true, false);
					rect.x += 6f;
					rect.yMin += rect.height;
				}
				Widgets.LabelCacheHeight(ref rect, project.customUnlockTexts[i], true, false);
				rect.yMin += rect.height;
			}
			return rect.yMin - yMin;
		}

		// Token: 0x06012C40 RID: 76864 RVA: 0x00592880 File Offset: 0x00590A80
		private float DrawContentSource(Rect rect, ResearchProjectDef project)
		{
			if (project.modContentPack == null || project.modContentPack.IsCoreMod)
			{
				return 0f;
			}
			float yMin = rect.yMin;
			TaggedString taggedString = "Stat_Source_Label".Translate() + ":  " + project.modContentPack.Name;
			Widgets.LabelCacheHeight(ref rect, taggedString.Colorize(Color.grey), true, false);
			ExpansionDef expansionDef = ModLister.AllExpansions.Find((ExpansionDef e) => e.linkedMod == project.modContentPack.PackageId);
			if (expansionDef != null)
			{
				GUI.DrawTexture(new Rect(Text.CalcSize(taggedString).x + 4f, rect.y, 20f, 20f), expansionDef.IconFromStatus);
			}
			return rect.yMax - yMin;
		}

		// Token: 0x06012C41 RID: 76865 RVA: 0x00592960 File Offset: 0x00590B60
		private string LabelSuffixForUnlocked(Def unlocked)
		{
			if (!ModLister.IdeologyInstalled)
			{
				return null;
			}
			this.tmpSuffixesForUnlocked.Clear();
			foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefs)
			{
				if (memeDef.AllDesignatorBuildables.Contains(unlocked))
				{
					this.tmpSuffixesForUnlocked.AddUnique(memeDef.LabelCap);
				}
				if (!memeDef.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
				{
					using (List<ThingStyleCategoryWithPriority>.Enumerator enumerator2 = memeDef.thingStyleCategories.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.category.AllDesignatorBuildables.Contains(unlocked))
							{
								this.tmpSuffixesForUnlocked.AddUnique(memeDef.LabelCap);
							}
						}
					}
				}
			}
			foreach (CultureDef cultureDef in DefDatabase<CultureDef>.AllDefs)
			{
				if (!cultureDef.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
				{
					using (List<ThingStyleCategoryWithPriority>.Enumerator enumerator2 = cultureDef.thingStyleCategories.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.category.AllDesignatorBuildables.Contains(unlocked))
							{
								this.tmpSuffixesForUnlocked.AddUnique(cultureDef.LabelCap);
							}
						}
					}
				}
			}
			if (!this.tmpSuffixesForUnlocked.Any<string>())
			{
				return null;
			}
			return " (" + this.tmpSuffixesForUnlocked.ToCommaList(false, false) + ")";
		}

		// Token: 0x06012C42 RID: 76866 RVA: 0x00592B24 File Offset: 0x00590D24
		private List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> UnlockedDefsGroupedByPrerequisites(ResearchProjectDef project)
		{
			if (this.cachedUnlockedDefsGroupedByPrerequisites == null)
			{
				this.cachedUnlockedDefsGroupedByPrerequisites = new Dictionary<ResearchProjectDef, List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>>();
			}
			List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> list;
			if (!this.cachedUnlockedDefsGroupedByPrerequisites.TryGetValue(project, out list))
			{
				list = ResearchPrerequisitesUtility.UnlockedDefsGroupedByPrerequisites(project);
				this.cachedUnlockedDefsGroupedByPrerequisites.Add(project, list);
			}
			return list;
		}

		// Token: 0x06012C43 RID: 76867 RVA: 0x00592B6C File Offset: 0x00590D6C
		private string HeaderLabel(ResearchPrerequisitesUtility.UnlockedHeader headerProject)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = "";
			for (int i = 0; i < headerProject.unlockedBy.Count; i++)
			{
				ResearchProjectDef researchProjectDef = headerProject.unlockedBy[i];
				string text = researchProjectDef.LabelCap;
				if (!researchProjectDef.IsFinished)
				{
					text = text.Colorize(MainTabWindow_Research.MissingPrerequisiteColor);
				}
				stringBuilder.Append(value).Append(text);
				value = ", ";
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06012C44 RID: 76868 RVA: 0x00592BE0 File Offset: 0x00590DE0
		private void DrawTechprintInfo(Rect rect, ref float y)
		{
			if (this.selectedProject.TechprintCount == 0)
			{
				return;
			}
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			string text = "ResearchTechprintsFromFactions".Translate();
			float num = Text.CalcHeight(text, rect.width);
			Widgets.Label(new Rect(rect.x, yMin, rect.width, num), text);
			rect.x += 6f;
			if (this.selectedProject.heldByFactionCategoryTags != null)
			{
				foreach (string b in this.selectedProject.heldByFactionCategoryTags)
				{
					foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
					{
						if (faction.def.categoryTag == b)
						{
							string name = faction.Name;
							Rect rect2 = new Rect(rect.x, yMin + num, rect.width, Mathf.Max(24f, Text.CalcHeight(name, rect.width - 24f - 6f)));
							Widgets.BeginGroup(rect2);
							Rect r = new Rect(0f, 0f, 24f, 24f).ContractedBy(2f);
							FactionUIUtility.DrawFactionIconWithTooltip(r, faction);
							Rect rect3 = new Rect(r.xMax + 6f, 0f, rect2.width - r.width - 6f, rect2.height);
							Text.Anchor = TextAnchor.MiddleLeft;
							Text.WordWrap = false;
							Widgets.Label(rect3, faction.Name);
							Text.Anchor = TextAnchor.UpperLeft;
							Text.WordWrap = true;
							Widgets.EndGroup();
							num += rect2.height;
						}
					}
				}
			}
			rect.xMin = xMin;
			y += num;
		}

		// Token: 0x06012C45 RID: 76869 RVA: 0x0015CA46 File Offset: 0x0015AC46
		private string GetLabel(ResearchProjectDef r)
		{
			return r.LabelCap;
		}

		// Token: 0x06012C46 RID: 76870 RVA: 0x00592E18 File Offset: 0x00591018
		private void SetPrerequisiteStatusColor(bool present, ResearchProjectDef project)
		{
			if (project.IsFinished)
			{
				return;
			}
			GUI.color = (present ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor);
		}

		// Token: 0x06012C47 RID: 76871 RVA: 0x00592E38 File Offset: 0x00591038
		private void DrawResearchBenchFacilityRequirement(ThingDef requiredFacility, CompAffectedByFacilities bestMatchingBench, ResearchProjectDef project, ref Rect rect)
		{
			Thing thing = null;
			Thing thing2 = null;
			if (bestMatchingBench != null)
			{
				thing = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility);
				thing2 = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility && bestMatchingBench.IsFacilityActive(x));
			}
			this.SetPrerequisiteStatusColor(thing2 != null, project);
			string text = requiredFacility.LabelCap;
			if (thing != null && thing2 == null)
			{
				text += " (" + "InactiveFacility".Translate() + ")";
			}
			rect.height = Text.CalcHeight(text, rect.width - 24f - 6f);
			Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(requiredFacility, -1), text, 2f, 6f, null, false, null);
		}

		// Token: 0x06012C48 RID: 76872 RVA: 0x00592F3C File Offset: 0x0059113C
		private Building_ResearchBench FindBenchFulfillingMostRequirements(ThingDef requiredResearchBench, List<ThingDef> requiredFacilities)
		{
			MainTabWindow_Research.tmpAllBuildings.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				MainTabWindow_Research.tmpAllBuildings.AddRange(maps[i].listerBuildings.allBuildingsColonist);
			}
			float num = 0f;
			Building_ResearchBench building_ResearchBench = null;
			for (int j = 0; j < MainTabWindow_Research.tmpAllBuildings.Count; j++)
			{
				Building_ResearchBench building_ResearchBench2 = MainTabWindow_Research.tmpAllBuildings[j] as Building_ResearchBench;
				if (building_ResearchBench2 != null && (requiredResearchBench == null || building_ResearchBench2.def == requiredResearchBench))
				{
					float researchBenchRequirementsScore = this.GetResearchBenchRequirementsScore(building_ResearchBench2, requiredFacilities);
					if (building_ResearchBench == null || researchBenchRequirementsScore > num)
					{
						num = researchBenchRequirementsScore;
						building_ResearchBench = building_ResearchBench2;
					}
				}
			}
			MainTabWindow_Research.tmpAllBuildings.Clear();
			return building_ResearchBench;
		}

		// Token: 0x06012C49 RID: 76873 RVA: 0x00592FF0 File Offset: 0x005911F0
		private float GetResearchBenchRequirementsScore(Building_ResearchBench bench, List<ThingDef> requiredFacilities)
		{
			float num = 0f;
			int i;
			Predicate<Thing> <>9__1;
			int j;
			for (i = 0; i < requiredFacilities.Count; i = j + 1)
			{
				CompAffectedByFacilities benchComp = bench.GetComp<CompAffectedByFacilities>();
				if (benchComp != null)
				{
					List<Thing> linkedFacilitiesListForReading = benchComp.LinkedFacilitiesListForReading;
					if (linkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacilities[i] && benchComp.IsFacilityActive(x)) != null)
					{
						num += 1f;
					}
					else
					{
						List<Thing> list = linkedFacilitiesListForReading;
						Predicate<Thing> match;
						if ((match = <>9__1) == null)
						{
							match = (<>9__1 = ((Thing x) => x.def == requiredFacilities[i]));
						}
						if (list.Find(match) != null)
						{
							num += 0.6f;
						}
					}
				}
				j = i;
			}
			return num;
		}

		// Token: 0x06012C4A RID: 76874 RVA: 0x005930D0 File Offset: 0x005912D0
		private void UpdateSearchResults()
		{
			this.quickSearchWidget.noResultsMatched = false;
			this.matchingProjects.Clear();
			foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord in this.tabs)
			{
				researchTabRecord.Reset();
			}
			if (!this.quickSearchWidget.filter.Active)
			{
				return;
			}
			foreach (ResearchProjectDef researchProjectDef in this.VisibleResearchProjects)
			{
				if (!researchProjectDef.IsHidden && (this.quickSearchWidget.filter.Matches(this.GetLabel(researchProjectDef)) || this.<UpdateSearchResults>g__MatchesUnlockedDefs|109_0(researchProjectDef)))
				{
					this.matchingProjects.Add(researchProjectDef);
				}
			}
			this.quickSearchWidget.noResultsMatched = !this.matchingProjects.Any<ResearchProjectDef>();
			using (List<MainTabWindow_Research.ResearchTabRecord>.Enumerator enumerator = this.tabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MainTabWindow_Research.ResearchTabRecord tab = enumerator.Current;
					tab.firstMatch = (from p in this.matchingProjects
					where tab.def == p.tab
					orderby p.ResearchViewX
					select p).FirstOrDefault<ResearchProjectDef>();
					if (!tab.AnyMatches)
					{
						tab.labelColor = new Color?(Color.grey);
					}
				}
			}
			if (!this.CurTabRecord.AnyMatches)
			{
				foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord2 in this.tabs)
				{
					if (researchTabRecord2.AnyMatches)
					{
						this.CurTab = researchTabRecord2.def;
						break;
					}
				}
			}
			this.scrollPositioner.Arm(true);
			if (this.CurTabRecord.firstMatch != null)
			{
				this.selectedProject = this.CurTabRecord.firstMatch;
			}
		}

		// Token: 0x06012C4B RID: 76875 RVA: 0x0059331C File Offset: 0x0059151C
		private bool MatchesUnlockedDef(Def unlocked)
		{
			return this.quickSearchWidget.filter.Matches(unlocked.label);
		}

		// Token: 0x06012C4C RID: 76876 RVA: 0x00593334 File Offset: 0x00591534
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			this.quickSearchWidget.Unfocus();
		}

		// Token: 0x06012C4F RID: 76879 RVA: 0x005934A8 File Offset: 0x005916A8
		[CompilerGenerated]
		private bool <UpdateSearchResults>g__MatchesUnlockedDefs|109_0(ResearchProjectDef proj)
		{
			foreach (Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>> pair in this.UnlockedDefsGroupedByPrerequisites(proj))
			{
				foreach (Def unlocked in pair.Second)
				{
					if (this.MatchesUnlockedDef(unlocked))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0400C2E1 RID: 49889
		protected ResearchProjectDef selectedProject;

		// Token: 0x0400C2E2 RID: 49890
		private ScrollPositioner scrollPositioner = new ScrollPositioner();

		// Token: 0x0400C2E3 RID: 49891
		private Vector2 leftScrollPosition = Vector2.zero;

		// Token: 0x0400C2E4 RID: 49892
		private float leftScrollViewHeight;

		// Token: 0x0400C2E5 RID: 49893
		private Vector2 rightScrollPosition;

		// Token: 0x0400C2E6 RID: 49894
		private float rightViewWidth;

		// Token: 0x0400C2E7 RID: 49895
		private ResearchTabDef curTabInt;

		// Token: 0x0400C2E8 RID: 49896
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400C2E9 RID: 49897
		private bool editMode;

		// Token: 0x0400C2EA RID: 49898
		private List<ResearchProjectDef> draggingTabs = new List<ResearchProjectDef>();

		// Token: 0x0400C2EB RID: 49899
		private List<MainTabWindow_Research.ResearchTabRecord> tabs = new List<MainTabWindow_Research.ResearchTabRecord>();

		// Token: 0x0400C2EC RID: 49900
		private List<ResearchProjectDef> cachedVisibleResearchProjects;

		// Token: 0x0400C2ED RID: 49901
		private Dictionary<ResearchProjectDef, List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>> cachedUnlockedDefsGroupedByPrerequisites;

		// Token: 0x0400C2EE RID: 49902
		private readonly HashSet<ResearchProjectDef> matchingProjects = new HashSet<ResearchProjectDef>();

		// Token: 0x0400C2EF RID: 49903
		private const float leftAreaWidthPercent = 0.22f;

		// Token: 0x0400C2F0 RID: 49904
		private const float LeftAreaWidthMin = 270f;

		// Token: 0x0400C2F1 RID: 49905
		private const float ProjectTitleHeight = 50f;

		// Token: 0x0400C2F2 RID: 49906
		private const float ProjectTitleLeftMargin = 0f;

		// Token: 0x0400C2F3 RID: 49907
		private const int ResearchItemW = 140;

		// Token: 0x0400C2F4 RID: 49908
		private const int ResearchItemH = 50;

		// Token: 0x0400C2F5 RID: 49909
		private const int ResearchItemPaddingW = 50;

		// Token: 0x0400C2F6 RID: 49910
		private const int ResearchItemPaddingH = 50;

		// Token: 0x0400C2F7 RID: 49911
		private const int CategoryRectW = 14;

		// Token: 0x0400C2F8 RID: 49912
		private const float IndentSpacing = 6f;

		// Token: 0x0400C2F9 RID: 49913
		private const float RowHeight = 24f;

		// Token: 0x0400C2FA RID: 49914
		private const float LeftStartButHeight = 55f;

		// Token: 0x0400C2FB RID: 49915
		private const float SearchBoxHeight = 24f;

		// Token: 0x0400C2FC RID: 49916
		private const float SearchBoxWidth = 200f;

		// Token: 0x0400C2FD RID: 49917
		private const float MinTabWidth = 100f;

		// Token: 0x0400C2FE RID: 49918
		private const float MaxTabWidth = 200f;

		// Token: 0x0400C2FF RID: 49919
		private const int SearchHighlightMargin = 4;

		// Token: 0x0400C300 RID: 49920
		private const KeyCode SelectMultipleKey = KeyCode.LeftShift;

		// Token: 0x0400C301 RID: 49921
		private const KeyCode DeselectKey = KeyCode.LeftControl;

		// Token: 0x0400C302 RID: 49922
		private static readonly Texture2D ResearchBarFillTex = SolidColorMaterials.NewSolidColorTexture(TexUI.ResearchMainTabColor);

		// Token: 0x0400C303 RID: 49923
		private static readonly Texture2D ResearchBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

		// Token: 0x0400C304 RID: 49924
		private static readonly CachedTexture BasicBackgroundTex = new CachedTexture("UI/AnomalyResearchCategoryMarkers/AnomalyResearchBackground_Basic");

		// Token: 0x0400C305 RID: 49925
		private static readonly CachedTexture AdvancedBackgroundTex = new CachedTexture("UI/AnomalyResearchCategoryMarkers/AnomalyResearchBackground_Advanced");

		// Token: 0x0400C306 RID: 49926
		private static readonly Color FulfilledPrerequisiteColor = Color.green;

		// Token: 0x0400C307 RID: 49927
		private static readonly Color MissingPrerequisiteColor = ColorLibrary.RedReadable;

		// Token: 0x0400C308 RID: 49928
		private static readonly Color ProjectWithMissingPrerequisiteLabelColor = Color.gray;

		// Token: 0x0400C309 RID: 49929
		private static readonly Color HiddenProjectLabelColor = Color.gray;

		// Token: 0x0400C30A RID: 49930
		private static readonly Color ActiveProjectLabelColor = new ColorInt(219, 201, 126, 255).ToColor;

		// Token: 0x0400C30B RID: 49931
		private static readonly Color NoMatchTintColor = Widgets.MenuSectionBGFillColor;

		// Token: 0x0400C30C RID: 49932
		private const float NoMatchTintFactor = 0.4f;

		// Token: 0x0400C30D RID: 49933
		private static readonly CachedTexture TechprintRequirementTex = new CachedTexture("UI/Icons/Research/Techprint");

		// Token: 0x0400C30E RID: 49934
		private static readonly CachedTexture StudyRequirementTex = new CachedTexture("UI/Icons/Study");

		// Token: 0x0400C30F RID: 49935
		private static List<string> lockedReasons = new List<string>();

		// Token: 0x0400C310 RID: 49936
		private List<ValueTuple<BuildableDef, List<string>>> cachedDefsWithMissingMemes = new List<ValueTuple<BuildableDef, List<string>>>();

		// Token: 0x0400C311 RID: 49937
		private static Dictionary<string, string> labelsWithNewlineCached = new Dictionary<string, string>();

		// Token: 0x0400C312 RID: 49938
		private static Dictionary<Pair<int, int>, string> techprintsInfoCached = new Dictionary<Pair<int, int>, string>();

		// Token: 0x0400C313 RID: 49939
		private List<string> tmpSuffixesForUnlocked = new List<string>();

		// Token: 0x0400C314 RID: 49940
		private static List<Building> tmpAllBuildings = new List<Building>();

		// Token: 0x02003414 RID: 13332
		private class ResearchTabRecord : TabRecord
		{
			// Token: 0x17003129 RID: 12585
			// (get) Token: 0x06012C50 RID: 76880 RVA: 0x00593544 File Offset: 0x00591744
			public bool AnyMatches
			{
				get
				{
					return this.firstMatch != null;
				}
			}

			// Token: 0x1700312A RID: 12586
			// (get) Token: 0x06012C51 RID: 76881 RVA: 0x0059354F File Offset: 0x0059174F
			public override string TutorTag
			{
				get
				{
					return this.def.tutorTag;
				}
			}

			// Token: 0x06012C52 RID: 76882 RVA: 0x0059355C File Offset: 0x0059175C
			public ResearchTabRecord(ResearchTabDef def, string label, Action clickedAction, Func<bool> selected) : base(label, clickedAction, selected)
			{
				this.def = def;
			}

			// Token: 0x06012C53 RID: 76883 RVA: 0x0059356F File Offset: 0x0059176F
			public void Reset()
			{
				this.firstMatch = null;
				this.labelColor = null;
			}

			// Token: 0x06012C54 RID: 76884 RVA: 0x00593584 File Offset: 0x00591784
			public override string GetTip()
			{
				if (this.cachedTip == null)
				{
					this.cachedTip = (this.def.generalTitle.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor) ?? "");
					if (!this.def.generalDescription.NullOrEmpty())
					{
						this.cachedTip = this.cachedTip + "\n" + this.def.generalDescription;
					}
				}
				return this.cachedTip;
			}

			// Token: 0x0400C315 RID: 49941
			public readonly ResearchTabDef def;

			// Token: 0x0400C316 RID: 49942
			public ResearchProjectDef firstMatch;

			// Token: 0x0400C317 RID: 49943
			private string cachedTip;
		}
	}
}
