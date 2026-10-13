using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FC3 RID: 4035
	public static class TouchPathEndModeUtility
	{
		// Token: 0x0600634E RID: 25422 RVA: 0x001F5CFC File Offset: 0x001F3EFC
		[Obsolete("Use IsCornerTouchAllowed_NewTemp()")]
		public static bool IsCornerTouchAllowed(IntVec3 corner, IntVec3 adjCardinalX, IntVec3 adjCardinalZ, PathingContext pc, bool forEdifice)
		{
			return TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(adjCardinalX, adjCardinalZ, pc, forEdifice ? pc.map.edificeGrid[corner] : null);
		}

		// Token: 0x0600634F RID: 25423 RVA: 0x001F5D20 File Offset: 0x001F3F20
		public static bool IsCornerTouchAllowed_NewTemp(IntVec3 adjCardinalX, IntVec3 adjCardinalZ, PathingContext pc, Thing target = null)
		{
			return (target != null && (target.IsConsideredEdificeForCornerTouch() || TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(target.def))) || ((pc.pathGrid.Walkable(adjCardinalX) && adjCardinalX.GetDoor(pc.map) == null) || (pc.pathGrid.Walkable(adjCardinalZ) && adjCardinalZ.GetDoor(pc.map) == null));
		}

		// Token: 0x06006350 RID: 25424 RVA: 0x001F5D84 File Offset: 0x001F3F84
		public static bool MakesOccupiedCellsAlwaysReachableDiagonally(ThingDef def)
		{
			ThingDef thingDef = def.IsFrame ? (def.entityDefToBuild as ThingDef) : def;
			return thingDef != null && thingDef.CanInteractThroughCorners;
		}

		// Token: 0x06006351 RID: 25425 RVA: 0x001F5DB4 File Offset: 0x001F3FB4
		public static bool IsAdjacentCornerAndNotAllowed(IntVec3 corner, IntVec3 BL, IntVec3 TL, IntVec3 TR, IntVec3 BR, PathingContext pc, Thing forThing)
		{
			IntVec3 b = new IntVec3(1, 0, 0);
			IntVec3 b2 = new IntVec3(0, 0, 1);
			return (corner == BL && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(BL + b2, BL + b, pc, forThing)) || (corner == TL && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(TL - b2, TL + b, pc, forThing)) || (corner == TR && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(TR - b2, TR - b, pc, forThing)) || (corner == BR && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(BR + b2, BR - b, pc, forThing));
		}

		// Token: 0x06006352 RID: 25426 RVA: 0x001F5E6C File Offset: 0x001F406C
		public static void AddAllowedAdjacentRegions(LocalTargetInfo dest, TraverseParms traverseParams, Map map, List<Region> regions)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(dest, out bl, out tl, out tr, out br);
			PathingContext pc = map.pathing.For(traverseParams);
			if (!dest.HasThing || (dest.Thing.def.size.x == 1 && dest.Thing.def.size.z == 1))
			{
				IntVec3 cell = dest.Cell;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = GenAdj.AdjacentCells[i] + cell;
					if (intVec.InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(intVec, bl, tl, tr, br, pc, dest.Thing))
					{
						Region region = intVec.GetRegion(map, RegionType.Set_Passable);
						if (region != null && region.Allows(traverseParams, true))
						{
							regions.Add(region);
						}
					}
				}
				return;
			}
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(dest);
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(list[j], bl, tl, tr, br, pc, dest.Thing))
				{
					Region region2 = list[j].GetRegion(map, RegionType.Set_Passable);
					if (region2 != null && region2.Allows(traverseParams, true))
					{
						regions.Add(region2);
					}
				}
			}
		}

		// Token: 0x06006353 RID: 25427 RVA: 0x001F5FBC File Offset: 0x001F41BC
		public static bool IsAdjacentOrInsideAndAllowedToTouch(IntVec3 root, LocalTargetInfo target, PathingContext pc)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(target, out bl, out tl, out tr, out br);
			return root.AdjacentTo8WayOrInside(target) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(root, bl, tl, tr, br, pc, target.Thing);
		}

		// Token: 0x06006354 RID: 25428 RVA: 0x001F5FF8 File Offset: 0x001F41F8
		private static bool IsConsideredEdificeForCornerTouch(this Thing thing)
		{
			if (thing == null)
			{
				return false;
			}
			ThingDef thingDef = (thing.def.IsFrame || thing.def.IsBlueprint) ? (thing.def.entityDefToBuild as ThingDef) : thing.def;
			if (thingDef == null)
			{
				return false;
			}
			if (!thingDef.IsEdifice() || !thingDef.holdsRoof)
			{
				BuildingProperties building = thingDef.building;
				return building != null && building.canConstructInCorner;
			}
			return true;
		}
	}
}
