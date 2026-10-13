using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AAC RID: 15020
	public class QuestNode_AddMemoryThought : QuestNode
	{
		// Token: 0x060147F1 RID: 83953 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060147F2 RID: 83954 RVA: 0x0061E600 File Offset: 0x0061C800
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				foreach (Pawn pawn in this.pawns.GetValue(slate))
				{
					QuestPart_AddMemoryThought questPart_AddMemoryThought = new QuestPart_AddMemoryThought();
					questPart_AddMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
					questPart_AddMemoryThought.def = this.def.GetValue(slate);
					questPart_AddMemoryThought.pawn = pawn;
					questPart_AddMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
					questPart_AddMemoryThought.addToLookTargets = (this.addToLookTargets.GetValue(slate) ?? true);
					QuestGen.quest.AddPart(questPart_AddMemoryThought);
				}
			}
		}

		// Token: 0x0400E2FC RID: 58108
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E2FD RID: 58109
		public SlateRef<ThoughtDef> def;

		// Token: 0x0400E2FE RID: 58110
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E2FF RID: 58111
		public SlateRef<Pawn> otherPawn;

		// Token: 0x0400E300 RID: 58112
		public SlateRef<bool?> addToLookTargets;
	}
}
