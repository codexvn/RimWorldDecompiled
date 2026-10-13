using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD9 RID: 15065
	public class QuestNode_MakeMinified : QuestNode
	{
		// Token: 0x06014882 RID: 84098 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014883 RID: 84099 RVA: 0x0062061C File Offset: 0x0061E81C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MinifiedThing var = this.thing.GetValue(slate).MakeMinified(DestroyMode.Vanish);
			QuestGen.slate.Set<MinifiedThing>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0400E3BE RID: 58302
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E3BF RID: 58303
		public SlateRef<Thing> thing;
	}
}
