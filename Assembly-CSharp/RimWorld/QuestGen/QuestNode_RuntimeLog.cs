using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A0F RID: 14863
	public class QuestNode_RuntimeLog : QuestNode
	{
		// Token: 0x060145DA RID: 83418 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060145DB RID: 83419 RVA: 0x006167BC File Offset: 0x006149BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Log questPart_Log = new QuestPart_Log();
			questPart_Log.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Log.message = this.message.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Log);
		}

		// Token: 0x0400E0ED RID: 57581
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E0EE RID: 57582
		[NoTranslate]
		public SlateRef<string> message;
	}
}
