using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D17 RID: 7447
	public sealed class ResearchManager : IExposable
	{
		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x0600A68B RID: 42635 RVA: 0x002FAB1D File Offset: 0x002F8D1D
		public bool AnyProjectIsAvailable
		{
			get
			{
				return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Any((ResearchProjectDef x) => x.baseCost > 0f && x.CanStartNow);
			}
		}

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x0600A68C RID: 42636 RVA: 0x002FAB48 File Offset: 0x002F8D48
		public List<ResearchManager.KnowledgeCategoryProject> CurrentAnomalyKnowledgeProjects
		{
			get
			{
				this.EnsureKnowledgeProjectsInitialized();
				return this.currentAnomalyKnowledgeProjects;
			}
		}

		// Token: 0x0600A68D RID: 42637 RVA: 0x002FAB58 File Offset: 0x002F8D58
		public bool TabInfoVisible(ResearchTabDef tab)
		{
			if (this.tabInfoVisibility == null)
			{
				this.tabInfoVisibility = new DefMap<ResearchTabDef, bool>();
				foreach (ResearchTabDef researchTabDef in DefDatabase<ResearchTabDef>.AllDefs)
				{
					this.tabInfoVisibility[researchTabDef] = researchTabDef.visibleByDefault;
				}
			}
			return this.tabInfoVisibility[tab];
		}

		// Token: 0x0600A68E RID: 42638 RVA: 0x002FABD0 File Offset: 0x002F8DD0
		public void ExposeData()
		{
			Scribe_Defs.Look<ResearchProjectDef>(ref this.currentProj, "currentProj");
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.progress, "progress", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ResearchProjectDef, int>(ref this.techprints, "techprints", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.anomalyKnowledge, "knowledge", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ResearchManager.KnowledgeCategoryProject>(ref this.currentAnomalyKnowledgeProjects, "currentKnowledgeProjects", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<DefMap<ResearchTabDef, bool>>(ref this.tabInfoVisibility, "tabInfoVisibility", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.gravEngineInspected, "gravEngineInspected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.ResearchManagerPostLoadInit();
				if (this.tabInfoVisibility == null)
				{
					this.tabInfoVisibility = new DefMap<ResearchTabDef, bool>();
				}
				foreach (ResearchTabDef researchTabDef in DefDatabase<ResearchTabDef>.AllDefs)
				{
					if (researchTabDef.visibleByDefault)
					{
						this.tabInfoVisibility[researchTabDef] = true;
					}
				}
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				if (this.techprints == null)
				{
					this.techprints = new Dictionary<ResearchProjectDef, int>();
				}
				if (ModsConfig.AnomalyActive)
				{
					if (this.anomalyKnowledge == null)
					{
						this.anomalyKnowledge = new Dictionary<ResearchProjectDef, float>();
					}
					this.EnsureKnowledgeProjectsInitialized();
				}
			}
		}

		// Token: 0x0600A68F RID: 42639 RVA: 0x002FAD08 File Offset: 0x002F8F08
		public void SetCurrentProject(ResearchProjectDef proj)
		{
			if (proj.baseCost > 0f)
			{
				this.currentProj = proj;
			}
			if (ModsConfig.AnomalyActive && proj.knowledgeCategory != null)
			{
				foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in this.CurrentAnomalyKnowledgeProjects)
				{
					if (knowledgeCategoryProject.category == proj.knowledgeCategory)
					{
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnomalyResearch, KnowledgeAmount.Total);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.StudyingEntities, KnowledgeAmount.Total);
						knowledgeCategoryProject.project = proj;
						break;
					}
				}
			}
		}

		// Token: 0x0600A690 RID: 42640 RVA: 0x002FADA4 File Offset: 0x002F8FA4
		public void StopProject(ResearchProjectDef proj)
		{
			if (this.currentProj == proj)
			{
				this.currentProj = null;
				return;
			}
			if (ModsConfig.AnomalyActive && proj.knowledgeCategory != null)
			{
				foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in this.CurrentAnomalyKnowledgeProjects)
				{
					if (knowledgeCategoryProject.category == proj.knowledgeCategory)
					{
						knowledgeCategoryProject.project = null;
						break;
					}
				}
			}
		}

		// Token: 0x0600A691 RID: 42641 RVA: 0x002FAE28 File Offset: 0x002F9028
		public ResearchProjectDef GetProject(KnowledgeCategoryDef category = null)
		{
			if (category == null)
			{
				return this.currentProj;
			}
			foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in this.CurrentAnomalyKnowledgeProjects)
			{
				if (knowledgeCategoryProject.category == category)
				{
					return knowledgeCategoryProject.project;
				}
			}
			return null;
		}

		// Token: 0x0600A692 RID: 42642 RVA: 0x002FAE94 File Offset: 0x002F9094
		public bool IsCurrentProject(ResearchProjectDef proj)
		{
			if (proj == this.currentProj)
			{
				return true;
			}
			if (ModsConfig.AnomalyActive)
			{
				using (List<ResearchManager.KnowledgeCategoryProject>.Enumerator enumerator = this.CurrentAnomalyKnowledgeProjects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.project == proj)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600A693 RID: 42643 RVA: 0x002FAF00 File Offset: 0x002F9100
		public float GetProgress(ResearchProjectDef proj)
		{
			if (proj.baseCost > 0f)
			{
				float result;
				if (this.progress.TryGetValue(proj, out result))
				{
					return result;
				}
				this.progress.Add(proj, 0f);
				return 0f;
			}
			else
			{
				if (!ModsConfig.AnomalyActive || proj.knowledgeCost <= 0f)
				{
					return 0f;
				}
				float result2;
				if (this.anomalyKnowledge.TryGetValue(proj, out result2))
				{
					return result2;
				}
				this.anomalyKnowledge.Add(proj, 0f);
				return 0f;
			}
		}

		// Token: 0x0600A694 RID: 42644 RVA: 0x002FAF88 File Offset: 0x002F9188
		public void AddProgress(ResearchProjectDef proj, float amount, Pawn source = null)
		{
			float num;
			if (this.progress.TryGetValue(proj, out num))
			{
				this.progress[proj] = num + amount;
			}
			else
			{
				this.progress.Add(proj, amount);
			}
			this.progress[proj] = Mathf.Min(this.progress[proj], proj.Cost);
			if (proj.PrerequisitesCompleted && proj.IsFinished)
			{
				this.FinishProject(proj, true, source, true);
			}
		}

		// Token: 0x0600A695 RID: 42645 RVA: 0x002FB000 File Offset: 0x002F9200
		public int GetTechprints(ResearchProjectDef proj)
		{
			int result;
			if (!this.techprints.TryGetValue(proj, out result))
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600A696 RID: 42646 RVA: 0x002FB020 File Offset: 0x002F9220
		public void ApplyTechprint(ResearchProjectDef proj, Pawn applyingPawn)
		{
			if (!ModLister.CheckRoyalty("Techprint"))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("LetterTechprintAppliedPartIntro".Translate(proj.Named("PROJECT")));
			stringBuilder.AppendLine();
			if (proj.TechprintCount > this.GetTechprints(proj))
			{
				this.AddTechprints(proj, 1);
				if (proj.TechprintCount == this.GetTechprints(proj))
				{
					stringBuilder.AppendLine("LetterTechprintAppliedPartJustUnlocked".Translate(proj.Named("PROJECT")));
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.AppendLine("LetterTechprintAppliedPartNotUnlockedYet".Translate(this.GetTechprints(proj), proj.TechprintCount.ToString(), proj.Named("PROJECT")));
					stringBuilder.AppendLine();
				}
			}
			else if (proj.IsFinished)
			{
				stringBuilder.AppendLine("LetterTechprintAppliedPartAlreadyResearched".Translate(proj.Named("PROJECT")));
				stringBuilder.AppendLine();
			}
			else if (!proj.IsFinished && proj.baseCost > 0f)
			{
				float num = (proj.baseCost - this.GetProgress(proj)) * 0.5f;
				stringBuilder.AppendLine("LetterTechprintAppliedPartAlreadyUnlocked".Translate(num, proj.Named("PROJECT")));
				stringBuilder.AppendLine();
				float num2;
				if (!this.progress.TryGetValue(proj, out num2))
				{
					this.progress.Add(proj, Mathf.Min(num, proj.baseCost));
				}
				else
				{
					this.progress[proj] = Mathf.Min(num2 + num, proj.baseCost);
				}
			}
			if (applyingPawn != null)
			{
				stringBuilder.AppendLine("LetterTechprintAppliedPartExpAwarded".Translate(2000.ToString(), SkillDefOf.Intellectual.label, applyingPawn.Named("PAWN")));
				applyingPawn.skills.Learn(SkillDefOf.Intellectual, 2000f, true, true);
			}
			if (stringBuilder.Length > 0)
			{
				Find.LetterStack.ReceiveLetter("LetterTechprintAppliedLabel".Translate(proj.Named("PROJECT")), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.PositiveEvent, null, 0, true);
			}
		}

		// Token: 0x0600A697 RID: 42647 RVA: 0x002FB280 File Offset: 0x002F9480
		public void AddTechprints(ResearchProjectDef proj, int amount)
		{
			int num;
			if (this.techprints.TryGetValue(proj, out num))
			{
				num += amount;
				if (num > proj.TechprintCount)
				{
					num = proj.TechprintCount;
				}
				this.techprints[proj] = num;
				return;
			}
			this.techprints.Add(proj, amount);
		}

		// Token: 0x0600A698 RID: 42648 RVA: 0x002FB2CC File Offset: 0x002F94CC
		public void ResearchPerformed(float amount, Pawn researcher)
		{
			if (this.currentProj == null)
			{
				Log.Error("Researched without having an active project.");
				return;
			}
			amount *= 0.00825f;
			amount *= Find.Storyteller.difficulty.researchSpeedFactor;
			if (((researcher != null) ? researcher.Faction : null) != null)
			{
				amount /= this.currentProj.CostFactor(researcher.Faction.def.techLevel);
			}
			if (DebugSettings.fastResearch)
			{
				amount *= 500f;
			}
			if (researcher != null)
			{
				researcher.records.AddTo(RecordDefOf.ResearchPointsResearched, amount);
			}
			float num = this.GetProgress(this.currentProj);
			num += amount;
			this.progress[this.currentProj] = num;
			if (this.currentProj.IsFinished)
			{
				this.FinishProject(this.currentProj, true, researcher, true);
			}
		}

		// Token: 0x0600A699 RID: 42649 RVA: 0x002FB398 File Offset: 0x002F9598
		public float GetKnowledge(ResearchProjectDef proj)
		{
			float result;
			if (this.anomalyKnowledge.TryGetValue(proj, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x0600A69A RID: 42650 RVA: 0x002FB3BC File Offset: 0x002F95BC
		public void ApplyKnowledge(KnowledgeCategoryDef category, float amount)
		{
			if (!ModLister.CheckAnomaly("Knowledge"))
			{
				return;
			}
			if (amount <= 0f)
			{
				return;
			}
			bool flag = false;
			ResearchProjectDef project = this.GetProject(category);
			if (project != null)
			{
				float num;
				if (this.ApplyKnowledge(project, amount, out num) && num > 0f)
				{
					amount = num;
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag && category.overflowCategory != null)
			{
				this.ApplyKnowledge(category.overflowCategory, amount);
			}
		}

		// Token: 0x0600A69B RID: 42651 RVA: 0x002FB424 File Offset: 0x002F9624
		public bool ApplyKnowledge(ResearchProjectDef project, float amount, out float remainder)
		{
			float num;
			if (this.anomalyKnowledge.TryGetValue(project, out num))
			{
				this.anomalyKnowledge[project] = num + amount;
			}
			else
			{
				this.anomalyKnowledge.Add(project, amount);
			}
			if (project.PrerequisitesCompleted && project.IsFinished)
			{
				remainder = this.anomalyKnowledge[project] - project.knowledgeCost;
				this.anomalyKnowledge[project] = Mathf.Min(this.anomalyKnowledge[project], project.Cost);
				this.FinishProject(project, true, null, false);
				return true;
			}
			remainder = 0f;
			return false;
		}

		// Token: 0x0600A69C RID: 42652 RVA: 0x002FB4BC File Offset: 0x002F96BC
		public void ReapplyAllMods()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (researchProjectDef.IsFinished)
				{
					researchProjectDef.ReapplyAllMods();
				}
			}
		}

		// Token: 0x0600A69D RID: 42653 RVA: 0x002FB510 File Offset: 0x002F9710
		public void FinishProject(ResearchProjectDef proj, bool doCompletionDialog = false, Pawn researcher = null, bool doCompletionLetter = true)
		{
			if (proj.prerequisites != null)
			{
				for (int i = 0; i < proj.prerequisites.Count; i++)
				{
					if (!proj.prerequisites[i].IsFinished)
					{
						this.FinishProject(proj.prerequisites[i], doCompletionDialog, researcher, doCompletionLetter);
					}
				}
			}
			int num = this.GetTechprints(proj);
			if (num < proj.TechprintCount)
			{
				this.AddTechprints(proj, proj.TechprintCount - num);
			}
			if (proj.RequiredAnalyzedThingCount > 0)
			{
				for (int j = 0; j < proj.requiredAnalyzed.Count; j++)
				{
					CompProperties_CompAnalyzableUnlockResearch compProperties = proj.requiredAnalyzed[j].GetCompProperties<CompProperties_CompAnalyzableUnlockResearch>();
					Find.AnalysisManager.ForceCompleteAnalysisProgress(compProperties.analysisID);
				}
			}
			if (proj.baseCost > 0f)
			{
				this.progress[proj] = proj.baseCost;
			}
			else if (ModsConfig.AnomalyActive && proj.knowledgeCost > 0f)
			{
				this.anomalyKnowledge.SetOrAdd(proj, proj.knowledgeCost);
				Find.SignalManager.SendSignal(new Signal("ThingStudied", true));
			}
			if (researcher != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.FinishedResearchProject, new object[]
				{
					researcher,
					proj
				});
			}
			this.ReapplyAllMods();
			if (proj.recalculatePower)
			{
				try
				{
					foreach (Map map in Find.Maps)
					{
						foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.PowerTrader))
						{
							CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
							if (compPowerTrader != null)
							{
								compPowerTrader.SetUpPowerVars();
							}
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
			if (doCompletionDialog)
			{
				DiaNode diaNode = new DiaNode("ResearchFinished".Translate(proj.LabelCap) + "\n\n" + proj.description);
				diaNode.options.Add(DiaOption.DefaultOK);
				DiaOption diaOption = new DiaOption("ResearchScreen".Translate());
				diaOption.resolveTree = true;
				diaOption.action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
					MainTabWindow_Research mainTabWindow_Research = MainButtonDefOf.Research.TabWindow as MainTabWindow_Research;
					if (mainTabWindow_Research != null && proj.tab != null)
					{
						mainTabWindow_Research.CurTab = proj.tab;
					}
				};
				diaNode.options.Add(diaOption);
				Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
			}
			if (doCompletionLetter && !proj.discoveredLetterTitle.NullOrEmpty() && Find.Storyteller.difficulty.AllowedBy(proj.discoveredLetterDisabledWhen))
			{
				Find.LetterStack.ReceiveLetter(proj.discoveredLetterTitle, proj.discoveredLetterText, LetterDefOf.NeutralEvent, null, 0, true);
			}
			if (proj.teachConcept != null)
			{
				LessonAutoActivator.TeachOpportunity(proj.teachConcept, OpportunityType.Important);
			}
			if (this.currentProj == proj)
			{
				this.currentProj = null;
			}
			else if (ModsConfig.AnomalyActive && proj.knowledgeCategory != null)
			{
				foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in this.CurrentAnomalyKnowledgeProjects)
				{
					if (knowledgeCategoryProject.project == proj)
					{
						knowledgeCategoryProject.project = null;
						break;
					}
				}
			}
			foreach (Def def in proj.UnlockedDefs)
			{
				ThingDef thingDef = def as ThingDef;
				if (thingDef != null)
				{
					thingDef.Notify_UnlockedByResearch();
				}
			}
			Find.SignalManager.SendSignal(new Signal("ResearchCompleted", true));
		}

		// Token: 0x0600A69E RID: 42654 RVA: 0x002FB980 File Offset: 0x002F9B80
		public void ResetAllProgress()
		{
			this.progress.Clear();
			this.techprints.Clear();
			this.anomalyKnowledge.Clear();
			this.currentProj = null;
			this.gravEngineInspected = false;
		}

		// Token: 0x0600A69F RID: 42655 RVA: 0x002FB9B4 File Offset: 0x002F9BB4
		private void EnsureKnowledgeProjectsInitialized()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (this.currentAnomalyKnowledgeProjects == null)
			{
				this.currentAnomalyKnowledgeProjects = new List<ResearchManager.KnowledgeCategoryProject>();
			}
			this.currentAnomalyKnowledgeProjects.RemoveAll((ResearchManager.KnowledgeCategoryProject x) => x.category == null);
			using (IEnumerator<KnowledgeCategoryDef> enumerator = DefDatabase<KnowledgeCategoryDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KnowledgeCategoryDef k = enumerator.Current;
					if (!this.currentAnomalyKnowledgeProjects.Any((ResearchManager.KnowledgeCategoryProject x) => x.category == k))
					{
						this.currentAnomalyKnowledgeProjects.Add(new ResearchManager.KnowledgeCategoryProject
						{
							category = k,
							project = null
						});
					}
				}
			}
		}

		// Token: 0x0600A6A0 RID: 42656 RVA: 0x002FBA88 File Offset: 0x002F9C88
		public void Notify_MonolithLevelChanged(int newLevel)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			foreach (ResearchTabDef researchTabDef in DefDatabase<ResearchTabDef>.AllDefs)
			{
				if (!this.TabInfoVisible(researchTabDef) && researchTabDef.minMonolithLevelVisible > 0 && (newLevel >= researchTabDef.minMonolithLevelVisible || (!Find.Anomaly.GenerateMonolith && Find.Storyteller.difficulty.AnomalyPlaystyleDef.enableAnomalyContent)))
				{
					this.tabInfoVisibility[researchTabDef] = true;
				}
			}
		}

		// Token: 0x0600A6A1 RID: 42657 RVA: 0x002FBB20 File Offset: 0x002F9D20
		public void DebugSetAllProjectsFinished()
		{
			this.progress.Clear();
			this.anomalyKnowledge.Clear();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (researchProjectDef.baseCost > 0f)
				{
					this.progress.Add(researchProjectDef, researchProjectDef.baseCost);
				}
				else if (researchProjectDef.knowledgeCost > 0f)
				{
					this.anomalyKnowledge.Add(researchProjectDef, researchProjectDef.knowledgeCost);
				}
			}
			this.ReapplyAllMods();
		}

		// Token: 0x0600A6A2 RID: 42658 RVA: 0x002FBBC4 File Offset: 0x002F9DC4
		public bool AnyProjectsAvailableWithKnowledgeCategory(KnowledgeCategoryDef category)
		{
			return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Any((ResearchProjectDef x) => x.CanStartNow && x.knowledgeCategory == category);
		}

		// Token: 0x04007350 RID: 29520
		private ResearchProjectDef currentProj;

		// Token: 0x04007351 RID: 29521
		private List<ResearchManager.KnowledgeCategoryProject> currentAnomalyKnowledgeProjects;

		// Token: 0x04007352 RID: 29522
		private Dictionary<ResearchProjectDef, float> progress = new Dictionary<ResearchProjectDef, float>();

		// Token: 0x04007353 RID: 29523
		private Dictionary<ResearchProjectDef, int> techprints = new Dictionary<ResearchProjectDef, int>();

		// Token: 0x04007354 RID: 29524
		private Dictionary<ResearchProjectDef, float> anomalyKnowledge = new Dictionary<ResearchProjectDef, float>();

		// Token: 0x04007355 RID: 29525
		private DefMap<ResearchTabDef, bool> tabInfoVisibility;

		// Token: 0x04007356 RID: 29526
		public bool gravEngineInspected;

		// Token: 0x04007357 RID: 29527
		public const float ResearchPointsPerWorkTick = 0.00825f;

		// Token: 0x04007358 RID: 29528
		public const int IntellectualExpPerTechprint = 2000;

		// Token: 0x04007359 RID: 29529
		public const string ResearchCompletedSignal = "ResearchCompleted";

		// Token: 0x02001D18 RID: 7448
		public class KnowledgeCategoryProject : IExposable
		{
			// Token: 0x0600A6A4 RID: 42660 RVA: 0x002FBC1D File Offset: 0x002F9E1D
			public void ExposeData()
			{
				Scribe_Defs.Look<KnowledgeCategoryDef>(ref this.category, "category");
				Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
			}

			// Token: 0x0400735A RID: 29530
			public KnowledgeCategoryDef category;

			// Token: 0x0400735B RID: 29531
			public ResearchProjectDef project;
		}
	}
}
