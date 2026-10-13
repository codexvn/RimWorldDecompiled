using System;
using System.Collections.Generic;

namespace RimWorld.Planet
{
	// Token: 0x02003CE7 RID: 15591
	public class WorldGenData : WorldComponent
	{
		// Token: 0x0601538D RID: 86925 RVA: 0x00659223 File Offset: 0x00657423
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x0400EC99 RID: 60569
		public Dictionary<PlanetLayer, List<PlanetTile>> roadNodes = new Dictionary<PlanetLayer, List<PlanetTile>>();

		// Token: 0x0400EC9A RID: 60570
		public Dictionary<PlanetLayer, List<PlanetTile>> ancientSites = new Dictionary<PlanetLayer, List<PlanetTile>>();
	}
}
