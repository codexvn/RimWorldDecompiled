using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AAE RID: 15022
	public class QuestNode_AddPawnReward : QuestNode
	{
		// Token: 0x060147F7 RID: 83959 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060147F8 RID: 83960 RVA: 0x0061E758 File Offset: 0x0061C958
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value == null)
			{
				return;
			}
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_Pawn
			{
				pawn = value,
				detailsHidden = this.rewardDetailsHidden.GetValue(slate)
			});
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x0400E302 RID: 58114
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E303 RID: 58115
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;

		// Token: 0x0400E304 RID: 58116
		public SlateRef<bool> rewardDetailsHidden;
	}
}
