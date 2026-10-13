using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B93 RID: 15251
	public class QuestNode_Root_ShuttleCrash_Rescue : QuestNode
	{
		// Token: 0x1700336F RID: 13167
		// (get) Token: 0x06014B6B RID: 84843 RVA: 0x00633160 File Offset: 0x00631360
		private static QuestGen_Pawns.GetPawnParms CivilianPawnParams
		{
			get
			{
				return new QuestGen_Pawns.GetPawnParms
				{
					mustBeOfFaction = Faction.OfEmpire,
					canGeneratePawn = true,
					mustBeWorldPawn = true,
					mustBeOfKind = PawnKindDefOf.Empire_Common_Lodger
				};
			}
		}

		// Token: 0x06014B6C RID: 84844 RVA: 0x006331A0 File Offset: 0x006313A0
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyalty("Shuttle crash rescue"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float questPoints = slate.Get<float>("points", 0f, false);
			slate.Set<Map>("map", map, false);
			slate.Set<int>("rescueDelay", 20000, false);
			slate.Set<int>("leaveDelay", 30000, false);
			slate.Set<int>("rescueShuttleAfterRaidDelay", 10000, false);
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("civilian");
			int maxExclusive = Mathf.FloorToInt(QuestNode_Root_ShuttleCrash_Rescue.MaxCiviliansByPointsCurve.Evaluate(questPoints));
			int num = Rand.Range(1, maxExclusive);
			int maxExclusive2 = Mathf.FloorToInt(QuestNode_Root_ShuttleCrash_Rescue.MaxSoldiersByPointsCurve.Evaluate(questPoints));
			int num2 = Rand.Range(1, maxExclusive2);
			Faction enemyFaction;
			this.TryFindEnemyFaction(out enemyFaction);
			Thing crashedShuttle = ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed, null);
			IntVec3 shuttleCrashPosition;
			this.TryFindShuttleCrashPosition(map, Faction.OfEmpire, crashedShuttle.def.size, new IntVec3?(crashedShuttle.def.interactionCellOffset), out shuttleCrashPosition);
			List<Pawn> civilians = new List<Pawn>();
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < num - 1; i++)
			{
				Pawn pawn = quest.GetPawn(QuestNode_Root_ShuttleCrash_Rescue.CivilianPawnParams);
				QuestUtility.AddQuestTag(ref pawn.questTags, questTagToAdd);
				civilians.Add(pawn);
				list.Add(pawn);
			}
			Pawn asker = quest.GetPawn(new QuestGen_Pawns.GetPawnParms
			{
				mustBeOfFaction = Faction.OfEmpire,
				canGeneratePawn = true,
				mustBeWorldPawn = true,
				seniorityRange = new FloatRange(100f, QuestNode_Root_ShuttleCrash_Rescue.MaxAskerSeniorityByPointsCurve.Evaluate(questPoints)),
				mustHaveRoyalTitleInCurrentFaction = true
			});
			civilians.Add(asker);
			PawnKindDef mustBeOfKind = new PawnKindDef[]
			{
				PawnKindDefOf.Empire_Fighter_Trooper,
				PawnKindDefOf.Empire_Fighter_Janissary,
				PawnKindDefOf.Empire_Fighter_Cataphract
			}.RandomElement<PawnKindDef>();
			List<Pawn> soldiers = new List<Pawn>();
			for (int j = 0; j < num2; j++)
			{
				Pawn pawn2 = quest.GetPawn(new QuestGen_Pawns.GetPawnParms
				{
					mustBeOfFaction = Faction.OfEmpire,
					canGeneratePawn = true,
					mustBeOfKind = mustBeOfKind,
					mustBeWorldPawn = true,
					mustBeCapableOfViolence = true
				});
				soldiers.Add(pawn2);
			}
			List<Pawn> allPassengers = new List<Pawn>();
			allPassengers.AddRange(soldiers);
			allPassengers.AddRange(civilians);
			quest.BiocodeWeapons(allPassengers, null);
			Thing rescueShuttle = QuestGen_Shuttle.GenerateShuttle(Faction.OfEmpire, allPassengers, null, false, false, false, -1, false, false, false, false, null, null, -1, null, false, true, false, false, false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("soldiers.Rescued");
			quest.RemoveFromRequiredPawnsOnRescue(rescueShuttle, soldiers, inSignal);
			Action <>9__4;
			quest.Delay(120, delegate
			{
				Quest quest = quest;
				LetterDef negativeEvent = LetterDefOf.NegativeEvent;
				string inSignal7 = null;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
				string label = "LetterLabelShuttleCrashed".Translate();
				string text4 = "LetterTextShuttleCrashed".Translate();
				quest.Letter(negativeEvent, inSignal7, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, Gen.YieldSingle<Thing>(crashedShuttle), false, text4, null, label, null, null);
				quest.SpawnSkyfaller(map, ThingDefOf.ShuttleCrashing, Gen.YieldSingle<Thing>(crashedShuttle), Faction.OfPlayer, new IntVec3?(shuttleCrashPosition), null, false, false, null, null);
				Quest quest2 = quest;
				MapParent parent = map.Parent;
				IEnumerable<Thing> allPassengers = allPassengers;
				string customLetterLabel = null;
				RulePack customLetterLabelRules = null;
				string customLetterText = null;
				RulePack customLetterTextRules = null;
				IntVec3? dropSpot = new IntVec3?(shuttleCrashPosition);
				quest2.DropPods(parent, allPassengers, customLetterLabel, customLetterLabelRules, customLetterText, customLetterTextRules, new bool?(false), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, dropSpot, true, false, false, null);
				quest.DefendPoint(map.Parent, asker, shuttleCrashPosition, soldiers, Faction.OfEmpire, null, null, new float?((float)12), false, false);
				IntVec3 position = shuttleCrashPosition + IntVec3.South;
				IEnumerable<Pawn> civilians;
				quest.WaitForEscort(map.Parent, civilians, Faction.OfEmpire, position, null, false);
				string inSignal8 = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.Spawned");
				quest.ExitOnShuttle(map.Parent, allPassengers, Faction.OfEmpire, rescueShuttle, inSignal8, false);
				Quest quest3 = quest;
				int delayTicks = 20000;
				civilians = civilians;
				Action complete;
				if ((complete = <>9__4) == null)
				{
					complete = (<>9__4 = delegate()
					{
						quest.Letter(LetterDefOf.NeutralEvent, null, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Thing>(rescueShuttle), false, "[rescueShuttleArrivedLetterText]", null, "[rescueShuttleArrivedLetterLabel]", null, null);
						TransportShip transportShip = quest.GenerateTransportShip(TransportShipDefOf.Ship_Shuttle, null, rescueShuttle, null).transportShip;
						quest.SendTransportShipAwayOnCleanup(transportShip, false, TransportShipDropMode.NonRequired);
						IntVec3 value;
						DropCellFinder.TryFindDropSpotNear(shuttleCrashPosition, map, out value, false, false, false, new IntVec2?(ThingDefOf.Shuttle.Size + new IntVec2(2, 2)), false);
						quest.AddShipJob_Arrive(transportShip, map.Parent, null, new IntVec3?(value), ShipJobStartMode.Instant, Faction.OfEmpire, null);
						quest.AddShipJob_WaitTime(transportShip, 30000, true, allPassengers.Cast<Thing>().ToList<Thing>(), null);
						quest.ShuttleLeaveDelay(rescueShuttle, 30000, null, null, null, null);
						quest.AddShipJob_FlyAway(transportShip, null, null, TransportShipDropMode.None, null);
					});
				}
				quest3.ShuttleDelay(delayTicks, civilians, complete, null, null, true);
				IntVec3 walkIntSpot;
				this.TryFindRaidWalkInPosition(map, shuttleCrashPosition, out walkIntSpot);
				float soldiersTotalCombatPower = 0f;
				for (int k = 0; k < soldiers.Count; k++)
				{
					soldiersTotalCombatPower += soldiers[k].kindDef.combatPower;
				}
				quest.Delay(10000, delegate
				{
					List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
					{
						faction = enemyFaction,
						groupKind = PawnGroupKindDefOf.Combat,
						points = (questPoints + soldiersTotalCombatPower) * 0.37f,
						tile = map.Tile
					}, true).ToList<Pawn>();
					for (int l = 0; l < list2.Count; l++)
					{
						Find.WorldPawns.PassToWorld(list2[l], PawnDiscardDecideMode.Decide);
						QuestGen.AddToGeneratedPawns(list2[l]);
					}
					QuestPart_PawnsArrive questPart_PawnsArrive = new QuestPart_PawnsArrive();
					questPart_PawnsArrive.pawns.AddRange(list2);
					questPart_PawnsArrive.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
					questPart_PawnsArrive.joinPlayer = false;
					questPart_PawnsArrive.mapParent = map.Parent;
					questPart_PawnsArrive.spawnNear = walkIntSpot;
					questPart_PawnsArrive.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
					questPart_PawnsArrive.sendStandardLetter = false;
					quest.AddPart(questPart_PawnsArrive);
					quest.AssaultThings(map.Parent, list2, enemyFaction, allPassengers, null, null, true);
					quest.Letter(LetterDefOf.ThreatBig, null, null, enemyFaction, null, false, QuestPart.SignalListenMode.OngoingOnly, list2, false, "[raidArrivedLetterText]", null, "[raidArrivedLetterLabel]", null, null);
				}, null, null, null, false, null, null, false, null, null, "RaidDelay", false, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			string text = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.SentSatisfied");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.SentUnsatisfied");
			string[] inSignalsShuttleSent = new string[]
			{
				text,
				text2
			};
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.Destroyed");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.LeftBehind");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("asker.Destroyed");
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("civilian.Destroyed");
			string inSignal5 = QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved");
			quest.GoodwillChangeShuttleSentThings(Faction.OfEmpire, list, -5, null, inSignalsShuttleSent, text3, HistoryEventDefOf.ShuttleGuardsMissedShuttle, true, false, QuestPart.SignalListenMode.Always);
			quest.GoodwillChangeShuttleSentThings(Faction.OfEmpire, Gen.YieldSingle<Pawn>(asker), -10, null, inSignalsShuttleSent, text3, HistoryEventDefOf.ShuttleCommanderMissedShuttle, true, false, QuestPart.SignalListenMode.Always);
			quest.Leave(allPassengers, "", false, true, null, false);
			quest.Letter(LetterDefOf.PositiveEvent, text, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[questCompletedSuccessLetterText]", null, "[questCompletedSuccessLetterLabel]", null, null);
			string questSuccess = QuestGen.GenerateNewSignal("QuestSuccess", true);
			quest.SignalPass(delegate
			{
				RewardsGeneratorParams rewardsGeneratorParams = new RewardsGeneratorParams
				{
					rewardValue = questPoints,
					allowGoodwill = true,
					allowRoyalFavor = true,
					allowDevelopmentPoints = true
				};
				Quest quest = quest;
				RewardsGeneratorParams parms = rewardsGeneratorParams;
				string inSignal7 = null;
				string customLetterLabel = null;
				string customLetterText = null;
				RulePack customLetterLabelRules = null;
				RulePack customLetterTextRules = null;
				Pawn asker = asker;
				quest.GiveRewards(parms, inSignal7, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules, null, null, null, false, asker, true, false, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, questSuccess, null);
			quest.SignalPass(null, text, questSuccess);
			Action <>9__6;
			Action <>9__7;
			quest.AnyOnTransporter(allPassengers, rescueShuttle, delegate
			{
				Quest quest = quest;
				IEnumerable<Pawn> pawns = Gen.YieldSingle<Pawn>(asker);
				Thing rescueShuttle = rescueShuttle;
				Action action;
				if ((action = <>9__6) == null)
				{
					action = (<>9__6 = delegate()
					{
						quest.Letter(LetterDefOf.PositiveEvent, null, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[questCompletedCiviliansLostSuccessLetterText]", null, "[questCompletedCiviliansLostSuccessLetterLabel]", null, null);
						quest.SignalPass(null, null, questSuccess);
					});
				}
				Action elseAction;
				if ((elseAction = <>9__7) == null)
				{
					elseAction = (<>9__7 = delegate()
					{
						quest.Letter(LetterDefOf.NegativeEvent, null, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[askerLostLetterText]", null, "[askerLostLetterLabel]", null, null);
						quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					});
				}
				quest.AnyOnTransporter(pawns, rescueShuttle, action, elseAction, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, delegate
			{
				quest.Letter(LetterDefOf.NegativeEvent, null, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[allLostLetterText]", null, "[allLostLetterLabel]", null, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, text2, null, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Letter(LetterDefOf.NegativeEvent, inSignal3, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Pawn>(asker), false, "[askerDiedLetterText]", null, "[askerDiedLetterLabel]", null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.Letter(LetterDefOf.NegativeEvent, inSignal4, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, civilians, false, "[civilianDiedLetterText]", null, "[civilianDiedLetterLabel]", null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal4, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.Letter(LetterDefOf.NegativeEvent, text3, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Thing>(rescueShuttle), false, "[shuttleDestroyedLetterText]", null, "[shuttleDestroyedLetterLabel]", null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.Letter(LetterDefOf.NegativeEvent, inSignal2, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Thing>(rescueShuttle), false, "[shuttleLeftBehindLetterText]", null, "[shuttleLeftBehindLetterLabel]", null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("asker.LeftMap"), QuestPart.SignalListenMode.OngoingOnly, true, false);
			string inSignal6 = QuestGenUtility.HardcodedSignalWithQuestID("askerFaction.BecameHostileToPlayer");
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal6, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.InvalidPreAcceptance, 0, null, inSignal6, QuestPart.SignalListenMode.NotYetAcceptedOnly, false, false);
			quest.Letter(LetterDefOf.NegativeEvent, inSignal5, null, Faction.OfEmpire, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Pawn>(asker), false, "[mapRemovedLetterText]", null, "[mapRemovedLetterLabel]", null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal5, QuestPart.SignalListenMode.OngoingOnly, false, false);
			slate.Set<Pawn>("asker", asker, false);
			slate.Set<Faction>("askerFaction", asker.Faction, false);
			slate.Set<Faction>("enemyFaction", enemyFaction, false);
			slate.Set<List<Pawn>>("soldiers", soldiers, false);
			slate.Set<List<Pawn>>("civilians", civilians, false);
			slate.Set<int>("civilianCountMinusOne", civilians.Count - 1, false);
			slate.Set<Thing>("rescueShuttle", rescueShuttle, false);
		}

		// Token: 0x06014B6D RID: 84845 RVA: 0x006338B5 File Offset: 0x00631AB5
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			return (from f in Find.FactionManager.AllFactionsVisible
			where f.HostileTo(Faction.OfEmpire) && f.HostileTo(Faction.OfPlayer)
			select f).TryRandomElement(out enemyFaction);
		}

		// Token: 0x06014B6E RID: 84846 RVA: 0x006338EB File Offset: 0x00631AEB
		private bool TryFindShuttleCrashPosition(Map map, Faction faction, IntVec2 size, IntVec3? interactionCell, out IntVec3 spot)
		{
			return DropCellFinder.FindSafeLandingSpot(out spot, faction, map, 35, 15, 25, new IntVec2?(size), interactionCell);
		}

		// Token: 0x06014B6F RID: 84847 RVA: 0x0063390C File Offset: 0x00631B0C
		private bool TryFindRaidWalkInPosition(Map map, IntVec3 shuttleCrashSpot, out IntVec3 spawnSpot)
		{
			Predicate<IntVec3> predicate = (IntVec3 p) => (map.TileInfo.AllowRoofedEdgeWalkIn || !map.roofGrid.Roofed(p)) && p.Walkable(map) && map.reachability.CanReach(p, shuttleCrashSpot, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some);
			if (RCellFinder.TryFindEdgeCellFromPositionAvoidingColony(shuttleCrashSpot, map, predicate, out spawnSpot))
			{
				return true;
			}
			if (CellFinder.TryFindRandomEdgeCellWith(predicate, map, CellFinder.EdgeRoadChance_Hostile, out spawnSpot))
			{
				return true;
			}
			spawnSpot = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06014B70 RID: 84848 RVA: 0x00633974 File Offset: 0x00631B74
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			if (Faction.OfEmpire == null)
			{
				return false;
			}
			Pawn pawn;
			if (!QuestGen_Pawns.GetPawnTest(QuestNode_Root_ShuttleCrash_Rescue.CivilianPawnParams, out pawn))
			{
				return false;
			}
			if (Faction.OfEmpire.PlayerRelationKind == FactionRelationKind.Hostile)
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindEnemyFaction(out faction))
			{
				return false;
			}
			Map map = QuestGen_Get.GetMap(false, null, false);
			IntVec3 shuttleCrashSpot;
			IntVec3 intVec;
			return map != null && this.TryFindShuttleCrashPosition(map, Faction.OfEmpire, ThingDefOf.ShuttleCrashed.size, new IntVec3?(ThingDefOf.ShuttleCrashed.interactionCellOffset), out shuttleCrashSpot) && this.TryFindRaidWalkInPosition(map, shuttleCrashSpot, out intVec);
		}

		// Token: 0x0400E655 RID: 58965
		private const int WanderRadius_Soldiers = 12;

		// Token: 0x0400E656 RID: 58966
		private const int QuestStartDelay = 120;

		// Token: 0x0400E657 RID: 58967
		private const int RescueShuttle_Delay = 20000;

		// Token: 0x0400E658 RID: 58968
		private const int RescueShuttle_LeaveDelay = 30000;

		// Token: 0x0400E659 RID: 58969
		private const int RaidDelay = 10000;

		// Token: 0x0400E65A RID: 58970
		private const float MinRaidDistance_Colony = 15f;

		// Token: 0x0400E65B RID: 58971
		private const float MinRaidDistance_ShuttleCrash = 15f;

		// Token: 0x0400E65C RID: 58972
		private const int FactionGoodwillChange_AskerLost = -10;

		// Token: 0x0400E65D RID: 58973
		private const int FactionGoodwillChange_CivilianLost = -5;

		// Token: 0x0400E65E RID: 58974
		private const float ThreatPointsFactor = 0.37f;

		// Token: 0x0400E65F RID: 58975
		private const float MinAskerSeniority = 100f;

		// Token: 0x0400E660 RID: 58976
		private static readonly SimpleCurve MaxCiviliansByPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 2f),
				true
			},
			{
				new CurvePoint(500f, 4f),
				true
			}
		};

		// Token: 0x0400E661 RID: 58977
		private static readonly SimpleCurve MaxSoldiersByPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 3f),
				true
			},
			{
				new CurvePoint(500f, 6f),
				true
			}
		};

		// Token: 0x0400E662 RID: 58978
		private static readonly SimpleCurve MaxAskerSeniorityByPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 100f),
				true
			},
			{
				new CurvePoint(1500f, 850f),
				true
			}
		};
	}
}
