using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000FC9 RID: 4041
	public class Pawn_MindState : IExposable
	{
		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06006392 RID: 25490 RVA: 0x001F89AF File Offset: 0x001F6BAF
		public bool InRoamingCooldown
		{
			get
			{
				return this.lastStartRoamCooldownTick != null && this.lastStartRoamCooldownTick.Value + 30000 > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06006393 RID: 25491 RVA: 0x001F89DD File Offset: 0x001F6BDD
		public bool AvailableForGoodwillReward
		{
			get
			{
				return Find.TickManager.TicksGame >= this.noAidRelationsGainUntilTick;
			}
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06006394 RID: 25492 RVA: 0x001F89F4 File Offset: 0x001F6BF4
		// (set) Token: 0x06006395 RID: 25493 RVA: 0x001F89FC File Offset: 0x001F6BFC
		public bool Active
		{
			get
			{
				return this.activeInt;
			}
			set
			{
				if (value != this.activeInt)
				{
					this.activeInt = value;
					if (this.pawn.Spawned)
					{
						this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
					}
				}
			}
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06006396 RID: 25494 RVA: 0x001F8A36 File Offset: 0x001F6C36
		public bool IsIdle
		{
			get
			{
				return !this.pawn.Downed && this.pawn.Spawned && this.lastJobTag == JobTag.Idle;
			}
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06006397 RID: 25495 RVA: 0x001F8A60 File Offset: 0x001F6C60
		public bool MeleeThreatStillThreat
		{
			get
			{
				return this.meleeThreat != null && this.meleeThreat.Spawned && !this.meleeThreat.ThreatDisabled(this.pawn) && this.meleeThreat.Awake() && this.pawn.Spawned && Find.TickManager.TicksGame <= this.lastMeleeThreatHarmTick + 400 && (float)(this.pawn.Position - this.meleeThreat.Position).LengthHorizontalSquared <= 9f && GenSight.LineOfSight(this.pawn.Position, this.meleeThreat.Position, this.pawn.Map);
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06006398 RID: 25496 RVA: 0x001F8B23 File Offset: 0x001F6D23
		// (set) Token: 0x06006399 RID: 25497 RVA: 0x001F8B2B File Offset: 0x001F6D2B
		public bool WildManEverReachedOutside
		{
			get
			{
				return this.wildManEverReachedOutsideInt;
			}
			set
			{
				if (this.wildManEverReachedOutsideInt == value)
				{
					return;
				}
				this.wildManEverReachedOutsideInt = value;
				ReachabilityUtility.ClearCacheFor(this.pawn);
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x0600639A RID: 25498 RVA: 0x001F8B49 File Offset: 0x001F6D49
		// (set) Token: 0x0600639B RID: 25499 RVA: 0x001F8B51 File Offset: 0x001F6D51
		public bool WillJoinColonyIfRescued
		{
			get
			{
				return this.willJoinColonyIfRescuedInt;
			}
			set
			{
				if (this.willJoinColonyIfRescuedInt == value)
				{
					return;
				}
				this.willJoinColonyIfRescuedInt = value;
				if (this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x0600639C RID: 25500 RVA: 0x001F8B8C File Offset: 0x001F6D8C
		public bool AnythingPreventsJoiningColonyIfRescued
		{
			get
			{
				return this.pawn.Faction == Faction.OfPlayer || (this.pawn.IsPrisoner && !this.pawn.HostFaction.HostileTo(Faction.OfPlayer)) || (!this.pawn.IsPrisoner && this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer) && !this.pawn.Downed);
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x0600639D RID: 25501 RVA: 0x001F8C14 File Offset: 0x001F6E14
		public bool CombatantRecently
		{
			get
			{
				return this.lastCombatantTick >= 0 && Find.TickManager.TicksGame <= this.lastCombatantTick + 3600;
			}
		}

		// Token: 0x0600639E RID: 25502 RVA: 0x001F8C3C File Offset: 0x001F6E3C
		public bool WasRecentlyCombatantTicks(int ticksDuration)
		{
			return this.lastCombatantTick >= 0 && Find.TickManager.TicksGame <= this.lastCombatantTick + ticksDuration;
		}

		// Token: 0x0600639F RID: 25503 RVA: 0x001F8C60 File Offset: 0x001F6E60
		public Pawn_MindState()
		{
		}

		// Token: 0x060063A0 RID: 25504 RVA: 0x001F8DEC File Offset: 0x001F6FEC
		public Pawn_MindState(Pawn pawn)
		{
			this.pawn = pawn;
			this.mentalStateHandler = new MentalStateHandler(pawn);
			this.mentalBreaker = new MentalBreaker(pawn);
			this.mentalFitGenerator = new MentalFitGenerator(pawn);
			this.inspirationHandler = new InspirationHandler(pawn);
			this.priorityWork = new PriorityWork(pawn);
		}

		// Token: 0x060063A1 RID: 25505 RVA: 0x001F8FBB File Offset: 0x001F71BB
		public void Reset(bool clearInspiration = false, bool clearMentalState = true)
		{
			this.Reset(clearInspiration, clearMentalState, false);
		}

		// Token: 0x060063A2 RID: 25506 RVA: 0x001F8FC8 File Offset: 0x001F71C8
		public void Reset(bool clearInspiration = false, bool clearMentalState = true, bool wasDowned = false)
		{
			if (clearMentalState)
			{
				this.mentalStateHandler.Reset();
				this.mentalBreaker.Reset();
				this.mentalFitGenerator.Reset();
			}
			if (clearInspiration)
			{
				this.inspirationHandler.Reset();
			}
			this.activeInt = true;
			this.lastJobTag = JobTag.Misc;
			this.lastIngestTick = -99999;
			this.nextApparelOptimizeTick = -99999;
			this.canFleeIndividual = true;
			this.exitMapAfterTick = -99999;
			this.lastDisturbanceTick = -99999;
			this.forcedGotoPosition = IntVec3.Invalid;
			this.knownExploder = null;
			this.wantsToTradeWithColony = false;
			this.traderDismissed = false;
			this.hasQuest = false;
			this.lastMannedThing = null;
			this.canLovinTick = -99999;
			this.canSleepTick = -99999;
			this.meleeThreat = null;
			this.lastMeleeThreatHarmTick = -99999;
			this.lastEngageTargetTick = -99999;
			this.lastAttackTargetTick = -99999;
			this.lastAttackedTarget = LocalTargetInfo.Invalid;
			this.enemyTarget = null;
			this.breachingTarget = null;
			this.duty = null;
			this.thinkData.Clear();
			this.lastAssignedInteractTime = -99999;
			this.interactionsToday = 0;
			this.lastInventoryRawFoodUseTick = 0;
			this.priorityWork.Clear();
			this.nextMoveOrderIsWait = true;
			this.lastTakeCombatEnhancingDrugTick = -99999;
			this.lastHarmTick = -99999;
			this.lastRangedHarmTick = -99999;
			this.anyCloseHostilesRecently = false;
			this.WillJoinColonyIfRescued = false;
			this.WildManEverReachedOutside = false;
			this.lastSelfTendTick = -99999;
			this.spawnedByInfestationThingComp = false;
			this.lastPredatorHuntingPlayerNotificationTick = -99999;
			this.lastSlaveSuppressedTick = -99999;
			this.lastStartRoamCooldownTick = null;
			this.nextInventoryStockTick = -99999;
			this.returnToHealingPod = false;
			this.resurrectTarget = null;
			this.lastRotStinkTick = -99999;
			this.lastSwamTick = -99999;
			if (!wasDowned)
			{
				this.timesGuestTendedToByPlayer = 0;
			}
		}

		// Token: 0x060063A3 RID: 25507 RVA: 0x001F91AD File Offset: 0x001F73AD
		public void Notify_PawnRedressed()
		{
			this.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame - 60000;
			this.SetupLastHumanMeatTick();
		}

		// Token: 0x060063A4 RID: 25508 RVA: 0x001F91CC File Offset: 0x001F73CC
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.meleeThreat, "meleeThreat", false);
			Scribe_References.Look<Thing>(ref this.enemyTarget, "enemyTarget", false);
			Scribe_References.Look<Thing>(ref this.knownExploder, "knownExploder", false);
			Scribe_References.Look<Thing>(ref this.lastMannedThing, "lastMannedThing", false);
			Scribe_References.Look<Thing>(ref this.droppedWeapon, "droppedWeapon", false);
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Collections.Look<int, int>(ref this.thinkData, "thinkData", LookMode.Value, LookMode.Value);
			Scribe_Values.Look<bool>(ref this.activeInt, "active", true, false);
			Scribe_Values.Look<JobTag>(ref this.lastJobTag, "lastJobTag", JobTag.Misc, false);
			Scribe_Values.Look<int>(ref this.lastIngestTick, "lastIngestTick", -99999, false);
			Scribe_Values.Look<int>(ref this.nextApparelOptimizeTick, "nextApparelOptimizeTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastEngageTargetTick, "lastEngageTargetTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canFleeIndividual, "canFleeIndividual", false, false);
			Scribe_Values.Look<int>(ref this.exitMapAfterTick, "exitMapAfterTick", -99999, false);
			Scribe_Values.Look<IntVec3>(ref this.forcedGotoPosition, "forcedGotoPosition", IntVec3.Invalid, false);
			Scribe_Values.Look<int>(ref this.lastMeleeThreatHarmTick, "lastMeleeThreatHarmTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAssignedInteractTime, "lastAssignedInteractTime", -99999, false);
			Scribe_Values.Look<int>(ref this.interactionsToday, "interactionsToday", 0, false);
			Scribe_Values.Look<int>(ref this.lastInventoryRawFoodUseTick, "lastInventoryRawFoodUseTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastDisturbanceTick, "lastDisturbanceTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.wantsToTradeWithColony, "wantsToTradeWithColony", false, false);
			Scribe_Values.Look<bool>(ref this.hasQuest, "hasQuest", false, false);
			Scribe_Values.Look<int>(ref this.canLovinTick, "canLovinTick", -99999, false);
			Scribe_Values.Look<int>(ref this.canSleepTick, "canSleepTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.nextMoveOrderIsWait, "nextMoveOrderIsWait", true, false);
			Scribe_Values.Look<int>(ref this.lastTakeCombatEnhancingDrugTick, "lastTakeCombatEnhancingDrugTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastTakeRecreationalDrugTick, "lastTakeRecreationalDrugTick", -60000, false);
			Scribe_Values.Look<int>(ref this.lastHarmTick, "lastHarmTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.anyCloseHostilesRecently, "anyCloseHostilesRecently", false, false);
			Scribe_Deep.Look<PawnDuty>(ref this.duty, "duty", Array.Empty<object>());
			Scribe_Deep.Look<MentalStateHandler>(ref this.mentalStateHandler, "mentalStateHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<MentalBreaker>(ref this.mentalBreaker, "mentalBreaker", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<MentalFitGenerator>(ref this.mentalFitGenerator, "mentalFitGenerator", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<InspirationHandler>(ref this.inspirationHandler, "inspirationHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PriorityWork>(ref this.priorityWork, "priorityWork", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.applyBedThoughtsTick, "applyBedThoughtsTick", 0, false);
			Scribe_Values.Look<int>(ref this.applyThroneThoughtsTick, "applyThroneThoughtsTick", 0, false);
			Scribe_Values.Look<bool>(ref this.applyBedThoughtsOnLeave, "applyBedThoughtsOnLeave", false, false);
			Scribe_Values.Look<bool>(ref this.willJoinColonyIfRescuedInt, "willJoinColonyIfRescued", false, false);
			Scribe_Values.Look<bool>(ref this.wildManEverReachedOutsideInt, "wildManEverReachedOutside", false, false);
			Scribe_Values.Look<int>(ref this.timesGuestTendedToByPlayer, "timesGuestTendedToByPlayer", 0, false);
			Scribe_Values.Look<int>(ref this.noAidRelationsGainUntilTick, "noAidRelationsGainUntilTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastSelfTendTick, "lastSelfTendTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
			Scribe_Values.Look<int>(ref this.lastPredatorHuntingPlayerNotificationTick, "lastPredatorHuntingPlayerNotificationTick", -99999, false);
			Scribe_Deep.Look<BreachingTargetData>(ref this.breachingTarget, "breachingTarget", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastSlaveSuppressedTick, "lastSlaveSuppressedTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastHumanMeatIngestedTick, "lastHumanMeatIngestedTick", -99999, false);
			Scribe_Values.Look<int?>(ref this.lastStartRoamCooldownTick, "lastStartRoamCooldownTick", null, false);
			Scribe_Values.Look<int>(ref this.nextInventoryStockTick, "nextInventoryStockTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.returnToHealingPod, "returnToHealingPod", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.lastBedDefSleptIn, "lastBedDefSleptIn");
			Scribe_Collections.Look<Pawn, AutofeedMode>(ref this.autoFeeders, "babyAutoBreastfeedMoms", LookMode.Reference, LookMode.Value, ref this.tmpLoadPawns, ref this.tmpLoadAutofeedMode, false, false, false);
			Scribe_Collections.Look<Pawn, float>(ref this.babyCaravanBreastfeed, "babyCaravanBreastfeed", LookMode.Reference, LookMode.Value, ref this.tmpLoadPawns, ref this.tmpLoadFloat, false, false, false);
			Scribe_Deep.Look<ResurrectCorpseData>(ref this.resurrectTarget, "resurrectTarget", null);
			Scribe_Values.Look<int>(ref this.lastRangedHarmTick, "lastRangedHarmTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastRotStinkTick, "lastRotStinkTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastBroughtToSafeTemperatureTick, "lastBroughtToSafeTemperatureTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastBecameVisibleTick, "lastBecameVisibleTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastBecameInvisibleTick, "lastBecameInvisibleTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastForcedVisibleTick, "lastForcedVisibleTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastCombatantTick, "lastCombatantTick", -99999, false);
			Scribe_Values.Look<int>(ref this.entityTicksInCaptivity, "entityTicksInCaptivity", 0, false);
			Scribe_Values.Look<int>(ref this.hibernationEndedTick, "hibernationEndedTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastDayInteractionTick, "lastDayInteractionTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastSwamTick, "lastSwamTick", -99999, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.autoFeeders == null)
			{
				this.autoFeeders = new Dictionary<Pawn, AutofeedMode>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.babyCaravanBreastfeed == null)
			{
				this.babyCaravanBreastfeed = new Dictionary<Pawn, float>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.mentalFitGenerator == null)
			{
				this.mentalFitGenerator = new MentalFitGenerator(this.pawn);
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060063A5 RID: 25509 RVA: 0x001F97B4 File Offset: 0x001F79B4
		public void MindStateTickInterval(int delta)
		{
			if (this.wantsToTradeWithColony)
			{
				TradeUtility.CheckInteractWithTradersTeachOpportunity(this.pawn);
			}
			if (this.meleeThreat != null && !this.MeleeThreatStillThreat)
			{
				this.meleeThreat = null;
			}
			this.mentalStateHandler.MentalStateHandlerTickInterval(delta);
			this.mentalBreaker.MentalBreakerTickInterval(delta);
			this.mentalFitGenerator.TickInterval(delta);
			this.inspirationHandler.InspirationHandlerTickInterval(delta);
			if (!this.pawn.GetPosture().Laying())
			{
				this.applyBedThoughtsTick = 0;
			}
			if (this.pawn.IsHashIntervalTick(100, delta))
			{
				if (this.pawn.Spawned)
				{
					int regionsToScan = this.anyCloseHostilesRecently ? 24 : 18;
					this.anyCloseHostilesRecently = PawnUtility.EnemiesAreNearby(this.pawn, regionsToScan, true, -1f, 1, false);
				}
				else
				{
					this.anyCloseHostilesRecently = false;
				}
			}
			if (this.WillJoinColonyIfRescued && this.AnythingPreventsJoiningColonyIfRescued)
			{
				this.WillJoinColonyIfRescued = false;
			}
			if (this.pawn.Spawned && this.pawn.IsWildMan() && !this.WildManEverReachedOutside && this.pawn.GetDistrict(RegionType.Set_Passable) != null && this.pawn.GetDistrict(RegionType.Set_Passable).TouchesMapEdge)
			{
				this.WildManEverReachedOutside = true;
			}
			if (this.pawn.Spawned && this.pawn.RaceProps.IsFlesh && this.pawn.needs.mood != null && this.pawn.IsHashIntervalTick(120, delta))
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (this.CanGainGainThoughtNow(terrain.traversedThought))
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(terrain.traversedThought, null);
				}
				WeatherDef curWeatherLerped = this.pawn.Map.weatherManager.CurWeatherLerped;
				if (this.CanGainGainThoughtNow(curWeatherLerped.weatherThought))
				{
					bool flag = this.pawn.Position.Roofed(this.pawn.Map);
					bool flag2 = curWeatherLerped.weatherThought.stages.Count == 1;
					if (!flag || !flag2)
					{
						int stage = (flag2 || flag) ? 0 : 1;
						this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(curWeatherLerped.weatherThought, stage, null);
					}
				}
				if (this.pawn.Position.GasDensity(this.pawn.Map, GasType.RotStink) > 0)
				{
					this.lastRotStinkTick = Find.TickManager.TicksGame;
				}
			}
			if (this.droppedWeapon != null && !this.droppedWeapon.Spawned)
			{
				this.droppedWeapon = null;
			}
			int num = GenLocalDate.DayTick(this.pawn);
			if (num < this.lastDayInteractionTick)
			{
				this.interactionsToday = 0;
			}
			this.lastDayInteractionTick = num;
			if (this.pawn.IsFighting())
			{
				Job curJob = this.pawn.CurJob;
				if (((curJob != null) ? curJob.def : null) != JobDefOf.Wait_Combat)
				{
					goto IL_30C;
				}
			}
			Pawn_EquipmentTracker equipment = this.pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) == null)
			{
				goto IL_31C;
			}
			IL_30C:
			this.lastCombatantTick = Find.TickManager.TicksGame;
			IL_31C:
			Pawn pawn = this.enemyTarget as Pawn;
			if (pawn != null && pawn.mindState != null)
			{
				pawn.mindState.lastCombatantTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060063A6 RID: 25510 RVA: 0x001F9B09 File Offset: 0x001F7D09
		private bool CanGainGainThoughtNow(ThoughtDef thought)
		{
			return thought != null && (thought != ThoughtDefOf.SoakingWet || GenTicks.TicksGame - this.lastSwamTick >= 7500);
		}

		// Token: 0x060063A7 RID: 25511 RVA: 0x001F9B30 File Offset: 0x001F7D30
		public void JoinColonyBecauseRescuedBy(Pawn by)
		{
			this.WillJoinColonyIfRescued = false;
			if (this.AnythingPreventsJoiningColonyIfRescued)
			{
				return;
			}
			InteractionWorker_RecruitAttempt.DoRecruit(by, this.pawn, false);
			if (this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null, null);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMeByOfferingHelp, by, null);
			}
			TaggedString taggedString = "LetterRescueQuestFinished".Translate(this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
			if (!this.pawn.Map.wasSpawnedViaGravShipLanding)
			{
				taggedString += " " + "LetterRescueQuestFinishedCaravanExtra".Translate(this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelRescueQuestFinished".Translate(), taggedString, LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
		}

		// Token: 0x060063A8 RID: 25512 RVA: 0x001F9C78 File Offset: 0x001F7E78
		public void ResetLastDisturbanceTick()
		{
			this.lastDisturbanceTick = -9999999;
		}

		// Token: 0x060063A9 RID: 25513 RVA: 0x001F9C88 File Offset: 0x001F7E88
		public void SetupLastHumanMeatTick()
		{
			if (this.pawn.Ideo != null && this.pawn.Ideo.IdeoCausesHumanMeatCravings())
			{
				this.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
				this.lastHumanMeatIngestedTick -= Pawn_MindState.LastHumanMeatEatenTicksRange.RandomInRange;
			}
		}

		// Token: 0x060063AA RID: 25514 RVA: 0x001F9CDE File Offset: 0x001F7EDE
		public IEnumerable<Gizmo> GetGizmos()
		{
			IEnumerator<Gizmo> enumerator;
			if (this.pawn.IsColonistPlayerControlled || this.pawn.IsColonySubhumanPlayerControlled)
			{
				foreach (Gizmo gizmo in this.priorityWork.GetGizmos())
				{
					yield return gizmo;
				}
				enumerator = null;
			}
			foreach (Gizmo gizmo2 in CaravanFormingUtility.GetGizmos(this.pawn))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060063AB RID: 25515 RVA: 0x001F9CEE File Offset: 0x001F7EEE
		public void SetNoAidRelationsGainUntilTick(int tick)
		{
			if (tick > this.noAidRelationsGainUntilTick)
			{
				this.noAidRelationsGainUntilTick = tick;
			}
		}

		// Token: 0x060063AC RID: 25516 RVA: 0x001F9D00 File Offset: 0x001F7F00
		public void Notify_OutfitChanged()
		{
			this.nextApparelOptimizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060063AD RID: 25517 RVA: 0x001F9D14 File Offset: 0x001F7F14
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			this.mentalStateHandler.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn))
			{
				this.lastHarmTick = Find.TickManager.TicksGame;
				if (dinfo.Def.isRanged)
				{
					this.lastRangedHarmTick = Find.TickManager.TicksGame;
				}
				if (this.pawn.Spawned)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (!this.mentalStateHandler.InMentalState && dinfo.Instigator != null && (pawn != null || dinfo.Instigator is Building_Turret) && dinfo.Instigator.Faction != null && (!ModsConfig.AnomalyActive || dinfo.Instigator.Faction != Faction.OfEntities) && (dinfo.Instigator.Faction.def.humanlikeFaction || (pawn != null && pawn.def.race.intelligence >= Intelligence.ToolUser)) && this.pawn.Faction == null && (this.pawn.IsAnimal || this.pawn.IsWildMan()) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.PredatorHunt || dinfo.Instigator != ((JobDriver_PredatorHunt)this.pawn.jobs.curDriver).Prey) && Rand.Chance(PawnUtility.GetManhunterOnDamageChance(this.pawn, dinfo.Instigator, -1f)))
					{
						this.StartManhunterBecauseOfPawnAction(pawn, "AnimalManhunterFromDamage", true);
					}
					else if (dinfo.Instigator != null && dinfo.Def.makesAnimalsFlee && FleeUtility.ShouldAnimalFleeDanger(this.pawn))
					{
						this.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
					}
				}
				if (this.pawn.GetPosture() != PawnPosture.Standing)
				{
					this.lastDisturbanceTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x060063AE RID: 25518 RVA: 0x001F9F10 File Offset: 0x001F8110
		public void Notify_ClamorImpact(Thing instigator)
		{
			this.canSleepTick = Find.TickManager.TicksGame + 1000;
			if (this.pawn.IsAnimal && instigator is Projectile && ((Projectile)instigator).AnimalsFleeImpact && (this.pawn.playerSettings == null || this.pawn.playerSettings.Master == null) && Rand.Chance(0.4f) && FleeUtility.ShouldAnimalFleeDanger(this.pawn))
			{
				this.StartFleeingBecauseOfPawnAction(((Projectile)instigator).Launcher);
			}
		}

		// Token: 0x060063AF RID: 25519 RVA: 0x001F9F9E File Offset: 0x001F819E
		internal void Notify_EngagedTarget()
		{
			this.lastEngageTargetTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060063B0 RID: 25520 RVA: 0x001F9FB0 File Offset: 0x001F81B0
		internal void Notify_AttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x060063B1 RID: 25521 RVA: 0x001F9FCC File Offset: 0x001F81CC
		internal bool CheckStartMentalStateBecauseRecruitAttempted(Pawn tamer)
		{
			if (!this.pawn.IsAnimal && (!this.pawn.IsWildMan() || this.pawn.IsPrisoner))
			{
				return false;
			}
			if (!this.mentalStateHandler.InMentalState && this.pawn.Faction == null && Rand.Chance(PawnUtility.GetManhunterOnTameFailChance(this.pawn)))
			{
				this.StartManhunterBecauseOfPawnAction(tamer, "AnimalManhunterFromTaming", false);
				return true;
			}
			return false;
		}

		// Token: 0x060063B2 RID: 25522 RVA: 0x001FA03E File Offset: 0x001F823E
		internal void Notify_DangerousExploderAboutToExplode(Thing exploder)
		{
			if (this.pawn.RaceProps.intelligence >= Intelligence.Humanlike && !this.pawn.Drafted)
			{
				this.knownExploder = exploder;
				this.pawn.jobs.CheckForJobOverride(0f, true);
			}
		}

		// Token: 0x060063B3 RID: 25523 RVA: 0x001FA080 File Offset: 0x001F8280
		public void Notify_Explosion(Explosion explosion)
		{
			if (this.pawn.Faction != null)
			{
				return;
			}
			if (explosion.radius < 3.5f || !this.pawn.Position.InHorDistOf(explosion.Position, explosion.radius + 7f))
			{
				return;
			}
			if (FleeUtility.ShouldAnimalFleeDanger(this.pawn))
			{
				this.StartFleeingBecauseOfPawnAction(explosion);
			}
		}

		// Token: 0x060063B4 RID: 25524 RVA: 0x001FA0E4 File Offset: 0x001F82E4
		public void Notify_TuckedIntoBed()
		{
			if (this.pawn.IsWildMan())
			{
				this.WildManEverReachedOutside = false;
			}
			this.ResetLastDisturbanceTick();
		}

		// Token: 0x060063B5 RID: 25525 RVA: 0x001FA100 File Offset: 0x001F8300
		public void Notify_SelfTended()
		{
			this.lastSelfTendTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060063B6 RID: 25526 RVA: 0x001FA112 File Offset: 0x001F8312
		public void Notify_PredatorHuntingPlayerNotification()
		{
			this.lastPredatorHuntingPlayerNotificationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060063B7 RID: 25527 RVA: 0x001FA124 File Offset: 0x001F8324
		private IEnumerable<Pawn> GetPackmates(Pawn pawn, float radius)
		{
			if (!pawn.Spawned)
			{
				yield break;
			}
			District pawnRoom = pawn.GetDistrict(RegionType.Set_Passable);
			IReadOnlyList<Pawn> raceMates = pawn.Map.mapPawns.AllPawnsSpawned;
			HashSet<ThingDef> acceptableDefs = new HashSet<ThingDef>(pawn.RaceProps.crossAggroWith.OrElseEmptyEnumerable<ThingDef>().Prepend(pawn.def));
			int num;
			for (int i = 0; i < raceMates.Count; i = num + 1)
			{
				if (pawn != raceMates[i] && acceptableDefs.Contains(raceMates[i].def) && raceMates[i].Faction == pawn.Faction && raceMates[i].Position.InHorDistOf(pawn.Position, radius) && raceMates[i].GetDistrict(RegionType.Set_Passable) == pawnRoom)
				{
					yield return raceMates[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060063B8 RID: 25528 RVA: 0x001FA13C File Offset: 0x001F833C
		private void StartManhunterBecauseOfPawnAction(Pawn instigator, string letterTextKey, bool causedByDamage = false)
		{
			if (!this.mentalStateHandler.TryStartMentalState(PawnUtility.ManhunterStateFor(this.pawn), null, false, false, false, null, false, false, false))
			{
				return;
			}
			string text = letterTextKey.Translate(this.pawn.Label, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			GlobalTargetInfo target = this.pawn;
			float num = 0.5f;
			if (causedByDamage)
			{
				num *= PawnUtility.GetManhunterChanceFactorForInstigator(instigator);
			}
			int num2 = 1;
			if (Find.Storyteller.difficulty.allowBigThreats && Rand.Value < num)
			{
				PawnKindDef pawnKindDef = this.pawn.RaceProps.manhunterPackUseLabelFrom ?? this.pawn.kindDef;
				Pawn value = this.pawn;
				foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
				{
					if (pawn.mindState.mentalStateHandler.TryStartMentalState(PawnUtility.ManhunterStateFor(pawn), null, false, false, false, null, false, causedByDamage, false))
					{
						num2++;
						if (pawn.kindDef == pawnKindDef)
						{
							value = pawn;
						}
					}
				}
				if (num2 > 1)
				{
					target = new TargetInfo(this.pawn.Position, this.pawn.Map, false);
					text += "\n\n";
					text += "AnimalManhunterOthers".Translate(pawnKindDef.GetLabelPlural(-1), value);
				}
			}
			string value2 = this.pawn.IsAnimal ? this.pawn.Label : this.pawn.def.label;
			string str = "LetterLabelAnimalManhunterRevenge".Translate(value2).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter(str, text, (num2 == 1) ? LetterDefOf.ThreatSmall : LetterDefOf.ThreatBig, target, null, null, null, null, 0, true);
		}

		// Token: 0x060063B9 RID: 25529 RVA: 0x001FA370 File Offset: 0x001F8570
		public void StartFleeingBecauseOfPawnAction(Thing instigator)
		{
			if (this.pawn.RaceProps.canLeaveMapFlying && this.pawn.Faction != Faction.OfPlayer && !this.pawn.Position.Roofed(this.pawn.Map) && !this.pawn.IsQuestLodger() && this.pawn.flight.CanEverFly && Rand.Chance(this.pawn.RaceProps.leaveMapOnFleeChance))
			{
				this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.ExitMapFlying), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				return;
			}
			List<Thing> threats = new List<Thing>
			{
				instigator
			};
			IntVec3 fleeDest = CellFinderLoose.GetFleeDest(this.pawn, threats, this.pawn.Position.DistanceTo(instigator.Position) + 28f);
			if (fleeDest != this.pawn.Position)
			{
				Vector3 lhs = (fleeDest - this.pawn.Position).ToVector3();
				Vector3 rhs = (this.pawn.Map.Center - this.pawn.Position).ToVector3();
				bool flag = Vector3.Dot(lhs, rhs) < 0f;
				IntVec3 c;
				if (this.pawn.IsAnimal && this.pawn.Faction == null && flag && Rand.Chance(0.5f) && CellFinderLoose.GetFleeExitPosition(this.pawn, 10f, out c))
				{
					Job job = JobMaker.MakeJob(JobDefOf.Flee, c, instigator);
					job.exitMapOnArrival = true;
					this.pawn.jobs.StartJob(job, JobCondition.InterruptOptional, null, false, true, null, null, false, false, null, false, true, false);
				}
				else
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false, null, false, true, false);
				}
			}
			if (this.pawn.RaceProps.herdAnimal && Rand.Chance(0.1f))
			{
				foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
				{
					if (FleeUtility.ShouldAnimalFleeDanger(pawn))
					{
						IntVec3 fleeDest2 = CellFinderLoose.GetFleeDest(pawn, threats, pawn.Position.DistanceTo(instigator.Position) + 28f);
						if (fleeDest2 != pawn.Position)
						{
							pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest2, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false, null, false, true, false);
						}
					}
				}
			}
		}

		// Token: 0x060063BA RID: 25530 RVA: 0x001FA698 File Offset: 0x001F8898
		public void SetAutofeeder(Pawn feeder, AutofeedMode setting)
		{
			if (setting == AutofeedMode.Childcare)
			{
				this.autoFeeders.Remove(feeder);
			}
			else
			{
				this.autoFeeders[feeder] = setting;
			}
			if (setting == AutofeedMode.Childcare)
			{
				if (feeder.WorkTypeIsDisabled(WorkTypeDefOf.Childcare))
				{
					Messages.Message("MessageChildcareDisabled".Translate(feeder.Named("FEEDER")), feeder, MessageTypeDefOf.CautionInput, false);
					return;
				}
				if (!feeder.workSettings.WorkIsActive(WorkTypeDefOf.Childcare))
				{
					Messages.Message("MessageChildcareNotAssigned".Translate(feeder.Named("FEEDER")), feeder, MessageTypeDefOf.CautionInput, false);
				}
			}
		}

		// Token: 0x060063BB RID: 25531 RVA: 0x001FA73F File Offset: 0x001F893F
		public AutofeedMode AutofeedSetting(Pawn feeder)
		{
			if (!this.pawn.DevelopmentalStage.Baby())
			{
				return AutofeedMode.Never;
			}
			return this.autoFeeders.TryGetValue(feeder, AutofeedMode.Childcare);
		}

		// Token: 0x060063BC RID: 25532 RVA: 0x001FA764 File Offset: 0x001F8964
		public IEnumerable<Pawn> Autofeeders()
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(this.pawn, out breastfeedFailReason))
			{
				return Enumerable.Empty<Pawn>();
			}
			return this.autoFeeders.Keys;
		}

		// Token: 0x060063BD RID: 25533 RVA: 0x001FA794 File Offset: 0x001F8994
		public bool AnyAutofeeder(AutofeedMode autofeed, Predicate<Pawn, Pawn> feederPredicate, List<Pawn> possibleFeeders = null)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(this.pawn, out breastfeedFailReason))
			{
				return false;
			}
			possibleFeeders = (possibleFeeders ?? this.pawn.MapHeld.mapPawns.FreeHumanlikesOfFaction(this.pawn.Faction));
			foreach (Pawn pawn in possibleFeeders)
			{
				if (pawn != this.pawn && autofeed == this.AutofeedSetting(pawn) && feederPredicate(this.pawn, pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060063BE RID: 25534 RVA: 0x001FA83C File Offset: 0x001F8A3C
		public void ClearBreastfeedCaravan()
		{
			this.babyCaravanBreastfeed.Clear();
		}

		// Token: 0x060063BF RID: 25535 RVA: 0x001FA84C File Offset: 0x001F8A4C
		public void BreastfeedCaravan(Pawn baby, float amountPct)
		{
			if (this.pawn.GetCaravan() == null)
			{
				Log.Warning(this.pawn.Label + " tried to caravan breastfeed while not in a caravan");
			}
			if (!this.babyCaravanBreastfeed.ContainsKey(baby))
			{
				this.babyCaravanBreastfeed.Add(baby, 0f);
			}
			float num = this.babyCaravanBreastfeed[baby] + amountPct;
			if (num >= 0.6f)
			{
				num -= 0.6f;
				baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedMe, this.pawn, null);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedBaby, baby, null);
			}
			this.babyCaravanBreastfeed[baby] = num;
		}

		// Token: 0x04004743 RID: 18243
		public Pawn pawn;

		// Token: 0x04004744 RID: 18244
		public MentalStateHandler mentalStateHandler;

		// Token: 0x04004745 RID: 18245
		public MentalBreaker mentalBreaker;

		// Token: 0x04004746 RID: 18246
		public MentalFitGenerator mentalFitGenerator;

		// Token: 0x04004747 RID: 18247
		public InspirationHandler inspirationHandler;

		// Token: 0x04004748 RID: 18248
		public PriorityWork priorityWork;

		// Token: 0x04004749 RID: 18249
		private bool activeInt = true;

		// Token: 0x0400474A RID: 18250
		public JobTag lastJobTag;

		// Token: 0x0400474B RID: 18251
		public int lastIngestTick = -99999;

		// Token: 0x0400474C RID: 18252
		public int nextApparelOptimizeTick = -99999;

		// Token: 0x0400474D RID: 18253
		public bool canFleeIndividual = true;

		// Token: 0x0400474E RID: 18254
		public int exitMapAfterTick = -99999;

		// Token: 0x0400474F RID: 18255
		public int lastDisturbanceTick = -99999;

		// Token: 0x04004750 RID: 18256
		public int hibernationEndedTick = -99999;

		// Token: 0x04004751 RID: 18257
		public IntVec3 forcedGotoPosition = IntVec3.Invalid;

		// Token: 0x04004752 RID: 18258
		public Thing knownExploder;

		// Token: 0x04004753 RID: 18259
		public bool wantsToTradeWithColony;

		// Token: 0x04004754 RID: 18260
		public bool traderDismissed;

		// Token: 0x04004755 RID: 18261
		public bool hasQuest;

		// Token: 0x04004756 RID: 18262
		public Thing lastMannedThing;

		// Token: 0x04004757 RID: 18263
		public int canLovinTick = -99999;

		// Token: 0x04004758 RID: 18264
		public int canSleepTick = -99999;

		// Token: 0x04004759 RID: 18265
		public Pawn meleeThreat;

		// Token: 0x0400475A RID: 18266
		public int lastMeleeThreatHarmTick = -99999;

		// Token: 0x0400475B RID: 18267
		public int lastEngageTargetTick = -99999;

		// Token: 0x0400475C RID: 18268
		public int lastAttackTargetTick = -99999;

		// Token: 0x0400475D RID: 18269
		public LocalTargetInfo lastAttackedTarget;

		// Token: 0x0400475E RID: 18270
		public Thing enemyTarget;

		// Token: 0x0400475F RID: 18271
		public BreachingTargetData breachingTarget;

		// Token: 0x04004760 RID: 18272
		public PawnDuty duty;

		// Token: 0x04004761 RID: 18273
		public Dictionary<int, int> thinkData = new Dictionary<int, int>();

		// Token: 0x04004762 RID: 18274
		public int lastAssignedInteractTime = -99999;

		// Token: 0x04004763 RID: 18275
		public int interactionsToday;

		// Token: 0x04004764 RID: 18276
		public int lastDayInteractionTick;

		// Token: 0x04004765 RID: 18277
		public int lastInventoryRawFoodUseTick;

		// Token: 0x04004766 RID: 18278
		public bool nextMoveOrderIsWait;

		// Token: 0x04004767 RID: 18279
		public bool nextMoveOrderIsCrawlBreak;

		// Token: 0x04004768 RID: 18280
		public int lastTakeCombatEnhancingDrugTick = -99999;

		// Token: 0x04004769 RID: 18281
		public int lastTakeRecreationalDrugTick = -60000;

		// Token: 0x0400476A RID: 18282
		public int lastHarmTick = -99999;

		// Token: 0x0400476B RID: 18283
		public int noAidRelationsGainUntilTick = -99999;

		// Token: 0x0400476C RID: 18284
		public bool anyCloseHostilesRecently;

		// Token: 0x0400476D RID: 18285
		public int applyBedThoughtsTick;

		// Token: 0x0400476E RID: 18286
		public int applyThroneThoughtsTick;

		// Token: 0x0400476F RID: 18287
		public bool applyBedThoughtsOnLeave;

		// Token: 0x04004770 RID: 18288
		public bool willJoinColonyIfRescuedInt;

		// Token: 0x04004771 RID: 18289
		private bool wildManEverReachedOutsideInt;

		// Token: 0x04004772 RID: 18290
		public int timesGuestTendedToByPlayer;

		// Token: 0x04004773 RID: 18291
		public int lastSelfTendTick = -99999;

		// Token: 0x04004774 RID: 18292
		public bool spawnedByInfestationThingComp;

		// Token: 0x04004775 RID: 18293
		public int lastPredatorHuntingPlayerNotificationTick = -99999;

		// Token: 0x04004776 RID: 18294
		public int lastSlaveSuppressedTick = -99999;

		// Token: 0x04004777 RID: 18295
		public ThingDef lastBedDefSleptIn;

		// Token: 0x04004778 RID: 18296
		public int lastHumanMeatIngestedTick = -99999;

		// Token: 0x04004779 RID: 18297
		public int? lastStartRoamCooldownTick;

		// Token: 0x0400477A RID: 18298
		public int nextInventoryStockTick = -99999;

		// Token: 0x0400477B RID: 18299
		public bool returnToHealingPod;

		// Token: 0x0400477C RID: 18300
		private Dictionary<Pawn, AutofeedMode> autoFeeders = new Dictionary<Pawn, AutofeedMode>();

		// Token: 0x0400477D RID: 18301
		private Dictionary<Pawn, float> babyCaravanBreastfeed = new Dictionary<Pawn, float>();

		// Token: 0x0400477E RID: 18302
		public int nextSleepingBreastfeedStrictTick = -99999;

		// Token: 0x0400477F RID: 18303
		public ResurrectCorpseData resurrectTarget;

		// Token: 0x04004780 RID: 18304
		public int lastRangedHarmTick;

		// Token: 0x04004781 RID: 18305
		public Thing droppedWeapon;

		// Token: 0x04004782 RID: 18306
		public int lastRotStinkTick = -99999;

		// Token: 0x04004783 RID: 18307
		public int lastBroughtToSafeTemperatureTick = -99999;

		// Token: 0x04004784 RID: 18308
		public int lastBecameVisibleTick = -99999;

		// Token: 0x04004785 RID: 18309
		public int lastBecameInvisibleTick = -99999;

		// Token: 0x04004786 RID: 18310
		public int lastForcedVisibleTick = -99999;

		// Token: 0x04004787 RID: 18311
		public int lastCombatantTick = -99999;

		// Token: 0x04004788 RID: 18312
		public int entityTicksInCaptivity;

		// Token: 0x04004789 RID: 18313
		public int lastSwamTick = -99999;

		// Token: 0x0400478A RID: 18314
		public float maxDistToSquadFlag = -1f;

		// Token: 0x0400478B RID: 18315
		private List<Pawn> tmpLoadPawns;

		// Token: 0x0400478C RID: 18316
		private List<AutofeedMode> tmpLoadAutofeedMode;

		// Token: 0x0400478D RID: 18317
		private List<float> tmpLoadFloat;

		// Token: 0x0400478E RID: 18318
		private const int UpdateAnyCloseHostilesRecentlyEveryTicks = 100;

		// Token: 0x0400478F RID: 18319
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToActivate = 18;

		// Token: 0x04004790 RID: 18320
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToDeactivate = 24;

		// Token: 0x04004791 RID: 18321
		private const float HarmForgetDistance = 3f;

		// Token: 0x04004792 RID: 18322
		private const int MeleeHarmForgetDelay = 400;

		// Token: 0x04004793 RID: 18323
		private const float ClamorImpactFleeChance = 0.4f;

		// Token: 0x04004794 RID: 18324
		private const float PawnActionFleeOffMapChance = 0.5f;

		// Token: 0x04004795 RID: 18325
		private const int RoamingCooldownTicks = 30000;

		// Token: 0x04004796 RID: 18326
		private static readonly IntRange LastHumanMeatEatenTicksRange = new IntRange(0, 60000);

		// Token: 0x04004797 RID: 18327
		public static readonly IntRange NextSleepingBreastfeedTickCheck = new IntRange(300, 500);

		// Token: 0x04004798 RID: 18328
		public const int DefaultCombatantCooldown = 3600;

		// Token: 0x04004799 RID: 18329
		public const int WeatherThoughInterval = 120;

		// Token: 0x0400479A RID: 18330
		public const int SwimSoakingWetImmuneDuration = 7500;
	}
}
