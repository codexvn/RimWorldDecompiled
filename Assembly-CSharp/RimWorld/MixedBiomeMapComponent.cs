using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F79 RID: 8057
	public class MixedBiomeMapComponent : MapComponent
	{
		// Token: 0x17001B14 RID: 6932
		// (get) Token: 0x0600B20C RID: 45580 RVA: 0x0032E1E3 File Offset: 0x0032C3E3
		public bool IsMixedBiome
		{
			get
			{
				return this.biomeGrid != null;
			}
		}

		// Token: 0x0600B20D RID: 45581 RVA: 0x00094327 File Offset: 0x00092527
		public MixedBiomeMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x0600B20E RID: 45582 RVA: 0x0032E1F0 File Offset: 0x0032C3F0
		public BiomeDef GetBiomeAt(IntVec3 cell)
		{
			if (this.biomeGrid == null || !ModsConfig.OdysseyActive)
			{
				return this.map.Biome;
			}
			if (!cell.InBounds(this.map))
			{
				return null;
			}
			int num = this.map.cellIndices.CellToIndex(cell);
			if (!this.biomeGrid[num])
			{
				return this.map.Biome;
			}
			return this.secondaryBiome;
		}

		// Token: 0x0600B20F RID: 45583 RVA: 0x0032E258 File Offset: 0x0032C458
		public override void ExposeData()
		{
			base.ExposeData();
			bool[] array = this.biomeGrid;
			List<bool> list = (array != null) ? array.ToList<bool>() : null;
			Scribe_Collections.Look<bool>(ref list, "biomeGrid", LookMode.Undefined, Array.Empty<object>());
			this.biomeGrid = ((list != null) ? list.ToArray() : null);
			Scribe_Defs.Look<BiomeDef>(ref this.secondaryBiome, "secondaryBiome");
		}

		// Token: 0x040079B7 RID: 31159
		public bool[] biomeGrid;

		// Token: 0x040079B8 RID: 31160
		public BiomeDef secondaryBiome;
	}
}
