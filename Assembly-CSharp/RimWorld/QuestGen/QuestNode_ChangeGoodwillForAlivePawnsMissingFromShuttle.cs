using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB4 RID: 15028
	public class QuestNode_ChangeGoodwillForAlivePawnsMissingFromShuttle : QuestNode
	{
		// Token: 0x06014809 RID: 83977 RVA: 0x0061EA21 File Offset: 0x0061CC21
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate) != null;
		}

		// Token: 0x0601480A RID: 83978 RVA: 0x0061EA34 File Offset: 0x0061CC34
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle part = new QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawns = this.pawns.GetValue(slate),
				faction = this.faction.GetValue(slate),
				goodwillChange = this.goodwillChange.GetValue(slate),
				historyEvent = this.reason.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x0400E314 RID: 58132
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E315 RID: 58133
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x0400E316 RID: 58134
		public SlateRef<Faction> faction;

		// Token: 0x0400E317 RID: 58135
		public SlateRef<int> goodwillChange;

		// Token: 0x0400E318 RID: 58136
		public SlateRef<HistoryEventDef> reason;
	}
}
