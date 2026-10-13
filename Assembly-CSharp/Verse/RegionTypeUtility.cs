using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200050B RID: 1291
	public static class RegionTypeUtility
	{
		// Token: 0x06002166 RID: 8550 RVA: 0x000AE5E1 File Offset: 0x000AC7E1
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000AE5E7 File Offset: 0x000AC7E7
		public static bool AllowsMultipleRegionsPerDistrict(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000AE5F0 File Offset: 0x000AC7F0
		public static RegionType GetExpectedRegionType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return RegionType.None;
			}
			if (c.GetDoor(map) != null)
			{
				return RegionType.Portal;
			}
			if (c.GetFence(map) != null)
			{
				return RegionType.Fence;
			}
			if (c.WalkableByNormal(map))
			{
				return RegionType.Normal;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.Fillage == FillCategory.Full)
				{
					return RegionType.None;
				}
			}
			return RegionType.ImpassableFreeAirExchange;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000AE659 File Offset: 0x000AC859
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) > RegionType.None;
		}
	}
}
