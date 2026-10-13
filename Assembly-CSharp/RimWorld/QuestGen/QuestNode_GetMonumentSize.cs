using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A57 RID: 14935
	public class QuestNode_GetMonumentSize : QuestNode
	{
		// Token: 0x060146CC RID: 83660 RVA: 0x00619C7E File Offset: 0x00617E7E
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060146CD RID: 83661 RVA: 0x00619C88 File Offset: 0x00617E88
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060146CE RID: 83662 RVA: 0x00619C95 File Offset: 0x00617E95
		private void DoWork(Slate slate)
		{
			if (this.monumentMarker.GetValue(slate) == null)
			{
				return;
			}
			slate.Set<IntVec2>(this.storeAs.GetValue(slate), this.monumentMarker.GetValue(slate).Size, false);
		}

		// Token: 0x0400E1D2 RID: 57810
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E1D3 RID: 57811
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
