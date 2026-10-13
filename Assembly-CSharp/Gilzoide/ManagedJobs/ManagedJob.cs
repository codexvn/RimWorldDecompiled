using System;
using System.Runtime.InteropServices;
using Unity.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000057 RID: 87
	public struct ManagedJob : IJob, IDisposable
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000AE39 File Offset: 0x00009039
		public IJob Job
		{
			get
			{
				if (!this._managedJobGcHandle.IsAllocated)
				{
					return null;
				}
				return (IJob)this._managedJobGcHandle.Target;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000AE5A File Offset: 0x0000905A
		public bool HasJob
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000AE68 File Offset: 0x00009068
		public ManagedJob(IJob managedJob)
		{
			this._managedJobGcHandle = ((managedJob != null) ? GCHandle.Alloc(managedJob) : default(GCHandle));
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000AE8F File Offset: 0x0000908F
		public void Execute()
		{
			IJob job = this.Job;
			if (job == null)
			{
				return;
			}
			job.Execute();
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000AEA1 File Offset: 0x000090A1
		public void Dispose()
		{
			if (this._managedJobGcHandle.IsAllocated)
			{
				this._managedJobGcHandle.Free();
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000AEBC File Offset: 0x000090BC
		public JobHandle Schedule(JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.Schedule(dependsOn);
			new DisposeJob<ManagedJob>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000AEEC File Offset: 0x000090EC
		public void Run()
		{
			try
			{
				this.Run<ManagedJob>();
			}
			finally
			{
				this.Dispose();
			}
		}

		// Token: 0x040000E2 RID: 226
		private GCHandle _managedJobGcHandle;
	}
}
