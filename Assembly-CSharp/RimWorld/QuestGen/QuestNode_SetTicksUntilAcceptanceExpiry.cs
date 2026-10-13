using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AF9 RID: 15097
	public class QuestNode_SetTicksUntilAcceptanceExpiry : QuestNode
	{
		// Token: 0x060148EB RID: 84203 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148EC RID: 84204 RVA: 0x00621B10 File Offset: 0x0061FD10
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.acceptanceExpireTick = GenTicks.TicksGame + this.ticks.GetValue(slate);
		}

		// Token: 0x0400E42A RID: 58410
		public SlateRef<int> ticks;
	}
}
