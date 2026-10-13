using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039FB RID: 14843
	public class QuestNode_CampLootReward : QuestNode
	{
		// Token: 0x06014595 RID: 83349 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014596 RID: 83350 RVA: 0x006155A0 File Offset: 0x006137A0
		protected override void RunInt()
		{
			QuestPart_Choice questPart_Choice = QuestGen.quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_CampLoot());
			questPart_Choice.choices.Add(choice);
		}
	}
}
