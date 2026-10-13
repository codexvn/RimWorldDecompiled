using System;
using System.Runtime.InteropServices;
using Unity.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000058 RID: 88
	public struct ManagedJobFor : IJobFor, IDisposable
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000AF20 File Offset: 0x00009120
		public IJobFor Job
		{
			get
			{
				if (!this._managedJobGcHandle.IsAllocated)
				{
					return null;
				}
				return (IJobFor)this._managedJobGcHandle.Target;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000AF41 File Offset: 0x00009141
		public bool HasJob
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000AF4C File Offset: 0x0000914C
		public ManagedJobFor(IJobFor managedJob)
		{
			this._managedJobGcHandle = ((managedJob != null) ? GCHandle.Alloc(managedJob) : default(GCHandle));
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000AF73 File Offset: 0x00009173
		public void Execute(int index)
		{
			IJobFor job = this.Job;
			if (job == null)
			{
				return;
			}
			job.Execute(index);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000AF86 File Offset: 0x00009186
		public void Dispose()
		{
			if (this._managedJobGcHandle.IsAllocated)
			{
				this._managedJobGcHandle.Free();
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public JobHandle Schedule(int arrayLength, JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.Schedule(arrayLength, dependsOn);
			new DisposeJob<ManagedJobFor>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000AFD0 File Offset: 0x000091D0
		public JobHandle ScheduleParallel(int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.ScheduleParallel(arrayLength, innerloopBatchCount, dependsOn);
			new DisposeJob<ManagedJobFor>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000B000 File Offset: 0x00009200
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

		// Token: 0x040000E3 RID: 227
		private GCHandle _managedJobGcHandle;
	}
}
