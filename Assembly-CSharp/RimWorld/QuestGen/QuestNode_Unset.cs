using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A0D RID: 14861
	public class QuestNode_Unset : QuestNode
	{
		// Token: 0x060145D4 RID: 83412 RVA: 0x00616758 File Offset: 0x00614958
		protected override bool TestRunInt(Slate slate)
		{
			slate.Remove(this.name.GetValue(slate), false);
			return true;
		}

		// Token: 0x060145D5 RID: 83413 RVA: 0x00616770 File Offset: 0x00614970
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Remove(this.name.GetValue(slate), false);
		}

		// Token: 0x0400E0EB RID: 57579
		[NoTranslate]
		public SlateRef<string> name;
	}
}
