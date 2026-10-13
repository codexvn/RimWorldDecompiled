using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A7E RID: 14974
	public class QuestNode_GetSameQuestsCount : QuestNode
	{
		// Token: 0x06014756 RID: 83798 RVA: 0x0061C48C File Offset: 0x0061A68C
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06014757 RID: 83799 RVA: 0x0061C496 File Offset: 0x0061A696
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014758 RID: 83800 RVA: 0x0061C4A4 File Offset: 0x0061A6A4
		private void SetVars(Slate slate)
		{
			int var = Find.QuestManager.QuestsListForReading.Count((Quest x) => x.root == QuestGen.Root);
			slate.Set<int>("sameQuestsCount", var, false);
		}

		// Token: 0x0400E26B RID: 57963
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
