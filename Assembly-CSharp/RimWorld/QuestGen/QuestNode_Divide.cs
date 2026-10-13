using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E0 RID: 14816
	public class QuestNode_Divide : QuestNode
	{
		// Token: 0x06014546 RID: 83270 RVA: 0x006141AB File Offset: 0x006123AB
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x06014547 RID: 83271 RVA: 0x006141C4 File Offset: 0x006123C4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) / this.value2.GetValue(slate), false);
		}

		// Token: 0x0400E044 RID: 57412
		public SlateRef<double> value1;

		// Token: 0x0400E045 RID: 57413
		public SlateRef<double> value2;

		// Token: 0x0400E046 RID: 57414
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
