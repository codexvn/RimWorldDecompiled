using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AEB RID: 15083
	public class QuestNode_RequirementsToAcceptColonistWithTitle : QuestNode
	{
		// Token: 0x060148BA RID: 84154 RVA: 0x00621330 File Offset: 0x0061F530
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptColonistWithTitle
			{
				minimumTitle = this.minimumTitle.GetValue(slate),
				faction = this.faction.GetValue(slate)
			});
		}

		// Token: 0x060148BB RID: 84155 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E402 RID: 58370
		public SlateRef<RoyalTitleDef> minimumTitle;

		// Token: 0x0400E403 RID: 58371
		public SlateRef<Faction> faction;
	}
}
