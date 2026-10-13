using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001307 RID: 4871
	public class JobDriver_RevenantAttack : JobDriver
	{
		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x0600777D RID: 30589 RVA: 0x002449FF File Offset: 0x00242BFF
		private int HypnotizeDurationTicks
		{
			get
			{
				return RevenantUtility.HypnotizeDurationSecondsFromNumColonistsCurve.Evaluate((float)RevenantUtility.NumSpawnedUnhypnotizedColonists(this.pawn.Map)).SecondsToTicks();
			}
		}

		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x0600777E RID: 30590 RVA: 0x00244A21 File Offset: 0x00242C21
		private float RevealRange
		{
			get
			{
				return RevenantUtility.RevealRangeFromNumColonistsCurve.Evaluate((float)RevenantUtility.NumSpawnedUnhypnotizedColonists(this.pawn.Map));
			}
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x0600777F RID: 30591 RVA: 0x00244A3E File Offset: 0x00242C3E
		private CompRevenant Comp
		{
			get
			{
				return this.pawn.TryGetComp<CompRevenant>();
			}
		}

		// Token: 0x06007780 RID: 30592 RVA: 0x00244A4B File Offset: 0x00242C4B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hypnotizeEndTick, "hypnotizeEndTick", 0, false);
		}

		// Token: 0x06007781 RID: 30593 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06007782 RID: 30594 RVA: 0x00244A65 File Offset: 0x00242C65
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.pawn.mindState.enemyTarget != null)
			{
				this.job.SetTarget(TargetIndex.A, this.pawn.mindState.enemyTarget);
			}
		}

		// Token: 0x06007783 RID: 30595 RVA: 0x00244AA0 File Offset: 0x00242CA0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil stalk = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate()
			{
				if (this.pawn.stances.FullBodyBusy)
				{
					return;
				}
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				JobDriver curDriver = this.pawn.jobs.curDriver;
				Find.Anomaly.Hypnotize(pawn, this.pawn, JobDriver_RevenantAttack.LongStunTicks);
				curDriver.ReadyForNextToil();
			});
			Toil stalk3 = stalk;
			stalk3.initAction = (Action)Delegate.Combine(stalk3.initAction, new Action(delegate()
			{
				this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
			}));
			Toil stalk2 = stalk;
			stalk2.tickIntervalAction = (Action<int>)Delegate.Combine(stalk2.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				Job curJob = stalk.actor.jobs.curJob;
				if (Rand.MTBEventOccurs(180f, 1f, (float)delta))
				{
					curJob.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(this.pawn, 10f) ?? curJob.GetTarget(TargetIndex.A).Pawn);
					this.pawn.mindState.enemyTarget = curJob.GetTarget(TargetIndex.A).Pawn;
				}
				if (curJob.targetA == null)
				{
					this.EndJobWith(JobCondition.Incompletable);
					return;
				}
				Pawn pawn = curJob.targetA.Pawn;
				if (this.pawn.Position.InHorDistOf(pawn.Position, this.RevealRange))
				{
					if (!this.Comp.Invisibility.PsychologicallyVisible)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRevenantRevealed".Translate(), this.Comp.everRevealed ? "LetterRevenantRevealed".Translate() : "LetterRevenantRevealedFirst".Translate(), LetterDefOf.ThreatBig, this.pawn, null, null, null, null, 60, true);
					}
					this.Comp.Invisibility.BecomeVisible(false);
					Find.TickManager.slower.SignalForceNormalSpeed();
				}
				if (this.Comp.Invisibility.PsychologicallyVisible && Rand.MTBEventOccurs(60f, 1f, (float)delta))
				{
					RevenantUtility.CreateRevenantSmear(this.pawn);
				}
			}));
			stalk.AddFinishAction(delegate
			{
				Pawn pawn = this.job.GetTarget(TargetIndex.A).Pawn;
				if (pawn == null || !pawn.Spawned || pawn.Map != this.pawn.Map)
				{
					if (pawn != null)
					{
						Find.Anomaly.EndHypnotize(pawn);
					}
					this.job.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(this.pawn, 20f));
					this.pawn.mindState.enemyTarget = this.job.GetTarget(TargetIndex.A).Pawn;
					if (this.pawn.mindState.enemyTarget == null)
					{
						this.Comp.revenantState = RevenantState.Wander;
					}
				}
			});
			yield return stalk;
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.initAction = delegate()
			{
				this.hypnotizeEndTick = Find.TickManager.TicksGame + this.HypnotizeDurationTicks;
			};
			toil.AddFinishAction(delegate
			{
				Pawn pawn = this.job.GetTarget(TargetIndex.A).Pawn;
				Find.Anomaly.EndHypnotize(pawn);
				this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
			});
			Toil toil2 = toil;
			toil2.tickAction = (Action)Delegate.Combine(toil2.tickAction, new Action(delegate()
			{
				this.job.GetTarget(TargetIndex.A).Pawn.rotationTracker.FaceTarget(this.pawn);
				if (this.pawn.Drawer.renderer.CurAnimation != AnimationDefOf.RevenantHypnotise)
				{
					this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantHypnotise);
				}
				if (!this.pawn.stances.stunner.Stunned && Find.TickManager.TicksGame >= this.hypnotizeEndTick)
				{
					Pawn pawn = this.job.GetTarget(TargetIndex.A).Pawn;
					this.Comp.Hypnotize(pawn);
					if (this.Comp.injuredWhileAttacking)
					{
						Thing thing = ThingMaker.MakeThing(ThingDefOf.RevenantFleshChunk, null);
						thing.TryGetComp<CompAnalyzableBiosignature>().biosignature = this.Comp.biosignature;
						Thing t = GenSpawn.Spawn(thing, this.pawn.PositionHeld, this.pawn.Map, WipeMode.Vanish);
						Find.LetterStack.ReceiveLetter("LetterRevenantFleshChunkLabel".Translate(), "LetterRevenantFleshChunkText".Translate(), LetterDefOf.NeutralEvent, t, null, null, null, null, 600, true);
						this.Comp.injuredWhileAttacking = false;
					}
					this.Comp.revenantState = RevenantState.Escape;
					this.EndJobWith(JobCondition.Succeeded);
				}
			}));
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.PlaySustainerOrSound(SoundDefOf.Pawn_Revenant_Hypnotize, 1f);
			yield return toil;
			yield break;
		}

		// Token: 0x06007784 RID: 30596 RVA: 0x001D7A1B File Offset: 0x001D5C1B
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x06007785 RID: 30597 RVA: 0x00244AB0 File Offset: 0x00242CB0
		public override void Notify_PatherFailed()
		{
			Pawn pawn = this.job.GetTarget(TargetIndex.A).Pawn;
			if (pawn != null)
			{
				Find.Anomaly.EndHypnotize(pawn);
			}
			this.job.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(this.pawn, 20f));
			this.pawn.mindState.enemyTarget = this.job.GetTarget(TargetIndex.A).Pawn;
			if (this.pawn.mindState.enemyTarget == null)
			{
				this.Comp.revenantState = RevenantState.Wander;
			}
			base.Notify_PatherFailed();
		}

		// Token: 0x0400516F RID: 20847
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x04005170 RID: 20848
		private const float NewTargetScanRadius = 20f;

		// Token: 0x04005171 RID: 20849
		private const int SmearMTBTicks = 60;

		// Token: 0x04005172 RID: 20850
		private const int CheckForCloserTargetMTB = 180;

		// Token: 0x04005173 RID: 20851
		private static readonly int LongStunTicks = 2500;

		// Token: 0x04005174 RID: 20852
		private int hypnotizeEndTick;
	}
}
