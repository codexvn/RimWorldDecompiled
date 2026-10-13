using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000E93 RID: 3731
	public class JobDriver_AttackMelee : JobDriver
	{
		// Token: 0x06005E07 RID: 24071 RVA: 0x001DF21F File Offset: 0x001DD41F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numMeleeAttacksMade, "numMeleeAttacksMade", 0, false);
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x001DF23C File Offset: 0x001DD43C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			IAttackTarget attackTarget = this.job.targetA.Thing as IAttackTarget;
			if (attackTarget != null)
			{
				this.pawn.Map.attackTargetReservationManager.Reserve(this.pawn, this.job, attackTarget);
			}
			return true;
		}

		// Token: 0x06005E09 RID: 24073 RVA: 0x001DF285 File Offset: 0x001DD485
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddFinishAction(delegate(JobCondition _)
			{
				if (!this.pawn.IsPlayerControlled || !this.pawn.Drafted)
				{
					return;
				}
				if (!this.job.playerInterruptedForced)
				{
					Thing targetThingA = base.TargetThingA;
					if (targetThingA != null && targetThingA.def.autoTargetNearbyIdenticalThings)
					{
						foreach (IntVec3 c in GenRadial.RadialCellsAround(base.TargetThingA.Position, 4f, false).InRandomOrder(null))
						{
							if (c.InBounds(base.Map))
							{
								foreach (Thing thing in c.GetThingList(base.Map))
								{
									if (thing.def == base.TargetThingA.def && this.pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && this.pawn.jobs.jobQueue.Count == 0)
									{
										Job job = this.job.Clone();
										job.targetA = thing;
										this.pawn.jobs.jobQueue.EnqueueFirst(job, null);
										return;
									}
								}
							}
						}
					}
				}
			});
			yield return Toils_General.DoAtomic(delegate
			{
				Pawn pawn = this.job.targetA.Thing as Pawn;
				if (pawn == null)
				{
					return;
				}
				bool flag = pawn.Downed && this.pawn.mindState.duty != null && this.pawn.mindState.duty.attackDownedIfStarving && this.pawn.Starving();
				CompActivity compActivity;
				bool flag2 = ModsConfig.AnomalyActive && pawn.TryGetComp(out compActivity) && compActivity.IsDormant;
				if (flag || flag2)
				{
					this.job.killIncappedTarget = true;
				}
			});
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate()
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (this.job.reactingToMeleeThreat)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && !pawn.Awake())
					{
						base.EndJobWith(JobCondition.InterruptForced);
						return;
					}
				}
				if (this.pawn.meleeVerbs.TryMeleeAttack(thing, this.job.verbToUse, false))
				{
					if (this.pawn.CurJob == null || this.pawn.jobs.curDriver != this)
					{
						return;
					}
					Lord lord = this.pawn.GetLord();
					if (((lord != null) ? lord.LordJob : null) != null)
					{
						LordJob_Ritual_Duel lordJob_Ritual_Duel = lord.LordJob as LordJob_Ritual_Duel;
						if (lordJob_Ritual_Duel != null)
						{
							lordJob_Ritual_Duel.Notify_MeleeAttack(this.pawn, thing);
						}
					}
					this.numMeleeAttacksMade++;
					if (this.numMeleeAttacksMade >= this.job.maxNumMeleeAttacks)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
				}
			}).FailOnDespawnedOrNull(TargetIndex.A);
			yield break;
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x001DF298 File Offset: 0x001DD498
		public override void Notify_PatherFailed()
		{
			if (this.job.attackDoorIfTargetLost)
			{
				Thing thing;
				using (PawnPath pawnPath = base.Map.pathFinder.FindPathNow(this.pawn.Position, base.TargetA.Cell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.PassDoors, false, false, false, true), null, PathEndMode.OnCell, null))
				{
					if (!pawnPath.Found)
					{
						return;
					}
					IntVec3 position;
					thing = pawnPath.FirstBlockingBuilding(out position, this.pawn);
				}
				if (thing != null)
				{
					IntVec3 position = thing.Position;
					if (position.InHorDistOf(this.pawn.Position, 6f))
					{
						this.job.targetA = thing;
						this.job.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
						this.job.expiryInterval = Rand.Range(2000, 4000);
						return;
					}
				}
			}
			base.Notify_PatherFailed();
		}

		// Token: 0x06005E0B RID: 24075 RVA: 0x001D7A1B File Offset: 0x001D5C1B
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x040043F8 RID: 17400
		private const int AutotargetRadius = 4;

		// Token: 0x040043F9 RID: 17401
		private int numMeleeAttacksMade;
	}
}
