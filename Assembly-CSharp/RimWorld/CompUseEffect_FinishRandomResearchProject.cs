using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F5C RID: 12124
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		// Token: 0x06011170 RID: 70000 RVA: 0x004E84F0 File Offset: 0x004E66F0
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			ResearchProjectDef project = Find.ResearchManager.GetProject(null);
			if (project != null)
			{
				this.FinishInstantly(project, usedBy);
			}
		}

		// Token: 0x06011171 RID: 70001 RVA: 0x004E851B File Offset: 0x004E671B
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (Find.ResearchManager.GetProject(null) == null)
			{
				return "NoActiveResearchProjectToFinish".Translate();
			}
			return true;
		}

		// Token: 0x06011172 RID: 70002 RVA: 0x004E8540 File Offset: 0x004E6740
		private void FinishInstantly(ResearchProjectDef proj, Pawn usedBy)
		{
			Find.ResearchManager.FinishProject(proj, true, null, true);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(proj.LabelCap), usedBy, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
