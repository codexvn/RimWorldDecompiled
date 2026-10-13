using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BE2 RID: 15330
	public class QuestNode_Filter_FactionNonPlayer : QuestNode_Filter
	{
		// Token: 0x06014CEB RID: 85227 RVA: 0x0063BB15 File Offset: 0x00639D15
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			return new QuestPart_Filter_FactionNonPlayer();
		}
	}
}
