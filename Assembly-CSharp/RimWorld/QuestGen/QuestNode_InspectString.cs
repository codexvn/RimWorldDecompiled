using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD1 RID: 15057
	public class QuestNode_InspectString : QuestNode
	{
		// Token: 0x0601486A RID: 84074 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601486B RID: 84075 RVA: 0x0061FF28 File Offset: 0x0061E128
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				return;
			}
			QuestPart_InspectString questPart_InspectString = new QuestPart_InspectString();
			questPart_InspectString.targets.AddRange(this.targets.GetValue(slate));
			questPart_InspectString.inspectString = this.inspectString.GetValue(slate);
			questPart_InspectString.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_InspectString);
		}

		// Token: 0x0400E395 RID: 58261
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E396 RID: 58262
		public SlateRef<IEnumerable<ISelectable>> targets;

		// Token: 0x0400E397 RID: 58263
		public SlateRef<string> inspectString;
	}
}
