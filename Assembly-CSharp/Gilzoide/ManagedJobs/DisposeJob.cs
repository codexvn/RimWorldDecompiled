using System;
using Unity.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000056 RID: 86
	public struct DisposeJob<TDisposable> : IJob where TDisposable : struct, IDisposable
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000AE06 File Offset: 0x00009006
		public readonly TDisposable Disposable { get; }

		// Token: 0x060001B8 RID: 440 RVA: 0x0000AE0E File Offset: 0x0000900E
		public DisposeJob(TDisposable disposable)
		{
			this.Disposable = disposable;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000AE18 File Offset: 0x00009018
		public void Execute()
		{
			TDisposable disposable = this.Disposable;
			disposable.Dispose();
		}
	}
}
