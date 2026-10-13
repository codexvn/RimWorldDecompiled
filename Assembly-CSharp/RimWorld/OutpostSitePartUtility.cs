using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200284A RID: 10314
	public static class OutpostSitePartUtility
	{
		// Token: 0x0600E194 RID: 57748 RVA: 0x004220E7 File Offset: 0x004202E7
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
