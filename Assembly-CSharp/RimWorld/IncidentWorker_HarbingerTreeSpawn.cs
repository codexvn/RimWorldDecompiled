using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDD RID: 7645
	public class IncidentWorker_HarbingerTreeSpawn : IncidentWorker_SpecialTreeSpawn
	{
		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x0600AA71 RID: 43633 RVA: 0x000E96BB File Offset: 0x000E78BB
		protected override int MaxTreesPerIncident
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600AA72 RID: 43634 RVA: 0x0030BE38 File Offset: 0x0030A038
		protected override bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return IncidentWorker_HarbingerTreeSpawn.TryGetHarbingerTreeSpawnCell(map, out cell, null);
		}

		// Token: 0x0600AA73 RID: 43635 RVA: 0x0030BE55 File Offset: 0x0030A055
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			bool flag = base.TryExecuteWorker(parms);
			if (flag)
			{
				Find.Anomaly.Notify_HarbingerTreeSpawned();
			}
			return flag;
		}

		// Token: 0x0600AA74 RID: 43636 RVA: 0x0030BE6C File Offset: 0x0030A06C
		public static bool TryGetHarbingerTreeSpawnCell(Map map, out IntVec3 cell, IntVec3? nearLoc = null)
		{
			GenStep_SpecialTrees genStep = (GenStep_SpecialTrees)GenStepDefOf.HarbingerTrees.genStep;
			Map anyPlayerHomeMap = Current.Game.AnyPlayerHomeMap;
			int num = (anyPlayerHomeMap != null) ? anyPlayerHomeMap.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeHarbinger).Count : 0;
			int maxProximityToSameTree = (num > 0) ? 7 : -1;
			if (nearLoc != null)
			{
				if (RCellFinder.TryFindRandomCellNearWith(nearLoc.Value, (IntVec3 x) => genStep.CanSpawnAt(x, map, 20, 0, 0.5f, 18, 20, 3, maxProximityToSameTree, 15), map, out cell, 5, maxProximityToSameTree))
				{
					return true;
				}
				if (RCellFinder.TryFindRandomCellNearWith(nearLoc.Value, (IntVec3 x) => genStep.CanSpawnAt(x, map, 2, 0, 0.5f, 22, 10, 3, maxProximityToSameTree, 15), map, out cell, 5, maxProximityToSameTree))
				{
					return true;
				}
			}
			return CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => genStep.CanSpawnAt(x, map, 20, 0, 0.5f, 18, 20, 3, maxProximityToSameTree, 15), map, 1000, out cell) || CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => genStep.CanSpawnAt(x, map, 2, 0, 0.5f, 22, 10, 3, maxProximityToSameTree, 15), map, 10000, out cell) || CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => genStep.CanSpawnAt(x, map, 20, 0, 0.5f, 18, 20, 3, -1, 15), map, 1000, out cell) || CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => genStep.CanSpawnAt(x, map, 2, 0, 0.5f, 22, 10, 3, -1, 15), map, 1000, out cell);
		}
	}
}
