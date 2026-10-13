using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200076B RID: 1899
	public class Hediff_Shambler : HediffWithComps
	{
		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002E8B RID: 11915 RVA: 0x000EF4D9 File Offset: 0x000ED6D9
		public bool IsRising
		{
			get
			{
				return this.pawn.health.hediffSet.HasHediff(HediffDefOf.Rising, false);
			}
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000EF4F8 File Offset: 0x000ED6F8
		public override void PostMake()
		{
			base.PostMake();
			this.headRotation = Rand.RangeSeeded(-20f, 20f, this.pawn.thingIDNumber);
			if (!this.pawn.Dead)
			{
				this.pawn.timesRaisedAsShambler++;
			}
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000EF54B File Offset: 0x000ED74B
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Shambler"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000EF572 File Offset: 0x000ED772
		public override void Notify_Spawned()
		{
			base.Notify_Spawned();
			this.pawn.Map.mapPawns.RegisterShambler(this.pawn);
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x000EF598 File Offset: 0x000ED798
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.IsRising)
			{
				if ((float)Find.TickManager.TicksGame > this.resurrectTimer)
				{
					this.FinishRising(true);
				}
				if (this.pawn.Spawned)
				{
					if ((float)Find.TickManager.TicksGame > this.resurrectTimer - 15f)
					{
						Sustainer sustainer = this.riseSustainer;
						if (sustainer == null)
						{
							return;
						}
						sustainer.End();
						return;
					}
					else if (this.IsRising)
					{
						if (this.riseSustainer == null || this.riseSustainer.Ended)
						{
							SoundInfo info = SoundInfo.InMap(this.pawn, MaintenanceType.PerTick);
							this.riseSustainer = SoundDefOf.Pawn_Shambler_Rise.TrySpawnSustainer(info);
						}
						if (this.riseEffecter == null)
						{
							this.riseEffecter = EffecterDefOf.ShamblerRaise.Spawn(this.pawn, this.pawn.Map, 1f);
						}
						if (this.pawn.Drawer.renderer.CurAnimation != AnimationDefOf.ShamblerRise)
						{
							this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.ShamblerRise);
						}
						this.riseSustainer.Maintain();
						this.riseEffecter.EffectTick(this.pawn, TargetInfo.Invalid);
					}
				}
				return;
			}
			if (Rand.MTBEventOccurs(1f, 60f, 1f))
			{
				FleckMaker.ThrowShamblerParticles(this.pawn);
			}
			if (this.pawn.IsBurning() && !this.pawn.Downed && Rand.MTBEventOccurs(45f, 60f, 1f))
			{
				((Fire)this.pawn.GetAttachment(ThingDefOf.Fire)).Destroy(DestroyMode.Vanish);
				this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
			}
			if (this.pawn.Spawned && !this.pawn.mutant.IsPassive && !this.pawn.Drafted)
			{
				if (Find.TickManager.TicksGame > this.nextTargetCheckTick)
				{
					this.nextTargetCheckTick = Find.TickManager.TicksGame + Hediff_Shambler.CheckForTargetTicksInterval.RandomInRange;
					Thing thing = MutantUtility.FindShamblerTarget(this.pawn);
					if (thing != null)
					{
						this.Notify_DelayedAlert(thing);
						MutantUtility.ActivateNearbyShamblers(this.pawn, thing);
					}
				}
				if (this.alertedTarget != null && (float)Find.TickManager.TicksGame > this.alertTimer)
				{
					this.pawn.mindState.enemyTarget = this.alertedTarget;
					this.pawn.mindState.Notify_EngagedTarget();
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					this.alertedTarget = null;
					if (DebugViewSettings.drawShamblerAlertMote)
					{
						MoteMaker.MakeColonistActionOverlay(this.pawn, ThingDefOf.Mote_ShamblerAlert);
					}
					SoundDefOf.Pawn_Shambler_Alert.PlayOneShot(this.pawn);
				}
			}
			if (this.ShouldSelfRaise())
			{
				this.StartRising(-1);
			}
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x000EF87C File Offset: 0x000EDA7C
		private bool ShouldSelfRaise()
		{
			return this.pawn.DevelopmentalStage != DevelopmentalStage.Baby && (this.pawn.Downed && this.pawn.CarriedBy == null) && (float)Find.TickManager.TicksGame > this.selfRaiseTimer;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x000EF8C8 File Offset: 0x000EDAC8
		public void StartRising(int lifespanTicks = -1)
		{
			if (!this.pawn.Dead && !this.pawn.Downed)
			{
				Log.Error("Tried to raise non dead/downed pawn as shambler");
				this.pawn.mutant.Turn(true);
				return;
			}
			MutantUtility.RestoreBodyParts(this.pawn);
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.pawn.Dead && !ResurrectionUtility.TryResurrect(this.pawn, new ResurrectionParams
			{
				noLord = true,
				restoreMissingParts = false,
				removeDiedThoughts = false
			}))
			{
				return;
			}
			Pawn_JobTracker jobs = this.pawn.jobs;
			if (jobs != null)
			{
				jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			this.resurrectTimer = (float)(Find.TickManager.TicksGame + Hediff_Shambler.ResurrectionSecondsRange.RandomInRange.SecondsToTicks());
			this.pawn.health.AddHediff(HediffDefOf.Rising, null, null, null);
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x000EF9B8 File Offset: 0x000EDBB8
		private void CancelRising()
		{
			Sustainer sustainer = this.riseSustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.resurrectTimer = -99999f;
			Hediff hediff;
			if (this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.Rising, out hediff))
			{
				this.pawn.health.RemoveHediff(hediff);
			}
			if (!this.pawn.Dead)
			{
				this.pawn.Kill(null, null);
			}
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x000EFA34 File Offset: 0x000EDC34
		private void FinishRising(bool stun = true)
		{
			Hediff hediff;
			if (this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.Rising, out hediff))
			{
				this.pawn.health.RemoveHediff(hediff);
			}
			Pawn_CarryTracker pawn_CarryTracker = this.pawn.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				Thing thing;
				pawn_CarryTracker.TryDropCarriedThing(pawn_CarryTracker.pawn.Position, ThingPlaceMode.Near, out thing, null);
				pawn_CarryTracker.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			if (!this.pawn.mutant.HasTurned)
			{
				this.pawn.mutant.Turn(false);
			}
			this.pawn.timesRaisedAsShambler++;
			MutantUtility.RestoreUntilNotDowned(this.pawn);
			if (this.pawn.Spawned && stun)
			{
				this.pawn.Rotation = Rot4.South;
				this.pawn.stances.stunner.StunFor(Hediff_Shambler.StunSecondsRange.RandomInRange.SecondsToTicks(), this.pawn, false, false, false);
			}
			this.pawn.Drawer.renderer.SetAnimation(null);
			this.StartSelfRaiseTimer();
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000EFB5C File Offset: 0x000EDD5C
		private void StartSelfRaiseTimer()
		{
			this.selfRaiseTimer = (float)(Find.TickManager.TicksGame + 2500 * Hediff_Shambler.SelfRaiseHoursRange.RandomInRange);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000EFB90 File Offset: 0x000EDD90
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Instigator == null || !this.pawn.HostileTo(dinfo.Instigator))
			{
				return;
			}
			if (this.pawn.Spawned && dinfo.Instigator != null && dinfo.Instigator is IAttackTarget && dinfo.Instigator.Spawned && this.pawn.HostileTo(dinfo.Instigator) && this.pawn.CanSee(dinfo.Instigator, null))
			{
				this.pawn.mindState.enemyTarget = dinfo.Instigator;
				this.pawn.mindState.Notify_EngagedTarget();
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptOptional, true, true);
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnAcquiredTarget(this.pawn, dinfo.Instigator);
				}
			}
			MutantUtility.ActivateNearbyShamblers(this.pawn, dinfo.Instigator);
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000EFC9C File Offset: 0x000EDE9C
		public void Notify_DelayedAlert(Thing target)
		{
			if (this.pawn.mutant.IsPassive || this.pawn.Drafted)
			{
				return;
			}
			if (this.pawn.mindState.enemyTarget != null)
			{
				return;
			}
			this.alertTimer = (float)(Find.TickManager.TicksGame + Hediff_Shambler.AlertSecondsRange.RandomInRange.SecondsToTicks());
			this.alertedTarget = target;
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_PawnAcquiredTarget(this.pawn, target);
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000EFD23 File Offset: 0x000EDF23
		public override void Notify_Downed()
		{
			this.StartSelfRaiseTimer();
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000EFD2B File Offset: 0x000EDF2B
		public override void Notify_PawnKilled()
		{
			this.corpseDamagePct = this.pawn.health.summaryHealth.SummaryHealthPercent;
			base.Notify_PawnKilled();
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000EFD50 File Offset: 0x000EDF50
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			if (this.IsRising)
			{
				this.CancelRising();
			}
			if (this.pawn.timesRaisedAsShambler == 1 && Rand.Chance(0.04f) && this.pawn.SpawnedOrAnyParentSpawned)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Bioferrite, null);
				thing.stackCount = 10;
				Thing t;
				GenPlace.TryPlaceThing(thing, this.pawn.PositionHeld, this.pawn.MapHeld, ThingPlaceMode.Near, out t, null, null, null, 1);
				t.SetForbidden(true, true);
			}
			if (this.pawn.Corpse != null)
			{
				this.pawn.Corpse.HitPoints = Mathf.Max(Mathf.RoundToInt((float)this.pawn.Corpse.MaxHitPoints * this.corpseDamagePct), 10);
			}
			this.pawn.health.AddHediff(HediffDefOf.ShamblerCorpse, null, null, null);
			base.Notify_PawnDied(dinfo, culprit);
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000EFE42 File Offset: 0x000EE042
		public override void PreRemoved()
		{
			Map mapHeld = this.pawn.MapHeld;
			if (mapHeld != null)
			{
				mapHeld.mapPawns.DeregisterShambler(this.pawn);
			}
			base.PreRemoved();
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000EFE6C File Offset: 0x000EE06C
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (this.pawn.Dead)
			{
				return;
			}
			if (this.IsRising)
			{
				this.CancelRising();
			}
			if (this.pawn.IsMutant)
			{
				if (this.pawn.mutant.HasTurned)
				{
					this.pawn.mutant.Revert(false);
					return;
				}
				this.pawn.mutant = null;
			}
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000EFED8 File Offset: 0x000EE0D8
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos && this.pawn.Downed && !this.IsRising)
			{
				yield return new Command_Action
				{
					defaultLabel = "Self Raise",
					action = delegate()
					{
						this.StartRising(-1);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000EFEE8 File Offset: 0x000EE0E8
		public override string GetInspectString()
		{
			if (this.ShouldSelfRaise())
			{
				return "ShamblerRegenerating".Translate();
			}
			if (this.IsRising)
			{
				return "ShamblerRising".Translate();
			}
			if (this.pawn.CurJobDef == JobDefOf.Wait_Wander || this.pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
			{
				return "ShamblerStanding".Translate();
			}
			if (this.pawn.CurJobDef == JobDefOf.GotoWander || this.pawn.CurJobDef == JobDefOf.Goto)
			{
				return "ShamblerShuffling".Translate();
			}
			return "";
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000EFF94 File Offset: 0x000EE194
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.alertTimer, "alertTimer", 0f, false);
			Scribe_Values.Look<int>(ref this.nextTargetCheckTick, "nextTargetCheckTick", 0, false);
			Scribe_References.Look<Thing>(ref this.alertedTarget, "alertedTarget", false);
			Scribe_Values.Look<float>(ref this.resurrectTimer, "resurrectTimer", 0f, false);
			Scribe_Values.Look<float>(ref this.selfRaiseTimer, "selfRaiseTimer", 0f, false);
			Scribe_Values.Look<float>(ref this.headRotation, "headRotation", 0f, false);
		}

		// Token: 0x0400246D RID: 9325
		private static readonly FloatRange ResurrectionSecondsRange = new FloatRange(5f, 10f);

		// Token: 0x0400246E RID: 9326
		private static readonly FloatRange StunSecondsRange = new FloatRange(1f, 3f);

		// Token: 0x0400246F RID: 9327
		private static readonly FloatRange AlertSecondsRange = new FloatRange(0.5f, 3f);

		// Token: 0x04002470 RID: 9328
		private static readonly IntRange SelfRaiseHoursRange = new IntRange(3, 4);

		// Token: 0x04002471 RID: 9329
		private const float ParticleSpawnMTBSeconds = 1f;

		// Token: 0x04002472 RID: 9330
		private static readonly IntRange CheckForTargetTicksInterval = new IntRange(900, 1800);

		// Token: 0x04002473 RID: 9331
		private const float ExtinguishFireMTB = 45f;

		// Token: 0x04002474 RID: 9332
		private const float BioferriteOnDeathChance = 0.04f;

		// Token: 0x04002475 RID: 9333
		private const int BioferriteAmountOnDeath = 10;

		// Token: 0x04002476 RID: 9334
		public float headRotation;

		// Token: 0x04002477 RID: 9335
		private float resurrectTimer;

		// Token: 0x04002478 RID: 9336
		private float selfRaiseTimer;

		// Token: 0x04002479 RID: 9337
		private float alertTimer;

		// Token: 0x0400247A RID: 9338
		private int nextTargetCheckTick = -99999;

		// Token: 0x0400247B RID: 9339
		private Thing alertedTarget;

		// Token: 0x0400247C RID: 9340
		private Effecter riseEffecter;

		// Token: 0x0400247D RID: 9341
		private Sustainer riseSustainer;

		// Token: 0x0400247E RID: 9342
		private float corpseDamagePct = 1f;
	}
}
