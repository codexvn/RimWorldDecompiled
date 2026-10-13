using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020033EF RID: 13295
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x170030F9 RID: 12537
		// (get) Token: 0x06012B28 RID: 76584 RVA: 0x00589210 File Offset: 0x00587410
		public override float ButtonBarPercent
		{
			get
			{
				ResearchProjectDef project = Find.ResearchManager.GetProject(null);
				if (project == null)
				{
					return 0f;
				}
				return project.ProgressPercent;
			}
		}
	}
}
