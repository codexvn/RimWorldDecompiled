using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB2 RID: 15026
	public class QuestNode_CannotRun : QuestNode
	{
		// Token: 0x06014803 RID: 83971 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void RunInt()
		{
		}

		// Token: 0x06014804 RID: 83972 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool TestRunInt(Slate slate)
		{
			return false;
		}
	}
}
