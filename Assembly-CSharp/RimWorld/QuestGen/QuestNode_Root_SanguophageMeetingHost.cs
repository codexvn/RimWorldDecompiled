using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B8C RID: 15244
	public class QuestNode_Root_SanguophageMeetingHost : QuestNode
	{
		// Token: 0x06014B45 RID: 84805 RVA: 0x00631610 File Offset: 0x0062F810
		protected override void RunInt()
		{
			QuestNode_Root_SanguophageMeetingHost.<>c__DisplayClass13_0 CS$<>8__locals1 = new QuestNode_Root_SanguophageMeetingHost.<>c__DisplayClass13_0();
			if (!ModLister.CheckBiotech("Sanguophage meeting host"))
			{
				return;
			}
			CS$<>8__locals1.quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			QuestNode_Root_SanguophageMeetingHost.<>c__DisplayClass13_0 CS$<>8__locals2 = CS$<>8__locals1;
			bool mustBeInfestable = false;
			int? num = null;
			CS$<>8__locals2.map = QuestGen_Get.GetMap(mustBeInfestable, num, false);
			float num2 = slate.Get<float>("points", 0f, false);
			CS$<>8__locals1.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("SanguophageMeeting");
			slate.Set<Map>("map", CS$<>8__locals1.map, false);
			if (!this.TryFindGatheringSpot(CS$<>8__locals1.map, out CS$<>8__locals1.gatherSpot))
			{
				return;
			}
			slate.Set<IntVec3>("gatherSpot", CS$<>8__locals1.gatherSpot, false);
			CS$<>8__locals1.meetingStartedSignal = QuestGen.GenerateNewSignal("MeetingStarted", true);
			CS$<>8__locals1.meetingCompletedSignal = QuestGen.GenerateNewSignal("MeetingCompleted", true);
			CS$<>8__locals1.raidArrivedSignal = QuestGen.GenerateNewSignal("RaidArrived", true);
			CS$<>8__locals1.beginDefendSignal = QuestGen.GenerateNewSignal("BeginDefend", true);
			string text = QuestGen.GenerateNewSignal("SanguophagesBecameHostile", true);
			CS$<>8__locals1.questFailedSignal = QuestGen.GenerateNewSignal("QuestFailed", true);
			string text2 = QuestGen.GenerateNewSignal("QuestSucceeded", true);
			CS$<>8__locals1.allSanguophagesGone = QuestGen.GenerateNewSignal("AllSanguophagesGone", true);
			string text3 = QuestGenUtility.QuestTagSignal(CS$<>8__locals1.questTag, "BeingAttacked");
			string text4 = QuestGenUtility.HardcodedSignalWithQuestID("sanguophages.Arrested");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("sanguophages.SwappedMap");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved");
			string text5 = QuestGenUtility.HardcodedSignalWithQuestID("sanguophages.SurgeryViolation");
			string text6 = QuestGenUtility.HardcodedSignalWithQuestID("sanguophages.XenogermAbsorbed");
			string text7 = QuestGenUtility.HardcodedSignalWithQuestID("sanguophages.XenogermReimplanted");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("enemies.Despawned");
			string inSignalRemovePawn = QuestGenUtility.HardcodedSignalWithQuestID("enemies.Arrested");
			slate.Set<string>("xenogermReimplantedSignal", text7, false);
			CS$<>8__locals1.meetingDurationTicks = QuestNode_Root_SanguophageMeetingHost.MeetingDurationRange.RandomInRange;
			slate.Set<int>("meetingDurationTicks", CS$<>8__locals1.meetingDurationTicks, false);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction.def.PermanentlyHostileTo(FactionDefOf.Sanguophages))
				{
					list.Add(new FactionRelation(faction, FactionRelationKind.Neutral));
				}
			}
			CS$<>8__locals1.faction = FactionGenerator.NewGeneratedFactionWithRelations(FactionDefOf.Sanguophages, list, true);
			CS$<>8__locals1.faction.temporary = true;
			Find.FactionManager.Add(CS$<>8__locals1.faction);
			slate.Set<Faction>("faction", CS$<>8__locals1.faction, false);
			CS$<>8__locals1.quest.ReserveFaction(CS$<>8__locals1.faction);
			CS$<>8__locals1.sanguophages = new List<Pawn>();
			PawnKindDef sanguophage = PawnKindDefOf.Sanguophage;
			Faction faction2 = CS$<>8__locals1.faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			PlanetTile? tile = null;
			bool forceGenerateNewPawn = true;
			bool allowDead = false;
			bool allowDowned = false;
			bool canGeneratePawnRelations = true;
			bool mustBeCapableOfViolence = true;
			float colonistRelationChanceFactor = 1f;
			bool forceAddFreeWarmLayerIfNeeded = false;
			bool allowGay = true;
			bool allowPregnant = false;
			bool allowFood = true;
			bool allowAddictions = true;
			bool inhabitant = false;
			bool certainlyBeenInCryptosleep = false;
			bool forceRedressWorldPawnIfFormerColonist = false;
			bool worldPawnFactionDoesntMatter = false;
			float biocodeWeaponChance = 0f;
			float biocodeApparelChance = 0f;
			Pawn extraPawnForExtraRelationChance = null;
			float relationWithExtraPawnChanceFactor = 1f;
			Predicate<Pawn> validatorPreGear = null;
			Predicate<Pawn> validatorPostGear = null;
			IEnumerable<TraitDef> forcedTraits = null;
			IEnumerable<TraitDef> prohibitedTraits = null;
			float? minChanceToRedressWorldPawn = null;
			float? fixedBiologicalAge = null;
			float? num3 = null;
			PawnGenerationRequest request = new PawnGenerationRequest(sanguophage, faction2, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, minChanceToRedressWorldPawn, fixedBiologicalAge, num3, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			int num4 = CountChanceUtility.RandomCount(QuestNode_Root_SanguophageMeetingHost.SanguophageCountChances);
			for (int i = 0; i < num4; i++)
			{
				Pawn item = CS$<>8__locals1.quest.GeneratePawn(request, false);
				CS$<>8__locals1.sanguophages.Add(item);
			}
			slate.Set<List<Pawn>>("sanguophages", CS$<>8__locals1.sanguophages, false);
			slate.Set<int>("sanguophageCount", num4, false);
			slate.Set<int>("sanguophageCountMinusOne", num4 - 1, false);
			Pawn pawn = CS$<>8__locals1.sanguophages.First<Pawn>();
			slate.Set<Pawn>("asker", pawn, false);
			CS$<>8__locals1.enemyFaction = null;
			CS$<>8__locals1.enemies = null;
			if (Find.Storyteller.difficulty.allowViolentQuests)
			{
				if ((from x in Find.FactionManager.AllFactionsVisible
				where x.def.permanentEnemy
				select x).TryRandomElement(out CS$<>8__locals1.enemyFaction))
				{
					slate.Set<Faction>("enemyFaction", CS$<>8__locals1.enemyFaction, false);
					PawnGroupMakerParms parms = new PawnGroupMakerParms
					{
						faction = CS$<>8__locals1.enemyFaction,
						groupKind = PawnGroupKindDefOf.Combat,
						points = IncidentWorker_Raid.AdjustedRaidPoints(num2 + 200f * (float)num4, PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, CS$<>8__locals1.enemyFaction, PawnGroupKindDefOf.Combat, CS$<>8__locals1.map, null),
						tile = CS$<>8__locals1.map.Tile
					};
					CS$<>8__locals1.enemies = PawnGroupMakerUtility.GeneratePawns(parms, true).ToList<Pawn>();
					if (CS$<>8__locals1.enemies.Any<Pawn>())
					{
						for (int j = 0; j < CS$<>8__locals1.enemies.Count; j++)
						{
							Find.WorldPawns.PassToWorld(CS$<>8__locals1.enemies[j], PawnDiscardDecideMode.Decide);
							QuestGen.AddToGeneratedPawns(CS$<>8__locals1.enemies[j]);
						}
						slate.Set<List<Pawn>>("enemies", CS$<>8__locals1.enemies, false);
						QuestPart_PawnsArrive questPart_PawnsArrive = new QuestPart_PawnsArrive();
						questPart_PawnsArrive.pawns.AddRange(CS$<>8__locals1.enemies);
						questPart_PawnsArrive.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
						questPart_PawnsArrive.mapParent = CS$<>8__locals1.map.Parent;
						questPart_PawnsArrive.inSignal = CS$<>8__locals1.raidArrivedSignal;
						questPart_PawnsArrive.sendStandardLetter = false;
						questPart_PawnsArrive.addPawnsToLookTargets = false;
						CS$<>8__locals1.quest.AddPart(questPart_PawnsArrive);
						CS$<>8__locals1.quest.AssaultThings(CS$<>8__locals1.map.Parent, CS$<>8__locals1.enemies, CS$<>8__locals1.enemyFaction, CS$<>8__locals1.sanguophages, CS$<>8__locals1.raidArrivedSignal, null, true);
						CS$<>8__locals1.quest.Letter(LetterDefOf.ThreatBig, CS$<>8__locals1.raidArrivedSignal, null, CS$<>8__locals1.enemyFaction, null, false, QuestPart.SignalListenMode.OngoingOnly, CS$<>8__locals1.enemies, false, "[raidArrivedLetterText]", null, "[raidArrivedLetterLabel]", null, null);
						CS$<>8__locals1.quest.AllPawnsDespawned(CS$<>8__locals1.enemies, delegate
						{
							CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals1.meetingCompletedSignal);
						}, null, inSignal3, null, null, inSignalRemovePawn, QuestPart.SignalListenMode.OngoingOnly);
					}
				}
			}
			CS$<>8__locals1.quest.Message("SanguophagesArrivingSoon".Translate(), MessageTypeDefOf.NeutralEvent, false, null, null, null).historical = false;
			int randomInRange = QuestNode_Root_SanguophageMeetingHost.ArrivalDelayRange.RandomInRange;
			CS$<>8__locals1.quest.Delay(randomInRange, delegate
			{
				CS$<>8__locals1.quest.PawnsArrive(CS$<>8__locals1.sanguophages, null, CS$<>8__locals1.map.Parent, PawnsArrivalModeDefOf.EdgeWalkInGroups, false, null, "[sanguophagesArriveLetterLabel]", "[sanguophagesArriveLetterText]", null, null, false, false, true);
				QuestPart_SagnuophageMeeting questPart_SagnuophageMeeting = new QuestPart_SagnuophageMeeting
				{
					inSignal = QuestGen.slate.Get<string>("inSignal", null, false),
					questTag = CS$<>8__locals1.questTag,
					faction = CS$<>8__locals1.faction,
					targetCell = CS$<>8__locals1.gatherSpot,
					meetingDurationTicks = CS$<>8__locals1.meetingDurationTicks,
					mapParent = CS$<>8__locals1.map.Parent,
					inSignalDefend = CS$<>8__locals1.beginDefendSignal,
					inSignalQuestSuccess = CS$<>8__locals1.meetingCompletedSignal,
					inSignalQuestFail = CS$<>8__locals1.questFailedSignal,
					outSignalMeetingStarted = CS$<>8__locals1.meetingStartedSignal,
					outSignalMeetingCompleted = CS$<>8__locals1.meetingCompletedSignal,
					outSignalAllSanguophagesGone = CS$<>8__locals1.allSanguophagesGone
				};
				questPart_SagnuophageMeeting.pawns.AddRange(CS$<>8__locals1.sanguophages);
				if (CS$<>8__locals1.enemies != null)
				{
					questPart_SagnuophageMeeting.enemyTargets.AddRange(CS$<>8__locals1.enemies);
				}
				CS$<>8__locals1.quest.AddPart(questPart_SagnuophageMeeting);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Arrival delay";
			int randomInRange2 = QuestNode_Root_SanguophageMeetingHost.EventDelayRange.RandomInRange;
			CS$<>8__locals1.quest.Delay(randomInRange2, delegate
			{
				List<Action> list2 = new List<Action>();
				list2.Add(delegate
				{
				});
				Action item2;
				if ((item2 = CS$<>8__locals1.<>9__7) == null)
				{
					item2 = (CS$<>8__locals1.<>9__7 = delegate()
					{
						Pawn joiner = CS$<>8__locals1.sanguophages.FirstOrDefault((Pawn x) => !x.Dead && !x.Downed);
						if (joiner != null)
						{
							CS$<>8__locals1.quest.WaitForDuration(CS$<>8__locals1.map.Parent, CS$<>8__locals1.sanguophages, CS$<>8__locals1.faction, CS$<>8__locals1.gatherSpot, 7500, null);
							Quest quest2 = CS$<>8__locals1.quest;
							Pawn joiner2 = joiner;
							string letterLabel = "LetterJoinOfferLabel".Translate(joiner.Named("PAWN"));
							string letterTitle = "LetterJoinOfferTitle".Translate(joiner.Named("PAWN"));
							string letterText = "LetterJoinOfferText".Translate(joiner.Named("PAWN"), CS$<>8__locals1.map.Parent.Named("MAP")).Resolve();
							Action accepted = delegate()
							{
								CS$<>8__locals1.quest.JoinPlayer(CS$<>8__locals1.map.Parent, Gen.YieldSingle<Pawn>(joiner), true, false, null);
								Quest quest4 = CS$<>8__locals1.quest;
								LetterDef positiveEvent = LetterDefOf.PositiveEvent;
								string inSignal4 = null;
								string chosenPawnSignal = null;
								Faction relatedFaction = null;
								MapParent useColonistsOnMap = null;
								bool useColonistsFromCaravanArg = false;
								QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
								IEnumerable<object> lookTargets = null;
								bool filterDeadPawnsFromLookTargets = false;
								string label = "LetterLabelMessageRecruitSuccess".Translate() + ": " + joiner.LabelShortCap;
								quest4.Letter(positiveEvent, inSignal4, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "MessageRecruitJoinOfferAccepted".Translate(joiner.Named("RECRUITEE")).Resolve(), null, label, null, null);
								CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals1.meetingCompletedSignal);
							};
							Action rejected;
							if ((rejected = CS$<>8__locals1.<>9__10) == null)
							{
								rejected = (CS$<>8__locals1.<>9__10 = delegate()
								{
									CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals1.meetingCompletedSignal);
								});
							}
							quest2.PawnJoinOffer(joiner2, letterLabel, letterTitle, letterText, accepted, rejected, null, null, null, false, true);
							Quest quest3 = CS$<>8__locals1.quest;
							int delayTicks = 7500;
							Action inner;
							if ((inner = CS$<>8__locals1.<>9__11) == null)
							{
								inner = (CS$<>8__locals1.<>9__11 = delegate()
								{
									CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals1.meetingCompletedSignal);
								});
							}
							quest3.Delay(delayTicks, inner, null, CS$<>8__locals1.meetingCompletedSignal, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
						}
					});
				}
				list2.Add(item2);
				List<Action> list3 = list2;
				if (CS$<>8__locals1.enemyFaction != null)
				{
					List<Action> list4 = list3;
					Action item3;
					if ((item3 = CS$<>8__locals1.<>9__5) == null)
					{
						item3 = (CS$<>8__locals1.<>9__5 = delegate()
						{
							CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals1.raidArrivedSignal);
							Quest quest2 = CS$<>8__locals1.quest;
							int delayTicks = 1250;
							Action inner;
							if ((inner = CS$<>8__locals1.<>9__12) == null)
							{
								inner = (CS$<>8__locals1.<>9__12 = delegate()
								{
									CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals1.beginDefendSignal);
								});
							}
							quest2.Delay(delayTicks, inner, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Defend delay";
						});
					}
					list4.Add(item3);
				}
				CS$<>8__locals1.quest.SignalRandom(list3, null, QuestPart.SignalListenMode.OngoingOnly);
			}, CS$<>8__locals1.meetingStartedSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Event delay";
			bool flag = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Any((Pawn x) => x.genes != null && x.genes.Xenotype == XenotypeDefOf.Sanguophage);
			Quest quest = CS$<>8__locals1.quest;
			RewardsGeneratorParams parms2 = new RewardsGeneratorParams
			{
				rewardValue = QuestNode_Root_SanguophageMeetingHost.RewardValueCurve.Evaluate(num2),
				thingRewardRequired = true,
				allowXenogermReimplantation = !flag
			};
			num = new int?(flag ? 3 : 2);
			Pawn asker = pawn;
			num3 = new float?(0f);
			quest.GiveRewards(parms2, text2, null, null, null, null, null, null, num, false, asker, false, false, num3);
			CS$<>8__locals1.quest.AnySignal(new string[]
			{
				text7,
				CS$<>8__locals1.meetingCompletedSignal
			}, null, new string[]
			{
				text2
			}, QuestPart.SignalListenMode.OngoingOnly);
			CS$<>8__locals1.quest.Letter(LetterDefOf.PositiveEvent, text2, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, CS$<>8__locals1.sanguophages, true, "[successLetterText]", null, "[successLetterLabel]", null, null);
			CS$<>8__locals1.quest.End(QuestEndOutcome.Success, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			CS$<>8__locals1.quest.AnySignal(new string[]
			{
				text4,
				text5,
				text6,
				text3
			}, null, new string[]
			{
				text,
				CS$<>8__locals1.questFailedSignal
			}, QuestPart.SignalListenMode.OngoingOnly);
			CS$<>8__locals1.quest.FactionRelationToPlayerChange(CS$<>8__locals1.faction, FactionRelationKind.Hostile, false, text);
			CS$<>8__locals1.quest.End(QuestEndOutcome.Fail, 0, null, text, QuestPart.SignalListenMode.OngoingOnly, false, false);
			CS$<>8__locals1.quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			CS$<>8__locals1.quest.End(QuestEndOutcome.Fail, 0, null, CS$<>8__locals1.allSanguophagesGone, QuestPart.SignalListenMode.OngoingOnly, false, false);
			CS$<>8__locals1.quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x06014B46 RID: 84806 RVA: 0x00631E58 File Offset: 0x00630058
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null, false);
			IntVec3 intVec;
			return map != null && FactionDefOf.Sanguophages.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) && this.TryFindGatheringSpot(map, out intVec);
		}

		// Token: 0x06014B47 RID: 84807 RVA: 0x00631EA8 File Offset: 0x006300A8
		private bool TryFindGatheringSpot(Map map, out IntVec3 cell)
		{
			this.tmpRooms.Clear();
			if (map == null)
			{
				cell = IntVec3.Invalid;
				return false;
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				Room room = building.Position.GetRoom(map);
				if (room != null && !this.tmpRooms.Contains(room))
				{
					this.tmpRooms.Add(room);
				}
			}
			this.tmpRooms.SortBy((Room r) => -QuestNode_Root_SanguophageMeetingHost.<TryFindGatheringSpot>g__RoomScore|16_0(r));
			Predicate<IntVec3> <>9__2;
			foreach (Room room2 in this.tmpRooms)
			{
				for (int i = 0; i < 10; i++)
				{
					IntVec3 root = room2.Cells.RandomElement<IntVec3>();
					Map map2 = map;
					int radius = 5;
					Predicate<IntVec3> extraValidator;
					if ((extraValidator = <>9__2) == null)
					{
						extraValidator = (<>9__2 = delegate(IntVec3 curCell)
						{
							if (curCell.GetEdifice(map) != null)
							{
								return false;
							}
							for (int j = 0; j < GenAdj.AdjacentCellsAround.Length; j++)
							{
								IntVec3 c = curCell + GenAdj.AdjacentCellsAround[j];
								if (!c.InBounds(map) || c.Fogged(map) || !c.Standable(map))
								{
									return false;
								}
							}
							return true;
						});
					}
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(root, map2, radius, extraValidator);
					if (map.reachability.CanReachColony(intVec))
					{
						cell = intVec;
						this.tmpRooms.Clear();
						return true;
					}
				}
			}
			this.tmpRooms.Clear();
			if (RCellFinder.TryFindRandomSpotJustOutsideColony(CellFinder.RandomEdgeCell(map), map, out cell))
			{
				return true;
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06014B4A RID: 84810 RVA: 0x0063221C File Offset: 0x0063041C
		[CompilerGenerated]
		internal static float <TryFindGatheringSpot>g__RoomScore|16_0(Room r)
		{
			if (r.CellCount < 10 || r.PsychologicallyOutdoors)
			{
				return -100f;
			}
			if (r.Role == RoomRoleDefOf.Barracks || r.Role == RoomRoleDefOf.Bedroom)
			{
				return -100f;
			}
			if (r.Role == RoomRoleDefOf.PrisonCell || r.Role == RoomRoleDefOf.PrisonBarracks)
			{
				return -1000f;
			}
			return r.GetStat(RoomStatDefOf.Impressiveness) + r.GetStat(RoomStatDefOf.Beauty);
		}

		// Token: 0x0400E610 RID: 58896
		private const string QuestTag = "SanguophageMeeting";

		// Token: 0x0400E611 RID: 58897
		private const string EnemyListAlias = "enemies";

		// Token: 0x0400E612 RID: 58898
		public const string SanguophageFactionAlias = "faction";

		// Token: 0x0400E613 RID: 58899
		public const string SanguophageListAlias = "sanguophages";

		// Token: 0x0400E614 RID: 58900
		public const string ReimplantedSignalAlias = "xenogermReimplantedSignal";

		// Token: 0x0400E615 RID: 58901
		public const string GatherSpotAlias = "gatherSpot";

		// Token: 0x0400E616 RID: 58902
		private const float ExtraRaidPointsPerSanguophage = 200f;

		// Token: 0x0400E617 RID: 58903
		private const int JoinerWaitDurationTicks = 7500;

		// Token: 0x0400E618 RID: 58904
		private static readonly List<CountChance> SanguophageCountChances = new List<CountChance>
		{
			new CountChance
			{
				count = 2,
				chance = 0.45f
			},
			new CountChance
			{
				count = 3,
				chance = 0.25f
			},
			new CountChance
			{
				count = 4,
				chance = 0.15f
			},
			new CountChance
			{
				count = 5,
				chance = 0.08f
			},
			new CountChance
			{
				count = 6,
				chance = 0.04f
			},
			new CountChance
			{
				count = 7,
				chance = 0.02f
			},
			new CountChance
			{
				count = 8,
				chance = 0.01f
			}
		};

		// Token: 0x0400E619 RID: 58905
		private static readonly IntRange MeetingDurationRange = new IntRange(10000, 20000);

		// Token: 0x0400E61A RID: 58906
		private static readonly IntRange EventDelayRange = new IntRange(5000, 7500);

		// Token: 0x0400E61B RID: 58907
		private static readonly IntRange ArrivalDelayRange = new IntRange(2500, 7500);

		// Token: 0x0400E61C RID: 58908
		private static readonly SimpleCurve RewardValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(500f, 400f),
				true
			},
			{
				new CurvePoint(3000f, 2000f),
				true
			},
			{
				new CurvePoint(30000f, 10000f),
				true
			}
		};

		// Token: 0x0400E61D RID: 58909
		private List<Room> tmpRooms = new List<Room>();
	}
}
