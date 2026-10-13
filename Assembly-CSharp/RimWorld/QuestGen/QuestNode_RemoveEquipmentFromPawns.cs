using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AE6 RID: 15078
	public class QuestNode_RemoveEquipmentFromPawns : QuestNode
	{
		// Token: 0x060148AA RID: 84138 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148AB RID: 84139 RVA: 0x006210C4 File Offset: 0x0061F2C4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_RemoveEquipmentFromPawns questPart_RemoveEquipmentFromPawns = new QuestPart_RemoveEquipmentFromPawns();
			questPart_RemoveEquipmentFromPawns.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_RemoveEquipmentFromPawns.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_RemoveEquipmentFromPawns);
		}

		// Token: 0x0400E3F4 RID: 58356
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E3F5 RID: 58357
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
