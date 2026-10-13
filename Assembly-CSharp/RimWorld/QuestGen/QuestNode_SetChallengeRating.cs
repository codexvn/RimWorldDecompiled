using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AF1 RID: 15089
	public class QuestNode_SetChallengeRating : QuestNode
	{
		// Token: 0x060148CC RID: 84172 RVA: 0x0062158E File Offset: 0x0061F78E
		protected override void RunInt()
		{
			QuestGen.quest.challengeRating = this.challengeRating.GetValue(QuestGen.slate);
		}

		// Token: 0x060148CD RID: 84173 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E40F RID: 58383
		public SlateRef<int> challengeRating;
	}
}
