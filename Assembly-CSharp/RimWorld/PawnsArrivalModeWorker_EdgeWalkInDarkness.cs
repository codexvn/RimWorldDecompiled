using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002396 RID: 9110
	public class PawnsArrivalModeWorker_EdgeWalkInDarkness : PawnsArrivalModeWorker
	{
		// Token: 0x0600C55B RID: 50523 RVA: 0x00396BF4 File Offset: 0x00394DF4
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 spawnCenter = parms.spawnCenter;
				Map map2 = map;
				int radius = 8;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => map.glowGrid.PsychGlowAt(x) == PsychGlow.Dark));
				}
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spawnCenter, map2, radius, extraValidator);
				if (!loc.IsValid)
				{
					loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
				}
				GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation, WipeMode.Vanish, false, false);
			}
		}

		// Token: 0x0600C55C RID: 50524 RVA: 0x00396C98 File Offset: 0x00394E98
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Ignore, false, (IntVec3 x) => map.glowGrid.PsychGlowAt(x) == PsychGlow.Dark) && !RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Ignore, false, null))
			{
				return false;
			}
			parms.spawnRotation = Rot4.FromAngleFlat((map.Center - parms.spawnCenter).AngleFlat);
			return true;
		}
	}
}
