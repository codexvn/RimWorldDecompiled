using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A77 RID: 14967
	public class QuestNode_GetRandomInRangeFloat : QuestNode
	{
		// Token: 0x0601473B RID: 83771 RVA: 0x0061BDB0 File Offset: 0x00619FB0
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x0601473C RID: 83772 RVA: 0x0061BDE8 File Offset: 0x00619FE8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x0400E252 RID: 57938
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E253 RID: 57939
		public SlateRef<FloatRange> range;
	}
}
