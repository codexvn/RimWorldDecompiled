using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200301C RID: 12316
	public class Alert_NeedResearchProject : Alert
	{
		// Token: 0x06011535 RID: 70965 RVA: 0x004F7F75 File Offset: 0x004F6175
		public Alert_NeedResearchProject()
		{
			this.defaultLabel = "NeedResearchProject".Translate();
			this.defaultExplanation = "NeedResearchProjectDesc".Translate();
		}

		// Token: 0x06011536 RID: 70966 RVA: 0x004F7FA8 File Offset: 0x004F61A8
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			if (Find.ResearchManager.GetProject(null) != null)
			{
				return false;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].listerBuildings.ColonistsHaveResearchBench())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (!Find.ResearchManager.AnyProjectIsAvailable)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06011537 RID: 70967 RVA: 0x004F8038 File Offset: 0x004F6238
		protected override void OnClick()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
			((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).CurTab = ResearchTabDefOf.Main;
		}
	}
}
