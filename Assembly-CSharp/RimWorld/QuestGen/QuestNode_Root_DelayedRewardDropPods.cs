using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B41 RID: 15169
	public class QuestNode_Root_DelayedRewardDropPods : QuestNode
	{
		// Token: 0x060149FB RID: 84475 RVA: 0x00628918 File Offset: 0x00626B18
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			List<Thing> rewards = slate.Get<List<Thing>>("rewards", null, false);
			Faction faction = slate.Get<Faction>("faction", null, false);
			Pawn pawn = slate.Get<Pawn>("giver", null, false);
			int delayTicks = slate.Get<int>("delayTicks", 0, false);
			string customerLetterLabel = slate.Get<string>("customLetterLabel", null, false);
			string customerLetterText = slate.Get<string>("customLetterText", null, false);
			if (pawn != null)
			{
				quest.ReservePawns(Gen.YieldSingle<Pawn>(pawn));
			}
			if (faction != null)
			{
				quest.ReserveFaction(faction);
			}
			quest.Delay(delayTicks, delegate
			{
				quest.DropPods(map.Parent, rewards, customerLetterLabel, null, customerLetterText, null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, false, false, null);
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, "RewardDelay", false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x060149FC RID: 84476 RVA: 0x00628A07 File Offset: 0x00626C07
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<List<Thing>>("rewards", null, false) != null;
		}
	}
}
