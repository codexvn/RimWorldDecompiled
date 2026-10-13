using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D8C RID: 7564
	public class IncidentWorker_DeepDrillInfestation : IncidentWorker
	{
		// Token: 0x0600A94C RID: 43340 RVA: 0x003072C5 File Offset: 0x003054C5
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			if (Faction.OfInsects == null)
			{
				return false;
			}
			Map map = (Map)parms.target;
			IncidentWorker_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, IncidentWorker_DeepDrillInfestation.tmpDrills);
			return IncidentWorker_DeepDrillInfestation.tmpDrills.Any<Thing>();
		}

		// Token: 0x0600A94D RID: 43341 RVA: 0x00307304 File Offset: 0x00305504
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IncidentWorker_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, IncidentWorker_DeepDrillInfestation.tmpDrills);
			Thing deepDrill;
			if (!IncidentWorker_DeepDrillInfestation.tmpDrills.TryRandomElement(out deepDrill))
			{
				return false;
			}
			IntVec3 intVec = CellFinder.FindNoWipeSpawnLocNear(deepDrill.Position, map, ThingDefOf.TunnelHiveSpawner, Rot4.North, 2, (IntVec3 x) => x.Walkable(map) && x.GetFirstThing(map, deepDrill.def) == null && x.GetFirstThingWithComp(map) == null && x.GetFirstThing(map, ThingDefOf.Hive) == null && x.GetFirstThing(map, ThingDefOf.TunnelHiveSpawner) == null);
			if (intVec == deepDrill.Position)
			{
				return false;
			}
			TunnelHiveSpawner tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
			tunnelHiveSpawner.spawnHive = false;
			tunnelHiveSpawner.insectsPoints = Mathf.Clamp(parms.points * Rand.Range(0.3f, 0.6f), 200f, 1000f);
			tunnelHiveSpawner.spawnedByInfestationThingComp = true;
			GenSpawn.Spawn(tunnelHiveSpawner, intVec, map, WipeMode.FullRefund);
			deepDrill.TryGetComp<CompCreatesInfestations>().Notify_CreatedInfestation();
			base.SendStandardLetter(parms, new TargetInfo(tunnelHiveSpawner.Position, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x04007524 RID: 29988
		private static List<Thing> tmpDrills = new List<Thing>();

		// Token: 0x04007525 RID: 29989
		private const float MinPointsFactor = 0.3f;

		// Token: 0x04007526 RID: 29990
		private const float MaxPointsFactor = 0.6f;

		// Token: 0x04007527 RID: 29991
		private const float MinPoints = 200f;

		// Token: 0x04007528 RID: 29992
		private const float MaxPoints = 1000f;
	}
}
