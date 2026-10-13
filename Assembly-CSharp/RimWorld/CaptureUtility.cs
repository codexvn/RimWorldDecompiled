using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001100 RID: 4352
	public static class CaptureUtility
	{
		// Token: 0x06006A08 RID: 27144 RVA: 0x002165A0 File Offset: 0x002147A0
		public static void OrderArrest(Pawn arrestor, Pawn victim)
		{
			Thing t;
			if (!CaptureUtility.TryGetBed(arrestor, victim, out t))
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Arrest, victim, t);
			job.count = 1;
			job.playerForced = true;
			arrestor.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x06006A09 RID: 27145 RVA: 0x00216608 File Offset: 0x00214808
		public static bool CanArrest(Pawn arrestor, Pawn victim, out string reason)
		{
			Thing thing;
			if (!CaptureUtility.TryGetBed(arrestor, victim, out thing))
			{
				reason = "NoPrisonerBedShort".Translate();
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x00216637 File Offset: 0x00214837
		public static bool TryGetBed(Pawn arrestor, Pawn victim, out Thing bed)
		{
			bed = RestUtility.FindBedFor(victim, arrestor, false, false, new GuestStatus?(GuestStatus.Prisoner));
			if (bed == null)
			{
				bed = RestUtility.FindBedFor(victim, arrestor, false, true, new GuestStatus?(GuestStatus.Prisoner));
			}
			return bed != null;
		}
	}
}
