using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ADA RID: 15066
	public class QuestNode_Message : QuestNode
	{
		// Token: 0x06014885 RID: 84101 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014886 RID: 84102 RVA: 0x0062065C File Offset: 0x0061E85C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Message message = new QuestPart_Message();
			message.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			message.messageType = (this.messageType.GetValue(slate) ?? MessageTypeDefOf.NeutralEvent);
			message.lookTargets = QuestGenUtility.ToLookTargets(this.lookTargets, slate);
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				message.message = x;
			}, QuestGenUtility.MergeRules(this.rules.GetValue(slate), this.text.GetValue(slate), "root"));
			QuestGen.quest.AddPart(message);
		}

		// Token: 0x0400E3C0 RID: 58304
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E3C1 RID: 58305
		public SlateRef<MessageTypeDef> messageType;

		// Token: 0x0400E3C2 RID: 58306
		public SlateRef<string> text;

		// Token: 0x0400E3C3 RID: 58307
		public SlateRef<RulePack> rules;

		// Token: 0x0400E3C4 RID: 58308
		[NoTranslate]
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x0400E3C5 RID: 58309
		private const string RootSymbol = "root";
	}
}
