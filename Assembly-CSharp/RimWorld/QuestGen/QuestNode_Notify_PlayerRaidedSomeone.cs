using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ADD RID: 15069
	public class QuestNode_Notify_PlayerRaidedSomeone : QuestNode
	{
		// Token: 0x0601488D RID: 84109 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601488E RID: 84110 RVA: 0x00620828 File Offset: 0x0061EA28
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Notify_PlayerRaidedSomeone questPart_Notify_PlayerRaidedSomeone = new QuestPart_Notify_PlayerRaidedSomeone();
			questPart_Notify_PlayerRaidedSomeone.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Notify_PlayerRaidedSomeone.getRaidersFromMap = this.getRaidersFromMap.GetValue(slate);
			questPart_Notify_PlayerRaidedSomeone.getRaidersFromMapParent = this.getRaidersFromMapParent.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Notify_PlayerRaidedSomeone);
		}

		// Token: 0x0400E3CD RID: 58317
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E3CE RID: 58318
		public SlateRef<Map> getRaidersFromMap;

		// Token: 0x0400E3CF RID: 58319
		public SlateRef<MapParent> getRaidersFromMapParent;
	}
}
