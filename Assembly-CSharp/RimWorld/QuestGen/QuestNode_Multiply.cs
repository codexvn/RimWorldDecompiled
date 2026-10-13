using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E1 RID: 14817
	public class QuestNode_Multiply : QuestNode
	{
		// Token: 0x06014549 RID: 83273 RVA: 0x00614203 File Offset: 0x00612403
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0601454A RID: 83274 RVA: 0x0061421C File Offset: 0x0061241C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) * this.value2.GetValue(slate), false);
		}

		// Token: 0x0400E047 RID: 57415
		public SlateRef<double> value1;

		// Token: 0x0400E048 RID: 57416
		public SlateRef<double> value2;

		// Token: 0x0400E049 RID: 57417
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
