using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001531 RID: 5425
	public class JobGiver_MurderousRage : ThinkNode_JobGiver
	{
		// Token: 0x060080F9 RID: 33017 RVA: 0x0026D6A0 File Offset: 0x0026B8A0
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_MurderousRage mentalState_MurderousRage = pawn.MentalState as MentalState_MurderousRage;
			if (mentalState_MurderousRage == null || !mentalState_MurderousRage.IsTargetStillValidAndReachable())
			{
				return null;
			}
			Thing spawnedParentOrMe = mentalState_MurderousRage.target.SpawnedParentOrMe;
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, spawnedParentOrMe);
			job.canBashDoors = true;
			job.killIncappedTarget = true;
			if (spawnedParentOrMe != mentalState_MurderousRage.target)
			{
				job.maxNumMeleeAttacks = 2;
			}
			return job;
		}
	}
}
