using System;
using System.Runtime.InteropServices;
using Unity.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000059 RID: 89
	public struct ManagedJobParallelFor : IJobParallelFor, IDisposable
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000B034 File Offset: 0x00009234
		public IJobParallelFor Job
		{
			get
			{
				if (!this._managedJobGcHandle.IsAllocated)
				{
					return null;
				}
				return (IJobParallelFor)this._managedJobGcHandle.Target;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000B055 File Offset: 0x00009255
		public bool HasJob
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000B060 File Offset: 0x00009260
		public ManagedJobParallelFor(IJobParallelFor managedJob)
		{
			this._managedJobGcHandle = ((managedJob != null) ? GCHandle.Alloc(managedJob) : default(GCHandle));
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000B087 File Offset: 0x00009287
		public void Execute(int index)
		{
			IJobParallelFor job = this.Job;
			if (job == null)
			{
				return;
			}
			job.Execute(index);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000B09A File Offset: 0x0000929A
		public void Dispose()
		{
			if (this._managedJobGcHandle.IsAllocated)
			{
				this._managedJobGcHandle.Free();
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000B0B4 File Offset: 0x000092B4
		public JobHandle Schedule(int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.Schedule(arrayLength, innerloopBatchCount, dependsOn);
			new DisposeJob<ManagedJobParallelFor>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000B0E4 File Offset: 0x000092E4
		public void Run(int arrayLength)
		{
			try
			{
				this.Run(arrayLength);
			}
			finally
			{
				this.Dispose();
			}
		}

		// Token: 0x040000E4 RID: 228
		private GCHandle _managedJobGcHandle;
	}
}
