using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AA9 RID: 15017
	public class QuestNode_AddContentsToShuttle : QuestNode
	{
		// Token: 0x060147E8 RID: 83944 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060147E9 RID: 83945 RVA: 0x0061E390 File Offset: 0x0061C590
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_AddContentsToShuttle questPart_AddContentsToShuttle = new QuestPart_AddContentsToShuttle();
			questPart_AddContentsToShuttle.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_AddContentsToShuttle.shuttle = this.shuttle.GetValue(slate);
			questPart_AddContentsToShuttle.Things = this.contents.GetValue(slate);
			QuestGen.quest.AddPart(questPart_AddContentsToShuttle);
		}

		// Token: 0x0400E2EF RID: 58095
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E2F0 RID: 58096
		public SlateRef<Thing> shuttle;

		// Token: 0x0400E2F1 RID: 58097
		public SlateRef<IEnumerable<Thing>> contents;
	}
}
