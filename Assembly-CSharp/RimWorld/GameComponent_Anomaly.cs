using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020037D2 RID: 14290
	public class GameComponent_Anomaly : GameComponent
	{
		// Token: 0x1700328A RID: 12938
		// (get) Token: 0x06013C9E RID: 81054 RVA: 0x005DED37 File Offset: 0x005DCF37
		public bool MonolithSpawned
		{
			get
			{
				return this.monolith != null && this.monolith.Spawned;
			}
		}

		// Token: 0x1700328B RID: 12939
		// (get) Token: 0x06013C9F RID: 81055 RVA: 0x005DED4E File Offset: 0x005DCF4E
		public LargeBuildingSpawnParms MonolithSpawnParms
		{
			get
			{
				return this.monolithSpawnParms;
			}
		}

		// Token: 0x1700328C RID: 12940
		// (get) Token: 0x06013CA0 RID: 81056 RVA: 0x005DED56 File Offset: 0x005DCF56
		public int Level
		{
			get
			{
				if (!this.MonolithSpawned)
				{
					return 0;
				}
				return this.level;
			}
		}

		// Token: 0x1700328D RID: 12941
		// (get) Token: 0x06013CA1 RID: 81057 RVA: 0x005DED68 File Offset: 0x005DCF68
		public int HighestLevelReached
		{
			get
			{
				return this.highestLevelReached;
			}
		}

		// Token: 0x1700328E RID: 12942
		// (get) Token: 0x06013CA2 RID: 81058 RVA: 0x005DED70 File Offset: 0x005DCF70
		public MonolithLevelDef LevelDef
		{
			get
			{
				if (!this.MonolithSpawned)
				{
					return MonolithLevelDefOf.Inactive;
				}
				return this.levelDef ?? MonolithLevelDefOf.Inactive;
			}
		}

		// Token: 0x1700328F RID: 12943
		// (get) Token: 0x06013CA3 RID: 81059 RVA: 0x005DED8F File Offset: 0x005DCF8F
		public MonolithLevelDef NextLevelDef
		{
			get
			{
				if (!this.LevelDef.advanceThroughActivation)
				{
					return null;
				}
				return DefDatabase<MonolithLevelDef>.AllDefs.FirstOrDefault((MonolithLevelDef x) => x.level == this.Level + 1);
			}
		}

		// Token: 0x17003290 RID: 12944
		// (get) Token: 0x06013CA4 RID: 81060 RVA: 0x005DEDB6 File Offset: 0x005DCFB6
		public bool QuestlineEnded
		{
			get
			{
				return this.LevelDef == MonolithLevelDefOf.Embraced || this.LevelDef == MonolithLevelDefOf.Disrupted;
			}
		}

		// Token: 0x17003291 RID: 12945
		// (get) Token: 0x06013CA5 RID: 81061 RVA: 0x005DEDD4 File Offset: 0x005DCFD4
		public int TicksSinceLastLevelChange
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLevelChangeTick;
			}
		}

		// Token: 0x17003292 RID: 12946
		// (get) Token: 0x06013CA6 RID: 81062 RVA: 0x005DEDE7 File Offset: 0x005DCFE7
		private int TicksSinceLastMetalhorrorBio
		{
			get
			{
				return Find.TickManager.TicksGame - this.newestMetalhorrorBiosignatureTick;
			}
		}

		// Token: 0x17003293 RID: 12947
		// (get) Token: 0x06013CA7 RID: 81063 RVA: 0x005DEDFA File Offset: 0x005DCFFA
		public bool CanNewMetalhorrorBiosignatureImplantOccur
		{
			get
			{
				return this.TicksSinceLastMetalhorrorBio >= 1800000;
			}
		}

		// Token: 0x17003294 RID: 12948
		// (get) Token: 0x06013CA8 RID: 81064 RVA: 0x005DEE0C File Offset: 0x005DD00C
		public IReadOnlyList<ChoiceLetter> MonolithLetters
		{
			get
			{
				return this.monolithLetters;
			}
		}

		// Token: 0x17003295 RID: 12949
		// (get) Token: 0x06013CA9 RID: 81065 RVA: 0x005DEE14 File Offset: 0x005DD014
		public bool MonolithStudyCompleted
		{
			get
			{
				return this.monolithLetters.Count == 3;
			}
		}

		// Token: 0x17003296 RID: 12950
		// (get) Token: 0x06013CAA RID: 81066 RVA: 0x005DEE24 File Offset: 0x005DD024
		public int MonolithNextIndex
		{
			get
			{
				return this.monolithNextIndex;
			}
		}

		// Token: 0x17003297 RID: 12951
		// (get) Token: 0x06013CAB RID: 81067 RVA: 0x005DEE2C File Offset: 0x005DD02C
		public int MonolithStudyProgress
		{
			get
			{
				return this.monolithStudyProgress;
			}
		}

		// Token: 0x17003298 RID: 12952
		// (get) Token: 0x06013CAC RID: 81068 RVA: 0x005DEE34 File Offset: 0x005DD034
		public bool GenerateMonolith
		{
			get
			{
				return Find.Storyteller.difficulty.AnomalyPlaystyleDef.generateMonolith;
			}
		}

		// Token: 0x17003299 RID: 12953
		// (get) Token: 0x06013CAD RID: 81069 RVA: 0x005DEE4A File Offset: 0x005DD04A
		public bool AmbientHorrorMode
		{
			get
			{
				return Find.Storyteller.difficulty.AnomalyPlaystyleDef == AnomalyPlaystyleDefOf.AmbientHorror;
			}
		}

		// Token: 0x1700329A RID: 12954
		// (get) Token: 0x06013CAE RID: 81070 RVA: 0x005DEE62 File Offset: 0x005DD062
		public bool AnomalyStudyEnabled
		{
			get
			{
				return ModsConfig.AnomalyActive && Find.Storyteller.difficulty.AnomalyPlaystyleDef.enableAnomalyContent && (this.HighestLevelReached > 0 || !this.GenerateMonolith);
			}
		}

		// Token: 0x1700329B RID: 12955
		// (get) Token: 0x06013CAF RID: 81071 RVA: 0x005DEE9C File Offset: 0x005DD09C
		public float AnomalyThreatFractionNow
		{
			get
			{
				AnomalyPlaystyleDef anomalyPlaystyleDef = Find.Storyteller.difficulty.AnomalyPlaystyleDef;
				if (!anomalyPlaystyleDef.enableAnomalyContent)
				{
					return 0f;
				}
				if (anomalyPlaystyleDef.overrideThreatFraction && Find.Storyteller.difficulty.overrideAnomalyThreatsFraction != null)
				{
					return Find.Storyteller.difficulty.overrideAnomalyThreatsFraction.Value;
				}
				MonolithLevelDef monolithLevelDef = this.LevelDef;
				if (monolithLevelDef.useInactiveAnomalyThreatFraction)
				{
					return Find.Storyteller.difficulty.anomalyThreatsInactiveFraction * monolithLevelDef.anomalyThreatFractionFactor;
				}
				if (monolithLevelDef.useActiveAnomalyThreatFraction)
				{
					return Find.Storyteller.difficulty.anomalyThreatsActiveFraction * monolithLevelDef.anomalyThreatFractionFactor;
				}
				return this.LevelDef.anomalyThreatFraction * monolithLevelDef.anomalyThreatFractionFactor;
			}
		}

		// Token: 0x06013CB0 RID: 81072 RVA: 0x005DEF54 File Offset: 0x005DD154
		public GameComponent_Anomaly(Game game)
		{
			if (ModsConfig.AnomalyActive)
			{
				this.levelDef = DefDatabase<MonolithLevelDef>.AllDefs.FirstOrDefault((MonolithLevelDef x) => x.level == this.level);
				this.level = this.levelDef.level;
				this.monolithSpawnParms = new LargeBuildingSpawnParms
				{
					minDistanceToColonyBuilding = 30f,
					minDistToEdge = 10,
					attemptSpawnLocationType = SpawnLocationType.Outdoors,
					attemptNotUnderBuildings = true,
					canSpawnOnImpassable = false,
					allowFogged = true,
					overrideSize = new IntVec2?(new IntVec2(ThingDefOf.VoidMonolith.size.x + 2, ThingDefOf.VoidMonolith.size.z + 2))
				};
			}
		}

		// Token: 0x06013CB1 RID: 81073 RVA: 0x005DF0AC File Offset: 0x005DD2AC
		public override void StartedNewGame()
		{
			base.StartedNewGame();
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			this.Notify_LevelChanged(true);
			this.nextHarbingerTreeCheckTick = Find.TickManager.TicksGame + Mathf.RoundToInt(GameComponent_Anomaly.HarbingerTreeCheckIntervalHours.RandomInRange * 2500f);
			this.monolith = (Find.AnyPlayerHomeMap.listerThings.ThingsOfDef(ThingDefOf.VoidMonolith).FirstOrDefault<Thing>() as Building_VoidMonolith);
		}

		// Token: 0x06013CB2 RID: 81074 RVA: 0x005DF11C File Offset: 0x005DD31C
		public override void GameComponentTick()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (this.monolith != null && this.monolith.quest == null && this.Level > 0)
			{
				this.monolith.CheckAndGenerateQuest();
			}
			if (this.monolith != null && !this.monolith.Spawned)
			{
				this.monolith = null;
			}
			if (Find.TickManager.TicksGame > this.nextHarbingerTreeCheckTick)
			{
				this.TrySpawnHarbingerTrees();
				this.nextHarbingerTreeCheckTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + GameComponent_Anomaly.HarbingerTreeCheckIntervalHours.RandomInRange * 2500f);
			}
			this.UpdateCorpseTrackers();
			this.UpdateHypnotized();
			if (this.fireLevelIncidentTick > 0 && Find.TickManager.TicksGame > this.fireLevelIncidentTick)
			{
				if (this.levelDef.incidentsOnReached != null)
				{
					foreach (IncidentDef def in this.levelDef.incidentsOnReached)
					{
						IncidentParms parms = new IncidentParms
						{
							target = Find.AnyPlayerHomeMap,
							points = StorytellerUtility.DefaultThreatPointsNow(this.monolith.Map),
							forced = true
						};
						Find.Storyteller.incidentQueue.Add(def, Find.TickManager.TicksGame, parms, 0);
					}
				}
				this.fireLevelIncidentTick = -99999;
			}
			if (this.fireGrayPallTick > 0 && GenTicks.TicksGame > this.fireGrayPallTick)
			{
				if (this.monolith != null)
				{
					int duration = Mathf.CeilToInt(GameComponent_Anomaly.GrayPallConditionDaysRange.RandomInRange * 60000f);
					GameCondition cond = GameConditionMaker.MakeCondition(GameConditionDefOf.GrayPall, duration);
					this.monolith.MapHeld.GameConditionManager.RegisterCondition(cond);
					TaggedString label = "LetterLabelGrayPallDescending".Translate();
					TaggedString text = "LetterGrayPallDescending".Translate();
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, null, 0, true);
				}
				this.fireGrayPallTick = -99999;
			}
			if (!this.metalHellPawns.NullOrEmpty<Pawn>() && Find.TickManager.TicksGame > this.metalHellReturnTick)
			{
				Map map = this.monolith.Map;
				Predicate<IntVec3> <>9__0;
				Predicate<IntVec3> <>9__1;
				foreach (Pawn pawn in this.metalHellPawns)
				{
					IntVec3 position = this.monolith.Position;
					Map map3 = this.monolith.Map;
					int squareRadius = 5;
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 c) => !c.Fogged(map) && c.Standable(map)));
					}
					IntVec3 intVec;
					if (CellFinder.TryFindRandomCellNear(position, map3, squareRadius, validator, out intVec, -1))
					{
						SkipUtility.SkipTo(pawn, intVec, this.monolith.Map);
					}
					else
					{
						Map map2 = this.monolith.Map;
						Predicate<IntVec3> validator2;
						if ((validator2 = <>9__1) == null)
						{
							validator2 = (<>9__1 = ((IntVec3 c) => !c.Fogged(map) && c.Standable(map) && map.reachability.CanReachColony(c)));
						}
						if (CellFinder.TryFindRandomCell(map2, validator2, out intVec))
						{
							SkipUtility.SkipTo(pawn, intVec, this.monolith.Map);
						}
					}
					this.monolith.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(intVec, this.monolith.Map, 1f), intVec, 60);
					SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(intVec, this.monolith.Map, false));
					Messages.Message("VoidNodeReturnedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
				}
				Find.LetterStack.ReceiveLetter("VoidNodeReturnedLabel".Translate(this.voidNodeActivator.Named("PAWN")), this.metalHellReturnLetterText, LetterDefOf.PositiveEvent, this.voidNodeActivator, null, null, null, null, 0, true);
				this.metalHellPawns.Clear();
				this.metalHellReturnTick = -99999;
				this.metalHellReturnLetterText = null;
			}
			if (Find.TickManager.TicksGame == this.metalHellClosedTick + 60 && this.levelDef == MonolithLevelDefOf.Disrupted)
			{
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (faction != Faction.OfPlayer && faction.def.humanlikeFaction && !faction.def.permanentEnemy)
					{
						faction.TryAffectGoodwillWith(Faction.OfPlayer, 50, true, true, HistoryEventDefOf.DestroyedVoidMonolith, null);
					}
				}
			}
		}

		// Token: 0x06013CB3 RID: 81075 RVA: 0x005DF608 File Offset: 0x005DD808
		public void Notify_NewMetalhorrorBiosignatureImplanted()
		{
			this.newestMetalhorrorBiosignatureTick = GenTicks.TicksGame;
		}

		// Token: 0x06013CB4 RID: 81076 RVA: 0x005DF615 File Offset: 0x005DD815
		public void Notify_MonolithStudyIncreased(ChoiceLetter letter, int nextIndex, int studyProgress)
		{
			this.monolithLetters.Add(letter);
			this.monolithNextIndex = nextIndex;
			this.monolithStudyProgress = studyProgress;
		}

		// Token: 0x06013CB5 RID: 81077 RVA: 0x005DF634 File Offset: 0x005DD834
		private void UpdateHypnotized()
		{
			foreach (KeyValuePair<Pawn, int> keyValuePair in this.hypnotisedPawns)
			{
				Pawn pawn;
				int num;
				keyValuePair.Deconstruct(out pawn, out num);
				Pawn item = pawn;
				int num2 = num;
				if (GenTicks.TicksGame > num2)
				{
					GameComponent_Anomaly.toRemove.Add(item);
				}
			}
			foreach (Pawn key in GameComponent_Anomaly.toRemove)
			{
				this.hypnotisedPawns.Remove(key);
			}
			GameComponent_Anomaly.toRemove.Clear();
		}

		// Token: 0x06013CB6 RID: 81078 RVA: 0x005DF6F8 File Offset: 0x005DD8F8
		private void UpdateCorpseTrackers()
		{
			this.ElevateLooseCorpseTrackers();
			foreach (UnnaturalCorpseTracker unnaturalCorpseTracker in this.looseCorpseTrackers)
			{
				unnaturalCorpseTracker.CorpseTick();
				if (unnaturalCorpseTracker.ShouldDisappear)
				{
					unnaturalCorpseTracker.Notify_Finished();
					GameComponent_Anomaly.tmpTrackers.Add(unnaturalCorpseTracker);
				}
			}
			foreach (KeyValuePair<Pawn, UnnaturalCorpseTracker> keyValuePair in this.corpseTrackers)
			{
				Pawn pawn;
				UnnaturalCorpseTracker unnaturalCorpseTracker2;
				keyValuePair.Deconstruct(out pawn, out unnaturalCorpseTracker2);
				Pawn item = pawn;
				UnnaturalCorpseTracker unnaturalCorpseTracker3 = unnaturalCorpseTracker2;
				unnaturalCorpseTracker3.CorpseTick();
				if (unnaturalCorpseTracker3.ShouldDisappear)
				{
					unnaturalCorpseTracker3.Notify_Finished();
					GameComponent_Anomaly.toRemove.Add(item);
				}
			}
			foreach (Pawn key in GameComponent_Anomaly.toRemove)
			{
				this.corpseTrackers.Remove(key);
			}
			foreach (UnnaturalCorpseTracker item2 in GameComponent_Anomaly.tmpTrackers)
			{
				this.looseCorpseTrackers.Remove(item2);
			}
			GameComponent_Anomaly.toRemove.Clear();
			GameComponent_Anomaly.tmpTrackers.Clear();
		}

		// Token: 0x06013CB7 RID: 81079 RVA: 0x005DF880 File Offset: 0x005DDA80
		public void RemoveCorpseTracker(Pawn pawn)
		{
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!this.corpseTrackers.TryGetValue(pawn, out unnaturalCorpseTracker))
			{
				return;
			}
			unnaturalCorpseTracker.Notify_Finished();
			this.corpseTrackers.Remove(pawn);
		}

		// Token: 0x06013CB8 RID: 81080 RVA: 0x005DF8B1 File Offset: 0x005DDAB1
		public void IncrementLevel()
		{
			if (this.LevelDef == MonolithLevelDefOf.Inactive)
			{
				this.level = 0;
			}
			this.level++;
			this.Notify_LevelChanged(false);
		}

		// Token: 0x06013CB9 RID: 81081 RVA: 0x005DF8DC File Offset: 0x005DDADC
		public void Hypnotize(Pawn pawn, Pawn instigator, int ticks)
		{
			Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_Hypnotized, instigator));
			this.hypnotisedPawns[pawn] = GenTicks.TicksGame + ticks;
		}

		// Token: 0x06013CBA RID: 81082 RVA: 0x005DF907 File Offset: 0x005DDB07
		public void EndHypnotize(Pawn pawn)
		{
			this.hypnotisedPawns.Remove(pawn);
		}

		// Token: 0x06013CBB RID: 81083 RVA: 0x005DF916 File Offset: 0x005DDB16
		public bool IsPawnHypnotized(Pawn pawn)
		{
			return this.hypnotisedPawns.ContainsKey(pawn);
		}

		// Token: 0x06013CBC RID: 81084 RVA: 0x005DF924 File Offset: 0x005DDB24
		public void SetLevel(MonolithLevelDef levelDef, bool silent = false)
		{
			int num = this.level;
			this.level = levelDef.level;
			if (num != this.level)
			{
				this.Notify_LevelChanged(silent);
			}
		}

		// Token: 0x06013CBD RID: 81085 RVA: 0x005DF948 File Offset: 0x005DDB48
		private void Notify_LevelChanged(bool silent = false)
		{
			if (!ModLister.CheckAnomaly("Monolith level"))
			{
				return;
			}
			this.highestLevelReached = Mathf.Max(this.highestLevelReached, this.level);
			this.lastLevelChangeTick = Find.TickManager.TicksGame;
			this.levelDef = DefDatabase<MonolithLevelDef>.AllDefs.FirstOrDefault((MonolithLevelDef x) => x.level == this.level);
			Building_VoidMonolith building_VoidMonolith = this.monolith;
			if (building_VoidMonolith != null)
			{
				building_VoidMonolith.SetLevel(this.levelDef);
			}
			Find.ResearchManager.Notify_MonolithLevelChanged(this.level);
			if (!silent)
			{
				if (!this.levelDef.incidentsOnReached.NullOrEmpty<IncidentDef>())
				{
					this.fireLevelIncidentTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + GameComponent_Anomaly.MonolithLevelIncidentDelayRangeHours.RandomInRange * 2500f);
				}
				if (this.Level > 0)
				{
					Find.CameraDriver.shaker.DoShake(0.05f, 300);
				}
				if (this.LevelDef.triggersGrayPall)
				{
					this.TriggerGrayPall();
				}
				if (this.LevelDef == MonolithLevelDefOf.VoidAwakened)
				{
					this.TriggerVoidAwakening();
				}
			}
			Find.SignalManager.SendSignal(new Signal("MonolithLevelChanged", true));
			if (this.level == 1)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.StudyingEntities, OpportunityType.Important);
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.EntityCodex, OpportunityType.GoodToKnow);
				IncidentParms parms = new IncidentParms
				{
					target = this.monolith.Map,
					points = StorytellerUtility.DefaultThreatPointsNow(this.monolith.Map),
					forced = true
				};
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.VoidCuriosity, Find.TickManager.TicksGame + Mathf.RoundToInt(GameComponent_Anomaly.VoidCuriosityIncidentDelayRangeDays.RandomInRange * 60000f), parms, 0);
			}
		}

		// Token: 0x06013CBE RID: 81086 RVA: 0x005DFAFB File Offset: 0x005DDCFB
		private void TriggerGrayPall()
		{
			this.fireGrayPallTick = GenTicks.TicksGame + 300;
		}

		// Token: 0x06013CBF RID: 81087 RVA: 0x005DFB10 File Offset: 0x005DDD10
		private void TriggerVoidAwakening()
		{
			Slate slate = new Slate();
			slate.Set<Map>("map", this.monolith.Map, false);
			QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.EndGame_VoidAwakening, slate);
		}

		// Token: 0x06013CC0 RID: 81088 RVA: 0x005DFB48 File Offset: 0x005DDD48
		public bool TryGetCellForMonolithSpawn(Map map, out IntVec3 cell, bool allowFogged = false)
		{
			LargeBuildingSpawnParms largeBuildingSpawnParms = this.MonolithSpawnParms.ForThing(ThingDefOf.VoidMonolith);
			LargeBuildingSpawnParms parms = largeBuildingSpawnParms;
			parms.minDistanceToColonyBuilding = 1f;
			largeBuildingSpawnParms.allowFogged = allowFogged;
			parms.allowFogged = allowFogged;
			return LargeBuildingCellFinder.TryFindCell(out cell, map, largeBuildingSpawnParms, null, (IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, map), false) || LargeBuildingCellFinder.TryFindCell(out cell, map, parms, null, (IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, map), false);
		}

		// Token: 0x06013CC1 RID: 81089 RVA: 0x005DFBD4 File Offset: 0x005DDDD4
		public Building_VoidMonolith SpawnNewMonolith(IntVec3 cell, Map map)
		{
			this.monolith = (ThingMaker.MakeThing(ThingDefOf.VoidMonolith, null) as Building_VoidMonolith);
			this.levelDef = DefDatabase<MonolithLevelDef>.AllDefs.FirstOrDefault((MonolithLevelDef x) => x.level == this.level);
			GenSpawn.Spawn(this.monolith, cell, map, WipeMode.Vanish);
			EffecterDefOf.VoidStructureSpawningSkipSequence.SpawnMaintained(this.monolith, map, 1f);
			if (this.Level > 0)
			{
				this.monolith.CheckAndGenerateQuest();
			}
			return this.monolith;
		}

		// Token: 0x06013CC2 RID: 81090 RVA: 0x005DFC53 File Offset: 0x005DDE53
		public void ResetMonolith()
		{
			this.SetLevel(MonolithLevelDefOf.Inactive, false);
			Building_VoidMonolith building_VoidMonolith = this.monolith;
			if (building_VoidMonolith != null)
			{
				building_VoidMonolith.Reset();
			}
			this.fireLevelIncidentTick = -99999;
			this.lastLevelChangeTick = -99999;
		}

		// Token: 0x06013CC3 RID: 81091 RVA: 0x005DFC88 File Offset: 0x005DDE88
		public bool VoidAwakeningActive()
		{
			foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
			{
				if (!quest.Historical && quest.root == QuestScriptDefOf.EndGame_VoidAwakening)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013CC4 RID: 81092 RVA: 0x005DFCF4 File Offset: 0x005DDEF4
		public bool PawnHasUnnaturalCorpse(Pawn pawn)
		{
			return this.corpseTrackers.ContainsKey(pawn);
		}

		// Token: 0x06013CC5 RID: 81093 RVA: 0x005DFD02 File Offset: 0x005DDF02
		public bool TryGetUnnaturalCorpseTrackerForHaunted(Pawn pawn, out UnnaturalCorpseTracker tracker)
		{
			return this.corpseTrackers.TryGetValue(pawn, out tracker);
		}

		// Token: 0x06013CC6 RID: 81094 RVA: 0x005DFD14 File Offset: 0x005DDF14
		public bool TryGetUnnaturalCorpseTrackerForAwoken(Pawn pawn, out UnnaturalCorpseTracker tracker)
		{
			foreach (KeyValuePair<Pawn, UnnaturalCorpseTracker> keyValuePair in this.corpseTrackers)
			{
				Pawn pawn2;
				UnnaturalCorpseTracker unnaturalCorpseTracker;
				keyValuePair.Deconstruct(out pawn2, out unnaturalCorpseTracker);
				UnnaturalCorpseTracker unnaturalCorpseTracker2 = unnaturalCorpseTracker;
				if (unnaturalCorpseTracker2.AwokenPawn == pawn)
				{
					tracker = unnaturalCorpseTracker2;
					return true;
				}
			}
			tracker = null;
			return false;
		}

		// Token: 0x06013CC7 RID: 81095 RVA: 0x005DFD88 File Offset: 0x005DDF88
		public bool HasActiveAwokenCorpse()
		{
			foreach (KeyValuePair<Pawn, UnnaturalCorpseTracker> keyValuePair in this.corpseTrackers)
			{
				Pawn pawn;
				UnnaturalCorpseTracker unnaturalCorpseTracker;
				keyValuePair.Deconstruct(out pawn, out unnaturalCorpseTracker);
				UnnaturalCorpseTracker unnaturalCorpseTracker2 = unnaturalCorpseTracker;
				if (!unnaturalCorpseTracker2.AwokenPawn.DestroyedOrNull() && unnaturalCorpseTracker2.AwokenPawn.Spawned && !unnaturalCorpseTracker2.Haunted.DestroyedOrNull() && !unnaturalCorpseTracker2.Haunted.Dead && unnaturalCorpseTracker2.Haunted.MapHeld == unnaturalCorpseTracker2.AwokenPawn.Map)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013CC8 RID: 81096 RVA: 0x005DFE38 File Offset: 0x005DE038
		public void RegisterUnnaturalCorpse(Pawn pawn, UnnaturalCorpse corpse)
		{
			if (this.corpseTrackers.ContainsKey(pawn))
			{
				Log.Error("Attempted to register a pawn (" + pawn.LabelShort + ") which already has an unnatural corpse");
				return;
			}
			this.corpseTrackers[pawn] = new UnnaturalCorpseTracker(pawn, corpse);
			Find.EntityCodex.SetDiscovered(EntityCodexEntryDefOf.UnnaturalCorpse, null, null);
		}

		// Token: 0x06013CC9 RID: 81097 RVA: 0x005DFE92 File Offset: 0x005DE092
		public void DevRemoveUnnaturalCorpse(Pawn pawn)
		{
			if (this.corpseTrackers.ContainsKey(pawn))
			{
				this.corpseTrackers[pawn].Notify_Finished();
				this.corpseTrackers.Remove(pawn);
			}
		}

		// Token: 0x06013CCA RID: 81098 RVA: 0x005DFEC0 File Offset: 0x005DE0C0
		public void Notify_MapRemoved(Map map)
		{
			if (this.monolith == null || this.monolith.Map != map)
			{
				return;
			}
			if (this.monolith.quest != null && !this.monolith.quest.Historical)
			{
				this.monolith.quest.End(QuestEndOutcome.Unknown, true, true);
			}
			if (this.levelDef == MonolithLevelDefOf.VoidAwakened || this.levelDef == MonolithLevelDefOf.Gleaming)
			{
				this.SetLevel(MonolithLevelDefOf.Waking, true);
			}
			foreach (Pawn pawn in this.metalHellPawns)
			{
				pawn.Destroy(DestroyMode.Vanish);
			}
			this.metalHellPawns.Clear();
			this.metalHellReturnTick = -99999;
			this.monolith = null;
			this.fireLevelIncidentTick = -99999;
		}

		// Token: 0x06013CCB RID: 81099 RVA: 0x005DFFA8 File Offset: 0x005DE1A8
		public void Notify_PawnDied(Pawn pawn)
		{
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (this.corpseTrackers.TryGetValue(pawn, out unnaturalCorpseTracker))
			{
				unnaturalCorpseTracker.Notify_PawnDied();
			}
		}

		// Token: 0x06013CCC RID: 81100 RVA: 0x005DFFCC File Offset: 0x005DE1CC
		public void Notify_PawnKilledViaAwoken(Pawn pawn)
		{
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (this.corpseTrackers.TryGetValue(pawn, out unnaturalCorpseTracker))
			{
				unnaturalCorpseTracker.Notify_PawnKilledViaAwoken();
			}
		}

		// Token: 0x06013CCD RID: 81101 RVA: 0x005DFFF0 File Offset: 0x005DE1F0
		private void TrySpawnHarbingerTrees()
		{
			IncidentParms parms = new IncidentParms
			{
				target = Find.AnyPlayerHomeMap,
				forced = true
			};
			Find.Storyteller.incidentQueue.Add(IncidentDefOf.HarbingerTreeSpawn, Find.TickManager.TicksGame, parms, 0);
		}

		// Token: 0x06013CCE RID: 81102 RVA: 0x005E0038 File Offset: 0x005DE238
		public void Notify_HarbingerTreeSpawned()
		{
			this.nextHarbingerTreeCheckTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + GameComponent_Anomaly.HarbingerTreeSpawnIntervalDays.RandomInRange * 60000f + GameComponent_Anomaly.HarbingerTreeCheckIntervalHours.RandomInRange * 2500f);
		}

		// Token: 0x06013CCF RID: 81103 RVA: 0x005E0084 File Offset: 0x005DE284
		public void Notify_HarbingerTreeDied()
		{
			this.nextHarbingerTreeCheckTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + 480000f + GameComponent_Anomaly.HarbingerTreeCheckIntervalHours.RandomInRange * 2500f);
		}

		// Token: 0x06013CD0 RID: 81104 RVA: 0x005E00C1 File Offset: 0x005DE2C1
		public override void AppendDebugString(StringBuilder sb)
		{
			sb.AppendLine("  " + base.GetType().Name + ":");
			sb.AppendLine("    level:  " + this.level.ToString());
		}

		// Token: 0x06013CD1 RID: 81105 RVA: 0x005E0100 File Offset: 0x005DE300
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.ElevateLooseCorpseTrackers();
				foreach (KeyValuePair<Pawn, int> keyValuePair in (from kvp in this.hypnotisedPawns
				where kvp.Key.DestroyedOrNull() || kvp.Key.Dead
				select kvp).ToList<KeyValuePair<Pawn, int>>())
				{
					Pawn pawn;
					int num;
					keyValuePair.Deconstruct(out pawn, out num);
					Pawn key = pawn;
					this.hypnotisedPawns.Remove(key);
				}
			}
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
			Scribe_Values.Look<int>(ref this.highestLevelReached, "highestLevelReached", 0, false);
			Scribe_Defs.Look<MonolithLevelDef>(ref this.levelDef, "levelDef");
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
			Scribe_Values.Look<int>(ref this.nextHarbingerTreeCheckTick, "nextHarbingerTreeCheckTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastLevelChangeTick, "lastLevelChangeTick", 0, false);
			Scribe_Values.Look<bool>(ref this.hasSeenGrayFlesh, "hasSeenGrayFlesh", false, false);
			Scribe_Values.Look<bool>(ref this.hasPerformedVoidProvocation, "hasPerformedVoidProvocation", false, false);
			Scribe_Values.Look<int>(ref this.lastLevelActivationLetterSent, "lastLevelActivationLetterSent", -1, false);
			Scribe_References.Look<Pawn>(ref this.voidNodeActivator, "voidNodeActivator", false);
			Scribe_Collections.Look<Pawn>(ref this.metalHellPawns, "metalHellPawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.metalHellReturnTick, "metalHellReturnTick", 0, false);
			Scribe_Values.Look<string>(ref this.metalHellReturnLetterText, "metalHellReturnLetterText", null, false);
			Scribe_Values.Look<int>(ref this.fireGrayPallTick, "fireGrayPallTick", 0, false);
			Scribe_Values.Look<int>(ref this.newestMetalhorrorBiosignatureTick, "newestMetalhorrorBiosignatureTick", 0, false);
			Scribe_Values.Look<bool>(ref this.hasBuiltHoldingPlatform, "hasBuiltHoldingPlatform", false, false);
			Scribe_Collections.Look<Pawn, UnnaturalCorpseTracker>(ref this.corpseTrackers, "corpses", LookMode.Reference, LookMode.Deep, ref this.workingPawnList, ref this.workingTrackerList, true, false, false);
			Scribe_Collections.Look<UnnaturalCorpseTracker>(ref this.looseCorpseTrackers, "looseCorpseTrackers", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn, int>(ref this.hypnotisedPawns, "hypnotisedPawns", LookMode.Reference, LookMode.Value, ref this.workingHypnotizedList, ref this.workingHypnotizedTickList, true, false, false);
			Scribe_Collections.Look<int>(ref this.emergedBiosignatures, "emergedBiosignatures", LookMode.Value);
			Scribe_Collections.Look<ChoiceLetter>(ref this.monolithLetters, "monolithLetters", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.monolithStudyProgress, "monolithStudyProgress", 0, false);
			Scribe_Values.Look<int>(ref this.monolithNextIndex, "monolithNextIndex", 0, false);
			Scribe_Values.Look<float>(ref this.monolithAnomalyKnowledge, "monolithAnomalyKnowledge", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.looseCorpseTrackers == null)
			{
				this.looseCorpseTrackers = new List<UnnaturalCorpseTracker>();
			}
		}

		// Token: 0x06013CD2 RID: 81106 RVA: 0x005E0398 File Offset: 0x005DE598
		private void ElevateLooseCorpseTrackers()
		{
			foreach (KeyValuePair<Pawn, UnnaturalCorpseTracker> keyValuePair in this.corpseTrackers)
			{
				Pawn pawn;
				UnnaturalCorpseTracker unnaturalCorpseTracker;
				keyValuePair.Deconstruct(out pawn, out unnaturalCorpseTracker);
				Pawn pawn2 = pawn;
				if (pawn2.Dead || pawn2.DestroyedOrNull())
				{
					GameComponent_Anomaly.toRemove.Add(pawn2);
				}
			}
			foreach (Pawn key in GameComponent_Anomaly.toRemove)
			{
				this.looseCorpseTrackers.Add(this.corpseTrackers[key]);
				this.corpseTrackers.Remove(key);
			}
			GameComponent_Anomaly.toRemove.Clear();
		}

		// Token: 0x0400DB31 RID: 56113
		public const string MonolithLevelChangedSignal = "MonolithLevelChanged";

		// Token: 0x0400DB32 RID: 56114
		private const int GrayPallDelayTicks = 300;

		// Token: 0x0400DB33 RID: 56115
		private const int LevelChangeScreenShakeDuration = 300;

		// Token: 0x0400DB34 RID: 56116
		private const int MinTimeBetweenNewMetalhorrorBiosignatureImplants = 1800000;

		// Token: 0x0400DB35 RID: 56117
		private const int MonolithStudyNoteLetters = 3;

		// Token: 0x0400DB36 RID: 56118
		private const float LevelChangeScreenShakeMagnitude = 0.05f;

		// Token: 0x0400DB37 RID: 56119
		private const float HarbingerTreeRespawnIntervalDays = 8f;

		// Token: 0x0400DB38 RID: 56120
		public const int PostEndGameReliefPeriod = 300000;

		// Token: 0x0400DB39 RID: 56121
		private static readonly FloatRange MonolithLevelIncidentDelayRangeHours = new FloatRange(12f, 36f);

		// Token: 0x0400DB3A RID: 56122
		private static readonly FloatRange HarbingerTreeCheckIntervalHours = new FloatRange(6f, 18f);

		// Token: 0x0400DB3B RID: 56123
		private static readonly FloatRange HarbingerTreeSpawnIntervalDays = new FloatRange(2f, 5f);

		// Token: 0x0400DB3C RID: 56124
		private static readonly FloatRange GrayPallConditionDaysRange = new FloatRange(1f, 3f);

		// Token: 0x0400DB3D RID: 56125
		private static readonly FloatRange VoidCuriosityIncidentDelayRangeDays = new FloatRange(15f, 20f);

		// Token: 0x0400DB3E RID: 56126
		private int level;

		// Token: 0x0400DB3F RID: 56127
		private int highestLevelReached;

		// Token: 0x0400DB40 RID: 56128
		private MonolithLevelDef levelDef;

		// Token: 0x0400DB41 RID: 56129
		public Building_VoidMonolith monolith;

		// Token: 0x0400DB42 RID: 56130
		private int fireLevelIncidentTick = -99999;

		// Token: 0x0400DB43 RID: 56131
		private int nextHarbingerTreeCheckTick = -99999;

		// Token: 0x0400DB44 RID: 56132
		private int lastLevelChangeTick = -99999;

		// Token: 0x0400DB45 RID: 56133
		private int fireGrayPallTick = -99999;

		// Token: 0x0400DB46 RID: 56134
		private int newestMetalhorrorBiosignatureTick = -1800000;

		// Token: 0x0400DB47 RID: 56135
		public bool hasBuiltHoldingPlatform;

		// Token: 0x0400DB48 RID: 56136
		public bool hasSeenGrayFlesh;

		// Token: 0x0400DB49 RID: 56137
		private Dictionary<Pawn, UnnaturalCorpseTracker> corpseTrackers = new Dictionary<Pawn, UnnaturalCorpseTracker>();

		// Token: 0x0400DB4A RID: 56138
		private List<UnnaturalCorpseTracker> looseCorpseTrackers = new List<UnnaturalCorpseTracker>();

		// Token: 0x0400DB4B RID: 56139
		private Dictionary<Pawn, int> hypnotisedPawns = new Dictionary<Pawn, int>();

		// Token: 0x0400DB4C RID: 56140
		public HashSet<int> emergedBiosignatures = new HashSet<int>();

		// Token: 0x0400DB4D RID: 56141
		public bool hasPerformedVoidProvocation;

		// Token: 0x0400DB4E RID: 56142
		public int lastLevelActivationLetterSent = -1;

		// Token: 0x0400DB4F RID: 56143
		private List<ChoiceLetter> monolithLetters = new List<ChoiceLetter>();

		// Token: 0x0400DB50 RID: 56144
		private int monolithNextIndex;

		// Token: 0x0400DB51 RID: 56145
		private int monolithStudyProgress;

		// Token: 0x0400DB52 RID: 56146
		public float monolithAnomalyKnowledge;

		// Token: 0x0400DB53 RID: 56147
		public Pawn voidNodeActivator;

		// Token: 0x0400DB54 RID: 56148
		public List<Pawn> metalHellPawns = new List<Pawn>();

		// Token: 0x0400DB55 RID: 56149
		public int metalHellReturnTick = -99999;

		// Token: 0x0400DB56 RID: 56150
		public int metalHellClosedTick = -99999;

		// Token: 0x0400DB57 RID: 56151
		public string metalHellReturnLetterText;

		// Token: 0x0400DB58 RID: 56152
		private LargeBuildingSpawnParms monolithSpawnParms;

		// Token: 0x0400DB59 RID: 56153
		private static readonly List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x0400DB5A RID: 56154
		private static readonly List<UnnaturalCorpseTracker> tmpTrackers = new List<UnnaturalCorpseTracker>();

		// Token: 0x0400DB5B RID: 56155
		private List<Pawn> workingPawnList;

		// Token: 0x0400DB5C RID: 56156
		private List<UnnaturalCorpseTracker> workingTrackerList;

		// Token: 0x0400DB5D RID: 56157
		private List<Pawn> workingHypnotizedList;

		// Token: 0x0400DB5E RID: 56158
		private List<int> workingHypnotizedTickList;
	}
}
