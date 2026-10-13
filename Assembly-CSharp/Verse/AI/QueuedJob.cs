using System;

namespace Verse.AI
{
	// Token: 0x02000F50 RID: 3920
	public class QueuedJob : IExposable
	{
		// Token: 0x060060FF RID: 24831 RVA: 0x000024DF File Offset: 0x000006DF
		public QueuedJob()
		{
		}

		// Token: 0x06006100 RID: 24832 RVA: 0x001EBA0D File Offset: 0x001E9C0D
		public QueuedJob(QueuedJob other) : this(other.job, other.tag)
		{
		}

		// Token: 0x06006101 RID: 24833 RVA: 0x001EBA21 File Offset: 0x001E9C21
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		// Token: 0x06006102 RID: 24834 RVA: 0x001EBA37 File Offset: 0x001E9C37
		public void Cleanup(Pawn pawn, bool canReturnToPool)
		{
			if (pawn != null)
			{
				pawn.ClearReservationsForJob(this.job);
			}
			if (canReturnToPool)
			{
				JobMaker.ReturnToPool(this.job);
			}
			this.job = null;
			this.tag = null;
		}

		// Token: 0x06006103 RID: 24835 RVA: 0x001EBA6C File Offset: 0x001E9C6C
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", Array.Empty<object>());
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}

		// Token: 0x040045EE RID: 17902
		public Job job;

		// Token: 0x040045EF RID: 17903
		public JobTag? tag;
	}
}
