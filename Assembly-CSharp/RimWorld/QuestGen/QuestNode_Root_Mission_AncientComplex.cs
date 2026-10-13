using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B69 RID: 15209
	public class QuestNode_Root_Mission_AncientComplex : QuestNode_Root_AncientComplex
	{
		// Token: 0x06014AA4 RID: 84644 RVA: 0x0062D788 File Offset: 0x0062B988
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyaltyAndIdeology("Ancient Complex mission"))
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			float points = slate.Get<float>("points", 0f, false);
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("AncientComplexMission");
			Precept_Relic precept_Relic = slate.Get<Precept_Relic>("relic", null, false);
			if (precept_Relic == null)
			{
				precept_Relic = Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>().RandomElement<Precept_Relic>();
				Log.Warning("Ancient Complex quest requires relic from parent quest. None found so picking random player relic");
			}
			Faction faction;
			this.TryFindEnemyFaction(out faction);
			Pawn pawn;
			this.TryFindAsker(faction, out pawn);
			PlanetTile planetTile;
			this.TryFindSiteTile(out planetTile, false);
			Map map;
			this.TryFindColonyMapWithFreeColonists(out map, points);
			slate.Set<PlanetTile>("tile", planetTile, false);
			int population = slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : map.mapPawns.FreeColonists.Where(new Func<Pawn, bool>(QuestNode_Root_Mission.PawnCanFight)).Count<Pawn>();
			int num;
			this.TryGetRequiredColonistCount(points, population, out num);
			string text = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.SentSatisfied");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("askerFaction.BecameHostileToPlayer");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("terminals.Destroyed");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("site.Destroyed");
			string inSignal3 = QuestGen.GenerateNewSignal("TerminalHacked", true);
			string text3 = QuestGen.GenerateNewSignal("AllTerminalsHacked", true);
			string text4 = QuestGen.GenerateNewSignal("MissionSuccess", true);
			string text5 = QuestGen.GenerateNewSignal("RequiredThingsLoaded", true);
			string text6 = QuestGen.GenerateNewSignal("SendShuttleAway", true);
			string text7 = QuestGen.GenerateNewSignal("EmptyShuttle", true);
			string text8 = QuestGen.GenerateNewSignal("CheckShuttleContents", true);
			string text9 = QuestGen.GenerateNewSignal("ShipThingAdded", true);
			QuestNode_Root_Mission_AncientComplex.AddQuestRelicParts(precept_Relic, quest);
			LayoutStructureSketch layoutStructureSketch = this.QuestSetupComplex(quest, points);
			float num2 = Find.Storyteller.difficulty.allowViolentQuests ? QuestNode_Root_Mission_AncientComplex.GetThreatPoints(map.wealthWatcher.WealthTotal, points, num) : 0f;
			SitePartParams parms = new SitePartParams
			{
				ancientLayoutStructureSketch = layoutStructureSketch,
				threatPoints = num2,
				ancientComplexRewardMaker = ThingSetMakerDefOf.MapGen_AncientComplexRoomLoot_Default
			};
			SitePartDefWithParams val = new SitePartDefWithParams(SitePartDefOf.AncientComplex, parms);
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(val), planetTile, Faction.OfAncients, false, null, null);
			site.preventGravshipLanding = true;
			TimedDetectionRaids component = site.GetComponent<TimedDetectionRaids>();
			if (component != null)
			{
				component.alertRaidsArrivingIn = true;
			}
			quest.Message("[terminalHackedMessage]", null, true, null, null, inSignal3);
			if (Find.Storyteller.difficulty.allowViolentQuests && Rand.Chance(0.5f))
			{
				quest.RandomRaid(site, QuestNode_Root_Mission_AncientComplex.RandomRaidPointsFactorRange * num2, faction, text3, PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, null, null, false);
			}
			Thing shuttle = QuestGen_Shuttle.GenerateShuttle(null, null, null, true, true, false, num, true, false, false, true, site, map.Parent, num, null, false, false, false, false, true);
			slate.Set<Thing>("shuttle", shuttle, false);
			QuestUtility.AddQuestTag(ref shuttle.questTags, questTagToAdd);
			quest.Message("MessageAncientComplexBackToShuttle".Translate(precept_Relic.Label), MessageTypeDefOf.PositiveEvent, false, null, new LookTargets(shuttle), text3);
			TransportShip transportShip = quest.GenerateTransportShip(TransportShipDefOf.Ship_Shuttle, null, shuttle, null).transportShip;
			slate.Set<TransportShip>("transportShip", transportShip, false);
			QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);
			quest.SendTransportShipAwayOnCleanup(transportShip, true, TransportShipDropMode.None);
			QuestPart_PassWhileActive questPart_PassWhileActive = new QuestPart_PassWhileActive();
			questPart_PassWhileActive.inSignalEnable = slate.Get<string>("inSignal", null, false);
			questPart_PassWhileActive.inSignal = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.ThingAdded");
			questPart_PassWhileActive.outSignal = text9;
			questPart_PassWhileActive.inSignalDisable = text6;
			quest.AddPart(questPart_PassWhileActive);
			QuestPart_Filter_AllRequiredThingsLoaded questPart_Filter_AllRequiredThingsLoaded = new QuestPart_Filter_AllRequiredThingsLoaded();
			questPart_Filter_AllRequiredThingsLoaded.inSignal = text9;
			questPart_Filter_AllRequiredThingsLoaded.shuttle = shuttle;
			questPart_Filter_AllRequiredThingsLoaded.outSignal = text5;
			quest.AddPart(questPart_Filter_AllRequiredThingsLoaded);
			QuestPart_Filter_AnyOnTransporterCapableOfHacking questPart_Filter_AnyOnTransporterCapableOfHacking = new QuestPart_Filter_AnyOnTransporterCapableOfHacking();
			questPart_Filter_AnyOnTransporterCapableOfHacking.transporter = shuttle;
			questPart_Filter_AnyOnTransporterCapableOfHacking.inSignal = text5;
			questPart_Filter_AnyOnTransporterCapableOfHacking.outSignal = text6;
			questPart_Filter_AnyOnTransporterCapableOfHacking.outSignalElse = text8;
			quest.AddPart(questPart_Filter_AnyOnTransporterCapableOfHacking);
			QuestPart_Dialog.Option option = new QuestPart_Dialog.Option();
			option.text = "SendShuttleAnyway".Translate();
			option.outSignal = text6;
			QuestPart_Dialog.Option option2 = new QuestPart_Dialog.Option();
			option2.text = "ReLoadShuttle".Translate();
			option2.outSignal = text7;
			QuestPart_Dialog dialog = new QuestPart_Dialog();
			dialog.title = "[passengersIncapableOfHackingDialogLabel]";
			dialog.text = "[passengersIncapableOfHackingDialogText]";
			dialog.options.Add(option);
			dialog.options.Add(option2);
			dialog.inSignal = text8;
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				dialog.title = x;
			}, QuestGenUtility.MergeRules(null, dialog.title, "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				dialog.text = x;
			}, QuestGenUtility.MergeRules(null, dialog.text, "root"));
			quest.AddPart(dialog);
			Quest quest4 = quest;
			TransportShip transportShip2 = transportShip;
			MapParent parent = map.Parent;
			Pawn mapOfPawn = null;
			Faction ofEmpire = Faction.OfEmpire;
			quest4.AddShipJob_Arrive(transportShip2, parent, mapOfPawn, null, ShipJobStartMode.Instant, ofEmpire, null);
			quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Force, text7);
			quest.Signal(text6, delegate
			{
				quest.SpawnWorldObject(site, null, null);
				quest.TendPawns(null, shuttle, null);
				quest.AddShipJob_WaitSendable(transportShip, site, true, false, null);
				quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
				quest.AddShipJob_WaitSendable(transportShip, map.Parent, true, true, null);
				quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
				quest.AddShipJob_FlyAway(transportShip, null, null, TransportShipDropMode.None, null);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.RequiredShuttleThings(shuttle, site, QuestGenUtility.HardcodedSignalWithQuestID("transportShip.FlewAway"), true, -1);
			quest.ShuttleLeaveDelay(shuttle, 60000, null, Gen.YieldSingle<string>(text), null, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			});
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.Destroyed");
			quest.FactionGoodwillChange(pawn.Faction, 0, inSignal4, true, true, true, HistoryEventDefOf.ShuttleDestroyed, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal4, QuestPart.SignalListenMode.OngoingOnly, true, false);
			string inSignal5 = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.LeftBehind");
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal5, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, inSignal, null);
			quest.FeedPawns(null, shuttle, text);
			quest.TendPawns(null, shuttle, text);
			int num3 = (int)(QuestNode_Root_Mission_AncientComplex.TimeLimitDays.RandomInRange * 60000f);
			slate.Set<int>("timeoutTicks", num3, false);
			quest.WorldObjectTimeout(site, num3, null, null, false, null, true);
			QuestPart_Filter_Hacked questPart_Filter_Hacked = new QuestPart_Filter_Hacked();
			questPart_Filter_Hacked.inSignal = inSignal2;
			questPart_Filter_Hacked.outSignalElse = QuestGen.GenerateNewSignal("FailQuestTerminalDestroyed", true);
			quest.AddPart(questPart_Filter_Hacked);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_Filter_Hacked.outSignalElse, QuestPart.SignalListenMode.OngoingOnly, true, false);
			Quest quest2 = quest;
			Action action = delegate()
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			};
			string inSignalEnable = null;
			string inSignalDisable = text3;
			quest2.SignalPassActivable(action, inSignalEnable, text2, null, null, inSignalDisable, false);
			QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
			questPart_PassAll.inSignals.Add(text3);
			questPart_PassAll.inSignals.Add(text2);
			questPart_PassAll.outSignal = text4;
			quest.AddPart(questPart_PassAll);
			quest.End(QuestEndOutcome.Success, 0, null, text4, QuestPart.SignalListenMode.OngoingOnly, true, false);
			QuestPart_PawnKilled questPart_PawnKilled = new QuestPart_PawnKilled();
			questPart_PawnKilled.mapParent = site;
			questPart_PawnKilled.faction = Faction.OfPlayer;
			questPart_PawnKilled.outSignal = QuestGen.GenerateNewSignal("PawnKilled", true);
			quest.AddPart(questPart_PawnKilled);
			quest.SignalPassActivable(delegate
			{
				QuestPart_Filter_AnyColonistCapableOfHacking questPart_Filter_AnyColonistCapableOfHacking = new QuestPart_Filter_AnyColonistCapableOfHacking();
				questPart_Filter_AnyColonistCapableOfHacking.mapParent = site;
				questPart_Filter_AnyColonistCapableOfHacking.inSignal = slate.Get<string>("inSignal", null, false);
				questPart_Filter_AnyColonistCapableOfHacking.outSignalElse = QuestGen.GenerateNewSignal("NoHackerLetter", true);
				quest.AddPart(questPart_Filter_AnyColonistCapableOfHacking);
				Quest quest3 = quest;
				LetterDef negativeEvent = LetterDefOf.NegativeEvent;
				string outSignalElse = questPart_Filter_AnyColonistCapableOfHacking.outSignalElse;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = "LetterLabelLastHackerLost".Translate();
				quest3.Letter(negativeEvent, outSignalElse, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextLastHackerLost".Translate(), null, label, null, null);
			}, null, questPart_PawnKilled.outSignal, null, null, null, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<List<Thing>>("terminals", layoutStructureSketch.thingsToSpawn, false);
			slate.Set<int>("terminalCount", layoutStructureSketch.thingsToSpawn.Count, false);
			slate.Set<Pawn>("asker", pawn, false);
			slate.Set<Faction>("enemyFaction", faction, false);
			slate.Set<int>("colonistCount", num, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x06014AA5 RID: 84645 RVA: 0x0062E080 File Offset: 0x0062C280
		public override LayoutStructureSketch QuestSetupComplex(Quest quest, float points)
		{
			Precept_Relic orAddRelicFromQuest = QuestNode_Root_Mission_AncientComplex.GetOrAddRelicFromQuest(quest);
			LayoutStructureSketch layoutStructureSketch = this.GenerateStructureSketch(points, true);
			layoutStructureSketch.thingDiscoveredMessage = "MessageAncientTerminalDiscovered".Translate(orAddRelicFromQuest.Label);
			string item = QuestGen.GenerateNewSignal("AllTerminalsHacked", false);
			string outSignalAny = QuestGen.GenerateNewSignal("TerminalHacked", false);
			List<string> list = new List<string>();
			for (int i = 0; i < layoutStructureSketch.thingsToSpawn.Count; i++)
			{
				Thing thing = layoutStructureSketch.thingsToSpawn[i];
				string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("terminal" + i.ToString());
				QuestUtility.AddQuestTag(thing, text);
				string item2 = QuestGenUtility.HardcodedSignalWithQuestID(text + ".Hacked");
				list.Add(item2);
				thing.TryGetComp<CompHackable>().defence = (float)(Rand.Chance(0.5f) ? QuestNode_Root_Mission_AncientComplex.HackDefenceRange.min : QuestNode_Root_Mission_AncientComplex.HackDefenceRange.max);
			}
			QuestPart_PassAllActivable questPart_PassAllActivable;
			if (!quest.TryGetFirstPartOfType<QuestPart_PassAllActivable>(out questPart_PassAllActivable))
			{
				questPart_PassAllActivable = quest.AddPart<QuestPart_PassAllActivable>();
				questPart_PassAllActivable.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
				questPart_PassAllActivable.expiryInfoPartKey = "TerminalsHacked";
			}
			questPart_PassAllActivable.inSignals = list;
			questPart_PassAllActivable.outSignalsCompleted.Add(item);
			questPart_PassAllActivable.outSignalAny = outSignalAny;
			return layoutStructureSketch;
		}

		// Token: 0x06014AA6 RID: 84646 RVA: 0x0062E1C4 File Offset: 0x0062C3C4
		private static void AddQuestRelicParts(Precept_Relic relic, Quest quest)
		{
			Reward_RelicInfo item = new Reward_RelicInfo
			{
				relic = relic,
				quest = quest
			};
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(item);
			questPart_Choice.choices.Add(choice);
		}

		// Token: 0x06014AA7 RID: 84647 RVA: 0x0062E20C File Offset: 0x0062C40C
		private static Precept_Relic GetOrAddRelicFromQuest(Quest quest)
		{
			foreach (QuestPart_Choice.Choice choice in quest.GetFirstOrAddPart<QuestPart_Choice>().choices)
			{
				foreach (Reward reward in choice.rewards)
				{
					Reward_RelicInfo reward_RelicInfo = reward as Reward_RelicInfo;
					if (reward_RelicInfo != null)
					{
						return reward_RelicInfo.relic;
					}
				}
			}
			Log.Error("Failed to get relic from quest part, quest = " + quest.ToStringSafe<Quest>());
			return null;
		}

		// Token: 0x06014AA8 RID: 84648 RVA: 0x0062E2C0 File Offset: 0x0062C4C0
		private static float GetThreatPoints(float colonyWealth, float points, int pawnCount)
		{
			return QuestNode_Root_AncientComplex.ThreatPointsOverPointsCurve.Evaluate(points) * QuestNode_Root_Mission_AncientComplex.ThreatPointsFactorOverColonyWealthCurve.Evaluate(colonyWealth) * QuestNode_Root_Mission_AncientComplex.ThreatPointsFactorOverPawnCountCurve.Evaluate((float)pawnCount);
		}

		// Token: 0x06014AA9 RID: 84649 RVA: 0x0062E2E8 File Offset: 0x0062C4E8
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			Faction faction;
			if (Find.FactionManager.AllFactionsVisible.Where(new Func<Faction, bool>(this.EnemyFactionValid)).TryRandomElement(out faction))
			{
				enemyFaction = faction;
				return true;
			}
			enemyFaction = null;
			return false;
		}

		// Token: 0x06014AAA RID: 84650 RVA: 0x0062E324 File Offset: 0x0062C524
		private bool TryFindAsker(Faction enemyFaction, out Pawn pawn)
		{
			IEnumerable<Faction> source = from f in Find.FactionManager.AllFactionsVisible
			where this.AskerFactionValid(f, enemyFaction)
			select f;
			Faction faction;
			if ((from f in source
			where f.def.techLevel >= TechLevel.Industrial
			select f).TryRandomElement(out faction))
			{
				pawn = faction.leader;
				return true;
			}
			Faction faction2;
			if (source.TryRandomElement(out faction2))
			{
				pawn = faction2.leader;
				return true;
			}
			pawn = null;
			return false;
		}

		// Token: 0x06014AAB RID: 84651 RVA: 0x0062E3B4 File Offset: 0x0062C5B4
		private bool EnemyFactionValid(Faction fac)
		{
			return fac.HostileTo(Faction.OfPlayer) && Find.FactionManager.AllFactionsVisible.Any((Faction x) => this.AskerFactionValid(x, fac));
		}

		// Token: 0x06014AAC RID: 84652 RVA: 0x0062E409 File Offset: 0x0062C609
		private bool AskerFactionValid(Faction faction, Faction enemyFaction)
		{
			return !faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && faction.HostileTo(enemyFaction) && faction.leader != null;
		}

		// Token: 0x06014AAD RID: 84653 RVA: 0x0062E434 File Offset: 0x0062C634
		private bool TryFindColonyMapWithFreeColonists(out Map map, float points)
		{
			int value = (int)QuestNode_Root_Mission_AncientComplex.MinFreeColonistsOverPointsCurve.Evaluate(points);
			map = QuestGen_Get.GetMap(false, new int?(value), true);
			return map != null;
		}

		// Token: 0x06014AAE RID: 84654 RVA: 0x0062E464 File Offset: 0x0062C664
		private bool TryGetRequiredColonistCount(float points, int population, out int requiredColonistCount)
		{
			requiredColonistCount = -1;
			int num = (int)QuestNode_Root_Mission_AncientComplex.MinFreeColonistsOverPointsCurve.Evaluate(points);
			if (population <= num)
			{
				return false;
			}
			requiredColonistCount = Rand.Range(num, population);
			return true;
		}

		// Token: 0x06014AAF RID: 84655 RVA: 0x0062E494 File Offset: 0x0062C694
		protected override bool TestRunInt(Slate slate)
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			float points = slate.Get<float>("points", 0f, false);
			Faction enemyFaction;
			Pawn pawn;
			if (!this.TryFindEnemyFaction(out enemyFaction) || !this.TryFindAsker(enemyFaction, out pawn))
			{
				return false;
			}
			Map map;
			if (!this.TryFindColonyMapWithFreeColonists(out map, points))
			{
				return false;
			}
			PlanetTile planetTile;
			if (!this.TryFindSiteTile(out planetTile, true))
			{
				return false;
			}
			int population = slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : map.mapPawns.FreeColonists.Where(new Func<Pawn, bool>(QuestNode_Root_Mission.PawnCanFight)).Count<Pawn>();
			int num;
			return this.TryGetRequiredColonistCount(points, population, out num);
		}

		// Token: 0x06014AB0 RID: 84656 RVA: 0x0062E538 File Offset: 0x0062C738
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 5, 85, false, QuestNode_Root_AncientComplex.AllowedLandmarks, 0.5f, true, TileFinderMode.Furthest, exitOnFirstTileFound, false, null, null);
		}

		// Token: 0x06014AB1 RID: 84657 RVA: 0x0062E560 File Offset: 0x0062C760
		[DebugOutput("Quests", false)]
		public static void MissionAncientComplex()
		{
			List<Tuple<float, int, float>> list = new List<Tuple<float, int, float>>();
			int[] array = new int[]
			{
				1,
				5,
				10,
				20
			};
			float[] array2 = new float[]
			{
				10000f,
				100000f,
				1000000f
			};
			foreach (float item in DebugActionsUtility.PointsOptions(false))
			{
				foreach (int item2 in array)
				{
					foreach (float item3 in array2)
					{
						list.Add(new Tuple<float, int, float>(item, item2, item3));
					}
				}
			}
			IEnumerable<Tuple<float, int, float>> dataSources = list;
			TableDataGetter<Tuple<float, int, float>>[] array5 = new TableDataGetter<Tuple<float, int, float>>[6];
			array5[0] = new TableDataGetter<Tuple<float, int, float>>("points", (Tuple<float, int, float> x) => x.Item1);
			array5[1] = new TableDataGetter<Tuple<float, int, float>>("wealth", (Tuple<float, int, float> x) => x.Item3);
			array5[2] = new TableDataGetter<Tuple<float, int, float>>("wealth threat factor", (Tuple<float, int, float> x) => string.Format("x{0}", QuestNode_Root_Mission_AncientComplex.ThreatPointsFactorOverColonyWealthCurve.Evaluate(x.Item3)));
			array5[3] = new TableDataGetter<Tuple<float, int, float>>("colonists", (Tuple<float, int, float> x) => x.Item2);
			array5[4] = new TableDataGetter<Tuple<float, int, float>>("colonist threat factor", (Tuple<float, int, float> x) => string.Format("x{0}", QuestNode_Root_Mission_AncientComplex.ThreatPointsFactorOverPawnCountCurve.Evaluate((float)x.Item2)));
			array5[5] = new TableDataGetter<Tuple<float, int, float>>("threat points", (Tuple<float, int, float> x) => QuestNode_Root_Mission_AncientComplex.GetThreatPoints(x.Item3, x.Item1, x.Item2));
			DebugTables.MakeTablesDialog<Tuple<float, int, float>>(dataSources, array5);
		}

		// Token: 0x0400E5A8 RID: 58792
		private const string RootSymbol = "root";

		// Token: 0x0400E5A9 RID: 58793
		private const int MinTilesFromColony = 5;

		// Token: 0x0400E5AA RID: 58794
		private const int MaxTilesFromColony = 85;

		// Token: 0x0400E5AB RID: 58795
		private const float MinMaxHackDefenceChance = 0.5f;

		// Token: 0x0400E5AC RID: 58796
		private const float ChanceToSpawnAllTerminalsHackedRaid = 0.5f;

		// Token: 0x0400E5AD RID: 58797
		private static readonly FloatRange TimeLimitDays = new FloatRange(2f, 5f);

		// Token: 0x0400E5AE RID: 58798
		private static readonly IntRange HackDefenceRange = new IntRange(300, 1800);

		// Token: 0x0400E5AF RID: 58799
		private static readonly FloatRange RandomRaidPointsFactorRange = new FloatRange(0.45f, 0.55f);

		// Token: 0x0400E5B0 RID: 58800
		private static readonly SimpleCurve MinFreeColonistsOverPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(500f, 3f),
				true
			},
			{
				new CurvePoint(10000f, 8f),
				true
			}
		};

		// Token: 0x0400E5B1 RID: 58801
		private static readonly SimpleCurve ThreatPointsFactorOverColonyWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(10000f, 0.5f),
				true
			},
			{
				new CurvePoint(100000f, 1f),
				true
			},
			{
				new CurvePoint(1000000f, 1.5f),
				true
			}
		};

		// Token: 0x0400E5B2 RID: 58802
		private static readonly SimpleCurve ThreatPointsFactorOverPawnCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.5f),
				true
			},
			{
				new CurvePoint(2f, 0.55f),
				true
			},
			{
				new CurvePoint(5f, 0.75f),
				true
			},
			{
				new CurvePoint(20f, 5f),
				true
			}
		};
	}
}
