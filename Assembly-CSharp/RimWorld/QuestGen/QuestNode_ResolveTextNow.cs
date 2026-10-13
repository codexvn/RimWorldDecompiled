using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BE7 RID: 15335
	public class QuestNode_ResolveTextNow : QuestNode
	{
		// Token: 0x06014CFB RID: 85243 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014CFC RID: 85244 RVA: 0x0063BD9C File Offset: 0x00639F9C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string var = QuestGenUtility.ResolveLocalTextWithDescriptionRules(this.rules.GetValue(slate), this.root.GetValue(slate));
			slate.Set<string>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0400E7AC RID: 59308
		[NoTranslate]
		public SlateRef<string> root;

		// Token: 0x0400E7AD RID: 59309
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E7AE RID: 59310
		public SlateRef<RulePack> rules;
	}
}
