using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FB4 RID: 4020
	public static class GenPath
	{
		// Token: 0x060062B8 RID: 25272 RVA: 0x001F1E14 File Offset: 0x001F0014
		public static TargetInfo ResolvePathMode(Pawn pawn, TargetInfo dest, ref PathEndMode peMode)
		{
			if (dest.HasThing && !dest.Thing.Spawned)
			{
				peMode = PathEndMode.Touch;
				return dest;
			}
			if (peMode == PathEndMode.InteractionCell)
			{
				if (!dest.HasThing)
				{
					string str = "Pathed to cell ";
					TargetInfo targetInfo = dest;
					Log.Error(str + targetInfo.ToString() + " with PathEndMode.InteractionCell.");
				}
				peMode = PathEndMode.OnCell;
				return new TargetInfo(dest.Thing.InteractionCell, dest.Thing.Map, false);
			}
			if (peMode == PathEndMode.ClosestTouch)
			{
				peMode = GenPath.ResolveClosestTouchPathMode(pawn, dest.Map, dest.Cell);
			}
			return dest;
		}

		// Token: 0x060062B9 RID: 25273 RVA: 0x001F1EAC File Offset: 0x001F00AC
		public static PathEndMode ResolveClosestTouchPathMode(Pawn pawn, Map map, IntVec3 target)
		{
			if (GenPath.ShouldNotEnterCell(pawn, map, target))
			{
				return PathEndMode.Touch;
			}
			return PathEndMode.OnCell;
		}

		// Token: 0x060062BA RID: 25274 RVA: 0x001F1EBC File Offset: 0x001F00BC
		private static bool ShouldNotEnterCell(Pawn pawn, Map map, IntVec3 dest)
		{
			if (map.pathing.For(pawn).pathGrid.Cost(dest) > 30)
			{
				return true;
			}
			if (!dest.Walkable(map))
			{
				return true;
			}
			if (pawn != null)
			{
				if (dest.IsForbidden(pawn))
				{
					return true;
				}
				Building_Door building_Door = dest.GetEdifice(map) as Building_Door;
				if (building_Door != null)
				{
					if (building_Door.IsForbidden(pawn))
					{
						return true;
					}
					if (!building_Door.PawnCanOpen(pawn))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060062BB RID: 25275 RVA: 0x001F1F26 File Offset: 0x001F0126
		public static string SpeedPercentString(float extraPathTicks)
		{
			return (13f / (extraPathTicks + 13f)).ToStringPercent();
		}
	}
}
