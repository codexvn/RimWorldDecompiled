using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BE3 RID: 15331
	public class QuestNode_ResolveQuestDescription : QuestNode
	{
		// Token: 0x06014CED RID: 85229 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014CEE RID: 85230 RVA: 0x0063BB1C File Offset: 0x00639D1C
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestDescription.Resolve();
		}

		// Token: 0x06014CEF RID: 85231 RVA: 0x0063BB4C File Offset: 0x00639D4C
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestDescription", out text, false) && !QuestGen.quest.hidden)
			{
				text = QuestGenUtility.ResolveAbsoluteText(QuestGen.QuestDescriptionRulesReadOnly, QuestGen.QuestDescriptionConstantsReadOnly, "questDescription", true);
				QuestGen.slate.Set<string>("resolvedQuestDescription", text, false);
			}
			QuestGen.quest.description = text;
		}

		// Token: 0x0400E7A4 RID: 59300
		public SlateRef<RulePack> rules;

		// Token: 0x0400E7A5 RID: 59301
		public const string TextRoot = "questDescription";
	}
}
