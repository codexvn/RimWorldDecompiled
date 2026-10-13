using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200033B RID: 827
	public class UnnaturalCorpseTracker : IExposable, ILoadReferenceable
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x00062607 File Offset: 0x00060807
		public UnnaturalCorpse Corpse
		{
			get
			{
				return this.corpse;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x0006260F File Offset: 0x0006080F
		public bool CorpseSpawned
		{
			get
			{
				return this.corpse != null;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0006261A File Offset: 0x0006081A
		public Pawn AwokenPawn
		{
			get
			{
				return this.awokenPawn;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060014E5 RID: 5349 RVA: 0x00062622 File Offset: 0x00060822
		public int SpawnedTicks
		{
			get
			{
				return GenTicks.TicksGame - this.spawnedTick;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x00062630 File Offset: 0x00060830
		public int TicksToKill
		{
			get
			{
				return this.ticksToKill;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060014E7 RID: 5351 RVA: 0x00062638 File Offset: 0x00060838
		public bool ShouldDisappear
		{
			get
			{
				return this.awokenFinished || (this.disappearTick != 0 && GenTicks.TicksGame >= this.disappearTick);
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0006265E File Offset: 0x0006085E
		public Pawn Haunted
		{
			get
			{
				return this.haunted;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x00062666 File Offset: 0x00060866
		public bool CanDestroyViaResearch
		{
			get
			{
				return this.research >= this.researchToDestroy;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x00062679 File Offset: 0x00060879
		public IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00062681 File Offset: 0x00060881
		public UnnaturalCorpseTracker()
		{
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00062694 File Offset: 0x00060894
		public UnnaturalCorpseTracker(Pawn haunted, UnnaturalCorpse corpse)
		{
			this.haunted = haunted;
			this.corpse = corpse;
			this.loadID = haunted.thingIDNumber;
			this.researchToDestroy = UnnaturalCorpseTracker.DestroyResearchRequiredRange.RandomInRange;
			this.awakenTick = GenTicks.TicksGame + Mathf.CeilToInt(UnnaturalCorpseTracker.GrowingLetterDelayDays.RandomInRange * 60000f);
			this.spawnedTick = GenTicks.TicksGame;
			corpse.LinkToTracker(this);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00062718 File Offset: 0x00060918
		public void Notify_Finished()
		{
			this.removing = true;
			if (this.awokenPawn != null && this.awokenPawn.Spawned)
			{
				Messages.Message("MessageAwokenDisappeared".Translate(), MessageTypeDefOf.NeutralEvent, true);
				SkipUtility.SkipDeSpawn(this.awokenPawn);
				this.awokenPawn.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.Corpse != null)
			{
				this.Corpse.DoDisappear();
			}
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00062786 File Offset: 0x00060986
		public void Notify_CorpseDestroyed()
		{
			if (!this.removing && !this.Haunted.DestroyedOrNull())
			{
				this.applyThoughtNextTick = true;
			}
			this.lastSpawnInCheck = GenTicks.TicksGame;
			this.corpse = null;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x000627B8 File Offset: 0x000609B8
		public void Notify_AwokenAttackStarting()
		{
			if (this.awokenPawn == null)
			{
				return;
			}
			Hediff hd;
			if (!this.awokenPawn.health.hediffSet.TryGetHediff(HediffDefOf.AwokenCorpse, out hd))
			{
				return;
			}
			hd.TryGetComp<HediffComp_MessageStageIncreased>().sendMessages = false;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x000627FC File Offset: 0x000609FC
		private void ApplyCorpseDestroyedThoughts()
		{
			this.applyThoughtNextTick = false;
			Pawn_NeedsTracker needs = this.haunted.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.UnnaturalCorpseDestroyed, null, null);
						}
					}
				}
			}
			Hediff hd;
			if (this.haunted.health.hediffSet.TryGetHediff(HediffDefOf.CorpseTorment, out hd))
			{
				hd.TryGetComp<HediffComp_Disappears>().ResetElapsedTicks();
				return;
			}
			this.haunted.health.AddHediff(HediffDefOf.CorpseTorment, null, null, null);
			Messages.Message("MessagePawnFainted".Translate(this.Haunted.Named("PAWN")), this.Haunted, MessageTypeDefOf.NegativeHealthEvent, true);
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x000628CE File Offset: 0x00060ACE
		public void Notify_PawnDied()
		{
			if (this.disappearTick != 0)
			{
				return;
			}
			if (this.awoken)
			{
				this.disappearTick = GenTicks.TicksGame + 600;
				return;
			}
			this.disappearTick = GenTicks.TicksGame + 7500;
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00062904 File Offset: 0x00060B04
		public void Notify_PawnKilledViaAwoken()
		{
			this.disappearTick = GenTicks.TicksGame + 600;
			Messages.Message("MessageAwokenKilledVictim".Translate(this.Haunted.Named("PAWN")), this.Haunted, MessageTypeDefOf.NegativeEvent, true);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00062958 File Offset: 0x00060B58
		public void CorpseTick()
		{
			if (this.disappearTick == 0 && this.Haunted.SpawnedOrAnyParentSpawned && this.Haunted.IsMutant)
			{
				this.disappearTick = GenTicks.TicksGame + 7500;
			}
			if (this.awoken)
			{
				this.AwokenTick();
				return;
			}
			if (this.applyThoughtNextTick)
			{
				this.ApplyCorpseDestroyedThoughts();
			}
			this.UpdateCorpseState();
			this.UpdateMentalBreak();
			this.AwakeningTick();
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000629C8 File Offset: 0x00060BC8
		private void UpdateMentalBreak()
		{
			if (!this.Haunted.SpawnedOrAnyParentSpawned || !this.CorpseSpawned)
			{
				return;
			}
			if (!this.mentalBreak && !this.Haunted.InMentalState && GenTicks.TicksGame >= this.minMentalBreakRefireTick && Rand.MTBEventOccurs(5f, 60000f, 1f))
			{
				this.mentalBreak = true;
			}
			if (this.mentalBreak)
			{
				this.TryTriggerMentalBreak();
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00062A38 File Offset: 0x00060C38
		public void TryTriggerMentalBreak()
		{
			if (MentalBreakDefOf.CorpseObsession.Worker.BreakCanOccur(this.Haunted))
			{
				this.mentalBreak = false;
				this.minMentalBreakRefireTick = GenTicks.TicksGame + 150000;
				this.Haunted.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_MysteriousCorpse".Translate(), MentalBreakDefOf.CorpseObsession);
				return;
			}
			MentalBreakDef breakDef;
			if (this.Haunted.mindState.mentalBreaker.TryGetRandomMentalBreak(MentalBreakIntensity.Major, out breakDef))
			{
				this.mentalBreak = false;
				this.minMentalBreakRefireTick = GenTicks.TicksGame + 150000;
				this.Haunted.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_MysteriousCorpse".Translate(), breakDef);
			}
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00062AF8 File Offset: 0x00060CF8
		private void AwokenTick()
		{
			if (this.awokenFinished)
			{
				return;
			}
			if (this.awokenPawn != null)
			{
				if (this.awokenPawn.Downed && !this.awokenPawn.Dead && !this.awokenPawn.health.hediffSet.HasHediff(HediffDefOf.RapidRegeneration, false))
				{
					this.awokenPawn.Kill(null, null);
				}
				if (this.awokenPawn.Dead)
				{
					TaggedString label = "UnnaturalCorpseAwokenDefeatedLetter".Translate();
					TaggedString text = "UnnaturalCorpseAwokenDefeatedLetterDesc".Translate(this.Haunted.Named("PAWN"));
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, this.awokenPawn, null, null, null, null, 0, true);
					if (this.awokenPawn.SpawnedOrAnyParentSpawned)
					{
						Thing thing;
						GenDrop.TryDropSpawn(ThingMaker.MakeThing(ThingDefOf.Shard, null), this.awokenPawn.PositionHeld, this.awokenPawn.MapHeld, ThingPlaceMode.Near, out thing, null, null, true);
						FleshbeastUtility.MeatSplatter(3, this.awokenPawn.PositionHeld, this.awokenPawn.MapHeld, FleshbeastUtility.MeatExplosionSize.Normal);
						this.awokenPawn.Corpse.Destroy(DestroyMode.Vanish);
					}
					this.awokenFinished = true;
					return;
				}
			}
			if ((this.awokenPawn.DestroyedOrNull() || !this.awokenPawn.SpawnedOrAnyParentSpawned) && this.Haunted.SpawnedOrAnyParentSpawned)
			{
				if (!this.teleportIn)
				{
					int num = Mathf.Max(GenTicks.TicksGame - this.lastSpawnInCheck, 1);
					this.lastSpawnInCheck = GenTicks.TicksGame;
					if (Rand.MTBEventOccurs(6f, 2500f, (float)num))
					{
						this.teleportIn = true;
					}
				}
				IntVec3 cell;
				if (this.teleportIn && CellFinder.TryFindRandomReachableNearbyCell(this.Haunted.PositionHeld, this.Haunted.MapHeld, 70f, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false), new Predicate<IntVec3>(this.IsValidAwokenSpawnCell), null, out cell, 999999))
				{
					this.teleportIn = false;
					this.awokenPawn = this.MakeNewAwoken();
					SkipUtility.SkipTo(this.awokenPawn, cell, this.Haunted.MapHeld);
					this.TurnAwoken(this.awokenPawn);
					Messages.Message("MessageAwokenReappeared".Translate(this.Haunted.Named("PAWN")), this.awokenPawn, MessageTypeDefOf.NegativeEvent, true);
					Find.MusicManagerPlay.CheckTransitions();
					return;
				}
			}
			else if (!this.awokenPawn.DestroyedOrNull() && this.awokenPawn.Spawned && !this.Haunted.DestroyedOrNull() && this.Haunted.MapHeld != this.awokenPawn.Map)
			{
				SkipUtility.SkipDeSpawn(this.awokenPawn);
				this.awokenPawn.Destroy(DestroyMode.Vanish);
				this.awokenPawn = null;
				Messages.Message("MessageAwokenVanished".Translate(this.Haunted.Named("PAWN")), this.awokenPawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00062DFC File Offset: 0x00060FFC
		private bool IsValidAwokenSpawnCell(IntVec3 cell)
		{
			Map mapHeld = this.Haunted.MapHeld;
			return cell.Standable(mapHeld) && !cell.Fogged(mapHeld);
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00062E2C File Offset: 0x0006102C
		private void UpdateCorpseState()
		{
			if (this.Haunted.Dead)
			{
				return;
			}
			if (this.corpse == null && this.Haunted.SpawnedOrAnyParentSpawned)
			{
				if (!this.teleportIn)
				{
					int num = Mathf.Max(GenTicks.TicksGame - this.lastSpawnInCheck, 1);
					this.lastSpawnInCheck = GenTicks.TicksGame;
					if (Rand.MTBEventOccurs(24f, 2500f, (float)num))
					{
						this.teleportIn = true;
					}
				}
				IntVec3 cell;
				if (this.teleportIn && AnomalyUtility.TryGetNearbyUnseenCell(this.Haunted, out cell))
				{
					this.SpawnNewCorpse(cell);
				}
			}
			if (this.corpse != null && this.InEscapableContainer())
			{
				if (!this.escape && Rand.MTBEventOccurs(24f, 2500f, 1f))
				{
					this.escape = true;
				}
				if (this.escape && AnomalyUtility.IsValidUnseenCell(Find.CameraDriver.CurrentViewRect.ExpandedBy(1), this.corpse.PositionHeld, this.corpse.MapHeld))
				{
					this.BreakoutOfContainer();
				}
			}
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00062F30 File Offset: 0x00061130
		private void AwakeningTick()
		{
			if (this.CorpseSpawned && this.rising && this.Corpse.Spawned)
			{
				if (this.riseSustainer == null || this.riseSustainer.Ended)
				{
					SoundInfo info = SoundInfo.InMap(this.Corpse, MaintenanceType.PerTick);
					this.riseSustainer = SoundDefOf.Pawn_Shambler_Rise.TrySpawnSustainer(info);
				}
				if (this.riseEffecter == null)
				{
					this.riseEffecter = EffecterDefOf.ShamblerRaise.Spawn(this.Corpse, this.Corpse.Map, 1f);
				}
				this.riseSustainer.Maintain();
				this.riseEffecter.EffectTick(this.Corpse, TargetInfo.Invalid);
			}
			if (this.awoken || GenTicks.TicksGame <= this.awakenTick)
			{
				return;
			}
			if (this.CorpseSpawned && this.Haunted.SpawnedOrAnyParentSpawned && !this.awoken && !this.sentAwakeningLetter)
			{
				this.sentAwakeningLetter = true;
				this.awakenTick = GenTicks.TicksGame + Mathf.CeilToInt(UnnaturalCorpseTracker.AwakenDelayDays.RandomInRange * 60000f);
				TaggedString label = "UnnaturalCorpseAwakeningLetter".Translate();
				TaggedString taggedString = "UnnaturalCorpseAwakeningLetterDesc".Translate(this.Haunted.Named("PAWN"));
				if (this.CanDestroyViaResearch)
				{
					taggedString += "\n\n" + "UnnaturalCorpseAwakeningLetterExtra_StudyComplete".Translate();
				}
				else
				{
					taggedString += "\n\n" + "UnnaturalCorpseAwakeningLetterExtra_StudyIncomplete".Translate();
				}
				Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, this.corpse, null, null, null, null, 0, true);
				return;
			}
			if (this.CorpseSpawned && this.Haunted.SpawnedOrAnyParentSpawned && this.Corpse.MapHeld == this.Haunted.MapHeld && !this.Haunted.Downed && this.Haunted.Awake() && !this.rising)
			{
				this.rising = true;
				this.awakenTick = GenTicks.TicksGame + 420;
				this.Corpse.InnerPawn.Drawer.renderer.SetAnimation(AnimationDefOf.ShamblerRise);
				TaggedString label2 = "UnnaturalCorpseAwokenLetter".Translate();
				TaggedString text = "UnnaturalCorpseAwokenLetterDesc".Translate(this.Haunted.Named("PAWN"));
				Find.LetterStack.ReceiveLetter(label2, text, LetterDefOf.ThreatBig, this.awokenPawn, null, null, null, null, 0, true);
				Find.TickManager.slower.SignalForceNormalSpeed();
				return;
			}
			if (this.CorpseSpawned)
			{
				this.awoken = true;
				IntVec3 positionHeld = this.Corpse.PositionHeld;
				this.Corpse.Destroy(DestroyMode.Vanish);
				this.awokenPawn = this.MakeNewAwoken();
				GenSpawn.Spawn(this.awokenPawn, positionHeld, this.Haunted.MapHeld, WipeMode.Vanish);
				this.TurnAwoken(this.awokenPawn);
				this.awokenPawn.Drawer.renderer.SetAnimation(null);
				float numSeconds = UnnaturalCorpseTracker.CombatPointsSecondsToKillCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.Haunted.MapHeld));
				this.ticksToKill = numSeconds.SecondsToTicks();
				Find.MusicManagerPlay.CheckTransitions();
			}
			this.awakenTick = GenTicks.TicksGame + 2500;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00063298 File Offset: 0x00061498
		private void SpawnNewCorpse(IntVec3 cell)
		{
			this.teleportIn = false;
			UnnaturalCorpse unnaturalCorpse = AnomalyUtility.MakeUnnaturalCorpse(this.Haunted);
			unnaturalCorpse.LinkToTracker(this);
			GenSpawn.Spawn(unnaturalCorpse, cell, this.Haunted.MapHeld, WipeMode.Vanish);
			this.spawnedTick = GenTicks.TicksGame;
			this.corpse = unnaturalCorpse;
			this.corpse.Forbiddable.Forbidden = true;
			TaggedString label = "LetterLabelCorpseReappeared".Translate();
			TaggedString text = "LetterCorpseReappeared".Translate(this.Haunted.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, this.corpse, null, null, null, null, 0, true);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0006333E File Offset: 0x0006153E
		private Pawn MakeNewAwoken()
		{
			Pawn pawn = Find.PawnDuplicator.Duplicate(this.Haunted);
			pawn.apparel.DestroyAll(DestroyMode.Vanish);
			return pawn;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0006335C File Offset: 0x0006155C
		private void TurnAwoken(Pawn pawn)
		{
			MutantUtility.SetPawnAsMutantInstantly(pawn, MutantDefOf.AwokenCorpse, pawn.GetRotStage());
			if (pawn.Faction != Faction.OfEntities)
			{
				pawn.SetFaction(Faction.OfEntities, null);
			}
			pawn.health.hediffSet.GetFirstHediff<Hediff_RapidRegeneration>().SetHpCapacity(UnnaturalCorpseTracker.CombatPointsPerHpRegenCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.Haunted.MapHeld)));
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(AbilityDefOf.UnnaturalCorpseSkip, true) : null;
			if (ability != null)
			{
				ability.StartCooldown(ability.def.cooldownTicksRange.RandomInRange);
			}
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x000633F4 File Offset: 0x000615F4
		private void BreakoutOfContainer()
		{
			if (!this.InEscapableContainer())
			{
				return;
			}
			this.escape = false;
			Thing thing = ThingOwnerUtility.SpawnedParentOrMe(this.corpse);
			Messages.Message("MessageCorpseEscaped".Translate(this.Haunted.Named("PAWN"), thing.Named("CONTAINER")), this.corpse, MessageTypeDefOf.NeutralEvent, true);
			Building_Casket building_Casket = thing as Building_Casket;
			if (building_Casket != null)
			{
				building_Casket.EjectContents();
			}
			else
			{
				Thing thing2;
				this.corpse.ParentHolder.GetDirectlyHeldThings().TryDrop(this.corpse, ThingPlaceMode.Near, 1, out thing2, null, null);
			}
			this.corpse.Forbiddable.Forbidden = true;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x000634A4 File Offset: 0x000616A4
		public void Notify_Studied(Pawn studier, float amount)
		{
			this.research += amount;
			if (this.research >= 3f && !this.sentLetterOne)
			{
				this.sentLetterOne = true;
				this.SendLetter(studier, "UnnaturalCorpseStudyLetter", "UnnaturalCorpseStudyLetterDesc");
				return;
			}
			if (this.CanDestroyViaResearch && !this.sentLetterTwo)
			{
				this.sentLetterTwo = true;
				this.SendLetter(studier, "UnnaturalCorpseStudyCompletedLetter", "UnnaturalCorpseStudyCompletedLetterDesc");
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00063518 File Offset: 0x00061718
		private void SendLetter(Pawn studier, string labelKey, string descKey)
		{
			TaggedString label = labelKey.Translate();
			TaggedString text = descKey.Translate(this.haunted.Named("VICTIM"), studier.Named("RESEARCHER"));
			Thing t = this.corpse.Spawned ? this.corpse : this.haunted;
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, t, null, null, null, null, 0, true);
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, t, null, null, null);
			choiceLetter.arrivalTick = Find.TickManager.TicksGame;
			this.letters.Add(choiceLetter);
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x000635B9 File Offset: 0x000617B9
		public void DevAwaken()
		{
			this.sentAwakeningLetter = true;
			this.awakenTick = GenTicks.TicksGame;
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x000635CD File Offset: 0x000617CD
		public void DevUnlockDeactivation()
		{
			this.sentLetterOne = true;
			this.sentLetterTwo = true;
			this.research += this.researchToDestroy;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x000635F0 File Offset: 0x000617F0
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.haunted, "haunted", false);
			Scribe_References.Look<Pawn>(ref this.awokenPawn, "awokenPawn", false);
			Scribe_References.Look<UnnaturalCorpse>(ref this.corpse, "corpse", true);
			Scribe_Values.Look<int>(ref this.disappearTick, "disappearTick", 0, false);
			Scribe_Values.Look<int>(ref this.awakenTick, "awakenTick", 0, false);
			Scribe_Values.Look<bool>(ref this.sentAwakeningLetter, "sentAwakeningLetter", false, false);
			Scribe_Values.Look<int>(ref this.minMentalBreakRefireTick, "minMentalBreakRefireTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastSpawnInCheck, "lastSpawnInCheck", 0, false);
			Scribe_Values.Look<bool>(ref this.teleportIn, "teleportIn", false, false);
			Scribe_Values.Look<bool>(ref this.escape, "escape", false, false);
			Scribe_Values.Look<bool>(ref this.mentalBreak, "mentalBreak", false, false);
			Scribe_Values.Look<bool>(ref this.removing, "removing", false, false);
			Scribe_Values.Look<float>(ref this.research, "research", 0f, false);
			Scribe_Values.Look<float>(ref this.researchToDestroy, "researchToDestroy", 0f, false);
			Scribe_Values.Look<bool>(ref this.sentLetterOne, "sentLetterOne", false, false);
			Scribe_Values.Look<bool>(ref this.sentLetterTwo, "sentLetterTwo", false, false);
			Scribe_Values.Look<bool>(ref this.awoken, "awoken", false, false);
			Scribe_Values.Look<bool>(ref this.awokenFinished, "awokenFinished", false, false);
			Scribe_Values.Look<bool>(ref this.rising, "rising", false, false);
			Scribe_Values.Look<int>(ref this.ticksToKill, "ticksToKill", 0, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Collections.Look<ChoiceLetter>(ref this.letters, "letters", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00063792 File Offset: 0x00061992
		public string GetUniqueLoadID()
		{
			return string.Format("MysteriousCorpseTracker_{0}", this.loadID);
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000637AC File Offset: 0x000619AC
		private bool InEscapableContainer()
		{
			if (this.corpse == null)
			{
				return false;
			}
			if (this.corpse.ParentHolder == null || this.corpse.ParentHolder is Map)
			{
				return false;
			}
			Building building = this.corpse.ParentHolder as Building;
			return (building != null && building.def.building.isEscapableContainer) || this.corpse.ParentHolder is CompTransporter;
		}

		// Token: 0x0400137B RID: 4987
		private static readonly FloatRange GrowingLetterDelayDays = new FloatRange(11f, 13f);

		// Token: 0x0400137C RID: 4988
		private static readonly FloatRange AwakenDelayDays = new FloatRange(1f, 3f);

		// Token: 0x0400137D RID: 4989
		private static readonly FloatRange DestroyResearchRequiredRange = new FloatRange(8f, 12f);

		// Token: 0x0400137E RID: 4990
		private const float FirstLetterResearchRequired = 3f;

		// Token: 0x0400137F RID: 4991
		private const int PawnDiedDisappearHours = 3;

		// Token: 0x04001380 RID: 4992
		private const int PawnKilledDisappearTicks = 600;

		// Token: 0x04001381 RID: 4993
		private const int MTBReappearHoursCorpse = 24;

		// Token: 0x04001382 RID: 4994
		private const int MTBReappearHoursAwoken = 6;

		// Token: 0x04001383 RID: 4995
		private const int MTBMentalBreakDays = 5;

		// Token: 0x04001384 RID: 4996
		private const int MTBEscapeContainerHours = 24;

		// Token: 0x04001385 RID: 4997
		private const int MinMentalBreakRefireTicks = 150000;

		// Token: 0x04001386 RID: 4998
		private const int RisingTime = 420;

		// Token: 0x04001387 RID: 4999
		private static readonly SimpleCurve CombatPointsPerHpRegenCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 150f),
				true
			},
			{
				new CurvePoint(3000f, 800f),
				true
			}
		};

		// Token: 0x04001388 RID: 5000
		private static readonly SimpleCurve CombatPointsSecondsToKillCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 25f),
				true
			},
			{
				new CurvePoint(3000f, 10f),
				true
			}
		};

		// Token: 0x04001389 RID: 5001
		private UnnaturalCorpse corpse;

		// Token: 0x0400138A RID: 5002
		private Pawn awokenPawn;

		// Token: 0x0400138B RID: 5003
		private Pawn haunted;

		// Token: 0x0400138C RID: 5004
		private int disappearTick;

		// Token: 0x0400138D RID: 5005
		private int lastSpawnInCheck;

		// Token: 0x0400138E RID: 5006
		private int loadID;

		// Token: 0x0400138F RID: 5007
		private int awakenTick;

		// Token: 0x04001390 RID: 5008
		private int spawnedTick;

		// Token: 0x04001391 RID: 5009
		private int minMentalBreakRefireTick;

		// Token: 0x04001392 RID: 5010
		private int ticksToKill;

		// Token: 0x04001393 RID: 5011
		private float research;

		// Token: 0x04001394 RID: 5012
		private float researchToDestroy;

		// Token: 0x04001395 RID: 5013
		private bool teleportIn;

		// Token: 0x04001396 RID: 5014
		private bool escape;

		// Token: 0x04001397 RID: 5015
		private bool mentalBreak;

		// Token: 0x04001398 RID: 5016
		private bool removing;

		// Token: 0x04001399 RID: 5017
		private bool sentLetterOne;

		// Token: 0x0400139A RID: 5018
		private bool sentLetterTwo;

		// Token: 0x0400139B RID: 5019
		private bool sentAwakeningLetter;

		// Token: 0x0400139C RID: 5020
		private bool awoken;

		// Token: 0x0400139D RID: 5021
		private bool rising;

		// Token: 0x0400139E RID: 5022
		private bool awokenFinished;

		// Token: 0x0400139F RID: 5023
		private List<ChoiceLetter> letters = new List<ChoiceLetter>();

		// Token: 0x040013A0 RID: 5024
		private bool applyThoughtNextTick;

		// Token: 0x040013A1 RID: 5025
		private Effecter riseEffecter;

		// Token: 0x040013A2 RID: 5026
		private Sustainer riseSustainer;
	}
}
