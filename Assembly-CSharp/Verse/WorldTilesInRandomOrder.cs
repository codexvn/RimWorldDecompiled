using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001A4 RID: 420
	public class WorldTilesInRandomOrder
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x00038008 File Offset: 0x00036208
		public List<int> Tiles
		{
			get
			{
				if (this.randomizedTiles == null)
				{
					this.randomizedTiles = new List<int>();
					for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
					{
						this.randomizedTiles.Add(i);
					}
					Rand.PushState();
					Rand.Seed = Find.World.info.Seed;
					this.randomizedTiles.Shuffle<int>();
					Rand.PopState();
				}
				return this.randomizedTiles;
			}
		}

		// Token: 0x040006CC RID: 1740
		private List<int> randomizedTiles;
	}
}
