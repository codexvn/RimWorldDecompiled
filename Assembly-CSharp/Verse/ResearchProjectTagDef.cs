using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D5 RID: 725
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x060011F5 RID: 4597 RVA: 0x00054074 File Offset: 0x00052274
		public int CompletedProjects()
		{
			int num = 0;
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ResearchProjectDef researchProjectDef = allDefsListForReading[i];
				if (researchProjectDef.IsFinished && researchProjectDef.HasTag(this))
				{
					num++;
				}
			}
			return num;
		}
	}
}
