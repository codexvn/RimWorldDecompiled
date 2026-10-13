using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001C1 RID: 449
	public static class JobMaker
	{
		// Token: 0x06000C5D RID: 3165 RVA: 0x0003A84B File Offset: 0x00038A4B
		public static Job MakeJob()
		{
			Job job = SimplePool<Job>.Get();
			job.loadID = Find.UniqueIDsManager.GetNextJobID();
			return job;
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0003A862 File Offset: 0x00038A62
		public static Job MakeJob(JobDef def)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			return job;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0003A870 File Offset: 0x00038A70
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			return job;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0003A885 File Offset: 0x00038A85
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			return job;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0003A8A1 File Offset: 0x00038AA1
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			job.targetC = targetC;
			return job;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0003A8C4 File Offset: 0x00038AC4
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0003A8E7 File Offset: 0x00038AE7
		public static Job MakeJob(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0003A903 File Offset: 0x00038B03
		public static void ReturnToPool(Job job)
		{
			if (job == null)
			{
				return;
			}
			if (SimplePool<Job>.FreeItemsCount >= 1000)
			{
				return;
			}
			job.Clear();
			SimplePool<Job>.Return(job);
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0003A922 File Offset: 0x00038B22
		public static Job WithCount(this Job job, int count)
		{
			job.count = count;
			return job;
		}

		// Token: 0x04000716 RID: 1814
		private const int MaxJobPoolSize = 1000;
	}
}
