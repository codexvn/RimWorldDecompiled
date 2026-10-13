using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A71 RID: 14961
	public class QuestNode_GetRandomElement : QuestNode
	{
		// Token: 0x06014729 RID: 83753 RVA: 0x0061BB8F File Offset: 0x00619D8F
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0601472A RID: 83754 RVA: 0x0061BB99 File Offset: 0x00619D99
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0601472B RID: 83755 RVA: 0x0061BBA8 File Offset: 0x00619DA8
		private void SetVars(Slate slate)
		{
			SlateRef<object> slateRef;
			if (this.options.TryRandomElement(out slateRef))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), slateRef.GetValue(slate), false);
			}
		}

		// Token: 0x0400E245 RID: 57925
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E246 RID: 57926
		public List<SlateRef<object>> options;
	}
}
