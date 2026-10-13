using System;

namespace RimWorld
{
	// Token: 0x02001BB8 RID: 7096
	public class QuestPart_Filter_Fail : QuestPart_Filter
	{
		// Token: 0x06009E05 RID: 40453 RVA: 0x002DBCD4 File Offset: 0x002D9ED4
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Fail;
		}
	}
}
