using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ABA RID: 15034
	public class QuestNode_DestroyOrPassToWorld : QuestNode
	{
		// Token: 0x0601481C RID: 83996 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601481D RID: 83997 RVA: 0x0061EE40 File Offset: 0x0061D040
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_DestroyThingsOrPassToWorld questPart_DestroyThingsOrPassToWorld = new QuestPart_DestroyThingsOrPassToWorld();
			questPart_DestroyThingsOrPassToWorld.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DestroyThingsOrPassToWorld.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DestroyThingsOrPassToWorld);
		}

		// Token: 0x0400E32C RID: 58156
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E32D RID: 58157
		public SlateRef<IEnumerable<Thing>> things;
	}
}
