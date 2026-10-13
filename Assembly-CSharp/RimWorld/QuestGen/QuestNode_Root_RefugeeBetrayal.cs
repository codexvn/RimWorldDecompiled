using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B52 RID: 15186
	public class QuestNode_Root_RefugeeBetrayal : QuestNode
	{
		// Token: 0x06014A3B RID: 84539 RVA: 0x0062A2F0 File Offset: 0x006284F0
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			slate.Set<Map>("map", map, false);
			List<Pawn> list = slate.Get<List<Pawn>>("lodgers", null, false);
			ExtraFaction extraFaction = slate.Get<ExtraFaction>("refugeeFaction", null, false);
			Pawn factionOpponent = slate.Get<Pawn>("factionOpponent", null, false);
			float val = (float)list.Count * 300f;
			FloatRange value = new FloatRange(0.7f, 1.3f) * val * Find.Storyteller.difficulty.EffectiveQuestRewardValueFactor;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(value);
			parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
			parms.makingFaction = extraFaction.faction;
			string item = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Recruited");
			string item2 = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.BecameMutant");
			string item3 = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Destroyed");
			string item4 = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Kidnapped");
			string item5 = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.LeftMap");
			string item6 = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Banished");
			List<Thing> betrayalRewardThings = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
			quest.ExtraFaction(extraFaction.faction, list, ExtraFactionType.MiniFaction, false, new List<string>
			{
				item,
				item2
			});
			quest.BetrayalOffer(list, extraFaction, factionOpponent, delegate
			{
				float num = 0f;
				for (int i = 0; i < betrayalRewardThings.Count; i++)
				{
					num += betrayalRewardThings[i].MarketValue * (float)betrayalRewardThings[i].stackCount;
				}
				slate.Set<string>("betrayalRewards", GenLabel.ThingsLabel(betrayalRewardThings, "  - "), false);
				slate.Set<float>("betrayalRewardMarketValue", num, false);
				quest.DropPods(map.Parent, betrayalRewardThings, null, null, null, null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.Always, null, false, false, false, null);
				quest.FactionGoodwillChange(factionOpponent.Faction, 10, null, true, true, true, null, QuestPart.SignalListenMode.Always, false);
				quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.Always, betrayalRewardThings, false, "[betrayalOfferRewardLetterText]", null, "[betrayalOfferRewardLetterLabel]", null, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.Always, false, false);
			}, delegate
			{
				string text;
				string text2;
				if (QuestGen.slate.Get<string>("inSignal", null, false).EndsWith("BetrayalOfferFailureRecruited"))
				{
					text = "[betrayalOfferFailedBecauseRecruitedLetterLabel]";
					text2 = "[betrayalOfferFailedBecauseRecruitedLetterText]";
				}
				else
				{
					text = "[betrayalOfferFailedLetterLabel]";
					text2 = "[betrayalOfferFailedLetterText]";
				}
				Quest quest;
				quest.DestroyThingsOrPassToWorld(betrayalRewardThings, null, true, QuestPart.SignalListenMode.Always);
				quest = quest;
				LetterDef negativeEvent = LetterDefOf.NegativeEvent;
				string inSignal = null;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.Always;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = text;
				quest.Letter(negativeEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, text2, null, label, null, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.Always, false, false);
			}, null, new List<string>
			{
				item,
				item3,
				item4,
				item5,
				item6
			}, null, QuestPart.SignalListenMode.Always);
		}

		// Token: 0x06014A3C RID: 84540 RVA: 0x0062A4D4 File Offset: 0x006286D4
		protected override bool TestRunInt(Slate slate)
		{
			FloatRange floatRange;
			return slate.Get<Pawn>("rewardGiver", null, false) != null && slate.TryGet<FloatRange>("marketValueRange", out floatRange, false) && slate.Get<Faction>("faction", null, false) != null && QuestGen_Get.GetMap(false, null, false) != null;
		}

		// Token: 0x0400E53A RID: 58682
		public const string FailureRecruitedSignal = "BetrayalOfferFailureRecruited";

		// Token: 0x0400E53B RID: 58683
		public const string FailureLeftSignal = "BetrayalOfferFailure";
	}
}
