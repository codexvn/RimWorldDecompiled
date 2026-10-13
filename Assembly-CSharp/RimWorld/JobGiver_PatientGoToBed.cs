using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014B6 RID: 5302
	public class JobGiver_PatientGoToBed : ThinkNode_JobGiver
	{
		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x06007F15 RID: 32533 RVA: 0x002645CC File Offset: 0x002627CC
		public override string CrawlingReportStringOverride
		{
			get
			{
				return base.CrawlingReportStringOverride ?? "ReportStringCrawlingToBed".Translate();
			}
		}

		// Token: 0x06007F16 RID: 32534 RVA: 0x002645E8 File Offset: 0x002627E8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.urgentOnly && !HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
			{
				return null;
			}
			if (!HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return null;
			}
			if (this.respectTimetable && RestUtility.TimetablePreventsLayDown(pawn) && !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn) && !HealthAIUtility.ShouldBeTendedNowByPlayer(pawn))
			{
				return null;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				return null;
			}
			if (pawn.Downed && !pawn.health.CanCrawl)
			{
				return null;
			}
			if (pawn.GetPosture().InBed() && pawn.Downed)
			{
				return null;
			}
			Thing thing = RestUtility.FindBedFor(pawn, pawn, false, false, null);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.LayDown, thing);
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x06007F17 RID: 32535 RVA: 0x0026469A File Offset: 0x0026289A
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PatientGoToBed jobGiver_PatientGoToBed = (JobGiver_PatientGoToBed)base.DeepCopy(resolve);
			jobGiver_PatientGoToBed.respectTimetable = this.respectTimetable;
			return jobGiver_PatientGoToBed;
		}

		// Token: 0x04005526 RID: 21798
		public bool respectTimetable = true;

		// Token: 0x04005527 RID: 21799
		public bool urgentOnly;
	}
}
