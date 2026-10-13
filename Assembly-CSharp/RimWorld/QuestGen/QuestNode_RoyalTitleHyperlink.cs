using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003C07 RID: 15367
	public class QuestNode_RoyalTitleHyperlink : QuestNode
	{
		// Token: 0x06014D78 RID: 85368 RVA: 0x0063E500 File Offset: 0x0063C700
		protected override void RunInt()
		{
			QuestPart_RoyalTitleHyperlink questPart_RoyalTitleHyperlink = new QuestPart_RoyalTitleHyperlink();
			questPart_RoyalTitleHyperlink.titleDef = this.title.GetValue(QuestGen.slate);
			questPart_RoyalTitleHyperlink.factionDef = this.faction.GetValue(QuestGen.slate);
			QuestGen.quest.AddPart(questPart_RoyalTitleHyperlink);
		}

		// Token: 0x06014D79 RID: 85369 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E861 RID: 59489
		public SlateRef<RoyalTitleDef> title;

		// Token: 0x0400E862 RID: 59490
		public SlateRef<FactionDef> faction;
	}
}
