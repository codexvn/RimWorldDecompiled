using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AE5 RID: 15077
	public class QuestNode_ReleaseParalyzedAnimals : QuestNode
	{
		// Token: 0x060148A7 RID: 84135 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148A8 RID: 84136 RVA: 0x0062107C File Offset: 0x0061F27C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_ReleaseParalyzedAnimals questPart_ReleaseParalyzedAnimals = new QuestPart_ReleaseParalyzedAnimals();
			questPart_ReleaseParalyzedAnimals.pawns.AddRange(value);
			QuestGen.quest.AddPart(questPart_ReleaseParalyzedAnimals);
		}

		// Token: 0x0400E3F3 RID: 58355
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
