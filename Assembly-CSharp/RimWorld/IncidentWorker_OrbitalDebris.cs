using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBA RID: 7610
	public class IncidentWorker_OrbitalDebris : IncidentWorker
	{
		// Token: 0x0600A9F7 RID: 43511 RVA: 0x0030A135 File Offset: 0x00308335
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ModsConfig.OdysseyActive && base.CanFireNowSub(parms);
		}

		// Token: 0x0600A9F8 RID: 43512 RVA: 0x0030A148 File Offset: 0x00308348
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			GenSpawn.Spawn(ThingDefOf.OrbitalDebrisSpawner, map.Center, map, WipeMode.Vanish);
			base.SendStandardLetter(parms, null, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
