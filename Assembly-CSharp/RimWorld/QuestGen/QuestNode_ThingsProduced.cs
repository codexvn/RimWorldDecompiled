using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B01 RID: 15105
	public class QuestNode_ThingsProduced : QuestNode
	{
		// Token: 0x06014907 RID: 84231 RVA: 0x00622340 File Offset: 0x00620540
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06014908 RID: 84232 RVA: 0x00622358 File Offset: 0x00620558
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ThingsProduced questPart_ThingsProduced = new QuestPart_ThingsProduced();
			questPart_ThingsProduced.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ThingsProduced.def = this.def.GetValue(slate);
			questPart_ThingsProduced.stuff = this.stuff.GetValue(slate);
			questPart_ThingsProduced.count = this.count.GetValue(slate);
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ThingsProduced);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ThingsProduced.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ThingsProduced);
		}

		// Token: 0x0400E44F RID: 58447
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E450 RID: 58448
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400E451 RID: 58449
		public SlateRef<ThingDef> def;

		// Token: 0x0400E452 RID: 58450
		public SlateRef<ThingDef> stuff;

		// Token: 0x0400E453 RID: 58451
		public SlateRef<int> count;

		// Token: 0x0400E454 RID: 58452
		public QuestNode node;
	}
}
