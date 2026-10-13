using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B25 RID: 15141
	public class QuestNode_Root_AncientSignalActivation : QuestNode
	{
		// Token: 0x06014982 RID: 84354 RVA: 0x006251C0 File Offset: 0x006233C0
		protected override void RunInt()
		{
			QuestNode_Root_AncientSignalActivation.<>c__DisplayClass4_0 CS$<>8__locals1 = new QuestNode_Root_AncientSignalActivation.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			QuestNode_Root_AncientSignalActivation.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
			Map map;
			if ((map = slate.Get<Map>("map", null, false)) == null)
			{
				map = QuestGen_Get.GetMap(false, null, false);
			}
			CS$<>8__locals2.map = map;
			CS$<>8__locals1.dropSpot = null;
			LookTargets lookTargets = null;
			IntVec3 intVec;
			if (this.TryFindRandomDropSpot(CS$<>8__locals1.map, out intVec))
			{
				lookTargets = new LookTargets(intVec, CS$<>8__locals1.map);
				CS$<>8__locals1.dropSpot = new IntVec3?(intVec);
			}
			Quest quest = CS$<>8__locals1.quest;
			string message = "MessageAncientSignalActivated".Translate();
			MessageTypeDef negativeEvent = MessageTypeDefOf.NegativeEvent;
			bool getLookTargetsFromSignal = false;
			RulePack rules = null;
			string initiateSignal = CS$<>8__locals1.quest.InitiateSignal;
			quest.Message(message, negativeEvent, getLookTargetsFromSignal, rules, lookTargets, initiateSignal);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
			parms.makingFaction = Faction.OfAncients;
			CS$<>8__locals1.rewards = ThingSetMakerDefOf.ResourcePod.root.Generate(parms);
			CS$<>8__locals1.quest.Delay(180, delegate
			{
				Quest quest2 = CS$<>8__locals1.quest;
				MapParent parent = CS$<>8__locals1.map.Parent;
				IEnumerable<Thing> rewards = CS$<>8__locals1.rewards;
				string customLetterLabel = null;
				RulePack customLetterLabelRules = null;
				string customLetterText = null;
				RulePack customLetterTextRules = null;
				IntVec3? dropSpot = CS$<>8__locals1.dropSpot;
				quest2.DropPods(parent, rewards, customLetterLabel, customLetterLabelRules, customLetterText, customLetterTextRules, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, dropSpot, true, false, false, null);
				Faction faction;
				if (Rand.Chance(QuestNode_Root_AncientSignalActivation.RaidChance) && Find.Storyteller.difficulty.allowViolentQuests && CS$<>8__locals1.<>4__this.TryFindRandomEnemyFaction(out faction))
				{
					CS$<>8__locals1.quest.Delay(QuestNode_Root_AncientSignalActivation.RaidDelayTicks, delegate
					{
						Quest quest3 = CS$<>8__locals1.quest;
						MapParent parent2 = CS$<>8__locals1.map.Parent;
						float min = QuestNode_Root_AncientSignalActivation.RaidRewardPoints.min;
						Faction faction = faction;
						string inSignalLeave = null;
						string customLetterLabel2 = null;
						string customLetterText2 = "MessageAncientSignalHostileDetected".Translate(faction.def.pawnsPlural.CapitalizeFirst(), faction);
						RulePack customLetterLabelRules2 = null;
						RulePack customLetterTextRules2 = null;
						PawnsArrivalModeDef edgeWalkIn = PawnsArrivalModeDefOf.EdgeWalkIn;
						RaidStrategyDef immediateAttack = RaidStrategyDefOf.ImmediateAttack;
						quest3.Raid(parent2, min, faction, inSignalLeave, customLetterLabel2, customLetterText2, customLetterLabelRules2, customLetterTextRules2, null, null, null, "root", edgeWalkIn, immediateAttack, null, false, true, true, true);
						CS$<>8__locals1.quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
					return;
				}
				CS$<>8__locals1.quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, "RewardDelay", false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x06014983 RID: 84355 RVA: 0x006252E2 File Offset: 0x006234E2
		private bool TryFindRandomEnemyFaction(out Faction faction)
		{
			faction = Find.FactionManager.RandomRaidableEnemyFaction(false, false, true, TechLevel.Industrial);
			return faction != null;
		}

		// Token: 0x06014984 RID: 84356 RVA: 0x006252FC File Offset: 0x006234FC
		private bool TryFindRandomDropSpot(Map map, out IntVec3 result)
		{
			return CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && !x.Fogged(map), map, 1000, out result);
		}

		// Token: 0x06014985 RID: 84357 RVA: 0x00625333 File Offset: 0x00623533
		protected override bool TestRunInt(Slate slate)
		{
			return ModLister.CheckIdeology("Ancient signal activation quest");
		}

		// Token: 0x0400E4A4 RID: 58532
		private const int DropPodDelayTicks = 180;

		// Token: 0x0400E4A5 RID: 58533
		private static int RaidDelayTicks = 60 * Rand.Range(5, 10);

		// Token: 0x0400E4A6 RID: 58534
		private static FloatRange RaidRewardPoints = new FloatRange(150f, 200f);

		// Token: 0x0400E4A7 RID: 58535
		private static float RaidChance = 0.5f;
	}
}
