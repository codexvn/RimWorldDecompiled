using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A8C RID: 10892
	public static class BlightUtility
	{
		// Token: 0x0600F27D RID: 62077 RVA: 0x00466A50 File Offset: 0x00464C50
		public static Plant GetFirstBlightableNowPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.BlightableNow)
			{
				return plant;
			}
			return null;
		}

		// Token: 0x0600F27E RID: 62078 RVA: 0x00466A74 File Offset: 0x00464C74
		public static Plant GetFirstBlightableEverPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.Blightable)
			{
				return plant;
			}
			return null;
		}
	}
}
