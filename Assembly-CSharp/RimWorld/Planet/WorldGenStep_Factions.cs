using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CF0 RID: 15600
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x170034EA RID: 13546
		// (get) Token: 0x060153AC RID: 86956 RVA: 0x00659D80 File Offset: 0x00657F80
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x060153AD RID: 86957 RVA: 0x00659D87 File Offset: 0x00657F87
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			FactionGenerator.GenerateFactionsIntoWorldLayer(layer, Current.CreatingWorld.info.factions);
		}

		// Token: 0x060153AE RID: 86958 RVA: 0x000026EA File Offset: 0x000008EA
		public override void GenerateWithoutWorldData(string seed, PlanetLayer layer)
		{
		}
	}
}
