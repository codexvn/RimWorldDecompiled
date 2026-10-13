using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C95 RID: 7317
	public class QuestPart_RequirementsToAcceptResearch : QuestPart_RequirementsToAccept
	{
		// Token: 0x0600A305 RID: 41733 RVA: 0x002ED560 File Offset: 0x002EB760
		public override AcceptanceReport CanAccept()
		{
			ResearchProjectDef researchProjectDef = this.project;
			if (researchProjectDef != null && !researchProjectDef.IsFinished)
			{
				return new AcceptanceReport("QuestRequiresResearch".Translate());
			}
			return true;
		}

		// Token: 0x0600A306 RID: 41734 RVA: 0x002ED59A File Offset: 0x002EB79A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x040070B4 RID: 28852
		public ResearchProjectDef project;
	}
}
