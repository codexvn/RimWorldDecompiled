using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BE8 RID: 15336
	public class QuestNode_ResolveTextRequests : QuestNode
	{
		// Token: 0x06014CFE RID: 85246 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014CFF RID: 85247 RVA: 0x0063BDE4 File Offset: 0x00639FE4
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
				QuestGen.AddQuestContentRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveTextRequests.Resolve();
		}

		// Token: 0x06014D00 RID: 85248 RVA: 0x0063BE34 File Offset: 0x0063A034
		public static void Resolve()
		{
			List<QuestTextRequest> textRequestsReadOnly = QuestGen.TextRequestsReadOnly;
			for (int i = 0; i < textRequestsReadOnly.Count; i++)
			{
				try
				{
					List<Rule> list = new List<Rule>();
					list.AddRange(QuestGen.QuestDescriptionRulesReadOnly);
					list.AddRange(QuestGen.QuestContentRulesReadOnly);
					if (textRequestsReadOnly[i].extraRules != null)
					{
						list.AddRange(textRequestsReadOnly[i].extraRules);
					}
					string obj = QuestGenUtility.ResolveAbsoluteText(list, QuestGen.QuestDescriptionConstantsReadOnly, textRequestsReadOnly[i].keyword, true);
					textRequestsReadOnly[i].setter(obj);
				}
				catch (Exception ex)
				{
					string str = "Error while resolving text request: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			textRequestsReadOnly.Clear();
		}

		// Token: 0x0400E7AF RID: 59311
		public SlateRef<RulePack> rules;
	}
}
