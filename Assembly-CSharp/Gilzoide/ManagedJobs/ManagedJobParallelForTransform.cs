using System;
using System.Runtime.InteropServices;
using Unity.Jobs;
using UnityEngine.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x0200005A RID: 90
	public struct ManagedJobParallelForTransform : IJobParallelForTransform, IDisposable
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000B118 File Offset: 0x00009318
		public IJobParallelForTransform Job
		{
			get
			{
				if (!this._managedJobGcHandle.IsAllocated)
				{
					return null;
				}
				return (IJobParallelForTransform)this._managedJobGcHandle.Target;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000B139 File Offset: 0x00009339
		public bool HasJob
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000B144 File Offset: 0x00009344
		public ManagedJobParallelForTransform(IJobParallelForTransform managedJob)
		{
			this._managedJobGcHandle = ((managedJob != null) ? GCHandle.Alloc(managedJob) : default(GCHandle));
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000B16B File Offset: 0x0000936B
		public void Execute(int index, TransformAccess transform)
		{
			IJobParallelForTransform job = this.Job;
			if (job == null)
			{
				return;
			}
			job.Execute(index, transform);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000B17F File Offset: 0x0000937F
		public void Dispose()
		{
			if (this._managedJobGcHandle.IsAllocated)
			{
				this._managedJobGcHandle.Free();
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000B19C File Offset: 0x0000939C
		public JobHandle Schedule(TransformAccessArray transforms, JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.Schedule(transforms, dependsOn);
			new DisposeJob<ManagedJobParallelForTransform>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x040000E5 RID: 229
		private GCHandle _managedJobGcHandle;
	}
}
