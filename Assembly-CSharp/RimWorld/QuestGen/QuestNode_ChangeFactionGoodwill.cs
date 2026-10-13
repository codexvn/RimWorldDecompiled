using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB3 RID: 15027
	public class QuestNode_ChangeFactionGoodwill : QuestNode
	{
		// Token: 0x06014806 RID: 83974 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014807 RID: 83975 RVA: 0x0061E928 File Offset: 0x0061CB28
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
			questPart_FactionGoodwillChange.change = this.change.GetValue(slate);
			questPart_FactionGoodwillChange.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_FactionGoodwillChange.canSendHostilityLetter = (this.canSendLetter.GetValue(slate) ?? true);
			questPart_FactionGoodwillChange.canSendMessage = (this.canSendMessage.GetValue(slate) ?? true);
			questPart_FactionGoodwillChange.ensureMakesHostile = this.ensureHostile.GetValue(slate);
			questPart_FactionGoodwillChange.historyEvent = this.reason.GetValue(slate);
			questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
		}

		// Token: 0x0400E30C RID: 58124
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E30D RID: 58125
		public SlateRef<Faction> faction;

		// Token: 0x0400E30E RID: 58126
		public SlateRef<Thing> factionOf;

		// Token: 0x0400E30F RID: 58127
		public SlateRef<int> change;

		// Token: 0x0400E310 RID: 58128
		public SlateRef<bool?> canSendLetter;

		// Token: 0x0400E311 RID: 58129
		public SlateRef<bool?> canSendMessage;

		// Token: 0x0400E312 RID: 58130
		public SlateRef<bool> ensureHostile;

		// Token: 0x0400E313 RID: 58131
		public SlateRef<HistoryEventDef> reason;
	}
}
