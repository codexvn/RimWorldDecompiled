using System;

namespace Verse
{
	// Token: 0x020004F2 RID: 1266
	public static class RegionAndRoomQuery
	{
		// Token: 0x060020C7 RID: 8391 RVA: 0x000AB88C File Offset: 0x000A9A8C
		public static Region RegionAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!c.InBounds(map))
			{
				return null;
			}
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt;
			}
			return null;
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000AB8C1 File Offset: 0x000A9AC1
		public static Region GetRegion(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.RegionAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000AB8DF File Offset: 0x000A9ADF
		public static Region GetRegionHeld(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(thing.PositionHeld, thing.MapHeld, allowedRegionTypes);
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000AB8F4 File Offset: 0x000A9AF4
		public static District DistrictAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			if (region == null)
			{
				return null;
			}
			return region.District;
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000AB918 File Offset: 0x000A9B18
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_All)
		{
			District district = RegionAndRoomQuery.DistrictAt(c, map, allowedRegionTypes);
			if (district == null)
			{
				return null;
			}
			return district.Room;
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000AB93C File Offset: 0x000A9B3C
		public static District GetDistrict(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Thing spawnedParentOrMe = thing.SpawnedParentOrMe;
			if (spawnedParentOrMe != null)
			{
				return RegionAndRoomQuery.DistrictAt(spawnedParentOrMe.Position, spawnedParentOrMe.Map, allowedRegionTypes);
			}
			return null;
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000AB968 File Offset: 0x000A9B68
		public static Room GetRoom(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_All)
		{
			District district = thing.GetDistrict(allowedRegionTypes);
			if (district == null)
			{
				return null;
			}
			return district.Room;
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000AB988 File Offset: 0x000A9B88
		public static bool IsOutside(this Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			return room == null || room.PsychologicallyOutdoors;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000AB9AC File Offset: 0x000A9BAC
		public static District DistirctAtFast(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt.District;
			}
			return null;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x000AB9DC File Offset: 0x000A9BDC
		public static Room RoomAtOrAdjacent(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, allowedRegionTypes);
			if (room != null)
			{
				return room;
			}
			for (int i = 0; i < 8; i++)
			{
				room = RegionAndRoomQuery.RoomAt(c + GenAdj.AdjacentCells[i], map, allowedRegionTypes);
				if (room != null)
				{
					return room;
				}
			}
			return room;
		}
	}
}
