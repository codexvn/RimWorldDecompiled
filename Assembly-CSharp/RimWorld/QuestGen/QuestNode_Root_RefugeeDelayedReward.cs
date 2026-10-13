using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B50 RID: 15184
	public class QuestNode_Root_RefugeeDelayedReward : QuestNode
	{
		// Token: 0x06014A36 RID: 84534 RVA: 0x0062A0E8 File Offset: 0x006282E8
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			Faction faction = slate.Get<Faction>("faction", null, false);
			FloatRange marketValueRange = slate.Get<FloatRange>("marketValueRange", default(FloatRange), false);
			Pawn val = slate.Get<Pawn>("rewardGiver", null, false);
			quest.ReservePawns(Gen.YieldSingle<Pawn>(val));
			quest.ReserveFaction(faction);
			int num = Rand.Range(5, 20) * 60000;
			slate.Set<int>("rewardDelayTicks", num, false);
			quest.Delay(num, delegate
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.totalMarketValueRange = new FloatRange?(marketValueRange);
				parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
				parms.makingFaction = faction;
				List<Thing> list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
				slate.Set<string>("listOfRewards", GenLabel.ThingsLabel(list, "  - "), false);
				quest.DropPods(map.Parent, list, null, null, "[rewardLetterText]", null, new bool?(true), true, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, false, false, null);
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, "RewardDelay", false, QuestPart.SignalListenMode.OngoingOnly, true);
		}

		// Token: 0x06014A37 RID: 84535 RVA: 0x0062A1E4 File Offset: 0x006283E4
		protected override bool TestRunInt(Slate slate)
		{
			FloatRange floatRange;
			return slate.Get<Pawn>("rewardGiver", null, false) != null && slate.TryGet<FloatRange>("marketValueRange", out floatRange, false) && slate.Get<Faction>("faction", null, false) != null && QuestGen_Get.GetMap(false, null, false) != null;
		}
	}
}
