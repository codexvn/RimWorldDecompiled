using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F75 RID: 8053
	public class BiomeConditionMapComponent : MapComponent
	{
		// Token: 0x0600B1FA RID: 45562 RVA: 0x00094327 File Offset: 0x00092527
		public BiomeConditionMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x0600B1FB RID: 45563 RVA: 0x0032D998 File Offset: 0x0032BB98
		public override void MapGenerated()
		{
			if (this.map.Biome.biomeMapConditions.NullOrEmpty<GameConditionDef>())
			{
				return;
			}
			foreach (GameConditionDef def in this.map.Biome.biomeMapConditions)
			{
				this.map.GameConditionManager.RegisterCondition(GameConditionMaker.MakeConditionPermanent(def));
			}
		}
	}
}
