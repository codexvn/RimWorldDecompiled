using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A79 RID: 14969
	public class QuestNode_GetRandomInRangeInt : QuestNode
	{
		// Token: 0x06014742 RID: 83778 RVA: 0x0061BEDC File Offset: 0x0061A0DC
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x06014743 RID: 83779 RVA: 0x0061BF14 File Offset: 0x0061A114
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x0400E259 RID: 57945
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E25A RID: 57946
		public SlateRef<IntRange> range;
	}
}
