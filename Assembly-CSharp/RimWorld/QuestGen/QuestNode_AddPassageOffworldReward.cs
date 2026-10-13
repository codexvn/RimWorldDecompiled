using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AAD RID: 15021
	public class QuestNode_AddPassageOffworldReward : QuestNode
	{
		// Token: 0x060147F4 RID: 83956 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060147F5 RID: 83957 RVA: 0x0061E6F8 File Offset: 0x0061C8F8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_PassageOffworld());
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x0400E301 RID: 58113
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
