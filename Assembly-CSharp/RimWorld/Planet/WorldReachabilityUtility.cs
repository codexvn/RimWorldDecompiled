using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C1D RID: 15389
	public static class WorldReachabilityUtility
	{
		// Token: 0x06014DF9 RID: 85497 RVA: 0x006405D4 File Offset: 0x0063E7D4
		public static bool CanReach(this Caravan c, PlanetTile tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
