using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD5 RID: 15061
	public class QuestNode_LendColonistsToFaction : QuestNode
	{
		// Token: 0x06014876 RID: 84086 RVA: 0x006201D8 File Offset: 0x0061E3D8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false);
			QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = new QuestPart_LendColonistsToFaction
			{
				inSignalEnable = inSignal,
				shuttle = this.shuttle.GetValue(slate),
				lendColonistsToFaction = this.lendColonistsToFactionOf.GetValue(slate).Faction,
				returnLentColonistsInTicks = this.returnLentColonistsInTicks.GetValue(slate),
				returnMap = slate.Get<Map>("map", null, false).Parent
			};
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_LendColonistsToFaction.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			if (!this.outSignalColonistsDied.GetValue(slate).NullOrEmpty())
			{
				questPart_LendColonistsToFaction.outSignalColonistsDied = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalColonistsDied.GetValue(slate));
			}
			QuestGen.quest.AddPart(questPart_LendColonistsToFaction);
			QuestGen.quest.TendPawnsWithMedicine(ThingDefOf.MedicineIndustrial, true, null, this.shuttle.GetValue(slate), inSignal);
		}

		// Token: 0x06014877 RID: 84087 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E3A4 RID: 58276
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E3A5 RID: 58277
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400E3A6 RID: 58278
		[NoTranslate]
		public SlateRef<string> outSignalColonistsDied;

		// Token: 0x0400E3A7 RID: 58279
		public SlateRef<Thing> shuttle;

		// Token: 0x0400E3A8 RID: 58280
		public SlateRef<Pawn> lendColonistsToFactionOf;

		// Token: 0x0400E3A9 RID: 58281
		public SlateRef<int> returnLentColonistsInTicks;
	}
}
