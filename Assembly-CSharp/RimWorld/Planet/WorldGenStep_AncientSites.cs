using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CEF RID: 15599
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		// Token: 0x170034E9 RID: 13545
		// (get) Token: 0x060153A8 RID: 86952 RVA: 0x00659D00 File Offset: 0x00657F00
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		// Token: 0x060153A9 RID: 86953 RVA: 0x00659D07 File Offset: 0x00657F07
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.GenerateAncientSites(layer);
		}

		// Token: 0x060153AA RID: 86954 RVA: 0x00659D10 File Offset: 0x00657F10
		private void GenerateAncientSites(PlanetLayer layer)
		{
			int num = GenMath.RoundRandom((float)layer.TilesCount / 100000f * this.ancientSitesPer100kTiles.RandomInRange);
			Dictionary<PlanetLayer, List<PlanetTile>> ancientSites = Find.World.genData.ancientSites;
			List<PlanetTile> list;
			if (!ancientSites.TryGetValue(layer, out list))
			{
				list = (ancientSites[layer] = new List<PlanetTile>());
			}
			for (int i = 0; i < num; i++)
			{
				list.Add(TileFinder.RandomSettlementTileFor(layer, null, false, null));
			}
		}

		// Token: 0x0400ECB2 RID: 60594
		public FloatRange ancientSitesPer100kTiles;
	}
}
