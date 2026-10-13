using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014A9 RID: 5289
	public class JobGiver_MeditateInBed : JobGiver_Meditate
	{
		// Token: 0x06007EE5 RID: 32485 RVA: 0x00262CC7 File Offset: 0x00260EC7
		protected override bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() != null && pawn.Awake() && MeditationUtility.ShouldMeditateInBed(pawn);
		}

		// Token: 0x06007EE6 RID: 32486 RVA: 0x00262CE4 File Offset: 0x00260EE4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!MeditationUtility.CanMeditateNow(pawn))
			{
				return null;
			}
			LocalTargetInfo targetC = ModsConfig.RoyaltyActive ? MeditationUtility.BestFocusAt(pawn.Position, pawn) : LocalTargetInfo.Invalid;
			Job job = JobMaker.MakeJob(JobDefOf.Meditate, pawn.Position, pawn.InBed() ? pawn.CurrentBed() : new LocalTargetInfo(pawn.Position), targetC);
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
