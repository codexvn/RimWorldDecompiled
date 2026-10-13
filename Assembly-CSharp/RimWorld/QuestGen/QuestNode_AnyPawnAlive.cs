using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BDE RID: 15326
	public class QuestNode_AnyPawnAlive : QuestNode
	{
		// Token: 0x06014CE0 RID: 85216 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014CE1 RID: 85217 RVA: 0x0063B960 File Offset: 0x00639B60
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter_AnyPawnAlive questPart_Filter_AnyPawnAlive = new QuestPart_Filter_AnyPawnAlive
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawns = this.pawns.GetValue(slate)
			};
			if (this.node != null)
			{
				questPart_Filter_AnyPawnAlive.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter_AnyPawnAlive.outSignal);
			}
			QuestGen.quest.AddPart(questPart_Filter_AnyPawnAlive);
		}

		// Token: 0x0400E79A RID: 59290
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E79B RID: 59291
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x0400E79C RID: 59292
		public QuestNode node;

		// Token: 0x0400E79D RID: 59293
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
