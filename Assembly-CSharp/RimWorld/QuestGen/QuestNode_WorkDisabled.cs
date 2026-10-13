using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B03 RID: 15107
	public class QuestNode_WorkDisabled : QuestNode
	{
		// Token: 0x0601490D RID: 84237 RVA: 0x00622510 File Offset: 0x00620710
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_WorkDisabled questPart_WorkDisabled = new QuestPart_WorkDisabled();
			questPart_WorkDisabled.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_WorkDisabled.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_WorkDisabled.disabledWorkTags = this.disabledWorkTags.GetValue(slate);
			QuestGen.quest.AddPart(questPart_WorkDisabled);
		}

		// Token: 0x0601490E RID: 84238 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E459 RID: 58457
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E45A RID: 58458
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E45B RID: 58459
		public SlateRef<WorkTags> disabledWorkTags;
	}
}
