using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AFC RID: 15100
	public class QuestNode_SituationalThought : QuestNode
	{
		// Token: 0x060148F4 RID: 84212 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148F5 RID: 84213 RVA: 0x00621DA4 File Offset: 0x0061FFA4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SituationalThought questPart_SituationalThought = new QuestPart_SituationalThought();
			questPart_SituationalThought.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SituationalThought.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_SituationalThought.def = this.def.GetValue(slate);
			questPart_SituationalThought.pawn = this.pawn.GetValue(slate);
			questPart_SituationalThought.delayTicks = this.delayTicks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SituationalThought);
		}

		// Token: 0x0400E436 RID: 58422
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E437 RID: 58423
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E438 RID: 58424
		public SlateRef<ThoughtDef> def;

		// Token: 0x0400E439 RID: 58425
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E43A RID: 58426
		public SlateRef<int> delayTicks;
	}
}
