using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200304C RID: 12364
	public class Alert_NeedResearchBench : Alert
	{
		// Token: 0x17002CF8 RID: 11512
		// (get) Token: 0x060115F0 RID: 71152 RVA: 0x004FBD80 File Offset: 0x004F9F80
		private bool HasRequiredResearchBench
		{
			get
			{
				ResearchProjectDef project = Find.ResearchManager.GetProject(null);
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (project.requiredResearchBuilding != null)
					{
						if (maps[i].listerBuildings.ColonistsHaveBuilding(project.requiredResearchBuilding))
						{
							return true;
						}
					}
					else if (maps[i].listerBuildings.ColonistsHaveResearchBench())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060115F1 RID: 71153 RVA: 0x004FBDE9 File Offset: 0x004F9FE9
		public Alert_NeedResearchBench()
		{
			this.defaultLabel = "NeedResearchBench".Translate();
		}

		// Token: 0x060115F2 RID: 71154 RVA: 0x004FBE06 File Offset: 0x004FA006
		public override AlertReport GetReport()
		{
			if (Find.CurrentGravship != null)
			{
				return false;
			}
			return Find.ResearchManager.GetProject(null) != null && !this.HasRequiredResearchBench;
		}

		// Token: 0x060115F3 RID: 71155 RVA: 0x004FBE34 File Offset: 0x004FA034
		protected override void OnClick()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
		}

		// Token: 0x060115F4 RID: 71156 RVA: 0x004FBE48 File Offset: 0x004FA048
		public override TaggedString GetExplanation()
		{
			ResearchProjectDef project = Find.ResearchManager.GetProject(null);
			return "NeedResearchBenchDesc".Translate(project.label, project.requiredResearchBuilding ?? ThingDefOf.SimpleResearchBench) + ("\n\n(" + "ClickToOpenResearchTab".Translate() + ")");
		}
	}
}
