using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039F4 RID: 14836
	public class QuestNode_AddRangeToList : QuestNode
	{
		// Token: 0x06014580 RID: 83328 RVA: 0x00614CAC File Offset: 0x00612EAC
		protected override bool TestRunInt(Slate slate)
		{
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
			return true;
		}

		// Token: 0x06014581 RID: 83329 RVA: 0x00614CE0 File Offset: 0x00612EE0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
		}

		// Token: 0x0400E084 RID: 57476
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400E085 RID: 57477
		public SlateRef<List<object>> value;
	}
}
