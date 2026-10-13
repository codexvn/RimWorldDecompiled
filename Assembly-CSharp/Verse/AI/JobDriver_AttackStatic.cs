using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E95 RID: 3733
	public class JobDriver_AttackStatic : JobDriver
	{
		// Token: 0x06005E18 RID: 24088 RVA: 0x001DF7D3 File Offset: 0x001DD9D3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x001DF7FF File Offset: 0x001DD9FF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddFinishAction(delegate(JobCondition jobCondition)
			{
				if (!this.pawn.IsPlayerControlled || !this.pawn.Drafted)
				{
					return;
				}
				if (!this.job.playerInterruptedForced)
				{
					Thing targetThingA = this.TargetThingA;
					if (targetThingA != null && targetThingA.def.autoTargetNearbyIdenticalThings)
					{
						Verb verb = this.pawn.TryGetAttackVerb(this.TargetA.Thing, !this.pawn.IsColonist, false);
						foreach (IntVec3 c in GenRadial.RadialCellsAround(this.TargetThingA.Position, 4f, false).InRandomOrder(null))
						{
							if (c.InBounds(this.Map))
							{
								foreach (Thing thing in c.GetThingList(this.Map))
								{
									if (thing.def == this.TargetThingA.def && verb != null && verb.CanHitTargetFrom(this.pawn.Position, thing) && this.pawn.jobs.jobQueue.Count == 0)
									{
										Job job = this.job.Clone();
										job.targetA = thing;
										job.endIfCantShootTargetFromCurPos = true;
										this.pawn.jobs.jobQueue.EnqueueFirst(job, null);
										return;
									}
								}
							}
						}
					}
				}
			});
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			Toil init = ToilMaker.MakeToil("MakeNewToils");
			init.initAction = delegate()
			{
				Pawn pawn = this.TargetThingA as Pawn;
				if (pawn != null)
				{
					this.startedIncapacitated = pawn.Downed;
				}
				this.pawn.pather.StopDead();
			};
			init.tickIntervalAction = delegate(int delta)
			{
				if (!this.TargetA.IsValid)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				if (this.TargetA.HasThing)
				{
					Pawn pawn = this.TargetA.Thing as Pawn;
					if (this.TargetA.Thing.Destroyed || (pawn != null && !this.startedIncapacitated && pawn.Downed) || (pawn != null && pawn.IsPsychologicallyInvisible()))
					{
						this.EndJobWith(JobCondition.Succeeded);
						return;
					}
				}
				if (this.numAttacksMade >= this.job.maxNumStaticAttacks && !this.pawn.stances.FullBodyBusy)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				if (this.pawn.TryStartAttack(this.TargetA))
				{
					this.numAttacksMade++;
					return;
				}
				if (!this.pawn.stances.FullBodyBusy)
				{
					Verb verb = this.pawn.TryGetAttackVerb(this.TargetA.Thing, !this.pawn.IsColonist, false);
					if (this.job.endIfCantShootTargetFromCurPos && (verb == null || !verb.CanHitTargetFrom(this.pawn.Position, this.TargetA)))
					{
						this.EndJobWith(JobCondition.Incompletable);
						return;
					}
					if (this.job.endIfCantShootInMelee)
					{
						if (verb == null)
						{
							this.EndJobWith(JobCondition.Incompletable);
							return;
						}
						float num = verb.verbProps.EffectiveMinRange(this.TargetA, this.pawn);
						if ((float)this.pawn.Position.DistanceToSquared(this.TargetA.Cell) < num * num && this.pawn.Position.AdjacentTo8WayOrInside(this.TargetA.Cell))
						{
							this.EndJobWith(JobCondition.Incompletable);
							return;
						}
					}
				}
			};
			init.defaultCompleteMode = ToilCompleteMode.Never;
			init.activeSkill = (() => Toils_Combat.GetActiveSkillForToil(init));
			yield return init;
			yield break;
		}

		// Token: 0x040043FE RID: 17406
		private const int AutotargetRadius = 4;

		// Token: 0x040043FF RID: 17407
		private bool startedIncapacitated;

		// Token: 0x04004400 RID: 17408
		private int numAttacksMade;
	}
}
