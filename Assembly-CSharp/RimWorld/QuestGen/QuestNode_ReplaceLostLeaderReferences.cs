using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AE8 RID: 15080
	public class QuestNode_ReplaceLostLeaderReferences : QuestNode
	{
		// Token: 0x060148B0 RID: 84144 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148B1 RID: 84145 RVA: 0x006211FC File Offset: 0x0061F3FC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ReplaceLostLeaderReferences questPart_ReplaceLostLeaderReferences = new QuestPart_ReplaceLostLeaderReferences();
			questPart_ReplaceLostLeaderReferences.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			questPart_ReplaceLostLeaderReferences.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			QuestGen.quest.AddPart(questPart_ReplaceLostLeaderReferences);
		}

		// Token: 0x0400E3FC RID: 58364
		public SlateRef<string> inSignal;

		// Token: 0x0400E3FD RID: 58365
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
