using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB1 RID: 15025
	public class QuestNode_BiocodeWeapons : QuestNode
	{
		// Token: 0x06014800 RID: 83968 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014801 RID: 83969 RVA: 0x0061E8B4 File Offset: 0x0061CAB4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_BiocodeWeapons questPart_BiocodeWeapons = new QuestPart_BiocodeWeapons();
			questPart_BiocodeWeapons.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_BiocodeWeapons.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_BiocodeWeapons);
		}

		// Token: 0x0400E30A RID: 58122
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E30B RID: 58123
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
