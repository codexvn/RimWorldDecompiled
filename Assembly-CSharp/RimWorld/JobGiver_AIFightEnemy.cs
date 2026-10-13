using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001404 RID: 5124
	public abstract class JobGiver_AIFightEnemy : ThinkNode_JobGiver
	{
		// Token: 0x06007CD8 RID: 31960
		protected abstract bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null);

		// Token: 0x06007CD9 RID: 31961 RVA: 0x002597AA File Offset: 0x002579AA
		protected virtual float GetFlagRadius(Pawn pawn)
		{
			return 999999f;
		}

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06007CDA RID: 31962 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool OnlyUseAbilityVerbs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x06007CDB RID: 31963 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool DisableAbilityVerbs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x06007CDC RID: 31964 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool OnlyUseRangedSearch
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007CDD RID: 31965 RVA: 0x00210AAE File Offset: 0x0020ECAE
		protected virtual IntVec3 GetFlagPosition(Pawn pawn)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x06007CDE RID: 31966 RVA: 0x002597B1 File Offset: 0x002579B1
		protected virtual IntRange ExpiryInterval_Melee
		{
			get
			{
				return new IntRange(360, 480);
			}
		}

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06007CDF RID: 31967 RVA: 0x002597C2 File Offset: 0x002579C2
		protected virtual IntRange ExpiryInterval_Ability
		{
			get
			{
				return new IntRange(30, 30);
			}
		}

		// Token: 0x06007CE0 RID: 31968 RVA: 0x002597D0 File Offset: 0x002579D0
		protected virtual bool ExtraTargetValidator(Pawn pawn, Thing target)
		{
			if (pawn.IsColonyMechPlayerControlled && target.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (this.humanlikesOnly)
			{
				Pawn pawn2 = target as Pawn;
				if (pawn2 != null && !pawn2.RaceProps.Humanlike)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007CE1 RID: 31969 RVA: 0x00259818 File Offset: 0x00257A18
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIFightEnemy jobGiver_AIFightEnemy = (JobGiver_AIFightEnemy)base.DeepCopy(resolve);
			jobGiver_AIFightEnemy.targetAcquireRadius = this.targetAcquireRadius;
			jobGiver_AIFightEnemy.targetKeepRadius = this.targetKeepRadius;
			jobGiver_AIFightEnemy.needLOSToAcquireNonPawnTargets = this.needLOSToAcquireNonPawnTargets;
			jobGiver_AIFightEnemy.chaseTarget = this.chaseTarget;
			jobGiver_AIFightEnemy.allowTurrets = this.allowTurrets;
			jobGiver_AIFightEnemy.ignoreNonCombatants = this.ignoreNonCombatants;
			jobGiver_AIFightEnemy.humanlikesOnly = this.humanlikesOnly;
			return jobGiver_AIFightEnemy;
		}

		// Token: 0x06007CE2 RID: 31970 RVA: 0x00259888 File Offset: 0x00257A88
		protected override Job TryGiveJob(Pawn pawn)
		{
			if ((pawn.IsColonist || pawn.IsColonySubhuman) && pawn.playerSettings.hostilityResponse != HostilityResponseMode.Attack)
			{
				Lord lord = pawn.GetLord();
				LordJob_Ritual_Duel lordJob_Ritual_Duel = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual_Duel;
				if (lordJob_Ritual_Duel == null || !lordJob_Ritual_Duel.duelists.Contains(pawn))
				{
					return null;
				}
			}
			this.UpdateEnemyTarget(pawn);
			Thing enemyTarget = pawn.mindState.enemyTarget;
			if (enemyTarget == null)
			{
				return null;
			}
			Pawn pawn2 = enemyTarget as Pawn;
			if (pawn2 != null && pawn2.IsPsychologicallyInvisible())
			{
				return null;
			}
			bool flag = !pawn.IsColonist && !pawn.IsColonySubhuman && !this.DisableAbilityVerbs;
			if (flag)
			{
				Job abilityJob = JobGiver_AIFightEnemy.GetAbilityJob(pawn, enemyTarget);
				if (abilityJob != null)
				{
					return abilityJob;
				}
			}
			if (this.OnlyUseAbilityVerbs)
			{
				IntVec3 intVec;
				if (!this.TryFindShootingPosition(pawn, out intVec, null))
				{
					return null;
				}
				if (intVec == pawn.Position)
				{
					Pawn_PathFollower pather = pawn.pather;
					if (pather != null)
					{
						pather.StopDead();
					}
					return JobMaker.MakeJob(JobDefOf.Wait_Combat, this.ExpiryInterval_Ability.RandomInRange, true);
				}
				Job job = JobMaker.MakeJob(JobDefOf.Goto, intVec);
				job.expiryInterval = this.ExpiryInterval_Ability.RandomInRange;
				job.checkOverrideOnExpire = true;
				return job;
			}
			else
			{
				Verb verb = pawn.TryGetAttackVerb(enemyTarget, flag, this.allowTurrets);
				if (verb == null)
				{
					return null;
				}
				if (verb.verbProps.IsMeleeAttack)
				{
					return this.MeleeAttackJob(pawn, enemyTarget);
				}
				bool flag2 = CoverUtility.CalculateOverallBlockChance(pawn, enemyTarget.Position, pawn.Map) > 0.01f;
				bool flag3 = pawn.Position.WalkableBy(pawn.Map, pawn) && pawn.Map.pawnDestinationReservationManager.CanReserve(pawn.Position, pawn, pawn.Drafted);
				bool flag4 = verb.CanHitTarget(enemyTarget);
				bool flag5 = (pawn.Position - enemyTarget.Position).LengthHorizontalSquared < 25;
				if ((flag2 && flag3 && flag4) || (flag5 && flag4))
				{
					Pawn_PathFollower pather2 = pawn.pather;
					if (pather2 != null)
					{
						pather2.StopDead();
					}
					return JobMaker.MakeJob(JobDefOf.Wait_Combat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
				}
				IntVec3 intVec2;
				if (!this.TryFindShootingPosition(pawn, out intVec2, null))
				{
					return null;
				}
				if (intVec2 == pawn.Position)
				{
					Pawn_PathFollower pather3 = pawn.pather;
					if (pather3 != null)
					{
						pather3.StopDead();
					}
					return JobMaker.MakeJob(JobDefOf.Wait_Combat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
				}
				Job job2 = JobMaker.MakeJob(JobDefOf.Goto, intVec2);
				job2.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
				job2.checkOverrideOnExpire = true;
				return job2;
			}
		}

		// Token: 0x06007CE3 RID: 31971 RVA: 0x00259B18 File Offset: 0x00257D18
		public static Job GetAbilityJob(Pawn pawn, Thing enemyTarget)
		{
			if (pawn.abilities == null)
			{
				return null;
			}
			List<Ability> list = pawn.abilities.AICastableAbilities(enemyTarget, true);
			if (list.NullOrEmpty<Ability>())
			{
				return null;
			}
			if (pawn.Position.WalkableBy(pawn.Map, pawn) && pawn.Map.pawnDestinationReservationManager.CanReserve(pawn.Position, pawn, pawn.Drafted))
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].verb.CanHitTarget(enemyTarget))
					{
						return list[i].GetJob(enemyTarget, enemyTarget);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					LocalTargetInfo localTargetInfo = list[j].AIGetAOETarget();
					if (localTargetInfo.IsValid)
					{
						return list[j].GetJob(localTargetInfo, localTargetInfo);
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k].verb.targetParams.canTargetSelf)
					{
						return list[k].GetJob(pawn, pawn);
					}
				}
			}
			return null;
		}

		// Token: 0x06007CE4 RID: 31972 RVA: 0x00259C48 File Offset: 0x00257E48
		protected virtual Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, enemyTarget);
			job.expiryInterval = this.ExpiryInterval_Melee.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
			return job;
		}

		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06007CE5 RID: 31973 RVA: 0x00259C87 File Offset: 0x00257E87
		protected virtual int TicksSinceEngageToLoseTarget
		{
			get
			{
				return 400;
			}
		}

		// Token: 0x06007CE6 RID: 31974 RVA: 0x00259C90 File Offset: 0x00257E90
		protected virtual void UpdateEnemyTarget(Pawn pawn)
		{
			Thing thing = pawn.mindState.enemyTarget;
			if (thing != null && this.ShouldLoseTarget(pawn))
			{
				thing = null;
			}
			if (thing == null)
			{
				thing = this.FindAttackTargetIfPossible(pawn);
				if (thing != null)
				{
					pawn.mindState.Notify_EngagedTarget();
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnAcquiredTarget(pawn, thing);
					}
				}
			}
			else
			{
				Thing thing2 = this.FindAttackTargetIfPossible(pawn);
				if (thing2 == null && !this.chaseTarget)
				{
					thing = null;
				}
				else if (thing2 != null && thing2 != thing)
				{
					pawn.mindState.Notify_EngagedTarget();
					thing = thing2;
				}
			}
			pawn.mindState.enemyTarget = thing;
			Pawn pawn2 = thing as Pawn;
			if (pawn2 != null && thing.Faction == Faction.OfPlayer && pawn.Position.InHorDistOf(thing.Position, 40f) && !pawn2.IsShambler && !pawn.IsPsychologicallyInvisible())
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x00259D6D File Offset: 0x00257F6D
		protected Thing FindAttackTargetIfPossible(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, !pawn.IsColonist, false) == null)
			{
				return null;
			}
			return this.FindAttackTarget(pawn);
		}

		// Token: 0x06007CE8 RID: 31976 RVA: 0x00259D8C File Offset: 0x00257F8C
		protected virtual bool ShouldLoseTarget(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			if (!enemyTarget.Destroyed && Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick <= this.TicksSinceEngageToLoseTarget && pawn.CanReach(enemyTarget, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (float)(pawn.Position - enemyTarget.Position).LengthHorizontalSquared <= this.targetKeepRadius * this.targetKeepRadius)
			{
				IAttackTarget attackTarget = enemyTarget as IAttackTarget;
				return attackTarget != null && attackTarget.ThreatDisabled(pawn);
			}
			return true;
		}

		// Token: 0x06007CE9 RID: 31977 RVA: 0x00259E1C File Offset: 0x0025801C
		protected virtual Thing FindAttackTarget(Pawn pawn)
		{
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			if (this.needLOSToAcquireNonPawnTargets)
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToNonPawns;
			}
			if (this.PrimaryVerbIsIncendiary(pawn))
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			if (this.ignoreNonCombatants)
			{
				targetScanFlags |= TargetScanFlags.IgnoreNonCombatants;
			}
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, targetScanFlags, (Thing x) => this.ExtraTargetValidator(pawn, x), 0f, this.targetAcquireRadius, this.GetFlagPosition(pawn), this.GetFlagRadius(pawn), false, true, false, this.OnlyUseRangedSearch);
		}

		// Token: 0x06007CEA RID: 31978 RVA: 0x00259EBC File Offset: 0x002580BC
		private bool PrimaryVerbIsIncendiary(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) != null)
			{
				List<Verb> allVerbs = pawn.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.isPrimary)
					{
						return allVerbs[i].IsIncendiary_Ranged();
					}
				}
			}
			return false;
		}

		// Token: 0x04005413 RID: 21523
		protected float targetAcquireRadius = 56f;

		// Token: 0x04005414 RID: 21524
		protected float targetKeepRadius = 65f;

		// Token: 0x04005415 RID: 21525
		private bool needLOSToAcquireNonPawnTargets;

		// Token: 0x04005416 RID: 21526
		protected bool chaseTarget;

		// Token: 0x04005417 RID: 21527
		protected bool allowTurrets;

		// Token: 0x04005418 RID: 21528
		protected bool ignoreNonCombatants;

		// Token: 0x04005419 RID: 21529
		protected bool humanlikesOnly;

		// Token: 0x0400541A RID: 21530
		public static readonly IntRange ExpiryInterval_ShooterSucceeded = new IntRange(450, 550);

		// Token: 0x0400541B RID: 21531
		private const int MinTargetDistanceToMove = 5;
	}
}
