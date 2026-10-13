using System;

namespace RimWorld
{
	// Token: 0x02001BB5 RID: 7093
	public class QuestPart_Filter_CanAcceptQuest : QuestPart_Filter
	{
		// Token: 0x06009DFE RID: 40446 RVA: 0x002DBC16 File Offset: 0x002D9E16
		protected override bool Pass(SignalArgs args)
		{
			return QuestUtility.CanAcceptQuest(this.quest);
		}
	}
}
