using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B67 RID: 15207
	public abstract class QuestNode_Root_Mission : QuestNode
	{
		// Token: 0x17003367 RID: 13159
		// (get) Token: 0x06014A8E RID: 84622
		protected abstract string QuestTag { get; }

		// Token: 0x17003368 RID: 13160
		// (get) Token: 0x06014A8F RID: 84623 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool AddCampLootReward
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003369 RID: 13161
		// (get) Token: 0x06014A90 RID: 84624 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool IsViolent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06014A91 RID: 84625
		protected abstract Pawn GetAsker(Quest quest);

		// Token: 0x06014A92 RID: 84626 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanGetAsker()
		{
			return true;
		}

		// Token: 0x06014A93 RID: 84627
		protected abstract int GetRequiredPawnCount(int population, float threatPoints);

		// Token: 0x06014A94 RID: 84628 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool DoesPawnCountAsAvailableForFight(Pawn p)
		{
			return true;
		}

		// Token: 0x06014A95 RID: 84629
		protected abstract Site GenerateSite(Pawn asker, float threatPoints, int pawnCount, int population, PlanetTile tile);

		// Token: 0x06014A96 RID: 84630 RVA: 0x0062CE70 File Offset: 0x0062B070
		protected virtual bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 80, 85, false, null, 0.5f, true, TileFinderMode.Near, exitOnFirstTileFound, this.canBeSpace, null, null);
		}

		// Token: 0x06014A97 RID: 84631 RVA: 0x0062CE9C File Offset: 0x0062B09C
		public static bool PawnCanFight(Pawn p)
		{
			return !p.Downed && p.health.hediffSet.BleedRateTotal <= 0f && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false) && !p.IsQuestLodger() && !p.IsSlave && p.DevelopmentalStage.Adult();
		}

		// Token: 0x06014A98 RID: 84632 RVA: 0x0062CF08 File Offset: 0x0062B108
		private void ResolveParameters(Slate slate, out int requiredPawnCount, out int population, out Map colonyMap)
		{
			try
			{
				foreach (Map map in Find.Maps)
				{
					if (map.IsPlayerHome)
					{
						QuestNode_Root_Mission.tmpMaps.Add(map);
					}
				}
				colonyMap = QuestNode_Root_Mission.tmpMaps.RandomElementWithFallback(null);
				if (colonyMap == null)
				{
					population = -1;
					requiredPawnCount = -1;
				}
				else
				{
					population = (slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : colonyMap.mapPawns.FreeColonists.Count(new Predicate<Pawn>(this.DoesPawnCountAsAvailableForFight)));
					requiredPawnCount = this.GetRequiredPawnCount(population, (float)slate.Get<int>("points", 0, false));
				}
			}
			finally
			{
				QuestNode_Root_Mission.tmpMaps.Clear();
			}
		}

		// Token: 0x06014A99 RID: 84633 RVA: 0x0062CFF0 File Offset: 0x0062B1F0
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyalty("Mission"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.QuestTag);
			QuestGenUtility.RunAdjustPointsForDistantFight();
			int num = slate.Get<int>("points", 0, false);
			Pawn asker = this.GetAsker(quest);
			int num2;
			int num3;
			Map map;
			this.ResolveParameters(slate, out num2, out num3, out map);
			if (num2 <= 0 || num3 <= 0)
			{
				Log.Error(string.Format("Mission '{0}' of type '{1}' and def '{2}' has invalid required pawn count ({3}) or population ({4}). This should have been caught in TestRunInt().", new object[]
				{
					text,
					base.GetType().Name,
					QuestGen.Root.defName,
					num2,
					num3
				}));
				return;
			}
			PlanetTile tile;
			this.TryFindSiteTile(out tile, false);
			slate.Set<Pawn>("asker", asker, false);
			slate.Set<Faction>("askerFaction", asker.Faction, false);
			slate.Set<int>("requiredPawnCount", num2, false);
			slate.Set<Map>("map", map, false);
			Site site = this.GenerateSite(asker, (float)num, num2, num3, tile);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("askerFaction.BecameHostileToPlayer");
			string text2 = QuestGenUtility.QuestTagSignal(text, "AllEnemiesDefeated");
			string signalSentSatisfied = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.SentSatisfied");
			string text3 = QuestGenUtility.QuestTagSignal(text, "MapRemoved");
			string signalChosenPawn = QuestGen.GenerateNewSignal("ChosenPawnSignal", true);
			Quest quest5 = quest;
			RewardsGeneratorParams parms = new RewardsGeneratorParams
			{
				allowGoodwill = true,
				allowRoyalFavor = true,
				giverFaction = asker.Faction,
				rewardValue = QuestNode_Root_Mission.RewardValueCurve.Evaluate((float)num),
				chosenPawnSignal = signalChosenPawn
			};
			string inSignal2 = text2;
			string customLetterLabel = null;
			string customLetterText = null;
			RulePack customLetterLabelRules = null;
			RulePack customLetterTextRules = null;
			bool addCampLootReward = this.AddCampLootReward;
			Pawn asker2 = asker;
			quest5.GiveRewards(parms, inSignal2, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules, null, delegate
			{
				Quest quest4 = quest;
				LetterDef choosePawn = LetterDefOf.ChoosePawn;
				string inSignal5 = null;
				string royalFavorLabel = asker.Faction.def.royalFavorLabel;
				string text4 = "LetterTextHonorAward_BanditCamp".Translate(asker.Faction.def.royalFavorLabel);
				quest4.Letter(choosePawn, inSignal5, signalChosenPawn, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, text4, null, royalFavorLabel, null, signalSentSatisfied);
			}, null, addCampLootReward, asker2, false, false, null);
			Thing shuttle = QuestGen_Shuttle.GenerateShuttle(null, null, null, true, true, false, num2, true, true, false, true, site, map.Parent, num2, null, false, false, false, false, true);
			slate.Set<Thing>("shuttle", shuttle, false);
			QuestUtility.AddQuestTag(ref shuttle.questTags, text);
			quest.SpawnWorldObject(site, null, null);
			TransportShip transportShip = quest.GenerateTransportShip(TransportShipDefOf.Ship_Shuttle, null, shuttle, null).transportShip;
			slate.Set<TransportShip>("transportShip", transportShip, false);
			QuestUtility.AddQuestTag(ref transportShip.questTags, text);
			quest.SendTransportShipAwayOnCleanup(transportShip, true, TransportShipDropMode.None);
			Quest quest2 = quest;
			TransportShip transportShip2 = transportShip;
			MapParent parent = map.Parent;
			Pawn mapOfPawn = null;
			Faction ofEmpire = Faction.OfEmpire;
			quest2.AddShipJob_Arrive(transportShip2, parent, mapOfPawn, null, ShipJobStartMode.Instant, ofEmpire, null);
			quest.AddShipJob_WaitSendable(transportShip, site, true, false, null);
			quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
			quest.AddShipJob_WaitSendable(transportShip, map.Parent, true, true, null);
			quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
			quest.AddShipJob_FlyAway(transportShip, null, null, TransportShipDropMode.None, null);
			quest.TendPawns(null, shuttle, signalSentSatisfied);
			quest.RequiredShuttleThings(shuttle, site, QuestGenUtility.HardcodedSignalWithQuestID("transportShip.FlewAway"), true, -1);
			quest.ShuttleLeaveDelay(shuttle, 60000, null, Gen.YieldSingle<string>(signalSentSatisfied), null, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			});
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.Killed");
			quest.FactionGoodwillChange(asker.Faction, 0, inSignal3, true, true, true, HistoryEventDefOf.ShuttleDestroyed, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, true, false);
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.LeftBehind");
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal4, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, inSignal, null);
			quest.FeedPawns(null, shuttle, signalSentSatisfied);
			QuestUtility.AddQuestTag(ref site.questTags, text);
			slate.Set<Site>("site", site, false);
			quest.SignalPassActivable(delegate
			{
				quest.Message("MessageMissionGetBackToShuttle".Translate(site.Faction.Named("FACTION")), MessageTypeDefOf.PositiveEvent, false, null, new LookTargets(shuttle), null);
				quest.Notify_PlayerRaidedSomeone(null, site, null);
			}, signalSentSatisfied, text2, null, null, null, false);
			quest.SignalPassAllSequence(delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, new List<string>
			{
				signalSentSatisfied,
				text2,
				text3
			}, null);
			Quest quest3 = quest;
			Action action = delegate()
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			};
			string inSignalEnable = null;
			string inSignalDisable = text2;
			quest3.SignalPassActivable(action, inSignalEnable, text3, null, null, inSignalDisable, false);
			int num4 = (int)(this.timeLimitDays.RandomInRange * 60000f);
			slate.Set<int>("timeoutTicks", num4, false);
			quest.WorldObjectTimeout(site, num4, null, null, false, null, true);
			List<Rule> list = new List<Rule>();
			list.AddRange(GrammarUtility.RulesForWorldObject("site", site, true));
			QuestGen.AddQuestDescriptionRules(list);
		}

		// Token: 0x06014A9A RID: 84634 RVA: 0x0062D550 File Offset: 0x0062B750
		protected override bool TestRunInt(Slate slate)
		{
			QuestGenUtility.TestRunAdjustPointsForDistantFight(slate);
			if (!ModLister.CheckRoyalty("Mission"))
			{
				return false;
			}
			if (this.IsViolent && !Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			int num;
			int num2;
			Map map;
			this.ResolveParameters(slate, out num, out num2, out map);
			PlanetTile planetTile;
			return num > 0 && num2 > 0 && (this.CanGetAsker() && map != null) && this.TryFindSiteTile(out planetTile, true);
		}

		// Token: 0x0400E59C RID: 58780
		public const int MinTilesAwayFromColony = 80;

		// Token: 0x0400E59D RID: 58781
		public const int MaxTilesAwayFromColony = 85;

		// Token: 0x0400E59E RID: 58782
		private static readonly SimpleCurve RewardValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(200f, 550f),
				true
			},
			{
				new CurvePoint(400f, 1100f),
				true
			},
			{
				new CurvePoint(800f, 1600f),
				true
			},
			{
				new CurvePoint(1600f, 2600f),
				true
			},
			{
				new CurvePoint(3200f, 3600f),
				true
			},
			{
				new CurvePoint(30000f, 10000f),
				true
			}
		};

		// Token: 0x0400E59F RID: 58783
		public FloatRange timeLimitDays = new FloatRange(2f, 5f);

		// Token: 0x0400E5A0 RID: 58784
		public bool canBeSpace;

		// Token: 0x0400E5A1 RID: 58785
		private static readonly List<Map> tmpMaps = new List<Map>();
	}
}
