using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AED RID: 15085
	public class QuestNode_RequirementsToAcceptResearch : QuestNode
	{
		// Token: 0x060148C0 RID: 84160 RVA: 0x006213E8 File Offset: 0x0061F5E8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptResearch
			{
				project = this.reserach.GetValue(slate)
			});
		}

		// Token: 0x060148C1 RID: 84161 RVA: 0x0062141C File Offset: 0x0061F61C
		protected override bool TestRunInt(Slate slate)
		{
			ResearchProjectDef value = this.reserach.GetValue(slate);
			return value == null || value.IsFinished;
		}

		// Token: 0x0400E407 RID: 58375
		public SlateRef<ResearchProjectDef> reserach;
	}
}
