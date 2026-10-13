using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B34 RID: 15156
	public class QuestNode_Root_Beggars : QuestNode
	{
		// Token: 0x17003360 RID: 13152
		// (get) Token: 0x060149BA RID: 84410 RVA: 0x00626113 File Offset: 0x00624313
		private static IEnumerable<ThingDef> AllowedThings
		{
			get
			{
				yield return ThingDefOf.Silver;
				yield return ThingDefOf.MedicineHerbal;
				yield return ThingDefOf.MedicineIndustrial;
				yield return ThingDefOf.Penoxycyline;
				yield return ThingDefOf.Beer;
				yield break;
			}
		}

		// Token: 0x060149BB RID: 84411 RVA: 0x0062611C File Offset: 0x0062431C
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Beggars"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float points = slate.Get<float>("points", 0f, false);
			slate.Set<int>("visitDurationTicks", 60000, false);
			float totalRequestValue = QuestNode_Root_Beggars.GetTotalRequestValue(points);
			ThingDef thingDef;
			int num;
			if (QuestNode_Root_Beggars.TryFindRandomRequestedThing(map, totalRequestValue, out thingDef, out num, QuestNode_Root_Beggars.AllowedThings))
			{
				slate.Set<ThingDef>("requestedThing", thingDef, false);
				slate.Set<string>("requestedThingDefName", thingDef.defName, false);
				slate.Set<int>("requestedThingCount", num, false);
			}
			int num2 = QuestNode_Root_Beggars.LodgerCountFromPopulation(slate, map);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction3 in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction3.def.PermanentlyHostileTo(FactionDefOf.Beggars))
				{
					list.Add(new FactionRelation
					{
						other = faction3,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(FactionDefOf.Beggars, list, true);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			slate.Set<Faction>("faction", faction, false);
			slate.Set<Map>("map", map, false);
			int num3 = 0;
			bool var = false;
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				List<ValueTuple<int, float>> list2 = new List<ValueTuple<int, float>>();
				list2.Add(new ValueTuple<int, float>(0, 0.4f));
				list2.Add(new ValueTuple<int, float>(Rand.Range(1, Mathf.Max(1, num2 / 2)), 0.4f));
				list2.Add(new ValueTuple<int, float>(num2, 0.2f));
				ValueTuple<int, float> valueTuple;
				list2.TryRandomElementByWeight((ValueTuple<int, float> p) => p.Item2, out valueTuple);
				num3 = valueTuple.Item1;
				var = (num2 == num3);
			}
			slate.Set<int>("childCount", num3, false);
			slate.Set<bool>("allChildren", var, false);
			List<Pawn> pawns = new List<Pawn>();
			for (int i = 0; i < num2; i++)
			{
				DevelopmentalStage developmentalStage = (i >= num2 - num3) ? DevelopmentalStage.Child : DevelopmentalStage.Adult;
				List<Pawn> pawns2 = pawns;
				Quest quest4 = quest;
				PawnKindDef beggar = PawnKindDefOf.Beggar;
				Faction faction2 = faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				DevelopmentalStage developmentalStages = developmentalStage;
				pawns2.Add(quest4.GeneratePawn(new PawnGenerationRequest(beggar, faction2, context, null, true, false, false, true, false, 1f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, null, null, false, false, false, -1, 0, false), false));
			}
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn = pawns[j];
				if (pawn.inventory != null)
				{
					for (int k = pawn.inventory.innerContainer.Count - 1; k >= 0; k--)
					{
						if (pawn.inventory.innerContainer[k].def == thingDef)
						{
							pawn.inventory.innerContainer[k].Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
			slate.Set<List<Pawn>>("beggars", pawns, false);
			slate.Set<int>("beggarCount", num2, false);
			quest.SetFactionHidden(faction, false, null);
			quest.PawnsArrive(pawns, null, map.Parent, null, false, null, null, null, null, null, false, false, false);
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("Receiver");
			string itemsReceivedSignal = QuestGen.GenerateNewSignal("ItemsReceived", true);
			Pawn pawn2 = pawns[0];
			QuestUtility.AddQuestTag(ref pawn2.questTags, text);
			QuestPart_BegForItems questPart_BegForItems = new QuestPart_BegForItems();
			questPart_BegForItems.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_BegForItems.outSignalItemsReceived = itemsReceivedSignal;
			questPart_BegForItems.pawns.AddRange(pawns);
			questPart_BegForItems.target = pawn2;
			questPart_BegForItems.faction = faction;
			questPart_BegForItems.mapParent = map.Parent;
			questPart_BegForItems.thingDef = thingDef;
			questPart_BegForItems.amount = num;
			quest.AddPart(questPart_BegForItems);
			quest.GiveRewards(new RewardsGeneratorParams
			{
				rewardValue = totalRequestValue,
				allowDevelopmentPoints = true,
				thingRewardDisallowed = true
			}, null, null, null, null, null, null, null, null, false, null, false, false, null);
			if (ModsConfig.OdysseyActive)
			{
				string inSignal = QuestGenUtility.HardcodedSignalWithQuestID(text + ".ReceivedItems");
				List<QuestScriptDef> list3 = QuestUtility.GetGiverQuests(QuestGiverTag.Beggars).ToList<QuestScriptDef>();
				if (Rand.Chance(0.35f) && !list3.NullOrEmpty<QuestScriptDef>())
				{
					QuestPart_AddGiverQuest questPart_AddGiverQuest = new QuestPart_AddGiverQuest();
					questPart_AddGiverQuest.inSignal = inSignal;
					questPart_AddGiverQuest.questScript = list3.RandomElementByWeight((QuestScriptDef q) => q.rootSelectionWeight);
					questPart_AddGiverQuest.discoveryMethodTranslationKey = "QuestDiscoveredFromBeggar";
					questPart_AddGiverQuest.sendAvailableLetter = true;
					questPart_AddGiverQuest.points = points;
					quest.AddPart(questPart_AddGiverQuest);
				}
			}
			string pawnLabelSingleOrPlural = (num2 > 1) ? faction.def.pawnsPlural : faction.def.pawnSingular;
			Action <>9__7;
			quest.Delay(60000, delegate
			{
				quest.Leave(pawns, null, false, false, null, false);
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Beggars, null, QuestPart.SignalListenMode.OngoingOnly);
				Quest quest2 = quest;
				Action action;
				if ((action = <>9__7) == null)
				{
					action = (<>9__7 = delegate()
					{
						quest.Message(string.Format("{0}: {1}", "MessageCharityEventRefused".Translate(), "MessageBeggarsLeavingWithNoItems".Translate(pawnLabelSingleOrPlural)), MessageTypeDefOf.NegativeEvent, false, null, pawns, null);
					});
				}
				quest2.AnyColonistWithCharityPrecept(action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, itemsReceivedSignal, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("beggars.Arrested");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("beggars.Killed");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("beggars.LeftMap");
			TaggedString leavingWithItemsMessage = (num2 > 1) ? "MessageBeggarsLeavingWithItemsPlural".Translate(pawnLabelSingleOrPlural) : "MessageBeggarsLeavingWithItemsSingular".Translate(pawnLabelSingleOrPlural);
			quest.AnyColonistWithCharityPrecept(delegate
			{
				quest.Message("MessageCharityEventFulfilled".Translate() + ": " + leavingWithItemsMessage, MessageTypeDefOf.PositiveEvent, false, null, pawns, null);
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityFulfilled_Beggars, null, QuestPart.SignalListenMode.OngoingOnly);
			}, delegate
			{
				quest.Message(leavingWithItemsMessage, MessageTypeDefOf.PositiveEvent, false, null, pawns, null);
			}, itemsReceivedSignal, null, null, QuestPart.SignalListenMode.OngoingOnly);
			Action <>9__9;
			Action <>9__8;
			quest.AnySignal(new string[]
			{
				text3,
				text2
			}, delegate
			{
				Quest quest2 = quest;
				Action action;
				if ((action = <>9__8) == null)
				{
					action = (<>9__8 = delegate()
					{
						Quest quest3 = quest;
						Action action2;
						if ((action2 = <>9__9) == null)
						{
							action2 = (<>9__9 = delegate()
							{
								quest.Message(string.Format("{0}: {1}", "MessageCharityEventRefused".Translate(), "MessageBeggarsLeavingWithNoItems".Translate(pawnLabelSingleOrPlural)), MessageTypeDefOf.NegativeEvent, false, null, pawns, null);
							});
						}
						quest3.AnyColonistWithCharityPrecept(action2, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
					});
				}
				quest2.SignalPassActivable(action, null, null, null, null, itemsReceivedSignal, false);
				quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, pawns, false, "[letterTextBeggarsBetrayed]", null, "[letterLabelBeggarsBetrayed]", null, null);
				QuestPart_FactionRelationChange questPart_FactionRelationChange = new QuestPart_FactionRelationChange();
				questPart_FactionRelationChange.faction = faction;
				questPart_FactionRelationChange.relationKind = FactionRelationKind.Hostile;
				questPart_FactionRelationChange.canSendHostilityLetter = false;
				questPart_FactionRelationChange.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				quest.AddPart(questPart_FactionRelationChange);
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Beggars_Betrayed, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved"), QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("faction.BecameHostileToPlayer"), QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.AllPawnsDespawned(pawns, delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, inSignal2, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x060149BC RID: 84412 RVA: 0x00626844 File Offset: 0x00624A44
		public static int LodgerCountFromPopulation(Slate slate, Map map)
		{
			int num = slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : map.mapPawns.FreeAdultColonistsSpawnedCount;
			float randomInRange;
			if (num <= QuestNode_Root_Beggars.PopulationCountToCurve.TrueMin)
			{
				randomInRange = QuestNode_Root_Beggars.LodgerCountBasedOnColonyPopulationFactorRange_Low.RandomInRange;
			}
			else if (num >= QuestNode_Root_Beggars.PopulationCountToCurve.TrueMax)
			{
				randomInRange = QuestNode_Root_Beggars.LodgerCountBaseOnColonyPopulationFactorRange_High.RandomInRange;
			}
			else
			{
				float t = Mathf.InverseLerp((float)QuestNode_Root_Beggars.PopulationCountToCurve.TrueMin, (float)QuestNode_Root_Beggars.PopulationCountToCurve.TrueMax, (float)num);
				FloatRange floatRange = new FloatRange(Mathf.Lerp(QuestNode_Root_Beggars.LodgerCountBasedOnColonyPopulationFactorRange_Low.TrueMin, QuestNode_Root_Beggars.LodgerCountBaseOnColonyPopulationFactorRange_High.TrueMin, t), Mathf.Lerp(QuestNode_Root_Beggars.LodgerCountBasedOnColonyPopulationFactorRange_Low.TrueMax, QuestNode_Root_Beggars.LodgerCountBaseOnColonyPopulationFactorRange_High.TrueMax, t));
				randomInRange = floatRange.RandomInRange;
			}
			return Mathf.Max(Mathf.RoundToInt((float)num * randomInRange), 1);
		}

		// Token: 0x060149BD RID: 84413 RVA: 0x00626948 File Offset: 0x00624B48
		private static float GetTotalRequestValue(float points)
		{
			return Mathf.Min(points * 0.85f, 700f);
		}

		// Token: 0x060149BE RID: 84414 RVA: 0x0062695C File Offset: 0x00624B5C
		private static bool TryFindRandomRequestedThing(Map map, float value, out ThingDef thingDef, out int count, IEnumerable<ThingDef> allowedThings)
		{
			QuestNode_Root_Beggars.requestCountDict.Clear();
			Func<ThingDef, bool> globalValidator = delegate(ThingDef td)
			{
				if (!td.PlayerAcquirable)
				{
					return false;
				}
				int num = ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(value / td.BaseMarketValue)));
				QuestNode_Root_Beggars.requestCountDict.Add(td, num);
				return PlayerItemAccessibilityUtility.Accessible(td, num, map);
			};
			if ((from def in allowedThings
			where globalValidator(def)
			select def).TryRandomElement(out thingDef))
			{
				count = QuestNode_Root_Beggars.requestCountDict[thingDef];
				return true;
			}
			count = 0;
			return false;
		}

		// Token: 0x060149BF RID: 84415 RVA: 0x006269C8 File Offset: 0x00624BC8
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null, false);
			if (map == null)
			{
				return false;
			}
			if (!FactionDefOf.Beggars.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp))
			{
				return false;
			}
			float points = slate.Get<float>("points", 0f, false);
			ThingDef thingDef;
			int num;
			return QuestNode_Root_Beggars.TryFindRandomRequestedThing(map, QuestNode_Root_Beggars.GetTotalRequestValue(points), out thingDef, out num, QuestNode_Root_Beggars.AllowedThings);
		}

		// Token: 0x060149C0 RID: 84416 RVA: 0x00626A34 File Offset: 0x00624C34
		[DebugOutput]
		public static void BeggarQuestItems()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Possible items with quantities based on points using the current map:");
			for (int i = 0; i < 10000; i += 100)
			{
				stringBuilder.AppendLine(string.Format("========== points {0} ==========", i));
				foreach (ThingDef thingDef in QuestNode_Root_Beggars.AllowedThings)
				{
					ThingDef thingDef2;
					int num;
					if (QuestNode_Root_Beggars.TryFindRandomRequestedThing(Find.CurrentMap, QuestNode_Root_Beggars.GetTotalRequestValue((float)i), out thingDef2, out num, Gen.YieldSingle<ThingDef>(thingDef)))
					{
						stringBuilder.AppendLine(string.Format("<color=green>[POSSIBLE]</color> {0} x{1}", thingDef2.label, num));
					}
					else
					{
						stringBuilder.AppendLine("<color=red>[NOT POSSIBLE]</color> " + thingDef.label);
					}
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0400E4CA RID: 58570
		private static readonly FloatRange LodgerCountBasedOnColonyPopulationFactorRange_Low = new FloatRange(0.3f, 1f);

		// Token: 0x0400E4CB RID: 58571
		private static readonly FloatRange LodgerCountBaseOnColonyPopulationFactorRange_High = new FloatRange(0.25f, 0.75f);

		// Token: 0x0400E4CC RID: 58572
		private static readonly IntRange PopulationCountToCurve = new IntRange(5, 15);

		// Token: 0x0400E4CD RID: 58573
		private const int VisitDuration = 60000;

		// Token: 0x0400E4CE RID: 58574
		private const float BeggarRequestValueFactor = 0.85f;

		// Token: 0x0400E4CF RID: 58575
		private const float MaxRequestValue = 700f;

		// Token: 0x0400E4D0 RID: 58576
		private const float GiveQuestChance = 0.35f;

		// Token: 0x0400E4D1 RID: 58577
		private static Dictionary<ThingDef, int> requestCountDict = new Dictionary<ThingDef, int>();
	}
}
