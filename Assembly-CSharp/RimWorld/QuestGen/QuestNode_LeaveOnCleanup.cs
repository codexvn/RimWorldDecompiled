using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD4 RID: 15060
	public class QuestNode_LeaveOnCleanup : QuestNode
	{
		// Token: 0x06014873 RID: 84083 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014874 RID: 84084 RVA: 0x0062014C File Offset: 0x0061E34C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_Leave questPart_Leave = new QuestPart_Leave();
			questPart_Leave.pawns.AddRange(value);
			questPart_Leave.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? questPart_Leave.sendStandardLetter);
			questPart_Leave.leaveOnCleanup = true;
			questPart_Leave.inSignalRemovePawn = this.inSignalRemovePawn.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Leave);
		}

		// Token: 0x0400E3A1 RID: 58273
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E3A2 RID: 58274
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x0400E3A3 RID: 58275
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;
	}
}
