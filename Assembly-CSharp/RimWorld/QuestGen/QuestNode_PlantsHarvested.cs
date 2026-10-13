using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AE2 RID: 15074
	public class QuestNode_PlantsHarvested : QuestNode
	{
		// Token: 0x0601489C RID: 84124 RVA: 0x00620E1C File Offset: 0x0061F01C
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0601489D RID: 84125 RVA: 0x00620E34 File Offset: 0x0061F034
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_PlantsHarvested questPart_PlantsHarvested = new QuestPart_PlantsHarvested();
			questPart_PlantsHarvested.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PlantsHarvested.plant = this.plant.GetValue(slate);
			questPart_PlantsHarvested.count = this.count.GetValue(slate);
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PlantsHarvested);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_PlantsHarvested.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_PlantsHarvested);
		}

		// Token: 0x0400E3E9 RID: 58345
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E3EA RID: 58346
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400E3EB RID: 58347
		public SlateRef<ThingDef> plant;

		// Token: 0x0400E3EC RID: 58348
		public SlateRef<int> count;

		// Token: 0x0400E3ED RID: 58349
		public QuestNode node;
	}
}
