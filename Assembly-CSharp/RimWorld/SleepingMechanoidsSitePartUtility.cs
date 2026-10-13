using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200284D RID: 10317
	public static class SleepingMechanoidsSitePartUtility
	{
		// Token: 0x0600E1A0 RID: 57760 RVA: 0x004220E7 File Offset: 0x004202E7
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
