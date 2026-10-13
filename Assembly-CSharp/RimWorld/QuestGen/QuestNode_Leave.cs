using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD3 RID: 15059
	public class QuestNode_Leave : QuestNode
	{
		// Token: 0x06014870 RID: 84080 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014871 RID: 84081 RVA: 0x00620070 File Offset: 0x0061E270
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_Leave questPart_Leave = new QuestPart_Leave();
			questPart_Leave.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Leave.pawns.AddRange(value);
			questPart_Leave.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? questPart_Leave.sendStandardLetter);
			questPart_Leave.leaveOnCleanup = (this.leaveOnCleanup.GetValue(slate) ?? questPart_Leave.leaveOnCleanup);
			questPart_Leave.inSignalRemovePawn = this.inSignalRemovePawn.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Leave);
		}

		// Token: 0x0400E39C RID: 58268
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E39D RID: 58269
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;

		// Token: 0x0400E39E RID: 58270
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E39F RID: 58271
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x0400E3A0 RID: 58272
		public SlateRef<bool?> leaveOnCleanup;
	}
}
