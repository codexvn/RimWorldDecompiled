using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014A7 RID: 5287
	public class JobGiver_KeepLyingDown : ThinkNode_JobGiver
	{
		// Token: 0x06007EDE RID: 32478 RVA: 0x00262B50 File Offset: 0x00260D50
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.GetPosture().Laying() && pawn.CurJob != null)
			{
				return pawn.CurJob;
			}
			return JobMaker.MakeJob(JobDefOf.LayDown, (pawn.InBed() && this.useBed) ? new LocalTargetInfo(pawn.CurrentBed()) : pawn.Position);
		}

		// Token: 0x06007EDF RID: 32479 RVA: 0x00262BAB File Offset: 0x00260DAB
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_KeepLyingDown jobGiver_KeepLyingDown = (JobGiver_KeepLyingDown)base.DeepCopy(resolve);
			jobGiver_KeepLyingDown.useBed = this.useBed;
			return jobGiver_KeepLyingDown;
		}

		// Token: 0x04005505 RID: 21765
		public bool useBed;
	}
}
