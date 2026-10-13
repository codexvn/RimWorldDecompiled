using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F51 RID: 3921
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06006104 RID: 24836 RVA: 0x001EBAA8 File Offset: 0x001E9CA8
		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		// Token: 0x17000F5C RID: 3932
		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06006106 RID: 24838 RVA: 0x001EBAC4 File Offset: 0x001E9CC4
		public bool AnyPlayerForced
		{
			get
			{
				for (int i = 0; i < this.jobs.Count; i++)
				{
					if (this.jobs[i].job.playerForced)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x001EBB04 File Offset: 0x001E9D04
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.jobs.RemoveAll(delegate(QueuedJob j)
				{
					Job job = j.job;
					return ((job != null) ? job.def : null) == null;
				});
			}
		}

		// Token: 0x06006108 RID: 24840 RVA: 0x001EBB5A File Offset: 0x001E9D5A
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x06006109 RID: 24841 RVA: 0x001EBB6F File Offset: 0x001E9D6F
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x0600610A RID: 24842 RVA: 0x001EBB84 File Offset: 0x001E9D84
		public void Clear(Pawn pawn, bool canReturnToPool)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				this.jobs[i] = null;
				queuedJob.Cleanup(pawn, canReturnToPool);
			}
			this.jobs.Clear();
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x001EBBD4 File Offset: 0x001E9DD4
		public bool Contains(Job j)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				if (((queuedJob != null) ? queuedJob.job : null) == j)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600610C RID: 24844 RVA: 0x001EBC18 File Offset: 0x001E9E18
		public QueuedJob Extract(Job j)
		{
			int num = this.jobs.FindIndex((QueuedJob qj) => qj.job == j);
			if (num >= 0)
			{
				QueuedJob result = this.jobs[num];
				this.jobs.RemoveAt(num);
				return result;
			}
			return null;
		}

		// Token: 0x0600610D RID: 24845 RVA: 0x001EBC68 File Offset: 0x001E9E68
		public QueuedJob Dequeue()
		{
			if (this.jobs.NullOrEmpty<QueuedJob>())
			{
				return null;
			}
			QueuedJob result = this.jobs[0];
			this.jobs.RemoveAt(0);
			return result;
		}

		// Token: 0x0600610E RID: 24846 RVA: 0x001EBC91 File Offset: 0x001E9E91
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x0600610F RID: 24847 RVA: 0x001EBCA0 File Offset: 0x001E9EA0
		public bool AnyCanBeginNow(Pawn pawn, bool whileLyingDown)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				if (this.jobs[i].job.CanBeginNow(pawn, whileLyingDown))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006110 RID: 24848 RVA: 0x001EBCE0 File Offset: 0x001E9EE0
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06006111 RID: 24849 RVA: 0x001EBCE0 File Offset: 0x001E9EE0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06006112 RID: 24850 RVA: 0x001EBCF4 File Offset: 0x001E9EF4
		public JobQueue Capture()
		{
			JobQueue jobQueue = new JobQueue();
			foreach (QueuedJob other in this.jobs)
			{
				jobQueue.jobs.Add(new QueuedJob(other));
			}
			return jobQueue;
		}

		// Token: 0x06006113 RID: 24851 RVA: 0x001EBD58 File Offset: 0x001E9F58
		public void RemoveAll(Pawn pawn, Predicate<Job> filter)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				if (filter(queuedJob.job))
				{
					this.jobs[i] = null;
					queuedJob.Cleanup(pawn, true);
				}
			}
			this.jobs.RemoveAll((QueuedJob job) => job == null);
		}

		// Token: 0x06006114 RID: 24852 RVA: 0x001EBDD8 File Offset: 0x001E9FD8
		public void RemoveAllWorkType(Pawn pawn, WorkTypeDef wType, bool hardDisable)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				if (queuedJob.job.workGiverDef != null && queuedJob.job.workGiverDef.workType == wType && (hardDisable || !queuedJob.job.playerForced))
				{
					this.jobs[i] = null;
					queuedJob.Cleanup(pawn, true);
				}
			}
			this.jobs.RemoveAll((QueuedJob job) => job == null);
		}

		// Token: 0x06006115 RID: 24853 RVA: 0x001EBE78 File Offset: 0x001EA078
		public void RemoveAllJoyKind(Pawn pawn, JoyKindDef joyKind)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				if (queuedJob.job.def.joyKind == joyKind)
				{
					this.jobs[i] = null;
					queuedJob.Cleanup(pawn, true);
				}
			}
			this.jobs.RemoveAll((QueuedJob job) => job == null);
		}

		// Token: 0x040045F0 RID: 17904
		private List<QueuedJob> jobs = new List<QueuedJob>();
	}
}
