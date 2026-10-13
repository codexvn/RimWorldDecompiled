using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB3 RID: 8115
	public class GenStep_Fog : GenStep
	{
		// Token: 0x17001B42 RID: 6978
		// (get) Token: 0x0600B30D RID: 45837 RVA: 0x00334EF8 File Offset: 0x003330F8
		public override int SeedPart
		{
			get
			{
				return 1568957891;
			}
		}

		// Token: 0x0600B30E RID: 45838 RVA: 0x00334F00 File Offset: 0x00333100
		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("GenerateInitialFogGrid");
			map.fogGrid.SetAllFogged();
			if (MapGenerator.PlayerStartSpot.IsValid)
			{
				IntVec3 intVec = MapGenerator.PlayerStartSpot;
				Building edifice = intVec.GetEdifice(map);
				if (edifice != null && edifice.def.MakeFog)
				{
					intVec = CellFinder.StandableCellNear(intVec, map, 5f, (IntVec3 c) => !c.Roofed(map));
				}
				FloodFillerFog.FloodUnfog(intVec, map);
			}
			else
			{
				GenStep_Fog.UnfogMapFromEdge(map);
			}
			List<IntVec3> rootsToUnfog = MapGenerator.rootsToUnfog;
			for (int i = 0; i < rootsToUnfog.Count; i++)
			{
				FloodFillerFog.FloodUnfog(rootsToUnfog[i], map);
				map.fogGrid.Unfog(rootsToUnfog[i]);
			}
			DeepProfiler.End();
		}

		// Token: 0x0600B30F RID: 45839 RVA: 0x00334FEC File Offset: 0x003331EC
		private static void UnfogMapFromEdge(Map map)
		{
			Predicate<IntVec3> validator = (IntVec3 c) => c.Standable(map) && !c.Roofed(map) && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.NoPassClosedDoorsOrWater, Danger.Deadly, false, false, false, true, false));
			IntVec3 root;
			if (!CellFinder.TryFindRandomCellNear(map.Center, map, 30, validator, out root, -1) && !CellFinder.TryFindRandomEdgeCellWith(validator, map, 0f, out root) && !CellFinder.TryFindRandomCell(map, validator, out root))
			{
				return;
			}
			FloodFillerFog.FloodUnfog(root, map);
		}
	}
}
