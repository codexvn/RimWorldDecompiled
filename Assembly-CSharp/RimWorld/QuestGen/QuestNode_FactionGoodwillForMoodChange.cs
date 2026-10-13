using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AC5 RID: 15045
	public class QuestNode_FactionGoodwillForMoodChange : QuestNode
	{
		// Token: 0x0601483D RID: 84029 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601483E RID: 84030 RVA: 0x0061F530 File Offset: 0x0061D730
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				QuestPart_FactionGoodwillForMoodChange questPart_FactionGoodwillForMoodChange = new QuestPart_FactionGoodwillForMoodChange();
				questPart_FactionGoodwillForMoodChange.pawns.AddRange(this.pawns.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.outSignalSuccess = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalSuccess.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.outSignalFailed = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalFailed.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
				QuestGen.quest.AddPart(questPart_FactionGoodwillForMoodChange);
			}
		}

		// Token: 0x0400E357 RID: 58199
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E358 RID: 58200
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E359 RID: 58201
		[NoTranslate]
		public SlateRef<string> outSignalSuccess;

		// Token: 0x0400E35A RID: 58202
		[NoTranslate]
		public SlateRef<string> outSignalFailed;

		// Token: 0x0400E35B RID: 58203
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
