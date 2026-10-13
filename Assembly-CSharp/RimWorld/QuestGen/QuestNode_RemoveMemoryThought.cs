using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AE7 RID: 15079
	public class QuestNode_RemoveMemoryThought : QuestNode
	{
		// Token: 0x060148AD RID: 84141 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148AE RID: 84142 RVA: 0x0062113C File Offset: 0x0061F33C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_RemoveMemoryThought questPart_RemoveMemoryThought = new QuestPart_RemoveMemoryThought();
			questPart_RemoveMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RemoveMemoryThought.def = this.def.GetValue(slate);
			questPart_RemoveMemoryThought.pawn = this.pawn.GetValue(slate);
			questPart_RemoveMemoryThought.count = this.count.GetValue(slate);
			questPart_RemoveMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
			questPart_RemoveMemoryThought.addToLookTargets = (this.addToLookTargets.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_RemoveMemoryThought);
		}

		// Token: 0x0400E3F6 RID: 58358
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E3F7 RID: 58359
		public SlateRef<ThoughtDef> def;

		// Token: 0x0400E3F8 RID: 58360
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E3F9 RID: 58361
		public SlateRef<Pawn> otherPawn;

		// Token: 0x0400E3FA RID: 58362
		public SlateRef<int?> count;

		// Token: 0x0400E3FB RID: 58363
		public SlateRef<bool?> addToLookTargets;
	}
}
