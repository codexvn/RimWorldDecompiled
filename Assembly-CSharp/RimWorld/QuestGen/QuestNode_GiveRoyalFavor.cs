using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ACC RID: 15052
	public class QuestNode_GiveRoyalFavor : QuestNode
	{
		// Token: 0x0601485A RID: 84058 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601485B RID: 84059 RVA: 0x0061FB50 File Offset: 0x0061DD50
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = new QuestPart_GiveRoyalFavor();
			questPart_GiveRoyalFavor.giveTo = this.giveTo.GetValue(slate);
			questPart_GiveRoyalFavor.giveToAccepter = this.giveToAccepter.GetValue(slate);
			questPart_GiveRoyalFavor.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_GiveRoyalFavor.amount = this.amount.GetValue(slate);
			questPart_GiveRoyalFavor.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_GiveRoyalFavor);
			if (this.isSingleReward.GetValue(slate))
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = questPart_GiveRoyalFavor.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(questPart_GiveRoyalFavor);
				choice.rewards.Add(new Reward_RoyalFavor
				{
					faction = questPart_GiveRoyalFavor.faction,
					amount = questPart_GiveRoyalFavor.amount
				});
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
				this.PostProcessRewardChoice(questPart_Choice);
			}
		}

		// Token: 0x0601485C RID: 84060 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void PostProcessRewardChoice(QuestPart_Choice rewardChoice)
		{
		}

		// Token: 0x0400E383 RID: 58243
		public SlateRef<Pawn> giveTo;

		// Token: 0x0400E384 RID: 58244
		public SlateRef<bool> giveToAccepter;

		// Token: 0x0400E385 RID: 58245
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E386 RID: 58246
		public SlateRef<Faction> faction;

		// Token: 0x0400E387 RID: 58247
		public SlateRef<Thing> factionOf;

		// Token: 0x0400E388 RID: 58248
		public SlateRef<int> amount;

		// Token: 0x0400E389 RID: 58249
		public SlateRef<bool> isSingleReward;
	}
}
