using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CFE RID: 3326
	public static class GenGrid
	{
		// Token: 0x0600551B RID: 21787 RVA: 0x001B72C4 File Offset: 0x001B54C4
		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return !map.IsPocketMap && !map.Tile.LayerDef.ignoreNoBuildArea && c.CloseToEdge(map, 10);
		}

		// Token: 0x0600551C RID: 21788 RVA: 0x001B72FB File Offset: 0x001B54FB
		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		// Token: 0x0600551D RID: 21789 RVA: 0x001B7308 File Offset: 0x001B5508
		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		// Token: 0x0600551E RID: 21790 RVA: 0x001B7354 File Offset: 0x001B5554
		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x001B739C File Offset: 0x001B559C
		public static bool OnEdge(this IntVec3 c, Map map, Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				return c.z == 0;
			}
			if (dir == Rot4.South)
			{
				return c.z == map.Size.z - 1;
			}
			if (dir == Rot4.West)
			{
				return c.x == 0;
			}
			if (dir == Rot4.East)
			{
				return c.x == map.Size.x - 1;
			}
			Log.ErrorOnce("Invalid edge direction", 55370769);
			return false;
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x001B7430 File Offset: 0x001B5630
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.info.Size;
			return (ulong)c.x < (ulong)((long)size.x) && (ulong)c.z < (ulong)((long)size.z);
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x001B746C File Offset: 0x001B566C
		public static bool InBounds(this IntVec3 c, Map map, int contractedBy)
		{
			IntVec3 size = map.Size;
			return c.x >= contractedBy && c.z >= contractedBy && c.x < size.x - contractedBy && c.z < size.z - contractedBy;
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x001B74B4 File Offset: 0x001B56B4
		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0f && v.z >= 0f && v.x < (float)size.x && v.z < (float)size.z;
		}

		// Token: 0x06005523 RID: 21795 RVA: 0x001B7504 File Offset: 0x001B5704
		public static CellRect BoundsRect(this Map map, int contractedBy = 0)
		{
			IntVec3 size = map.Size;
			return new CellRect(contractedBy, contractedBy, size.x - contractedBy * 2, size.z - contractedBy * 2);
		}

		// Token: 0x06005524 RID: 21796 RVA: 0x001B7533 File Offset: 0x001B5733
		public static bool WalkableByNormal(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c);
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x001B754B File Offset: 0x001B574B
		public static bool WalkableByFenceBlocked(this IntVec3 c, Map map)
		{
			return map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x001B7563 File Offset: 0x001B5763
		public static bool WalkableBy(this IntVec3 c, Map map, Pawn pawn)
		{
			return map.pathing.For(pawn).pathGrid.Walkable(c);
		}

		// Token: 0x06005527 RID: 21799 RVA: 0x001B757C File Offset: 0x001B577C
		public static bool WalkableByAny(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c) || map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x001B75AE File Offset: 0x001B57AE
		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c) && map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x001B75E0 File Offset: 0x001B57E0
		public static bool Standable(this IntVec3 c, Map map)
		{
			if (!c.Walkable(map))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600552A RID: 21802 RVA: 0x001B762C File Offset: 0x001B582C
		public static bool StandableBy(this IntVec3 c, Map map, Pawn pawn)
		{
			if (!c.WalkableBy(map, pawn))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x001B767C File Offset: 0x001B587C
		public static bool StandableByAny(this IntVec3 c, Map map)
		{
			if (!c.WalkableByAny(map))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x001B76C8 File Offset: 0x001B58C8
		public static bool Impassable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600552D RID: 21805 RVA: 0x001B770A File Offset: 0x001B590A
		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordanceDef surfaceType)
		{
			return c.GetAffordances(map).Contains(surfaceType);
		}

		// Token: 0x0600552E RID: 21806 RVA: 0x001B771C File Offset: 0x001B591C
		public static bool CanBeSeenOver(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x0600552F RID: 21807 RVA: 0x001B774C File Offset: 0x001B594C
		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06005530 RID: 21808 RVA: 0x001B7770 File Offset: 0x001B5970
		public static bool CanBeSeenOver(this Building b)
		{
			if (b.def.Fillage != FillCategory.Full)
			{
				return true;
			}
			Building_Door building_Door = b as Building_Door;
			return building_Door != null && building_Door.Open;
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x001B77A4 File Offset: 0x001B59A4
		public static SurfaceType GetSurfaceType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return SurfaceType.None;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.surfaceType != SurfaceType.None)
				{
					return thingList[i].def.surfaceType;
				}
			}
			return SurfaceType.None;
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x001B77FB File Offset: 0x001B59FB
		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x001B7808 File Offset: 0x001B5A08
		public static void Fit(this Camera camera, CellRect bounds, float yPos)
		{
			camera.transform.position = bounds.CenterVector3 + Vector3.up * yPos;
			camera.aspect = (float)bounds.Width / (float)bounds.Height;
			camera.orthographicSize = (float)bounds.Height / 2f;
		}

		// Token: 0x04003D79 RID: 15737
		public const int NoBuildEdgeWidth = 10;

		// Token: 0x04003D7A RID: 15738
		public const int NoZoneEdgeWidth = 5;
	}
}
