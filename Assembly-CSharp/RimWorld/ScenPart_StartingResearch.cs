using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020028EC RID: 10476
	public class ScenPart_StartingResearch : ScenPart
	{
		// Token: 0x0600E519 RID: 58649 RVA: 0x0042E300 File Offset: 0x0042C500
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.project.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<ResearchProjectDef>(this.NonRedundantResearchProjects(), (ResearchProjectDef d) => d.LabelCap, (ResearchProjectDef d) => delegate()
				{
					this.project = d;
				});
			}
		}

		// Token: 0x0600E51A RID: 58650 RVA: 0x0042E372 File Offset: 0x0042C572
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x0600E51B RID: 58651 RVA: 0x0042E385 File Offset: 0x0042C585
		private IEnumerable<ResearchProjectDef> NonRedundantResearchProjects()
		{
			return DefDatabase<ResearchProjectDef>.AllDefs.Where(delegate(ResearchProjectDef d)
			{
				if (d.tags == null || Find.Scenario.playerFaction.factionDef.startingResearchTags == null)
				{
					return true;
				}
				return !d.tags.Any((ResearchProjectTagDef tag) => Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag));
			});
		}

		// Token: 0x0600E51C RID: 58652 RVA: 0x0042E3B0 File Offset: 0x0042C5B0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x0600E51D RID: 58653 RVA: 0x0042E3C8 File Offset: 0x0042C5C8
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(this.project.LabelCap);
		}

		// Token: 0x0600E51E RID: 58654 RVA: 0x0042E3E9 File Offset: 0x0042C5E9
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.project == null;
		}

		// Token: 0x0600E51F RID: 58655 RVA: 0x0042E3FE File Offset: 0x0042C5FE
		public override void PostGameStart()
		{
			if (this.project != null)
			{
				Find.ResearchManager.FinishProject(this.project, false, null, false);
			}
		}

		// Token: 0x0600E520 RID: 58656 RVA: 0x0042E41B File Offset: 0x0042C61B
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.project != null) ? this.project.GetHashCode() : 0);
		}

		// Token: 0x04009952 RID: 39250
		private ResearchProjectDef project;
	}
}
