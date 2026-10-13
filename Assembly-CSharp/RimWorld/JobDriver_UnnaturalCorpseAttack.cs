using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200133B RID: 4923
	public class JobDriver_UnnaturalCorpseAttack : JobDriver
	{
		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x060078F6 RID: 30966 RVA: 0x002495EF File Offset: 0x002477EF
		private Pawn Victim
		{
			get
			{
				return base.TargetPawnA;
			}
		}

		// Token: 0x060078F7 RID: 30967 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060078F8 RID: 30968 RVA: 0x002495F7 File Offset: 0x002477F7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			UnnaturalCorpseTracker tracker;
			Find.Anomaly.TryGetUnnaturalCorpseTrackerForHaunted(this.Victim, out tracker);
			int killTicks = tracker.TicksToKill;
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate()
			{
				JobDriver curDriver = this.pawn.jobs.curDriver;
				int ticks = killTicks + 60;
				Find.Anomaly.Hypnotize(this.Victim, this.pawn, ticks);
				curDriver.ReadyForNextToil();
			});
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			Toil toil2 = toil;
			toil2.initAction = (Action)Delegate.Combine(toil2.initAction, new Action(delegate()
			{
				Messages.Message("MessageAwokenAttacking".Translate(this.Victim.Named("PAWN")), this.Victim, MessageTypeDefOf.NegativeEvent, true);
				tracker.Notify_AwokenAttackStarting();
			}));
			Toil toil3 = toil;
			toil3.tickAction = (Action)Delegate.Combine(toil3.tickAction, new Action(delegate()
			{
				this.job.GetTarget(TargetIndex.A).Pawn.rotationTracker.FaceTarget(this.pawn);
				this.Victim.health.GetOrAddHediff(HediffDefOf.AwokenHypnosis, null, null, null).TryGetComp<HediffComp_Disappears>().ticksToDisappear++;
				if (this.pawn.Drawer.renderer.CurAnimation != AnimationDefOf.UnnaturalCorpseAwokenKilling)
				{
					this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.UnnaturalCorpseAwokenKilling);
				}
			}));
			toil.defaultDuration = killTicks;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.PlaySustainerOrSound(SoundDefOf.Pawn_Awoken_Hypnotize, 1f);
			toil.AddFinishAction(delegate
			{
				if (this.pawn.Drawer.renderer.CurAnimation == AnimationDefOf.UnnaturalCorpseAwokenKilling)
				{
					this.pawn.Drawer.renderer.SetAnimation(null);
				}
			});
			yield return toil;
			Toil toil4 = ToilMaker.MakeToil("MakeNewToils");
			toil4.initAction = delegate()
			{
				Pawn pawn = this.job.GetTarget(TargetIndex.A).Pawn;
				BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_UnnaturalCorpseAttack, this.pawn));
				Thing thing = pawn;
				DamageDef psychic = DamageDefOf.Psychic;
				float amount = 99999f;
				float armorPenetration = 99999f;
				float angle = -1f;
				BodyPartRecord hitPart = brain;
				thing.TakeDamage(new DamageInfo(psychic, amount, armorPenetration, angle, this.pawn, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				Find.Anomaly.Notify_PawnKilledViaAwoken(pawn);
			};
			toil4.PlaySustainerOrSound(SoundDefOf.CocoonDestroyed, 1f);
			yield return toil4;
			yield break;
		}

		// Token: 0x060078F9 RID: 30969 RVA: 0x001D7A1B File Offset: 0x001D5C1B
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04005225 RID: 21029
		private const TargetIndex VictimIndex = TargetIndex.A;
	}
}
