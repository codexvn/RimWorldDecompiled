using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001523 RID: 5411
	public class JobGiver_Berserk : ThinkNode_JobGiver
	{
		// Token: 0x060080D0 RID: 32976 RVA: 0x0026CC98 File Offset: 0x0026AE98
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.5f)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Combat);
				job.expiryInterval = 90;
				job.canUseRangedWeapon = false;
				return job;
			}
			if (pawn.TryGetAttackVerb(null, false, false) == null)
			{
				return null;
			}
			Thing thing = this.FindAttackTarget(pawn);
			if (thing != null)
			{
				Job job2 = JobMaker.MakeJob(JobDefOf.AttackMelee, thing);
				job2.maxNumMeleeAttacks = 1;
				job2.expiryInterval = Rand.Range(420, 900);
				job2.canBashDoors = true;
				return job2;
			}
			return null;
		}

		// Token: 0x060080D1 RID: 32977 RVA: 0x0026CD18 File Offset: 0x0026AF18
		private Thing FindAttackTarget(Pawn pawn)
		{
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable, new Predicate<Thing>(this.IsGoodTarget), 0f, this.maxAttackDistance, default(IntVec3), float.MaxValue, true, true, false, false);
		}

		// Token: 0x060080D2 RID: 32978 RVA: 0x0026CD5C File Offset: 0x0026AF5C
		protected virtual bool IsGoodTarget(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && pawn.Spawned && !pawn.Downed && !pawn.IsPsychologicallyInvisible();
		}

		// Token: 0x060080D3 RID: 32979 RVA: 0x0026CD8E File Offset: 0x0026AF8E
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Berserk jobGiver_Berserk = (JobGiver_Berserk)base.DeepCopy(resolve);
			jobGiver_Berserk.maxAttackDistance = this.maxAttackDistance;
			return jobGiver_Berserk;
		}

		// Token: 0x0400560C RID: 22028
		private const float WaitChance = 0.5f;

		// Token: 0x0400560D RID: 22029
		private const int WaitTicks = 90;

		// Token: 0x0400560E RID: 22030
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x0400560F RID: 22031
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x04005610 RID: 22032
		private float maxAttackDistance = 40f;
	}
}
