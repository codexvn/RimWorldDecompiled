using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ACE RID: 15054
	public class QuestNode_GiveTechprints : QuestNode
	{
		// Token: 0x06014860 RID: 84064 RVA: 0x0061FCDC File Offset: 0x0061DEDC
		protected override bool TestRunInt(Slate slate)
		{
			ResearchProjectDef researchProjectDef = this.FindTargetProject(slate);
			if (researchProjectDef == null || researchProjectDef.TechprintRequirementMet)
			{
				return false;
			}
			if (this.storeProjectAs.GetValue(slate) != null)
			{
				slate.Set<ResearchProjectDef>(this.storeProjectAs.GetValue(slate), researchProjectDef, false);
			}
			return true;
		}

		// Token: 0x06014861 RID: 84065 RVA: 0x0061FD24 File Offset: 0x0061DF24
		private ResearchProjectDef FindTargetProject(Slate slate)
		{
			if (this.fixedProject.GetValue(slate) != null)
			{
				return this.fixedProject.GetValue(slate);
			}
			return (from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.IsFinished && !p.TechprintRequirementMet
			select p).RandomElement<ResearchProjectDef>();
		}

		// Token: 0x06014862 RID: 84066 RVA: 0x0061FD7C File Offset: 0x0061DF7C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ResearchProjectDef researchProjectDef = this.FindTargetProject(slate);
			QuestPart_GiveTechprints questPart_GiveTechprints = new QuestPart_GiveTechprints();
			questPart_GiveTechprints.amount = 1;
			questPart_GiveTechprints.project = researchProjectDef;
			questPart_GiveTechprints.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_GiveTechprints.outSignalWasGiven = QuestGenUtility.HardcodedSignalWithQuestID("AddedTechprints");
			QuestGen.quest.AddPart(questPart_GiveTechprints);
			if (this.storeProjectAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<ResearchProjectDef>(this.storeProjectAs.GetValue(slate), researchProjectDef, false);
			}
		}

		// Token: 0x0400E38A RID: 58250
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E38B RID: 58251
		public SlateRef<ResearchProjectDef> fixedProject;

		// Token: 0x0400E38C RID: 58252
		[NoTranslate]
		public SlateRef<string> storeProjectAs;
	}
}
