using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000F10 RID: 3856
	public static class Toils_Combat
	{
		// Token: 0x06006046 RID: 24646 RVA: 0x001E8A88 File Offset: 0x001E6C88
		public static Toil TrySetJobToUseAttackVerb(TargetIndex targetInd)
		{
			Toil toil = ToilMaker.MakeToil("TrySetJobToUseAttackVerb");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				bool allowManualCastWeapons = !actor.IsColonist;
				Verb verb = actor.TryGetAttackVerb(curJob.GetTarget(targetInd).Thing, allowManualCastWeapons, false);
				if (verb == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				curJob.verbToUse = verb;
			};
			return toil;
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x001E8AD0 File Offset: 0x001E6CD0
		public static Toil GotoCastPosition(TargetIndex targetInd, TargetIndex castPositionInd = TargetIndex.None, bool closeIfDowned = false, float maxRangeFactor = 1f)
		{
			Toil toil = ToilMaker.MakeToil("GotoCastPosition");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(targetInd).Thing;
				Pawn pawn = thing as Pawn;
				if (actor == thing)
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				if (thing == null)
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				CastPositionRequest newReq = default(CastPositionRequest);
				newReq.caster = toil.actor;
				newReq.target = thing;
				newReq.verb = curJob.verbToUse;
				newReq.maxRangeFromTarget = ((!closeIfDowned || pawn == null || !pawn.Downed) ? Mathf.Max(curJob.verbToUse.verbProps.range * maxRangeFactor, 1.42f) : Mathf.Min(curJob.verbToUse.verbProps.range, (float)pawn.RaceProps.executionRange));
				newReq.wantCoverFromTarget = false;
				if (castPositionInd != TargetIndex.None)
				{
					newReq.preferredCastPosition = new IntVec3?(curJob.GetTarget(castPositionInd).Cell);
				}
				IntVec3 intVec;
				if (!CastPositionFinder.TryFindCastPosition(newReq, out intVec))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				toil.actor.pather.StartPath(intVec, PathEndMode.OnCell);
				actor.Map.pawnDestinationReservationManager.Reserve(actor, curJob, intVec);
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x001E8B4A File Offset: 0x001E6D4A
		public static Toil CastVerb(TargetIndex targetInd, bool canHitNonTargetPawns = true)
		{
			return Toils_Combat.CastVerb(targetInd, TargetIndex.None, canHitNonTargetPawns);
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x001E8B54 File Offset: 0x001E6D54
		public static Toil CastVerb(TargetIndex targetInd, TargetIndex destInd, bool canHitNonTargetPawns = true)
		{
			Toil toil = ToilMaker.MakeToil("CastVerb");
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(targetInd);
				LocalTargetInfo destTarg = (destInd != TargetIndex.None) ? toil.actor.jobs.curJob.GetTarget(destInd) : LocalTargetInfo.Invalid;
				toil.actor.jobs.curJob.verbToUse.TryStartCastOn(target, destTarg, false, canHitNonTargetPawns, toil.actor.jobs.curJob.preventFriendlyFire, false);
			};
			toil.defaultCompleteMode = ToilCompleteMode.FinishedBusy;
			toil.activeSkill = (() => Toils_Combat.GetActiveSkillForToil(toil));
			return toil;
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x001E8BCC File Offset: 0x001E6DCC
		public static SkillDef GetActiveSkillForToil(Toil toil)
		{
			Verb verbToUse = toil.actor.jobs.curJob.verbToUse;
			if (verbToUse != null && verbToUse.EquipmentSource != null)
			{
				if (verbToUse.EquipmentSource.def.IsMeleeWeapon)
				{
					return SkillDefOf.Melee;
				}
				if (verbToUse.EquipmentSource.def.IsRangedWeapon)
				{
					return SkillDefOf.Shooting;
				}
			}
			return null;
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x001E8C2B File Offset: 0x001E6E2B
		public static Toil FollowAndMeleeAttack(TargetIndex targetInd, Action hitAction)
		{
			return Toils_Combat.FollowAndMeleeAttack(targetInd, TargetIndex.None, hitAction);
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x001E8C38 File Offset: 0x001E6E38
		public static Toil FollowAndMeleeAttack(TargetIndex targetInd, TargetIndex standPositionInd, Action hitAction)
		{
			Toil followAndAttack = ToilMaker.MakeToil("FollowAndMeleeAttack");
			followAndAttack.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = followAndAttack.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver curDriver = actor.jobs.curDriver;
				LocalTargetInfo target = curJob.GetTarget(targetInd);
				Thing thing = target.Thing;
				Pawn pawn = thing as Pawn;
				if (!thing.Spawned || (pawn != null && pawn.IsPsychologicallyInvisible()))
				{
					curDriver.ReadyForNextToil();
					return;
				}
				LocalTargetInfo localTargetInfo = target;
				PathEndMode peMode = PathEndMode.Touch;
				if (standPositionInd != TargetIndex.None)
				{
					LocalTargetInfo target2 = curJob.GetTarget(standPositionInd);
					if (target2.IsValid)
					{
						localTargetInfo = target2;
						peMode = PathEndMode.OnCell;
					}
				}
				if (localTargetInfo != actor.pather.Destination || (!actor.pather.Moving && !actor.CanReachImmediate(target, PathEndMode.Touch)))
				{
					actor.pather.StartPath(localTargetInfo, peMode);
					return;
				}
				if (actor.CanReachImmediate(target, PathEndMode.Touch))
				{
					if (pawn != null)
					{
						bool flag = pawn.IsMutant && pawn.mutant.Def.canAttackWhileCrawling && !pawn.ThreatDisabled(null);
						if (pawn.Downed && !flag && !curJob.killIncappedTarget)
						{
							curDriver.ReadyForNextToil();
							return;
						}
					}
					hitAction();
				}
			};
			followAndAttack.activeSkill = (() => SkillDefOf.Melee);
			followAndAttack.defaultCompleteMode = ToilCompleteMode.Never;
			return followAndAttack;
		}
	}
}
