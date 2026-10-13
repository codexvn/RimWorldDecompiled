using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039DF RID: 14815
	public class QuestNode_Add : QuestNode
	{
		// Token: 0x06014543 RID: 83267 RVA: 0x0061414C File Offset: 0x0061234C
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x06014544 RID: 83268 RVA: 0x00614164 File Offset: 0x00612364
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) + this.value2.GetValue(slate), false);
		}

		// Token: 0x0400E041 RID: 57409
		public SlateRef<double> value1;

		// Token: 0x0400E042 RID: 57410
		public SlateRef<double> value2;

		// Token: 0x0400E043 RID: 57411
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
