using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039F6 RID: 14838
	public class QuestNode_AddToList : QuestNode
	{
		// Token: 0x06014586 RID: 83334 RVA: 0x00614DDC File Offset: 0x00612FDC
		protected override bool TestRunInt(Slate slate)
		{
			QuestGenUtility.AddToOrMakeList(slate, this.name.GetValue(slate), this.value.GetValue(slate));
			return true;
		}

		// Token: 0x06014587 RID: 83335 RVA: 0x00614E00 File Offset: 0x00613000
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.name.GetValue(slate), this.value.GetValue(slate));
		}

		// Token: 0x0400E088 RID: 57480
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400E089 RID: 57481
		public SlateRef<object> value;
	}
}
