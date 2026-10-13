using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B89 RID: 15241
	public class QuestNode_Root_ReliquaryPilgrims : QuestNode
	{
		// Token: 0x06014B39 RID: 84793 RVA: 0x00630CC4 File Offset: 0x0062EEC4
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Reliquary pilgrims"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			int randomInRange = QuestNode_Root_ReliquaryPilgrims.PilgrimCount.RandomInRange;
			if (map == null)
			{
				return;
			}
			FactionDef factionDef;
			PawnKindDef kindDef;
			this.GetFactionAndPawnKind(out factionDef, out kindDef);
			Precept_Relic precept_Relic;
			Building building;
			Thing var;
			QuestNode_Root_ReliquaryPilgrims.TryFindReliquaryWithRelic(map, out precept_Relic, out building, out var);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction2.def.PermanentlyHostileTo(factionDef))
				{
					list.Add(new FactionRelation
					{
						other = faction2,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(factionDef, list, true);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			List<Pawn> list2 = new List<Pawn>();
			int num = 0;
			bool var2 = false;
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				List<ValueTuple<int, float>> list3 = new List<ValueTuple<int, float>>();
				list3.Add(new ValueTuple<int, float>(0, 0.25f));
				list3.Add(new ValueTuple<int, float>(Rand.Range(1, Mathf.Max(1, randomInRange / 2)), 0.5f));
				list3.Add(new ValueTuple<int, float>(randomInRange, 0.25f));
				ValueTuple<int, float> valueTuple;
				list3.TryRandomElementByWeight((ValueTuple<int, float> p) => p.Item2, out valueTuple);
				num = valueTuple.Item1;
				var2 = (randomInRange == num);
			}
			slate.Set<int>("childCount", num, false);
			slate.Set<bool>("allChildren", var2, false);
			for (int i = 0; i < randomInRange; i++)
			{
				DevelopmentalStage developmentalStages = (i >= randomInRange - num) ? DevelopmentalStage.Child : DevelopmentalStage.Adult;
				Pawn pawn = quest.GeneratePawn(kindDef, faction, true, null, 0f, true, null, 0f, 0f, false, true, developmentalStages, false);
				pawn.ideo.SetIdeo(precept_Relic.ideo);
				list2.Add(pawn);
			}
			quest.SetFactionHidden(faction, false, null);
			quest.PawnsArrive(list2, null, map.Parent, null, false, null, "[pilgrimsArrivedLetterLabel]", "[pilgrimsArrivedLetterText]", null, null, false, false, true);
			string text = QuestGen.GenerateNewSignal("VenerationCompleted", true);
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("relicThing.Despawned");
			QuestPart_Venerate questPart_Venerate = new QuestPart_Venerate();
			questPart_Venerate.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_Venerate.inSignalForceExit = text2;
			questPart_Venerate.pawns.AddRange(list2);
			questPart_Venerate.target = building;
			questPart_Venerate.venerateDurationTicks = QuestNode_Root_ReliquaryPilgrims.VisitDurationTicksRange.RandomInRange;
			questPart_Venerate.faction = faction;
			questPart_Venerate.mapParent = map.Parent;
			questPart_Venerate.outSignalVenerationCompleted = text;
			quest.AddPart(questPart_Venerate);
			Quest quest2 = quest;
			string message = "[pilgrimsLeavingMessage]";
			MessageTypeDef neutralEvent = MessageTypeDefOf.NeutralEvent;
			bool getLookTargetsFromSignal = false;
			RulePack rules = null;
			string inSignal = text;
			quest2.Message(message, neutralEvent, getLookTargetsFromSignal, rules, list2, inSignal);
			string text3 = QuestGen.GenerateNewSignal("AllLeftMap", true);
			QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
			questPart_PassAll.outSignal = text3;
			quest.AddPart(questPart_PassAll);
			slate.Set<List<Pawn>>("pawns", list2, false);
			slate.Set<Faction>("faction", faction, false);
			foreach (Pawn pawn2 in list2)
			{
				string text4 = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(pawn2.ThingID);
				QuestUtility.AddQuestTag(pawn2, text4);
				string item = QuestGenUtility.HardcodedSignalWithQuestID(text4 + ".LeftMap");
				questPart_PassAll.inSignals.Add(item);
			}
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(QuestNode_Root_ReliquaryPilgrims.RewardMarketValueRange);
			parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
			parms.makingFaction = faction;
			parms.countRange = new IntRange?(new IntRange(1, 1));
			List<Thing> list4 = ThingSetMakerDefOf.Reward_ReliquaryPilgrims.root.Generate(parms);
			QuestPart_DelayedRewardDropPods delayedReward = new QuestPart_DelayedRewardDropPods();
			delayedReward.inSignal = text3;
			delayedReward.faction = faction;
			delayedReward.giver = list2[0];
			delayedReward.rewards.AddRange(list4);
			delayedReward.delayTicks = QuestNode_Root_ReliquaryPilgrims.RewardDelayRangeTicks.RandomInRange;
			delayedReward.chance = QuestNode_Root_ReliquaryPilgrims.RewardChance;
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				delayedReward.customLetterText = x;
			}, QuestGenUtility.MergeRules(null, "[delayedRewardLetterText]", "root"));
			quest.Message("[pilgrimsLeftMessage]", MessageTypeDefOf.NeutralEvent, false, null, null, text3);
			quest.AddPart(delayedReward);
			string item2 = QuestGenUtility.HardcodedSignalWithQuestID("pawns.Arrested");
			string item3 = QuestGenUtility.HardcodedSignalWithQuestID("pawns.Killed");
			quest.AnySignal(new List<string>
			{
				item2,
				item3
			}, delegate
			{
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Pilgrims_Betrayed, null, QuestPart.SignalListenMode.Always);
				QuestPart_FactionRelationChange questPart_FactionRelationChange = new QuestPart_FactionRelationChange();
				questPart_FactionRelationChange.faction = faction;
				questPart_FactionRelationChange.relationKind = FactionRelationKind.Hostile;
				questPart_FactionRelationChange.canSendHostilityLetter = false;
				questPart_FactionRelationChange.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				quest.AddPart(questPart_FactionRelationChange);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			string text5 = QuestGen.GenerateNewSignal("RelicInvalidated", true);
			QuestPart_PassAnyActivable questPart_PassAnyActivable = new QuestPart_PassAnyActivable();
			questPart_PassAnyActivable.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_PassAnyActivable.inSignals.Add(text2);
			questPart_PassAnyActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID("reliquary.Destroyed"));
			questPart_PassAnyActivable.outSignalsCompleted.Add(text5);
			questPart_PassAnyActivable.inSignalDisable = text;
			quest.AddPart(questPart_PassAnyActivable);
			quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Pilgrims, text2, QuestPart.SignalListenMode.OngoingOnly);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_PossibleFutureReward());
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				choice.rewards.Add(new Reward_DevelopmentPoints(quest));
			}
			questPart_Choice.choices.Add(choice);
			quest.End(QuestEndOutcome.Fail, 0, null, text5, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved"), QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("faction.BecameHostileToPlayer"), QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Success, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			slate.Set<Map>("map", map, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<Thing>("relicThing", var, false);
			slate.Set<Building>("reliquary", building, false);
			slate.Set<int>("pilgrimCount", randomInRange, false);
			slate.Set<string>("rewards", GenLabel.ThingsLabel(list4, "  - "), false);
			slate.Set<float>("rewardsMarketValue", TradeUtility.TotalMarketValue(list4), false);
			slate.Set<string>("venerateDate", GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(quest.acceptanceTick), Find.WorldGrid.LongLatOf(map.Tile)), false);
			slate.Set<bool>("pilgrimFaction", faction.def == FactionDefOf.Pilgrims, false);
		}

		// Token: 0x06014B3A RID: 84794 RVA: 0x0063140C File Offset: 0x0062F60C
		private static bool TryFindReliquaryWithRelic(Map map, out Precept_Relic relic, out Building reliquary, out Thing relicThing)
		{
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.Reliquary).InRandomOrder(null))
			{
				CompThingContainer compThingContainer = thing.TryGetComp<CompThingContainer>();
				if (compThingContainer != null)
				{
					foreach (Thing thing2 in ((IEnumerable<Thing>)compThingContainer.GetDirectlyHeldThings()))
					{
						Precept_Relic precept_Relic = thing2.StyleSourcePrecept as Precept_Relic;
						if (precept_Relic != null)
						{
							reliquary = (Building)thing;
							relic = precept_Relic;
							relicThing = thing2;
							return true;
						}
					}
				}
			}
			reliquary = null;
			relic = null;
			relicThing = null;
			return false;
		}

		// Token: 0x06014B3B RID: 84795 RVA: 0x006314D8 File Offset: 0x0062F6D8
		private void GetFactionAndPawnKind(out FactionDef factionDef, out PawnKindDef pawnKind)
		{
			if (Rand.Bool)
			{
				factionDef = FactionDefOf.Pilgrims;
				pawnKind = PawnKindDefOf.PovertyPilgrim;
				return;
			}
			factionDef = FactionDefOf.OutlanderCivil;
			pawnKind = PawnKindDefOf.WellEquippedTraveler;
		}

		// Token: 0x06014B3C RID: 84796 RVA: 0x00631500 File Offset: 0x0062F700
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null, false);
			Precept_Relic precept_Relic;
			Building building;
			Thing thing;
			return map != null && QuestNode_Root_ReliquaryPilgrims.TryFindReliquaryWithRelic(map, out precept_Relic, out building, out thing);
		}

		// Token: 0x0400E605 RID: 58885
		private static IntRange VisitDurationTicksRange = new IntRange(5000, 10000);

		// Token: 0x0400E606 RID: 58886
		private static IntRange PilgrimCount = new IntRange(1, 4);

		// Token: 0x0400E607 RID: 58887
		private static float RewardChance = 0.5f;

		// Token: 0x0400E608 RID: 58888
		private static FloatRange RewardMarketValueRange = new FloatRange(1000f, 2000f);

		// Token: 0x0400E609 RID: 58889
		private static IntRange RewardDelayRangeTicks = new IntRange(300000, 600000);

		// Token: 0x0400E60A RID: 58890
		private const string RootSymbol = "root";
	}
}
