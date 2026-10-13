using System;

namespace RimWorld
{
	// Token: 0x02001BBE RID: 7102
	public class QuestPart_Filter_UnknownOutcome : QuestPart_Filter
	{
		// Token: 0x06009E14 RID: 40468 RVA: 0x002DBE00 File Offset: 0x002DA000
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return !args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) || questEndOutcome == QuestEndOutcome.Unknown;
		}
	}
}
