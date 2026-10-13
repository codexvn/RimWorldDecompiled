using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ABC RID: 15036
	public class QuestNode_DestroyWorldObject : QuestNode
	{
		// Token: 0x06014822 RID: 84002 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014823 RID: 84003 RVA: 0x0061EF0C File Offset: 0x0061D10C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DestroyWorldObject questPart_DestroyWorldObject = new QuestPart_DestroyWorldObject();
			questPart_DestroyWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_DestroyWorldObject.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_DestroyWorldObject);
		}

		// Token: 0x0400E32F RID: 58159
		public SlateRef<WorldObject> worldObject;

		// Token: 0x0400E330 RID: 58160
		[NoTranslate]
		public SlateRef<string> inSignal;
	}
}
