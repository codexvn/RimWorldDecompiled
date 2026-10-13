using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200256D RID: 9581
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x0600D1A2 RID: 53666 RVA: 0x003DA278 File Offset: 0x003D8478
		public bool TryGetCachedValue(GameInitData initData, out int ticksAbs)
		{
			if (initData.startingTile == this.cachedForStartingTile && initData.startingSeason == this.cachedForStartingSeason)
			{
				ticksAbs = this.cachedTicks;
				return true;
			}
			ticksAbs = -1;
			return false;
		}

		// Token: 0x0600D1A3 RID: 53667 RVA: 0x003DA2A9 File Offset: 0x003D84A9
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}

		// Token: 0x04008E41 RID: 36417
		private int cachedTicks = -1;

		// Token: 0x04008E42 RID: 36418
		private PlanetTile cachedForStartingTile = PlanetTile.Invalid;

		// Token: 0x04008E43 RID: 36419
		private Season cachedForStartingSeason;
	}
}
