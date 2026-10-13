using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003057 RID: 12375
	public class Alert_NeedAnomalyProject : Alert
	{
		// Token: 0x0601161B RID: 71195 RVA: 0x004FC740 File Offset: 0x004FA940
		public Alert_NeedAnomalyProject()
		{
			this.defaultLabel = "NeedAnomalyProject".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0601161C RID: 71196 RVA: 0x004FC790 File Offset: 0x004FA990
		public override AlertReport GetReport()
		{
			if (Find.CurrentMap == null)
			{
				return false;
			}
			if (!Find.Anomaly.AnomalyStudyEnabled)
			{
				return false;
			}
			if (!Find.ResearchManager.AnyProjectIsAvailable)
			{
				return false;
			}
			this.GetCurrentCategories();
			this.GetMissingCategories();
			this.missingProjectCategories.RemoveAll((KnowledgeCategoryDef c) => !Find.ResearchManager.AnyProjectsAvailableWithKnowledgeCategory(c));
			return this.missingProjectCategories.Count > 0;
		}

		// Token: 0x0601161D RID: 71197 RVA: 0x004FC81C File Offset: 0x004FAA1C
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (KnowledgeCategoryDef knowledgeCategoryDef in this.missingProjectCategories)
			{
				this.sb.AppendLine(string.Format("  - {0}", "NeedAnomalyProjectDescLine".Translate(knowledgeCategoryDef.label.Named("NAME"))));
			}
			return string.Format("{0}\n{1}\n\n{2}", "NeedAnomalyProjectDesc".Translate(), this.sb.ToString().TrimEndNewlines(), "NeedAnomalyProjectDescAppended".Translate());
		}

		// Token: 0x0601161E RID: 71198 RVA: 0x004FC8E8 File Offset: 0x004FAAE8
		private void GetMissingCategories()
		{
			this.missingProjectCategories.Clear();
			foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in Find.ResearchManager.CurrentAnomalyKnowledgeProjects)
			{
				if (knowledgeCategoryProject.project == null && this.currentCategories.Contains(knowledgeCategoryProject.category) && knowledgeCategoryProject.category.overflowCategory == null)
				{
					this.missingProjectCategories.Add(knowledgeCategoryProject.category);
				}
			}
		}

		// Token: 0x0601161F RID: 71199 RVA: 0x004FC97C File Offset: 0x004FAB7C
		private void GetCurrentCategories()
		{
			this.currentCategories.Clear();
			foreach (Thing thing in Find.StudyManager.GetStudiableThingsAndPlatforms(Find.CurrentMap))
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					thing = building_HoldingPlatform.HeldPawn;
					CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.CanStudy)
					{
						continue;
					}
				}
				CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
				if (compStudiable != null && compStudiable.studyEnabled && compStudiable.KnowledgeCategory != null)
				{
					this.currentCategories.Add(compStudiable.KnowledgeCategory);
				}
			}
		}

		// Token: 0x06011620 RID: 71200 RVA: 0x0045EDCE File Offset: 0x0045CFCE
		protected override void OnClick()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
			((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).CurTab = ResearchTabDefOf.Anomaly;
		}

		// Token: 0x0400B3EE RID: 46062
		private HashSet<KnowledgeCategoryDef> currentCategories = new HashSet<KnowledgeCategoryDef>();

		// Token: 0x0400B3EF RID: 46063
		private List<KnowledgeCategoryDef> missingProjectCategories = new List<KnowledgeCategoryDef>();

		// Token: 0x0400B3F0 RID: 46064
		private StringBuilder sb = new StringBuilder();
	}
}
