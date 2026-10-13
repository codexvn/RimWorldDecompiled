using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C51 RID: 15441
	public class MoveableAreas : IExposable
	{
		// Token: 0x06014FA7 RID: 85927 RVA: 0x00647BCC File Offset: 0x00645DCC
		public void ExposeData()
		{
			Scribe_Collections.Look<MoveableStockpile>(ref this.stockpiles, "stockpiles", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<MoveableGrowZone>(ref this.growZones, "growZones", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<MoveableArea_Allowed>(ref this.allowedAreas, "allowedAreas", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<MoveableStorageGroup>(ref this.storageGroups, "storageGroups", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea_Allowed>(ref this.homeArea, "homeArea", Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea>(ref this.buildRoofArea, "buildRoofArea", Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea>(ref this.noRoofArea, "noRoofArea", Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea>(ref this.snowClearArea, "snowClearArea", Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea>(ref this.pollutionClearArea, "pollutionClearArea", Array.Empty<object>());
		}

		// Token: 0x0400E9D2 RID: 59858
		public List<MoveableStockpile> stockpiles = new List<MoveableStockpile>();

		// Token: 0x0400E9D3 RID: 59859
		public List<MoveableGrowZone> growZones = new List<MoveableGrowZone>();

		// Token: 0x0400E9D4 RID: 59860
		public List<MoveableArea_Allowed> allowedAreas = new List<MoveableArea_Allowed>();

		// Token: 0x0400E9D5 RID: 59861
		public List<MoveableStorageGroup> storageGroups = new List<MoveableStorageGroup>();

		// Token: 0x0400E9D6 RID: 59862
		public MoveableArea_Allowed homeArea;

		// Token: 0x0400E9D7 RID: 59863
		public MoveableArea buildRoofArea;

		// Token: 0x0400E9D8 RID: 59864
		public MoveableArea noRoofArea;

		// Token: 0x0400E9D9 RID: 59865
		public MoveableArea snowClearArea;

		// Token: 0x0400E9DA RID: 59866
		public MoveableArea pollutionClearArea;
	}
}
