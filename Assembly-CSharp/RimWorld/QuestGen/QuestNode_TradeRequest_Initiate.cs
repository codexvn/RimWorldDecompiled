using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BDA RID: 15322
	public class QuestNode_TradeRequest_Initiate : QuestNode
	{
		// Token: 0x06014CD1 RID: 85201 RVA: 0x0063B38C File Offset: 0x0063958C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_InitiateTradeRequest part = new QuestPart_InitiateTradeRequest
			{
				settlement = this.settlement.GetValue(slate),
				requestedThingDef = this.requestedThingDef.GetValue(slate),
				requestedCount = this.requestedThingCount.GetValue(slate),
				requestDuration = this.duration.GetValue(slate),
				keepAfterQuestEnds = false,
				inSignal = slate.Get<string>("inSignal", null, false)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x06014CD2 RID: 85202 RVA: 0x0063B414 File Offset: 0x00639614
		protected override bool TestRunInt(Slate slate)
		{
			return this.settlement.GetValue(slate) != null && this.requestedThingCount.GetValue(slate) > 0 && this.requestedThingDef.GetValue(slate) != null && this.duration.GetValue(slate) > 0;
		}

		// Token: 0x0400E787 RID: 59271
		public SlateRef<Settlement> settlement;

		// Token: 0x0400E788 RID: 59272
		public SlateRef<ThingDef> requestedThingDef;

		// Token: 0x0400E789 RID: 59273
		public SlateRef<int> requestedThingCount;

		// Token: 0x0400E78A RID: 59274
		public SlateRef<int> duration;
	}
}
