using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AAB RID: 15019
	public class QuestNode_AddItemsReward : QuestNode
	{
		// Token: 0x060147EE RID: 83950 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060147EF RID: 83951 RVA: 0x0061E4FC File Offset: 0x0061C6FC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			RewardsGeneratorParams value = this.parms.GetValue(QuestGen.slate);
			IEnumerable<Thing> value2 = this.items.GetValue(slate);
			if (value2.EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			Reward_Items reward_Items = new Reward_Items();
			reward_Items.items.AddRange(value2);
			choice.rewards.Add(reward_Items);
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
			if (this.generateQuestParts)
			{
				foreach (QuestPart part in reward_Items.GenerateQuestParts(0, value, null, null, null, null))
				{
					QuestGen.quest.AddPart(part);
				}
			}
		}

		// Token: 0x0400E2F8 RID: 58104
		public SlateRef<IEnumerable<Thing>> items;

		// Token: 0x0400E2F9 RID: 58105
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;

		// Token: 0x0400E2FA RID: 58106
		public SlateRef<RewardsGeneratorParams> parms;

		// Token: 0x0400E2FB RID: 58107
		public bool generateQuestParts = true;
	}
}
