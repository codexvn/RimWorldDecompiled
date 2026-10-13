using System;

namespace RimWorld
{
	// Token: 0x02001BBC RID: 7100
	public class QuestPart_Filter_Success : QuestPart_Filter
	{
		// Token: 0x06009E0F RID: 40463 RVA: 0x002DBD80 File Offset: 0x002D9F80
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Success;
		}
	}
}
