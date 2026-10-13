using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AEE RID: 15086
	public class QuestNode_SendShuttleAway : QuestNode
	{
		// Token: 0x060148C3 RID: 84163 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148C4 RID: 84164 RVA: 0x00621440 File Offset: 0x0061F640
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAway questPart_SendShuttleAway = new QuestPart_SendShuttleAway();
			questPart_SendShuttleAway.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SendShuttleAway.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAway.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAway);
		}

		// Token: 0x0400E408 RID: 58376
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E409 RID: 58377
		public SlateRef<Thing> shuttle;

		// Token: 0x0400E40A RID: 58378
		public SlateRef<bool> dropEverything;
	}
}
