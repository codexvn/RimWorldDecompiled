using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BE4 RID: 15332
	public class QuestNode_ResolveQuestName : QuestNode
	{
		// Token: 0x06014CF1 RID: 85233 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014CF2 RID: 85234 RVA: 0x0063BBB0 File Offset: 0x00639DB0
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestNameRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestName.Resolve();
		}

		// Token: 0x06014CF3 RID: 85235 RVA: 0x0063BBE0 File Offset: 0x00639DE0
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestName", out text, false))
			{
				text = (QuestGen.quest.hidden ? QuestGen.quest.root.defName : QuestNode_ResolveQuestName.GenerateName().StripTags());
				QuestGen.slate.Set<string>("resolvedQuestName", text, false);
			}
			QuestGen.quest.name = text;
		}

		// Token: 0x06014CF4 RID: 85236 RVA: 0x0063BC48 File Offset: 0x00639E48
		private static string GenerateName()
		{
			GrammarRequest request = default(GrammarRequest);
			request.Rules.AddRange(QuestGen.QuestNameRulesReadOnly);
			foreach (KeyValuePair<string, string> keyValuePair in QuestGen.QuestNameConstantsReadOnly)
			{
				request.Constants.Add(keyValuePair.Key, keyValuePair.Value);
			}
			QuestGenUtility.AddSlateVars(ref request);
			Predicate<string> predicate = (string x) => !Find.QuestManager.QuestsListForReading.Any((Quest y) => y.name == x);
			if (QuestGen.Root.nameMustBeUnique)
			{
				return NameGenerator.GenerateName(request, predicate, false, "questName", null);
			}
			string text = null;
			for (int i = 0; i < 20; i++)
			{
				text = NameGenerator.GenerateName(request, null, false, "questName", null);
				if (predicate(text) || QuestGen.Root.defaultHidden)
				{
					break;
				}
			}
			return text;
		}

		// Token: 0x0400E7A6 RID: 59302
		public SlateRef<RulePack> rules;

		// Token: 0x0400E7A7 RID: 59303
		public const string TextRoot = "questName";

		// Token: 0x0400E7A8 RID: 59304
		private const int MaxTriesTryAvoidDuplicateName = 20;
	}
}
