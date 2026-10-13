using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E9E RID: 7838
	public abstract class DeferredSpawnWorker : IExposable
	{
		// Token: 0x0600ADF5 RID: 44533
		public abstract void OnSpawn(Thing thing);

		// Token: 0x0600ADF6 RID: 44534 RVA: 0x000026EA File Offset: 0x000008EA
		public void ExposeData()
		{
		}
	}
}
