using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ADE RID: 15070
	public class QuestNode_NoWorldObject : QuestNode
	{
		// Token: 0x06014890 RID: 84112 RVA: 0x0062089C File Offset: 0x0061EA9C
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06014891 RID: 84113 RVA: 0x006208B4 File Offset: 0x0061EAB4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_NoWorldObject questPart_NoWorldObject = new QuestPart_NoWorldObject();
			questPart_NoWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_NoWorldObject.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_NoWorldObject);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_NoWorldObject.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_NoWorldObject);
		}

		// Token: 0x0400E3D0 RID: 58320
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E3D1 RID: 58321
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400E3D2 RID: 58322
		public SlateRef<WorldObject> worldObject;

		// Token: 0x0400E3D3 RID: 58323
		public QuestNode node;
	}
}
