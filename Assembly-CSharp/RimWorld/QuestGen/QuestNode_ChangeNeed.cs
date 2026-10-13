using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB8 RID: 15032
	public class QuestNode_ChangeNeed : QuestNode
	{
		// Token: 0x06014816 RID: 83990 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014817 RID: 83991 RVA: 0x0061ED1C File Offset: 0x0061CF1C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeNeed questPart_ChangeNeed = new QuestPart_ChangeNeed();
			questPart_ChangeNeed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeNeed.pawn = this.pawn.GetValue(slate);
			questPart_ChangeNeed.need = this.need.GetValue(slate);
			questPart_ChangeNeed.offset = this.offset.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeNeed);
		}

		// Token: 0x0400E325 RID: 58149
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E326 RID: 58150
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E327 RID: 58151
		public SlateRef<NeedDef> need;

		// Token: 0x0400E328 RID: 58152
		public SlateRef<float> offset;
	}
}
