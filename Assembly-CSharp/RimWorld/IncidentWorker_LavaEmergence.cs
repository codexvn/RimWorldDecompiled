using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB0 RID: 7600
	public class IncidentWorker_LavaEmergence : IncidentWorker
	{
		// Token: 0x0600A9D7 RID: 43479 RVA: 0x003099EC File Offset: 0x00307BEC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			GenSpawn.Spawn(ThingDefOf.LavaEmergence, map.Center, map, WipeMode.Vanish);
			return true;
		}
	}
}
