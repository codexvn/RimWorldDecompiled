using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AF0 RID: 15088
	public class QuestNode_SetAllApparelLocked : QuestNode
	{
		// Token: 0x060148C9 RID: 84169 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148CA RID: 84170 RVA: 0x0062151C File Offset: 0x0061F71C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SetAllApparelLocked questPart_SetAllApparelLocked = new QuestPart_SetAllApparelLocked();
			questPart_SetAllApparelLocked.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_SetAllApparelLocked.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_SetAllApparelLocked);
		}

		// Token: 0x0400E40D RID: 58381
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E40E RID: 58382
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
