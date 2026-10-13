using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB0 RID: 15024
	public class QuestNode_BetrayMTB : QuestNode
	{
		// Token: 0x060147FD RID: 83965 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060147FE RID: 83966 RVA: 0x0061E808 File Offset: 0x0061CA08
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_BetrayMTB questPart_BetrayMTB = new QuestPart_BetrayMTB();
			questPart_BetrayMTB.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_BetrayMTB.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_BetrayMTB.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignal.GetValue(slate)));
			questPart_BetrayMTB.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_BetrayMTB);
		}

		// Token: 0x0400E306 RID: 58118
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E307 RID: 58119
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E308 RID: 58120
		[NoTranslate]
		public SlateRef<string> outSignal;

		// Token: 0x0400E309 RID: 58121
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
