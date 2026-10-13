using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E2 RID: 14818
	public class QuestNode_MultiplyRange : QuestNode
	{
		// Token: 0x0601454C RID: 83276 RVA: 0x0061425B File Offset: 0x0061245B
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0601454D RID: 83277 RVA: 0x00614274 File Offset: 0x00612474
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<FloatRange>(this.storeAs.GetValue(slate), this.range.GetValue(slate) * this.value.GetValue(slate), false);
		}

		// Token: 0x0400E04A RID: 57418
		public SlateRef<FloatRange> range;

		// Token: 0x0400E04B RID: 57419
		public SlateRef<float> value;

		// Token: 0x0400E04C RID: 57420
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
