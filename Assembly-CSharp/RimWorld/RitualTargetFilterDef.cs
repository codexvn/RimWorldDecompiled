using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027D2 RID: 10194
	public class RitualTargetFilterDef : Def
	{
		// Token: 0x0600DF22 RID: 57122 RVA: 0x0041941B File Offset: 0x0041761B
		public RitualTargetFilter GetInstance()
		{
			return (RitualTargetFilter)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x0400963F RID: 38463
		public Type workerClass;

		// Token: 0x04009640 RID: 38464
		public bool fallBackToGatherSpot;

		// Token: 0x04009641 RID: 38465
		public bool fallbackToRitualSpot;
	}
}
