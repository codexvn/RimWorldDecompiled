using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A51 RID: 14929
	public class QuestNode_GetMapWealth : QuestNode
	{
		// Token: 0x060146B5 RID: 83637 RVA: 0x00619314 File Offset: 0x00617514
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
			return true;
		}

		// Token: 0x060146B6 RID: 83638 RVA: 0x00619340 File Offset: 0x00617540
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
		}

		// Token: 0x0400E1C5 RID: 57797
		public SlateRef<Map> map;

		// Token: 0x0400E1C6 RID: 57798
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
