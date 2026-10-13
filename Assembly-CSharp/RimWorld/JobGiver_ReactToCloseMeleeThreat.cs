using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020014BE RID: 5310
	public class JobGiver_ReactToCloseMeleeThreat : ThinkNode_JobGiver
	{
		// Token: 0x06007F2E RID: 32558 RVA: 0x00264DB8 File Offset: 0x00262FB8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn meleeThreat = pawn.mindState.meleeThreat;
			if (meleeThreat == null)
			{
				return null;
			}
			if (this.maxDistance > 0f && pawn.Position.DistanceTo(meleeThreat.Position) > this.maxDistance)
			{
				return null;
			}
			if (meleeThreat.IsPsychologicallyInvisible())
			{
				return null;
			}
			CompActivity comp = meleeThreat.GetComp<CompActivity>();
			if (comp != null && comp.IsDormant)
			{
				return null;
			}
			if (this.IsHunting(pawn, meleeThreat))
			{
				return null;
			}
			if (this.IsDueling(pawn, meleeThreat))
			{
				return null;
			}
			if (this.IsHateChanting(pawn))
			{
				return null;
			}
			if (pawn.IsAwokenCorpse)
			{
				return null;
			}
			if (MentalStateUtility.IsHavingMentalBreak(pawn))
			{
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse && pawn.playerSettings.hostilityResponse != HostilityResponseMode.Attack)
			{
				return null;
			}
			if (!pawn.mindState.MeleeThreatStillThreat)
			{
				pawn.mindState.meleeThreat = null;
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, meleeThreat);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = 200;
			job.reactingToMeleeThreat = true;
			return job;
		}

		// Token: 0x06007F2F RID: 32559 RVA: 0x00264ED4 File Offset: 0x002630D4
		private bool IsDueling(Pawn pawn, Pawn other)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual_Duel lordJob_Ritual_Duel = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual_Duel;
			return lordJob_Ritual_Duel != null && lordJob_Ritual_Duel.Opponent(pawn) == other;
		}

		// Token: 0x06007F30 RID: 32560 RVA: 0x00264F08 File Offset: 0x00263108
		private bool IsHunting(Pawn pawn, Pawn prey)
		{
			if (pawn.CurJob == null)
			{
				return false;
			}
			JobDriver_Hunt jobDriver_Hunt = pawn.jobs.curDriver as JobDriver_Hunt;
			if (jobDriver_Hunt != null)
			{
				return jobDriver_Hunt.Victim == prey;
			}
			JobDriver_PredatorHunt jobDriver_PredatorHunt = pawn.jobs.curDriver as JobDriver_PredatorHunt;
			return jobDriver_PredatorHunt != null && jobDriver_PredatorHunt.Prey == prey;
		}

		// Token: 0x06007F31 RID: 32561 RVA: 0x00264F5C File Offset: 0x0026315C
		private bool IsHateChanting(Pawn pawn)
		{
			if (ModsConfig.AnomalyActive)
			{
				Lord lord = pawn.GetLord();
				return ((lord != null) ? lord.LordJob : null) is LordJob_HateChant;
			}
			return false;
		}

		// Token: 0x0400552E RID: 21806
		private float maxDistance = -1f;

		// Token: 0x0400552F RID: 21807
		private const int MaxMeleeChaseTicks = 200;
	}
}
