using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A39 RID: 14905
	public class QuestNode_GetBodySize : QuestNode
	{
		// Token: 0x06014663 RID: 83555 RVA: 0x00618342 File Offset: 0x00616542
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
			return true;
		}

		// Token: 0x06014664 RID: 83556 RVA: 0x00618370 File Offset: 0x00616570
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
		}

		// Token: 0x0400E179 RID: 57721
		public SlateRef<PawnKindDef> pawnKind;

		// Token: 0x0400E17A RID: 57722
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
