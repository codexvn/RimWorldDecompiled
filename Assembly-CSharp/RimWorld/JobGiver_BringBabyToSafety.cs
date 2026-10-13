using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200147C RID: 5244
	public class JobGiver_BringBabyToSafety : ThinkNode_JobGiver
	{
		// Token: 0x06007E59 RID: 32345 RVA: 0x002607C0 File Offset: 0x0025E9C0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = ChildcareUtility.FindUnsafeBaby(pawn, AutofeedMode.Urgent);
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.BringBabyToSafetyUnforced, pawn2);
			job.count = 1;
			return job;
		}
	}
}
