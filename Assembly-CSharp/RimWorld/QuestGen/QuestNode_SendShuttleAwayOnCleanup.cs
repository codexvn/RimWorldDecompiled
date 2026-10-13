using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AEF RID: 15087
	public class QuestNode_SendShuttleAwayOnCleanup : QuestNode
	{
		// Token: 0x060148C6 RID: 84166 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148C7 RID: 84167 RVA: 0x006214C4 File Offset: 0x0061F6C4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAwayOnCleanup questPart_SendShuttleAwayOnCleanup = new QuestPart_SendShuttleAwayOnCleanup();
			questPart_SendShuttleAwayOnCleanup.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAwayOnCleanup.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAwayOnCleanup);
		}

		// Token: 0x0400E40B RID: 58379
		public SlateRef<Thing> shuttle;

		// Token: 0x0400E40C RID: 58380
		public SlateRef<bool> dropEverything;
	}
}
