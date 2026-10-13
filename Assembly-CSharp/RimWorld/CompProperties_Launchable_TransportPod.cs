using System;

namespace RimWorld
{
	// Token: 0x02002D16 RID: 11542
	public class CompProperties_Launchable_TransportPod : CompProperties_Launchable
	{
		// Token: 0x0601038B RID: 66443 RVA: 0x004B1277 File Offset: 0x004AF477
		public CompProperties_Launchable_TransportPod()
		{
			this.compClass = typeof(CompLaunchable_TransportPod);
		}

		// Token: 0x0400A96F RID: 43375
		public bool requiresFuelingPort = true;
	}
}
