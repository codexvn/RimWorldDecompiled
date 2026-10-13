using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E3 RID: 14819
	public class QuestNode_Subtract : QuestNode
	{
		// Token: 0x0601454F RID: 83279 RVA: 0x006142B7 File Offset: 0x006124B7
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x06014550 RID: 83280 RVA: 0x006142D0 File Offset: 0x006124D0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) - this.value2.GetValue(slate), false);
		}

		// Token: 0x0400E04D RID: 57421
		public SlateRef<double> value1;

		// Token: 0x0400E04E RID: 57422
		public SlateRef<double> value2;

		// Token: 0x0400E04F RID: 57423
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
