using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AAA RID: 15018
	public class QuestNode_AddHediff : QuestNode
	{
		// Token: 0x060147EB RID: 83947 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060147EC RID: 83948 RVA: 0x0061E410 File Offset: 0x0061C610
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null || this.hediffDef.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_AddHediff questPart_AddHediff = new QuestPart_AddHediff();
			questPart_AddHediff.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_AddHediff.hediffDef = this.hediffDef.GetValue(slate);
			questPart_AddHediff.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_AddHediff.checkDiseaseContractChance = this.checkDiseaseContractChance.GetValue(slate);
			if (this.partsToAffect.GetValue(slate) != null)
			{
				questPart_AddHediff.partsToAffect = new List<BodyPartDef>();
				questPart_AddHediff.partsToAffect.AddRange(this.partsToAffect.GetValue(slate));
			}
			questPart_AddHediff.addToHyperlinks = this.addToHyperlinks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_AddHediff);
		}

		// Token: 0x0400E2F2 RID: 58098
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E2F3 RID: 58099
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E2F4 RID: 58100
		public SlateRef<HediffDef> hediffDef;

		// Token: 0x0400E2F5 RID: 58101
		public SlateRef<IEnumerable<BodyPartDef>> partsToAffect;

		// Token: 0x0400E2F6 RID: 58102
		public SlateRef<bool> checkDiseaseContractChance;

		// Token: 0x0400E2F7 RID: 58103
		public SlateRef<bool> addToHyperlinks;
	}
}
