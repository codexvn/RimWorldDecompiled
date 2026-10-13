using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020004E8 RID: 1256
	public static class ReachabilityUtility
	{
		// Token: 0x06002079 RID: 8313 RVA: 0x000A9E28 File Offset: 0x000A8028
		public static bool CanReach(this Pawn pawn, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, bool canBashFences = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences, true));
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000A9E68 File Offset: 0x000A8068
		public static bool CanReach(this Pawn pawn, IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, bool canBashFences = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(start, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences, true));
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000A9EA4 File Offset: 0x000A80A4
		public static bool CanReachNonLocal(this Pawn pawn, TargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReachNonLocal(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, false, true));
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000A9EE4 File Offset: 0x000A80E4
		public static bool CanReachMapEdge(this Pawn p)
		{
			return p.Spawned && p.Map.reachability.CanReachMapEdge(p.Position, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true));
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000A9F20 File Offset: 0x000A8120
		public static void ClearCache()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCache();
			}
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000A9F58 File Offset: 0x000A8158
		public static void ClearCacheFor(Pawn p)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCacheFor(p);
			}
		}
	}
}
