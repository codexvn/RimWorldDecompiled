using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BEA RID: 15338
	public class QuestNode_TextRules : QuestNode
	{
		// Token: 0x06014D02 RID: 85250 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014D03 RID: 85251 RVA: 0x0063BF00 File Offset: 0x0063A100
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			switch (this.target.GetValue(slate))
			{
			case TextRulesTarget.Description:
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(slate));
				return;
			case TextRulesTarget.Name:
				QuestGen.AddQuestNameRules(this.rules.GetValue(slate));
				return;
			case TextRulesTarget.DecriptionAndName:
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(slate));
				QuestGen.AddQuestNameRules(this.rules.GetValue(slate));
				return;
			default:
				return;
			}
		}

		// Token: 0x0400E7B4 RID: 59316
		public SlateRef<RulePack> rules;

		// Token: 0x0400E7B5 RID: 59317
		public SlateRef<TextRulesTarget> target;
	}
}
