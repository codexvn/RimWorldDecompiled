using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BE1 RID: 15329
	public class QuestNode_Filter_DecreeNotPossible : QuestNode_Filter
	{
		// Token: 0x06014CE9 RID: 85225 RVA: 0x0063BB0E File Offset: 0x00639D0E
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			return new QuestPart_Filter_DecreeNotPossible();
		}
	}
}
