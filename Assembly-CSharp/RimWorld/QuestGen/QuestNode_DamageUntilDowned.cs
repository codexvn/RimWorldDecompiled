using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB9 RID: 15033
	public class QuestNode_DamageUntilDowned : QuestNode
	{
		// Token: 0x06014819 RID: 83993 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601481A RID: 83994 RVA: 0x0061EDA4 File Offset: 0x0061CFA4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_DamageUntilDowned questPart_DamageUntilDowned = new QuestPart_DamageUntilDowned();
			questPart_DamageUntilDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DamageUntilDowned.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_DamageUntilDowned.allowBleedingWounds = (this.allowBleedingWounds.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_DamageUntilDowned);
		}

		// Token: 0x0400E329 RID: 58153
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E32A RID: 58154
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E32B RID: 58155
		public SlateRef<bool?> allowBleedingWounds;
	}
}
