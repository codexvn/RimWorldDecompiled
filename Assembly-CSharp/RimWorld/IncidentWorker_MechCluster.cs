using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB3 RID: 7603
	public class IncidentWorker_MechCluster : IncidentWorker
	{
		// Token: 0x0600A9DD RID: 43485 RVA: 0x00309A68 File Offset: 0x00307C68
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ModsConfig.RoyaltyActive && base.CanFireNowSub(parms) && Faction.OfMechanoids != null;
		}

		// Token: 0x0600A9DE RID: 43486 RVA: 0x00309A88 File Offset: 0x00307C88
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(parms.points, map, true, false);
			IntVec3 center = MechClusterUtility.FindClusterPosition(map, sketch, 100, 0.5f);
			if (!center.IsValid)
			{
				return false;
			}
			IEnumerable<Thing> targets = from t in MechClusterUtility.SpawnCluster(center, map, sketch, true, true, parms.questTag)
			where t.def != ThingDefOf.Wall && t.def != ThingDefOf.Barricade
			select t;
			base.SendStandardLetter(parms, new LookTargets(targets), Array.Empty<NamedArgument>());
			return true;
		}
	}
}
