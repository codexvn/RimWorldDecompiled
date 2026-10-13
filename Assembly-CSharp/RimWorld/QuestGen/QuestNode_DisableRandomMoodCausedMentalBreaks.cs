using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ABD RID: 15037
	public class QuestNode_DisableRandomMoodCausedMentalBreaks : QuestNode
	{
		// Token: 0x06014825 RID: 84005 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014826 RID: 84006 RVA: 0x0061EF70 File Offset: 0x0061D170
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_DisableRandomMoodCausedMentalBreaks questPart_DisableRandomMoodCausedMentalBreaks = new QuestPart_DisableRandomMoodCausedMentalBreaks();
			questPart_DisableRandomMoodCausedMentalBreaks.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DisableRandomMoodCausedMentalBreaks.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_DisableRandomMoodCausedMentalBreaks.pawns.AddRange(value);
			QuestGen.quest.AddPart(questPart_DisableRandomMoodCausedMentalBreaks);
		}

		// Token: 0x0400E331 RID: 58161
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E332 RID: 58162
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E333 RID: 58163
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
